using FastColoredTextBoxNS;
using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using PgMulti.Tasks;
using PgMulti.AppData;

namespace PgMulti.QueryEditor
{
    public class EditorTab
    {
        public bool PendingSaveDB = true;
        public string? LocalPath;
        public PgTask? LastTask = null;

        private const string _AutoSelectSymbols = "()=/&%$!*;:><[]{}@ .,-+";

        private short _Position;
        private int _Id = -1;

        private Data _Data;
        private TabPage _TabPage;
        private CustomFctb _Fctb;
        private AutocompleteMenu _AutocompleteMenu;

        public EditorTab(Data d, TabPage tp, MainForm mainForm)
        {
            _Data = d;
            _TabPage = tp;
            _Fctb = (CustomFctb)tp.Controls[0];
            _AutocompleteMenu = new AutocompleteMenu(_Fctb);
            _AutocompleteMenu.ForeColor = Color.FromArgb(30, 30, 30);
            _AutocompleteMenu.BackColor = Color.FromArgb(247, 249, 254);
            _AutocompleteMenu.SelectedColor = Color.FromArgb(196, 213, 255);
            _AutocompleteMenu.SearchPattern = @"[\w\.]";
            _AutocompleteMenu.AllowTabKey = true;
            _AutocompleteMenu.Items.SetAutocompleteItems(new PGAutocompleteEnumerable(_AutocompleteMenu, _Fctb, mainForm, _Data.PGLanguageData, _Data.PGSimpleLanguageData));
            _AutocompleteMenu.MinFragmentLength = 0;
            _AutocompleteMenu.ImageList = mainForm.ilAutocompleteMenu;
            _AutocompleteMenu.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            _AutocompleteMenu.TipFont = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            _AutocompleteMenu.PreselectedFont = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            _AutocompleteMenu.MinimumSize = new Size(400, 100);
            _AutocompleteMenu.MaximumSize = new Size(500, 400);
            _AutocompleteMenu.MaxTooltipWidth = 500;
            _AutocompleteMenu.ToolTipDuration = 30000;
            _AutocompleteMenu.AppearInterval = (_Data.Config.AutocompleteDelay == 0 ? int.MaxValue : _Data!.Config.AutocompleteDelay);
            _AutocompleteMenu.ProcessKeyDown += _AutocompleteMenu_ProcessKeyDown;
            _AutocompleteMenu.ProcessKeyPressing += _AutocompleteMenu_ProcessKeyPressing;
        }

        private void _AutocompleteMenu_ProcessKeyDown(object? sender, ProcessKeyDownEventArgs e)
        {
            string fragmentText = _AutocompleteMenu.Fragment.Text.ToLower();

            if (fragmentText.Length == 0 && !e.ExplicitlySelected)
            {
                e.Select = false;
                return;
            }

            string itemText = e.Item.MenuText.ToLower();

            int pos = fragmentText.IndexOf('.');
            if (pos != -1) fragmentText = fragmentText.Substring(pos + 1);

            if (!itemText.StartsWith(fragmentText))
            {
                e.Select = false;
                return;
            }

            if (e.KeyData == Keys.Enter)
            {
                e.Select = true;
                e.Handled = true;
                return;
            }
        }

        private void _AutocompleteMenu_ProcessKeyPressing(object? sender, ProcessKeyPressingEventArgs e)
        {
            string fragmentText = _AutocompleteMenu.Fragment.Text.ToLower();

            if (fragmentText.Length == 0 && !e.ExplicitlySelected)
            {
                e.Select = false;
                return;
            }

            string itemText = e.Item.MenuText.ToLower();

            int pos = fragmentText.IndexOf('.');
            if (pos != -1) fragmentText = fragmentText.Substring(pos + 1);

            if (!itemText.StartsWith(fragmentText))
            {
                e.Select = false;
                return;
            }

            if (!(e.Item is AutocompleteItemCustom) || ((AutocompleteItemCustom)e.Item).NeverAutoSelectOnSymbol)
            {
                e.Select = false;
                return;
            }

            if (e.KeyChar == '*' && fragmentText.Length == 0)
            {
                e.Select = false;
                return;
            }

            e.Select = _AutoSelectSymbols.Contains(e.KeyChar) && !itemText.Contains(e.KeyChar);
        }

        public EditorTab(Data d, DataRow dr, TabPage tp, MainForm frmPrincipal) : this(d, tp, frmPrincipal)
        {
            LocalPath = dr.Field<string?>("path");
            _Position = (short)dr.Field<long>("position");
            _Id = (int)dr.Field<long>("id");
            ((CustomFctb)_TabPage.Controls[0]).Text = dr.Field<string>("text");
            _TabPage.Text = dr.Field<string>("name");

            _TabPage.Tag = this;

            PendingSaveDB = false;
        }

        public EditorTab(Data d, ClosedEditorTab cet, TabPage tp, MainForm frmPrincipal, int position) : this(d, tp, frmPrincipal)
        {
            _Id = cet.Id;
            LocalPath = cet.Path;
            ((CustomFctb)_TabPage.Controls[0]).Text = cet.Text;
            _TabPage.Text = cet.Name;

            _TabPage.Tag = this;

            cet.Reopen(position);

            PendingSaveDB = false;
        }

        public TabPage TabPage
        {
            get
            {
                return _TabPage;
            }
        }

        public short Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position == value) return;
                PendingSaveDB = true;
                _Position = value;
            }
        }

        public int Id { get => _Id; }

        public AutocompleteMenu AutocompleteMenu
        {
            get
            {
                return _AutocompleteMenu;
            }
        }

        public CustomFctb Fctb
        {
            get
            {
                return _Fctb;
            }
        }

        public void Save()
        {
            if (!PendingSaveDB) return;

            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                if (Id == -1)
                {
                    cmd.CommandText = "INSERT INTO editortabs (position,text,name,path) VALUES (:position,:text,:name,:path) RETURNING id";

                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("text", ((CustomFctb)_TabPage.Controls[0]).Text);
                    cmd.Parameters.AddWithValue("name", _TabPage.Text);
                    cmd.Parameters.AddWithValue("path", LocalPath == null ? DBNull.Value : LocalPath);

                    _Id = (int)(long)cmd.ExecuteScalar()!;
                }
                else
                {
                    cmd.CommandText = "UPDATE editortabs SET position=:position,text=:text,name=:name,path=:path WHERE id=:id";

                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("text", ((CustomFctb)_TabPage.Controls[0]).Text);
                    cmd.Parameters.AddWithValue("name", _TabPage.Text);
                    cmd.Parameters.AddWithValue("path", LocalPath == null ? DBNull.Value : LocalPath);
                    cmd.Parameters.AddWithValue("id", Id);

                    cmd.ExecuteNonQuery();
                }
            }

            PendingSaveDB = false;
        }

        public void SetClosed()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE editortabs SET closedAt=:closedAt,position=NULL WHERE id=:id";
                cmd.Parameters.AddWithValue("closedAt", DateTime.Now.Ticks);
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "DELETE FROM editortabs WHERE id=:id";
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

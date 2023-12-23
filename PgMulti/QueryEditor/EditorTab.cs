using FastColoredTextBoxNS;
using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using PgMulti.Tasks;
using PgMulti.AppData;
using System.Windows.Forms;
using Irony.Parsing;
using PgMulti.SqlSyntax;

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
        private MainForm _MainForm;
        private TabPage _TabPage;
        private CustomFctb _Fctb;
        private AutocompleteMenu _AutocompleteMenu;

        public EditorTab(Data d, MainForm mainForm, CreateEditorTabOptions o) : this(d, mainForm, false)
        {
            if (o.Text != null)
            {
                _Fctb.Text = o.Text;
            }
            if (o.Title != null)
            {
                _TabPage.Text = o.Title;
            }
            if (o.Path != null)
            {
                LocalPath = o.Path;
                _TabPage.ToolTipText = LocalPath;
            }

            PendingSaveDB = true;
            _MainForm.EnableTimerSaveTabs();

            InitEvents();
            if (o.Text != null) _Fctb.DoHighlighting();
            if (o.PendingFileSave) SetPendingFileSave();
        }

        public EditorTab(Data d, DataRow dr, MainForm frmPrincipal) : this(d, frmPrincipal, false)
        {
            LocalPath = dr.Field<string?>("path");
            _Position = (short)dr.Field<long>("position");
            _Id = (int)dr.Field<long>("id");
            _Fctb.Text = dr.Field<string>("text");
            _Fctb.ClearUndo();
            _TabPage.Text = dr.Field<string>("name");
            _TabPage.ToolTipText = LocalPath;

            PendingSaveDB = false;

            InitEvents();
            _Fctb.DoHighlighting();
        }

        public EditorTab(Data d, ClosedEditorTab cet, MainForm frmPrincipal, int position) : this(d, frmPrincipal, false)
        {
            _Id = cet.Id;
            LocalPath = cet.Path;
            _Fctb.Text = cet.Text;
            _TabPage.Text = cet.Name;
            _TabPage.ToolTipText = LocalPath;

            cet.Reopen(position);

            PendingSaveDB = false;

            InitEvents();
            _Fctb.DoHighlighting();
        }

        private EditorTab(Data d, MainForm mainForm, bool _)
        {
            _Data = d;
            _MainForm = mainForm;

            _Fctb = new CustomFctb(true);
            _Fctb.SetParser(_Data!.PGLanguageData);
            _Fctb.CaretBlinking = false;
            _Fctb.AutoScrollMinSize = new Size(669, 645);
            _Fctb.BackBrush = null;
            _Fctb.CharHeight = 15;
            _Fctb.CharWidth = 7;
            _Fctb.Cursor = Cursors.IBeam;
            _Fctb.DisabledColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            _Fctb.Dock = DockStyle.Fill;
            _Fctb.Font = new Font("Cascadia Code", _Data!.Config.FontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            _Fctb.IsReplaceMode = false;
            _Fctb.Location = new Point(0, 0);
            _Fctb.Name = "_Fctb";
            _Fctb.SelectionColor = Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            _Fctb.Size = new Size(668, 380);
            _Fctb.TabIndex = 0;
            _Fctb.Text = "";
            _Fctb.Zoom = 100;
            _Fctb.AcceptsTab = true;
            _Fctb.AcceptsReturn = true;
            _Fctb.AutoIndent = true;
            _Fctb.AutoIndentExistingLines = false;
            _Fctb.Paddings = new Padding(20);
            _Fctb.AutoCompleteBrackets = true;
            _Fctb.AutoIndentChars = false;


            _TabPage = new TabPage(Properties.Text.new_doc_title);

            _MainForm.SqlEditorTabControl.SuspendDrawing();
            _TabPage.Controls.Add(_Fctb);
            _TabPage.Controls.Add(_Fctb.HScrollBar);
            _TabPage.Controls.Add(_Fctb.VScrollBar);
            _MainForm.SqlEditorTabControl.TabPages.Insert(_MainForm.SqlEditorTabControl.TabPages.Count - 1, _TabPage);
            _MainForm.SqlEditorTabControl.ResumeDrawing();

            _TabPage.Tag = this;

            _AutocompleteMenu = new AutocompleteMenu(_Fctb);
            _AutocompleteMenu.ForeColor = Color.FromArgb(30, 30, 30);
            _AutocompleteMenu.BackColor = Color.FromArgb(247, 249, 254);
            _AutocompleteMenu.SelectedColor = Color.FromArgb(196, 213, 255);
            _AutocompleteMenu.SearchPattern = @"[\w\.\""]";
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
        }

        private void InitEvents()
        {
            _Fctb.ResetTextChangedDelayedEvent();

            _Fctb.TextChangedDelayed += fctbSql_TextChangedDelayed;
            _Fctb.Enter += fctbSql_Enter;
            _Fctb.Leave += fctbSql_Leave;
            _Fctb.KeyDown += fctbSql_KeyDown;
            _Fctb.MouseUp += fctbSql_MouseUp;
            _Fctb.ParseTreeUpdated += fctbSql_ParseTreeUpdated;
            _Fctb.SecondaryFormShowed += fctbSql_SecondaryFormShowed;
            _Fctb.SecondaryFormClosed += fctbSql_SecondaryFormClosed;
            _Fctb.AutoIndentNeeded += fctbSql_AutoIndentNeeded;
            _Fctb.KeyPressing += fctbSql_KeyPressing;

            _AutocompleteMenu.ProcessKeyDown += _AutocompleteMenu_ProcessKeyDown;
            _AutocompleteMenu.ProcessKeyPressing += _AutocompleteMenu_ProcessKeyPressing;
        }

        private void SetPendingFileSave()
        {
            if (!_TabPage.Text.EndsWith(" *")) _TabPage.Text += " *";

            if (!_MainForm.Text.EndsWith(" *")) _MainForm.Text += " *";
            PendingSaveDB = true;
            _MainForm.EnableTimerSaveTabs();

            _MainForm.UpdateSearchResults();
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

        private void fctbSql_KeyPressing(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                if (_Fctb.Selection.Start == _Fctb.Selection.End)
                {
                    System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[_Fctb.Selection.Start.iLine].Substring(0, _Fctb.Selection.Start.iChar), @"^\s+$");

                    if (m.Success)
                    {
                        int n = m.Length % _Fctb.TabLength;
                        if (n == 0)
                        {
                            n = _Fctb.TabLength;
                        }

                        _Fctb.BeginAutoUndo();
                        _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                        _Fctb.Selection.End = new Place(_Fctb.Selection.End.iChar - n, _Fctb.Selection.End.iLine);
                        _Fctb.InsertText("");
                        _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                        _Fctb.EndAutoUndo();

                        e.Handled = true;
                    }
                }
            }
        }

        private void fctbSql_TextChangedDelayed(object? sender, EventArgs e)
        {
            SetPendingFileSave();
        }

        private void fctbSql_Enter(object? sender, EventArgs e)
        {
            _MainForm.EnableTimerPosition(true);
        }

        private void fctbSql_Leave(object? sender, EventArgs e)
        {
            _MainForm.EnableTimerPosition(false);
        }

        private void fctbSql_KeyDown(object? sender, KeyEventArgs e)
        {
            _MainForm.SqlEditorProcessKey((CustomFctb)sender!, e);
        }

        private void fctbSql_MouseUp(object? sender, MouseEventArgs e)
        {
            _MainForm.SqlEditorProcessClick((CustomFctb)sender!, e);
        }

        private void fctbSql_ParseTreeUpdated(object? sender, EventArgs e)
        {
            _MainForm.SqlEditorProcessParseTreeUpdated((CustomFctb)sender!);
        }

        private void fctbSql_SecondaryFormClosed(object? sender, SecondaryFormEventArgs e)
        {
            _MainForm.SecondaryForms.Remove(e.Form);
        }

        private void fctbSql_SecondaryFormShowed(object? sender, SecondaryFormEventArgs e)
        {
            _MainForm.SecondaryForms.Add(e.Form);
        }

        private void fctbSql_AutoIndentNeeded(object? sender, AutoIndentEventArgs e)
        {
            if (e.IsCurrentLine)
            {
                string currentLineText = e.LineText.Trim();
                if (currentLineText == "" || currentLineText == ")")
                {
                    string previousText = _Fctb.GetRange(new Place(0, 0), new Place(0, e.iLine)).Text;
                    bool semiColonAdded;
                    bool dollarStringTagAdded;
                    List<AstNode>? stmts = ListStatements(previousText, out semiColonAdded, out dollarStringTagAdded);
                    if (stmts == null) return;

                    List<AstNode> tokens = stmts[stmts.Count - 1].RecursiveTokens;

                    int sentenceIndent = -1;
                    bool isNewSentenceAfterSemiColon;

                    if (dollarStringTagAdded)
                    {
                        AstNode dollarStringContent = stmts[stmts.Count - 1]["stmtContent"]!.Children.First(an => an[0].Name == "dollarString")[0]["dollarStringContent"]!;
                        List<AstNode> dollarStringContentTokens = dollarStringContent.RecursiveTokens;

                        if (dollarStringContentTokens[dollarStringContentTokens.Count - 1].Token!.Text == ";")
                        {
                            isNewSentenceAfterSemiColon = true;

                            for (int i = dollarStringContentTokens.Count - 2; i >= 0; i--)
                            {
                                string tokenText = dollarStringContentTokens[i].Token!.Text.ToUpper();

                                if (tokenText == ";" || tokenText == "DECLARE" || tokenText == "THEN" || tokenText == "ELSE" || tokenText == "LOOP"
                                     || (tokenText == "BEGIN" && _Fctb.Lines[dollarStringContentTokens[i].Token!.Location.Line].ToUpper().EndsWith(tokenText))) // Because BEGIN can also be part of a stmt, but we cannot have complete syntax validation here
                                {
                                    sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[dollarStringContentTokens[i + 1].Token!.Location.Line], @"^\s*").Length;
                                    break;
                                }
                            }

                            if (sentenceIndent == -1)
                            {
                                sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[dollarStringContentTokens[0].Token!.Location.Line], @"^\s*").Length;
                            }
                        }
                        else
                        {
                            isNewSentenceAfterSemiColon = false;
                        }
                    }
                    else
                    {
                        isNewSentenceAfterSemiColon = !semiColonAdded;

                        if (isNewSentenceAfterSemiColon)
                        {
                            int lastStmtStartLine = tokens[0].Token!.Location.Line;
                            sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[lastStmtStartLine], @"^\s*").Length;
                        }
                    }


                    if (isNewSentenceAfterSemiColon)
                    {
                        e.AbsoluteIndentation = sentenceIndent;
                    }
                    else
                    {
                        int lastTokenIndex;
                        if (dollarStringTagAdded)
                        {
                            lastTokenIndex = tokens.Count - 4;
                        }
                        else
                        {
                            lastTokenIndex = tokens.Count - 2;
                        }
                        Token previousToken = tokens[lastTokenIndex].Token!;
                        string previousTokenText = previousToken.Text.ToUpper();

                        if (previousTokenText == "(")
                        {
                            if (currentLineText == ")")
                            {
                                int previousIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[previousToken.Location.Line], @"^\s*").Length;
                                string replacementText;

                                if (_Fctb.Lines[previousToken.Location.Line].Trim() == "(")
                                {
                                    replacementText = "(\r\n" + new String(' ', previousIndent + e.TabLength) + "\r\n" + new String(' ', previousIndent) + ")";
                                }
                                else
                                {
                                    replacementText = "\r\n" + new String(' ', previousIndent) + "(\r\n" + new String(' ', previousIndent + e.TabLength) + "\r\n" + new String(' ', previousIndent) + ")";
                                }

                                _Fctb.BeginAutoUndo();
                                _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                                _Fctb.Selection.Start = new Place(previousToken.Location.Column, previousToken.Location.Line);
                                _Fctb.Selection.End = new Place(_Fctb.Lines[e.iLine].Length, e.iLine);
                                _Fctb.InsertText(replacementText);
                                _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                                _Fctb.EndAutoUndo();

                                Place finalCaretPosition;
                                if (_Fctb.Lines[previousToken.Location.Line].Trim() == "(")
                                {
                                    finalCaretPosition = new Place(_Fctb.Lines[e.iLine].Length, e.iLine);
                                }
                                else
                                {
                                    finalCaretPosition = new Place(_Fctb.Lines[e.iLine + 1].Length, e.iLine + 1);
                                }

                                _Fctb.Selection.Start = finalCaretPosition;
                                _Fctb.Selection.End = finalCaretPosition;
                                e.AbsoluteIndentation = 0;
                            }
                            else
                            {
                                e.Shift = e.TabLength;
                            }
                        }
                        else if (dollarStringTagAdded &&
                                (
                                    previousTokenText == "DECLARE"
                                    || (previousTokenText == "BEGIN" && _Fctb.Lines[previousToken.Location.Line].ToUpper().EndsWith("BEGIN")) // Because BEGIN can also be part of a stmt, but we cannot have complete syntax validation here
                                    || previousTokenText == "THEN"
                                    || previousTokenText == "ELSE"
                                    || previousTokenText == "LOOP"
                                    || previousToken.Terminal.Name == "dollar_string_tag"
                                )
                            )
                        {
                            e.Shift = e.TabLength;
                        }
                    }
                }

            }

        }

        protected List<AstNode>? ListStatements(string txt, out bool semiColonAdded, out bool dollarStringTagAdded)
        {
            Parser parser = new Parser(_Data!.PGSimpleLanguageData);
            ParseTree parseTree = parser.Parse(txt);

            dollarStringTagAdded = false;

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                int indexInsertPointToken = parseTree.Tokens.Count - 1;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "EOF") indexInsertPointToken--;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "line_comment") indexInsertPointToken--;

                Token insertPointToken = parseTree.Tokens[indexInsertPointToken];
                Token endToken = parseTree.Tokens[parseTree.Tokens.Count - 1];

                int insertPoint = insertPointToken.Location.Position + insertPointToken.Length;
                int end = endToken.Location.Position + endToken.Length;

                txt = txt.Substring(0, insertPoint) + ";" + txt.Substring(insertPoint, end - insertPoint);
                semiColonAdded = true;

                parseTree = parser.Parse(txt);

                if (parseTree.Status == ParseTreeStatus.Error)
                {
                    string? stringTag = parseTree.Tokens.Select(tk => tk.Text).FirstOrDefault(tt => System.Text.RegularExpressions.Regex.Match(tt, @"^\$.*\$$").Success);

                    if (stringTag != null)
                    {
                        txt = txt.Substring(0, insertPoint) + " x " + stringTag + txt.Substring(insertPoint, end - insertPoint);
                        dollarStringTagAdded = true;

                        parseTree = parser.Parse(txt);
                    }
                }
            }
            else
            {
                semiColonAdded = false;
            }


            if (parseTree.Status == ParseTreeStatus.Error) return null;

            AstNode astRoot = AstNode.ProcessParseTree(parseTree);

            if (astRoot.Children.Count == 0) return null;

            return astRoot.Children[0].Children;
        }

        public class CreateEditorTabOptions
        {
            public string? Title = null;
            public string? Text = null;
            public string? Path = null;
            public bool Focus = false;
            public bool PendingFileSave = true;
        }
    }
}

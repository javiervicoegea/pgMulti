using Irony;
using PgMulti.AppData;
using PgMulti.Forms;
using PgMulti.Properties;
using PgMulti.QueryEditor;
using System.Globalization;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.DataFormats;

namespace PgMulti
{
    public partial class LogForm : Form
    {
        private Data _Data;
        private Panel? _PnlSelectedItem = null;
        private int? _LastLogIdScroll = null;
        private bool _CompletedLog = false;
        private Log? _SelectedLog = null;
        private ClosedEditorTab? _SelectedClosedTab = null;
        private DB? _FilterDB;
        private DB? _PreselectedDB;
        private CustomFctb fctbSql;
        private bool _ClosedTabsMode;

        public LogForm(Data d, bool closedTabsMode, DB? preselectedDB)
        {
            InitializeComponent();

            scFilter.Panel1Collapsed = true;

            fctbSql = new CustomFctb();
            fctbSql.AutoCompleteBracketsList = new char[] { '(', ')', '{', '}', '[', ']', '\"', '\"', '\'', '\'' };
            fctbSql.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            fctbSql.AutoScrollMinSize = new Size(71, 59);
            fctbSql.BackBrush = null;
            fctbSql.CharHeight = 19;
            fctbSql.CharWidth = 10;
            fctbSql.DefaultMarkerSize = 8;
            fctbSql.DisabledColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            fctbSql.Dock = DockStyle.Fill;
            fctbSql.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
            fctbSql.Hotkeys = "PgUp=GoPageUp, PgDn=GoPageDown, End=GoEnd, Home=GoHome, Left=GoLeft, Up=GoUp, Right=GoRight, Down=GoDown, F3=FindNext, Shift+PgUp=GoPageUpWithSelection, Shift+PgDn=GoPageDownWithSelection, Shift+End=GoEndWithSelection, Shift+Home=GoHomeWithSelection, Shift+Left=GoLeftWithSelection, Shift+Up=GoUpWithSelection, Shift+Right=GoRightWithSelection, Shift+Down=GoDownWithSelection, Ctrl+End=GoLastLine, Ctrl+Home=GoFirstLine, Ctrl+Left=GoWordLeft, Ctrl+Up=ScrollUp, Ctrl+Right=GoWordRight, Ctrl+Down=ScrollDown, Ctrl+Ins=Copy, Ctrl+Del=ClearWordRight, Ctrl+0=ZoomNormal, Ctrl+A=SelectAll, Ctrl+C=Copy, Ctrl+F=FindDialog, Ctrl+G=GoToDialog, Ctrl+Add=ZoomIn, Ctrl+Subtract=ZoomOut, Ctrl+OemMinus=NavigateBackward, Ctrl+Shift+End=GoLastLineWithSelection, Ctrl+Shift+Home=GoFirstLineWithSelection, Ctrl+Shift+Left=GoWordLeftWithSelection, Ctrl+Shift+Right=GoWordRightWithSelection, Ctrl+Shift+OemMinus=NavigateForward, Alt+F=FindChar";
            fctbSql.IsReplaceMode = false;
            fctbSql.Location = new Point(0, 0);
            fctbSql.Paddings = new Padding(20);
            fctbSql.ReadOnly = true;
            fctbSql.SelectionColor = Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            //fctbSql.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbSql.ServiceColors")));
            fctbSql.Size = new Size(461, 663);
            fctbSql.TabIndex = 0;
            fctbSql.Zoom = 100;
            scList.Panel2.Controls.Add(fctbSql);


            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;

            string shortFormat = $"{dtf.ShortDatePattern} {dtf.ShortTimePattern}";
            dtpTimestampFromFilter.CustomFormat = shortFormat;
            dtpTimestampToFilter.CustomFormat = shortFormat;

            InitializeText();

            _Data = d;
            _PreselectedDB = preselectedDB;

            fctbSql.SetParser(_Data.PGLanguageData);
            DialogResult = DialogResult.Cancel;

            SetMode(closedTabsMode);
        }

        public Log? SelectedLog
        {
            get
            {
                return _SelectedLog;
            }
        }

        public ClosedEditorTab? SelectedClosedTab
        {
            get
            {
                return _SelectedClosedTab;
            }
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
        }

        private ClosedEditorTab.Filter GetClosedTabsFilter()
        {
            ClosedEditorTab.Filter f = new ClosedEditorTab.Filter();

            if (tsbFilter.Checked)
            {
                f.FromTimestamp = dtpTimestampFromFilter.Checked ? dtpTimestampFromFilter.Value : null;
                f.ToTimestamp = dtpTimestampToFilter.Checked ? dtpTimestampToFilter.Value : null;
                f.Text = txtTextFilter.Text;
            }

            return f;
        }

        private Log.Filter GetLogFilter()
        {
            Log.Filter f = new Log.Filter();

            if (tsbFilter.Checked)
            {
                f.DB = _FilterDB;
                f.FromTimestamp = dtpTimestampFromFilter.Checked ? dtpTimestampFromFilter.Value : null;
                f.ToTimestamp = dtpTimestampToFilter.Checked ? dtpTimestampToFilter.Value : null;
                f.Text = txtTextFilter.Text;
            }

            return f;
        }

        private void UpdateFilter()
        {
            Enabled = false;
            flpLog.Controls.Clear();
            Select(null);

            if (_ClosedTabsMode)
            {
                LoadClosedTabs(GetClosedTabsFilter());
            }
            else
            {
                LoadLogs(null, GetLogFilter());
            }
            Enabled = true;
        }

        private void SetMode(bool closedTabsMode)
        {
            _ClosedTabsMode = closedTabsMode;
            _SelectedLog = null;
            _SelectedClosedTab = null;
            _PnlSelectedItem = null;
            _LastLogIdScroll = null;
            _CompletedLog = false;
            flpLog.Controls.Clear();

            if (closedTabsMode)
            {
                tsddbMode.Text = Properties.Text.closed_tabs_log;
                tsbEditReopen.Text = Properties.Text.reopen_selected_tab;
                LoadClosedTabs(GetClosedTabsFilter());
            }
            else
            {
                tsddbMode.Text = Properties.Text.execution_log;
                tsbEditReopen.Text = Properties.Text.edit_selected_script;
                LoadLogs(null, GetLogFilter());
            }

        }

        private void LoadLogs(int? lastLogId, Log.Filter f)
        {
            int? newLastLogId = null;
            foreach (Log h in Log.List(_Data, lastLogId, f))
            {
                Panel pnlLogItem;
                Label txtSqlSummary;
                Label txtDBs;
                Label txtTimestamp;

                pnlLogItem = new Panel();
                txtTimestamp = new Label();
                txtSqlSummary = new Label();
                txtDBs = new Label();

                pnlLogItem.Tag = h;

                pnlLogItem.SuspendLayout();

                // 
                // pnlLogItem
                // 
                pnlLogItem.Controls.Add(txtSqlSummary);
                pnlLogItem.Controls.Add(txtDBs);
                pnlLogItem.Controls.Add(txtTimestamp);
                pnlLogItem.Location = new Point(3, 3);
                pnlLogItem.Name = "pnlLogItem";
                pnlLogItem.Size = new Size(506, 136);
                pnlLogItem.TabIndex = 0;
                pnlLogItem.BorderStyle = BorderStyle.FixedSingle;
                pnlLogItem.BackColor = SystemColors.Control;
                // 
                // txtTimestamp
                // 
                txtTimestamp.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
                txtTimestamp.Location = new Point(9, 10);
                txtTimestamp.Name = "txtTimestamp";
                txtTimestamp.Size = new Size(478, 25);
                txtTimestamp.TabIndex = 0;
                txtTimestamp.Text = string.Format("{0:g}", h.Timestamp);
                // 
                // txtSqlSummary
                // 
                txtSqlSummary.Location = new Point(9, 35);
                txtSqlSummary.Name = "txtSql";
                txtSqlSummary.Size = new Size(478, 60);
                txtSqlSummary.TabIndex = 0;
                txtSqlSummary.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
                txtSqlSummary.Text = h.SqlText;
                // 
                // txtDBs
                // 
                txtDBs.AutoEllipsis = true;
                txtDBs.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
                txtDBs.Location = new Point(9, 95);
                txtDBs.Name = "txtDBs";
                txtDBs.Size = new Size(478, 25);
                txtDBs.TabIndex = 0;
                txtDBs.Text = h.DBsDescription;


                flpLog.Controls.Add(pnlLogItem);

                pnlLogItem.Click += pnlLogItem_Click;
                txtTimestamp.Click += pnlLogItemSubControl_Click;
                txtSqlSummary.Click += pnlLogItemSubControl_Click;
                txtDBs.Click += pnlLogItemSubControl_Click;


                pnlLogItem.ResumeLayout(false);

                newLastLogId = h.Id;
            }

            if (newLastLogId.HasValue)
            {
                _LastLogIdScroll = newLastLogId.Value;
            }
            else
            {
                _CompletedLog = true;
            }
        }

        private void LoadClosedTabs(ClosedEditorTab.Filter f)
        {
            foreach (ClosedEditorTab cet in _Data.ListClosedEditorTabs(f))
            {
                Panel pnlClosedTab;
                Label txtSqlSummary;
                Label txtName;
                Label txtTimestamp;

                pnlClosedTab = new Panel();
                txtTimestamp = new Label();
                txtSqlSummary = new Label();
                txtName = new Label();

                pnlClosedTab.Tag = cet;

                pnlClosedTab.SuspendLayout();

                // 
                // pnlClosedTab
                // 
                pnlClosedTab.BackColor = SystemColors.Control;
                pnlClosedTab.Controls.Add(txtSqlSummary);
                pnlClosedTab.Controls.Add(txtName);
                pnlClosedTab.Controls.Add(txtTimestamp);
                pnlClosedTab.Location = new Point(3, 3);
                pnlClosedTab.Name = "pnlClosedTab";
                pnlClosedTab.Size = new Size(506, 136);
                pnlClosedTab.TabIndex = 0;
                pnlClosedTab.BorderStyle = BorderStyle.FixedSingle;
                // 
                // txtTimestamp
                // 
                txtTimestamp.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
                txtTimestamp.Location = new Point(9, 10);
                txtTimestamp.Name = "txtTimestamp";
                txtTimestamp.Size = new Size(478, 25);
                txtTimestamp.TabIndex = 0;
                txtTimestamp.Text = string.Format("{0:g}", cet.ClosedAt);
                // 
                // txtSqlSummary
                // 
                txtSqlSummary.AutoEllipsis = true;
                txtSqlSummary.Location = new Point(9, 35);
                txtSqlSummary.Name = "txtSql";
                txtSqlSummary.Size = new Size(478, 60);
                txtSqlSummary.TabIndex = 0;
                txtSqlSummary.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
                txtSqlSummary.Text = Data.AutoEllipsis(cet.Text);
                // 
                // txtName
                // 
                txtName.AutoEllipsis = true;
                txtName.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
                txtName.Location = new Point(9, 95);
                txtName.Name = "txtName";
                txtName.Size = new Size(478, 25);
                txtName.TabIndex = 0;
                txtName.Text = cet.Name;


                flpLog.Controls.Add(pnlClosedTab);

                pnlClosedTab.Click += pnlLogItem_Click;
                txtTimestamp.Click += pnlLogItemSubControl_Click;
                txtSqlSummary.Click += pnlLogItemSubControl_Click;
                txtName.Click += pnlLogItemSubControl_Click;


                pnlClosedTab.ResumeLayout(false);
            }
            _CompletedLog = true;
        }

        private void pnlLogItemSubControl_Click(object? sender, EventArgs e)
        {
            Control ctrl = (Control)sender!;
            pnlLogItem_Click(ctrl.Parent, e);
        }

        private void pnlLogItem_Click(object? sender, EventArgs e)
        {
            Panel pnlItem = (Panel)sender!;
            Select(pnlItem);
        }

        private void Select(Panel? pnlItem)
        {
            if (_PnlSelectedItem != null)
            {
                _PnlSelectedItem.BorderStyle = BorderStyle.FixedSingle;
                _PnlSelectedItem.BackColor = Color.FromKnownColor(KnownColor.Control);
            }

            _PnlSelectedItem = pnlItem;

            if (pnlItem == null)
            {
                _SelectedLog = null;
                _SelectedClosedTab = null;
                fctbSql.Text = "";
            }
            else
            {
                pnlItem.BorderStyle = BorderStyle.Fixed3D;
                pnlItem.BackColor = Color.White;

                if (pnlItem.Tag is Log)
                {
                    Log h = (Log)pnlItem.Tag;

                    fctbSql.Text = h.SqlText;
                    _SelectedLog = h;
                }
                else if (pnlItem.Tag is ClosedEditorTab)
                {
                    ClosedEditorTab cet = (ClosedEditorTab)pnlItem.Tag;

                    fctbSql.Text = cet.Text;
                    _SelectedClosedTab = cet;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        private void CheckScroll()
        {
            if (_CompletedLog) return;

            if (flpLog.VerticalScroll.Value + flpLog.Height > 0.9 * flpLog.VerticalScroll.Maximum)
            {
                LoadLogs(_LastLogIdScroll, GetLogFilter());
            }
        }


        private void flpLog_Scroll(object sender, ScrollEventArgs e)
        {
            CheckScroll();
        }

        private void flpLog_MouseWheel(object sender, MouseEventArgs e)
        {
            CheckScroll();
        }

        private void tsmiExecutionLog_Click(object sender, EventArgs e)
        {
            SetMode(false);
        }

        private void tsmiClosedTabs_Click(object sender, EventArgs e)
        {
            SetMode(true);
        }

        private void tsbFilter_Click(object sender, EventArgs e)
        {
            scFilter.Panel1Collapsed = !tsbFilter.Checked;
            UpdateFilter();
        }

        private void dtpTimestampFromFilter_ValueChanged(object sender, EventArgs e)
        {
            tmrDelayFilterUpdate.Enabled = true;
        }

        private void dtpTimestampToFilter_ValueChanged(object sender, EventArgs e)
        {
            tmrDelayFilterUpdate.Enabled = true;
        }

        private void txtTextFilter_TextChanged(object sender, EventArgs e)
        {
            tmrDelayFilterUpdate.Enabled = true;
        }

        private void btnSelectDB_Click(object sender, EventArgs e)
        {
            SelectDBForm f = new SelectDBForm(_Data, _PreselectedDB);
            f.ShowDialog(this);
            if (f.DialogResult != DialogResult.OK) return;

            _FilterDB = f.SelectedDB;
            txtDBFilter.Text = _FilterDB == null ? "" : _FilterDB.Alias;
            UpdateFilter();
        }

        private void btnRemoveDBFilter_Click(object sender, EventArgs e)
        {
            _FilterDB = null;
            txtDBFilter.Text = "";
            UpdateFilter();
        }

        private void tsbEditReopen_Click(object sender, EventArgs e)
        {
            if (_SelectedLog == null && _SelectedClosedTab == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tmrDelayFilterUpdate_Tick(object sender, EventArgs e)
        {
            UpdateFilter();
            tmrDelayFilterUpdate.Enabled = false;
        }


        #region TextI18n
        private void InitializeText()
        {
            tsddbMode.Text = Properties.Text.execution_log;
            tsmiExecutionLog.Text = Properties.Text.execution_log;
            tsmiClosedTabs.Text = Properties.Text.closed_tabs_log;
            tsbEditReopen.Text = Properties.Text.edit_selected_script;
            tsbFilter.Text = Properties.Text.filter;
            Text = Properties.Text.history;
            lblTimestampFromFilter.Text = Properties.Text.from + ":";
            lblTimestampToFilter.Text = Properties.Text.to + ":";
            lblTextFilter.Text = Properties.Text.text + ":";
            lblDBFilter.Text = Properties.Text.db + ":";
        }
        #endregion
    }
}

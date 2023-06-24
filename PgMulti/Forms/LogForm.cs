using Irony;
using PgMulti.AppData;
using PgMulti.QueryEditor;
using System.Runtime.ConstrainedExecution;

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

        public LogForm(Data d, bool closedTabsMode)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
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

        private void SetMode(bool closedTabsMode)
        {
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
                LoadClosedTabs();
            }
            else
            {
                tsddbMode.Text = Properties.Text.execution_log;
                tsbEditReopen.Text = Properties.Text.edit_selected_script;
                LoadLogs(null);
            }

        }

        private void LoadLogs(int? lastLogId)
        {
            int? newLastLogId = null;
            foreach (Log h in Log.List(_Data, lastLogId))
            {
                Panel pnlLogItem;
                Label txtSqlSummary;
                Label txtDBs;
                Label txtTimestamp;

                pnlLogItem = new System.Windows.Forms.Panel();
                txtTimestamp = new System.Windows.Forms.Label();
                txtSqlSummary = new System.Windows.Forms.Label();
                txtDBs = new System.Windows.Forms.Label();

                pnlLogItem.Tag = h;

                pnlLogItem.SuspendLayout();

                // 
                // pnlLogItem
                // 
                pnlLogItem.Controls.Add(txtSqlSummary);
                pnlLogItem.Controls.Add(txtDBs);
                pnlLogItem.Controls.Add(txtTimestamp);
                pnlLogItem.Location = new System.Drawing.Point(3, 3);
                pnlLogItem.Name = "pnlLogItem";
                pnlLogItem.Size = new System.Drawing.Size(506, 136);
                pnlLogItem.TabIndex = 0;
                pnlLogItem.BorderStyle = BorderStyle.FixedSingle;
                // 
                // txtTimestamp
                // 
                txtTimestamp.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
                txtTimestamp.Location = new System.Drawing.Point(9, 10);
                txtTimestamp.Name = "txtTimestamp";
                txtTimestamp.Size = new System.Drawing.Size(478, 25);
                txtTimestamp.TabIndex = 0;
                txtTimestamp.Text = string.Format("{0:g}", h.Timestamp);
                // 
                // txtSqlSummary
                // 
                txtSqlSummary.Location = new System.Drawing.Point(9, 35);
                txtSqlSummary.Name = "txtSql";
                txtSqlSummary.Size = new System.Drawing.Size(478, 60);
                txtSqlSummary.TabIndex = 0;
                txtSqlSummary.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                txtSqlSummary.Text = h.SqlText;
                // 
                // txtDBs
                // 
                txtDBs.AutoEllipsis = true;
                txtDBs.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                txtDBs.Location = new System.Drawing.Point(9, 95);
                txtDBs.Name = "txtDBs";
                txtDBs.Size = new System.Drawing.Size(478, 25);
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

        private void LoadClosedTabs()
        {
            foreach (ClosedEditorTab cet in _Data.ListClosedEditorTabs())
            {
                Panel pnlClosedTab;
                Label txtSqlSummary;
                Label txtName;
                Label txtTimestamp;

                pnlClosedTab = new System.Windows.Forms.Panel();
                txtTimestamp = new System.Windows.Forms.Label();
                txtSqlSummary = new System.Windows.Forms.Label();
                txtName = new System.Windows.Forms.Label();

                pnlClosedTab.Tag = cet;

                pnlClosedTab.SuspendLayout();

                // 
                // pnlClosedTab
                // 
                pnlClosedTab.Controls.Add(txtSqlSummary);
                pnlClosedTab.Controls.Add(txtName);
                pnlClosedTab.Controls.Add(txtTimestamp);
                pnlClosedTab.Location = new System.Drawing.Point(3, 3);
                pnlClosedTab.Name = "pnlClosedTab";
                pnlClosedTab.Size = new System.Drawing.Size(506, 136);
                pnlClosedTab.TabIndex = 0;
                pnlClosedTab.BorderStyle = BorderStyle.FixedSingle;
                // 
                // txtTimestamp
                // 
                txtTimestamp.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
                txtTimestamp.Location = new System.Drawing.Point(9, 10);
                txtTimestamp.Name = "txtTimestamp";
                txtTimestamp.Size = new System.Drawing.Size(478, 25);
                txtTimestamp.TabIndex = 0;
                txtTimestamp.Text = string.Format("{0:g}", cet.ClosedAt);
                // 
                // txtSqlSummary
                // 
                txtSqlSummary.AutoEllipsis = true;
                txtSqlSummary.Location = new System.Drawing.Point(9, 35);
                txtSqlSummary.Name = "txtSql";
                txtSqlSummary.Size = new System.Drawing.Size(478, 60);
                txtSqlSummary.TabIndex = 0;
                txtSqlSummary.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                txtSqlSummary.Text = Data.AutoEllipsis(cet.Text);
                // 
                // txtName
                // 
                txtName.AutoEllipsis = true;
                txtName.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                txtName.Location = new System.Drawing.Point(9, 95);
                txtName.Name = "txtName";
                txtName.Size = new System.Drawing.Size(478, 25);
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

        private void Select(Panel pnlItem)
        {
            if (_PnlSelectedItem != null)
            {
                _PnlSelectedItem.BorderStyle = BorderStyle.FixedSingle;
                _PnlSelectedItem.BackColor = Color.FromKnownColor(KnownColor.Control);
            }

            pnlItem.BorderStyle = BorderStyle.Fixed3D;
            pnlItem.BackColor = Color.White;
            _PnlSelectedItem = pnlItem;

            if (_PnlSelectedItem.Tag is Log)
            {
                Log h = (Log)_PnlSelectedItem.Tag;

                fctbSql.Text = h.SqlText;
                _SelectedLog = h;
            }
            else if (_PnlSelectedItem.Tag is ClosedEditorTab)
            {
                ClosedEditorTab cet = (ClosedEditorTab)_PnlSelectedItem.Tag;

                fctbSql.Text = cet.Text;
                _SelectedClosedTab = cet;
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        private void CheckScroll()
        {
            if (_CompletedLog) return;

            if (flpLog.VerticalScroll.Value + flpLog.Height > 0.9 * flpLog.VerticalScroll.Maximum)
            {
                LoadLogs(_LastLogIdScroll);
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

        private void tsbEditReopen_Click(object sender, EventArgs e)
        {
            if (_SelectedLog == null && _SelectedClosedTab == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }


        #region TextI18n
        private void InitializeText()
        {
            this.tsddbMode.Text = Properties.Text.execution_log;
            this.tsmiExecutionLog.Text = Properties.Text.execution_log;
            this.tsmiClosedTabs.Text = Properties.Text.closed_tabs_log;
            this.tsbEditReopen.Text = Properties.Text.edit_selected_script;
            this.Text = Properties.Text.history;
        }
        #endregion
    }
}

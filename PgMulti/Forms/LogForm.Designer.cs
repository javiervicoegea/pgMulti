using Timer = System.Windows.Forms.Timer;

namespace PgMulti
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            tsc = new ToolStripContainer();
            scFilter = new SplitContainer();
            btnRemoveDBFilter = new Button();
            btnSelectDBFilter = new Button();
            txtTextFilter = new TextBox();
            dtpTimestampToFilter = new DateTimePicker();
            lblTextFilter = new Label();
            dtpTimestampFromFilter = new DateTimePicker();
            lblTimestampToFilter = new Label();
            txtDBFilter = new TextBox();
            lblTimestampFromFilter = new Label();
            lblDBFilter = new Label();
            scList = new SplitContainer();
            flpLog = new FlowLayoutPanel();
            ts = new ToolStrip();
            tsddbMode = new ToolStripDropDownButton();
            tsmiExecutionLog = new ToolStripMenuItem();
            tsmiClosedTabs = new ToolStripMenuItem();
            tsbFilter = new ToolStripButton();
            tsbEditReopen = new ToolStripButton();
            pnlDBFilter = new Panel();
            tmrDelayFilterUpdate = new Timer();
            tsc.ContentPanel.SuspendLayout();
            tsc.TopToolStripPanel.SuspendLayout();
            tsc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scFilter).BeginInit();
            scFilter.Panel1.SuspendLayout();
            scFilter.Panel2.SuspendLayout();
            scFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scList).BeginInit();
            scList.Panel1.SuspendLayout();
            scList.SuspendLayout();
            ts.SuspendLayout();
            pnlDBFilter.SuspendLayout();
            SuspendLayout();
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            tsc.ContentPanel.Controls.Add(scFilter);
            tsc.ContentPanel.Size = new Size(1373, 750);
            tsc.Dock = DockStyle.Fill;
            tsc.Location = new Point(0, 0);
            tsc.Name = "tsc";
            tsc.Size = new Size(1373, 787);
            tsc.TabIndex = 0;
            tsc.Text = "tsc";
            // 
            // tsc.TopToolStripPanel
            // 
            tsc.TopToolStripPanel.Controls.Add(ts);
            // 
            // scFilter
            // 
            scFilter.Dock = DockStyle.Fill;
            scFilter.Location = new Point(0, 0);
            scFilter.Name = "scFilter";
            // 
            // scFilter.Panel1
            // 
            scFilter.Panel1.Controls.Add(pnlDBFilter);
            scFilter.Panel1.Controls.Add(txtTextFilter);
            scFilter.Panel1.Controls.Add(dtpTimestampToFilter);
            scFilter.Panel1.Controls.Add(lblTextFilter);
            scFilter.Panel1.Controls.Add(dtpTimestampFromFilter);
            scFilter.Panel1.Controls.Add(lblTimestampToFilter);
            scFilter.Panel1.Controls.Add(lblTimestampFromFilter);
            // 
            // scFilter.Panel2
            // 
            scFilter.Panel2.Controls.Add(scList);
            scFilter.Size = new Size(1373, 750);
            scFilter.SplitterDistance = 379;
            scFilter.TabIndex = 0;
            // 
            // btnRemoveDBFilter
            // 
            btnRemoveDBFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoveDBFilter.Image = Properties.Resources.remove_24p;
            btnRemoveDBFilter.Location = new Point(326, -3);
            btnRemoveDBFilter.Name = "btnRemoveDBFilter";
            btnRemoveDBFilter.Size = new Size(32, 32);
            btnRemoveDBFilter.TabIndex = 2;
            btnRemoveDBFilter.UseVisualStyleBackColor = true;
            btnRemoveDBFilter.Click += btnRemoveDBFilter_Click;
            // 
            // btnSelectDBFilter
            // 
            btnSelectDBFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSelectDBFilter.Image = Properties.Resources.select_from_list_24p;
            btnSelectDBFilter.Location = new Point(288, -3);
            btnSelectDBFilter.Name = "btnSelectDBFilter";
            btnSelectDBFilter.Size = new Size(32, 32);
            btnSelectDBFilter.TabIndex = 2;
            btnSelectDBFilter.UseVisualStyleBackColor = true;
            btnSelectDBFilter.Click += btnSelectDB_Click;
            // 
            // txtTextFilter
            // 
            txtTextFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTextFilter.Location = new Point(143, 90);
            txtTextFilter.Name = "txtTextFilter";
            txtTextFilter.Size = new Size(227, 27);
            txtTextFilter.TabIndex = 1;
            txtTextFilter.TextChanged += txtTextFilter_TextChanged;
            // 
            // dtpTimestampToFilter
            // 
            dtpTimestampToFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpTimestampToFilter.Checked = false;
            dtpTimestampToFilter.Location = new Point(143, 57);
            dtpTimestampToFilter.Name = "dtpTimestampToFilter";
            dtpTimestampToFilter.ShowCheckBox = true;
            dtpTimestampToFilter.Size = new Size(200, 27);
            dtpTimestampToFilter.TabIndex = 1;
            dtpTimestampToFilter.Format = DateTimePickerFormat.Custom;
            dtpTimestampToFilter.ValueChanged += dtpTimestampToFilter_ValueChanged;
            // 
            // lblTextFilter
            // 
            lblTextFilter.AutoSize = true;
            lblTextFilter.Location = new Point(12, 93);
            lblTextFilter.Name = "lblTextFilter";
            lblTextFilter.Size = new Size(86, 20);
            lblTextFilter.TabIndex = 0;
            lblTextFilter.Text = "lblTextFilter";
            // 
            // dtpTimestampFromFilter
            // 
            dtpTimestampFromFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            dtpTimestampFromFilter.Checked = false;
            dtpTimestampFromFilter.Location = new Point(143, 24);
            dtpTimestampFromFilter.Name = "dtpTimestampFromFilter";
            dtpTimestampFromFilter.ShowCheckBox = true;
            dtpTimestampFromFilter.Size = new Size(200, 27);
            dtpTimestampFromFilter.TabIndex = 1;
            dtpTimestampFromFilter.Format = DateTimePickerFormat.Custom;
            dtpTimestampFromFilter.ValueChanged += dtpTimestampFromFilter_ValueChanged;
            // 
            // lblTimestampToFilter
            // 
            lblTimestampToFilter.AutoSize = true;
            lblTimestampToFilter.Location = new Point(12, 60);
            lblTimestampToFilter.Name = "lblTimestampToFilter";
            lblTimestampToFilter.Size = new Size(149, 20);
            lblTimestampToFilter.TabIndex = 0;
            lblTimestampToFilter.Text = "lblTimestampToFilter";
            // 
            // txtDBFilter
            // 
            txtDBFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDBFilter.Location = new Point(131, 0);
            txtDBFilter.Name = "txtDBFilter";
            txtDBFilter.ReadOnly = true;
            txtDBFilter.Size = new Size(151, 27);
            txtDBFilter.TabIndex = 1;
            // 
            // lblTimestampFromFilter
            // 
            lblTimestampFromFilter.AutoSize = true;
            lblTimestampFromFilter.Location = new Point(12, 27);
            lblTimestampFromFilter.Name = "lblTimestampFromFilter";
            lblTimestampFromFilter.Size = new Size(167, 20);
            lblTimestampFromFilter.TabIndex = 0;
            lblTimestampFromFilter.Text = "lblTimestampFromFilter";
            // 
            // lblDBFilter
            // 
            lblDBFilter.AutoSize = true;
            lblDBFilter.Location = new Point(0, 3);
            lblDBFilter.Name = "lblDBFilter";
            lblDBFilter.Size = new Size(79, 20);
            lblDBFilter.TabIndex = 0;
            lblDBFilter.Text = "lblDBFilter";
            // 
            // scList
            // 
            scList.Dock = DockStyle.Fill;
            scList.FixedPanel = FixedPanel.Panel1;
            scList.IsSplitterFixed = true;
            scList.Location = new Point(0, 0);
            scList.Name = "scList";
            // 
            // scList.Panel1
            // 
            scList.Panel1.Controls.Add(flpLog);
            scList.Size = new Size(990, 750);
            scList.SplitterDistance = 535;
            scList.TabIndex = 0;
            // 
            // flpLog
            // 
            flpLog.AutoScroll = true;
            flpLog.AutoSize = true;
            flpLog.BackColor = SystemColors.ControlDark;
            flpLog.Dock = DockStyle.Fill;
            flpLog.Location = new Point(0, 0);
            flpLog.Name = "flpLog";
            flpLog.Size = new Size(535, 750);
            flpLog.TabIndex = 0;
            flpLog.Scroll += flpLog_Scroll;
            flpLog.MouseWheel += flpLog_MouseWheel;
            // 
            // ts
            // 
            ts.Dock = DockStyle.None;
            ts.ImageScalingSize = new Size(30, 30);
            ts.Items.AddRange(new ToolStripItem[] { tsddbMode, tsbFilter, tsbEditReopen });
            ts.Location = new Point(4, 0);
            ts.Name = "ts";
            ts.Size = new Size(125, 37);
            ts.TabIndex = 0;
            // 
            // tsddbMode
            // 
            tsddbMode.DropDownItems.AddRange(new ToolStripItem[] { tsmiExecutionLog, tsmiClosedTabs });
            tsddbMode.Image = Properties.Resources.historial;
            tsddbMode.ImageTransparentColor = Color.Magenta;
            tsddbMode.Name = "tsddbMode";
            tsddbMode.Size = new Size(44, 34);
            // 
            // tsmiExecutionLog
            // 
            tsmiExecutionLog.Image = Properties.Resources.ejecutar;
            tsmiExecutionLog.Name = "tsmiExecutionLog";
            tsmiExecutionLog.Size = new Size(83, 26);
            tsmiExecutionLog.Click += tsmiExecutionLog_Click;
            // 
            // tsmiClosedTabs
            // 
            tsmiClosedTabs.Image = Properties.Resources.tab;
            tsmiClosedTabs.Name = "tsmiClosedTabs";
            tsmiClosedTabs.Size = new Size(83, 26);
            tsmiClosedTabs.Click += tsmiClosedTabs_Click;
            // 
            // tsbFilter
            // 
            tsbFilter.CheckOnClick = true;
            tsbFilter.Image = Properties.Resources.filter;
            tsbFilter.ImageTransparentColor = Color.Magenta;
            tsbFilter.Name = "tsbFilter";
            tsbFilter.Size = new Size(34, 34);
            tsbFilter.Click += tsbFilter_Click;
            // 
            // tsbEditReopen
            // 
            tsbEditReopen.Image = Properties.Resources.extract;
            tsbEditReopen.ImageTransparentColor = Color.Magenta;
            tsbEditReopen.Name = "tsbEditReopen";
            tsbEditReopen.Size = new Size(34, 34);
            tsbEditReopen.Click += tsbEditReopen_Click;
            // 
            // pnlDBFilter
            // 
            pnlDBFilter.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            pnlDBFilter.Controls.Add(lblDBFilter);
            pnlDBFilter.Controls.Add(btnRemoveDBFilter);
            pnlDBFilter.Controls.Add(txtDBFilter);
            pnlDBFilter.Controls.Add(btnSelectDBFilter);
            pnlDBFilter.Location = new Point(12, 123);
            pnlDBFilter.Name = "pnlDBFilter";
            pnlDBFilter.Size = new Size(358, 29);
            pnlDBFilter.TabIndex = 3;
            // 
            // tmrDelayFilterUpdate
            // 
            tmrDelayFilterUpdate.Interval = 1000;
            tmrDelayFilterUpdate.Enabled = false;
            tmrDelayFilterUpdate.Tick += tmrDelayFilterUpdate_Tick;
            // 
            // LogForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1373, 787);
            Controls.Add(tsc);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LogForm";
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            Load += LogForm_Load;
            tsc.ContentPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.PerformLayout();
            tsc.ResumeLayout(false);
            tsc.PerformLayout();
            scFilter.Panel1.ResumeLayout(false);
            scFilter.Panel1.PerformLayout();
            scFilter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scFilter).EndInit();
            scFilter.ResumeLayout(false);
            scList.Panel1.ResumeLayout(false);
            scList.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scList).EndInit();
            scList.ResumeLayout(false);
            ts.ResumeLayout(false);
            ts.PerformLayout();
            pnlDBFilter.ResumeLayout(false);
            pnlDBFilter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripContainer tsc;
        private SplitContainer scFilter;
        private SplitContainer scList;
        private FlowLayoutPanel flpLog;
        private ToolStrip ts;
        private ToolStripButton tsbFilter;
        private ToolStripButton tsbEditReopen;
        private ToolStripDropDownButton tsddbMode;
        private ToolStripMenuItem tsmiExecutionLog;
        private ToolStripMenuItem tsmiClosedTabs;
        private Button btnSelectDBFilter;
        private TextBox txtDBFilter;
        private Label lblDBFilter;
        private Button btnRemoveDBFilter;
        private DateTimePicker dtpTimestampToFilter;
        private DateTimePicker dtpTimestampFromFilter;
        private Label lblTimestampToFilter;
        private Label lblTimestampFromFilter;
        private TextBox txtTextFilter;
        private Label lblTextFilter;
        private Panel pnlDBFilter;
        private Timer tmrDelayFilterUpdate;
    }
}
using PgMulti.QueryEditor;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flpLog = new System.Windows.Forms.FlowLayoutPanel();
            this.fctbSql = new PgMulti.QueryEditor.CustomFctb();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsddbMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiExecutionLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClosedTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbEditReopen = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1000, 663);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1000, 700);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flpLog);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fctbSql);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 663);
            this.splitContainer1.SplitterDistance = 535;
            this.splitContainer1.TabIndex = 0;
            // 
            // flpLog
            // 
            this.flpLog.AutoScroll = true;
            this.flpLog.AutoSize = true;
            this.flpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpLog.Location = new System.Drawing.Point(0, 0);
            this.flpLog.Name = "flpLog";
            this.flpLog.Size = new System.Drawing.Size(535, 663);
            this.flpLog.TabIndex = 0;
            this.flpLog.Scroll += flpLog_Scroll;
            this.flpLog.MouseWheel += flpLog_MouseWheel;
            // 
            // fctbSql
            // 
            this.fctbSql.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctbSql.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(" +
    "?<range>:)\\s*(?<range>[^;]+);";
            this.fctbSql.AutoScrollMinSize = new System.Drawing.Size(71, 59);
            this.fctbSql.BackBrush = null;
            this.fctbSql.CharHeight = 19;
            this.fctbSql.CharWidth = 10;
            this.fctbSql.DefaultMarkerSize = 8;
            this.fctbSql.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctbSql.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fctbSql.Hotkeys = resources.GetString("fctbSql.Hotkeys");
            this.fctbSql.IsReplaceMode = false;
            this.fctbSql.Location = new System.Drawing.Point(0, 0);
            this.fctbSql.Name = "fctbSql";
            this.fctbSql.Paddings = new System.Windows.Forms.Padding(20);
            this.fctbSql.ReadOnly = true;
            this.fctbSql.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbSql.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbSql.ServiceColors")));
            this.fctbSql.Size = new System.Drawing.Size(461, 663);
            this.fctbSql.TabIndex = 0;
            this.fctbSql.Zoom = 100;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbMode,
            this.tsbEditReopen});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(91, 37);
            this.toolStrip1.TabIndex = 0;
            // 
            // tsddbMode
            // 
            this.tsddbMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExecutionLog,
            this.tsmiClosedTabs});
            this.tsddbMode.Image = global::PgMulti.Properties.Resources.historial;
            this.tsddbMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbMode.Name = "tsddbMode";
            this.tsddbMode.Size = new System.Drawing.Size(44, 34);
            // 
            // tsmiExecutionLog
            // 
            this.tsmiExecutionLog.Image = global::PgMulti.Properties.Resources.ejecutar;
            this.tsmiExecutionLog.Name = "tsmiExecutionLog";
            this.tsmiExecutionLog.Size = new System.Drawing.Size(234, 36);
            this.tsmiExecutionLog.Click += new System.EventHandler(this.tsmiExecutionLog_Click);
            // 
            // tsmiClosedTabs
            // 
            this.tsmiClosedTabs.Image = global::PgMulti.Properties.Resources.tab;
            this.tsmiClosedTabs.Name = "tsmiClosedTabs";
            this.tsmiClosedTabs.Size = new System.Drawing.Size(234, 36);
            this.tsmiClosedTabs.Click += new System.EventHandler(this.tsmiClosedTabs_Click);
            // 
            // tsbEditReopen
            // 
            this.tsbEditReopen.Image = global::PgMulti.Properties.Resources.extract;
            this.tsbEditReopen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditReopen.Name = "tsbEditReopen";
            this.tsbEditReopen.Size = new System.Drawing.Size(34, 34);
            this.tsbEditReopen.Click += new System.EventHandler(this.tsbEditReopen_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripContainer toolStripContainer1;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flpLog;
        private CustomFctb fctbSql;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbEditReopen;
        private ToolStripDropDownButton tsddbMode;
        private ToolStripMenuItem tsmiExecutionLog;
        private ToolStripMenuItem tsmiClosedTabs;
    }
}
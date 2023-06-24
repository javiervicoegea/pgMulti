namespace PgMulti.Forms
{
    partial class SelectTablesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTablesForm));
            this.tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            this.tvaTables = new Aga.Controls.Tree.TreeViewAdv();
            this.nsiConnections = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntbConnections = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.ncbTables = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nsiTables = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntbTables = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.tsc = new System.Windows.Forms.ToolStripContainer();
            this.sc = new System.Windows.Forms.SplitContainer();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.tsbOk = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsc.ContentPanel.SuspendLayout();
            this.tsc.TopToolStripPanel.SuspendLayout();
            this.tsc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sc)).BeginInit();
            this.sc.Panel1.SuspendLayout();
            this.sc.SuspendLayout();
            this.ts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvaConnections
            // 
            this.tvaConnections.AsyncExpanding = true;
            this.tvaConnections.AutoRowHeight = true;
            this.tvaConnections.BackColor = System.Drawing.SystemColors.Window;
            this.tvaConnections.DefaultToolTipProvider = null;
            this.tvaConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvaConnections.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvaConnections.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tvaConnections.Indent = 25;
            this.tvaConnections.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvaConnections.LoadOnDemand = false;
            this.tvaConnections.Location = new System.Drawing.Point(0, 0);
            this.tvaConnections.Margin = new System.Windows.Forms.Padding(10);
            this.tvaConnections.Model = null;
            this.tvaConnections.Name = "tvaConnections";
            this.tvaConnections.NodeControls.Add(this.nsiConnections);
            this.tvaConnections.NodeControls.Add(this.ntbConnections);
            this.tvaConnections.RowHeight = 25;
            this.tvaConnections.SelectedNode = null;
            this.tvaConnections.Size = new System.Drawing.Size(275, 498);
            this.tvaConnections.TabIndex = 0;
            this.tvaConnections.SelectionChanged += new System.EventHandler(this.tvaConnections_SelectionChanged);
            // 
            // tvaTables
            // 
            this.tvaTables.AsyncExpanding = true;
            this.tvaTables.AutoRowHeight = true;
            this.tvaTables.BackColor = System.Drawing.SystemColors.Window;
            this.tvaTables.DefaultToolTipProvider = null;
            this.tvaTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvaTables.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvaTables.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tvaTables.Indent = 25;
            this.tvaTables.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvaTables.LoadOnDemand = false;
            this.tvaTables.Location = new System.Drawing.Point(0, 0);
            this.tvaTables.Margin = new System.Windows.Forms.Padding(10);
            this.tvaTables.Model = null;
            this.tvaTables.Name = "tvaTables";
            this.tvaTables.NodeControls.Add(this.ncbTables);
            this.tvaTables.NodeControls.Add(this.nsiTables);
            this.tvaTables.NodeControls.Add(this.ntbTables);
            this.tvaTables.RowHeight = 25;
            this.tvaTables.SelectedNode = null;
            this.tvaTables.Size = new System.Drawing.Size(275, 498);
            this.tvaTables.TabIndex = 0;
            // 
            // nsiConnections
            // 
            this.nsiConnections.DataPropertyName = "Image";
            this.nsiConnections.LeftMargin = 5;
            this.nsiConnections.ParentColumn = null;
            this.nsiConnections.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntbConnections
            // 
            this.ntbConnections.DataPropertyName = "Text";
            this.ntbConnections.IncrementalSearchEnabled = true;
            this.ntbConnections.LeftMargin = 5;
            this.ntbConnections.ParentColumn = null;
            // 
            // ncbTables
            // 
            this.ncbTables.DataPropertyName = "CheckState";
            this.ncbTables.EditEnabled = true;
            this.ncbTables.ImageSize = 15;
            this.ncbTables.LeftMargin = 5;
            this.ncbTables.ParentColumn = null;
            // 
            // nsiTables
            // 
            this.nsiTables.DataPropertyName = "Image";
            this.nsiTables.LeftMargin = 5;
            this.nsiTables.ParentColumn = null;
            this.nsiTables.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntbTables
            // 
            this.ntbTables.DataPropertyName = "Text";
            this.ntbTables.IncrementalSearchEnabled = true;
            this.ntbTables.LeftMargin = 5;
            this.ntbTables.ParentColumn = null;
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            this.tsc.ContentPanel.Controls.Add(this.sc);
            this.tsc.ContentPanel.Size = new System.Drawing.Size(827, 498);
            this.tsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsc.Location = new System.Drawing.Point(0, 0);
            this.tsc.Name = "tsc";
            this.tsc.Size = new System.Drawing.Size(827, 525);
            this.tsc.TabIndex = 0;
            this.tsc.Text = "tsc";
            // 
            // tsc.TopToolStripPanel
            // 
            this.tsc.TopToolStripPanel.Controls.Add(this.ts);
            // 
            // sc
            // 
            this.sc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc.Location = new System.Drawing.Point(0, 0);
            this.sc.Name = "sc";
            // 
            // sc.Panel1
            // 
            this.sc.Panel1.Controls.Add(this.tvaConnections);
            this.sc.Size = new System.Drawing.Size(827, 498);
            this.sc.SplitterDistance = 275;
            this.sc.TabIndex = 1;
            // 
            // sc.Panel2
            // 
            this.sc.Panel2.Controls.Add(this.tvaTables);
            this.sc.Size = new System.Drawing.Size(827, 498);
            this.sc.SplitterDistance = 275;
            this.sc.TabIndex = 2;
            // 
            // ts
            // 
            this.ts.Dock = System.Windows.Forms.DockStyle.None;
            this.ts.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOk,
            this.tsbCancel});
            this.ts.Location = new System.Drawing.Point(4, 0);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(71, 27);
            this.ts.TabIndex = 0;
            // 
            // tsbOk
            // 
            this.tsbOk.Image = global::PgMulti.Properties.Resources.ok;
            this.tsbOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOk.Name = "tsbOk";
            this.tsbOk.Size = new System.Drawing.Size(29, 24);
            this.tsbOk.Click += new System.EventHandler(this.tsbOk_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Image = global::PgMulti.Properties.Resources.cerrar;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(29, 24);
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // SelectTablesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 525);
            this.Controls.Add(this.tsc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectTablesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.SelectTablesForm_Load);
            this.tsc.ContentPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.PerformLayout();
            this.tsc.ResumeLayout(false);
            this.tsc.PerformLayout();
            this.sc.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sc)).EndInit();
            this.sc.ResumeLayout(false);
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Aga.Controls.Tree.TreeViewAdv tvaTables;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiConnections;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbConnections;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox ncbTables;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiTables;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbTables;
        private ToolStripContainer tsc;
        private ToolStrip ts;
        private ToolStripButton tsbOk;
        private ToolStripButton tsbCancel;
        private SplitContainer sc;
    }
}
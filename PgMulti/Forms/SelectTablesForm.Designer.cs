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
            tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            nsiConnections = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            ntbConnections = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            tvaTables = new Aga.Controls.Tree.TreeViewAdv();
            ncbTables = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            nsiTables = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            ntbTables = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            sc = new SplitContainer();
            btnCancel = new Button();
            btnOk = new Button();
            ((System.ComponentModel.ISupportInitialize)sc).BeginInit();
            sc.Panel1.SuspendLayout();
            sc.Panel2.SuspendLayout();
            sc.SuspendLayout();
            SuspendLayout();
            // 
            // tvaConnections
            // 
            tvaConnections.AsyncExpanding = true;
            tvaConnections.AutoRowHeight = true;
            tvaConnections.BackColor = SystemColors.Window;
            tvaConnections.DefaultToolTipProvider = null;
            tvaConnections.Dock = DockStyle.Fill;
            tvaConnections.DragDropMarkColor = Color.Black;
            tvaConnections.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tvaConnections.Indent = 25;
            tvaConnections.LineColor = SystemColors.ControlDark;
            tvaConnections.Location = new Point(0, 0);
            tvaConnections.Margin = new Padding(10);
            tvaConnections.Model = null;
            tvaConnections.Name = "tvaConnections";
            tvaConnections.NodeControls.Add(nsiConnections);
            tvaConnections.NodeControls.Add(ntbConnections);
            tvaConnections.RowHeight = 25;
            tvaConnections.SelectedNode = null;
            tvaConnections.Size = new Size(274, 528);
            tvaConnections.TabIndex = 0;
            tvaConnections.SelectionChanged += tvaConnections_SelectionChanged;
            // 
            // nsiConnections
            // 
            nsiConnections.DataPropertyName = "Image";
            nsiConnections.LeftMargin = 5;
            nsiConnections.ParentColumn = null;
            nsiConnections.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntbConnections
            // 
            ntbConnections.DataPropertyName = "Text";
            ntbConnections.IncrementalSearchEnabled = true;
            ntbConnections.LeftMargin = 5;
            ntbConnections.ParentColumn = null;
            // 
            // tvaTables
            // 
            tvaTables.AsyncExpanding = true;
            tvaTables.AutoRowHeight = true;
            tvaTables.BackColor = SystemColors.Window;
            tvaTables.DefaultToolTipProvider = null;
            tvaTables.Dock = DockStyle.Fill;
            tvaTables.DragDropMarkColor = Color.Black;
            tvaTables.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tvaTables.Indent = 25;
            tvaTables.LineColor = SystemColors.ControlDark;
            tvaTables.Location = new Point(0, 0);
            tvaTables.Margin = new Padding(10);
            tvaTables.Model = null;
            tvaTables.Name = "tvaTables";
            tvaTables.NodeControls.Add(ncbTables);
            tvaTables.NodeControls.Add(nsiTables);
            tvaTables.NodeControls.Add(ntbTables);
            tvaTables.RowHeight = 25;
            tvaTables.SelectedNode = null;
            tvaTables.Size = new Size(547, 528);
            tvaTables.TabIndex = 0;
            // 
            // ncbTables
            // 
            ncbTables.DataPropertyName = "CheckState";
            ncbTables.EditEnabled = true;
            ncbTables.ImageSize = 20;
            ncbTables.LeftMargin = 5;
            ncbTables.ParentColumn = null;
            // 
            // nsiTables
            // 
            nsiTables.DataPropertyName = "Image";
            nsiTables.LeftMargin = 5;
            nsiTables.ParentColumn = null;
            nsiTables.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntbTables
            // 
            ntbTables.DataPropertyName = "Text";
            ntbTables.IncrementalSearchEnabled = true;
            ntbTables.LeftMargin = 5;
            ntbTables.ParentColumn = null;
            // 
            // sc
            // 
            sc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            sc.Location = new Point(12, 12);
            sc.Name = "sc";
            // 
            // sc.Panel1
            // 
            sc.Panel1.Controls.Add(tvaConnections);
            // 
            // sc.Panel2
            // 
            sc.Panel2.Controls.Add(tvaTables);
            sc.Size = new Size(825, 528);
            sc.SplitterDistance = 274;
            sc.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(728, 546);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(109, 29);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(613, 546);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(109, 29);
            btnOk.TabIndex = 6;
            btnOk.Text = "btnOk";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // SelectTablesForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(849, 587);
            Controls.Add(sc);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SelectTablesForm";
            StartPosition = FormStartPosition.CenterParent;
            Load += SelectTablesForm_Load;
            sc.Panel1.ResumeLayout(false);
            sc.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sc).EndInit();
            sc.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Aga.Controls.Tree.TreeViewAdv tvaTables;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiConnections;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbConnections;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox ncbTables;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiTables;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbTables;
        private SplitContainer sc;
        private Button btnCancel;
        private Button btnOk;
    }
}
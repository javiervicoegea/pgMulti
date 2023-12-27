namespace PgMulti.Forms
{
    partial class InsertIntoTableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertIntoTableForm));
            flp = new FlowLayoutPanel();
            btnCancel = new Button();
            btnOk = new Button();
            splitContainer1 = new SplitContainer();
            tvaTables = new Aga.Controls.Tree.TreeViewAdv();
            nsiTables = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            ntbTables = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            flp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // flp
            // 
            flp.Controls.Add(btnCancel);
            flp.Controls.Add(btnOk);
            flp.Dock = DockStyle.Bottom;
            flp.FlowDirection = FlowDirection.RightToLeft;
            flp.Location = new Point(0, 612);
            flp.Name = "flp";
            flp.Padding = new Padding(5);
            flp.Size = new Size(1512, 61);
            flp.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(1392, 15);
            btnCancel.Margin = new Padding(10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 29);
            btnCancel.TabIndex = 1;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(1272, 15);
            btnOk.Margin = new Padding(10);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 29);
            btnOk.TabIndex = 0;
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tvaTables);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Size = new Size(1512, 612);
            splitContainer1.SplitterDistance = 502;
            splitContainer1.TabIndex = 2;
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
            tvaTables.NodeControls.Add(nsiTables);
            tvaTables.NodeControls.Add(ntbTables);
            tvaTables.RowHeight = 25;
            tvaTables.SelectedNode = null;
            tvaTables.Size = new Size(502, 612);
            tvaTables.TabIndex = 1;
            tvaTables.SelectionChanged += tvaTables_SelectionChanged;
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
            // InsertIntoTableForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1512, 673);
            Controls.Add(splitContainer1);
            Controls.Add(flp);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "InsertIntoTableForm";
            StartPosition = FormStartPosition.CenterParent;
            FormClosed += InsertIntoTableForm_FormClosed;
            Load += InsertIntoTableForm_Load;
            flp.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flp;
        private Button btnCancel;
        private Button btnOk;
        private SplitContainer splitContainer1;
        private Aga.Controls.Tree.TreeViewAdv tvaTables;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiTables;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbTables;
    }
}
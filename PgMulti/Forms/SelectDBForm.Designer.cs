namespace PgMulti.Forms
{
    partial class SelectDBForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectDBForm));
            tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            nsiConnections = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            ntbConnections = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // tvaConnections
            // 
            tvaConnections.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvaConnections.AsyncExpanding = true;
            tvaConnections.AutoRowHeight = true;
            tvaConnections.BackColor = SystemColors.Window;
            tvaConnections.DefaultToolTipProvider = null;
            tvaConnections.DragDropMarkColor = Color.Black;
            tvaConnections.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tvaConnections.Indent = 25;
            tvaConnections.LineColor = SystemColors.ControlDark;
            tvaConnections.Location = new Point(19, 19);
            tvaConnections.Margin = new Padding(10);
            tvaConnections.Model = null;
            tvaConnections.Name = "tvaConnections";
            tvaConnections.NodeControls.Add(nsiConnections);
            tvaConnections.NodeControls.Add(ntbConnections);
            tvaConnections.RowHeight = 25;
            tvaConnections.SelectedNode = null;
            tvaConnections.Size = new Size(773, 488);
            tvaConnections.TabIndex = 1;
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
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(568, 520);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(109, 29);
            btnOk.TabIndex = 2;
            btnOk.Text = "btnOk";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(683, 520);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(109, 29);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SelectDBForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(811, 561);
            Controls.Add(tvaConnections);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SelectDBForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SelectDBForm";
            Load += SelectDBForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiConnections;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbConnections;
        private Button btnOk;
        private Button btnCancel;
    }
}
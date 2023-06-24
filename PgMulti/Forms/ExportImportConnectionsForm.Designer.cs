namespace PgMulti.Forms
{
    partial class ExportImportConnectionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportImportConnectionsForm));
            this.ncb = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nsi = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntb = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlConnections = new System.Windows.Forms.Panel();
            this.chkEncryptPasswords = new System.Windows.Forms.CheckBox();
            this.pnlPassword2 = new System.Windows.Forms.Panel();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.lblPassword2 = new System.Windows.Forms.Label();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.lblPassword1 = new System.Windows.Forms.Label();
            this.pnlPassword1 = new System.Windows.Forms.Panel();
            this.chkExportPasswords = new System.Windows.Forms.CheckBox();
            this.sfdExportConfig = new System.Windows.Forms.SaveFileDialog();
            this.pnlConnections.SuspendLayout();
            this.pnlPassword2.SuspendLayout();
            this.pnlPassword1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ncb
            // 
            this.ncb.DataPropertyName = "CheckState";
            this.ncb.EditEnabled = true;
            this.ncb.ImageSize = 15;
            this.ncb.LeftMargin = 5;
            this.ncb.ParentColumn = null;
            // 
            // nsi
            // 
            this.nsi.DataPropertyName = "Image";
            this.nsi.LeftMargin = 5;
            this.nsi.ParentColumn = null;
            this.nsi.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntb
            // 
            this.ntb.DataPropertyName = "Text";
            this.ntb.IncrementalSearchEnabled = true;
            this.ntb.LeftMargin = 5;
            this.ntb.ParentColumn = null;
            // 
            // tvaConnections
            // 
            this.tvaConnections.AutoRowHeight = true;
            this.tvaConnections.BackColor = System.Drawing.SystemColors.Window;
            this.tvaConnections.DefaultToolTipProvider = null;
            this.tvaConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvaConnections.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvaConnections.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tvaConnections.Indent = 25;
            this.tvaConnections.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvaConnections.Location = new System.Drawing.Point(0, 0);
            this.tvaConnections.Margin = new System.Windows.Forms.Padding(10);
            this.tvaConnections.Model = null;
            this.tvaConnections.Name = "tvaConnections";
            this.tvaConnections.NodeControls.Add(this.ncb);
            this.tvaConnections.NodeControls.Add(this.nsi);
            this.tvaConnections.NodeControls.Add(this.ntb);
            this.tvaConnections.RowHeight = 25;
            this.tvaConnections.SelectedNode = null;
            this.tvaConnections.Size = new System.Drawing.Size(382, 426);
            this.tvaConnections.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(551, 463);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 29);
            this.btnOk.TabIndex = 2;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(678, 463);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 29);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlConnections
            // 
            this.pnlConnections.Controls.Add(this.tvaConnections);
            this.pnlConnections.Location = new System.Drawing.Point(12, 12);
            this.pnlConnections.Name = "pnlConnections";
            this.pnlConnections.Size = new System.Drawing.Size(382, 426);
            this.pnlConnections.TabIndex = 2;
            // 
            // chkEncryptPasswords
            // 
            this.chkEncryptPasswords.AutoSize = true;
            this.chkEncryptPasswords.Location = new System.Drawing.Point(420, 42);
            this.chkEncryptPasswords.Name = "chkEncryptPasswords";
            this.chkEncryptPasswords.Size = new System.Drawing.Size(169, 24);
            this.chkEncryptPasswords.TabIndex = 1;
            this.chkEncryptPasswords.Text = "chkEncryptPasswords";
            this.chkEncryptPasswords.UseVisualStyleBackColor = true;
            this.chkEncryptPasswords.Visible = false;
            this.chkEncryptPasswords.CheckedChanged += new System.EventHandler(this.chkEncryptPasswords_CheckedChanged);
            // 
            // pnlPassword2
            // 
            this.pnlPassword2.Controls.Add(this.txtPassword2);
            this.pnlPassword2.Controls.Add(this.lblPassword2);
            this.pnlPassword2.Location = new System.Drawing.Point(407, 162);
            this.pnlPassword2.Name = "pnlPassword2";
            this.pnlPassword2.Size = new System.Drawing.Size(381, 62);
            this.pnlPassword2.TabIndex = 4;
            this.pnlPassword2.Visible = false;
            // 
            // txtPassword2
            // 
            this.txtPassword2.Location = new System.Drawing.Point(13, 29);
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.Size = new System.Drawing.Size(365, 27);
            this.txtPassword2.TabIndex = 0;
            this.txtPassword2.UseSystemPasswordChar = true;
            // 
            // lblPassword2
            // 
            this.lblPassword2.AutoSize = true;
            this.lblPassword2.Location = new System.Drawing.Point(13, 6);
            this.lblPassword2.Name = "lblPassword2";
            this.lblPassword2.Size = new System.Drawing.Size(95, 20);
            this.lblPassword2.TabIndex = 6;
            this.lblPassword2.Text = "lblPassword2";
            // 
            // txtPassword1
            // 
            this.txtPassword1.Location = new System.Drawing.Point(13, 29);
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.Size = new System.Drawing.Size(365, 27);
            this.txtPassword1.TabIndex = 0;
            this.txtPassword1.UseSystemPasswordChar = true;
            // 
            // lblPassword1
            // 
            this.lblPassword1.AutoSize = true;
            this.lblPassword1.Location = new System.Drawing.Point(13, 6);
            this.lblPassword1.Name = "lblPassword1";
            this.lblPassword1.Size = new System.Drawing.Size(95, 20);
            this.lblPassword1.TabIndex = 7;
            this.lblPassword1.Text = "lblPassword1";
            // 
            // pnlPassword1
            // 
            this.pnlPassword1.Controls.Add(this.lblPassword1);
            this.pnlPassword1.Controls.Add(this.txtPassword1);
            this.pnlPassword1.Location = new System.Drawing.Point(407, 94);
            this.pnlPassword1.Name = "pnlPassword1";
            this.pnlPassword1.Size = new System.Drawing.Size(381, 62);
            this.pnlPassword1.TabIndex = 5;
            this.pnlPassword1.Visible = false;
            // 
            // chkExportPasswords
            // 
            this.chkExportPasswords.AutoSize = true;
            this.chkExportPasswords.Location = new System.Drawing.Point(420, 12);
            this.chkExportPasswords.Name = "chkExportPasswords";
            this.chkExportPasswords.Size = new System.Drawing.Size(163, 24);
            this.chkExportPasswords.TabIndex = 0;
            this.chkExportPasswords.Text = "chkExportPasswords";
            this.chkExportPasswords.UseVisualStyleBackColor = true;
            this.chkExportPasswords.Visible = false;
            this.chkExportPasswords.CheckedChanged += new System.EventHandler(this.chkExportPasswords_CheckedChanged);
            // 
            // sfdExportConfig
            // 
            this.sfdExportConfig.DefaultExt = "pgmc";
            this.sfdExportConfig.FilterIndex = 0;
            // 
            // ExportImportConnectionsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 504);
            this.Controls.Add(this.pnlPassword1);
            this.Controls.Add(this.pnlPassword2);
            this.Controls.Add(this.chkExportPasswords);
            this.Controls.Add(this.chkEncryptPasswords);
            this.Controls.Add(this.pnlConnections);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportImportConnectionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.ExportImportConnectionsForm_Load);
            this.pnlConnections.ResumeLayout(false);
            this.pnlPassword2.ResumeLayout(false);
            this.pnlPassword2.PerformLayout();
            this.pnlPassword1.ResumeLayout(false);
            this.pnlPassword1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Aga.Controls.Tree.NodeControls.NodeCheckBox ncb;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsi;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntb;
        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Button btnOk;
        private Button btnCancel;
        private Panel pnlConnections;
        private CheckBox chkEncryptPasswords;
        private Panel pnlPassword2;
        private TextBox txtPassword2;
        private Label lblPassword2;
        private TextBox txtPassword1;
        private Label lblPassword1;
        private Panel pnlPassword1;
        private CheckBox chkExportPasswords;
        private SaveFileDialog sfdExportConfig;
    }
}
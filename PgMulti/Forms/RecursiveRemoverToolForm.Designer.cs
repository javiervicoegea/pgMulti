namespace PgMulti.Forms
{
    partial class RecursiveRemoverToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecursiveRemoverToolForm));
            btnOk = new Button();
            btnCancel = new Button();
            lblRootTableName = new Label();
            txtRootTableName = new TextBox();
            lblDeleteTuplesWhereClause = new Label();
            txtDeleteTuplesWhereClause = new TextBox();
            lblPreserveTuplesWhereClause = new Label();
            txtPreserveTuplesWhereClause = new TextBox();
            lblSchemaName = new Label();
            txtSchemaName = new TextBox();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Location = new Point(465, 174);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(132, 29);
            btnOk.TabIndex = 3;
            btnOk.Text = "x";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(603, 174);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(132, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "x";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblRootTableName
            // 
            lblRootTableName.AutoSize = true;
            lblRootTableName.Location = new Point(37, 23);
            lblRootTableName.Name = "lblRootTableName";
            lblRootTableName.Size = new Size(16, 20);
            lblRootTableName.TabIndex = 2;
            lblRootTableName.Text = "x";
            // 
            // txtRootTableName
            // 
            txtRootTableName.Location = new Point(351, 20);
            txtRootTableName.Name = "txtRootTableName";
            txtRootTableName.ReadOnly = true;
            txtRootTableName.Size = new Size(384, 27);
            txtRootTableName.TabIndex = 5;
            // 
            // lblDeleteTuplesWhereClause
            // 
            lblDeleteTuplesWhereClause.AutoSize = true;
            lblDeleteTuplesWhereClause.Location = new Point(37, 56);
            lblDeleteTuplesWhereClause.Name = "lblDeleteTuplesWhereClause";
            lblDeleteTuplesWhereClause.Size = new Size(16, 20);
            lblDeleteTuplesWhereClause.TabIndex = 2;
            lblDeleteTuplesWhereClause.Text = "x";
            // 
            // txtDeleteTuplesWhereClause
            // 
            txtDeleteTuplesWhereClause.Location = new Point(351, 53);
            txtDeleteTuplesWhereClause.Name = "txtDeleteTuplesWhereClause";
            txtDeleteTuplesWhereClause.Size = new Size(384, 27);
            txtDeleteTuplesWhereClause.TabIndex = 0;
            // 
            // lblPreserveTuplesWhereClause
            // 
            lblPreserveTuplesWhereClause.AutoSize = true;
            lblPreserveTuplesWhereClause.Location = new Point(37, 89);
            lblPreserveTuplesWhereClause.Name = "lblPreserveTuplesWhereClause";
            lblPreserveTuplesWhereClause.Size = new Size(16, 20);
            lblPreserveTuplesWhereClause.TabIndex = 2;
            lblPreserveTuplesWhereClause.Text = "x";
            // 
            // txtPreserveTuplesWhereClause
            // 
            txtPreserveTuplesWhereClause.Location = new Point(351, 86);
            txtPreserveTuplesWhereClause.Name = "txtPreserveTuplesWhereClause";
            txtPreserveTuplesWhereClause.Size = new Size(384, 27);
            txtPreserveTuplesWhereClause.TabIndex = 1;
            // 
            // lblSchemaName
            // 
            lblSchemaName.AutoSize = true;
            lblSchemaName.Location = new Point(37, 122);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new Size(16, 20);
            lblSchemaName.TabIndex = 2;
            lblSchemaName.Text = "x";
            // 
            // txtSchemaName
            // 
            txtSchemaName.Location = new Point(351, 119);
            txtSchemaName.Name = "txtSchemaName";
            txtSchemaName.Size = new Size(384, 27);
            txtSchemaName.TabIndex = 2;
            // 
            // RecursiveRemoverToolForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(752, 225);
            Controls.Add(txtSchemaName);
            Controls.Add(lblSchemaName);
            Controls.Add(txtPreserveTuplesWhereClause);
            Controls.Add(lblPreserveTuplesWhereClause);
            Controls.Add(txtDeleteTuplesWhereClause);
            Controls.Add(lblDeleteTuplesWhereClause);
            Controls.Add(txtRootTableName);
            Controls.Add(lblRootTableName);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "RecursiveRemoverToolForm";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblRootTableName;
        private TextBox txtRootTableName;
        private Label lblDeleteTuplesWhereClause;
        private TextBox txtDeleteTuplesWhereClause;
        private Label lblPreserveTuplesWhereClause;
        private TextBox txtPreserveTuplesWhereClause;
        private Label lblSchemaName;
        private TextBox txtSchemaName;
    }
}
namespace PgMulti
{
    partial class EditDBForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDBForm));
            this.lblDBAlias = new System.Windows.Forms.Label();
            this.txtDBAlias = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDbName = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDBAlias
            // 
            this.lblDBAlias.AutoSize = true;
            this.lblDBAlias.Location = new System.Drawing.Point(18, 24);
            this.lblDBAlias.Name = "lblDBAlias";
            this.lblDBAlias.Size = new System.Drawing.Size(0, 20);
            this.lblDBAlias.TabIndex = 0;
            // 
            // txtDBAlias
            // 
            this.txtDBAlias.Location = new System.Drawing.Point(129, 21);
            this.txtDBAlias.MaxLength = 200;
            this.txtDBAlias.Name = "txtDBAlias";
            this.txtDBAlias.Size = new System.Drawing.Size(395, 27);
            this.txtDBAlias.TabIndex = 0;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(18, 57);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(0, 20);
            this.lblServer.TabIndex = 0;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(129, 54);
            this.txtServer.MaxLength = 200;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(395, 27);
            this.txtServer.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 0;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(129, 87);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(58, 27);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "5432";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(18, 156);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(0, 20);
            this.lblUser.TabIndex = 0;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(129, 153);
            this.txtUser.MaxLength = 200;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(181, 27);
            this.txtUser.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(18, 189);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(0, 20);
            this.lblPassword.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(129, 186);
            this.txtPassword.MaxLength = 200;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(181, 27);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(230, 248);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(94, 29);
            this.btnTest.TabIndex = 6;
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(330, 248);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 29);
            this.btnOk.TabIndex = 7;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(430, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDbName
            // 
            this.lblDbName.AutoSize = true;
            this.lblDbName.Location = new System.Drawing.Point(18, 123);
            this.lblDbName.Name = "lblDbName";
            this.lblDbName.Size = new System.Drawing.Size(0, 20);
            this.lblDbName.TabIndex = 0;
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(129, 120);
            this.txtDBName.MaxLength = 200;
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(395, 27);
            this.txtDBName.TabIndex = 3;
            // 
            // EditDBForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 300);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.lblDbName);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtDBAlias);
            this.Controls.Add(this.lblDBAlias);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditDBForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmEditDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblDBAlias;
        private TextBox txtDBAlias;
        private Label lblServer;
        private TextBox txtServer;
        private Label label3;
        private TextBox txtPort;
        private Label lblUser;
        private TextBox txtUser;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnTest;
        private Button btnOk;
        private Button btnCancel;
        private Label lblDbName;
        private TextBox txtDBName;
    }
}
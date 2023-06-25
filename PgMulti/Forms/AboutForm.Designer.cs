namespace PgMulti
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblAttributions = new System.Windows.Forms.Label();
            this.txtAttributions = new System.Windows.Forms.TextBox();
            this.llUrl = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(11, 235);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(664, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "v";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Location = new System.Drawing.Point(12, 273);
            this.lblAuthor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(663, 26);
            this.lblAuthor.TabIndex = 1;
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAttributions
            // 
            this.lblAttributions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAttributions.Location = new System.Drawing.Point(11, 350);
            this.lblAttributions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAttributions.Name = "lblAttributions";
            this.lblAttributions.Size = new System.Drawing.Size(664, 30);
            this.lblAttributions.TabIndex = 0;
            this.lblAttributions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAttributions
            // 
            this.txtAttributions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.txtAttributions.Location = new System.Drawing.Point(11, 394);
            this.txtAttributions.Margin = new System.Windows.Forms.Padding(2);
            this.txtAttributions.Multiline = true;
            this.txtAttributions.Name = "txtAttributions";
            this.txtAttributions.ReadOnly = true;
            this.txtAttributions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAttributions.Size = new System.Drawing.Size(664, 220);
            this.txtAttributions.TabIndex = 2;
            this.txtAttributions.Text = resources.GetString("txtAttributions.Text");
            // 
            // llUrl
            // 
            this.llUrl.Location = new System.Drawing.Point(12, 299);
            this.llUrl.Name = "llUrl";
            this.llUrl.Size = new System.Drawing.Size(663, 51);
            this.llUrl.TabIndex = 3;
            this.llUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llUrl_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PgMulti.Properties.Resources.logo_completo;
            this.pictureBox1.Location = new System.Drawing.Point(232, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(205, 205);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 625);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.llUrl);
            this.Controls.Add(this.txtAttributions);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblAttributions);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmAcercaDe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTitle;
        private Label lblAuthor;
        private Label lblAttributions;
        private TextBox txtAttributions;
        private LinkLabel llUrl;
        private PictureBox pictureBox1;
    }
}
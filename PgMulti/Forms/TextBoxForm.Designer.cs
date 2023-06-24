namespace PgMulti
{
    partial class TextBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextBoxForm));
            this.txtText = new System.Windows.Forms.TextBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.tsbNull = new System.Windows.Forms.ToolStripButton();
            this.tsbOk = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tsToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Location = new System.Drawing.Point(0, 0);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ReadOnly = true;
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtText.Size = new System.Drawing.Size(815, 459);
            this.txtText.TabIndex = 0;
            this.txtText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtText_KeyUp);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtText);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(815, 459);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(815, 486);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsToolbar);
            // 
            // tsToolbar
            // 
            this.tsToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.tsToolbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNull,
            this.tsbOk,
            this.tsbCancel});
            this.tsToolbar.Location = new System.Drawing.Point(4, 0);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.Size = new System.Drawing.Size(100, 27);
            this.tsToolbar.TabIndex = 0;
            // 
            // tsbNull
            // 
            this.tsbNull.CheckOnClick = true;
            this.tsbNull.Image = global::PgMulti.Properties.Resources._null;
            this.tsbNull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNull.Name = "tsbNull";
            this.tsbNull.Size = new System.Drawing.Size(29, 24);
            this.tsbNull.Click += new System.EventHandler(this.tsbNull_Click);
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
            // TextBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 486);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TextBoxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.TextBoxForm_Shown);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox txtText;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip tsToolbar;
        private ToolStripButton tsbNull;
        private ToolStripButton tsbOk;
        private ToolStripButton tsbCancel;
    }
}
namespace SampleApp
{
    partial class Test
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
            this.tvaServidores = new Aga.Controls.Tree.TreeViewAdv();
            this.ncb = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nsi = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntb = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tvaServidores
            // 
            this.tvaServidores.AutoRowHeight = true;
            this.tvaServidores.BackColor = System.Drawing.SystemColors.Window;
            this.tvaServidores.DefaultToolTipProvider = null;
            this.tvaServidores.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvaServidores.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.tvaServidores.Indent = 25;
            this.tvaServidores.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvaServidores.Location = new System.Drawing.Point(0, 0);
            this.tvaServidores.Margin = new System.Windows.Forms.Padding(10);
            this.tvaServidores.Model = null;
            this.tvaServidores.Name = "tvaServidores";
            this.tvaServidores.NodeControls.Add(this.ncb);
            this.tvaServidores.NodeControls.Add(this.nsi);
            this.tvaServidores.NodeControls.Add(this.ntb);
            this.tvaServidores.RowHeight = 25;
            this.tvaServidores.SelectedNode = null;
            this.tvaServidores.Size = new System.Drawing.Size(457, 431);
            this.tvaServidores.TabIndex = 0;
            // 
            // ncb
            // 
            this.ncb.DataPropertyName = "CheckState";
            this.ncb.EditEnabled = true;
            this.ncb.LeftMargin = 0;
            this.ncb.ParentColumn = null;
            // 
            // nsi
            // 
            this.nsi.DataPropertyName = "Icon";
            this.nsi.LeftMargin = 1;
            this.nsi.ParentColumn = null;
            this.nsi.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // ntb
            // 
            this.ntb.DataPropertyName = "Text";
            this.ntb.EditEnabled = true;
            this.ntb.IncrementalSearchEnabled = true;
            this.ntb.LeftMargin = 3;
            this.ntb.ParentColumn = null;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(647, 107);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(597, 175);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(566, 248);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tvaServidores);
            this.Name = "Test";
            this.Text = "Test";
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv tvaServidores;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox ncb;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsi;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}
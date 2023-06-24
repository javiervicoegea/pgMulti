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
            this.tsc = new System.Windows.Forms.ToolStripContainer();
            this.tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            this.nsiConnections = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntbConnections = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.tsbOk = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsc.ContentPanel.SuspendLayout();
            this.tsc.TopToolStripPanel.SuspendLayout();
            this.tsc.SuspendLayout();
            this.ts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            this.tsc.ContentPanel.Controls.Add(this.tvaConnections);
            this.tsc.ContentPanel.Size = new System.Drawing.Size(800, 423);
            this.tsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsc.Location = new System.Drawing.Point(0, 0);
            this.tsc.Name = "tsc";
            this.tsc.Size = new System.Drawing.Size(800, 450);
            this.tsc.TabIndex = 0;
            this.tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            this.tsc.TopToolStripPanel.Controls.Add(this.ts);
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
            this.tvaConnections.Location = new System.Drawing.Point(0, 0);
            this.tvaConnections.Margin = new System.Windows.Forms.Padding(10);
            this.tvaConnections.Model = null;
            this.tvaConnections.Name = "tvaConnections";
            this.tvaConnections.NodeControls.Add(this.nsiConnections);
            this.tvaConnections.NodeControls.Add(this.ntbConnections);
            this.tvaConnections.RowHeight = 25;
            this.tvaConnections.SelectedNode = null;
            this.tvaConnections.Size = new System.Drawing.Size(800, 423);
            this.tvaConnections.TabIndex = 1;
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
            this.ts.TabIndex = 1;
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
            // SelectDBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tsc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectDBForm";
            this.Text = "SelectDBForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.SelectDBForm_Load);
            this.tsc.ContentPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.PerformLayout();
            this.tsc.ResumeLayout(false);
            this.tsc.PerformLayout();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripContainer tsc;
        private ToolStrip ts;
        private ToolStripButton tsbOk;
        private ToolStripButton tsbCancel;
        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsiConnections;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntbConnections;
    }
}
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PgMulti.Forms
{
    partial class OrderedDiagramColumnSelectorControl
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            sc = new SplitContainer();
            tsc = new System.Windows.Forms.ToolStripContainer();
            ts = new System.Windows.Forms.ToolStrip();
            tsbUp = new System.Windows.Forms.ToolStripButton();
            tsbDown = new System.Windows.Forms.ToolStripButton();
            tsbLeft = new System.Windows.Forms.ToolStripButton();
            tsbRight = new System.Windows.Forms.ToolStripButton();
            lbCurrentColumns = new ListBox();
            lbAvailableColumns = new ListBox();
            lblCurrentColumns = new Label();
            lblAvailableColumns = new Label();
            ((System.ComponentModel.ISupportInitialize)sc).BeginInit();
            sc.Panel1.SuspendLayout();
            sc.Panel2.SuspendLayout();
            sc.SuspendLayout();
            tsc.ContentPanel.SuspendLayout();
            tsc.TopToolStripPanel.SuspendLayout();
            tsc.SuspendLayout();
            ts.SuspendLayout();
            SuspendLayout();
            // 
            // sc
            // 
            sc.Dock = DockStyle.Fill;
            sc.Name = "sc";
            sc.TabIndex = 0;
            // 
            // sc.Panel1
            // 
            sc.Panel1.Controls.Add(tsc);
            sc.Panel1MinSize = 200;
            // 
            // sc.Panel2
            // 
            sc.Panel2.AutoScroll = true;
            sc.Panel2.Controls.Add(lbAvailableColumns);
            sc.Panel2.Controls.Add(lblAvailableColumns);
            sc.Panel2MinSize = 100;
            sc.TabIndex = 0;
            sc.Orientation = Orientation.Vertical;
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            tsc.ContentPanel.Controls.Add(this.lbCurrentColumns);
            tsc.ContentPanel.Controls.Add(this.lblCurrentColumns);
            tsc.ContentPanel.Size = new System.Drawing.Size(354, 323);
            tsc.Dock = System.Windows.Forms.DockStyle.Fill;
            tsc.Location = new System.Drawing.Point(0, 0);
            tsc.Name = "tsc";
            tsc.Size = new System.Drawing.Size(354, 350);
            tsc.TabIndex = 0;
            // 
            // tscColumns.TopToolStripPanel
            // 
            tsc.RightToolStripPanel.Controls.Add(ts);
            // 
            // ts
            // 
            ts.Dock = System.Windows.Forms.DockStyle.None;
            ts.ImageScalingSize = new System.Drawing.Size(20, 20);
            ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbUp,
            this.tsbDown,
            this.tsbLeft,
            this.tsbRight});
            ts.Location = new System.Drawing.Point(4, 0);
            ts.Name = "ts";
            ts.Size = new System.Drawing.Size(71, 27);
            ts.TabIndex = 0;
            // 
            // tsbUp
            // 
            this.tsbUp.Image = global::PgMulti.Properties.Resources.up;
            this.tsbUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUp.Name = "tsbUp";
            this.tsbUp.Size = new System.Drawing.Size(29, 24);
            this.tsbUp.Click += tsbUp_Click;
            // 
            // tsbDown
            // 
            this.tsbDown.Image = global::PgMulti.Properties.Resources.down;
            this.tsbDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDown.Name = "tsbDown";
            this.tsbDown.Size = new System.Drawing.Size(29, 24);
            this.tsbDown.Click += tsbDown_Click;
            // 
            // tsbLeft
            // 
            this.tsbLeft.Image = global::PgMulti.Properties.Resources.left;
            this.tsbLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLeft.Name = "tsbLeft";
            this.tsbLeft.Size = new System.Drawing.Size(29, 24);
            this.tsbLeft.Click += tsbLeft_Click;
            // 
            // tsbRight
            // 
            this.tsbRight.Image = global::PgMulti.Properties.Resources.right;
            this.tsbRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRight.Name = "tsbRight";
            this.tsbRight.Size = new System.Drawing.Size(29, 24);
            this.tsbRight.Click += tsbRight_Click;
            //
            // lbCurrentFields
            //
            lbCurrentColumns.Dock = DockStyle.Fill;
            lbCurrentColumns.Name = "lbCurrentColumns";
            lbCurrentColumns.SelectionMode = SelectionMode.One;
            lbCurrentColumns.SelectedIndexChanged += lbCurrentFields_SelectedIndexChanged;
            lbCurrentColumns.DoubleClick += lbCurrentColumns_DoubleClick;
            // 
            // lblCurrentFields
            // 
            this.lblCurrentColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCurrentColumns.AutoSize = true;
            this.lblCurrentColumns.Dock = DockStyle.Top;
            this.lblCurrentColumns.Name = "lblCurrentColumns";
            this.lblCurrentColumns.TabIndex = 0;
            this.lblCurrentColumns.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            //
            // lbAvailableFields
            //
            lbAvailableColumns.Dock = DockStyle.Fill;
            lbAvailableColumns.Name = "lbAvailableColumns";
            lbAvailableColumns.SelectionMode = SelectionMode.One;
            lbAvailableColumns.SelectedIndexChanged += lbAvailableFields_SelectedIndexChanged;
            lbAvailableColumns.DoubleClick += lbAvailableColumns_DoubleClick;
            // 
            // lblAvailableFields
            // 
            this.lblAvailableColumns.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAvailableColumns.AutoSize = true;
            this.lblAvailableColumns.Dock = DockStyle.Top;
            this.lblAvailableColumns.Name = "lblAvailableColumns";
            this.lblAvailableColumns.TabIndex = 0;
            this.lblAvailableColumns.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            //
            // OrderedFieldListControl
            //
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Controls.Add(sc);
            Size = new System.Drawing.Size(200, 200);
            ts.ResumeLayout(false);
            ts.PerformLayout();
            tsc.ContentPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.PerformLayout();
            tsc.ResumeLayout(false);
            tsc.PerformLayout();
            sc.Panel1.ResumeLayout(false);
            sc.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sc).EndInit();
            sc.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer sc;
        private ToolStripContainer tsc;
        private ToolStrip ts;
        private ToolStripButton tsbUp;
        private ToolStripButton tsbDown;
        private ToolStripButton tsbLeft;
        private ToolStripButton tsbRight;
        private ListBox lbCurrentColumns;
        private ListBox lbAvailableColumns;
        private Label lblCurrentColumns;
        private Label lblAvailableColumns;
    }
}

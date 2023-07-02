namespace PgMulti.Forms
{
    partial class DiagramForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramForm));
            this.ms = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsc = new System.Windows.Forms.ToolStripContainer();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.tsbAddTables = new System.Windows.Forms.ToolStripButton();
            this.tsbExpandDiagram = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomFull = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscbTables = new System.Windows.Forms.ToolStripComboBox();
            this.tmrSave = new System.Windows.Forms.Timer(this.components);
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ms.SuspendLayout();
            this.tsc.TopToolStripPanel.SuspendLayout();
            this.tsc.SuspendLayout();
            this.ts.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // ms
            // 
            this.ms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem});
            this.ms.Location = new System.Drawing.Point(0, 0);
            this.ms.Name = "ms";
            this.ms.Size = new System.Drawing.Size(800, 28);
            this.ms.TabIndex = 0;
            this.ms.Text = "ms";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            this.tsc.ContentPanel.Size = new System.Drawing.Size(800, 394);
            this.tsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsc.Location = new System.Drawing.Point(0, 28);
            this.tsc.Name = "tsc";
            this.tsc.Size = new System.Drawing.Size(800, 422);
            this.tsc.TabIndex = 1;
            this.tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            this.tsc.TopToolStripPanel.Controls.Add(this.ts);
            // 
            // ts
            // 
            this.ts.Dock = System.Windows.Forms.DockStyle.None;
            this.ts.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddTables,
            this.tsbExpandDiagram,
            this.tsbZoomFull,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tscbTables});
            this.ts.Location = new System.Drawing.Point(4, 0);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(634, 28);
            this.ts.TabIndex = 0;
            // 
            // tsbAddTables
            // 
            this.tsbAddTables.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddTables.Image = global::PgMulti.Properties.Resources.add_table;
            this.tsbAddTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddTables.Name = "tsbAddTables";
            this.tsbAddTables.Size = new System.Drawing.Size(29, 25);
            this.tsbAddTables.Click += new System.EventHandler(this.tsbAddTables_Click);
            // 
            // tsbExpandDiagram
            // 
            this.tsbExpandDiagram.BackColor = System.Drawing.SystemColors.Control;
            this.tsbExpandDiagram.CheckOnClick = true;
            this.tsbExpandDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExpandDiagram.Image = global::PgMulti.Properties.Resources.expand;
            this.tsbExpandDiagram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExpandDiagram.Name = "tsbExpandDiagram";
            this.tsbExpandDiagram.Size = new System.Drawing.Size(29, 25);
            this.tsbExpandDiagram.Text = "toolStripButton1";
            this.tsbExpandDiagram.CheckedChanged += new System.EventHandler(this.tsbExpandDiagram_CheckedChanged);
            // 
            // tsbZoomFull
            // 
            this.tsbZoomFull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomFull.Image = global::PgMulti.Properties.Resources.zoom_full;
            this.tsbZoomFull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomFull.Name = "tsbZoomFull";
            this.tsbZoomFull.Size = new System.Drawing.Size(29, 25);
            this.tsbZoomFull.Text = "toolStripButton1";
            this.tsbZoomFull.Click += new System.EventHandler(this.tsbZoomFull_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(126, 25);
            this.toolStripLabel1.Text = "Seleccionar tabla:";
            // 
            // tscbTables
            // 
            this.tscbTables.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbTables.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbTables.Name = "tscbTables";
            this.tscbTables.Size = new System.Drawing.Size(400, 28);
            this.tscbTables.SelectedIndexChanged += new System.EventHandler(this.tscbTables_SelectedIndexChanged);
            // 
            // tmrSave
            // 
            this.tmrSave.Interval = 2000;
            this.tmrSave.Tick += new System.EventHandler(this.tmrSave_Tick);
            // 
            // cms
            // 
            this.cms.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTableToolStripMenuItem,
            this.addRelationToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.editToolStripMenuItem});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(211, 128);
            // 
            // addTableToolStripMenuItem
            // 
            this.addTableToolStripMenuItem.Name = "addTableToolStripMenuItem";
            this.addTableToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.addTableToolStripMenuItem.Text = "Añadir tabla";
            // 
            // addRelationToolStripMenuItem
            // 
            this.addRelationToolStripMenuItem.Name = "addRelationToolStripMenuItem";
            this.addRelationToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.addRelationToolStripMenuItem.Text = "Añadir relación";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.removeToolStripMenuItem.Text = "Eliminar";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.editToolStripMenuItem.Text = "Editar";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // DiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tsc);
            this.Controls.Add(this.ms);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.ms;
            this.Name = "DiagramForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagramForm_FormClosing);
            this.Load += new System.EventHandler(this.DiagramForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiagramForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DiagramForm_KeyUp);
            this.Resize += new System.EventHandler(this.DiagramForm_Resize);
            this.ms.ResumeLayout(false);
            this.ms.PerformLayout();
            this.tsc.TopToolStripPanel.ResumeLayout(false);
            this.tsc.TopToolStripPanel.PerformLayout();
            this.tsc.ResumeLayout(false);
            this.tsc.PerformLayout();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip ms;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripContainer tsc;
        private ToolStrip ts;
        private ToolStripButton tsbAddTables;
        private System.Windows.Forms.Timer tmrSave;
        private ContextMenuStrip cms;
        private ToolStripMenuItem addTableToolStripMenuItem;
        private ToolStripMenuItem addRelationToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripButton tsbExpandDiagram;
        private ToolStripButton tsbZoomFull;
        private ToolStripComboBox tscbTables;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
    }
}
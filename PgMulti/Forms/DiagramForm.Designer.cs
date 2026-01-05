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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramForm));
            tsc = new ToolStripContainer();
            ts = new ToolStrip();
            tsbSave = new ToolStripButton();
            tsbAddTables = new ToolStripButton();
            tsbRepositionTables = new ToolStripButton();
            tsbZoomFull = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tslSelectTable = new ToolStripLabel();
            tscbTables = new ToolStripComboBox();
            tmrSave = new System.Windows.Forms.Timer(components);
            cms = new ContextMenuStrip(components);
            tsmiAddTable = new ToolStripMenuItem();
            tsmiAddRelation = new ToolStripMenuItem();
            tsmiRemove = new ToolStripMenuItem();
            tsmiEdit = new ToolStripMenuItem();
            tsc.TopToolStripPanel.SuspendLayout();
            tsc.SuspendLayout();
            ts.SuspendLayout();
            cms.SuspendLayout();
            SuspendLayout();
            // 
            // tsc
            // 
            // 
            // tsc.ContentPanel
            // 
            tsc.ContentPanel.Size = new Size(800, 413);
            tsc.Dock = DockStyle.Fill;
            tsc.Location = new Point(0, 0);
            tsc.Name = "tsc";
            tsc.Size = new Size(800, 450);
            tsc.TabIndex = 1;
            tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            tsc.TopToolStripPanel.Controls.Add(ts);
            // 
            // ts
            // 
            ts.Dock = DockStyle.None;
            ts.ImageScalingSize = new Size(30, 30);
            ts.Items.AddRange(new ToolStripItem[] { tsbSave, tsbAddTables, tsbRepositionTables, tsbZoomFull, toolStripSeparator1, tslSelectTable, tscbTables });
            ts.Location = new Point(4, 0);
            ts.Name = "ts";
            ts.Size = new Size(598, 37);
            ts.TabIndex = 0;
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Enabled = false;
            tsbSave.Image = Properties.Resources.guardar;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(34, 34);
            tsbSave.Text = "toolStripButton1";
            tsbSave.Click += tsbSave_Click;
            // 
            // tsbAddTables
            // 
            tsbAddTables.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddTables.Image = Properties.Resources.add_table;
            tsbAddTables.ImageTransparentColor = Color.Magenta;
            tsbAddTables.Name = "tsbAddTables";
            tsbAddTables.Size = new Size(34, 34);
            tsbAddTables.Click += tsbAddTables_Click;
            // 
            // tsbExpandDiagram
            // 
            tsbRepositionTables.BackColor = SystemColors.Control;
            tsbRepositionTables.CheckOnClick = true;
            tsbRepositionTables.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRepositionTables.Image = Properties.Resources.expand;
            tsbRepositionTables.ImageTransparentColor = Color.Magenta;
            tsbRepositionTables.Name = "tsbExpandDiagram";
            tsbRepositionTables.Size = new Size(34, 34);
            tsbRepositionTables.Text = "toolStripButton1";
            tsbRepositionTables.CheckedChanged += tsbRepositionTables_CheckedChanged;
            // 
            // tsbZoomFull
            // 
            tsbZoomFull.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbZoomFull.Image = Properties.Resources.zoom_full;
            tsbZoomFull.ImageTransparentColor = Color.Magenta;
            tsbZoomFull.Name = "tsbZoomFull";
            tsbZoomFull.Size = new Size(34, 34);
            tsbZoomFull.Text = "toolStripButton1";
            tsbZoomFull.Click += tsbZoomFull_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 37);
            // 
            // tslSelectTable
            // 
            tslSelectTable.Name = "tslSelectTable";
            tslSelectTable.Size = new Size(0, 34);
            // 
            // tscbTables
            // 
            tscbTables.AutoCompleteSource = AutoCompleteSource.ListItems;
            tscbTables.Name = "tscbTables";
            tscbTables.Size = new Size(400, 37);
            tscbTables.SelectedIndexChanged += tscbTables_SelectedIndexChanged;
            tscbTables.KeyUp += tscbTables_KeyUp;
            // 
            // tmrSave
            // 
            tmrSave.Interval = 2000;
            tmrSave.Tick += tmrSave_Tick;
            // 
            // cms
            // 
            cms.ImageScalingSize = new Size(20, 20);
            cms.Items.AddRange(new ToolStripItem[] { tsmiAddTable, tsmiAddRelation, tsmiRemove, tsmiEdit });
            cms.Name = "cms";
            cms.Size = new Size(74, 108);
            cms.Opening += cms_Opening;
            // 
            // tsmiAddTable
            // 
            tsmiAddTable.Image = Properties.Resources.add_table;
            tsmiAddTable.Name = "tsmiAddTable";
            tsmiAddTable.Size = new Size(73, 26);
            tsmiAddTable.Click += tsmiAddTable_Click;
            // 
            // tsmiAddRelation
            // 
            tsmiAddRelation.Image = Properties.Resources.connect;
            tsmiAddRelation.Name = "tsmiAddRelation";
            tsmiAddRelation.Size = new Size(73, 26);
            tsmiAddRelation.Click += tsmiAddRelation_Click;
            // 
            // tsmiRemove
            // 
            tsmiRemove.Image = Properties.Resources.borrar;
            tsmiRemove.Name = "tsmiRemove";
            tsmiRemove.Size = new Size(73, 26);
            tsmiRemove.Click += tsmiRemove_Click;
            // 
            // tsmiEdit
            // 
            tsmiEdit.Image = Properties.Resources.editar;
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new Size(73, 26);
            tsmiEdit.Click += tsmiEdit_Click;
            // 
            // DiagramForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tsc);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "DiagramForm";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormClosing += DiagramForm_FormClosing;
            Load += DiagramForm_Load;
            KeyDown += DiagramForm_KeyDown;
            KeyUp += DiagramForm_KeyUp;
            Resize += DiagramForm_Resize;
            tsc.TopToolStripPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.PerformLayout();
            tsc.ResumeLayout(false);
            tsc.PerformLayout();
            ts.ResumeLayout(false);
            ts.PerformLayout();
            cms.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ToolStripContainer tsc;
        private ToolStrip ts;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbAddTables;
        private System.Windows.Forms.Timer tmrSave;
        private ContextMenuStrip cms;
        private ToolStripMenuItem tsmiAddTable;
        private ToolStripMenuItem tsmiRemove;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripButton tsbRepositionTables;
        private ToolStripButton tsbZoomFull;
        private ToolStripComboBox tscbTables;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel tslSelectTable;
        private ToolStripMenuItem tsmiAddRelation;
    }
}
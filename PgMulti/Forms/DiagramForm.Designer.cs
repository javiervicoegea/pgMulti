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
            tsbNew= new ToolStripButton();
            tsbOpen = new ToolStripButton();
            tsbSave = new ToolStripButton();
            tsbSaveAs = new ToolStripButton();
            tsbExportToImage = new ToolStripButton();
            tsbSqlFullDefinition = new ToolStripButton();
            tsbSqlTransformDB = new ToolStripButton();
            tsbPrint = new ToolStripButton();
            tsbAddTablesFromDataBase = new ToolStripButton();
            tsbAddNewTable = new ToolStripButton();
            tsbAddNewRelation = new ToolStripButton();
            tsbRemove = new ToolStripButton();
            tsbEdit = new ToolStripButton();
            tsbRepositionTables = new ToolStripButton();
            tsbSuggestRelatedTables = new ToolStripButton();
            tsbZoomFull = new ToolStripButton();
            tss1 = new ToolStripSeparator();
            tss2 = new ToolStripSeparator();
            tss3 = new ToolStripSeparator();
            tss4 = new ToolStripSeparator();
            tslSelectTable = new ToolStripLabel();
            tscbTables = new ToolStripComboBox();
            tmrSave = new System.Windows.Forms.Timer(components);
            cms = new ContextMenuStrip(components);
            tsmiAddNewTable = new ToolStripMenuItem();
            tsmiAddNewRelation = new ToolStripMenuItem();
            tsmiRemove = new ToolStripMenuItem();
            tsmiEdit = new ToolStripMenuItem();
            ofdOpenDiagram = new OpenFileDialog();
            sfdSaveDiagram = new SaveFileDialog();
            sfdExportDiagram = new SaveFileDialog();
            pd = new PrintDialog();
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
            // 
            // tsc.TopToolStripPanel
            // 
            tsc.TopToolStripPanel.Controls.Add(ts);
            // 
            // ts
            // 
            ts.Dock = DockStyle.None;
            ts.ImageScalingSize = new Size(30, 30);
            ts.Items.AddRange(new ToolStripItem[] { tsbNew, tsbOpen, tsbSave, tsbSaveAs, tsbExportToImage, tsbSqlFullDefinition, tsbSqlTransformDB, tsbPrint, tss1, tsbAddNewTable, tsbAddNewRelation, tsbAddTablesFromDataBase, tss2, tsbRemove, tsbEdit, tss3, tsbRepositionTables, tsbSuggestRelatedTables, tsbZoomFull, tss4, tslSelectTable, tscbTables });
            ts.Location = new Point(4, 0);
            ts.Name = "ts";
            ts.Size = new Size(598, 37);
            ts.TabIndex = 0;
            // 
            // tsbNew
            // 
            tsbNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbNew.Image = Properties.Resources.new_diagram;
            tsbNew.ImageTransparentColor = Color.Magenta;
            tsbNew.Name = "tsbNew";
            tsbNew.Size = new Size(34, 34);
            tsbNew.Click += tsbNew_Click;
            // 
            // tsbOpen
            // 
            tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpen.Image = Properties.Resources.abrir;
            tsbOpen.ImageTransparentColor = Color.Magenta;
            tsbOpen.Name = "tsbOpen";
            tsbOpen.Size = new Size(34, 34);
            tsbOpen.Click += tsbOpen_Click;
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Enabled = false;
            tsbSave.Image = Properties.Resources.guardar;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(34, 34);
            tsbSave.Click += tsbSave_Click;
            // 
            // tsbSaveAs
            // 
            tsbSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSaveAs.Enabled = false;
            tsbSaveAs.Image = Properties.Resources.guardar_como;
            tsbSaveAs.ImageTransparentColor = Color.Magenta;
            tsbSaveAs.Name = "tsbSaveAs";
            tsbSaveAs.Size = new Size(34, 34);
            tsbSaveAs.Click += tsbSaveAs_Click;
            // 
            // tsbExportToImage
            // 
            tsbExportToImage.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbExportToImage.Image = Properties.Resources.image;
            tsbExportToImage.ImageTransparentColor = Color.Magenta;
            tsbExportToImage.Name = "tsbExportToImage";
            tsbExportToImage.Size = new Size(34, 34);
            tsbExportToImage.Click += tsbExportToImage_Click;
            // 
            // tsbSqlFullDefinition
            // 
            tsbSqlFullDefinition.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSqlFullDefinition.Image = Properties.Resources.sql_new;
            tsbSqlFullDefinition.ImageTransparentColor = Color.Magenta;
            tsbSqlFullDefinition.Name = "tsbSqlFullDefinition";
            tsbSqlFullDefinition.Size = new Size(34, 34);
            tsbSqlFullDefinition.Click += tsbSqlFullDefinition_Click;
            // 
            // tsbSqlTransformDB
            // 
            tsbSqlTransformDB.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSqlTransformDB.Image = Properties.Resources.sql_transform;
            tsbSqlTransformDB.ImageTransparentColor = Color.Magenta;
            tsbSqlTransformDB.Name = "tsbSqlTransformDB";
            tsbSqlTransformDB.Size = new Size(34, 34);
            tsbSqlTransformDB.Click += tsbSqlTransformDB_Click;
            // 
            // tsbPrint
            // 
            tsbPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbPrint.Image = Properties.Resources.print;
            tsbPrint.ImageTransparentColor = Color.Magenta;
            tsbPrint.Name = "tsbPrint";
            tsbPrint.Size = new Size(34, 34);
            tsbPrint.Click += tsbPrint_Click;
            // 
            // tsbAddTablesFromDataBase
            // 
            tsbAddTablesFromDataBase.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddTablesFromDataBase.Image = Properties.Resources.tva_db;
            tsbAddTablesFromDataBase.ImageTransparentColor = Color.Magenta;
            tsbAddTablesFromDataBase.Name = "tsbAddTablesFromDataBase";
            tsbAddTablesFromDataBase.Size = new Size(34, 34);
            tsbAddTablesFromDataBase.Click += tsbAddTablesFromDataBase_Click;
            // 
            // tsbAddNewTable
            // 
            tsbAddNewTable.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddNewTable.Image = Properties.Resources.tabla;
            tsbAddNewTable.ImageTransparentColor = Color.Magenta;
            tsbAddNewTable.Name = "tsbAddNewTable";
            tsbAddNewTable.Size = new Size(34, 34);
            tsbAddNewTable.Click += tsbAddNewTable_Click;
            // 
            // tsbAddNewRelation
            // 
            tsbAddNewRelation.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddNewRelation.Image = Properties.Resources.connect;
            tsbAddNewRelation.ImageTransparentColor = Color.Magenta;
            tsbAddNewRelation.Name = "tsbAddNewRelation";
            tsbAddNewRelation.Size = new Size(34, 34);
            tsbAddNewRelation.Click += tsbAddNewRelation_Click;
            // 
            // tsbRemove
            // 
            tsbRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemove.Enabled = false;
            tsbRemove.Image = Properties.Resources.borrar;
            tsbRemove.ImageTransparentColor = Color.Magenta;
            tsbRemove.Name = "tsbRemove";
            tsbRemove.Size = new Size(34, 34);
            tsbRemove.Click += tsbRemove_Click;
            // 
            // tsbEdit
            // 
            tsbEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEdit.Enabled = false;
            tsbEdit.Image = Properties.Resources.editar;
            tsbEdit.ImageTransparentColor = Color.Magenta;
            tsbEdit.Name = "tsbEdit";
            tsbEdit.Size = new Size(34, 34);
            tsbEdit.Click += tsbEdit_Click;
            // 
            // tsbRepositionTables
            // 
            tsbRepositionTables.BackColor = SystemColors.Control;
            tsbRepositionTables.CheckOnClick = true;
            tsbRepositionTables.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRepositionTables.Image = Properties.Resources.expand;
            tsbRepositionTables.ImageTransparentColor = Color.Magenta;
            tsbRepositionTables.Name = "tsbRepositionTables";
            tsbRepositionTables.Size = new Size(34, 34);
            tsbRepositionTables.CheckedChanged += tsbRepositionTables_CheckedChanged;
            // 
            // tsbSuggestRelatedTables
            // 
            tsbSuggestRelatedTables.BackColor = SystemColors.Control;
            tsbSuggestRelatedTables.CheckOnClick = true;
            tsbSuggestRelatedTables.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSuggestRelatedTables.Image = Properties.Resources.expand;
            tsbSuggestRelatedTables.ImageTransparentColor = Color.Magenta;
            tsbSuggestRelatedTables.Name = "tsbSuggestRelatedTables";
            tsbSuggestRelatedTables.Size = new Size(34, 34);
            tsbSuggestRelatedTables.Visible = false;
            tsbSuggestRelatedTables.CheckedChanged += tsbSuggestRelatedTables_CheckedChanged;
            // 
            // tsbZoomFull
            // 
            tsbZoomFull.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbZoomFull.Image = Properties.Resources.zoom_full;
            tsbZoomFull.ImageTransparentColor = Color.Magenta;
            tsbZoomFull.Name = "tsbZoomFull";
            tsbZoomFull.Size = new Size(34, 34);
            tsbZoomFull.Click += tsbZoomFull_Click;
            // 
            // tss1
            // 
            tss1.Name = "tss1";
            tss1.Size = new Size(6, 37);
            // 
            // tss2
            // 
            tss2.Name = "tss2";
            tss2.Size = new Size(6, 37);
            // 
            // tss3
            // 
            tss3.Name = "tss3";
            tss3.Size = new Size(6, 37);
            // 
            // tss4
            // 
            tss4.Name = "tss4";
            tss4.Size = new Size(6, 37);
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
            cms.Items.AddRange(new ToolStripItem[] { tsmiAddNewTable, tsmiAddNewRelation, tsmiEdit, tsmiRemove });
            cms.Name = "cms";
            cms.Size = new Size(74, 108);
            cms.Opening += cms_Opening;
            // 
            // tsmiAddNewTable
            // 
            tsmiAddNewTable.Image = Properties.Resources.tabla;
            tsmiAddNewTable.Name = "tsmiAddNewTable";
            tsmiAddNewTable.Size = new Size(73, 26);
            tsmiAddNewTable.Click += tsmiAddNewTable_Click;
            // 
            // tsmiAddNewRelation
            // 
            tsmiAddNewRelation.Image = Properties.Resources.connect;
            tsmiAddNewRelation.Name = "tsmiAddNewRelation";
            tsmiAddNewRelation.Size = new Size(73, 26);
            tsmiAddNewRelation.Click += tsmiAddNewRelation_Click;
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
            // ofdOpenDiagram
            // 
            ofdOpenDiagram.FileName = "*.pgdx";
            ofdOpenDiagram.FilterIndex = 0;
            // 
            // sfdSaveDiagram
            // 
            sfdSaveDiagram.DefaultExt = "pgdx";
            sfdSaveDiagram.FileName = "pgMultiDiagram.pgdx";
            sfdSaveDiagram.FilterIndex = 0;
            // 
            // sfdExportDiagram
            // 
            sfdExportDiagram.FilterIndex = 0;
            // 
            // pd
            // 
            pd.AllowSomePages = false;
            pd.AllowSelection = false;
            pd.UseEXDialog = true;
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
        private ToolStripButton tsbNew;
        private ToolStripButton tsbOpen;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbSaveAs;
        private ToolStripButton tsbExportToImage;
        private ToolStripButton tsbSqlFullDefinition;
        private ToolStripButton tsbSqlTransformDB;
        private ToolStripButton tsbPrint;
        private ToolStripButton tsbAddTablesFromDataBase;
        private ToolStripButton tsbAddNewTable;
        private ToolStripButton tsbAddNewRelation;
        private ToolStripButton tsbRemove;
        private ToolStripButton tsbEdit;
        private ToolStripButton tsbRepositionTables;
        private ToolStripButton tsbSuggestRelatedTables;
        private ToolStripButton tsbZoomFull;
        private System.Windows.Forms.Timer tmrSave;
        private ContextMenuStrip cms;
        private ToolStripMenuItem tsmiAddNewTable;
        private ToolStripMenuItem tsmiRemove;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripComboBox tscbTables;
        private ToolStripSeparator tss1;
        private ToolStripSeparator tss2;
        private ToolStripSeparator tss3;
        private ToolStripSeparator tss4;
        private ToolStripLabel tslSelectTable;
        private ToolStripMenuItem tsmiAddNewRelation;
        private OpenFileDialog ofdOpenDiagram;
        private SaveFileDialog sfdSaveDiagram;
        private SaveFileDialog sfdExportDiagram;
        private PrintDialog pd;
    }
}
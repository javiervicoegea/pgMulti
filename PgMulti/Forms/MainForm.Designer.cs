using PgMulti.QueryEditor;
using TradeWright.UI.Forms;

namespace PgMulti
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            this.cmsServers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tscmiNewGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiNewDB = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiExploreTable = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiRecursiveRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiCreateTableDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiUp = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiDown = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.ncb = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.nsi = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ntb = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbNewGroup = new System.Windows.Forms.ToolStripButton();
            this.tsbNewDB = new System.Windows.Forms.ToolStripButton();
            this.tssNew = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExploreTable = new System.Windows.Forms.ToolStripButton();
            this.tsbRecursiveRemove = new System.Windows.Forms.ToolStripButton();
            this.tsbCreateTableDiagram = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbRemove = new System.Windows.Forms.ToolStripButton();
            this.tssEdit = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUp = new System.Windows.Forms.ToolStripButton();
            this.tsbDown = new System.Windows.Forms.ToolStripButton();
            this.tssUpDown = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tcSql = new TradeWright.UI.Forms.TabControlExtra();
            this.tpNewTab = new System.Windows.Forms.TabPage();
            this.ilTabControl = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRun = new System.Windows.Forms.ToolStripButton();
            this.tsbExportCsv = new System.Windows.Forms.ToolStripButton();
            this.tsddbTransactions = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiTransactionModeManual = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransactionModeAutoSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransactionModeAutoCoordinated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTransactionLevelReadCommitted = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransactionLevelRepeatableRead = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransactionLevelSerializable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.tsbReplace = new System.Windows.Forms.ToolStripButton();
            this.tsbGoTo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFormat = new System.Windows.Forms.ToolStripButton();
            this.tsbHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOpenDiagram = new System.Windows.Forms.ToolStripButton();
            this.tsbNewDiagram = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbErrors = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tslPosition = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
            this.lbResult = new System.Windows.Forms.ListBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.tsbCurrentTabLastTask = new System.Windows.Forms.ToolStripButton();
            this.tssResult = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRemoveSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.tsbStopSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbStopAll = new System.Windows.Forms.ToolStripButton();
            this.tcResult = new System.Windows.Forms.TabControl();
            this.tpResult = new System.Windows.Forms.TabPage();
            this.toolStripContainer6 = new System.Windows.Forms.ToolStripContainer();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.tsddbAutoScroll = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiManualScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tpTable = new System.Windows.Forms.TabPage();
            this.toolStripContainer5 = new System.Windows.Forms.ToolStripContainer();
            this.gvTable = new System.Windows.Forms.DataGridView();
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsbTextEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSetNull = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip5 = new System.Windows.Forms.ToolStrip();
            this.tsddbTables = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsddbInsertRow = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbDeleteRows = new System.Windows.Forms.ToolStripButton();
            this.tsbApplyTableChanges = new System.Windows.Forms.ToolStripButton();
            this.tpExecutedSql = new System.Windows.Forms.TabPage();
            this.toolStripContainer4 = new System.Windows.Forms.ToolStripContainer();
            this.fctbExecutedSql = new PgMulti.QueryEditor.CustomFctb();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.tsbEditExecutedSql = new System.Windows.Forms.ToolStripButton();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBack = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiForward = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFind = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGoTo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiIncreaseFont = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReduceFont = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImportConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoreOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAllTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAllTabsExceptThisOne = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReopenLastClosedTab = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClosedTabsLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiBack = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiForward = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiFind = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiGoTo = new System.Windows.Forms.ToolStripMenuItem();
            this.tscmiFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.ilServers = new System.Windows.Forms.ImageList(this.components);
            this.ofdSql = new System.Windows.Forms.OpenFileDialog();
            this.sfdSql = new System.Windows.Forms.SaveFileDialog();
            this.tmrPosition = new System.Windows.Forms.Timer(this.components);
            this.tmrSaveTabs = new System.Windows.Forms.Timer(this.components);
            this.tmrResult = new System.Windows.Forms.Timer(this.components);
            this.tmrFitGridColumns = new System.Windows.Forms.Timer(this.components);
            this.ilAutocompleteMenu = new System.Windows.Forms.ImageList(this.components);
            this.cmsFctb = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sfdCsv = new System.Windows.Forms.SaveFileDialog();
            this.mm = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRunMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDiagrams = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdImportConfig = new System.Windows.Forms.OpenFileDialog();
            this.ofdOpenDiagram = new System.Windows.Forms.OpenFileDialog();
            this.sfdSaveDiagram = new System.Windows.Forms.SaveFileDialog();
            this.tmrReenableRunButton = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.cmsServers.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tcSql.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStripContainer3.ContentPanel.SuspendLayout();
            this.toolStripContainer3.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer3.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tcResult.SuspendLayout();
            this.tpResult.SuspendLayout();
            this.toolStripContainer6.ContentPanel.SuspendLayout();
            this.toolStripContainer6.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer6.SuspendLayout();
            this.toolStrip6.SuspendLayout();
            this.tpTable.SuspendLayout();
            this.toolStripContainer5.ContentPanel.SuspendLayout();
            this.toolStripContainer5.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTable)).BeginInit();
            this.cmsTable.SuspendLayout();
            this.toolStrip5.SuspendLayout();
            this.tpExecutedSql.SuspendLayout();
            this.toolStripContainer4.ContentPanel.SuspendLayout();
            this.toolStripContainer4.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbExecutedSql)).BeginInit();
            this.toolStrip4.SuspendLayout();
            this.cmsTabs.SuspendLayout();
            this.cmsFctb.SuspendLayout();
            this.mm.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1539, 502);
            this.splitContainer1.SplitterDistance = 295;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStripContainer2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.toolStripContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1539, 295);
            this.splitContainer2.SplitterDistance = 410;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.tvaConnections);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(410, 258);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(410, 295);
            this.toolStripContainer2.TabIndex = 1;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // tvaConnections
            // 
            this.tvaConnections.AllowDrop = true;
            this.tvaConnections.AsyncExpanding = true;
            this.tvaConnections.AutoRowHeight = true;
            this.tvaConnections.BackColor = System.Drawing.SystemColors.Window;
            this.tvaConnections.ContextMenuStrip = this.cmsServers;
            this.tvaConnections.DefaultToolTipProvider = null;
            this.tvaConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvaConnections.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvaConnections.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tvaConnections.Indent = 25;
            this.tvaConnections.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvaConnections.LoadOnDemand = true;
            this.tvaConnections.Location = new System.Drawing.Point(0, 0);
            this.tvaConnections.Margin = new System.Windows.Forms.Padding(10);
            this.tvaConnections.Model = null;
            this.tvaConnections.Name = "tvaConnections";
            this.tvaConnections.NodeControls.Add(this.ncb);
            this.tvaConnections.NodeControls.Add(this.nsi);
            this.tvaConnections.NodeControls.Add(this.ntb);
            this.tvaConnections.RowHeight = 25;
            this.tvaConnections.SelectedNode = null;
            this.tvaConnections.Size = new System.Drawing.Size(410, 258);
            this.tvaConnections.TabIndex = 0;
            this.tvaConnections.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvaConnections_ItemDrag);
            this.tvaConnections.SelectionChanged += new System.EventHandler(this.tvaServers_SelectionChanged);
            this.tvaConnections.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvaConnections_DragDrop);
            this.tvaConnections.DragOver += new System.Windows.Forms.DragEventHandler(this.tvaConnections_DragOver);
            this.tvaConnections.DoubleClick += new System.EventHandler(this.tvaServers_DoubleClick);
            this.tvaConnections.Enter += new System.EventHandler(this.tvaConnections_Enter);
            this.tvaConnections.Leave += new System.EventHandler(this.tvaConnections_Leave);
            // 
            // cmsServers
            // 
            this.cmsServers.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsServers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscmiNewGroup,
            this.tscmiNewDB,
            this.tscmiExploreTable,
            this.tscmiRecursiveRemove,
            this.tscmiCreateTableDiagram,
            this.tscmiEdit,
            this.tscmiRemove,
            this.tscmiUp,
            this.tscmiDown,
            this.tscmiRefresh,
            this.tscmiCopyText});
            this.cmsServers.Name = "cmsServers";
            this.cmsServers.Size = new System.Drawing.Size(74, 264);
            // 
            // tscmiNewGroup
            // 
            this.tscmiNewGroup.Image = global::PgMulti.Properties.Resources.nuevo_grupo;
            this.tscmiNewGroup.Name = "tscmiNewGroup";
            this.tscmiNewGroup.Size = new System.Drawing.Size(73, 26);
            this.tscmiNewGroup.Click += new System.EventHandler(this.tscmiNewGroup_Click);
            // 
            // tscmiNewDB
            // 
            this.tscmiNewDB.Image = global::PgMulti.Properties.Resources.nueva_db;
            this.tscmiNewDB.Name = "tscmiNewDB";
            this.tscmiNewDB.Size = new System.Drawing.Size(73, 26);
            this.tscmiNewDB.Click += new System.EventHandler(this.tscmiNewDB_Click);
            // 
            // tscmiExploreTable
            // 
            this.tscmiExploreTable.Image = global::PgMulti.Properties.Resources.abrir_tabla;
            this.tscmiExploreTable.Name = "tscmiExploreTable";
            this.tscmiExploreTable.Size = new System.Drawing.Size(73, 26);
            this.tscmiExploreTable.Click += new System.EventHandler(this.tscmiExploreTable_Click);
            // 
            // tscmiRecursiveRemove
            // 
            this.tscmiRecursiveRemove.Image = global::PgMulti.Properties.Resources.borrar_todos;
            this.tscmiRecursiveRemove.Name = "tscmiRecursiveRemove";
            this.tscmiRecursiveRemove.Size = new System.Drawing.Size(73, 26);
            this.tscmiRecursiveRemove.Click += new System.EventHandler(this.tscmiRecursiveRemove_Click);
            // 
            // tscmiCreateTableDiagram
            // 
            this.tscmiCreateTableDiagram.Image = global::PgMulti.Properties.Resources.diagram;
            this.tscmiCreateTableDiagram.Name = "tscmiCreateTableDiagram";
            this.tscmiCreateTableDiagram.Size = new System.Drawing.Size(73, 26);
            this.tscmiCreateTableDiagram.Click += new System.EventHandler(this.tscmiCreateTableDiagram_Click);
            // 
            // tscmiEdit
            // 
            this.tscmiEdit.Image = global::PgMulti.Properties.Resources.editar;
            this.tscmiEdit.Name = "tscmiEdit";
            this.tscmiEdit.Size = new System.Drawing.Size(73, 26);
            this.tscmiEdit.Click += new System.EventHandler(this.tscmiEdit_Click);
            // 
            // tscmiRemove
            // 
            this.tscmiRemove.Image = global::PgMulti.Properties.Resources.borrar;
            this.tscmiRemove.Name = "tscmiRemove";
            this.tscmiRemove.Size = new System.Drawing.Size(73, 26);
            this.tscmiRemove.Click += new System.EventHandler(this.tscmiRemove_Click);
            // 
            // tscmiUp
            // 
            this.tscmiUp.Image = global::PgMulti.Properties.Resources.arriba;
            this.tscmiUp.Name = "tscmiUp";
            this.tscmiUp.Size = new System.Drawing.Size(73, 26);
            this.tscmiUp.Click += new System.EventHandler(this.tscmiUp_Click);
            // 
            // tscmiDown
            // 
            this.tscmiDown.Image = global::PgMulti.Properties.Resources.abajo;
            this.tscmiDown.Name = "tscmiDown";
            this.tscmiDown.Size = new System.Drawing.Size(73, 26);
            this.tscmiDown.Click += new System.EventHandler(this.tscmiDown_Click);
            // 
            // tscmiRefresh
            // 
            this.tscmiRefresh.Image = global::PgMulti.Properties.Resources.actualizar;
            this.tscmiRefresh.Name = "tscmiRefresh";
            this.tscmiRefresh.Size = new System.Drawing.Size(73, 26);
            this.tscmiRefresh.Click += new System.EventHandler(this.tscmiRefresh_Click);
            // 
            // tscmiCopyText
            // 
            this.tscmiCopyText.Image = global::PgMulti.Properties.Resources.copiar;
            this.tscmiCopyText.Name = "tscmiCopyText";
            this.tscmiCopyText.Size = new System.Drawing.Size(73, 26);
            this.tscmiCopyText.Click += new System.EventHandler(this.tscmiCopyText_Click);
            // 
            // ncb
            // 
            this.ncb.DataPropertyName = "CheckState";
            this.ncb.EditEnabled = true;
            this.ncb.ImageSize = 20;
            this.ncb.LeftMargin = 5;
            this.ncb.ParentColumn = null;
            // 
            // nsi
            // 
            this.nsi.DataPropertyName = "Image";
            this.nsi.LeftMargin = 5;
            this.nsi.ParentColumn = null;
            this.nsi.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntb
            // 
            this.ntb.DataPropertyName = "Text";
            this.ntb.IncrementalSearchEnabled = true;
            this.ntb.LeftMargin = 5;
            this.ntb.ParentColumn = null;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewGroup,
            this.tsbNewDB,
            this.tssNew,
            this.tsbExploreTable,
            this.tsbRecursiveRemove,
            this.tsbCreateTableDiagram,
            this.tsbEdit,
            this.tsbRemove,
            this.tssEdit,
            this.tsbUp,
            this.tsbDown,
            this.tssUpDown,
            this.tsbCollapseAll,
            this.tsbRefresh});
            this.toolStrip2.Location = new System.Drawing.Point(4, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(81, 37);
            this.toolStrip2.TabIndex = 0;
            // 
            // tsbNewGroup
            // 
            this.tsbNewGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewGroup.Image = global::PgMulti.Properties.Resources.nuevo_grupo;
            this.tsbNewGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewGroup.Name = "tsbNewGroup";
            this.tsbNewGroup.Size = new System.Drawing.Size(34, 34);
            this.tsbNewGroup.Visible = false;
            this.tsbNewGroup.Click += new System.EventHandler(this.tsbNewGroup_Click);
            // 
            // tsbNewDB
            // 
            this.tsbNewDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewDB.Image = global::PgMulti.Properties.Resources.nueva_db;
            this.tsbNewDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewDB.Name = "tsbNewDB";
            this.tsbNewDB.Size = new System.Drawing.Size(34, 34);
            this.tsbNewDB.Visible = false;
            this.tsbNewDB.Click += new System.EventHandler(this.tsbNewDB_Click);
            // 
            // tssNew
            // 
            this.tssNew.Name = "tssNew";
            this.tssNew.Size = new System.Drawing.Size(6, 37);
            this.tssNew.Visible = false;
            // 
            // tsbExploreTable
            // 
            this.tsbExploreTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExploreTable.Image = global::PgMulti.Properties.Resources.abrir_tabla;
            this.tsbExploreTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExploreTable.Name = "tsbExploreTable";
            this.tsbExploreTable.Size = new System.Drawing.Size(34, 34);
            this.tsbExploreTable.Visible = false;
            this.tsbExploreTable.Click += new System.EventHandler(this.tsbExploreTable_Click);
            // 
            // tsbRecursiveRemove
            // 
            this.tsbRecursiveRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRecursiveRemove.Image = global::PgMulti.Properties.Resources.borrar_todos;
            this.tsbRecursiveRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRecursiveRemove.Name = "tsbRecursiveRemove";
            this.tsbRecursiveRemove.Size = new System.Drawing.Size(34, 34);
            this.tsbRecursiveRemove.Visible = false;
            this.tsbRecursiveRemove.Click += new System.EventHandler(this.tsbRecursiveRemove_Click);
            // 
            // tsbCreateTableDiagram
            // 
            this.tsbCreateTableDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCreateTableDiagram.Image = global::PgMulti.Properties.Resources.diagram;
            this.tsbCreateTableDiagram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCreateTableDiagram.Name = "tsbCreateTableDiagram";
            this.tsbCreateTableDiagram.Size = new System.Drawing.Size(34, 34);
            this.tsbCreateTableDiagram.Visible = false;
            this.tsbCreateTableDiagram.Click += new System.EventHandler(this.tsbCreateTableDiagram_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEdit.Image = global::PgMulti.Properties.Resources.editar;
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(34, 34);
            this.tsbEdit.Visible = false;
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbRemove
            // 
            this.tsbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemove.Image = global::PgMulti.Properties.Resources.borrar;
            this.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemove.Name = "tsbRemove";
            this.tsbRemove.Size = new System.Drawing.Size(34, 34);
            this.tsbRemove.Visible = false;
            this.tsbRemove.Click += new System.EventHandler(this.tsbRemove_Click);
            // 
            // tssEdit
            // 
            this.tssEdit.Name = "tssEdit";
            this.tssEdit.Size = new System.Drawing.Size(6, 37);
            this.tssEdit.Visible = false;
            // 
            // tsbUp
            // 
            this.tsbUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUp.Image = global::PgMulti.Properties.Resources.arriba;
            this.tsbUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUp.Name = "tsbUp";
            this.tsbUp.Size = new System.Drawing.Size(34, 34);
            this.tsbUp.Visible = false;
            this.tsbUp.Click += new System.EventHandler(this.tsbUp_Click);
            // 
            // tsbDown
            // 
            this.tsbDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDown.Image = global::PgMulti.Properties.Resources.abajo;
            this.tsbDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDown.Name = "tsbDown";
            this.tsbDown.Size = new System.Drawing.Size(34, 34);
            this.tsbDown.Visible = false;
            this.tsbDown.Click += new System.EventHandler(this.tsbDown_Click);
            // 
            // tssUpDown
            // 
            this.tssUpDown.Name = "tssUpDown";
            this.tssUpDown.Size = new System.Drawing.Size(6, 37);
            this.tssUpDown.Visible = false;
            // 
            // tsbCollapseAll
            // 
            this.tsbCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCollapseAll.Image = global::PgMulti.Properties.Resources.contraer;
            this.tsbCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCollapseAll.Name = "tsbCollapseAll";
            this.tsbCollapseAll.Size = new System.Drawing.Size(34, 34);
            this.tsbCollapseAll.Click += new System.EventHandler(this.tsbCollapseAll_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::PgMulti.Properties.Resources.actualizar;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(34, 34);
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tcSql);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1125, 258);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1125, 295);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // tcSql
            // 
            this.tcSql.Controls.Add(this.tpNewTab);
            this.tcSql.DisplayStyle = TradeWright.UI.Forms.TabStyle.Rounded;
            // 
            // 
            // 
            this.tcSql.DisplayStyleProvider.BlendStyle = TradeWright.UI.Forms.BlendStyle.Normal;
            this.tcSql.DisplayStyleProvider.BorderColorDisabled = System.Drawing.SystemColors.ControlLight;
            this.tcSql.DisplayStyleProvider.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.tcSql.DisplayStyleProvider.BorderColorHighlighted = System.Drawing.SystemColors.ControlDark;
            this.tcSql.DisplayStyleProvider.BorderColorSelected = System.Drawing.SystemColors.ControlDark;
            this.tcSql.DisplayStyleProvider.BorderColorUnselected = System.Drawing.SystemColors.ControlDark;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorFocused = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorFocusedActive = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorHighlighted = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorHighlightedActive = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorSelected = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorSelectedActive = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.CloserButtonFillColorUnselected = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorFocused = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorFocusedActive = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorHighlighted = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorHighlightedActive = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorSelected = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorSelectedActive = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserButtonOutlineColorUnselected = System.Drawing.Color.Empty;
            this.tcSql.DisplayStyleProvider.CloserColorFocused = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.CloserColorFocusedActive = System.Drawing.Color.White;
            this.tcSql.DisplayStyleProvider.CloserColorHighlighted = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.CloserColorHighlightedActive = System.Drawing.Color.White;
            this.tcSql.DisplayStyleProvider.CloserColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(102)))), ((int)(((byte)(115)))));
            this.tcSql.DisplayStyleProvider.CloserColorSelectedActive = System.Drawing.Color.White;
            this.tcSql.DisplayStyleProvider.CloserColorUnselected = System.Drawing.Color.Black;
            this.tcSql.DisplayStyleProvider.FocusTrack = false;
            this.tcSql.DisplayStyleProvider.HotTrack = true;
            this.tcSql.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tcSql.DisplayStyleProvider.Opacity = 1F;
            this.tcSql.DisplayStyleProvider.Overlap = 3;
            this.tcSql.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
            this.tcSql.DisplayStyleProvider.PageBackgroundColorDisabled = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.PageBackgroundColorFocused = System.Drawing.SystemColors.ControlLight;
            this.tcSql.DisplayStyleProvider.PageBackgroundColorHighlighted = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.tcSql.DisplayStyleProvider.PageBackgroundColorSelected = System.Drawing.SystemColors.ControlLightLight;
            this.tcSql.DisplayStyleProvider.PageBackgroundColorUnselected = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.Radius = 10;
            this.tcSql.DisplayStyleProvider.SelectedTabIsLarger = false;
            this.tcSql.DisplayStyleProvider.ShowTabCloser = true;
            this.tcSql.DisplayStyleProvider.TabColorDisabled1 = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.TabColorDisabled2 = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.TabColorFocused1 = System.Drawing.SystemColors.ControlLightLight;
            this.tcSql.DisplayStyleProvider.TabColorFocused2 = System.Drawing.SystemColors.ControlLightLight;
            this.tcSql.DisplayStyleProvider.TabColorHighLighted1 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.tcSql.DisplayStyleProvider.TabColorHighLighted2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(237)))), ((int)(((byte)(252)))));
            this.tcSql.DisplayStyleProvider.TabColorSelected1 = System.Drawing.SystemColors.ControlLightLight;
            this.tcSql.DisplayStyleProvider.TabColorSelected2 = System.Drawing.SystemColors.ControlLightLight;
            this.tcSql.DisplayStyleProvider.TabColorUnSelected1 = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.TabColorUnSelected2 = System.Drawing.SystemColors.Control;
            this.tcSql.DisplayStyleProvider.TabPageMargin = new System.Windows.Forms.Padding(1);
            this.tcSql.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.tcSql.DisplayStyleProvider.TextColorFocused = System.Drawing.SystemColors.ControlText;
            this.tcSql.DisplayStyleProvider.TextColorHighlighted = System.Drawing.SystemColors.ControlText;
            this.tcSql.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.tcSql.DisplayStyleProvider.TextColorUnselected = System.Drawing.SystemColors.ControlText;
            this.tcSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSql.HotTrack = true;
            this.tcSql.AllowReorder = true;
            this.tcSql.NewTabButton = true;
            this.tcSql.ImageList = this.ilTabControl;
            this.tcSql.Location = new System.Drawing.Point(0, 0);
            this.tcSql.Name = "tcSql";
            this.tcSql.SelectedIndex = 0;
            this.tcSql.Size = new System.Drawing.Size(1125, 258);
            this.tcSql.TabIndex = 0;
            this.tcSql.TabClosing += new System.EventHandler<System.Windows.Forms.TabControlCancelEventArgs>(this.tcSql_TabClosing);
            this.tcSql.SelectedIndexChanged += new System.EventHandler(this.tcSql_SelectedIndexChanged);
            this.tcSql.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tcSql_MouseDown);
            this.tcSql.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tcSql_MouseUp);
            this.tcSql.ReorderedTabs += new EventHandler<EventArgs>(tcSql_ReorderedTabs);
            // 
            // tpNewTab
            // 
            this.tpNewTab.ImageIndex = 0;
            this.tpNewTab.Location = new System.Drawing.Point(4, 32);
            this.tpNewTab.Name = "tpNewTab";
            this.tpNewTab.Size = new System.Drawing.Size(1117, 222);
            this.tpNewTab.TabIndex = 0;
            // 
            // ilTabControl
            // 
            this.ilTabControl.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTabControl.ImageStream")));
            this.ilTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTabControl.Images.SetKeyName(0, "nuevo.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbSave,
            this.tsbSaveAll,
            this.toolStripSeparator2,
            this.tsbRun,
            this.tsbExportCsv,
            this.tsddbTransactions,
            this.toolStripSeparator3,
            this.tsbFind,
            this.tsbReplace,
            this.tsbGoTo,
            this.toolStripSeparator5,
            this.tsbFormat,
            this.tsbHistory,
            this.toolStripSeparator7,
            this.tsbOpenDiagram,
            this.tsbNewDiagram,
            this.toolStripSeparator1,
            this.tsddbErrors,
            this.toolStripSeparator6,
            this.tslPosition});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(545, 37);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::PgMulti.Properties.Resources.abrir;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(34, 34);
            this.tsbOpen.Text = "toolStripButton1";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::PgMulti.Properties.Resources.guardar;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(34, 34);
            this.tsbSave.Text = "toolStripButton1";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSaveAll
            // 
            this.tsbSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveAll.Image = global::PgMulti.Properties.Resources.guardar_todo;
            this.tsbSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Size = new System.Drawing.Size(34, 34);
            this.tsbSaveAll.Text = "toolStripButton1";
            this.tsbSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbRun
            // 
            this.tsbRun.Enabled = false;
            this.tsbRun.Image = global::PgMulti.Properties.Resources.ejecutar;
            this.tsbRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRun.Name = "tsbRun";
            this.tsbRun.Size = new System.Drawing.Size(34, 34);
            this.tsbRun.ToolTipText = "Ejecutar";
            this.tsbRun.Click += new System.EventHandler(this.tsbRun_Click);
            // 
            // tsbExportCsv
            // 
            this.tsbExportCsv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportCsv.Image = global::PgMulti.Properties.Resources.download;
            this.tsbExportCsv.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportCsv.Name = "tsbExportCsv";
            this.tsbExportCsv.Size = new System.Drawing.Size(34, 34);
            this.tsbExportCsv.Click += new System.EventHandler(this.tsbExportCsv_Click);
            // 
            // tsddbTransactions
            // 
            this.tsddbTransactions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbTransactions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTransactionModeManual,
            this.tsmiTransactionModeAutoSingle,
            this.tsmiTransactionModeAutoCoordinated,
            this.toolStripSeparator4,
            this.tsmiTransactionLevelReadCommitted,
            this.tsmiTransactionLevelRepeatableRead,
            this.tsmiTransactionLevelSerializable});
            this.tsddbTransactions.Image = global::PgMulti.Properties.Resources.transaccion;
            this.tsddbTransactions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbTransactions.Name = "tsddbTransactions";
            this.tsddbTransactions.Size = new System.Drawing.Size(44, 34);
            this.tsddbTransactions.Text = "toolStripDropDownButton2";
            // 
            // tsmiTransactionModeManual
            // 
            this.tsmiTransactionModeManual.Name = "tsmiTransactionModeManual";
            this.tsmiTransactionModeManual.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionModeManual.Text = "Manual";
            this.tsmiTransactionModeManual.Click += new System.EventHandler(this.tsmiTransactionModeManual_Click);
            // 
            // tsmiTransactionModeAutoSingle
            // 
            this.tsmiTransactionModeAutoSingle.Name = "tsmiTransactionModeAutoSingle";
            this.tsmiTransactionModeAutoSingle.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionModeAutoSingle.Text = "Auto-Single";
            this.tsmiTransactionModeAutoSingle.Click += new System.EventHandler(this.tsmiTransactionModeAutoSingle_Click);
            // 
            // tsmiTransactionModeAutoCoordinated
            // 
            this.tsmiTransactionModeAutoCoordinated.Name = "tsmiTransactionModeAutoCoordinated";
            this.tsmiTransactionModeAutoCoordinated.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionModeAutoCoordinated.Text = "Auto-Coord";
            this.tsmiTransactionModeAutoCoordinated.Click += new System.EventHandler(this.tsmiTransactionModeAutoCoordinated_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
            // 
            // tsmiTransactionLevelReadCommitted
            // 
            this.tsmiTransactionLevelReadCommitted.Name = "tsmiTransactionLevelReadCommitted";
            this.tsmiTransactionLevelReadCommitted.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionLevelReadCommitted.Text = "Read committed";
            this.tsmiTransactionLevelReadCommitted.Click += new System.EventHandler(this.tsmiTransactionLevelReadCommitted_Click);
            // 
            // tsmiTransactionLevelRepeatableRead
            // 
            this.tsmiTransactionLevelRepeatableRead.Name = "tsmiTransactionLevelRepeatableRead";
            this.tsmiTransactionLevelRepeatableRead.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionLevelRepeatableRead.Text = "Repeatable read";
            this.tsmiTransactionLevelRepeatableRead.Click += new System.EventHandler(this.tsmiTransactionLevelRepeatableRead_Click);
            // 
            // tsmiTransactionLevelSerializable
            // 
            this.tsmiTransactionLevelSerializable.Name = "tsmiTransactionLevelSerializable";
            this.tsmiTransactionLevelSerializable.Size = new System.Drawing.Size(203, 26);
            this.tsmiTransactionLevelSerializable.Text = "Serializable";
            this.tsmiTransactionLevelSerializable.Click += new System.EventHandler(this.tsmiTransactionLevelSerializable_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.Image = global::PgMulti.Properties.Resources.buscar;
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(34, 34);
            this.tsbFind.Text = "toolStripButton1";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // tsbReplace
            // 
            this.tsbReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReplace.Image = global::PgMulti.Properties.Resources.reemplazar;
            this.tsbReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReplace.Name = "tsbReplace";
            this.tsbReplace.Size = new System.Drawing.Size(34, 34);
            this.tsbReplace.Text = "toolStripButton2";
            this.tsbReplace.Click += new System.EventHandler(this.tsbReplace_Click);
            // 
            // tsbGoTo
            // 
            this.tsbGoTo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGoTo.Image = global::PgMulti.Properties.Resources.goto_line;
            this.tsbGoTo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGoTo.Name = "tsbGoTo";
            this.tsbGoTo.Size = new System.Drawing.Size(34, 34);
            this.tsbGoTo.Text = "toolStripButton3";
            this.tsbGoTo.Click += new System.EventHandler(this.tsbGoTo_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbFormat
            // 
            this.tsbFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFormat.Image = global::PgMulti.Properties.Resources.autoformato;
            this.tsbFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFormat.Name = "tsbFormat";
            this.tsbFormat.Size = new System.Drawing.Size(34, 34);
            this.tsbFormat.Text = "toolStripButton4";
            this.tsbFormat.Click += new System.EventHandler(this.tsbFormat_Click);
            // 
            // tsbHistory
            // 
            this.tsbHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHistory.Image = global::PgMulti.Properties.Resources.historial;
            this.tsbHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHistory.Name = "tsbHistory";
            this.tsbHistory.Size = new System.Drawing.Size(34, 34);
            this.tsbHistory.Click += new System.EventHandler(this.tsbHistory_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbOpenDiagram
            // 
            this.tsbOpenDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenDiagram.Image = global::PgMulti.Properties.Resources.open_diagram;
            this.tsbOpenDiagram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenDiagram.Name = "tsbOpenDiagram";
            this.tsbOpenDiagram.Size = new System.Drawing.Size(34, 34);
            this.tsbOpenDiagram.Text = "toolStripButton1";
            this.tsbOpenDiagram.Click += new System.EventHandler(this.tsbOpenDiagram_Click);
            // 
            // tsbNewDiagram
            // 
            this.tsbNewDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewDiagram.Image = global::PgMulti.Properties.Resources.new_diagram;
            this.tsbNewDiagram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewDiagram.Name = "tsbNewDiagram";
            this.tsbNewDiagram.Size = new System.Drawing.Size(34, 34);
            this.tsbNewDiagram.Text = "toolStripButton1";
            this.tsbNewDiagram.Click += new System.EventHandler(this.tsbNewDiagram_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            // 
            // tsddbErrors
            // 
            this.tsddbErrors.Image = global::PgMulti.Properties.Resources.ok;
            this.tsddbErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbErrors.Name = "tsddbErrors";
            this.tsddbErrors.Size = new System.Drawing.Size(44, 34);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 37);
            // 
            // tslPosition
            // 
            this.tslPosition.Name = "tslPosition";
            this.tslPosition.Size = new System.Drawing.Size(0, 34);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.toolStripContainer3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tcResult);
            this.splitContainer3.Size = new System.Drawing.Size(1539, 203);
            this.splitContainer3.SplitterDistance = 505;
            this.splitContainer3.TabIndex = 0;
            // 
            // toolStripContainer3
            // 
            // 
            // toolStripContainer3.ContentPanel
            // 
            this.toolStripContainer3.ContentPanel.Controls.Add(this.lbResult);
            this.toolStripContainer3.ContentPanel.Size = new System.Drawing.Size(505, 166);
            this.toolStripContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer3.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer3.Name = "toolStripContainer3";
            this.toolStripContainer3.Size = new System.Drawing.Size(505, 203);
            this.toolStripContainer3.TabIndex = 1;
            this.toolStripContainer3.Text = "toolStripContainer3";
            // 
            // toolStripContainer3.TopToolStripPanel
            // 
            this.toolStripContainer3.TopToolStripPanel.Controls.Add(this.toolStrip3);
            // 
            // lbResult
            // 
            this.lbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbResult.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbResult.FormattingEnabled = true;
            this.lbResult.ItemHeight = 20;
            this.lbResult.Location = new System.Drawing.Point(0, 0);
            this.lbResult.Name = "lbResult";
            this.lbResult.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbResult.Size = new System.Drawing.Size(505, 166);
            this.lbResult.TabIndex = 0;
            this.lbResult.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbResult_DrawItem);
            this.lbResult.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbResult_MeasureItem);
            this.lbResult.SelectedIndexChanged += new System.EventHandler(this.lbResult_SelectedIndexChanged);
            this.lbResult.Resize += new System.EventHandler(this.lbResult_Resize);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCurrentTabLastTask,
            this.tssResult,
            this.tsbRemoveSelected,
            this.tsbRemoveAll,
            this.tsbStopSelected,
            this.tsbStopAll});
            this.toolStrip3.Location = new System.Drawing.Point(4, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(53, 37);
            this.toolStrip3.TabIndex = 0;
            // 
            // tsbCurrentTabLastTask
            // 
            this.tsbCurrentTabLastTask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCurrentTabLastTask.Image = global::PgMulti.Properties.Resources.last;
            this.tsbCurrentTabLastTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCurrentTabLastTask.Name = "tsbCurrentTabLastTask";
            this.tsbCurrentTabLastTask.Size = new System.Drawing.Size(34, 34);
            this.tsbCurrentTabLastTask.Click += new System.EventHandler(this.tsbCurrentTabLastTask_Click);
            // 
            // tssResult
            // 
            this.tssResult.Name = "tssResult";
            this.tssResult.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbRemoveSelected
            // 
            this.tsbRemoveSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveSelected.Image = global::PgMulti.Properties.Resources.borrar;
            this.tsbRemoveSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveSelected.Name = "tsbRemoveSelected";
            this.tsbRemoveSelected.Size = new System.Drawing.Size(34, 34);
            this.tsbRemoveSelected.Visible = false;
            this.tsbRemoveSelected.Click += new System.EventHandler(this.tsbRemoveSelected_Click);
            // 
            // tsbRemoveAll
            // 
            this.tsbRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveAll.Image = global::PgMulti.Properties.Resources.borrar_todos;
            this.tsbRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveAll.Name = "tsbRemoveAll";
            this.tsbRemoveAll.Size = new System.Drawing.Size(34, 34);
            this.tsbRemoveAll.Visible = false;
            this.tsbRemoveAll.Click += new System.EventHandler(this.tsbRemoveAll_Click);
            // 
            // tsbStopSelected
            // 
            this.tsbStopSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStopSelected.Image = global::PgMulti.Properties.Resources.detener;
            this.tsbStopSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStopSelected.Name = "tsbStopSelected";
            this.tsbStopSelected.Size = new System.Drawing.Size(34, 34);
            this.tsbStopSelected.Visible = false;
            this.tsbStopSelected.Click += new System.EventHandler(this.tsbStopSelected_Click);
            // 
            // tsbStopAll
            // 
            this.tsbStopAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStopAll.Image = global::PgMulti.Properties.Resources.detener_todos;
            this.tsbStopAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStopAll.Name = "tsbStopAll";
            this.tsbStopAll.Size = new System.Drawing.Size(34, 34);
            this.tsbStopAll.Visible = false;
            this.tsbStopAll.Click += new System.EventHandler(this.tsbDetenerTodos_Click);
            // 
            // tcResult
            // 
            this.tcResult.Controls.Add(this.tpResult);
            this.tcResult.Controls.Add(this.tpTable);
            this.tcResult.Controls.Add(this.tpExecutedSql);
            this.tcResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcResult.Location = new System.Drawing.Point(0, 0);
            this.tcResult.Name = "tcResult";
            this.tcResult.SelectedIndex = 0;
            this.tcResult.Size = new System.Drawing.Size(1030, 203);
            this.tcResult.TabIndex = 0;
            // 
            // tpResult
            // 
            this.tpResult.Controls.Add(this.toolStripContainer6);
            this.tpResult.Location = new System.Drawing.Point(4, 29);
            this.tpResult.Name = "tpResult";
            this.tpResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpResult.Size = new System.Drawing.Size(1022, 170);
            this.tpResult.TabIndex = 0;
            this.tpResult.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer6
            // 
            // 
            // toolStripContainer6.ContentPanel
            // 
            this.toolStripContainer6.ContentPanel.Controls.Add(this.txtResult);
            this.toolStripContainer6.ContentPanel.Size = new System.Drawing.Size(1016, 137);
            this.toolStripContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer6.Location = new System.Drawing.Point(3, 3);
            this.toolStripContainer6.Name = "toolStripContainer6";
            this.toolStripContainer6.Size = new System.Drawing.Size(1016, 164);
            this.toolStripContainer6.TabIndex = 1;
            this.toolStripContainer6.Text = "toolStripContainer6";
            // 
            // toolStripContainer6.TopToolStripPanel
            // 
            this.toolStripContainer6.TopToolStripPanel.Controls.Add(this.toolStrip6);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.White;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(1016, 137);
            this.txtResult.TabIndex = 0;
            // 
            // toolStrip6
            // 
            this.toolStrip6.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbAutoScroll});
            this.toolStrip6.Location = new System.Drawing.Point(4, 0);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Size = new System.Drawing.Size(47, 27);
            this.toolStrip6.TabIndex = 0;
            // 
            // tsddbAutoScroll
            // 
            this.tsddbAutoScroll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAutoScroll,
            this.tsmiManualScroll});
            this.tsddbAutoScroll.Image = global::PgMulti.Properties.Resources.scroll_auto;
            this.tsddbAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbAutoScroll.Name = "tsddbAutoScroll";
            this.tsddbAutoScroll.Size = new System.Drawing.Size(34, 24);
            this.tsddbAutoScroll.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddbAutoScroll_DropDownItemClicked);
            // 
            // tsmiAutoScroll
            // 
            this.tsmiAutoScroll.Image = global::PgMulti.Properties.Resources.scroll_auto;
            this.tsmiAutoScroll.Name = "tsmiAutoScroll";
            this.tsmiAutoScroll.Size = new System.Drawing.Size(83, 26);
            this.tsmiAutoScroll.Tag = "auto";
            // 
            // tsmiManualScroll
            // 
            this.tsmiManualScroll.Image = global::PgMulti.Properties.Resources.scroll;
            this.tsmiManualScroll.Name = "tsmiManualScroll";
            this.tsmiManualScroll.Size = new System.Drawing.Size(83, 26);
            this.tsmiManualScroll.Tag = "manual";
            // 
            // tpTable
            // 
            this.tpTable.Controls.Add(this.toolStripContainer5);
            this.tpTable.Location = new System.Drawing.Point(4, 29);
            this.tpTable.Name = "tpTable";
            this.tpTable.Padding = new System.Windows.Forms.Padding(3);
            this.tpTable.Size = new System.Drawing.Size(1022, 170);
            this.tpTable.TabIndex = 1;
            this.tpTable.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer5
            // 
            // 
            // toolStripContainer5.ContentPanel
            // 
            this.toolStripContainer5.ContentPanel.Controls.Add(this.gvTable);
            this.toolStripContainer5.ContentPanel.Size = new System.Drawing.Size(1016, 137);
            this.toolStripContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer5.Location = new System.Drawing.Point(3, 3);
            this.toolStripContainer5.Name = "toolStripContainer5";
            this.toolStripContainer5.Size = new System.Drawing.Size(1016, 164);
            this.toolStripContainer5.TabIndex = 1;
            this.toolStripContainer5.Text = "toolStripContainer5";
            // 
            // toolStripContainer5.TopToolStripPanel
            // 
            this.toolStripContainer5.TopToolStripPanel.Controls.Add(this.toolStrip5);
            // 
            // gvTable
            // 
            this.gvTable.AllowUserToAddRows = false;
            this.gvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTable.ContextMenuStrip = this.cmsTable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.NullValue = "[NULL]";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTable.Location = new System.Drawing.Point(0, 0);
            this.gvTable.Name = "gvTable";
            this.gvTable.RowHeadersWidth = 51;
            this.gvTable.RowTemplate.Height = 29;
            this.gvTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gvTable.ShowCellErrors = false;
            this.gvTable.ShowEditingIcon = false;
            this.gvTable.ShowRowErrors = false;
            this.gvTable.Size = new System.Drawing.Size(1016, 137);
            this.gvTable.TabIndex = 0;
            this.gvTable.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.gvTable_CellToolTipTextNeeded);
            this.gvTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvTable_CellMouseDown);
            this.gvTable.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvTable_CellMouseUp);
            this.gvTable.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gvTable_CellPainting);
            this.gvTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvTable_CellValueChanged);
            this.gvTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvTable_DataError);
            this.gvTable.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.gvTable_RowsRemoved);
            // 
            // cmsTable
            // 
            this.cmsTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbTextEditor,
            this.tsbSetNull});
            this.cmsTable.Name = "cmsTable";
            this.cmsTable.Size = new System.Drawing.Size(74, 56);
            // 
            // tsbTextEditor
            // 
            this.tsbTextEditor.Image = global::PgMulti.Properties.Resources.editar;
            this.tsbTextEditor.Name = "tsbTextEditor";
            this.tsbTextEditor.Size = new System.Drawing.Size(73, 26);
            this.tsbTextEditor.Click += new System.EventHandler(this.tsbTextEditor_Click);
            // 
            // tsbSetNull
            // 
            this.tsbSetNull.Image = global::PgMulti.Properties.Resources._null;
            this.tsbSetNull.Name = "tsbSetNull";
            this.tsbSetNull.Size = new System.Drawing.Size(73, 26);
            this.tsbSetNull.Click += new System.EventHandler(this.tsbSetNull_Click);
            // 
            // toolStrip5
            // 
            this.toolStrip5.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbTables,
            this.tsddbInsertRow,
            this.tsbDeleteRows,
            this.tsbApplyTableChanges});
            this.toolStrip5.Location = new System.Drawing.Point(4, 0);
            this.toolStrip5.Name = "toolStrip5";
            this.toolStrip5.Size = new System.Drawing.Size(139, 27);
            this.toolStrip5.TabIndex = 0;
            // 
            // tsddbTables
            // 
            this.tsddbTables.Image = global::PgMulti.Properties.Resources.tabla;
            this.tsddbTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbTables.Name = "tsddbTables";
            this.tsddbTables.Size = new System.Drawing.Size(34, 24);
            this.tsddbTables.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddbTables_DropDownItemClicked);
            // 
            // tsddbInsertRow
            // 
            this.tsddbInsertRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbInsertRow.Image = global::PgMulti.Properties.Resources.nuevo;
            this.tsddbInsertRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbInsertRow.Name = "tsddbInsertRow";
            this.tsddbInsertRow.Size = new System.Drawing.Size(34, 24);
            this.tsddbInsertRow.Click += new System.EventHandler(this.tsddbInsertRow_Click);
            // 
            // tsbDeleteRows
            // 
            this.tsbDeleteRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDeleteRows.Image = global::PgMulti.Properties.Resources.borrar;
            this.tsbDeleteRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteRows.Name = "tsbDeleteRows";
            this.tsbDeleteRows.Size = new System.Drawing.Size(29, 24);
            this.tsbDeleteRows.Click += new System.EventHandler(this.tsbDeleteRows_Click);
            // 
            // tsbApplyTableChanges
            // 
            this.tsbApplyTableChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbApplyTableChanges.Image = global::PgMulti.Properties.Resources.ok;
            this.tsbApplyTableChanges.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbApplyTableChanges.Name = "tsbApplyTableChanges";
            this.tsbApplyTableChanges.Size = new System.Drawing.Size(29, 24);
            this.tsbApplyTableChanges.Click += new System.EventHandler(this.tsbApplyTableChanges_Click);
            // 
            // tpExecutedSql
            // 
            this.tpExecutedSql.Controls.Add(this.toolStripContainer4);
            this.tpExecutedSql.Location = new System.Drawing.Point(4, 29);
            this.tpExecutedSql.Name = "tpExecutedSql";
            this.tpExecutedSql.Size = new System.Drawing.Size(1022, 170);
            this.tpExecutedSql.TabIndex = 2;
            this.tpExecutedSql.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer4
            // 
            // 
            // toolStripContainer4.ContentPanel
            // 
            this.toolStripContainer4.ContentPanel.Controls.Add(this.fctbExecutedSql);
            this.toolStripContainer4.ContentPanel.Size = new System.Drawing.Size(1022, 133);
            this.toolStripContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer4.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer4.Name = "toolStripContainer4";
            this.toolStripContainer4.Size = new System.Drawing.Size(1022, 170);
            this.toolStripContainer4.TabIndex = 1;
            this.toolStripContainer4.Text = "toolStripContainer4";
            // 
            // toolStripContainer4.TopToolStripPanel
            // 
            this.toolStripContainer4.TopToolStripPanel.Controls.Add(this.toolStrip4);
            // 
            // fctbExecutedSql
            // 
            this.fctbExecutedSql.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctbExecutedSql.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(" +
    "?<range>:)\\s*(?<range>[^;]+);";
            this.fctbExecutedSql.AutoScrollMinSize = new System.Drawing.Size(42, 58);
            this.fctbExecutedSql.BackBrush = null;
            this.fctbExecutedSql.CharHeight = 18;
            this.fctbExecutedSql.CharWidth = 10;
            this.fctbExecutedSql.DefaultMarkerSize = 8;
            this.fctbExecutedSql.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbExecutedSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctbExecutedSql.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fctbExecutedSql.Hotkeys = resources.GetString("fctbExecutedSql.Hotkeys");
            this.fctbExecutedSql.IsReplaceMode = false;
            this.fctbExecutedSql.Location = new System.Drawing.Point(0, 0);
            this.fctbExecutedSql.Name = "fctbExecutedSql";
            this.fctbExecutedSql.Paddings = new System.Windows.Forms.Padding(20);
            this.fctbExecutedSql.ReadOnly = true;
            this.fctbExecutedSql.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbExecutedSql.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbExecutedSql.ServiceColors")));
            this.fctbExecutedSql.Size = new System.Drawing.Size(1022, 133);
            this.fctbExecutedSql.TabIndex = 0;
            this.fctbExecutedSql.Zoom = 100;
            this.fctbExecutedSql.SecondaryFormShowed += new System.EventHandler<FastColoredTextBoxNS.SecondaryFormEventArgs>(this.fctbExecutedSql_SecondaryFormShowed);
            this.fctbExecutedSql.SecondaryFormClosed += new System.EventHandler<FastColoredTextBoxNS.SecondaryFormEventArgs>(this.fctbExecutedSql_SecondaryFormClosed);
            // 
            // toolStrip4
            // 
            this.toolStrip4.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEditExecutedSql});
            this.toolStrip4.Location = new System.Drawing.Point(4, 0);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(47, 37);
            this.toolStrip4.TabIndex = 0;
            // 
            // tsbEditExecutedSql
            // 
            this.tsbEditExecutedSql.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditExecutedSql.Image = global::PgMulti.Properties.Resources.editar;
            this.tsbEditExecutedSql.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditExecutedSql.Name = "tsbEditExecutedSql";
            this.tsbEditExecutedSql.Size = new System.Drawing.Size(34, 34);
            this.tsbEditExecutedSql.Click += new System.EventHandler(this.tsbEditExecutedSql_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Image = global::PgMulti.Properties.Resources.about;
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(224, 26);
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // tsmiNew
            // 
            this.tsmiNew.Image = global::PgMulti.Properties.Resources.nuevo_documento;
            this.tsmiNew.Name = "tsmiNew";
            this.tsmiNew.Size = new System.Drawing.Size(224, 26);
            this.tsmiNew.Click += new System.EventHandler(this.tsmiNew_Click);
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Image = global::PgMulti.Properties.Resources.abrir;
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(224, 26);
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Image = global::PgMulti.Properties.Resources.guardar;
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new System.Drawing.Size(224, 26);
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Image = global::PgMulti.Properties.Resources.guardar_como;
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.Size = new System.Drawing.Size(224, 26);
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // tsmiSaveAll
            // 
            this.tsmiSaveAll.Image = global::PgMulti.Properties.Resources.guardar_todo;
            this.tsmiSaveAll.Name = "tsmiSaveAll";
            this.tsmiSaveAll.Size = new System.Drawing.Size(224, 26);
            this.tsmiSaveAll.Click += new System.EventHandler(this.tsmiSaveAll_Click);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Image = global::PgMulti.Properties.Resources.cerrar;
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(224, 26);
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // tsmiCloseAll
            // 
            this.tsmiCloseAll.Image = global::PgMulti.Properties.Resources.cerrar_todo;
            this.tsmiCloseAll.Name = "tsmiCloseAll";
            this.tsmiCloseAll.Size = new System.Drawing.Size(224, 26);
            this.tsmiCloseAll.Click += new System.EventHandler(this.tsmiCloseAll_Click);
            // 
            // tsmiBack
            // 
            this.tsmiBack.Image = global::PgMulti.Properties.Resources.atras;
            this.tsmiBack.Name = "tsmiBack";
            this.tsmiBack.Size = new System.Drawing.Size(224, 26);
            this.tsmiBack.Click += new System.EventHandler(this.tsmiBack_Click);
            // 
            // tsmiForward
            // 
            this.tsmiForward.Image = global::PgMulti.Properties.Resources.adelante;
            this.tsmiForward.Name = "tsmiForward";
            this.tsmiForward.Size = new System.Drawing.Size(224, 26);
            this.tsmiForward.Click += new System.EventHandler(this.tsmiForward_Click);
            // 
            // tsmiUndo
            // 
            this.tsmiUndo.Image = global::PgMulti.Properties.Resources.undo;
            this.tsmiUndo.Name = "tsmiUndo";
            this.tsmiUndo.Size = new System.Drawing.Size(224, 26);
            this.tsmiUndo.Click += new System.EventHandler(this.tsmiUndo_Click);
            // 
            // tsmiRedo
            // 
            this.tsmiRedo.Image = global::PgMulti.Properties.Resources.redo;
            this.tsmiRedo.Name = "tsmiRedo";
            this.tsmiRedo.Size = new System.Drawing.Size(224, 26);
            this.tsmiRedo.Click += new System.EventHandler(this.tsmiRedo_Click);
            // 
            // tsmiCut
            // 
            this.tsmiCut.Image = global::PgMulti.Properties.Resources.cortar;
            this.tsmiCut.Name = "tsmiCut";
            this.tsmiCut.Size = new System.Drawing.Size(224, 26);
            this.tsmiCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Image = global::PgMulti.Properties.Resources.copiar;
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(224, 26);
            this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiPaste
            // 
            this.tsmiPaste.Image = global::PgMulti.Properties.Resources.pegar;
            this.tsmiPaste.Name = "tsmiPaste";
            this.tsmiPaste.Size = new System.Drawing.Size(224, 26);
            this.tsmiPaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiFind
            // 
            this.tsmiFind.Image = global::PgMulti.Properties.Resources.buscar;
            this.tsmiFind.Name = "tsmiFind";
            this.tsmiFind.Size = new System.Drawing.Size(224, 26);
            this.tsmiFind.Click += new System.EventHandler(this.tsmiFind_Click);
            // 
            // tsmiReplace
            // 
            this.tsmiReplace.Image = global::PgMulti.Properties.Resources.reemplazar;
            this.tsmiReplace.Name = "tsmiReplace";
            this.tsmiReplace.Size = new System.Drawing.Size(224, 26);
            this.tsmiReplace.Click += new System.EventHandler(this.tsmiReplace_Click);
            // 
            // tsmiGoTo
            // 
            this.tsmiGoTo.Image = global::PgMulti.Properties.Resources.linea;
            this.tsmiGoTo.Name = "tsmiGoTo";
            this.tsmiGoTo.Size = new System.Drawing.Size(224, 26);
            this.tsmiGoTo.Click += new System.EventHandler(this.tsmiGoTo_Click);
            // 
            // tsmiFormat
            // 
            this.tsmiFormat.Image = global::PgMulti.Properties.Resources.autoformato;
            this.tsmiFormat.Name = "tsmiFormat";
            this.tsmiFormat.Size = new System.Drawing.Size(224, 26);
            this.tsmiFormat.Click += new System.EventHandler(this.tsmiFormat_Click);
            // 
            // tsmiIncreaseFont
            // 
            this.tsmiIncreaseFont.Image = global::PgMulti.Properties.Resources.zoomin;
            this.tsmiIncreaseFont.Name = "tsmiIncreaseFont";
            this.tsmiIncreaseFont.Size = new System.Drawing.Size(224, 26);
            this.tsmiIncreaseFont.Click += new System.EventHandler(this.tsmiIncreaseFont_Click);
            // 
            // tsmiReduceFont
            // 
            this.tsmiReduceFont.Image = global::PgMulti.Properties.Resources.zoomout;
            this.tsmiReduceFont.Name = "tsmiReduceFont";
            this.tsmiReduceFont.Size = new System.Drawing.Size(224, 26);
            this.tsmiReduceFont.Click += new System.EventHandler(this.tsmiReduceFont_Click);
            // 
            // tsmiChangePassword
            // 
            this.tsmiChangePassword.Image = global::PgMulti.Properties.Resources.password;
            this.tsmiChangePassword.Name = "tsmiChangePassword";
            this.tsmiChangePassword.Size = new System.Drawing.Size(224, 26);
            this.tsmiChangePassword.Click += new System.EventHandler(this.tsmiChangePassword_Click);
            // 
            // tsmiImportConnections
            // 
            this.tsmiImportConnections.Image = global::PgMulti.Properties.Resources.import;
            this.tsmiImportConnections.Name = "tsmiImportConnections";
            this.tsmiImportConnections.Size = new System.Drawing.Size(224, 26);
            this.tsmiImportConnections.Click += new System.EventHandler(this.tsmiImportConnections_Click);
            // 
            // tsmiExportConnections
            // 
            this.tsmiExportConnections.Image = global::PgMulti.Properties.Resources.export;
            this.tsmiExportConnections.Name = "tsmiExportConnections";
            this.tsmiExportConnections.Size = new System.Drawing.Size(224, 26);
            this.tsmiExportConnections.Click += new System.EventHandler(this.tsmiExportConnections_Click);
            // 
            // tsmiMoreOptions
            // 
            this.tsmiMoreOptions.Image = global::PgMulti.Properties.Resources.opciones;
            this.tsmiMoreOptions.Name = "tsmiMoreOptions";
            this.tsmiMoreOptions.Size = new System.Drawing.Size(224, 26);
            this.tsmiMoreOptions.Click += new System.EventHandler(this.tsmMoreOptions_Click);
            // 
            // cmsTabs
            // 
            this.cmsTabs.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseTab,
            this.tsmiCloseAllTabs,
            this.tsmiCloseAllTabsExceptThisOne,
            this.tsmiReopenLastClosedTab,
            this.tsmiClosedTabsLog});
            this.cmsTabs.Name = "cmsTabs";
            this.cmsTabs.Size = new System.Drawing.Size(74, 134);
            // 
            // tsmiCloseTab
            // 
            this.tsmiCloseTab.Image = global::PgMulti.Properties.Resources.cerrar;
            this.tsmiCloseTab.Name = "tsmiCloseTab";
            this.tsmiCloseTab.Size = new System.Drawing.Size(73, 26);
            this.tsmiCloseTab.Click += new System.EventHandler(this.tsmiCloseTab_Click);
            // 
            // tsmiCloseAllTabs
            // 
            this.tsmiCloseAllTabs.Image = global::PgMulti.Properties.Resources.cerrar_todo;
            this.tsmiCloseAllTabs.Name = "tsmiCloseAllTabs";
            this.tsmiCloseAllTabs.Size = new System.Drawing.Size(73, 26);
            this.tsmiCloseAllTabs.Click += new System.EventHandler(this.tsmiCloseAllTabs_Click);
            // 
            // tsmiCloseAllTabsExceptThisOne
            // 
            this.tsmiCloseAllTabsExceptThisOne.Image = global::PgMulti.Properties.Resources.cerrar_todo;
            this.tsmiCloseAllTabsExceptThisOne.Name = "tsmiCloseAllTabsExceptThisOne";
            this.tsmiCloseAllTabsExceptThisOne.Size = new System.Drawing.Size(73, 26);
            this.tsmiCloseAllTabsExceptThisOne.Click += new System.EventHandler(this.tsmiCloseAllTabsExceptThisOne_Click);
            // 
            // tsmiReopenLastClosedTab
            // 
            this.tsmiReopenLastClosedTab.Image = global::PgMulti.Properties.Resources.undo;
            this.tsmiReopenLastClosedTab.Name = "tsmiReopenLastClosedTab";
            this.tsmiReopenLastClosedTab.Size = new System.Drawing.Size(73, 26);
            this.tsmiReopenLastClosedTab.Click += new System.EventHandler(this.tsmiReopenLastClosedTab_Click);
            // 
            // tsmiClosedTabsLog
            // 
            this.tsmiClosedTabsLog.Image = global::PgMulti.Properties.Resources.historial;
            this.tsmiClosedTabsLog.Name = "tsmiClosedTabsLog";
            this.tsmiClosedTabsLog.Size = new System.Drawing.Size(73, 26);
            this.tsmiClosedTabsLog.Click += new System.EventHandler(this.tsmiClosedTabsLog_Click);
            // 
            // tscmiBack
            // 
            this.tscmiBack.Image = global::PgMulti.Properties.Resources.atras;
            this.tscmiBack.Name = "tscmiBack";
            this.tscmiBack.Size = new System.Drawing.Size(73, 26);
            this.tscmiBack.Click += new System.EventHandler(this.tsmiBack_Click);
            // 
            // tscmiForward
            // 
            this.tscmiForward.Image = global::PgMulti.Properties.Resources.adelante;
            this.tscmiForward.Name = "tscmiForward";
            this.tscmiForward.Size = new System.Drawing.Size(73, 26);
            this.tscmiForward.Click += new System.EventHandler(this.tsmiForward_Click);
            // 
            // tscmiUndo
            // 
            this.tscmiUndo.Image = global::PgMulti.Properties.Resources.undo;
            this.tscmiUndo.Name = "tscmiUndo";
            this.tscmiUndo.Size = new System.Drawing.Size(73, 26);
            this.tscmiUndo.Click += new System.EventHandler(this.tsmiUndo_Click);
            // 
            // tscmiRedo
            // 
            this.tscmiRedo.Image = global::PgMulti.Properties.Resources.redo;
            this.tscmiRedo.Name = "tscmiRedo";
            this.tscmiRedo.Size = new System.Drawing.Size(73, 26);
            this.tscmiRedo.Click += new System.EventHandler(this.tsmiRedo_Click);
            // 
            // tscmiCut
            // 
            this.tscmiCut.Image = global::PgMulti.Properties.Resources.cortar;
            this.tscmiCut.Name = "tscmiCut";
            this.tscmiCut.Size = new System.Drawing.Size(73, 26);
            this.tscmiCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tscmiCopy
            // 
            this.tscmiCopy.Image = global::PgMulti.Properties.Resources.copiar;
            this.tscmiCopy.Name = "tscmiCopy";
            this.tscmiCopy.Size = new System.Drawing.Size(73, 26);
            this.tscmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tscmiPaste
            // 
            this.tscmiPaste.Image = global::PgMulti.Properties.Resources.pegar;
            this.tscmiPaste.Name = "tscmiPaste";
            this.tscmiPaste.Size = new System.Drawing.Size(73, 26);
            this.tscmiPaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tscmiFind
            // 
            this.tscmiFind.Image = global::PgMulti.Properties.Resources.buscar;
            this.tscmiFind.Name = "tscmiFind";
            this.tscmiFind.Size = new System.Drawing.Size(73, 26);
            this.tscmiFind.Click += new System.EventHandler(this.tsmiFind_Click);
            // 
            // tscmiReplace
            // 
            this.tscmiReplace.Image = global::PgMulti.Properties.Resources.reemplazar;
            this.tscmiReplace.Name = "tscmiReplace";
            this.tscmiReplace.Size = new System.Drawing.Size(73, 26);
            this.tscmiReplace.Click += new System.EventHandler(this.tsmiReplace_Click);
            // 
            // tscmiGoTo
            // 
            this.tscmiGoTo.Image = global::PgMulti.Properties.Resources.linea;
            this.tscmiGoTo.Name = "tscmiGoTo";
            this.tscmiGoTo.Size = new System.Drawing.Size(73, 26);
            this.tscmiGoTo.Click += new System.EventHandler(this.tsmiGoTo_Click);
            // 
            // tscmiFormat
            // 
            this.tscmiFormat.Image = global::PgMulti.Properties.Resources.autoformato;
            this.tscmiFormat.Name = "tscmiFormat";
            this.tscmiFormat.Size = new System.Drawing.Size(73, 26);
            this.tscmiFormat.Click += new System.EventHandler(this.tsmiFormat_Click);
            // 
            // ilServers
            // 
            this.ilServers.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilServers.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilServers.ImageStream")));
            this.ilServers.TransparentColor = System.Drawing.Color.Transparent;
            this.ilServers.Images.SetKeyName(0, "carpeta.png");
            this.ilServers.Images.SetKeyName(1, "db.png");
            this.ilServers.Images.SetKeyName(2, "nuevo.png");
            // 
            // ofdSql
            // 
            this.ofdSql.FileName = "openFileDialog1";
            this.ofdSql.FilterIndex = 0;
            // 
            // sfdSql
            // 
            this.sfdSql.DefaultExt = "sql";
            this.sfdSql.FilterIndex = 0;
            // 
            // tmrPosition
            // 
            this.tmrPosition.Interval = 500;
            this.tmrPosition.Tick += new System.EventHandler(this.tmrPosition_Tick);
            // 
            // tmrSaveTabs
            // 
            this.tmrSaveTabs.Interval = 2000;
            this.tmrSaveTabs.Tick += new System.EventHandler(this.tmrSaveTabs_Tick);
            // 
            // tmrResult
            // 
            this.tmrResult.Interval = 500;
            this.tmrResult.Tick += new System.EventHandler(this.tmrResult_Tick);
            // 
            // tmrFitGridColumns
            // 
            this.tmrFitGridColumns.Tick += new System.EventHandler(this.tmrFitGridColumns_Tick);
            // 
            // ilAutocompleteMenu
            // 
            this.ilAutocompleteMenu.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilAutocompleteMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilAutocompleteMenu.ImageStream")));
            this.ilAutocompleteMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.ilAutocompleteMenu.Images.SetKeyName(0, "schema.png");
            this.ilAutocompleteMenu.Images.SetKeyName(1, "tva_table.png");
            this.ilAutocompleteMenu.Images.SetKeyName(2, "tva_1n.png");
            this.ilAutocompleteMenu.Images.SetKeyName(3, "tva_n1.png");
            this.ilAutocompleteMenu.Images.SetKeyName(4, "tva_binary.png");
            this.ilAutocompleteMenu.Images.SetKeyName(5, "tva_bool.png");
            this.ilAutocompleteMenu.Images.SetKeyName(6, "tva_date.png");
            this.ilAutocompleteMenu.Images.SetKeyName(7, "tva_dato.png");
            this.ilAutocompleteMenu.Images.SetKeyName(8, "tva_int.png");
            this.ilAutocompleteMenu.Images.SetKeyName(9, "tva_money.png");
            this.ilAutocompleteMenu.Images.SetKeyName(10, "tva_number.png");
            this.ilAutocompleteMenu.Images.SetKeyName(11, "tva_text.png");
            this.ilAutocompleteMenu.Images.SetKeyName(12, "tva_time.png");
            this.ilAutocompleteMenu.Images.SetKeyName(13, "postgresql.png");
            this.ilAutocompleteMenu.Images.SetKeyName(14, "key.png");
            this.ilAutocompleteMenu.Images.SetKeyName(15, "tva_element.png");
            this.ilAutocompleteMenu.Images.SetKeyName(16, "current_fragment.png");
            // 
            // cmsFctb
            // 
            this.cmsFctb.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsFctb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscmiBack,
            this.tscmiForward,
            this.tscmiUndo,
            this.tscmiRedo,
            this.tscmiCut,
            this.tscmiCopy,
            this.tscmiPaste,
            this.tscmiFind,
            this.tscmiReplace,
            this.tscmiGoTo,
            this.tscmiFormat});
            this.cmsFctb.Name = "cmsFctb";
            this.cmsFctb.Size = new System.Drawing.Size(74, 290);
            // 
            // sfdCsv
            // 
            this.sfdCsv.DefaultExt = "csv";
            this.sfdCsv.FilterIndex = 0;
            // 
            // mm
            // 
            this.mm.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiEdit,
            this.tsmiRunMenu,
            this.tsmiDiagrams,
            this.tsmiOptions,
            this.tsmiUpdates});
            this.mm.Location = new System.Drawing.Point(0, 0);
            this.mm.Name = "mm";
            this.mm.Size = new System.Drawing.Size(1539, 28);
            this.mm.TabIndex = 4;
            this.mm.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNew,
            this.tsmiOpen,
            this.tsmiSave,
            this.tsmiSaveAs,
            this.tsmiSaveAll,
            this.tsmiClose,
            this.tsmiCloseAll,
            this.tsmiAbout});
            this.tsmiFile.Image = global::PgMulti.Properties.Resources.archivo;
            this.tsmiFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiFile.Size = new System.Drawing.Size(93, 24);
            this.tsmiFile.Text = "Archivo";
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBack,
            this.tsmiForward,
            this.tsmiUndo,
            this.tsmiRedo,
            this.tsmiCut,
            this.tsmiCopy,
            this.tsmiPaste,
            this.tsmiFind,
            this.tsmiReplace,
            this.tsmiGoTo,
            this.tsmiFormat});
            this.tsmiEdit.Image = global::PgMulti.Properties.Resources.editar;
            this.tsmiEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiEdit.Size = new System.Drawing.Size(82, 24);
            this.tsmiEdit.Text = "Editar";
            // 
            // tsmiRunMenu
            // 
            this.tsmiRunMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRun,
            this.tsmiExportCsv});
            this.tsmiRunMenu.Image = global::PgMulti.Properties.Resources.ejecutar;
            this.tsmiRunMenu.Name = "tsmiRunMenu";
            this.tsmiRunMenu.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiRunMenu.Size = new System.Drawing.Size(68, 24);
            this.tsmiRunMenu.Text = "Run";
            // 
            // tsmiRun
            // 
            this.tsmiRun.Image = global::PgMulti.Properties.Resources.ejecutar;
            this.tsmiRun.Name = "tsmiRun";
            this.tsmiRun.Size = new System.Drawing.Size(263, 26);
            this.tsmiRun.Text = "Run";
            this.tsmiRun.Click += new System.EventHandler(this.tsmiRun_Click);
            // 
            // tsmiExportCsv
            // 
            this.tsmiExportCsv.Image = global::PgMulti.Properties.Resources.download;
            this.tsmiExportCsv.Name = "tsmiExportCsv";
            this.tsmiExportCsv.Size = new System.Drawing.Size(263, 26);
            this.tsmiExportCsv.Text = "Run and download results";
            this.tsmiExportCsv.Click += new System.EventHandler(this.tsmiExportCsv_Click);
            // 
            // tsmiDiagrams
            // 
            this.tsmiDiagrams.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewDiagram,
            this.tsmiOpenDiagram});
            this.tsmiDiagrams.Image = global::PgMulti.Properties.Resources.diagram;
            this.tsmiDiagrams.Name = "tsmiDiagrams";
            this.tsmiDiagrams.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiDiagrams.Size = new System.Drawing.Size(115, 24);
            this.tsmiDiagrams.Text = "Diagramas";
            // 
            // tsmiNewDiagram
            // 
            this.tsmiNewDiagram.Image = global::PgMulti.Properties.Resources.new_diagram;
            this.tsmiNewDiagram.Name = "tsmiNewDiagram";
            this.tsmiNewDiagram.Size = new System.Drawing.Size(224, 26);
            this.tsmiNewDiagram.Text = "Nuevo diagrama";
            this.tsmiNewDiagram.Click += new System.EventHandler(this.tsmiNewDiagram_Click);
            // 
            // tsmiOpenDiagram
            // 
            this.tsmiOpenDiagram.Image = global::PgMulti.Properties.Resources.open_diagram;
            this.tsmiOpenDiagram.Name = "tsmiOpenDiagram";
            this.tsmiOpenDiagram.Size = new System.Drawing.Size(224, 26);
            this.tsmiOpenDiagram.Text = "Abrir diagrama";
            this.tsmiOpenDiagram.Click += new System.EventHandler(this.tsmiOpenDiagram_Click);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiIncreaseFont,
            this.tsmiReduceFont,
            this.tsmiHistory,
            this.tsmiChangePassword,
            this.tsmiExportConnections,
            this.tsmiImportConnections,
            this.tsmiMoreOptions});
            this.tsmiOptions.Image = global::PgMulti.Properties.Resources.opciones;
            this.tsmiOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiOptions.Size = new System.Drawing.Size(105, 24);
            this.tsmiOptions.Text = "Opciones";
            // 
            // tsmiHistory
            // 
            this.tsmiHistory.Image = global::PgMulti.Properties.Resources.historial;
            this.tsmiHistory.Name = "tsmiHistory";
            this.tsmiHistory.Size = new System.Drawing.Size(224, 26);
            this.tsmiHistory.Click += new System.EventHandler(this.tsmiHistory_Click);
            // 
            // tsmiUpdates
            // 
            this.tsmiUpdates.Image = global::PgMulti.Properties.Resources.updates;
            this.tsmiUpdates.Name = "tsmiUpdates";
            this.tsmiUpdates.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiUpdates.Size = new System.Drawing.Size(191, 24);
            this.tsmiUpdates.Text = "Buscar actualizaciones";
            this.tsmiUpdates.Visible = false;
            this.tsmiUpdates.Click += new System.EventHandler(this.tsmiUpdates_Click);
            // 
            // ofdImportConfig
            // 
            this.ofdImportConfig.FileName = "openFileDialog1";
            this.ofdImportConfig.FilterIndex = 0;
            // 
            // ofdOpenDiagram
            // 
            this.ofdOpenDiagram.FileName = "openFileDialog1";
            this.ofdOpenDiagram.FilterIndex = 0;
            // 
            // sfdSaveDiagram
            // 
            this.sfdSaveDiagram.DefaultExt = "csv";
            this.sfdSaveDiagram.FilterIndex = 0;
            // 
            // tmrReenableRunButton
            // 
            this.tmrReenableRunButton.Interval = 2000;
            this.tmrReenableRunButton.Tick += new System.EventHandler(this.tmrReenableRunButton_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1539, 530);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mm;
            this.Name = "MainForm";
            this.Text = "pgMulti";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.cmsServers.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tcSql.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStripContainer3.ContentPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer3.TopToolStripPanel.PerformLayout();
            this.toolStripContainer3.ResumeLayout(false);
            this.toolStripContainer3.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tcResult.ResumeLayout(false);
            this.tpResult.ResumeLayout(false);
            this.toolStripContainer6.ContentPanel.ResumeLayout(false);
            this.toolStripContainer6.ContentPanel.PerformLayout();
            this.toolStripContainer6.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer6.TopToolStripPanel.PerformLayout();
            this.toolStripContainer6.ResumeLayout(false);
            this.toolStripContainer6.PerformLayout();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.tpTable.ResumeLayout(false);
            this.toolStripContainer5.ContentPanel.ResumeLayout(false);
            this.toolStripContainer5.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer5.TopToolStripPanel.PerformLayout();
            this.toolStripContainer5.ResumeLayout(false);
            this.toolStripContainer5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTable)).EndInit();
            this.cmsTable.ResumeLayout(false);
            this.toolStrip5.ResumeLayout(false);
            this.toolStrip5.PerformLayout();
            this.tpExecutedSql.ResumeLayout(false);
            this.toolStripContainer4.ContentPanel.ResumeLayout(false);
            this.toolStripContainer4.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer4.TopToolStripPanel.PerformLayout();
            this.toolStripContainer4.ResumeLayout(false);
            this.toolStripContainer4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbExecutedSql)).EndInit();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.cmsTabs.ResumeLayout(false);
            this.cmsFctb.ResumeLayout(false);
            this.mm.ResumeLayout(false);
            this.mm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Aga.Controls.Tree.TreeViewAdv tvaConnections;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox ncb;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon nsi;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ntb;
        private ToolStrip toolStrip1;
        private ToolStripContainer toolStripContainer1;
        private ImageList ilServers;
        private SplitContainer splitContainer3;
        private ListBox lbResult;
        private TabControl tcResult;
        private TabPage tpResult;
        private TabPage tpTable;
        private ToolStripButton tsbRun;
        private ToolStripButton tsbHistory;
        private ToolStripContainer toolStripContainer2;
        private ToolStrip toolStrip2;
        private ToolStripButton tsbNewGroup;
        private ToolStripButton tsbNewDB;
        private ToolStripButton tsbEdit;
        private ToolStripButton tsbRemove;
        private ToolStripSeparator tssNew;
        private ToolStripSeparator tssEdit;
        private ToolStripButton tsbUp;
        private ToolStripButton tsbDown;
        private OpenFileDialog ofdSql;
        private SaveFileDialog sfdSql;
        private TabControlExtra tcSql;
        private TextBox txtResult;
        private DataGridView gvTable;
        private ToolStripContainer toolStripContainer3;
        private ToolStrip toolStrip3;
        private ToolStripButton tsbRemoveSelected;
        private ToolStripButton tsbRemoveAll;
        private ToolStripButton tsbStopSelected;
        private ToolStripButton tsbStopAll;
        private ToolStripLabel tslPosition;
        private System.Windows.Forms.Timer tmrPosition;
        private System.Windows.Forms.Timer tmrResult;
        private System.Windows.Forms.Timer tmrSaveTabs;
        private TabPage tpExecutedSql;
        private CustomFctb fctbExecutedSql;
        private ToolStripContainer toolStripContainer4;
        private ToolStrip toolStrip4;
        private ToolStripButton tsbEditExecutedSql;
        private System.Windows.Forms.Timer tmrFitGridColumns;
        private ToolStripContainer toolStripContainer5;
        private ToolStrip toolStrip5;
        private ToolStripDropDownButton tsddbTables;
        private ToolStripContainer toolStripContainer6;
        private ToolStrip toolStrip6;
        private ToolStripDropDownButton tsddbAutoScroll;
        private ToolStripMenuItem tsmiAutoScroll;
        private ToolStripMenuItem tsmiManualScroll;
        private ToolStripSeparator tssUpDown;
        private ToolStripDropDownButton tsddbErrors;
        internal ImageList ilAutocompleteMenu;
        private ToolStripButton tsbRefresh;
        private ToolStripMenuItem tsmiFind;
        private ToolStripMenuItem tsmiReplace;
        private ToolStripMenuItem tsmiGoTo;
        private ToolStripMenuItem tsmiCut;
        private ToolStripMenuItem tsmiCopy;
        private ToolStripMenuItem tsmiPaste;
        private ToolStripMenuItem tsmiFormat;
        private ToolStripMenuItem tsmiNew;
        private ToolStripMenuItem tsmiOpen;
        private ToolStripMenuItem tsmiSave;
        private ToolStripMenuItem tsmiSaveAs;
        private ToolStripMenuItem tsmiSaveAll;
        private ToolStripMenuItem tsmiClose;
        private ToolStripMenuItem tsmiCloseAll;
        private ToolStripMenuItem tsmiChangePassword;
        private ToolStripMenuItem tsmiUndo;
        private ToolStripMenuItem tsmiRedo;
        private ToolStripMenuItem tsmiBack;
        private ToolStripMenuItem tsmiForward;
        private ToolStripMenuItem tscmiBack;
        private ToolStripMenuItem tscmiForward;
        private ToolStripMenuItem tscmiUndo;
        private ToolStripMenuItem tscmiRedo;
        private ToolStripMenuItem tscmiCut;
        private ToolStripMenuItem tscmiCopy;
        private ToolStripMenuItem tscmiPaste;
        private ToolStripMenuItem tscmiFind;
        private ToolStripMenuItem tscmiReplace;
        private ToolStripMenuItem tscmiGoTo;
        private ToolStripMenuItem tscmiFormat;
        private ToolStripMenuItem tsmiIncreaseFont;
        private ToolStripMenuItem tsmiReduceFont;
        private ToolStripSeparator toolStripSeparator1;
        private ContextMenuStrip cmsFctb;
        private ToolStripButton tsbApplyTableChanges;
        private ContextMenuStrip cmsTable;
        private ToolStripMenuItem tsbTextEditor;
        private ContextMenuStrip cmsServers;
        private ToolStripMenuItem tscmiNewGroup;
        private ToolStripMenuItem tscmiNewDB;
        private ToolStripMenuItem tscmiExploreTable;
        private ToolStripMenuItem tscmiRecursiveRemove;
        private ToolStripMenuItem tscmiCreateTableDiagram;
        private ToolStripMenuItem tscmiCopyText;
        private ToolStripMenuItem tscmiEdit;
        private ToolStripMenuItem tscmiRemove;
        private ToolStripMenuItem tscmiUp;
        private ToolStripMenuItem tscmiDown;
        private ToolStripMenuItem tscmiRefresh;
        private ToolStripButton tsbCollapseAll;
        private ToolStripMenuItem tsbSetNull;
        private ToolStripDropDownButton tsddbInsertRow;
        private ToolStripButton tsbDeleteRows;
        private ToolStripMenuItem tsmiImportConnections;
        private ToolStripMenuItem tsmiExportConnections;
        private ToolStripMenuItem tsmiMoreOptions;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripButton tsbExploreTable;
        private ToolStripButton tsbRecursiveRemove;
        private TabPage tpNewTab;
        private ImageList ilTabControl;
        private ContextMenuStrip cmsTabs;
        private ToolStripMenuItem tsmiCloseTab;
        private ToolStripMenuItem tsmiCloseAllTabs;
        private ToolStripMenuItem tsmiCloseAllTabsExceptThisOne;
        private ToolStripButton tsbExportCsv;
        private SaveFileDialog sfdCsv;
        private MenuStrip mm;
        private ToolStripMenuItem tsmiFile;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripMenuItem tsmiOptions;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbOpen;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbSaveAll;
        private ToolStripButton tsbFind;
        private ToolStripButton tsbReplace;
        private ToolStripButton tsbGoTo;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsbFormat;
        private ToolStripDropDownButton tsddbTransactions;
        private ToolStripMenuItem tsmiTransactionModeManual;
        private ToolStripMenuItem tsmiTransactionModeAutoSingle;
        private ToolStripMenuItem tsmiTransactionModeAutoCoordinated;
        private ToolStripMenuItem tsmiTransactionLevelReadCommitted;
        private ToolStripMenuItem tsmiTransactionLevelRepeatableRead;
        private ToolStripMenuItem tsmiTransactionLevelSerializable;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem tsmiHistory;
        private ToolStripMenuItem tsmiRunMenu;
        private ToolStripMenuItem tsmiRun;
        private ToolStripMenuItem tsmiExportCsv;
        private ToolStripSeparator toolStripSeparator6;
        private OpenFileDialog ofdImportConfig;
        private ToolStripButton tsbCurrentTabLastTask;
        private ToolStripSeparator tssResult;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton tsbOpenDiagram;
        private ToolStripButton tsbNewDiagram;
        private OpenFileDialog ofdOpenDiagram;
        private SaveFileDialog sfdSaveDiagram;
        private ToolStripMenuItem tsmiDiagrams;
        private ToolStripMenuItem tsmiNewDiagram;
        private ToolStripMenuItem tsmiOpenDiagram;
        private ToolStripMenuItem tsmiClosedTabsLog;
        private System.Windows.Forms.Timer tmrReenableRunButton;
        private ToolStripMenuItem tsmiReopenLastClosedTab;
        private ToolStripButton tsbCreateTableDiagram;
        private ToolStripMenuItem tsmiUpdates;
    }
}
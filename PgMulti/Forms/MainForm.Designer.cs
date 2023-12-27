using FastColoredTextBoxNS;
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            tcLeftPanel = new TabControl();
            tpConnections = new TabPage();
            toolStripContainer2 = new ToolStripContainer();
            tvaConnections = new Aga.Controls.Tree.TreeViewAdv();
            cmsServers = new ContextMenuStrip(components);
            tscmiNewGroup = new ToolStripMenuItem();
            tscmiNewDB = new ToolStripMenuItem();
            tscmiExploreTable = new ToolStripMenuItem();
            tscmiRecursiveRemove = new ToolStripMenuItem();
            tscmiCreateTableDiagram = new ToolStripMenuItem();
            tscmiEdit = new ToolStripMenuItem();
            tscmiRemove = new ToolStripMenuItem();
            tscmiUp = new ToolStripMenuItem();
            tscmiDown = new ToolStripMenuItem();
            tscmiRefresh = new ToolStripMenuItem();
            tscmiCopyText = new ToolStripMenuItem();
            ncb = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            nsi = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            ntb = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            toolStrip2 = new ToolStrip();
            tsbNewGroup = new ToolStripButton();
            tsbNewDB = new ToolStripButton();
            tssNew = new ToolStripSeparator();
            tsbExploreTable = new ToolStripButton();
            tsbRecursiveRemove = new ToolStripButton();
            tsbCreateTableDiagram = new ToolStripButton();
            tsbEdit = new ToolStripButton();
            tsbRemove = new ToolStripButton();
            tssEdit = new ToolStripSeparator();
            tsbUp = new ToolStripButton();
            tsbDown = new ToolStripButton();
            tssUpDown = new ToolStripSeparator();
            tsbCollapseAll = new ToolStripButton();
            tsbRefresh = new ToolStripButton();
            tpSearchAndReplace = new TabPage();
            tlpSearchAndReplace = new TableLayoutPanel();
            lblSearch = new Label();
            txtSearchText = new TextBox();
            flpSearchOptions = new FlowLayoutPanel();
            chkSearchMatchCase = new CheckBox();
            chkSearchMatchWholeWords = new CheckBox();
            chkSearchRegex = new CheckBox();
            chkSearchWithinSelectedText = new CheckBox();
            lblSearchResultsSummary = new Label();
            flpSearchButtons = new FlowLayoutPanel();
            btnSearch = new Button();
            btnGoNextSearchResult = new Button();
            btnUpdateSearchSelectedText = new Button();
            lblReplace = new Label();
            txtReplaceText = new TextBox();
            flpReplaceButtons = new FlowLayoutPanel();
            btnReplaceCurrent = new Button();
            btnReplaceAll = new Button();
            toolStripContainer1 = new ToolStripContainer();
            tcSql = new TabControlExtra();
            tpNewTab = new TabPage();
            ilTabControl = new ImageList(components);
            toolStrip1 = new ToolStrip();
            tsbOpen = new ToolStripButton();
            tsbSave = new ToolStripButton();
            tsbSaveAll = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsbRun = new ToolStripButton();
            tsbExportCsv = new ToolStripButton();
            tsddbTransactions = new ToolStripDropDownButton();
            tsmiTransactionModeManual = new ToolStripMenuItem();
            tsmiTransactionModeAutoSingle = new ToolStripMenuItem();
            tsmiTransactionModeAutoCoordinated = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            tsmiTransactionLevelReadCommitted = new ToolStripMenuItem();
            tsmiTransactionLevelRepeatableRead = new ToolStripMenuItem();
            tsmiTransactionLevelSerializable = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            tsbSearchAndReplace = new ToolStripButton();
            tsbGoTo = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            tsbFormat = new ToolStripButton();
            tsbHistory = new ToolStripButton();
            toolStripSeparator7 = new ToolStripSeparator();
            tsbOpenDiagram = new ToolStripButton();
            tsbNewDiagram = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsddbErrors = new ToolStripDropDownButton();
            toolStripSeparator6 = new ToolStripSeparator();
            tslPosition = new ToolStripLabel();
            splitContainer3 = new SplitContainer();
            toolStripContainer3 = new ToolStripContainer();
            lbResult = new ListBox();
            toolStrip3 = new ToolStrip();
            tsbCurrentTabLastTask = new ToolStripButton();
            tssResult = new ToolStripSeparator();
            tsbRemoveSelected = new ToolStripButton();
            tsbRemoveAll = new ToolStripButton();
            tsbStopSelected = new ToolStripButton();
            tsbStopAll = new ToolStripButton();
            tcResult = new TabControl();
            tpResult = new TabPage();
            toolStripContainer6 = new ToolStripContainer();
            fctbResult = new FastColoredTextBox();
            toolStrip6 = new ToolStrip();
            tsddbAutoScroll = new ToolStripDropDownButton();
            tsmiAutoScroll = new ToolStripMenuItem();
            tsmiManualScroll = new ToolStripMenuItem();
            tpTable = new TabPage();
            toolStripContainer5 = new ToolStripContainer();
            gvTable = new DataGridView();
            cmsTable = new ContextMenuStrip(components);
            tsbCopyCellText = new ToolStripMenuItem();
            tsbLoadCellBinaryValueFromFile = new ToolStripMenuItem();
            tsbSaveCellBinaryValueInFile = new ToolStripMenuItem();
            tsbTextEditor = new ToolStripMenuItem();
            tsbSetNull = new ToolStripMenuItem();
            toolStrip5 = new ToolStrip();
            tsddbTables = new ToolStripDropDownButton();
            tsddbInsertRow = new ToolStripDropDownButton();
            tsbDeleteRows = new ToolStripButton();
            tsbApplyTableChanges = new ToolStripButton();
            tpExecutedSql = new TabPage();
            toolStripContainer4 = new ToolStripContainer();
            fctbExecutedSql = new CustomFctb();
            toolStrip4 = new ToolStrip();
            tsbEditExecutedSql = new ToolStripButton();
            tsmiAbout = new ToolStripMenuItem();
            tsmiNew = new ToolStripMenuItem();
            tsmiOpen = new ToolStripMenuItem();
            tsmiSave = new ToolStripMenuItem();
            tsmiSaveAs = new ToolStripMenuItem();
            tsmiSaveAll = new ToolStripMenuItem();
            tsmiClose = new ToolStripMenuItem();
            tsmiCloseAll = new ToolStripMenuItem();
            tsmiBack = new ToolStripMenuItem();
            tsmiForward = new ToolStripMenuItem();
            tsmiUndo = new ToolStripMenuItem();
            tsmiRedo = new ToolStripMenuItem();
            tsmiCut = new ToolStripMenuItem();
            tsmiCopy = new ToolStripMenuItem();
            tsmiPaste = new ToolStripMenuItem();
            tsmiSearchAndReplace = new ToolStripMenuItem();
            tsmiGoTo = new ToolStripMenuItem();
            tsmiFormat = new ToolStripMenuItem();
            tsmiIncreaseFont = new ToolStripMenuItem();
            tsmiReduceFont = new ToolStripMenuItem();
            tsmiChangePassword = new ToolStripMenuItem();
            tsmiImportConnections = new ToolStripMenuItem();
            tsmiExportConnections = new ToolStripMenuItem();
            tsmiMoreOptions = new ToolStripMenuItem();
            cmsTabs = new ContextMenuStrip(components);
            tsmiCloseTab = new ToolStripMenuItem();
            tsmiCloseAllTabs = new ToolStripMenuItem();
            tsmiCloseAllTabsExceptThisOne = new ToolStripMenuItem();
            tsmiReopenLastClosedTab = new ToolStripMenuItem();
            tsmiClosedTabsLog = new ToolStripMenuItem();
            tsmiCopyPath = new ToolStripMenuItem();
            tsmiOpenFolder = new ToolStripMenuItem();
            tscmiBack = new ToolStripMenuItem();
            tscmiForward = new ToolStripMenuItem();
            tscmiUndo = new ToolStripMenuItem();
            tscmiRedo = new ToolStripMenuItem();
            tscmiCut = new ToolStripMenuItem();
            tscmiCopy = new ToolStripMenuItem();
            tscmiPaste = new ToolStripMenuItem();
            tscmiSearchAndReplace = new ToolStripMenuItem();
            tscmiGoTo = new ToolStripMenuItem();
            tscmiFormat = new ToolStripMenuItem();
            ilServers = new ImageList(components);
            ofdSql = new OpenFileDialog();
            sfdSql = new SaveFileDialog();
            ofdBinaryCell = new OpenFileDialog();
            sfdBinaryCell = new SaveFileDialog();
            tmrPosition = new System.Windows.Forms.Timer(components);
            tmrSaveTabs = new System.Windows.Forms.Timer(components);
            tmrResult = new System.Windows.Forms.Timer(components);
            tmrFitGridColumns = new System.Windows.Forms.Timer(components);
            ilAutocompleteMenu = new ImageList(components);
            cmsFctb = new ContextMenuStrip(components);
            sfdCsv = new SaveFileDialog();
            mm = new MenuStrip();
            tsmiFile = new ToolStripMenuItem();
            tsmiEdit = new ToolStripMenuItem();
            tsmiRunMenu = new ToolStripMenuItem();
            tsmiRun = new ToolStripMenuItem();
            tsmiExportCsv = new ToolStripMenuItem();
            tsmiCopyToTable = new ToolStripMenuItem();
            tsmiDiagrams = new ToolStripMenuItem();
            tsmiNewDiagram = new ToolStripMenuItem();
            tsmiOpenDiagram = new ToolStripMenuItem();
            tsmiOptions = new ToolStripMenuItem();
            tsmiHistory = new ToolStripMenuItem();
            tsmiUpdates = new ToolStripMenuItem();
            ofdImportConfig = new OpenFileDialog();
            ofdOpenDiagram = new OpenFileDialog();
            sfdSaveDiagram = new SaveFileDialog();
            tmrReenableRunButton = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tcLeftPanel.SuspendLayout();
            tpConnections.SuspendLayout();
            toolStripContainer2.ContentPanel.SuspendLayout();
            toolStripContainer2.TopToolStripPanel.SuspendLayout();
            toolStripContainer2.SuspendLayout();
            cmsServers.SuspendLayout();
            toolStrip2.SuspendLayout();
            tpSearchAndReplace.SuspendLayout();
            tlpSearchAndReplace.SuspendLayout();
            flpSearchOptions.SuspendLayout();
            flpSearchButtons.SuspendLayout();
            flpReplaceButtons.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            tcSql.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            toolStripContainer3.ContentPanel.SuspendLayout();
            toolStripContainer3.TopToolStripPanel.SuspendLayout();
            toolStripContainer3.SuspendLayout();
            toolStrip3.SuspendLayout();
            tcResult.SuspendLayout();
            tpResult.SuspendLayout();
            toolStripContainer6.ContentPanel.SuspendLayout();
            toolStripContainer6.TopToolStripPanel.SuspendLayout();
            toolStripContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fctbResult).BeginInit();
            toolStrip6.SuspendLayout();
            tpTable.SuspendLayout();
            toolStripContainer5.ContentPanel.SuspendLayout();
            toolStripContainer5.TopToolStripPanel.SuspendLayout();
            toolStripContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gvTable).BeginInit();
            cmsTable.SuspendLayout();
            toolStrip5.SuspendLayout();
            tpExecutedSql.SuspendLayout();
            toolStripContainer4.TopToolStripPanel.SuspendLayout();
            toolStripContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fctbExecutedSql).BeginInit();
            toolStrip4.SuspendLayout();
            cmsTabs.SuspendLayout();
            cmsFctb.SuspendLayout();
            mm.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 28);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer3);
            splitContainer1.Size = new Size(1539, 1262);
            splitContainer1.SplitterDistance = 740;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tcLeftPanel);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(toolStripContainer1);
            splitContainer2.Size = new Size(1539, 740);
            splitContainer2.SplitterDistance = 410;
            splitContainer2.TabIndex = 0;
            // 
            // tcLeftPanel
            // 
            tcLeftPanel.Alignment = TabAlignment.Bottom;
            tcLeftPanel.Controls.Add(tpConnections);
            tcLeftPanel.Controls.Add(tpSearchAndReplace);
            tcLeftPanel.Dock = DockStyle.Fill;
            tcLeftPanel.Location = new Point(0, 0);
            tcLeftPanel.Name = "tcLeftPanel";
            tcLeftPanel.SelectedIndex = 0;
            tcLeftPanel.Size = new Size(410, 740);
            tcLeftPanel.TabIndex = 2;
            // 
            // tpConnections
            // 
            tpConnections.Controls.Add(toolStripContainer2);
            tpConnections.Location = new Point(4, 4);
            tpConnections.Name = "tpConnections";
            tpConnections.Padding = new Padding(3);
            tpConnections.Size = new Size(402, 707);
            tpConnections.TabIndex = 0;
            tpConnections.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            toolStripContainer2.ContentPanel.Controls.Add(tvaConnections);
            toolStripContainer2.ContentPanel.Size = new Size(396, 664);
            toolStripContainer2.Dock = DockStyle.Fill;
            toolStripContainer2.Location = new Point(3, 3);
            toolStripContainer2.Name = "toolStripContainer2";
            toolStripContainer2.Size = new Size(396, 701);
            toolStripContainer2.TabIndex = 1;
            toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            toolStripContainer2.TopToolStripPanel.Controls.Add(toolStrip2);
            // 
            // tvaConnections
            // 
            tvaConnections.AllowDrop = true;
            tvaConnections.AsyncExpanding = true;
            tvaConnections.AutoRowHeight = true;
            tvaConnections.BackColor = SystemColors.Window;
            tvaConnections.ContextMenuStrip = cmsServers;
            tvaConnections.DefaultToolTipProvider = null;
            tvaConnections.Dock = DockStyle.Fill;
            tvaConnections.DragDropMarkColor = Color.Black;
            tvaConnections.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tvaConnections.Indent = 25;
            tvaConnections.LineColor = SystemColors.ControlDark;
            tvaConnections.LoadOnDemand = true;
            tvaConnections.Location = new Point(0, 0);
            tvaConnections.Margin = new Padding(10);
            tvaConnections.Model = null;
            tvaConnections.Name = "tvaConnections";
            tvaConnections.NodeControls.Add(ncb);
            tvaConnections.NodeControls.Add(nsi);
            tvaConnections.NodeControls.Add(ntb);
            tvaConnections.RowHeight = 25;
            tvaConnections.SelectedNode = null;
            tvaConnections.Size = new Size(396, 664);
            tvaConnections.TabIndex = 0;
            tvaConnections.ItemDrag += tvaConnections_ItemDrag;
            tvaConnections.SelectionChanged += tvaServers_SelectionChanged;
            tvaConnections.DragDrop += tvaConnections_DragDrop;
            tvaConnections.DragOver += tvaConnections_DragOver;
            tvaConnections.DoubleClick += tvaServers_DoubleClick;
            tvaConnections.Enter += tvaConnections_Enter;
            tvaConnections.Leave += tvaConnections_Leave;
            // 
            // cmsServers
            // 
            cmsServers.ImageScalingSize = new Size(20, 20);
            cmsServers.Items.AddRange(new ToolStripItem[] { tscmiNewGroup, tscmiNewDB, tscmiExploreTable, tscmiRecursiveRemove, tscmiCreateTableDiagram, tscmiEdit, tscmiRemove, tscmiUp, tscmiDown, tscmiRefresh, tscmiCopyText });
            cmsServers.Name = "cmsServers";
            cmsServers.Size = new Size(74, 290);
            // 
            // tscmiNewGroup
            // 
            tscmiNewGroup.Image = Properties.Resources.nuevo_grupo;
            tscmiNewGroup.Name = "tscmiNewGroup";
            tscmiNewGroup.Size = new Size(73, 26);
            tscmiNewGroup.Click += tscmiNewGroup_Click;
            // 
            // tscmiNewDB
            // 
            tscmiNewDB.Image = Properties.Resources.nueva_db;
            tscmiNewDB.Name = "tscmiNewDB";
            tscmiNewDB.Size = new Size(73, 26);
            tscmiNewDB.Click += tscmiNewDB_Click;
            // 
            // tscmiExploreTable
            // 
            tscmiExploreTable.Image = Properties.Resources.abrir_tabla;
            tscmiExploreTable.Name = "tscmiExploreTable";
            tscmiExploreTable.Size = new Size(73, 26);
            tscmiExploreTable.Click += tscmiExploreTable_Click;
            // 
            // tscmiRecursiveRemove
            // 
            tscmiRecursiveRemove.Image = Properties.Resources.borrar_todos;
            tscmiRecursiveRemove.Name = "tscmiRecursiveRemove";
            tscmiRecursiveRemove.Size = new Size(73, 26);
            tscmiRecursiveRemove.Click += tscmiRecursiveRemove_Click;
            // 
            // tscmiCreateTableDiagram
            // 
            tscmiCreateTableDiagram.Image = Properties.Resources.diagram;
            tscmiCreateTableDiagram.Name = "tscmiCreateTableDiagram";
            tscmiCreateTableDiagram.Size = new Size(73, 26);
            tscmiCreateTableDiagram.Click += tscmiCreateTableDiagram_Click;
            // 
            // tscmiEdit
            // 
            tscmiEdit.Image = Properties.Resources.editar;
            tscmiEdit.Name = "tscmiEdit";
            tscmiEdit.Size = new Size(73, 26);
            tscmiEdit.Click += tscmiEdit_Click;
            // 
            // tscmiRemove
            // 
            tscmiRemove.Image = Properties.Resources.borrar;
            tscmiRemove.Name = "tscmiRemove";
            tscmiRemove.Size = new Size(73, 26);
            tscmiRemove.Click += tscmiRemove_Click;
            // 
            // tscmiUp
            // 
            tscmiUp.Image = Properties.Resources.arriba;
            tscmiUp.Name = "tscmiUp";
            tscmiUp.Size = new Size(73, 26);
            tscmiUp.Click += tscmiUp_Click;
            // 
            // tscmiDown
            // 
            tscmiDown.Image = Properties.Resources.abajo;
            tscmiDown.Name = "tscmiDown";
            tscmiDown.Size = new Size(73, 26);
            tscmiDown.Click += tscmiDown_Click;
            // 
            // tscmiRefresh
            // 
            tscmiRefresh.Image = Properties.Resources.actualizar;
            tscmiRefresh.Name = "tscmiRefresh";
            tscmiRefresh.Size = new Size(73, 26);
            tscmiRefresh.Click += tscmiRefresh_Click;
            // 
            // tscmiCopyText
            // 
            tscmiCopyText.Image = Properties.Resources.copiar;
            tscmiCopyText.Name = "tscmiCopyText";
            tscmiCopyText.Size = new Size(73, 26);
            tscmiCopyText.Click += tscmiCopyText_Click;
            // 
            // ncb
            // 
            ncb.DataPropertyName = "CheckState";
            ncb.EditEnabled = true;
            ncb.ImageSize = 20;
            ncb.LeftMargin = 5;
            ncb.ParentColumn = null;
            // 
            // nsi
            // 
            nsi.DataPropertyName = "Image";
            nsi.LeftMargin = 5;
            nsi.ParentColumn = null;
            nsi.ScaleMode = Aga.Controls.Tree.ImageScaleMode.AlwaysScale;
            // 
            // ntb
            // 
            ntb.DataPropertyName = "Text";
            ntb.IncrementalSearchEnabled = true;
            ntb.LeftMargin = 5;
            ntb.ParentColumn = null;
            // 
            // toolStrip2
            // 
            toolStrip2.Dock = DockStyle.None;
            toolStrip2.ImageScalingSize = new Size(30, 30);
            toolStrip2.Items.AddRange(new ToolStripItem[] { tsbNewGroup, tsbNewDB, tssNew, tsbExploreTable, tsbRecursiveRemove, tsbCreateTableDiagram, tsbEdit, tsbRemove, tssEdit, tsbUp, tsbDown, tssUpDown, tsbCollapseAll, tsbRefresh });
            toolStrip2.Location = new Point(4, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(81, 37);
            toolStrip2.TabIndex = 0;
            // 
            // tsbNewGroup
            // 
            tsbNewGroup.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbNewGroup.Image = Properties.Resources.nuevo_grupo;
            tsbNewGroup.ImageTransparentColor = Color.Magenta;
            tsbNewGroup.Name = "tsbNewGroup";
            tsbNewGroup.Size = new Size(34, 34);
            tsbNewGroup.Visible = false;
            tsbNewGroup.Click += tsbNewGroup_Click;
            // 
            // tsbNewDB
            // 
            tsbNewDB.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbNewDB.Image = Properties.Resources.nueva_db;
            tsbNewDB.ImageTransparentColor = Color.Magenta;
            tsbNewDB.Name = "tsbNewDB";
            tsbNewDB.Size = new Size(34, 34);
            tsbNewDB.Visible = false;
            tsbNewDB.Click += tsbNewDB_Click;
            // 
            // tssNew
            // 
            tssNew.Name = "tssNew";
            tssNew.Size = new Size(6, 37);
            tssNew.Visible = false;
            // 
            // tsbExploreTable
            // 
            tsbExploreTable.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbExploreTable.Image = Properties.Resources.abrir_tabla;
            tsbExploreTable.ImageTransparentColor = Color.Magenta;
            tsbExploreTable.Name = "tsbExploreTable";
            tsbExploreTable.Size = new Size(34, 34);
            tsbExploreTable.Visible = false;
            tsbExploreTable.Click += tsbExploreTable_Click;
            // 
            // tsbRecursiveRemove
            // 
            tsbRecursiveRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRecursiveRemove.Image = Properties.Resources.borrar_todos;
            tsbRecursiveRemove.ImageTransparentColor = Color.Magenta;
            tsbRecursiveRemove.Name = "tsbRecursiveRemove";
            tsbRecursiveRemove.Size = new Size(34, 34);
            tsbRecursiveRemove.Visible = false;
            tsbRecursiveRemove.Click += tsbRecursiveRemove_Click;
            // 
            // tsbCreateTableDiagram
            // 
            tsbCreateTableDiagram.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCreateTableDiagram.Image = Properties.Resources.diagram;
            tsbCreateTableDiagram.ImageTransparentColor = Color.Magenta;
            tsbCreateTableDiagram.Name = "tsbCreateTableDiagram";
            tsbCreateTableDiagram.Size = new Size(34, 34);
            tsbCreateTableDiagram.Visible = false;
            tsbCreateTableDiagram.Click += tsbCreateTableDiagram_Click;
            // 
            // tsbEdit
            // 
            tsbEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEdit.Image = Properties.Resources.editar;
            tsbEdit.ImageTransparentColor = Color.Magenta;
            tsbEdit.Name = "tsbEdit";
            tsbEdit.Size = new Size(34, 34);
            tsbEdit.Visible = false;
            tsbEdit.Click += tsbEdit_Click;
            // 
            // tsbRemove
            // 
            tsbRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemove.Image = Properties.Resources.borrar;
            tsbRemove.ImageTransparentColor = Color.Magenta;
            tsbRemove.Name = "tsbRemove";
            tsbRemove.Size = new Size(34, 34);
            tsbRemove.Visible = false;
            tsbRemove.Click += tsbRemove_Click;
            // 
            // tssEdit
            // 
            tssEdit.Name = "tssEdit";
            tssEdit.Size = new Size(6, 37);
            tssEdit.Visible = false;
            // 
            // tsbUp
            // 
            tsbUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbUp.Image = Properties.Resources.arriba;
            tsbUp.ImageTransparentColor = Color.Magenta;
            tsbUp.Name = "tsbUp";
            tsbUp.Size = new Size(34, 34);
            tsbUp.Visible = false;
            tsbUp.Click += tsbUp_Click;
            // 
            // tsbDown
            // 
            tsbDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDown.Image = Properties.Resources.abajo;
            tsbDown.ImageTransparentColor = Color.Magenta;
            tsbDown.Name = "tsbDown";
            tsbDown.Size = new Size(34, 34);
            tsbDown.Visible = false;
            tsbDown.Click += tsbDown_Click;
            // 
            // tssUpDown
            // 
            tssUpDown.Name = "tssUpDown";
            tssUpDown.Size = new Size(6, 37);
            tssUpDown.Visible = false;
            // 
            // tsbCollapseAll
            // 
            tsbCollapseAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCollapseAll.Image = Properties.Resources.contraer;
            tsbCollapseAll.ImageTransparentColor = Color.Magenta;
            tsbCollapseAll.Name = "tsbCollapseAll";
            tsbCollapseAll.Size = new Size(34, 34);
            tsbCollapseAll.Click += tsbCollapseAll_Click;
            // 
            // tsbRefresh
            // 
            tsbRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRefresh.Image = Properties.Resources.actualizar;
            tsbRefresh.ImageTransparentColor = Color.Magenta;
            tsbRefresh.Name = "tsbRefresh";
            tsbRefresh.Size = new Size(34, 34);
            tsbRefresh.Click += tsbRefresh_Click;
            // 
            // tpSearchAndReplace
            // 
            tpSearchAndReplace.Controls.Add(tlpSearchAndReplace);
            tpSearchAndReplace.Location = new Point(4, 4);
            tpSearchAndReplace.Name = "tpSearchAndReplace";
            tpSearchAndReplace.Padding = new Padding(3);
            tpSearchAndReplace.Size = new Size(402, 707);
            tpSearchAndReplace.TabIndex = 1;
            tpSearchAndReplace.UseVisualStyleBackColor = true;
            // 
            // tlpSearchAndReplace
            // 
            tlpSearchAndReplace.ColumnCount = 1;
            tlpSearchAndReplace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSearchAndReplace.Controls.Add(lblSearch, 0, 0);
            tlpSearchAndReplace.Controls.Add(txtSearchText, 0, 1);
            tlpSearchAndReplace.Controls.Add(flpSearchOptions, 0, 2);
            tlpSearchAndReplace.Controls.Add(lblSearchResultsSummary, 0, 3);
            tlpSearchAndReplace.Controls.Add(flpSearchButtons, 0, 4);
            tlpSearchAndReplace.Controls.Add(lblReplace, 0, 5);
            tlpSearchAndReplace.Controls.Add(txtReplaceText, 0, 6);
            tlpSearchAndReplace.Controls.Add(flpReplaceButtons, 0, 7);
            tlpSearchAndReplace.Dock = DockStyle.Fill;
            tlpSearchAndReplace.Location = new Point(3, 3);
            tlpSearchAndReplace.Name = "tlpSearchAndReplace";
            tlpSearchAndReplace.RowCount = 8;
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpSearchAndReplace.RowStyles.Add(new RowStyle());
            tlpSearchAndReplace.Size = new Size(396, 701);
            tlpSearchAndReplace.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Dock = DockStyle.Fill;
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblSearch.Location = new Point(3, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Padding = new Padding(0, 20, 0, 0);
            lblSearch.Size = new Size(390, 50);
            lblSearch.TabIndex = 5;
            // 
            // txtSearchText
            // 
            txtSearchText.Dock = DockStyle.Fill;
            txtSearchText.Location = new Point(3, 53);
            txtSearchText.Name = "txtSearchText";
            txtSearchText.Size = new Size(390, 27);
            txtSearchText.TabIndex = 0;
            txtSearchText.TextChanged += txtSearchText_TextChanged;
            txtSearchText.Enter += txtSearchText_Enter;
            txtSearchText.KeyUp += txtSearchText_KeyUp;
            // 
            // flpSearchOptions
            // 
            flpSearchOptions.Controls.Add(chkSearchMatchCase);
            flpSearchOptions.Controls.Add(chkSearchMatchWholeWords);
            flpSearchOptions.Controls.Add(chkSearchRegex);
            flpSearchOptions.Controls.Add(chkSearchWithinSelectedText);
            flpSearchOptions.Dock = DockStyle.Fill;
            flpSearchOptions.FlowDirection = FlowDirection.TopDown;
            flpSearchOptions.Location = new Point(3, 83);
            flpSearchOptions.Name = "flpSearchOptions";
            flpSearchOptions.Size = new Size(390, 134);
            flpSearchOptions.TabIndex = 1;
            // 
            // chkSearchMatchCase
            // 
            chkSearchMatchCase.AutoSize = true;
            chkSearchMatchCase.Location = new Point(3, 3);
            chkSearchMatchCase.Name = "chkSearchMatchCase";
            chkSearchMatchCase.Size = new Size(18, 17);
            chkSearchMatchCase.TabIndex = 0;
            chkSearchMatchCase.UseVisualStyleBackColor = true;
            chkSearchMatchCase.CheckedChanged += chkSearchMatchCase_CheckedChanged;
            // 
            // chkSearchMatchWholeWords
            // 
            chkSearchMatchWholeWords.AutoSize = true;
            chkSearchMatchWholeWords.Location = new Point(3, 26);
            chkSearchMatchWholeWords.Name = "chkSearchMatchWholeWords";
            chkSearchMatchWholeWords.Size = new Size(18, 17);
            chkSearchMatchWholeWords.TabIndex = 1;
            chkSearchMatchWholeWords.UseVisualStyleBackColor = true;
            chkSearchMatchWholeWords.CheckedChanged += chkSearchMatchWholeWords_CheckedChanged;
            // 
            // chkSearchRegex
            // 
            chkSearchRegex.AutoSize = true;
            chkSearchRegex.Location = new Point(3, 49);
            chkSearchRegex.Name = "chkSearchRegex";
            chkSearchRegex.Size = new Size(18, 17);
            chkSearchRegex.TabIndex = 2;
            chkSearchRegex.UseVisualStyleBackColor = true;
            chkSearchRegex.CheckedChanged += chkSearchRegex_CheckedChanged;
            // 
            // chkSearchWithinSelectedText
            // 
            chkSearchWithinSelectedText.AutoSize = true;
            chkSearchWithinSelectedText.Location = new Point(3, 72);
            chkSearchWithinSelectedText.Name = "chkSearchWithinSelectedText";
            chkSearchWithinSelectedText.Size = new Size(18, 17);
            chkSearchWithinSelectedText.TabIndex = 3;
            chkSearchWithinSelectedText.UseVisualStyleBackColor = true;
            chkSearchWithinSelectedText.CheckedChanged += chkSearchWithinSelectedText_CheckedChanged;
            // 
            // lblSearchResultsSummary
            // 
            lblSearchResultsSummary.AutoSize = true;
            lblSearchResultsSummary.Dock = DockStyle.Fill;
            lblSearchResultsSummary.Location = new Point(3, 220);
            lblSearchResultsSummary.Name = "lblSearchResultsSummary";
            lblSearchResultsSummary.Size = new Size(390, 60);
            lblSearchResultsSummary.TabIndex = 4;
            // 
            // flpSearchButtons
            // 
            flpSearchButtons.Controls.Add(btnSearch);
            flpSearchButtons.Controls.Add(btnGoNextSearchResult);
            flpSearchButtons.Controls.Add(btnUpdateSearchSelectedText);
            flpSearchButtons.Dock = DockStyle.Fill;
            flpSearchButtons.Location = new Point(3, 283);
            flpSearchButtons.Name = "flpSearchButtons";
            flpSearchButtons.Size = new Size(390, 94);
            flpSearchButtons.TabIndex = 2;
            // 
            // btnSearch
            // 
            btnSearch.AutoSize = true;
            btnSearch.Location = new Point(3, 3);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(94, 30);
            btnSearch.TabIndex = 0;
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Visible = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnGoNextSearchResult
            // 
            btnGoNextSearchResult.AutoSize = true;
            btnGoNextSearchResult.Enabled = false;
            btnGoNextSearchResult.Location = new Point(103, 3);
            btnGoNextSearchResult.Name = "btnGoNextSearchResult";
            btnGoNextSearchResult.Size = new Size(94, 30);
            btnGoNextSearchResult.TabIndex = 1;
            btnGoNextSearchResult.UseVisualStyleBackColor = true;
            btnGoNextSearchResult.Click += btnGoNextSearchResult_Click;
            // 
            // btnUpdateSearchSelectedText
            // 
            btnUpdateSearchSelectedText.AutoSize = true;
            btnUpdateSearchSelectedText.Location = new Point(203, 3);
            btnUpdateSearchSelectedText.Name = "btnUpdateSearchSelectedText";
            btnUpdateSearchSelectedText.Size = new Size(94, 30);
            btnUpdateSearchSelectedText.TabIndex = 1;
            btnUpdateSearchSelectedText.UseVisualStyleBackColor = true;
            btnUpdateSearchSelectedText.Visible = false;
            btnUpdateSearchSelectedText.Click += btnUpdateSearchSelectedText_Click;
            // 
            // lblReplace
            // 
            lblReplace.AutoSize = true;
            lblReplace.Dock = DockStyle.Fill;
            lblReplace.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblReplace.Location = new Point(3, 380);
            lblReplace.Name = "lblReplace";
            lblReplace.Padding = new Padding(0, 20, 0, 0);
            lblReplace.Size = new Size(390, 50);
            lblReplace.TabIndex = 6;
            // 
            // txtReplaceText
            // 
            txtReplaceText.Dock = DockStyle.Fill;
            txtReplaceText.Location = new Point(3, 433);
            txtReplaceText.Name = "txtReplaceText";
            txtReplaceText.Size = new Size(390, 27);
            txtReplaceText.TabIndex = 3;
            // 
            // flpReplaceButtons
            // 
            flpReplaceButtons.Controls.Add(btnReplaceCurrent);
            flpReplaceButtons.Controls.Add(btnReplaceAll);
            flpReplaceButtons.Dock = DockStyle.Fill;
            flpReplaceButtons.Location = new Point(3, 463);
            flpReplaceButtons.Name = "flpReplaceButtons";
            flpReplaceButtons.Size = new Size(390, 235);
            flpReplaceButtons.TabIndex = 4;
            // 
            // btnReplaceCurrent
            // 
            btnReplaceCurrent.AutoSize = true;
            btnReplaceCurrent.Enabled = false;
            btnReplaceCurrent.Location = new Point(3, 3);
            btnReplaceCurrent.Name = "btnReplaceCurrent";
            btnReplaceCurrent.Size = new Size(94, 30);
            btnReplaceCurrent.TabIndex = 0;
            btnReplaceCurrent.UseVisualStyleBackColor = true;
            btnReplaceCurrent.Click += btnReplaceCurrent_Click;
            // 
            // btnReplaceAll
            // 
            btnReplaceAll.AutoSize = true;
            btnReplaceAll.Enabled = false;
            btnReplaceAll.Location = new Point(103, 3);
            btnReplaceAll.Name = "btnReplaceAll";
            btnReplaceAll.Size = new Size(94, 30);
            btnReplaceAll.TabIndex = 1;
            btnReplaceAll.UseVisualStyleBackColor = true;
            btnReplaceAll.Click += btnReplaceAll_Click;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(tcSql);
            toolStripContainer1.ContentPanel.Size = new Size(1125, 703);
            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Location = new Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(1125, 740);
            toolStripContainer1.TabIndex = 1;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(toolStrip1);
            // 
            // tcSql
            // 
            tcSql.AllowReorder = true;
            tcSql.Controls.Add(tpNewTab);
            tcSql.DisplayStyle = TabStyle.Rounded;
            // 
            // 
            // 
            tcSql.DisplayStyleProvider.BlendStyle = BlendStyle.Normal;
            tcSql.DisplayStyleProvider.BorderColorDisabled = SystemColors.ControlLight;
            tcSql.DisplayStyleProvider.BorderColorFocused = Color.FromArgb(127, 157, 185);
            tcSql.DisplayStyleProvider.BorderColorHighlighted = SystemColors.ControlDark;
            tcSql.DisplayStyleProvider.BorderColorSelected = SystemColors.ControlDark;
            tcSql.DisplayStyleProvider.BorderColorUnselected = SystemColors.ControlDark;
            tcSql.DisplayStyleProvider.CloserButtonFillColorFocused = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonFillColorFocusedActive = Color.Black;
            tcSql.DisplayStyleProvider.CloserButtonFillColorHighlighted = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonFillColorHighlightedActive = Color.Black;
            tcSql.DisplayStyleProvider.CloserButtonFillColorSelected = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonFillColorSelectedActive = Color.Black;
            tcSql.DisplayStyleProvider.CloserButtonFillColorUnselected = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorFocused = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorFocusedActive = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorHighlighted = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorHighlightedActive = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorSelected = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorSelectedActive = Color.Empty;
            tcSql.DisplayStyleProvider.CloserButtonOutlineColorUnselected = Color.Empty;
            tcSql.DisplayStyleProvider.CloserColorFocused = Color.Black;
            tcSql.DisplayStyleProvider.CloserColorFocusedActive = Color.White;
            tcSql.DisplayStyleProvider.CloserColorHighlighted = Color.Black;
            tcSql.DisplayStyleProvider.CloserColorHighlightedActive = Color.White;
            tcSql.DisplayStyleProvider.CloserColorSelected = Color.FromArgb(95, 102, 115);
            tcSql.DisplayStyleProvider.CloserColorSelectedActive = Color.White;
            tcSql.DisplayStyleProvider.CloserColorUnselected = Color.Black;
            tcSql.DisplayStyleProvider.FocusTrack = false;
            tcSql.DisplayStyleProvider.HotTrack = true;
            tcSql.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleCenter;
            tcSql.DisplayStyleProvider.Opacity = 1F;
            tcSql.DisplayStyleProvider.Overlap = 3;
            tcSql.DisplayStyleProvider.Padding = new Point(6, 3);
            tcSql.DisplayStyleProvider.PageBackgroundColorDisabled = SystemColors.Control;
            tcSql.DisplayStyleProvider.PageBackgroundColorFocused = SystemColors.ControlLight;
            tcSql.DisplayStyleProvider.PageBackgroundColorHighlighted = Color.FromArgb(236, 244, 252);
            tcSql.DisplayStyleProvider.PageBackgroundColorSelected = SystemColors.ControlLightLight;
            tcSql.DisplayStyleProvider.PageBackgroundColorUnselected = SystemColors.Control;
            tcSql.DisplayStyleProvider.Radius = 10;
            tcSql.DisplayStyleProvider.SelectedTabIsLarger = false;
            tcSql.DisplayStyleProvider.ShowTabCloser = true;
            tcSql.DisplayStyleProvider.TabColorDisabled1 = SystemColors.Control;
            tcSql.DisplayStyleProvider.TabColorDisabled2 = SystemColors.Control;
            tcSql.DisplayStyleProvider.TabColorFocused1 = SystemColors.ControlLightLight;
            tcSql.DisplayStyleProvider.TabColorFocused2 = SystemColors.ControlLightLight;
            tcSql.DisplayStyleProvider.TabColorHighLighted1 = Color.FromArgb(236, 244, 252);
            tcSql.DisplayStyleProvider.TabColorHighLighted2 = Color.FromArgb(221, 237, 252);
            tcSql.DisplayStyleProvider.TabColorSelected1 = SystemColors.ControlLightLight;
            tcSql.DisplayStyleProvider.TabColorSelected2 = SystemColors.ControlLightLight;
            tcSql.DisplayStyleProvider.TabColorUnSelected1 = SystemColors.Control;
            tcSql.DisplayStyleProvider.TabColorUnSelected2 = SystemColors.Control;
            tcSql.DisplayStyleProvider.TabPageMargin = new Padding(1);
            tcSql.DisplayStyleProvider.TextColorDisabled = SystemColors.ControlDark;
            tcSql.DisplayStyleProvider.TextColorFocused = SystemColors.ControlText;
            tcSql.DisplayStyleProvider.TextColorHighlighted = SystemColors.ControlText;
            tcSql.DisplayStyleProvider.TextColorSelected = SystemColors.ControlText;
            tcSql.DisplayStyleProvider.TextColorUnselected = SystemColors.ControlText;
            tcSql.Dock = DockStyle.Fill;
            tcSql.HotTrack = true;
            tcSql.ImageList = ilTabControl;
            tcSql.Location = new Point(0, 0);
            tcSql.Name = "tcSql";
            tcSql.NewTabButton = true;
            tcSql.SelectedIndex = 0;
            tcSql.Size = new Size(1125, 703);
            tcSql.TabIndex = 0;
            tcSql.TabClosing += tcSql_TabClosing;
            tcSql.ReorderedTabs += tcSql_ReorderedTabs;
            tcSql.SelectedIndexChanged += tcSql_SelectedIndexChanged;
            tcSql.MouseDown += tcSql_MouseDown;
            tcSql.MouseUp += tcSql_MouseUp;
            // 
            // tpNewTab
            // 
            tpNewTab.ImageIndex = 0;
            tpNewTab.Location = new Point(4, 32);
            tpNewTab.Name = "tpNewTab";
            tpNewTab.Size = new Size(1117, 667);
            tpNewTab.TabIndex = 0;
            // 
            // ilTabControl
            // 
            ilTabControl.ColorDepth = ColorDepth.Depth8Bit;
            ilTabControl.ImageStream = (ImageListStreamer)resources.GetObject("ilTabControl.ImageStream");
            ilTabControl.TransparentColor = Color.Transparent;
            ilTabControl.Images.SetKeyName(0, "nuevo.png");
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.None;
            toolStrip1.ImageScalingSize = new Size(30, 30);
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbOpen, tsbSave, tsbSaveAll, toolStripSeparator2, tsbRun, tsbExportCsv, tsddbTransactions, toolStripSeparator3, tsbSearchAndReplace, tsbGoTo, toolStripSeparator5, tsbFormat, tsbHistory, toolStripSeparator7, tsbOpenDiagram, tsbNewDiagram, toolStripSeparator1, tsddbErrors, toolStripSeparator6, tslPosition });
            toolStrip1.Location = new Point(4, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(511, 37);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpen.Image = Properties.Resources.abrir;
            tsbOpen.ImageTransparentColor = Color.Magenta;
            tsbOpen.Name = "tsbOpen";
            tsbOpen.Size = new Size(34, 34);
            tsbOpen.Text = "toolStripButton1";
            tsbOpen.Click += tsbOpen_Click;
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Image = Properties.Resources.guardar;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(34, 34);
            tsbSave.Text = "toolStripButton1";
            tsbSave.Click += tsbSave_Click;
            // 
            // tsbSaveAll
            // 
            tsbSaveAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSaveAll.Image = Properties.Resources.guardar_todo;
            tsbSaveAll.ImageTransparentColor = Color.Magenta;
            tsbSaveAll.Name = "tsbSaveAll";
            tsbSaveAll.Size = new Size(34, 34);
            tsbSaveAll.Text = "toolStripButton1";
            tsbSaveAll.Click += tsbSaveAll_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 37);
            // 
            // tsbRun
            // 
            tsbRun.Enabled = false;
            tsbRun.Image = Properties.Resources.ejecutar;
            tsbRun.ImageTransparentColor = Color.Magenta;
            tsbRun.Name = "tsbRun";
            tsbRun.Size = new Size(34, 34);
            tsbRun.ToolTipText = "Ejecutar";
            tsbRun.Click += tsbRun_Click;
            // 
            // tsbExportCsv
            // 
            tsbExportCsv.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbExportCsv.Image = Properties.Resources.download;
            tsbExportCsv.ImageTransparentColor = Color.Magenta;
            tsbExportCsv.Name = "tsbExportCsv";
            tsbExportCsv.Size = new Size(34, 34);
            tsbExportCsv.Click += tsbExportCsv_Click;
            // 
            // tsddbTransactions
            // 
            tsddbTransactions.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsddbTransactions.DropDownItems.AddRange(new ToolStripItem[] { tsmiTransactionModeManual, tsmiTransactionModeAutoSingle, tsmiTransactionModeAutoCoordinated, toolStripSeparator4, tsmiTransactionLevelReadCommitted, tsmiTransactionLevelRepeatableRead, tsmiTransactionLevelSerializable });
            tsddbTransactions.Image = Properties.Resources.transaccion;
            tsddbTransactions.ImageTransparentColor = Color.Magenta;
            tsddbTransactions.Name = "tsddbTransactions";
            tsddbTransactions.Size = new Size(44, 34);
            tsddbTransactions.Text = "toolStripDropDownButton2";
            // 
            // tsmiTransactionModeManual
            // 
            tsmiTransactionModeManual.Name = "tsmiTransactionModeManual";
            tsmiTransactionModeManual.Size = new Size(203, 26);
            tsmiTransactionModeManual.Text = "Manual";
            tsmiTransactionModeManual.Click += tsmiTransactionModeManual_Click;
            // 
            // tsmiTransactionModeAutoSingle
            // 
            tsmiTransactionModeAutoSingle.Name = "tsmiTransactionModeAutoSingle";
            tsmiTransactionModeAutoSingle.Size = new Size(203, 26);
            tsmiTransactionModeAutoSingle.Text = "Auto-Single";
            tsmiTransactionModeAutoSingle.Click += tsmiTransactionModeAutoSingle_Click;
            // 
            // tsmiTransactionModeAutoCoordinated
            // 
            tsmiTransactionModeAutoCoordinated.Name = "tsmiTransactionModeAutoCoordinated";
            tsmiTransactionModeAutoCoordinated.Size = new Size(203, 26);
            tsmiTransactionModeAutoCoordinated.Text = "Auto-Coord";
            tsmiTransactionModeAutoCoordinated.Click += tsmiTransactionModeAutoCoordinated_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(200, 6);
            // 
            // tsmiTransactionLevelReadCommitted
            // 
            tsmiTransactionLevelReadCommitted.Name = "tsmiTransactionLevelReadCommitted";
            tsmiTransactionLevelReadCommitted.Size = new Size(203, 26);
            tsmiTransactionLevelReadCommitted.Text = "Read committed";
            tsmiTransactionLevelReadCommitted.Click += tsmiTransactionLevelReadCommitted_Click;
            // 
            // tsmiTransactionLevelRepeatableRead
            // 
            tsmiTransactionLevelRepeatableRead.Name = "tsmiTransactionLevelRepeatableRead";
            tsmiTransactionLevelRepeatableRead.Size = new Size(203, 26);
            tsmiTransactionLevelRepeatableRead.Text = "Repeatable read";
            tsmiTransactionLevelRepeatableRead.Click += tsmiTransactionLevelRepeatableRead_Click;
            // 
            // tsmiTransactionLevelSerializable
            // 
            tsmiTransactionLevelSerializable.Name = "tsmiTransactionLevelSerializable";
            tsmiTransactionLevelSerializable.Size = new Size(203, 26);
            tsmiTransactionLevelSerializable.Text = "Serializable";
            tsmiTransactionLevelSerializable.Click += tsmiTransactionLevelSerializable_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 37);
            // 
            // tsbSearchAndReplace
            // 
            tsbSearchAndReplace.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSearchAndReplace.Image = Properties.Resources.buscar;
            tsbSearchAndReplace.ImageTransparentColor = Color.Magenta;
            tsbSearchAndReplace.Name = "tsbSearchAndReplace";
            tsbSearchAndReplace.Size = new Size(34, 34);
            tsbSearchAndReplace.Text = "toolStripButton1";
            tsbSearchAndReplace.Click += tsbSearchAndReplace_Click;
            // 
            // tsbGoTo
            // 
            tsbGoTo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbGoTo.Image = Properties.Resources.goto_line;
            tsbGoTo.ImageTransparentColor = Color.Magenta;
            tsbGoTo.Name = "tsbGoTo";
            tsbGoTo.Size = new Size(34, 34);
            tsbGoTo.Text = "toolStripButton3";
            tsbGoTo.Click += tsbGoTo_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 37);
            // 
            // tsbFormat
            // 
            tsbFormat.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbFormat.Image = Properties.Resources.autoformato;
            tsbFormat.ImageTransparentColor = Color.Magenta;
            tsbFormat.Name = "tsbFormat";
            tsbFormat.Size = new Size(34, 34);
            tsbFormat.Text = "toolStripButton4";
            tsbFormat.Click += tsbFormat_Click;
            // 
            // tsbHistory
            // 
            tsbHistory.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbHistory.Image = Properties.Resources.historial;
            tsbHistory.ImageTransparentColor = Color.Magenta;
            tsbHistory.Name = "tsbHistory";
            tsbHistory.Size = new Size(34, 34);
            tsbHistory.Click += tsbHistory_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 37);
            // 
            // tsbOpenDiagram
            // 
            tsbOpenDiagram.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpenDiagram.Image = Properties.Resources.open_diagram;
            tsbOpenDiagram.ImageTransparentColor = Color.Magenta;
            tsbOpenDiagram.Name = "tsbOpenDiagram";
            tsbOpenDiagram.Size = new Size(34, 34);
            tsbOpenDiagram.Text = "toolStripButton1";
            tsbOpenDiagram.Click += tsbOpenDiagram_Click;
            // 
            // tsbNewDiagram
            // 
            tsbNewDiagram.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbNewDiagram.Image = Properties.Resources.new_diagram;
            tsbNewDiagram.ImageTransparentColor = Color.Magenta;
            tsbNewDiagram.Name = "tsbNewDiagram";
            tsbNewDiagram.Size = new Size(34, 34);
            tsbNewDiagram.Text = "toolStripButton1";
            tsbNewDiagram.Click += tsbNewDiagram_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 37);
            // 
            // tsddbErrors
            // 
            tsddbErrors.Image = Properties.Resources.ok;
            tsddbErrors.ImageTransparentColor = Color.Magenta;
            tsddbErrors.Name = "tsddbErrors";
            tsddbErrors.Size = new Size(44, 34);
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 37);
            // 
            // tslPosition
            // 
            tslPosition.Name = "tslPosition";
            tslPosition.Size = new Size(0, 34);
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.FixedPanel = FixedPanel.Panel1;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(toolStripContainer3);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(tcResult);
            splitContainer3.Size = new Size(1539, 518);
            splitContainer3.SplitterDistance = 505;
            splitContainer3.TabIndex = 0;
            // 
            // toolStripContainer3
            // 
            // 
            // toolStripContainer3.ContentPanel
            // 
            toolStripContainer3.ContentPanel.Controls.Add(lbResult);
            toolStripContainer3.ContentPanel.Size = new Size(505, 481);
            toolStripContainer3.Dock = DockStyle.Fill;
            toolStripContainer3.Location = new Point(0, 0);
            toolStripContainer3.Name = "toolStripContainer3";
            toolStripContainer3.Size = new Size(505, 518);
            toolStripContainer3.TabIndex = 1;
            toolStripContainer3.Text = "toolStripContainer3";
            // 
            // toolStripContainer3.TopToolStripPanel
            // 
            toolStripContainer3.TopToolStripPanel.Controls.Add(toolStrip3);
            // 
            // lbResult
            // 
            lbResult.Dock = DockStyle.Fill;
            lbResult.DrawMode = DrawMode.OwnerDrawVariable;
            lbResult.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lbResult.FormattingEnabled = true;
            lbResult.ItemHeight = 20;
            lbResult.Location = new Point(0, 0);
            lbResult.Name = "lbResult";
            lbResult.SelectionMode = SelectionMode.MultiExtended;
            lbResult.Size = new Size(505, 481);
            lbResult.TabIndex = 0;
            lbResult.DrawItem += lbResult_DrawItem;
            lbResult.MeasureItem += lbResult_MeasureItem;
            lbResult.SelectedIndexChanged += lbResult_SelectedIndexChanged;
            lbResult.Resize += lbResult_Resize;
            // 
            // toolStrip3
            // 
            toolStrip3.Dock = DockStyle.None;
            toolStrip3.ImageScalingSize = new Size(30, 30);
            toolStrip3.Items.AddRange(new ToolStripItem[] { tsbCurrentTabLastTask, tssResult, tsbRemoveSelected, tsbRemoveAll, tsbStopSelected, tsbStopAll });
            toolStrip3.Location = new Point(4, 0);
            toolStrip3.Name = "toolStrip3";
            toolStrip3.Size = new Size(53, 37);
            toolStrip3.TabIndex = 0;
            // 
            // tsbCurrentTabLastTask
            // 
            tsbCurrentTabLastTask.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCurrentTabLastTask.Image = Properties.Resources.last;
            tsbCurrentTabLastTask.ImageTransparentColor = Color.Magenta;
            tsbCurrentTabLastTask.Name = "tsbCurrentTabLastTask";
            tsbCurrentTabLastTask.Size = new Size(34, 34);
            tsbCurrentTabLastTask.Click += tsbCurrentTabLastTask_Click;
            // 
            // tssResult
            // 
            tssResult.Name = "tssResult";
            tssResult.Size = new Size(6, 37);
            // 
            // tsbRemoveSelected
            // 
            tsbRemoveSelected.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemoveSelected.Image = Properties.Resources.borrar;
            tsbRemoveSelected.ImageTransparentColor = Color.Magenta;
            tsbRemoveSelected.Name = "tsbRemoveSelected";
            tsbRemoveSelected.Size = new Size(34, 34);
            tsbRemoveSelected.Visible = false;
            tsbRemoveSelected.Click += tsbRemoveSelected_Click;
            // 
            // tsbRemoveAll
            // 
            tsbRemoveAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemoveAll.Image = Properties.Resources.borrar_todos;
            tsbRemoveAll.ImageTransparentColor = Color.Magenta;
            tsbRemoveAll.Name = "tsbRemoveAll";
            tsbRemoveAll.Size = new Size(34, 34);
            tsbRemoveAll.Visible = false;
            tsbRemoveAll.Click += tsbRemoveAll_Click;
            // 
            // tsbStopSelected
            // 
            tsbStopSelected.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbStopSelected.Image = Properties.Resources.detener;
            tsbStopSelected.ImageTransparentColor = Color.Magenta;
            tsbStopSelected.Name = "tsbStopSelected";
            tsbStopSelected.Size = new Size(34, 34);
            tsbStopSelected.Visible = false;
            tsbStopSelected.Click += tsbStopSelected_Click;
            // 
            // tsbStopAll
            // 
            tsbStopAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbStopAll.Image = Properties.Resources.detener_todos;
            tsbStopAll.ImageTransparentColor = Color.Magenta;
            tsbStopAll.Name = "tsbStopAll";
            tsbStopAll.Size = new Size(34, 34);
            tsbStopAll.Visible = false;
            tsbStopAll.Click += tsbDetenerTodos_Click;
            // 
            // tcResult
            // 
            tcResult.Controls.Add(tpResult);
            tcResult.Controls.Add(tpTable);
            tcResult.Controls.Add(tpExecutedSql);
            tcResult.Dock = DockStyle.Fill;
            tcResult.Location = new Point(0, 0);
            tcResult.Name = "tcResult";
            tcResult.SelectedIndex = 0;
            tcResult.Size = new Size(1030, 518);
            tcResult.TabIndex = 0;
            // 
            // tpResult
            // 
            tpResult.Controls.Add(toolStripContainer6);
            tpResult.Location = new Point(4, 29);
            tpResult.Name = "tpResult";
            tpResult.Padding = new Padding(3);
            tpResult.Size = new Size(1022, 485);
            tpResult.TabIndex = 0;
            tpResult.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer6
            // 
            // 
            // toolStripContainer6.ContentPanel
            // 
            toolStripContainer6.ContentPanel.Controls.Add(fctbResult);
            toolStripContainer6.ContentPanel.Size = new Size(1016, 452);
            toolStripContainer6.Dock = DockStyle.Fill;
            toolStripContainer6.Location = new Point(3, 3);
            toolStripContainer6.Name = "toolStripContainer6";
            toolStripContainer6.Size = new Size(1016, 479);
            toolStripContainer6.TabIndex = 1;
            toolStripContainer6.Text = "toolStripContainer6";
            // 
            // toolStripContainer6.TopToolStripPanel
            // 
            toolStripContainer6.TopToolStripPanel.Controls.Add(toolStrip6);
            // 
            // fctbResult
            // 
            fctbResult.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            fctbResult.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            fctbResult.AutoScrollMinSize = new Size(42, 59);
            fctbResult.BackBrush = null;
            fctbResult.CharHeight = 19;
            fctbResult.CharWidth = 10;
            fctbResult.DefaultMarkerSize = 8;
            fctbResult.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            fctbResult.Dock = DockStyle.Fill;
            fctbResult.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
            fctbResult.Hotkeys = resources.GetString("fctbResult.Hotkeys");
            fctbResult.IsReplaceMode = false;
            fctbResult.Location = new Point(0, 0);
            fctbResult.Name = "fctbResult";
            fctbResult.Paddings = new Padding(20);
            fctbResult.ReadOnly = true;
            fctbResult.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            fctbResult.ServiceColors = (ServiceColors)resources.GetObject("fctbResult.ServiceColors");
            fctbResult.ShowLineNumbers = false;
            fctbResult.Size = new Size(1016, 452);
            fctbResult.TabIndex = 0;
            fctbResult.Zoom = 100;
            fctbResult.SecondaryFormShowed += fctbResult_SecondaryFormShowed;
            fctbResult.SecondaryFormClosed += fctbResult_SecondaryFormClosed;
            // 
            // toolStrip6
            // 
            toolStrip6.Dock = DockStyle.None;
            toolStrip6.ImageScalingSize = new Size(20, 20);
            toolStrip6.Items.AddRange(new ToolStripItem[] { tsddbAutoScroll });
            toolStrip6.Location = new Point(4, 0);
            toolStrip6.Name = "toolStrip6";
            toolStrip6.Size = new Size(47, 27);
            toolStrip6.TabIndex = 0;
            // 
            // tsddbAutoScroll
            // 
            tsddbAutoScroll.DropDownItems.AddRange(new ToolStripItem[] { tsmiAutoScroll, tsmiManualScroll });
            tsddbAutoScroll.Image = Properties.Resources.scroll_auto;
            tsddbAutoScroll.ImageTransparentColor = Color.Magenta;
            tsddbAutoScroll.Name = "tsddbAutoScroll";
            tsddbAutoScroll.Size = new Size(34, 24);
            tsddbAutoScroll.DropDownItemClicked += tsddbAutoScroll_DropDownItemClicked;
            // 
            // tsmiAutoScroll
            // 
            tsmiAutoScroll.Image = Properties.Resources.scroll_auto;
            tsmiAutoScroll.Name = "tsmiAutoScroll";
            tsmiAutoScroll.Size = new Size(83, 26);
            tsmiAutoScroll.Tag = "auto";
            // 
            // tsmiManualScroll
            // 
            tsmiManualScroll.Image = Properties.Resources.scroll;
            tsmiManualScroll.Name = "tsmiManualScroll";
            tsmiManualScroll.Size = new Size(83, 26);
            tsmiManualScroll.Tag = "manual";
            // 
            // tpTable
            // 
            tpTable.Controls.Add(toolStripContainer5);
            tpTable.Location = new Point(4, 29);
            tpTable.Name = "tpTable";
            tpTable.Padding = new Padding(3);
            tpTable.Size = new Size(1022, 485);
            tpTable.TabIndex = 1;
            tpTable.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer5
            // 
            // 
            // toolStripContainer5.ContentPanel
            // 
            toolStripContainer5.ContentPanel.Controls.Add(gvTable);
            toolStripContainer5.ContentPanel.Size = new Size(1016, 452);
            toolStripContainer5.Dock = DockStyle.Fill;
            toolStripContainer5.Location = new Point(3, 3);
            toolStripContainer5.Name = "toolStripContainer5";
            toolStripContainer5.Size = new Size(1016, 479);
            toolStripContainer5.TabIndex = 1;
            toolStripContainer5.Text = "toolStripContainer5";
            // 
            // toolStripContainer5.TopToolStripPanel
            // 
            toolStripContainer5.TopToolStripPanel.Controls.Add(toolStrip5);
            // 
            // gvTable
            // 
            gvTable.AllowUserToAddRows = false;
            gvTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gvTable.ContextMenuStrip = cmsTable;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.NullValue = "[NULL]";
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            gvTable.DefaultCellStyle = dataGridViewCellStyle1;
            gvTable.Dock = DockStyle.Fill;
            gvTable.Location = new Point(0, 0);
            gvTable.Name = "gvTable";
            gvTable.RowHeadersWidth = 51;
            gvTable.RowTemplate.Height = 29;
            gvTable.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gvTable.ShowCellErrors = false;
            gvTable.ShowEditingIcon = false;
            gvTable.ShowRowErrors = false;
            gvTable.Size = new Size(1016, 452);
            gvTable.TabIndex = 0;
            gvTable.CellMouseDown += gvTable_CellMouseDown;
            gvTable.CellMouseUp += gvTable_CellMouseUp;
            gvTable.CellPainting += gvTable_CellPainting;
            gvTable.CellToolTipTextNeeded += gvTable_CellToolTipTextNeeded;
            gvTable.CellValueChanged += gvTable_CellValueChanged;
            gvTable.DataError += gvTable_DataError;
            gvTable.RowPostPaint += gvTable_RowPostPaint;
            gvTable.RowsRemoved += gvTable_RowsRemoved;
            // 
            // cmsTable
            // 
            cmsTable.ImageScalingSize = new Size(20, 20);
            cmsTable.Items.AddRange(new ToolStripItem[] { tsbCopyCellText, tsbLoadCellBinaryValueFromFile, tsbSaveCellBinaryValueInFile, tsbTextEditor, tsbSetNull });
            cmsTable.Name = "cmsTable";
            cmsTable.Size = new Size(74, 134);
            // 
            // tsbCopyCellText
            // 
            tsbCopyCellText.Image = Properties.Resources.copiar;
            tsbCopyCellText.Name = "tsbCopyCellText";
            tsbCopyCellText.Size = new Size(73, 26);
            tsbCopyCellText.Click += tsbCopyCellText_Click;
            // 
            // tsbLoadCellBinaryValueFromFile
            // 
            tsbLoadCellBinaryValueFromFile.Image = Properties.Resources.abrir;
            tsbLoadCellBinaryValueFromFile.Name = "tsbLoadCellBinaryValueFromFile";
            tsbLoadCellBinaryValueFromFile.Size = new Size(73, 26);
            tsbLoadCellBinaryValueFromFile.Click += tsbLoadCellBinaryValueFromFile_Click;
            // 
            // tsbSaveCellBinaryValueInFile
            // 
            tsbSaveCellBinaryValueInFile.Image = Properties.Resources.guardar_como;
            tsbSaveCellBinaryValueInFile.Name = "tsbSaveCellBinaryValueInFile";
            tsbSaveCellBinaryValueInFile.Size = new Size(73, 26);
            tsbSaveCellBinaryValueInFile.Click += tsbSaveCellBinaryValueInFile_Click;
            // 
            // tsbTextEditor
            // 
            tsbTextEditor.Image = Properties.Resources.editar;
            tsbTextEditor.Name = "tsbTextEditor";
            tsbTextEditor.Size = new Size(73, 26);
            tsbTextEditor.Click += tsbTextEditor_Click;
            // 
            // tsbSetNull
            // 
            tsbSetNull.Image = Properties.Resources._null;
            tsbSetNull.Name = "tsbSetNull";
            tsbSetNull.Size = new Size(73, 26);
            tsbSetNull.Click += tsbSetNull_Click;
            // 
            // toolStrip5
            // 
            toolStrip5.Dock = DockStyle.None;
            toolStrip5.ImageScalingSize = new Size(20, 20);
            toolStrip5.Items.AddRange(new ToolStripItem[] { tsddbTables, tsddbInsertRow, tsbDeleteRows, tsbApplyTableChanges });
            toolStrip5.Location = new Point(4, 0);
            toolStrip5.Name = "toolStrip5";
            toolStrip5.Size = new Size(139, 27);
            toolStrip5.TabIndex = 0;
            // 
            // tsddbTables
            // 
            tsddbTables.Image = Properties.Resources.tabla;
            tsddbTables.ImageTransparentColor = Color.Magenta;
            tsddbTables.Name = "tsddbTables";
            tsddbTables.Size = new Size(34, 24);
            tsddbTables.DropDownItemClicked += tsddbTables_DropDownItemClicked;
            // 
            // tsddbInsertRow
            // 
            tsddbInsertRow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsddbInsertRow.Image = Properties.Resources.nuevo;
            tsddbInsertRow.ImageTransparentColor = Color.Magenta;
            tsddbInsertRow.Name = "tsddbInsertRow";
            tsddbInsertRow.Size = new Size(34, 24);
            tsddbInsertRow.Click += tsddbInsertRow_Click;
            // 
            // tsbDeleteRows
            // 
            tsbDeleteRows.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteRows.Image = Properties.Resources.borrar;
            tsbDeleteRows.ImageTransparentColor = Color.Magenta;
            tsbDeleteRows.Name = "tsbDeleteRows";
            tsbDeleteRows.Size = new Size(29, 24);
            tsbDeleteRows.Click += tsbDeleteRows_Click;
            // 
            // tsbApplyTableChanges
            // 
            tsbApplyTableChanges.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbApplyTableChanges.Image = Properties.Resources.ok;
            tsbApplyTableChanges.ImageTransparentColor = Color.Magenta;
            tsbApplyTableChanges.Name = "tsbApplyTableChanges";
            tsbApplyTableChanges.Size = new Size(29, 24);
            tsbApplyTableChanges.Click += tsbApplyTableChanges_Click;
            // 
            // tpExecutedSql
            // 
            tpExecutedSql.Controls.Add(toolStripContainer4);
            tpExecutedSql.Location = new Point(4, 29);
            tpExecutedSql.Name = "tpExecutedSql";
            tpExecutedSql.Size = new Size(1022, 485);
            tpExecutedSql.TabIndex = 2;
            tpExecutedSql.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer4
            // 
            // 
            // toolStripContainer4.ContentPanel
            // 
            toolStripContainer4.ContentPanel.Controls.Add(fctbExecutedSql);
            toolStripContainer4.ContentPanel.Size = new Size(1022, 448);
            toolStripContainer4.Dock = DockStyle.Fill;
            toolStripContainer4.Location = new Point(0, 0);
            toolStripContainer4.Name = "toolStripContainer4";
            toolStripContainer4.Size = new Size(1022, 485);
            toolStripContainer4.TabIndex = 1;
            toolStripContainer4.Text = "toolStripContainer4";
            // 
            // toolStripContainer4.TopToolStripPanel
            // 
            toolStripContainer4.TopToolStripPanel.Controls.Add(toolStrip4);
            // 
            // fctbExecutedSql
            // 
            fctbExecutedSql.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            fctbExecutedSql.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            fctbExecutedSql.AutoScrollMinSize = new Size(42, 58);
            fctbExecutedSql.BackBrush = null;
            fctbExecutedSql.CharHeight = 18;
            fctbExecutedSql.CharWidth = 10;
            fctbExecutedSql.DefaultMarkerSize = 8;
            fctbExecutedSql.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            fctbExecutedSql.Dock = DockStyle.Fill;
            fctbExecutedSql.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
            fctbExecutedSql.Hotkeys = resources.GetString("fctbExecutedSql.Hotkeys");
            fctbExecutedSql.IsReplaceMode = false;
            fctbExecutedSql.Location = new Point(0, 0);
            fctbExecutedSql.Name = "fctbExecutedSql";
            fctbExecutedSql.Paddings = new Padding(20);
            fctbExecutedSql.ReadOnly = true;
            fctbExecutedSql.SearchMatches = null;
            fctbExecutedSql.SearchRange = null;
            fctbExecutedSql.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            fctbExecutedSql.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("fctbExecutedSql.ServiceColors");
            fctbExecutedSql.Size = new Size(1022, 448);
            fctbExecutedSql.TabIndex = 0;
            fctbExecutedSql.Zoom = 100;
            fctbExecutedSql.SecondaryFormShowed += fctbExecutedSql_SecondaryFormShowed;
            fctbExecutedSql.SecondaryFormClosed += fctbExecutedSql_SecondaryFormClosed;
            // 
            // toolStrip4
            // 
            toolStrip4.Dock = DockStyle.None;
            toolStrip4.ImageScalingSize = new Size(30, 30);
            toolStrip4.Items.AddRange(new ToolStripItem[] { tsbEditExecutedSql });
            toolStrip4.Location = new Point(4, 0);
            toolStrip4.Name = "toolStrip4";
            toolStrip4.Size = new Size(47, 37);
            toolStrip4.TabIndex = 0;
            // 
            // tsbEditExecutedSql
            // 
            tsbEditExecutedSql.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEditExecutedSql.Image = Properties.Resources.editar;
            tsbEditExecutedSql.ImageTransparentColor = Color.Magenta;
            tsbEditExecutedSql.Name = "tsbEditExecutedSql";
            tsbEditExecutedSql.Size = new Size(34, 34);
            tsbEditExecutedSql.Click += tsbEditExecutedSql_Click;
            // 
            // tsmiAbout
            // 
            tsmiAbout.Image = Properties.Resources.about;
            tsmiAbout.Name = "tsmiAbout";
            tsmiAbout.Size = new Size(224, 26);
            tsmiAbout.Click += tsmiAbout_Click;
            // 
            // tsmiNew
            // 
            tsmiNew.Image = Properties.Resources.nuevo_documento;
            tsmiNew.Name = "tsmiNew";
            tsmiNew.Size = new Size(224, 26);
            tsmiNew.Click += tsmiNew_Click;
            // 
            // tsmiOpen
            // 
            tsmiOpen.Image = Properties.Resources.abrir;
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.Size = new Size(224, 26);
            tsmiOpen.Click += tsmiOpen_Click;
            // 
            // tsmiSave
            // 
            tsmiSave.Image = Properties.Resources.guardar;
            tsmiSave.Name = "tsmiSave";
            tsmiSave.Size = new Size(224, 26);
            tsmiSave.Click += tsmiSave_Click;
            // 
            // tsmiSaveAs
            // 
            tsmiSaveAs.Image = Properties.Resources.guardar_como;
            tsmiSaveAs.Name = "tsmiSaveAs";
            tsmiSaveAs.Size = new Size(224, 26);
            tsmiSaveAs.Click += tsmiSaveAs_Click;
            // 
            // tsmiSaveAll
            // 
            tsmiSaveAll.Image = Properties.Resources.guardar_todo;
            tsmiSaveAll.Name = "tsmiSaveAll";
            tsmiSaveAll.Size = new Size(224, 26);
            tsmiSaveAll.Click += tsmiSaveAll_Click;
            // 
            // tsmiClose
            // 
            tsmiClose.Image = Properties.Resources.cerrar;
            tsmiClose.Name = "tsmiClose";
            tsmiClose.Size = new Size(224, 26);
            tsmiClose.Click += tsmiClose_Click;
            // 
            // tsmiCloseAll
            // 
            tsmiCloseAll.Image = Properties.Resources.cerrar_todo;
            tsmiCloseAll.Name = "tsmiCloseAll";
            tsmiCloseAll.Size = new Size(224, 26);
            tsmiCloseAll.Click += tsmiCloseAll_Click;
            // 
            // tsmiBack
            // 
            tsmiBack.Image = Properties.Resources.atras;
            tsmiBack.Name = "tsmiBack";
            tsmiBack.Size = new Size(224, 26);
            tsmiBack.Click += tsmiBack_Click;
            // 
            // tsmiForward
            // 
            tsmiForward.Image = Properties.Resources.adelante;
            tsmiForward.Name = "tsmiForward";
            tsmiForward.Size = new Size(224, 26);
            tsmiForward.Click += tsmiForward_Click;
            // 
            // tsmiUndo
            // 
            tsmiUndo.Image = Properties.Resources.undo;
            tsmiUndo.Name = "tsmiUndo";
            tsmiUndo.Size = new Size(224, 26);
            tsmiUndo.Click += tsmiUndo_Click;
            // 
            // tsmiRedo
            // 
            tsmiRedo.Image = Properties.Resources.redo;
            tsmiRedo.Name = "tsmiRedo";
            tsmiRedo.Size = new Size(224, 26);
            tsmiRedo.Click += tsmiRedo_Click;
            // 
            // tsmiCut
            // 
            tsmiCut.Image = Properties.Resources.cortar;
            tsmiCut.Name = "tsmiCut";
            tsmiCut.Size = new Size(224, 26);
            tsmiCut.Click += tsmiCut_Click;
            // 
            // tsmiCopy
            // 
            tsmiCopy.Image = Properties.Resources.copiar;
            tsmiCopy.Name = "tsmiCopy";
            tsmiCopy.Size = new Size(224, 26);
            tsmiCopy.Click += tsmiCopy_Click;
            // 
            // tsmiPaste
            // 
            tsmiPaste.Image = Properties.Resources.pegar;
            tsmiPaste.Name = "tsmiPaste";
            tsmiPaste.Size = new Size(224, 26);
            tsmiPaste.Click += tsmiPaste_Click;
            // 
            // tsmiSearchAndReplace
            // 
            tsmiSearchAndReplace.Image = Properties.Resources.buscar;
            tsmiSearchAndReplace.Name = "tsmiSearchAndReplace";
            tsmiSearchAndReplace.Size = new Size(224, 26);
            tsmiSearchAndReplace.Click += tsmiSearchAndReplace_Click;
            // 
            // tsmiGoTo
            // 
            tsmiGoTo.Image = Properties.Resources.linea;
            tsmiGoTo.Name = "tsmiGoTo";
            tsmiGoTo.Size = new Size(224, 26);
            tsmiGoTo.Click += tsmiGoTo_Click;
            // 
            // tsmiFormat
            // 
            tsmiFormat.Image = Properties.Resources.autoformato;
            tsmiFormat.Name = "tsmiFormat";
            tsmiFormat.Size = new Size(224, 26);
            tsmiFormat.Click += tsmiFormat_Click;
            // 
            // tsmiIncreaseFont
            // 
            tsmiIncreaseFont.Image = Properties.Resources.zoomin;
            tsmiIncreaseFont.Name = "tsmiIncreaseFont";
            tsmiIncreaseFont.Size = new Size(224, 26);
            tsmiIncreaseFont.Click += tsmiIncreaseFont_Click;
            // 
            // tsmiReduceFont
            // 
            tsmiReduceFont.Image = Properties.Resources.zoomout;
            tsmiReduceFont.Name = "tsmiReduceFont";
            tsmiReduceFont.Size = new Size(224, 26);
            tsmiReduceFont.Click += tsmiReduceFont_Click;
            // 
            // tsmiChangePassword
            // 
            tsmiChangePassword.Image = Properties.Resources.password;
            tsmiChangePassword.Name = "tsmiChangePassword";
            tsmiChangePassword.Size = new Size(224, 26);
            tsmiChangePassword.Click += tsmiChangePassword_Click;
            // 
            // tsmiImportConnections
            // 
            tsmiImportConnections.Image = Properties.Resources.import;
            tsmiImportConnections.Name = "tsmiImportConnections";
            tsmiImportConnections.Size = new Size(224, 26);
            tsmiImportConnections.Click += tsmiImportConnections_Click;
            // 
            // tsmiExportConnections
            // 
            tsmiExportConnections.Image = Properties.Resources.export;
            tsmiExportConnections.Name = "tsmiExportConnections";
            tsmiExportConnections.Size = new Size(224, 26);
            tsmiExportConnections.Click += tsmiExportConnections_Click;
            // 
            // tsmiMoreOptions
            // 
            tsmiMoreOptions.Image = Properties.Resources.opciones;
            tsmiMoreOptions.Name = "tsmiMoreOptions";
            tsmiMoreOptions.Size = new Size(224, 26);
            tsmiMoreOptions.Click += tsmMoreOptions_Click;
            // 
            // cmsTabs
            // 
            cmsTabs.ImageScalingSize = new Size(20, 20);
            cmsTabs.Items.AddRange(new ToolStripItem[] { tsmiCloseTab, tsmiCloseAllTabs, tsmiCloseAllTabsExceptThisOne, tsmiReopenLastClosedTab, tsmiClosedTabsLog, tsmiCopyPath, tsmiOpenFolder });
            cmsTabs.Name = "cmsTabs";
            cmsTabs.Size = new Size(74, 186);
            // 
            // tsmiCloseTab
            // 
            tsmiCloseTab.Image = Properties.Resources.cerrar;
            tsmiCloseTab.Name = "tsmiCloseTab";
            tsmiCloseTab.Size = new Size(73, 26);
            tsmiCloseTab.Click += tsmiCloseTab_Click;
            // 
            // tsmiCloseAllTabs
            // 
            tsmiCloseAllTabs.Image = Properties.Resources.cerrar_todo;
            tsmiCloseAllTabs.Name = "tsmiCloseAllTabs";
            tsmiCloseAllTabs.Size = new Size(73, 26);
            tsmiCloseAllTabs.Click += tsmiCloseAllTabs_Click;
            // 
            // tsmiCloseAllTabsExceptThisOne
            // 
            tsmiCloseAllTabsExceptThisOne.Image = Properties.Resources.cerrar_todo;
            tsmiCloseAllTabsExceptThisOne.Name = "tsmiCloseAllTabsExceptThisOne";
            tsmiCloseAllTabsExceptThisOne.Size = new Size(73, 26);
            tsmiCloseAllTabsExceptThisOne.Click += tsmiCloseAllTabsExceptThisOne_Click;
            // 
            // tsmiReopenLastClosedTab
            // 
            tsmiReopenLastClosedTab.Image = Properties.Resources.undo;
            tsmiReopenLastClosedTab.Name = "tsmiReopenLastClosedTab";
            tsmiReopenLastClosedTab.Size = new Size(73, 26);
            tsmiReopenLastClosedTab.Click += tsmiReopenLastClosedTab_Click;
            // 
            // tsmiClosedTabsLog
            // 
            tsmiClosedTabsLog.Image = Properties.Resources.historial;
            tsmiClosedTabsLog.Name = "tsmiClosedTabsLog";
            tsmiClosedTabsLog.Size = new Size(73, 26);
            tsmiClosedTabsLog.Click += tsmiClosedTabsLog_Click;
            // 
            // tsmiCopyPath
            // 
            tsmiCopyPath.Image = Properties.Resources.copiar;
            tsmiCopyPath.Name = "tsmiCopyPath";
            tsmiCopyPath.Size = new Size(73, 26);
            tsmiCopyPath.Click += tsmiCopyPath_Click;
            // 
            // tsmiOpenFolder
            // 
            tsmiOpenFolder.Image = Properties.Resources.abrir;
            tsmiOpenFolder.Name = "tsmiOpenFolder";
            tsmiOpenFolder.Size = new Size(73, 26);
            tsmiOpenFolder.Click += tsmiOpenFolder_Click;
            // 
            // tscmiBack
            // 
            tscmiBack.Image = Properties.Resources.atras;
            tscmiBack.Name = "tscmiBack";
            tscmiBack.Size = new Size(73, 26);
            tscmiBack.Click += tsmiBack_Click;
            // 
            // tscmiForward
            // 
            tscmiForward.Image = Properties.Resources.adelante;
            tscmiForward.Name = "tscmiForward";
            tscmiForward.Size = new Size(73, 26);
            tscmiForward.Click += tsmiForward_Click;
            // 
            // tscmiUndo
            // 
            tscmiUndo.Image = Properties.Resources.undo;
            tscmiUndo.Name = "tscmiUndo";
            tscmiUndo.Size = new Size(73, 26);
            tscmiUndo.Click += tsmiUndo_Click;
            // 
            // tscmiRedo
            // 
            tscmiRedo.Image = Properties.Resources.redo;
            tscmiRedo.Name = "tscmiRedo";
            tscmiRedo.Size = new Size(73, 26);
            tscmiRedo.Click += tsmiRedo_Click;
            // 
            // tscmiCut
            // 
            tscmiCut.Image = Properties.Resources.cortar;
            tscmiCut.Name = "tscmiCut";
            tscmiCut.Size = new Size(73, 26);
            tscmiCut.Click += tsmiCut_Click;
            // 
            // tscmiCopy
            // 
            tscmiCopy.Image = Properties.Resources.copiar;
            tscmiCopy.Name = "tscmiCopy";
            tscmiCopy.Size = new Size(73, 26);
            tscmiCopy.Click += tsmiCopy_Click;
            // 
            // tscmiPaste
            // 
            tscmiPaste.Image = Properties.Resources.pegar;
            tscmiPaste.Name = "tscmiPaste";
            tscmiPaste.Size = new Size(73, 26);
            tscmiPaste.Click += tsmiPaste_Click;
            // 
            // tscmiSearchAndReplace
            // 
            tscmiSearchAndReplace.Image = Properties.Resources.buscar;
            tscmiSearchAndReplace.Name = "tscmiSearchAndReplace";
            tscmiSearchAndReplace.Size = new Size(73, 26);
            tscmiSearchAndReplace.Click += tsmiSearchAndReplace_Click;
            // 
            // tscmiGoTo
            // 
            tscmiGoTo.Image = Properties.Resources.linea;
            tscmiGoTo.Name = "tscmiGoTo";
            tscmiGoTo.Size = new Size(73, 26);
            tscmiGoTo.Click += tsmiGoTo_Click;
            // 
            // tscmiFormat
            // 
            tscmiFormat.Image = Properties.Resources.autoformato;
            tscmiFormat.Name = "tscmiFormat";
            tscmiFormat.Size = new Size(73, 26);
            tscmiFormat.Click += tsmiFormat_Click;
            // 
            // ilServers
            // 
            ilServers.ColorDepth = ColorDepth.Depth8Bit;
            ilServers.ImageStream = (ImageListStreamer)resources.GetObject("ilServers.ImageStream");
            ilServers.TransparentColor = Color.Transparent;
            ilServers.Images.SetKeyName(0, "carpeta.png");
            ilServers.Images.SetKeyName(1, "db.png");
            ilServers.Images.SetKeyName(2, "nuevo.png");
            // 
            // ofdSql
            // 
            ofdSql.FileName = "openFileDialog1";
            ofdSql.FilterIndex = 0;
            // 
            // sfdSql
            // 
            sfdSql.DefaultExt = "sql";
            sfdSql.FilterIndex = 0;
            // 
            // ofdBinaryCell
            // 
            ofdBinaryCell.FileName = "";
            ofdBinaryCell.FilterIndex = 0;
            // 
            // sfdBinaryCell
            // 
            sfdBinaryCell.DefaultExt = "";
            sfdBinaryCell.FilterIndex = 0;
            // 
            // tmrPosition
            // 
            tmrPosition.Interval = 500;
            tmrPosition.Tick += tmrPosition_Tick;
            // 
            // tmrSaveTabs
            // 
            tmrSaveTabs.Interval = 2000;
            tmrSaveTabs.Tick += tmrSaveTabs_Tick;
            // 
            // tmrResult
            // 
            tmrResult.Interval = 500;
            tmrResult.Tick += tmrResult_Tick;
            // 
            // tmrFitGridColumns
            // 
            tmrFitGridColumns.Tick += tmrFitGridColumns_Tick;
            // 
            // ilAutocompleteMenu
            // 
            ilAutocompleteMenu.ColorDepth = ColorDepth.Depth32Bit;
            ilAutocompleteMenu.ImageStream = (ImageListStreamer)resources.GetObject("ilAutocompleteMenu.ImageStream");
            ilAutocompleteMenu.TransparentColor = Color.Transparent;
            ilAutocompleteMenu.Images.SetKeyName(0, "schema.png");
            ilAutocompleteMenu.Images.SetKeyName(1, "tva_table.png");
            ilAutocompleteMenu.Images.SetKeyName(2, "tva_1n.png");
            ilAutocompleteMenu.Images.SetKeyName(3, "tva_n1.png");
            ilAutocompleteMenu.Images.SetKeyName(4, "tva_binary.png");
            ilAutocompleteMenu.Images.SetKeyName(5, "tva_bool.png");
            ilAutocompleteMenu.Images.SetKeyName(6, "tva_date.png");
            ilAutocompleteMenu.Images.SetKeyName(7, "tva_dato.png");
            ilAutocompleteMenu.Images.SetKeyName(8, "tva_int.png");
            ilAutocompleteMenu.Images.SetKeyName(9, "tva_money.png");
            ilAutocompleteMenu.Images.SetKeyName(10, "tva_number.png");
            ilAutocompleteMenu.Images.SetKeyName(11, "tva_text.png");
            ilAutocompleteMenu.Images.SetKeyName(12, "tva_time.png");
            ilAutocompleteMenu.Images.SetKeyName(13, "postgresql.png");
            ilAutocompleteMenu.Images.SetKeyName(14, "key.png");
            ilAutocompleteMenu.Images.SetKeyName(15, "tva_element.png");
            ilAutocompleteMenu.Images.SetKeyName(16, "current_fragment.png");
            // 
            // cmsFctb
            // 
            cmsFctb.ImageScalingSize = new Size(20, 20);
            cmsFctb.Items.AddRange(new ToolStripItem[] { tscmiBack, tscmiForward, tscmiUndo, tscmiRedo, tscmiCut, tscmiCopy, tscmiPaste, tscmiSearchAndReplace, tscmiGoTo, tscmiFormat });
            cmsFctb.Name = "cmsFctb";
            cmsFctb.Size = new Size(74, 264);
            // 
            // sfdCsv
            // 
            sfdCsv.DefaultExt = "csv";
            sfdCsv.FilterIndex = 0;
            // 
            // mm
            // 
            mm.ImageScalingSize = new Size(20, 20);
            mm.Items.AddRange(new ToolStripItem[] { tsmiFile, tsmiEdit, tsmiRunMenu, tsmiDiagrams, tsmiOptions, tsmiUpdates });
            mm.Location = new Point(0, 0);
            mm.Name = "mm";
            mm.Size = new Size(1539, 28);
            mm.TabIndex = 4;
            mm.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new ToolStripItem[] { tsmiNew, tsmiOpen, tsmiSave, tsmiSaveAs, tsmiSaveAll, tsmiClose, tsmiCloseAll, tsmiAbout });
            tsmiFile.Image = Properties.Resources.archivo;
            tsmiFile.ImageTransparentColor = Color.Magenta;
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiFile.Size = new Size(34, 24);
            // 
            // tsmiEdit
            // 
            tsmiEdit.DropDownItems.AddRange(new ToolStripItem[] { tsmiBack, tsmiForward, tsmiUndo, tsmiRedo, tsmiCut, tsmiCopy, tsmiPaste, tsmiSearchAndReplace, tsmiGoTo, tsmiFormat });
            tsmiEdit.Image = Properties.Resources.editar;
            tsmiEdit.ImageTransparentColor = Color.Magenta;
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiEdit.Size = new Size(34, 24);
            // 
            // tsmiRunMenu
            // 
            tsmiRunMenu.DropDownItems.AddRange(new ToolStripItem[] { tsmiRun, tsmiExportCsv, tsmiCopyToTable });
            tsmiRunMenu.Image = Properties.Resources.ejecutar;
            tsmiRunMenu.Name = "tsmiRunMenu";
            tsmiRunMenu.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiRunMenu.Size = new Size(34, 24);
            // 
            // tsmiRun
            // 
            tsmiRun.Image = Properties.Resources.ejecutar;
            tsmiRun.Name = "tsmiRun";
            tsmiRun.Size = new Size(224, 26);
            tsmiRun.Click += tsmiRun_Click;
            // 
            // tsmiExportCsv
            // 
            tsmiExportCsv.Image = Properties.Resources.download;
            tsmiExportCsv.Name = "tsmiExportCsv";
            tsmiExportCsv.Size = new Size(224, 26);
            tsmiExportCsv.Click += tsmiExportCsv_Click;
            // 
            // tsmiCopyToTable
            // 
            tsmiCopyToTable.Image = Properties.DiagramIcons.add_row;
            tsmiCopyToTable.Name = "tsmiCopyToTable";
            tsmiCopyToTable.Size = new Size(224, 26);
            tsmiCopyToTable.Click += tsmiCopyToTable_Click;
            // 
            // tsmiDiagrams
            // 
            tsmiDiagrams.DropDownItems.AddRange(new ToolStripItem[] { tsmiNewDiagram, tsmiOpenDiagram });
            tsmiDiagrams.Image = Properties.Resources.diagram;
            tsmiDiagrams.Name = "tsmiDiagrams";
            tsmiDiagrams.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiDiagrams.Size = new Size(34, 24);
            // 
            // tsmiNewDiagram
            // 
            tsmiNewDiagram.Image = Properties.Resources.new_diagram;
            tsmiNewDiagram.Name = "tsmiNewDiagram";
            tsmiNewDiagram.Size = new Size(224, 26);
            tsmiNewDiagram.Click += tsmiNewDiagram_Click;
            // 
            // tsmiOpenDiagram
            // 
            tsmiOpenDiagram.Image = Properties.Resources.open_diagram;
            tsmiOpenDiagram.Name = "tsmiOpenDiagram";
            tsmiOpenDiagram.Size = new Size(224, 26);
            tsmiOpenDiagram.Click += tsmiOpenDiagram_Click;
            // 
            // tsmiOptions
            // 
            tsmiOptions.DropDownItems.AddRange(new ToolStripItem[] { tsmiIncreaseFont, tsmiReduceFont, tsmiHistory, tsmiChangePassword, tsmiExportConnections, tsmiImportConnections, tsmiMoreOptions });
            tsmiOptions.Image = Properties.Resources.opciones;
            tsmiOptions.ImageTransparentColor = Color.Magenta;
            tsmiOptions.Name = "tsmiOptions";
            tsmiOptions.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiOptions.Size = new Size(34, 24);
            // 
            // tsmiHistory
            // 
            tsmiHistory.Image = Properties.Resources.historial;
            tsmiHistory.Name = "tsmiHistory";
            tsmiHistory.Size = new Size(224, 26);
            tsmiHistory.Click += tsmiHistory_Click;
            // 
            // tsmiUpdates
            // 
            tsmiUpdates.Image = Properties.Resources.updates;
            tsmiUpdates.Name = "tsmiUpdates";
            tsmiUpdates.Overflow = ToolStripItemOverflow.AsNeeded;
            tsmiUpdates.Size = new Size(34, 24);
            tsmiUpdates.Visible = false;
            tsmiUpdates.Click += tsmiUpdates_Click;
            // 
            // ofdImportConfig
            // 
            ofdImportConfig.FileName = "openFileDialog1";
            ofdImportConfig.FilterIndex = 0;
            // 
            // ofdOpenDiagram
            // 
            ofdOpenDiagram.FileName = "openFileDialog1";
            ofdOpenDiagram.FilterIndex = 0;
            // 
            // sfdSaveDiagram
            // 
            sfdSaveDiagram.DefaultExt = "csv";
            sfdSaveDiagram.FilterIndex = 0;
            // 
            // tmrReenableRunButton
            // 
            tmrReenableRunButton.Interval = 2000;
            tmrReenableRunButton.Tick += tmrReenableRunButton_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1539, 1290);
            Controls.Add(splitContainer1);
            Controls.Add(mm);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mm;
            Name = "MainForm";
            Text = "pgMulti";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Resize += MainForm_Resize;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tcLeftPanel.ResumeLayout(false);
            tpConnections.ResumeLayout(false);
            toolStripContainer2.ContentPanel.ResumeLayout(false);
            toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer2.TopToolStripPanel.PerformLayout();
            toolStripContainer2.ResumeLayout(false);
            toolStripContainer2.PerformLayout();
            cmsServers.ResumeLayout(false);
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            tpSearchAndReplace.ResumeLayout(false);
            tlpSearchAndReplace.ResumeLayout(false);
            tlpSearchAndReplace.PerformLayout();
            flpSearchOptions.ResumeLayout(false);
            flpSearchOptions.PerformLayout();
            flpSearchButtons.ResumeLayout(false);
            flpSearchButtons.PerformLayout();
            flpReplaceButtons.ResumeLayout(false);
            flpReplaceButtons.PerformLayout();
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            tcSql.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            toolStripContainer3.ContentPanel.ResumeLayout(false);
            toolStripContainer3.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer3.TopToolStripPanel.PerformLayout();
            toolStripContainer3.ResumeLayout(false);
            toolStripContainer3.PerformLayout();
            toolStrip3.ResumeLayout(false);
            toolStrip3.PerformLayout();
            tcResult.ResumeLayout(false);
            tpResult.ResumeLayout(false);
            toolStripContainer6.ContentPanel.ResumeLayout(false);
            toolStripContainer6.ContentPanel.PerformLayout();
            toolStripContainer6.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer6.TopToolStripPanel.PerformLayout();
            toolStripContainer6.ResumeLayout(false);
            toolStripContainer6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fctbResult).EndInit();
            toolStrip6.ResumeLayout(false);
            toolStrip6.PerformLayout();
            tpTable.ResumeLayout(false);
            toolStripContainer5.ContentPanel.ResumeLayout(false);
            toolStripContainer5.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer5.TopToolStripPanel.PerformLayout();
            toolStripContainer5.ResumeLayout(false);
            toolStripContainer5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gvTable).EndInit();
            cmsTable.ResumeLayout(false);
            toolStrip5.ResumeLayout(false);
            toolStrip5.PerformLayout();
            tpExecutedSql.ResumeLayout(false);
            toolStripContainer4.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer4.TopToolStripPanel.PerformLayout();
            toolStripContainer4.ResumeLayout(false);
            toolStripContainer4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fctbExecutedSql).EndInit();
            toolStrip4.ResumeLayout(false);
            toolStrip4.PerformLayout();
            cmsTabs.ResumeLayout(false);
            cmsFctb.ResumeLayout(false);
            mm.ResumeLayout(false);
            mm.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private OpenFileDialog ofdBinaryCell;
        private SaveFileDialog sfdBinaryCell;
        private TabControlExtra tcSql;
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
        private FastColoredTextBox fctbResult;
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
        private ToolStripMenuItem tsmiSearchAndReplace;
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
        private ToolStripMenuItem tscmiSearchAndReplace;
        private ToolStripMenuItem tscmiGoTo;
        private ToolStripMenuItem tscmiFormat;
        private ToolStripMenuItem tsmiIncreaseFont;
        private ToolStripMenuItem tsmiReduceFont;
        private ToolStripSeparator toolStripSeparator1;
        private ContextMenuStrip cmsFctb;
        private ToolStripButton tsbApplyTableChanges;
        private ContextMenuStrip cmsTable;
        private ToolStripMenuItem tsbCopyCellText;
        private ToolStripMenuItem tsbLoadCellBinaryValueFromFile;
        private ToolStripMenuItem tsbSaveCellBinaryValueInFile;
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
        private ToolStripButton tsbSearchAndReplace;
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
        private ToolStripMenuItem tsmiCopyPath;
        private ToolStripMenuItem tsmiOpenFolder;
        private System.Windows.Forms.Timer tmrReenableRunButton;
        private ToolStripMenuItem tsmiReopenLastClosedTab;
        private ToolStripButton tsbCreateTableDiagram;
        private ToolStripMenuItem tsmiUpdates;
        private TabControl tcLeftPanel;
        private TabPage tpConnections;
        private TabPage tpSearchAndReplace;
        private TableLayoutPanel tlpSearchAndReplace;
        private TextBox txtSearchText;
        private TextBox txtReplaceText;
        private FlowLayoutPanel flpSearchOptions;
        private CheckBox chkSearchMatchCase;
        private CheckBox chkSearchMatchWholeWords;
        private CheckBox chkSearchRegex;
        private FlowLayoutPanel flpSearchButtons;
        private FlowLayoutPanel flpReplaceButtons;
        private Button btnSearch;
        private Button btnGoNextSearchResult;
        private Button btnReplaceCurrent;
        private Button btnReplaceAll;
        private Label lblSearchResultsSummary;
        private CheckBox chkSearchWithinSelectedText;
        private Label lblSearch;
        private Label lblReplace;
        private Button btnUpdateSearchSelectedText;
        private ToolStripMenuItem tsmiCopyToTable;
    }
}
using System.Windows.Forms;

namespace PgMulti.Forms
{
    partial class DiagramTableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramTableForm));
            this.lblTableName = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.tc = new System.Windows.Forms.TabControl();
            this.tcGeneral = new System.Windows.Forms.TabPage();
            this.tlpGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.txtSchemaName = new System.Windows.Forms.TextBox();
            this.lblSchemaName = new System.Windows.Forms.Label();
            this.tcColumns = new System.Windows.Forms.TabPage();
            this.scColumns = new System.Windows.Forms.SplitContainer();
            this.tscColumns = new System.Windows.Forms.ToolStripContainer();
            this.gvColumns = new System.Windows.Forms.DataGridView();
            this.gvcColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvcColumnDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvcColumnPrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gvcColumnNotNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsColumns = new System.Windows.Forms.ToolStrip();
            this.tsbAddColumn = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveColumn = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveUpColumn = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveDownColumn = new System.Windows.Forms.ToolStripButton();
            this.pnlColumn = new System.Windows.Forms.Panel();
            this.tlpColumn = new System.Windows.Forms.TableLayoutPanel();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.txtColumnName = new System.Windows.Forms.TextBox();
            this.lblColumnType = new System.Windows.Forms.Label();
            this.cbColumnType = new System.Windows.Forms.ComboBox();
            this.lblColumnTypeInitials = new System.Windows.Forms.Label();
            this.txtColumnTypeInitials = new System.Windows.Forms.TextBox();
            this.lblColumnPrimaryKey = new System.Windows.Forms.Label();
            this.chkColumnPrimaryKey = new System.Windows.Forms.CheckBox();
            this.lblColumnIdentity = new System.Windows.Forms.Label();
            this.chkColumnIdentity = new System.Windows.Forms.CheckBox();
            this.lblColumnDefault = new System.Windows.Forms.Label();
            this.lblColumnNotNull = new System.Windows.Forms.Label();
            this.chkColumnNotNull = new System.Windows.Forms.CheckBox();
            this.txtColumnDefault = new System.Windows.Forms.TextBox();

            this.pnlConfirmation = new System.Windows.Forms.Panel();
            this.flp = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tc.SuspendLayout();
            this.tcGeneral.SuspendLayout();
            this.tlpGeneral.SuspendLayout();
            this.tcColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scColumns)).BeginInit();
            this.scColumns.Panel1.SuspendLayout();
            this.scColumns.Panel2.SuspendLayout();
            this.scColumns.SuspendLayout();
            this.tscColumns.ContentPanel.SuspendLayout();
            this.tscColumns.TopToolStripPanel.SuspendLayout();
            this.tscColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvColumns)).BeginInit();
            this.tsColumns.SuspendLayout();
            this.pnlColumn.SuspendLayout();
            this.tlpColumn.SuspendLayout();

            this.pnlConfirmation.SuspendLayout();
            this.flp.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTableName
            // 
            this.lblTableName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTableName.AutoSize = true;
            this.lblTableName.Location = new System.Drawing.Point(3, 10);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(0, 20);
            this.lblTableName.TabIndex = 0;
            // 
            // txtTableName
            // 
            this.txtTableName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTableName.Location = new System.Drawing.Point(203, 6);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(338, 27);
            this.txtTableName.TabIndex = 0;
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tcGeneral);
            this.tc.Controls.Add(this.tcColumns);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(800, 389);
            this.tc.TabIndex = 0;
            // 
            // tcGeneral
            // 
            this.tcGeneral.Controls.Add(this.tlpGeneral);
            this.tcGeneral.Location = new System.Drawing.Point(4, 29);
            this.tcGeneral.Name = "tcGeneral";
            this.tcGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tcGeneral.Size = new System.Drawing.Size(792, 356);
            this.tcGeneral.TabIndex = 0;
            this.tcGeneral.UseVisualStyleBackColor = true;
            // 
            // tlpGeneral
            // 
            this.tlpGeneral.AutoScroll = true;
            this.tlpGeneral.ColumnCount = 2;
            this.tlpGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGeneral.Controls.Add(this.lblTableName, 0, 0);
            this.tlpGeneral.Controls.Add(this.txtTableName, 1, 0);
            this.tlpGeneral.Controls.Add(this.txtSchemaName, 1, 1);
            this.tlpGeneral.Controls.Add(this.lblSchemaName, 0, 1);
            this.tlpGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGeneral.Location = new System.Drawing.Point(3, 3);
            this.tlpGeneral.Name = "tlpGeneral";
            this.tlpGeneral.RowCount = 3;
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGeneral.Size = new System.Drawing.Size(786, 350);
            this.tlpGeneral.TabIndex = 2;
            // 
            // txtSchemaName
            // 
            this.txtSchemaName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSchemaName.Location = new System.Drawing.Point(203, 46);
            this.txtSchemaName.Name = "txtSchemaName";
            this.txtSchemaName.Size = new System.Drawing.Size(338, 27);
            this.txtSchemaName.TabIndex = 1;
            // 
            // lblSchemaName
            // 
            this.lblSchemaName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSchemaName.AutoSize = true;
            this.lblSchemaName.Location = new System.Drawing.Point(3, 50);
            this.lblSchemaName.Name = "lblSchemaName";
            this.lblSchemaName.Size = new System.Drawing.Size(0, 20);
            this.lblSchemaName.TabIndex = 0;
            // 
            // tcColumns
            // 
            this.tcColumns.Controls.Add(this.scColumns);
            this.tcColumns.Location = new System.Drawing.Point(4, 29);
            this.tcColumns.Name = "tcColumns";
            this.tcColumns.Padding = new System.Windows.Forms.Padding(3);
            this.tcColumns.Size = new System.Drawing.Size(792, 356);
            this.tcColumns.TabIndex = 1;
            this.tcColumns.UseVisualStyleBackColor = true;
            // 
            // scColumns
            // 
            this.scColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scColumns.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scColumns.Location = new System.Drawing.Point(3, 3);
            this.scColumns.Name = "scColumns";
            // 
            // scColumns.Panel1
            // 
            this.scColumns.Panel1.Controls.Add(this.tscColumns);
            // 
            // scColumns.Panel2
            // 
            this.scColumns.Panel2.AutoScroll = true;
            this.scColumns.Panel2.Controls.Add(this.pnlColumn);
            this.scColumns.Panel2MinSize = 400;
            this.scColumns.Size = new System.Drawing.Size(786, 350);
            this.scColumns.SplitterDistance = 354;
            this.scColumns.TabIndex = 0;
            // 
            // tscColumns
            // 
            // 
            // tscColumns.ContentPanel
            // 
            this.tscColumns.ContentPanel.Controls.Add(this.gvColumns);
            this.tscColumns.ContentPanel.Size = new System.Drawing.Size(354, 323);
            this.tscColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscColumns.Location = new System.Drawing.Point(0, 0);
            this.tscColumns.Name = "tscColumns";
            this.tscColumns.Size = new System.Drawing.Size(354, 350);
            this.tscColumns.TabIndex = 0;
            this.tscColumns.Text = "toolStripContainer1";
            // 
            // tscColumns.TopToolStripPanel
            // 
            this.tscColumns.TopToolStripPanel.Controls.Add(this.tsColumns);
            // 
            // gvColumns
            // 
            this.gvColumns.AllowUserToAddRows = false;
            this.gvColumns.AllowUserToResizeRows = false;
            this.gvColumns.AutoGenerateColumns = false;
            this.gvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gvcColumnName,
            this.gvcColumnDataType,
            this.gvcColumnPrimaryKey,
            this.gvcColumnNotNull});
            this.gvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvColumns.Location = new System.Drawing.Point(0, 0);
            this.gvColumns.MultiSelect = false;
            this.gvColumns.Name = "gvColumns";
            this.gvColumns.ReadOnly = true;
            this.gvColumns.RowHeadersWidth = 51;
            this.gvColumns.RowTemplate.Height = 29;
            this.gvColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvColumns.Size = new System.Drawing.Size(354, 323);
            this.gvColumns.TabIndex = 0;
            this.gvColumns.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvColumns_RowValidating);
            this.gvColumns.SelectionChanged += new System.EventHandler(this.gvColumns_SelectionChanged);
            // 
            // gvcColumnName
            // 
            this.gvcColumnName.DataPropertyName = "name";
            this.gvcColumnName.HeaderText = "Name";
            this.gvcColumnName.MinimumWidth = 6;
            this.gvcColumnName.Name = "gvcColumnName";
            this.gvcColumnName.ReadOnly = true;
            this.gvcColumnName.Width = 125;
            // 
            // gvcColumnDataType
            // 
            this.gvcColumnDataType.DataPropertyName = "type_name";
            this.gvcColumnDataType.HeaderText = "DataType";
            this.gvcColumnDataType.MinimumWidth = 6;
            this.gvcColumnDataType.Name = "gvcColumnDataType";
            this.gvcColumnDataType.ReadOnly = true;
            this.gvcColumnDataType.Width = 125;
            // 
            // gvcColumnPrimaryKey
            // 
            this.gvcColumnPrimaryKey.DataPropertyName = "pk";
            this.gvcColumnPrimaryKey.HeaderText = "PK";
            this.gvcColumnPrimaryKey.MinimumWidth = 6;
            this.gvcColumnPrimaryKey.Name = "gvcColumnPrimaryKey";
            this.gvcColumnPrimaryKey.ReadOnly = true;
            this.gvcColumnPrimaryKey.Width = 125;
            // 
            // gvcColumnNotNull
            // 
            this.gvcColumnNotNull.DataPropertyName = "not_null";
            this.gvcColumnNotNull.HeaderText = "NN";
            this.gvcColumnNotNull.MinimumWidth = 6;
            this.gvcColumnNotNull.Name = "gvcColumnNotNull";
            this.gvcColumnNotNull.ReadOnly = true;
            this.gvcColumnNotNull.Width = 125;
            // 
            // tsColumns
            // 
            this.tsColumns.Dock = System.Windows.Forms.DockStyle.None;
            this.tsColumns.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddColumn,
            this.tsbRemoveColumn,
            this.tsbMoveUpColumn,
            this.tsbMoveDownColumn});
            this.tsColumns.Location = new System.Drawing.Point(4, 0);
            this.tsColumns.Name = "tsColumns";
            this.tsColumns.Size = new System.Drawing.Size(71, 27);
            this.tsColumns.TabIndex = 0;
            // 
            // tsbAddColumn
            // 
            this.tsbAddColumn.Image = global::PgMulti.Properties.Resources.nuevo;
            this.tsbAddColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddColumn.Name = "tsbAdd";
            this.tsbAddColumn.Size = new System.Drawing.Size(29, 24);
            this.tsbAddColumn.Click += new System.EventHandler(this.tsbColumnAdd_Click);
            // 
            // tsbRemoveColumn
            // 
            this.tsbRemoveColumn.Enabled = false;
            this.tsbRemoveColumn.Image = global::PgMulti.Properties.Resources.borrar;
            this.tsbRemoveColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveColumn.Name = "tsbRemove";
            this.tsbRemoveColumn.Size = new System.Drawing.Size(29, 24);
            this.tsbRemoveColumn.Click += new System.EventHandler(this.tsbColumnRemove_Click);
            // 
            // tsbMoveUpColumn
            // 
            this.tsbMoveUpColumn.Enabled = false;
            this.tsbMoveUpColumn.Image = global::PgMulti.Properties.Resources.up;
            this.tsbMoveUpColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveUpColumn.Name = "tsbMoveUpColumn";
            this.tsbMoveUpColumn.Size = new System.Drawing.Size(29, 24);
            this.tsbMoveUpColumn.Click += new System.EventHandler(this.tsbMoveUpColumn_Click);
            // 
            // tsbMoveDownColumn
            // 
            this.tsbMoveDownColumn.Enabled = false;
            this.tsbMoveDownColumn.Image = global::PgMulti.Properties.Resources.down;
            this.tsbMoveDownColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveDownColumn.Name = "tsbMoveDownColumn";
            this.tsbMoveDownColumn.Size = new System.Drawing.Size(29, 24);
            this.tsbMoveDownColumn.Click += new System.EventHandler(this.tsbMoveDownColumn_Click);
            // 
            // panel3
            // 
            this.pnlColumn.AutoScroll = true;
            this.pnlColumn.Controls.Add(this.tlpColumn);
            this.pnlColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColumn.Location = new System.Drawing.Point(0, 0);
            this.pnlColumn.Name = "panel3";
            this.pnlColumn.Size = new System.Drawing.Size(428, 350);
            this.pnlColumn.TabIndex = 4;
            this.pnlColumn.Padding = new Padding(0, 30, 0, 0);
            // 
            // tlpColumn
            // 
            this.tlpColumn.AutoScroll = true;
            this.tlpColumn.AutoSize = true;
            this.tlpColumn.ColumnCount = 2;
            this.tlpColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpColumn.Controls.Add(this.lblColumnName, 0, 0);
            this.tlpColumn.Controls.Add(this.txtColumnName, 1, 0);
            this.tlpColumn.Controls.Add(this.lblColumnType, 0, 1);
            this.tlpColumn.Controls.Add(this.cbColumnType, 1, 1);
            this.tlpColumn.Controls.Add(this.lblColumnTypeInitials, 0, 2);
            this.tlpColumn.Controls.Add(this.txtColumnTypeInitials, 1, 2);
            this.tlpColumn.Controls.Add(this.lblColumnPrimaryKey, 0, 3);
            this.tlpColumn.Controls.Add(this.chkColumnPrimaryKey, 1, 3);
            this.tlpColumn.Controls.Add(this.lblColumnIdentity, 0, 4);
            this.tlpColumn.Controls.Add(this.chkColumnIdentity, 1, 4);
            this.tlpColumn.Controls.Add(this.lblColumnDefault, 0, 5);
            this.tlpColumn.Controls.Add(this.lblColumnNotNull, 0, 6);
            this.tlpColumn.Controls.Add(this.chkColumnNotNull, 1, 6);
            this.tlpColumn.Controls.Add(this.txtColumnDefault, 1, 5);
            this.tlpColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpColumn.Location = new System.Drawing.Point(0, 0);
            this.tlpColumn.Name = "tlpColumn";
            this.tlpColumn.RowCount = 8;
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpColumn.Size = new System.Drawing.Size(428, 350);
            this.tlpColumn.TabIndex = 3;
            // 
            // lblColumnName
            // 
            this.lblColumnName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnName.AutoSize = true;
            this.lblColumnName.Location = new System.Drawing.Point(3, 10);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(0, 20);
            this.lblColumnName.TabIndex = 0;
            // 
            // txtColumnName
            // 
            this.txtColumnName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColumnName.Enabled = false;
            this.txtColumnName.Location = new System.Drawing.Point(203, 6);
            this.txtColumnName.Name = "txtColumnName";
            this.txtColumnName.Size = new System.Drawing.Size(212, 27);
            this.txtColumnName.TabIndex = 0;
            this.txtColumnName.TextChanged += new System.EventHandler(this.txtColumnName_TextChanged);
            // 
            // lblColumnType
            // 
            this.lblColumnType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnType.AutoSize = true;
            this.lblColumnType.Location = new System.Drawing.Point(3, 50);
            this.lblColumnType.Name = "lblColumnType";
            this.lblColumnType.Size = new System.Drawing.Size(0, 20);
            this.lblColumnType.TabIndex = 0;
            // 
            // cbColumnType
            // 
            this.cbColumnType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbColumnType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbColumnType.Enabled = false;
            this.cbColumnType.FormattingEnabled = true;
            this.cbColumnType.Items.AddRange(new object[] {
            "bigint",
            "bigserial",
            "bit",
            "bit varying",
            "bit varying(n)",
            "bit(n)",
            "bool",
            "boolean",
            "bytea",
            "char",
            "character",
            "character varying",
            "character varying(n)",
            "character(n)",
            "cidr",
            "date",
            "datetime",
            "decimal",
            "double precision",
            "float4",
            "float8",
            "inet",
            "int2",
            "int4",
            "int8",
            "integer",
            "interval",
            "json",
            "jsonb",
            "money",
            "numeric(n,n)",
            "point",
            "real",
            "regclass",
            "regconfig",
            "regnamespace",
            "serial",
            "serial2",
            "serial4",
            "serial8",
            "smallint",
            "smallserial",
            "text",
            "time",
            "time with time zone",
            "time without time zone",
            "timestamp",
            "timestamp with time zone",
            "timestamp without time zone",
            "tsquery",
            "tsvector",
            "varbit",
            "varchar",
            "xml"});
            this.cbColumnType.Location = new System.Drawing.Point(203, 43);
            this.cbColumnType.Name = "cbColumnType";
            this.cbColumnType.Size = new System.Drawing.Size(212, 28);
            this.cbColumnType.TabIndex = 1;
            this.cbColumnType.TextChanged += new System.EventHandler(this.cbColumnType_TextChanged);
            this.cbColumnType.Enter += new System.EventHandler(this.cbColumnType_Enter);
            this.cbColumnType.Leave += new System.EventHandler(this.cbColumnType_Leave);
            this.cbColumnType.Validating += new System.ComponentModel.CancelEventHandler(this.cbColumnType_Validating);
            // 
            // lblColumnTypeInitials
            // 
            this.lblColumnTypeInitials.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnTypeInitials.AutoSize = true;
            this.lblColumnTypeInitials.Location = new System.Drawing.Point(3, 90);
            this.lblColumnTypeInitials.Name = "lblColumnTypeInitials";
            this.lblColumnTypeInitials.Size = new System.Drawing.Size(0, 20);
            this.lblColumnTypeInitials.TabIndex = 0;
            // 
            // txtColumnTypeInitials
            // 
            this.txtColumnTypeInitials.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColumnTypeInitials.Enabled = false;
            this.txtColumnTypeInitials.Location = new System.Drawing.Point(203, 86);
            this.txtColumnTypeInitials.Name = "txtColumnTypeInitials";
            this.txtColumnTypeInitials.Size = new System.Drawing.Size(144, 27);
            this.txtColumnTypeInitials.TabIndex = 2;
            this.txtColumnTypeInitials.TextChanged += new System.EventHandler(this.txtColumnTypeInitials_TextChanged);
            // 
            // lblPrimaryKey
            // 
            this.lblColumnPrimaryKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnPrimaryKey.AutoSize = true;
            this.lblColumnPrimaryKey.Location = new System.Drawing.Point(3, 130);
            this.lblColumnPrimaryKey.Name = "lblColumnPrimaryKey";
            this.lblColumnPrimaryKey.Size = new System.Drawing.Size(0, 20);
            this.lblColumnPrimaryKey.TabIndex = 0;
            // 
            // chkColumnPrimaryKey
            // 
            this.chkColumnPrimaryKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkColumnPrimaryKey.AutoSize = true;
            this.chkColumnPrimaryKey.Enabled = false;
            this.chkColumnPrimaryKey.Location = new System.Drawing.Point(203, 131);
            this.chkColumnPrimaryKey.Name = "chkColumnPrimaryKey";
            this.chkColumnPrimaryKey.Size = new System.Drawing.Size(18, 17);
            this.chkColumnPrimaryKey.TabIndex = 3;
            this.chkColumnPrimaryKey.UseVisualStyleBackColor = true;
            this.chkColumnPrimaryKey.CheckedChanged += new System.EventHandler(this.chkColumnPrimaryKey_CheckedChanged);
            // 
            // lblColumnIdentity
            // 
            this.lblColumnIdentity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnIdentity.AutoSize = true;
            this.lblColumnIdentity.Location = new System.Drawing.Point(3, 170);
            this.lblColumnIdentity.Name = "lblColumnIdentity";
            this.lblColumnIdentity.Size = new System.Drawing.Size(0, 20);
            this.lblColumnIdentity.TabIndex = 0;
            // 
            // chkColumnIdentity
            // 
            this.chkColumnIdentity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkColumnIdentity.AutoSize = true;
            this.chkColumnIdentity.Enabled = false;
            this.chkColumnIdentity.Location = new System.Drawing.Point(203, 171);
            this.chkColumnIdentity.Name = "chkColumnIdentity";
            this.chkColumnIdentity.Size = new System.Drawing.Size(18, 17);
            this.chkColumnIdentity.TabIndex = 4;
            this.chkColumnIdentity.UseVisualStyleBackColor = true;
            this.chkColumnIdentity.CheckedChanged += new System.EventHandler(this.chkColumnIdentity_CheckedChanged);
            // 
            // lblColumnDefault
            // 
            this.lblColumnDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnDefault.AutoSize = true;
            this.lblColumnDefault.Location = new System.Drawing.Point(3, 210);
            this.lblColumnDefault.Name = "lblColumnDefault";
            this.lblColumnDefault.Size = new System.Drawing.Size(0, 20);
            this.lblColumnDefault.TabIndex = 0;
            // 
            // lblColumnNotNull
            // 
            this.lblColumnNotNull.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblColumnNotNull.AutoSize = true;
            this.lblColumnNotNull.Location = new System.Drawing.Point(3, 250);
            this.lblColumnNotNull.Name = "lblColumnNotNull";
            this.lblColumnNotNull.Size = new System.Drawing.Size(0, 20);
            this.lblColumnNotNull.TabIndex = 0;
            // 
            // chkColumnNotNull
            // 
            this.chkColumnNotNull.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkColumnNotNull.AutoSize = true;
            this.chkColumnNotNull.Enabled = false;
            this.chkColumnNotNull.Location = new System.Drawing.Point(203, 251);
            this.chkColumnNotNull.Name = "chkColumnNotNull";
            this.chkColumnNotNull.Size = new System.Drawing.Size(18, 17);
            this.chkColumnNotNull.TabIndex = 6;
            this.chkColumnNotNull.UseVisualStyleBackColor = true;
            this.chkColumnNotNull.CheckedChanged += new System.EventHandler(this.chkColumnNotNull_CheckedChanged);
            // 
            // txtColumnDefault
            // 
            this.txtColumnDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColumnDefault.Enabled = false;
            this.txtColumnDefault.Location = new System.Drawing.Point(203, 206);
            this.txtColumnDefault.Name = "txtColumnDefault";
            this.txtColumnDefault.Size = new System.Drawing.Size(114, 27);
            this.txtColumnDefault.TabIndex = 5;
            this.txtColumnDefault.TextChanged += new System.EventHandler(this.txtColumnDefault_TextChanged);

            // 
            // pnlConfirmation
            // 
            this.pnlConfirmation.Controls.Add(this.flp);
            this.pnlConfirmation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlConfirmation.Location = new System.Drawing.Point(0, 389);
            this.pnlConfirmation.Name = "pnlConfirmation";
            this.pnlConfirmation.Size = new System.Drawing.Size(800, 61);
            this.pnlConfirmation.TabIndex = 3;
            // 
            // flp
            // 
            this.flp.Controls.Add(this.btnCancel);
            this.flp.Controls.Add(this.btnOk);
            this.flp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flp.Location = new System.Drawing.Point(0, 0);
            this.flp.Name = "flp";
            this.flp.Padding = new System.Windows.Forms.Padding(5);
            this.flp.Size = new System.Drawing.Size(800, 61);
            this.flp.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(680, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(560, 15);
            this.btnOk.Margin = new System.Windows.Forms.Padding(10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 29);
            this.btnOk.TabIndex = 0;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // TableForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.pnlConfirmation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.tc.ResumeLayout(false);
            this.tcGeneral.ResumeLayout(false);
            this.tlpGeneral.ResumeLayout(false);
            this.tlpGeneral.PerformLayout();
            this.tcColumns.ResumeLayout(false);
            this.scColumns.Panel1.ResumeLayout(false);
            this.scColumns.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scColumns)).EndInit();
            this.scColumns.ResumeLayout(false);
            this.tscColumns.ContentPanel.ResumeLayout(false);
            this.tscColumns.TopToolStripPanel.ResumeLayout(false);
            this.tscColumns.TopToolStripPanel.PerformLayout();
            this.tscColumns.ResumeLayout(false);
            this.tscColumns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvColumns)).EndInit();
            this.tsColumns.ResumeLayout(false);
            this.tsColumns.PerformLayout();
            this.pnlColumn.ResumeLayout(false);
            this.pnlColumn.PerformLayout();
            this.tlpColumn.ResumeLayout(false);
            this.tlpColumn.PerformLayout();
            this.pnlConfirmation.ResumeLayout(false);
            this.flp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblTableName;
        private TextBox txtTableName;
        private TabControl tc;
        private TabPage tcGeneral;
        private TabPage tcColumns;
        private Panel pnlConfirmation;
        private FlowLayoutPanel flp;
        private Button btnCancel;
        private Button btnOk;
        private TableLayoutPanel tlpGeneral;
        private TextBox txtSchemaName;
        private Label lblSchemaName;
        private SplitContainer scColumns;
        private ToolStripContainer tscColumns;
        private ToolStrip tsColumns;
        private ToolStripButton tsbAddColumn;
        private ToolStripButton tsbRemoveColumn;
        private ToolStripButton tsbMoveUpColumn;
        private ToolStripButton tsbMoveDownColumn;
        private TableLayoutPanel tlpColumn;
        private Label lblColumnName;
        private TextBox txtColumnName;
        private Label lblColumnType;
        private Label lblColumnTypeInitials;
        private Label lblColumnPrimaryKey;
        private Label lblColumnNotNull;
        private Label lblColumnIdentity;
        private ComboBox cbColumnType;
        private TextBox txtColumnTypeInitials;
        private CheckBox chkColumnPrimaryKey;
        private CheckBox chkColumnNotNull;
        private CheckBox chkColumnIdentity;
        private Label lblColumnDefault;
        private TextBox txtColumnDefault;
        private Panel pnlColumn;
        private DataGridView gvColumns;
        private DataGridViewTextBoxColumn gvcColumnName;
        private DataGridViewTextBoxColumn gvcColumnDataType;
        private DataGridViewCheckBoxColumn gvcColumnPrimaryKey;
        private DataGridViewCheckBoxColumn gvcColumnNotNull;
    }
}

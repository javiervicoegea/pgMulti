namespace PgMulti.Forms
{
    partial class TableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableForm));
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
            this.gvcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvcDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvcPrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gvcNotNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsColumns = new System.Windows.Forms.ToolStrip();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbRemove = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tlpColumn = new System.Windows.Forms.TableLayoutPanel();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.txtColumnName = new System.Windows.Forms.TextBox();
            this.lblColumnType = new System.Windows.Forms.Label();
            this.cbColumnType = new System.Windows.Forms.ComboBox();
            this.lblColumnTypeInitials = new System.Windows.Forms.Label();
            this.txtColumnTypeInitials = new System.Windows.Forms.TextBox();
            this.lblPrimaryKey = new System.Windows.Forms.Label();
            this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.chkIdentity = new System.Windows.Forms.CheckBox();
            this.lblColumnDefault = new System.Windows.Forms.Label();
            this.lblNotNull = new System.Windows.Forms.Label();
            this.chkNotNull = new System.Windows.Forms.CheckBox();
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
            this.panel3.SuspendLayout();
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
            this.scColumns.Panel2.Controls.Add(this.panel3);
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
            this.gvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gvcName,
            this.gvcDataType,
            this.gvcPrimaryKey,
            this.gvcNotNull});
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
            this.gvColumns.Enter += new System.EventHandler(this.gvColumns_Enter);
            this.gvColumns.Leave += new System.EventHandler(this.gvColumns_Leave);
            // 
            // gvcName
            // 
            this.gvcName.DataPropertyName = "name";
            this.gvcName.HeaderText = "Name";
            this.gvcName.MinimumWidth = 6;
            this.gvcName.Name = "gvcName";
            this.gvcName.ReadOnly = true;
            this.gvcName.Width = 125;
            // 
            // gvcDataType
            // 
            this.gvcDataType.DataPropertyName = "type_name";
            this.gvcDataType.HeaderText = "DataType";
            this.gvcDataType.MinimumWidth = 6;
            this.gvcDataType.Name = "gvcDataType";
            this.gvcDataType.ReadOnly = true;
            this.gvcDataType.Width = 125;
            // 
            // gvcPrimaryKey
            // 
            this.gvcPrimaryKey.DataPropertyName = "pk";
            this.gvcPrimaryKey.HeaderText = "PK";
            this.gvcPrimaryKey.MinimumWidth = 6;
            this.gvcPrimaryKey.Name = "gvcPrimaryKey";
            this.gvcPrimaryKey.ReadOnly = true;
            this.gvcPrimaryKey.Width = 125;
            // 
            // gvcNotNull
            // 
            this.gvcNotNull.DataPropertyName = "not_null";
            this.gvcNotNull.HeaderText = "NN";
            this.gvcNotNull.MinimumWidth = 6;
            this.gvcNotNull.Name = "gvcNotNull";
            this.gvcNotNull.ReadOnly = true;
            this.gvcNotNull.Width = 125;
            // 
            // tsColumns
            // 
            this.tsColumns.Dock = System.Windows.Forms.DockStyle.None;
            this.tsColumns.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd,
            this.tsbRemove});
            this.tsColumns.Location = new System.Drawing.Point(4, 0);
            this.tsColumns.Name = "tsColumns";
            this.tsColumns.Size = new System.Drawing.Size(71, 27);
            this.tsColumns.TabIndex = 0;
            // 
            // tsbAdd
            // 
            this.tsbAdd.Image = global::PgMulti.Properties.Resources.nuevo;
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(29, 24);
            this.tsbAdd.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbRemove
            // 
            this.tsbRemove.Image = global::PgMulti.Properties.Resources.borrar;
            this.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemove.Name = "tsbRemove";
            this.tsbRemove.Size = new System.Drawing.Size(29, 24);
            this.tsbRemove.Click += new System.EventHandler(this.tsbRemove_Click);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.tlpColumn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(428, 350);
            this.panel3.TabIndex = 4;
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
            this.tlpColumn.Controls.Add(this.lblPrimaryKey, 0, 3);
            this.tlpColumn.Controls.Add(this.chkPrimaryKey, 1, 3);
            this.tlpColumn.Controls.Add(this.lblIdentity, 0, 4);
            this.tlpColumn.Controls.Add(this.chkIdentity, 1, 4);
            this.tlpColumn.Controls.Add(this.lblColumnDefault, 0, 5);
            this.tlpColumn.Controls.Add(this.lblNotNull, 0, 6);
            this.tlpColumn.Controls.Add(this.chkNotNull, 1, 6);
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
            this.lblPrimaryKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrimaryKey.AutoSize = true;
            this.lblPrimaryKey.Location = new System.Drawing.Point(3, 130);
            this.lblPrimaryKey.Name = "lblPrimaryKey";
            this.lblPrimaryKey.Size = new System.Drawing.Size(0, 20);
            this.lblPrimaryKey.TabIndex = 0;
            // 
            // chkPrimaryKey
            // 
            this.chkPrimaryKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkPrimaryKey.AutoSize = true;
            this.chkPrimaryKey.Enabled = false;
            this.chkPrimaryKey.Location = new System.Drawing.Point(203, 131);
            this.chkPrimaryKey.Name = "chkPrimaryKey";
            this.chkPrimaryKey.Size = new System.Drawing.Size(18, 17);
            this.chkPrimaryKey.TabIndex = 3;
            this.chkPrimaryKey.UseVisualStyleBackColor = true;
            this.chkPrimaryKey.CheckedChanged += new System.EventHandler(this.chkPrimaryKey_CheckedChanged);
            // 
            // lblIdentity
            // 
            this.lblIdentity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblIdentity.AutoSize = true;
            this.lblIdentity.Location = new System.Drawing.Point(3, 170);
            this.lblIdentity.Name = "lblIdentity";
            this.lblIdentity.Size = new System.Drawing.Size(0, 20);
            this.lblIdentity.TabIndex = 0;
            // 
            // chkIdentity
            // 
            this.chkIdentity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIdentity.AutoSize = true;
            this.chkIdentity.Enabled = false;
            this.chkIdentity.Location = new System.Drawing.Point(203, 171);
            this.chkIdentity.Name = "chkIdentity";
            this.chkIdentity.Size = new System.Drawing.Size(18, 17);
            this.chkIdentity.TabIndex = 4;
            this.chkIdentity.UseVisualStyleBackColor = true;
            this.chkIdentity.CheckedChanged += new System.EventHandler(this.chkIdentity_CheckedChanged);
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
            // lblNotNull
            // 
            this.lblNotNull.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNotNull.AutoSize = true;
            this.lblNotNull.Location = new System.Drawing.Point(3, 250);
            this.lblNotNull.Name = "lblNotNull";
            this.lblNotNull.Size = new System.Drawing.Size(0, 20);
            this.lblNotNull.TabIndex = 0;
            // 
            // chkNotNull
            // 
            this.chkNotNull.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkNotNull.AutoSize = true;
            this.chkNotNull.Enabled = false;
            this.chkNotNull.Location = new System.Drawing.Point(203, 251);
            this.chkNotNull.Name = "chkNotNull";
            this.chkNotNull.Size = new System.Drawing.Size(18, 17);
            this.chkNotNull.TabIndex = 6;
            this.chkNotNull.UseVisualStyleBackColor = true;
            this.chkNotNull.CheckedChanged += new System.EventHandler(this.chkNotNull_CheckedChanged);
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
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
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
        private ToolStripButton tsbAdd;
        private ToolStripButton tsbRemove;
        private TableLayoutPanel tlpColumn;
        private Label lblColumnName;
        private TextBox txtColumnName;
        private Label lblColumnType;
        private Label lblColumnTypeInitials;
        private Label lblPrimaryKey;
        private Label lblNotNull;
        private Label lblIdentity;
        private ComboBox cbColumnType;
        private TextBox txtColumnTypeInitials;
        private CheckBox chkPrimaryKey;
        private CheckBox chkNotNull;
        private CheckBox chkIdentity;
        private Label lblColumnDefault;
        private TextBox txtColumnDefault;
        private Panel panel3;
        private DataGridView gvColumns;
        private DataGridViewTextBoxColumn gvcName;
        private DataGridViewTextBoxColumn gvcDataType;
        private DataGridViewCheckBoxColumn gvcPrimaryKey;
        private DataGridViewCheckBoxColumn gvcNotNull;
    }
}
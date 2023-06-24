namespace PgMulti.Forms
{
    partial class ExpandDiagramPanel
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
            this.components = new System.ComponentModel.Container();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.tbRepulsion = new System.Windows.Forms.TrackBar();
            this.chkAutoZoom = new System.Windows.Forms.CheckBox();
            this.chkEnableRepulsion = new System.Windows.Forms.CheckBox();
            this.chkSuggestAddRelatedTables = new System.Windows.Forms.CheckBox();
            this.txtSuggestAddRelatedTablesSelectedDB = new System.Windows.Forms.TextBox();
            this.btnSuggestAddRelatedTablesSelectDB = new System.Windows.Forms.Button();
            this.tmrRelocator = new System.Windows.Forms.Timer(this.components);
            this.lblRepulsion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbRepulsion)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExplanation
            // 
            this.lblExplanation.BackColor = System.Drawing.Color.Transparent;
            this.lblExplanation.Location = new System.Drawing.Point(12, 9);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(604, 65);
            this.lblExplanation.TabIndex = 0;
            this.lblExplanation.Text = "label1";
            // 
            // tbRepulsion
            // 
            this.tbRepulsion.AutoSize = false;
            this.tbRepulsion.Enabled = false;
            this.tbRepulsion.LargeChange = 1000000;
            this.tbRepulsion.Location = new System.Drawing.Point(196, 122);
            this.tbRepulsion.Maximum = 8000000;
            this.tbRepulsion.Minimum = 1000000;
            this.tbRepulsion.Name = "tbRepulsion";
            this.tbRepulsion.Size = new System.Drawing.Size(420, 32);
            this.tbRepulsion.SmallChange = 1000000;
            this.tbRepulsion.TabIndex = 1;
            this.tbRepulsion.TickFrequency = 1000000;
            this.tbRepulsion.Value = 1000000;
            this.tbRepulsion.Scroll += new System.EventHandler(this.tbRepulsion_Scroll);
            // 
            // chkAutoZoom
            // 
            this.chkAutoZoom.AutoSize = true;
            this.chkAutoZoom.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoZoom.Checked = true;
            this.chkAutoZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoZoom.Location = new System.Drawing.Point(12, 203);
            this.chkAutoZoom.Name = "chkAutoZoom";
            this.chkAutoZoom.Size = new System.Drawing.Size(101, 24);
            this.chkAutoZoom.TabIndex = 3;
            this.chkAutoZoom.Text = "checkBox1";
            this.chkAutoZoom.UseVisualStyleBackColor = false;
            // 
            // chkEnableRepulsion
            // 
            this.chkEnableRepulsion.AutoSize = true;
            this.chkEnableRepulsion.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableRepulsion.Location = new System.Drawing.Point(12, 80);
            this.chkEnableRepulsion.Name = "chkEnableRepulsion";
            this.chkEnableRepulsion.Size = new System.Drawing.Size(101, 24);
            this.chkEnableRepulsion.TabIndex = 4;
            this.chkEnableRepulsion.Text = "checkBox1";
            this.chkEnableRepulsion.UseVisualStyleBackColor = false;
            this.chkEnableRepulsion.CheckedChanged += new System.EventHandler(this.chkEnableRepulsion_CheckedChanged);
            // 
            // chkSuggestAddRelatedTables
            // 
            this.chkSuggestAddRelatedTables.AutoSize = true;
            this.chkSuggestAddRelatedTables.BackColor = System.Drawing.Color.Transparent;
            this.chkSuggestAddRelatedTables.Location = new System.Drawing.Point(12, 265);
            this.chkSuggestAddRelatedTables.Name = "chkSuggestAddRelatedTables";
            this.chkSuggestAddRelatedTables.Size = new System.Drawing.Size(101, 24);
            this.chkSuggestAddRelatedTables.TabIndex = 5;
            this.chkSuggestAddRelatedTables.Text = "checkBox1";
            this.chkSuggestAddRelatedTables.UseVisualStyleBackColor = false;
            this.chkSuggestAddRelatedTables.CheckedChanged += new System.EventHandler(this.chkSuggestAddRelatedTables_CheckedChanged);
            // 
            // txtSuggestAddRelatedTablesSelectedDB
            // 
            this.txtSuggestAddRelatedTablesSelectedDB.Enabled = false;
            this.txtSuggestAddRelatedTablesSelectedDB.Location = new System.Drawing.Point(36, 317);
            this.txtSuggestAddRelatedTablesSelectedDB.Name = "txtSuggestAddRelatedTablesSelectedDB";
            this.txtSuggestAddRelatedTablesSelectedDB.ReadOnly = true;
            this.txtSuggestAddRelatedTablesSelectedDB.Size = new System.Drawing.Size(442, 27);
            this.txtSuggestAddRelatedTablesSelectedDB.TabIndex = 6;
            // 
            // btnSuggestAddRelatedTablesSelectDB
            // 
            this.btnSuggestAddRelatedTablesSelectDB.Enabled = false;
            this.btnSuggestAddRelatedTablesSelectDB.Location = new System.Drawing.Point(484, 316);
            this.btnSuggestAddRelatedTablesSelectDB.Name = "btnSuggestAddRelatedTablesSelectDB";
            this.btnSuggestAddRelatedTablesSelectDB.Size = new System.Drawing.Size(132, 29);
            this.btnSuggestAddRelatedTablesSelectDB.TabIndex = 7;
            this.btnSuggestAddRelatedTablesSelectDB.Text = "button1";
            this.btnSuggestAddRelatedTablesSelectDB.UseVisualStyleBackColor = true;
            this.btnSuggestAddRelatedTablesSelectDB.Click += new System.EventHandler(this.btnSuggestAddRelatedTablesSelectDB_Click);
            // 
            // tmrRelocator
            // 
            this.tmrRelocator.Interval = 50;
            this.tmrRelocator.Tick += new System.EventHandler(this.tmrRelocator_Tick);
            // 
            // lblRepulsion
            // 
            this.lblRepulsion.AutoSize = true;
            this.lblRepulsion.Enabled = false;
            this.lblRepulsion.Location = new System.Drawing.Point(36, 128);
            this.lblRepulsion.Name = "lblRepulsion";
            this.lblRepulsion.Size = new System.Drawing.Size(50, 20);
            this.lblRepulsion.TabIndex = 8;
            this.lblRepulsion.Text = "label1";
            // 
            // ExpandDiagramPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lblRepulsion);
            this.Controls.Add(this.btnSuggestAddRelatedTablesSelectDB);
            this.Controls.Add(this.txtSuggestAddRelatedTablesSelectedDB);
            this.Controls.Add(this.chkSuggestAddRelatedTables);
            this.Controls.Add(this.chkEnableRepulsion);
            this.Controls.Add(this.chkAutoZoom);
            this.Controls.Add(this.tbRepulsion);
            this.Controls.Add(this.lblExplanation);
            this.Name = "ExpandDiagramPanel";
            this.Size = new System.Drawing.Size(637, 369);
            ((System.ComponentModel.ISupportInitialize)(this.tbRepulsion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblExplanation;
        private TrackBar tbRepulsion;
        private System.Windows.Forms.Timer tmrRelocator;
        private CheckBox chkAutoZoom;
        private CheckBox chkEnableRepulsion;
        private CheckBox chkSuggestAddRelatedTables;
        private TextBox txtSuggestAddRelatedTablesSelectedDB;
        private Button btnSuggestAddRelatedTablesSelectDB;
        private Label lblRepulsion;
    }
}

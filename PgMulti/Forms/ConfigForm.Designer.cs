namespace PgMulti
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblKeepServerSelection = new System.Windows.Forms.Label();
            this.chkKeepServerSelection = new System.Windows.Forms.CheckBox();
            this.lblAutocomplete = new System.Windows.Forms.Label();
            this.chkAutocomplete = new System.Windows.Forms.CheckBox();
            this.txtAutocompleteDelay = new System.Windows.Forms.TextBox();
            this.lblAutocompleteDelay = new System.Windows.Forms.Label();
            this.lblMergeTables = new System.Windows.Forms.Label();
            this.chkMergeTables = new System.Windows.Forms.CheckBox();
            this.lblManualTransactionModeTitle = new System.Windows.Forms.Label();
            this.cbTransactionMode = new System.Windows.Forms.ComboBox();
            this.tc = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.lblMaxRowsInfo = new System.Windows.Forms.Label();
            this.txtMaxRows = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.lblMaxRowsTitle = new System.Windows.Forms.Label();
            this.lblMaxRows = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblAutoCoordinatedTransactionModeInfo = new System.Windows.Forms.Label();
            this.lblAutoSingleTransactionModeInfo = new System.Windows.Forms.Label();
            this.lblManualTransactionModeInfo = new System.Windows.Forms.Label();
            this.cbTransactionLevel = new System.Windows.Forms.ComboBox();
            this.lblTransactionLevel = new System.Windows.Forms.Label();
            this.lblTransactionMode = new System.Windows.Forms.Label();
            this.lblAutoCoordinatedTransactionModeTitle = new System.Windows.Forms.Label();
            this.lblAutoSingleTransactionModeTitle = new System.Windows.Forms.Label();
            this.tc.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(902, 753);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 29);
            this.btnOk.TabIndex = 1;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1002, 753);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblKeepServerSelection
            // 
            this.lblKeepServerSelection.Location = new System.Drawing.Point(25, 44);
            this.lblKeepServerSelection.Name = "lblKeepServerSelection";
            this.lblKeepServerSelection.Size = new System.Drawing.Size(1065, 65);
            this.lblKeepServerSelection.TabIndex = 2;
            this.lblKeepServerSelection.Text = "x";
            // 
            // chkKeepServerSelection
            // 
            this.chkKeepServerSelection.AutoSize = true;
            this.chkKeepServerSelection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkKeepServerSelection.Location = new System.Drawing.Point(6, 15);
            this.chkKeepServerSelection.Name = "chkKeepServerSelection";
            this.chkKeepServerSelection.Size = new System.Drawing.Size(18, 17);
            this.chkKeepServerSelection.TabIndex = 0;
            this.chkKeepServerSelection.UseVisualStyleBackColor = true;
            // 
            // lblAutocomplete
            // 
            this.lblAutocomplete.Location = new System.Drawing.Point(25, 141);
            this.lblAutocomplete.Name = "lblAutocomplete";
            this.lblAutocomplete.Size = new System.Drawing.Size(1065, 83);
            this.lblAutocomplete.TabIndex = 2;
            this.lblAutocomplete.Text = "x";
            // 
            // chkAutocomplete
            // 
            this.chkAutocomplete.AutoSize = true;
            this.chkAutocomplete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkAutocomplete.Location = new System.Drawing.Point(6, 112);
            this.chkAutocomplete.Name = "chkAutocomplete";
            this.chkAutocomplete.Size = new System.Drawing.Size(18, 17);
            this.chkAutocomplete.TabIndex = 1;
            this.chkAutocomplete.UseVisualStyleBackColor = true;
            this.chkAutocomplete.CheckedChanged += new System.EventHandler(this.chkAutocomplete_CheckedChanged);
            // 
            // txtAutocompleteDelay
            // 
            this.txtAutocompleteDelay.Location = new System.Drawing.Point(126, 221);
            this.txtAutocompleteDelay.Name = "txtAutocompleteDelay";
            this.txtAutocompleteDelay.Size = new System.Drawing.Size(125, 27);
            this.txtAutocompleteDelay.TabIndex = 3;
            // 
            // lblAutocompleteDelay
            // 
            this.lblAutocompleteDelay.AutoSize = true;
            this.lblAutocompleteDelay.Location = new System.Drawing.Point(25, 224);
            this.lblAutocompleteDelay.Name = "lblAutocompleteDelay";
            this.lblAutocompleteDelay.Size = new System.Drawing.Size(0, 20);
            this.lblAutocompleteDelay.TabIndex = 2;
            // 
            // lblMergeTables
            // 
            this.lblMergeTables.Location = new System.Drawing.Point(25, 296);
            this.lblMergeTables.Name = "lblMergeTables";
            this.lblMergeTables.Size = new System.Drawing.Size(1065, 61);
            this.lblMergeTables.TabIndex = 2;
            this.lblMergeTables.Text = "x";
            // 
            // chkMergeTables
            // 
            this.chkMergeTables.AutoSize = true;
            this.chkMergeTables.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkMergeTables.Location = new System.Drawing.Point(6, 267);
            this.chkMergeTables.Name = "chkMergeTables";
            this.chkMergeTables.Size = new System.Drawing.Size(18, 17);
            this.chkMergeTables.TabIndex = 4;
            this.chkMergeTables.UseVisualStyleBackColor = true;
            // 
            // lblManualTransactionModeTitle
            // 
            this.lblManualTransactionModeTitle.AutoSize = true;
            this.lblManualTransactionModeTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblManualTransactionModeTitle.Location = new System.Drawing.Point(47, 89);
            this.lblManualTransactionModeTitle.Name = "lblManualTransactionModeTitle";
            this.lblManualTransactionModeTitle.Size = new System.Drawing.Size(0, 20);
            this.lblManualTransactionModeTitle.TabIndex = 2;
            // 
            // cbTransactionMode
            // 
            this.cbTransactionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransactionMode.FormattingEnabled = true;
            this.cbTransactionMode.Location = new System.Drawing.Point(107, 39);
            this.cbTransactionMode.Name = "cbTransactionMode";
            this.cbTransactionMode.Size = new System.Drawing.Size(507, 28);
            this.cbTransactionMode.TabIndex = 1;
            this.cbTransactionMode.SelectedIndexChanged += new System.EventHandler(this.cbTransactionMode_SelectedIndexChanged);
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tabPage1);
            this.tc.Controls.Add(this.tabPage2);
            this.tc.Location = new System.Drawing.Point(12, 12);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(1084, 682);
            this.tc.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbLanguage);
            this.tabPage1.Controls.Add(this.chkKeepServerSelection);
            this.tabPage1.Controls.Add(this.lblKeepServerSelection);
            this.tabPage1.Controls.Add(this.lblMaxRowsInfo);
            this.tabPage1.Controls.Add(this.lblMergeTables);
            this.tabPage1.Controls.Add(this.txtMaxRows);
            this.tabPage1.Controls.Add(this.txtAutocompleteDelay);
            this.tabPage1.Controls.Add(this.lblLanguage);
            this.tabPage1.Controls.Add(this.lblMaxRowsTitle);
            this.tabPage1.Controls.Add(this.lblMaxRows);
            this.tabPage1.Controls.Add(this.chkMergeTables);
            this.tabPage1.Controls.Add(this.lblAutocompleteDelay);
            this.tabPage1.Controls.Add(this.lblAutocomplete);
            this.tabPage1.Controls.Add(this.chkAutocomplete);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1076, 649);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(126, 580);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(298, 28);
            this.cbLanguage.TabIndex = 9;
            // 
            // lblMaxRowsInfo
            // 
            this.lblMaxRowsInfo.Location = new System.Drawing.Point(23, 392);
            this.lblMaxRowsInfo.Name = "lblMaxRowsInfo";
            this.lblMaxRowsInfo.Size = new System.Drawing.Size(1065, 115);
            this.lblMaxRowsInfo.TabIndex = 2;
            this.lblMaxRowsInfo.Text = "x";
            // 
            // txtMaxRows
            // 
            this.txtMaxRows.Location = new System.Drawing.Point(126, 517);
            this.txtMaxRows.Name = "txtMaxRows";
            this.txtMaxRows.Size = new System.Drawing.Size(125, 27);
            this.txtMaxRows.TabIndex = 7;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLanguage.Location = new System.Drawing.Point(25, 583);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(0, 20);
            this.lblLanguage.TabIndex = 8;
            // 
            // lblMaxRowsTitle
            // 
            this.lblMaxRowsTitle.AutoSize = true;
            this.lblMaxRowsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMaxRowsTitle.Location = new System.Drawing.Point(25, 363);
            this.lblMaxRowsTitle.Name = "lblMaxRowsTitle";
            this.lblMaxRowsTitle.Size = new System.Drawing.Size(0, 20);
            this.lblMaxRowsTitle.TabIndex = 5;
            // 
            // lblMaxRows
            // 
            this.lblMaxRows.AutoSize = true;
            this.lblMaxRows.Location = new System.Drawing.Point(23, 520);
            this.lblMaxRows.Name = "lblMaxRows";
            this.lblMaxRows.Size = new System.Drawing.Size(0, 20);
            this.lblMaxRows.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblAutoCoordinatedTransactionModeInfo);
            this.tabPage2.Controls.Add(this.lblAutoSingleTransactionModeInfo);
            this.tabPage2.Controls.Add(this.lblManualTransactionModeInfo);
            this.tabPage2.Controls.Add(this.cbTransactionLevel);
            this.tabPage2.Controls.Add(this.cbTransactionMode);
            this.tabPage2.Controls.Add(this.lblTransactionLevel);
            this.tabPage2.Controls.Add(this.lblTransactionMode);
            this.tabPage2.Controls.Add(this.lblAutoCoordinatedTransactionModeTitle);
            this.tabPage2.Controls.Add(this.lblAutoSingleTransactionModeTitle);
            this.tabPage2.Controls.Add(this.lblManualTransactionModeTitle);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1076, 649);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblAutoCoordinatedTransactionModeInfo
            // 
            this.lblAutoCoordinatedTransactionModeInfo.Location = new System.Drawing.Point(47, 361);
            this.lblAutoCoordinatedTransactionModeInfo.Name = "lblAutoCoordinatedTransactionModeInfo";
            this.lblAutoCoordinatedTransactionModeInfo.Size = new System.Drawing.Size(994, 120);
            this.lblAutoCoordinatedTransactionModeInfo.TabIndex = 8;
            this.lblAutoCoordinatedTransactionModeInfo.Text = "x";
            // 
            // lblAutoSingleTransactionModeInfo
            // 
            this.lblAutoSingleTransactionModeInfo.Location = new System.Drawing.Point(47, 257);
            this.lblAutoSingleTransactionModeInfo.Name = "lblAutoSingleTransactionModeInfo";
            this.lblAutoSingleTransactionModeInfo.Size = new System.Drawing.Size(994, 79);
            this.lblAutoSingleTransactionModeInfo.TabIndex = 8;
            this.lblAutoSingleTransactionModeInfo.Text = "x";
            // 
            // lblManualTransactionModeInfo
            // 
            this.lblManualTransactionModeInfo.Location = new System.Drawing.Point(47, 114);
            this.lblManualTransactionModeInfo.Name = "lblManualTransactionModeInfo";
            this.lblManualTransactionModeInfo.Size = new System.Drawing.Size(994, 108);
            this.lblManualTransactionModeInfo.TabIndex = 8;
            this.lblManualTransactionModeInfo.Text = "x";
            // 
            // cbTransactionLevel
            // 
            this.cbTransactionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransactionLevel.FormattingEnabled = true;
            this.cbTransactionLevel.Items.AddRange(new object[] {
            "Read committed",
            "Repeatable read",
            "Serializable"});
            this.cbTransactionLevel.Location = new System.Drawing.Point(107, 517);
            this.cbTransactionLevel.Name = "cbTransactionLevel";
            this.cbTransactionLevel.Size = new System.Drawing.Size(507, 28);
            this.cbTransactionLevel.TabIndex = 6;
            // 
            // lblTransactionLevel
            // 
            this.lblTransactionLevel.AutoSize = true;
            this.lblTransactionLevel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTransactionLevel.Location = new System.Drawing.Point(47, 520);
            this.lblTransactionLevel.Name = "lblTransactionLevel";
            this.lblTransactionLevel.Size = new System.Drawing.Size(0, 20);
            this.lblTransactionLevel.TabIndex = 5;
            // 
            // lblTransactionMode
            // 
            this.lblTransactionMode.AutoSize = true;
            this.lblTransactionMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTransactionMode.Location = new System.Drawing.Point(47, 42);
            this.lblTransactionMode.Name = "lblTransactionMode";
            this.lblTransactionMode.Size = new System.Drawing.Size(0, 20);
            this.lblTransactionMode.TabIndex = 0;
            // 
            // lblAutoCoordinatedTransactionModeTitle
            // 
            this.lblAutoCoordinatedTransactionModeTitle.AutoSize = true;
            this.lblAutoCoordinatedTransactionModeTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAutoCoordinatedTransactionModeTitle.Location = new System.Drawing.Point(47, 336);
            this.lblAutoCoordinatedTransactionModeTitle.Name = "lblAutoCoordinatedTransactionModeTitle";
            this.lblAutoCoordinatedTransactionModeTitle.Size = new System.Drawing.Size(0, 20);
            this.lblAutoCoordinatedTransactionModeTitle.TabIndex = 4;
            // 
            // lblAutoSingleTransactionModeTitle
            // 
            this.lblAutoSingleTransactionModeTitle.AutoSize = true;
            this.lblAutoSingleTransactionModeTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAutoSingleTransactionModeTitle.Location = new System.Drawing.Point(47, 232);
            this.lblAutoSingleTransactionModeTitle.Name = "lblAutoSingleTransactionModeTitle";
            this.lblAutoSingleTransactionModeTitle.Size = new System.Drawing.Size(0, 20);
            this.lblAutoSingleTransactionModeTitle.TabIndex = 3;
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1108, 794);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.tc.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblKeepServerSelection;
        private CheckBox chkKeepServerSelection;
        private Label lblAutocomplete;
        private CheckBox chkAutocomplete;
        private TextBox txtAutocompleteDelay;
        private Label lblAutocompleteDelay;
        private Label lblMergeTables;
        private CheckBox chkMergeTables;
        private Label lblManualTransactionModeTitle;
        private ComboBox cbTransactionMode;
        private TabControl tc;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label lblManualTransactionModeInfo;
        private Label lblAutoCoordinatedTransactionModeInfo;
        private Label lblAutoSingleTransactionModeInfo;
        private Label lblTransactionMode;
        private Label lblAutoCoordinatedTransactionModeTitle;
        private Label lblAutoSingleTransactionModeTitle;
        private ComboBox cbTransactionLevel;
        private Label lblTransactionLevel;
        private Label lblMaxRowsInfo;
        private Label lblMaxRowsTitle;
        private Label lblMaxRows;
        private TextBox txtMaxRows;
        private ComboBox cbLanguage;
        private Label lblLanguage;
    }
}
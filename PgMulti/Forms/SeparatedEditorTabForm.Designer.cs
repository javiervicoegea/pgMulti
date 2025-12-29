using FastColoredTextBoxNS;
using System.Windows.Forms;

namespace PgMulti
{
    partial class SeparatedEditorTabForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextBoxForm));
            splitContainer = new SplitContainer();
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
            toolStripContainer = new ToolStripContainer();
            toolStrip = new ToolStrip();
            tsbSave = new ToolStripButton();
            tsbSaveAs = new ToolStripButton();
            tsbSearchAndReplace = new ToolStripButton();
            tsbGoTo = new ToolStripButton();
            tsbFormat = new ToolStripButton();
            tsddbErrors = new ToolStripDropDownButton();
            tslPosition = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripSeparator4 = new ToolStripSeparator();

            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            tlpSearchAndReplace.SuspendLayout();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.TopToolStripPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            toolStrip.SuspendLayout();
            this.SuspendLayout();

            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.FixedPanel = FixedPanel.Panel1;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tlpSearchAndReplace);
            splitContainer.Panel1Collapsed = true;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer.Panel2.Controls.Add(toolStripContainer);
            splitContainer.Size = new Size(1539, 740);
            splitContainer.SplitterDistance = 410;
            splitContainer.TabIndex = 0;
            // 
            // tlpSearchAndReplace
            // 
            tlpSearchAndReplace.AutoScroll = true;
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
            // toolStrip
            // 
            toolStrip.Dock = DockStyle.None;
            toolStrip.ImageScalingSize = new Size(30, 30);
            toolStrip.Items.AddRange(new ToolStripItem[] { tsbSave, tsbSaveAs, toolStripSeparator1, tsbSearchAndReplace, tsbGoTo, toolStripSeparator2, tsbFormat, toolStripSeparator3, tsddbErrors, toolStripSeparator4, tslPosition });
            toolStrip.Location = new Point(4, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(511, 37);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
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
            // tsbSaveAs
            // 
            tsbSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSaveAs.Image = Properties.Resources.guardar_como;
            tsbSaveAs.ImageTransparentColor = Color.Magenta;
            tsbSaveAs.Name = "tsbSaveAs";
            tsbSaveAs.Size = new Size(34, 34);
            tsbSaveAs.Text = "toolStripButton1";
            tsbSaveAs.Click += tsbSaveAs_Click;
            // 
            // tsbSearchAndReplace
            // 
            tsbSearchAndReplace.CheckOnClick = true;
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
            // tsddbErrors
            // 
            tsddbErrors.Image = Properties.Resources.ok;
            tsddbErrors.ImageTransparentColor = Color.Magenta;
            tsddbErrors.Name = "tsddbErrors";
            tsddbErrors.Size = new Size(44, 34);
            // 
            // tslPosition
            // 
            tslPosition.Name = "tslPosition";
            tslPosition.Size = new Size(0, 34);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 37);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 37);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 37);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 37);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.Size = new Size(1125, 703);
            toolStripContainer.Dock = DockStyle.Fill;
            toolStripContainer.Location = new Point(0, 0);
            toolStripContainer.Name = "toolStripContainer1";
            toolStripContainer.Size = new Size(1125, 740);
            toolStripContainer.TabIndex = 1;
            toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);
            // 
            // CustomFctbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 486);
            this.Controls.Add(splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomFctbForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.PerformLayout();
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            tlpSearchAndReplace.ResumeLayout(false);
            tlpSearchAndReplace.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);

            this.ResumeLayout(false);
            this.FormClosing += CustomFctbForm_FormClosing;
            this.Load += SeparatedEditorTabForm_Load;
        }

        #endregion

        private SplitContainer splitContainer;
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
        private ToolStrip toolStrip;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbSaveAs;
        private ToolStripContainer toolStripContainer;
        private ToolStripButton tsbSearchAndReplace;
        private ToolStripButton tsbGoTo;
        private ToolStripButton tsbFormat;
        private ToolStripDropDownButton tsddbErrors;
        private ToolStripLabel tslPosition;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
    }
}
using FastColoredTextBoxNS;

namespace PgMulti
{
    partial class TextBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextBoxForm));
            txtText = new FastColoredTextBox();
            tsc = new ToolStripContainer();
            tsToolbar = new ToolStrip();
            tsbWordWrap = new ToolStripButton();
            tsbOpenFile = new ToolStripButton();
            tsbSaveFile = new ToolStripButton();
            tsbNull = new ToolStripButton();
            ofdOpenFile = new OpenFileDialog();
            sfdSaveFile = new SaveFileDialog();
            btnCancel = new Button();
            btnOk = new Button();
            ((System.ComponentModel.ISupportInitialize)txtText).BeginInit();
            tsc.ContentPanel.SuspendLayout();
            tsc.TopToolStripPanel.SuspendLayout();
            tsc.SuspendLayout();
            tsToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // txtText
            // 
            txtText.AutoCompleteBracketsList = new char[]
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
            txtText.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            txtText.AutoScrollMinSize = new Size(0, 59);
            txtText.BackBrush = null;
            txtText.CharHeight = 19;
            txtText.CharWidth = 10;
            txtText.DefaultMarkerSize = 8;
            txtText.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            txtText.Dock = DockStyle.Fill;
            txtText.Font = new Font("Cascadia Code", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtText.Hotkeys = resources.GetString("txtText.Hotkeys");
            txtText.IsReplaceMode = false;
            txtText.Location = new Point(0, 0);
            txtText.Name = "txtText";
            txtText.Paddings = new Padding(20);
            txtText.ReadOnly = true;
            txtText.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            txtText.ServiceColors = (ServiceColors)resources.GetObject("txtText.ServiceColors");
            txtText.Size = new Size(745, 411);
            txtText.TabIndex = 0;
            txtText.WordWrap = true;
            txtText.Zoom = 100;
            txtText.KeyUp += txtText_KeyUp;
            // 
            // tsc
            // 
            tsc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // tsc.ContentPanel
            // 
            tsc.ContentPanel.Controls.Add(txtText);
            tsc.ContentPanel.Size = new Size(745, 411);
            tsc.Location = new Point(0, 0);
            tsc.Name = "tsc";
            tsc.Size = new Size(745, 438);
            tsc.TabIndex = 1;
            tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            tsc.TopToolStripPanel.Controls.Add(tsToolbar);
            // 
            // tsToolbar
            // 
            tsToolbar.Dock = DockStyle.None;
            tsToolbar.ImageScalingSize = new Size(20, 20);
            tsToolbar.Items.AddRange(new ToolStripItem[] { tsbWordWrap, tsbOpenFile, tsbSaveFile, tsbNull });
            tsToolbar.Location = new Point(4, 0);
            tsToolbar.Name = "tsToolbar";
            tsToolbar.Size = new Size(129, 27);
            tsToolbar.TabIndex = 0;
            // 
            // tsbWordWrap
            // 
            tsbWordWrap.Checked = true;
            tsbWordWrap.CheckOnClick = true;
            tsbWordWrap.CheckState = CheckState.Checked;
            tsbWordWrap.Image = Properties.Resources.text_wrap;
            tsbWordWrap.ImageTransparentColor = Color.Magenta;
            tsbWordWrap.Name = "tsbWordWrap";
            tsbWordWrap.Size = new Size(29, 24);
            tsbWordWrap.Click += tsbWordWrap_Click;
            // 
            // tsbOpenFile
            // 
            tsbOpenFile.Image = Properties.Resources.abrir;
            tsbOpenFile.ImageTransparentColor = Color.Magenta;
            tsbOpenFile.Name = "tsbOpenFile";
            tsbOpenFile.Size = new Size(29, 24);
            tsbOpenFile.Click += tsbOpenFile_Click;
            // 
            // tsbSaveFile
            // 
            tsbSaveFile.Image = Properties.Resources.guardar_como;
            tsbSaveFile.ImageTransparentColor = Color.Magenta;
            tsbSaveFile.Name = "tsbSaveFile";
            tsbSaveFile.Size = new Size(29, 24);
            tsbSaveFile.Click += tsbSaveFile_Click;
            // 
            // tsbNull
            // 
            tsbNull.CheckOnClick = true;
            tsbNull.Image = Properties.Resources._null;
            tsbNull.ImageTransparentColor = Color.Magenta;
            tsbNull.Name = "tsbNull";
            tsbNull.Size = new Size(29, 24);
            tsbNull.Click += tsbNull_Click;
            // 
            // ofdOpenFile
            // 
            ofdOpenFile.FileName = "txt";
            ofdOpenFile.FilterIndex = 0;
            // 
            // sfdSaveFile
            // 
            sfdSaveFile.DefaultExt = "txt";
            sfdSaveFile.FilterIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(624, 444);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(109, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(509, 444);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(109, 29);
            btnOk.TabIndex = 4;
            btnOk.Text = "btnOk";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // TextBoxForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(745, 485);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(tsc);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TextBoxForm";
            StartPosition = FormStartPosition.CenterParent;
            Shown += TextBoxForm_Shown;
            ((System.ComponentModel.ISupportInitialize)txtText).EndInit();
            tsc.ContentPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.PerformLayout();
            tsc.ResumeLayout(false);
            tsc.PerformLayout();
            tsToolbar.ResumeLayout(false);
            tsToolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FastColoredTextBox txtText;
        private ToolStripContainer tsc;
        private ToolStrip tsToolbar;
        private ToolStripButton tsbWordWrap;
        private ToolStripButton tsbOpenFile;
        private ToolStripButton tsbSaveFile;
        private ToolStripButton tsbNull;
        private OpenFileDialog ofdOpenFile;
        private SaveFileDialog sfdSaveFile;
        private Button btnCancel;
        private Button btnOk;
    }
}
using PgMulti.QueryEditor;

namespace PgMulti
{
    partial class ConfirmSqlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmSqlForm));
            tsc = new ToolStripContainer();
            ts = new ToolStrip();
            tslSummary = new ToolStripLabel();
            tsddbScript = new ToolStripDropDownButton();
            tsbEdit = new ToolStripButton();
            btnCancel = new Button();
            btnRunAll = new Button();
            fctbSql = new PgMulti.QueryEditor.CustomFctb();
            tsc.TopToolStripPanel.SuspendLayout();
            tsc.SuspendLayout();
            ts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).BeginInit();
            SuspendLayout();
            // 
            // tsc
            // 
            tsc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // tsc.ContentPanel
            // 
            tsc.ContentPanel.Controls.Add(fctbSql);
            tsc.ContentPanel.Size = new Size(707, 551);
            tsc.Location = new Point(0, 0);
            tsc.Name = "tsc";
            tsc.Size = new Size(1191, 580);
            tsc.TabIndex = 0;
            tsc.Text = "toolStripContainer1";
            // 
            // tsc.TopToolStripPanel
            // 
            tsc.TopToolStripPanel.Controls.Add(ts);
            // 
            // fctbSql
            // 
            this.fctbSql.AutoCompleteBracketsList = new char[] {
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
            this.fctbSql.BackBrush = null;
            this.fctbSql.CharHeight = 19;
            this.fctbSql.CharWidth = 10;
            this.fctbSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctbSql.Font = new System.Drawing.Font("Cascadia Code", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fctbSql.Hotkeys = resources.GetString("fctbSql.Hotkeys");
            this.fctbSql.IsReplaceMode = false;
            this.fctbSql.Location = new System.Drawing.Point(0, 0);
            this.fctbSql.Name = "fctbSql";
            this.fctbSql.Paddings = new System.Windows.Forms.Padding(20);
            this.fctbSql.ReadOnly = true;
            this.fctbSql.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbSql.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbSql.ServiceColors")));
            this.fctbSql.Size = new System.Drawing.Size(1191, 590);
            this.fctbSql.TabIndex = 0;
            this.fctbSql.Zoom = 100;
            // 
            // ts
            // 
            ts.Dock = DockStyle.None;
            ts.ImageScalingSize = new Size(30, 30);
            ts.Items.AddRange(new ToolStripItem[] { tslSummary, tsddbScript, tsbEdit });
            ts.Location = new Point(4, 0);
            ts.Name = "ts";
            ts.Size = new Size(137, 37);
            ts.TabIndex = 0;
            // 
            // tslSummary
            // 
            tslSummary.Name = "tslSummary";
            tslSummary.Size = new Size(23, 34);
            tslSummary.Text = "xx";
            // 
            // tsddbScript
            // 
            tsddbScript.Image = Properties.Resources.archivo;
            tsddbScript.ImageTransparentColor = Color.Magenta;
            tsddbScript.Name = "tsddbScript";
            tsddbScript.Size = new Size(67, 34);
            tsddbScript.Text = "xx";
            // 
            // tsbEdit
            // 
            tsbEdit.Image = Properties.Resources.editar;
            tsbEdit.ImageTransparentColor = Color.Magenta;
            tsbEdit.Name = "tsbEdit";
            tsbEdit.Size = new Size(34, 34);
            tsbEdit.Click += tsbEdit_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(1070, 586);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(109, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnRunAll
            // 
            btnRunAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRunAll.Location = new Point(955, 586);
            btnRunAll.Name = "btnRunAll";
            btnRunAll.Size = new Size(109, 29);
            btnRunAll.TabIndex = 4;
            btnRunAll.Text = "btnRunAll";
            btnRunAll.UseVisualStyleBackColor = true;
            btnRunAll.Click += btnRunAll_Click;
            // 
            // ConfirmSqlForm
            // 
            AcceptButton = btnRunAll;
            CancelButton = btnCancel;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1191, 627);
            Controls.Add(btnCancel);
            Controls.Add(btnRunAll);
            Controls.Add(tsc);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ConfirmSqlForm";
            StartPosition = FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).EndInit();
            tsc.TopToolStripPanel.ResumeLayout(false);
            tsc.TopToolStripPanel.PerformLayout();
            tsc.ResumeLayout(false);
            tsc.PerformLayout();
            ts.ResumeLayout(false);
            ts.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripContainer tsc;
        private CustomFctb fctbSql;
        private ToolStrip ts;
        private ToolStripLabel tslSummary;
        private ToolStripDropDownButton tsddbScript;
        private ToolStripButton tsbEdit;
        private Button btnCancel;
        private Button btnRunAll;
    }
}
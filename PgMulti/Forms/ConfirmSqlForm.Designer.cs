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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmSqlForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.fctbSql = new PgMulti.QueryEditor.CustomFctb();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tslSummary = new System.Windows.Forms.ToolStripLabel();
            this.tsddbScript = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbRunAll = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.fctbSql);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1191, 590);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1191, 627);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
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
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSummary,
            this.tsddbScript,
            this.tsbRunAll,
            this.tsbCancel,
            this.tsbEdit});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(205, 37);
            this.toolStrip1.TabIndex = 0;
            // 
            // tslSummary
            // 
            this.tslSummary.Name = "tslSummary";
            this.tslSummary.Size = new System.Drawing.Size(23, 34);
            this.tslSummary.Text = "xx";
            // 
            // tsddbScript
            // 
            this.tsddbScript.Image = global::PgMulti.Properties.Resources.archivo;
            this.tsddbScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbScript.Name = "tsddbScript";
            this.tsddbScript.Size = new System.Drawing.Size(67, 34);
            this.tsddbScript.Text = "xx";
            // 
            // tsbRunAll
            // 
            this.tsbRunAll.Image = global::PgMulti.Properties.Resources.ok;
            this.tsbRunAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunAll.Name = "tsbRunAll";
            this.tsbRunAll.Size = new System.Drawing.Size(34, 34);
            this.tsbRunAll.Click += new System.EventHandler(this.tsbRunAll_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Image = global::PgMulti.Properties.Resources.cerrar;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(34, 34);
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.Image = global::PgMulti.Properties.Resources.editar;
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(34, 34);
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // ConfirmSqlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 627);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfirmSqlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbSql)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripContainer toolStripContainer1;
        private CustomFctb fctbSql;
        private ToolStrip toolStrip1;
        private ToolStripLabel tslSummary;
        private ToolStripDropDownButton tsddbScript;
        private ToolStripButton tsbRunAll;
        private ToolStripButton tsbCancel;
        private ToolStripButton tsbEdit;
    }
}
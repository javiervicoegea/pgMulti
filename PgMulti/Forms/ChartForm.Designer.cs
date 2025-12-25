using FastColoredTextBoxNS;

namespace PgMulti
{
    partial class ChartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartForm));
            toolStripContainer1 = new ToolStripContainer();
            chChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            tsToolbar = new ToolStrip();
            tscbChartType = new ToolStripComboBox();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chChart).BeginInit();
            tsToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(chChart);
            toolStripContainer1.ContentPanel.Size = new Size(815, 458);
            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Location = new Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(815, 486);
            toolStripContainer1.TabIndex = 1;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(tsToolbar);
            // 
            // chChart
            // 
            chChart.Dock = DockStyle.Fill;
            chChart.Location = new Point(0, 0);
            chChart.Name = "chChart";
            chChart.Size = new Size(815, 458);
            chChart.TabIndex = 0;
            // 
            // tsToolbar
            // 
            tsToolbar.Dock = DockStyle.None;
            tsToolbar.ImageScalingSize = new Size(20, 20);
            tsToolbar.Items.AddRange(new ToolStripItem[] { tscbChartType });
            tsToolbar.Location = new Point(4, 0);
            tsToolbar.Name = "tsToolbar";
            tsToolbar.Size = new Size(177, 28);
            tsToolbar.TabIndex = 0;
            // 
            // tscbChartType
            // 
            tscbChartType.DropDownStyle = ComboBoxStyle.DropDownList;
            tscbChartType.Name = "tscbChartType";
            tscbChartType.Size = new Size(121, 28);
            tscbChartType.SelectedIndexChanged += tscbChartType_SelectedIndexChanged;
            // 
            // ChartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(815, 486);
            Controls.Add(toolStripContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ChartForm";
            StartPosition = FormStartPosition.CenterParent;
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chChart).EndInit();
            tsToolbar.ResumeLayout(false);
            tsToolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ToolStripContainer toolStripContainer1;
        private ToolStrip tsToolbar;
        private System.Windows.Forms.DataVisualization.Charting.Chart chChart;
        private ToolStripComboBox tscbChartType;
    }
}
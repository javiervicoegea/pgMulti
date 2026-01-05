namespace PgMulti.Forms
{
    partial class RepositionTablesOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositionTablesOptionsForm));
            lblExplanation = new Label();
            tbRepulsion = new TrackBar();
            chkAutoZoom = new CheckBox();
            tmrRelocator = new System.Windows.Forms.Timer(components);
            lblRepulsion = new Label();
            btnToggleRepulsion = new Button();
            ((System.ComponentModel.ISupportInitialize)tbRepulsion).BeginInit();
            SuspendLayout();
            // 
            // lblExplanation
            // 
            lblExplanation.BackColor = Color.Transparent;
            lblExplanation.Location = new Point(12, 9);
            lblExplanation.Name = "lblExplanation";
            lblExplanation.Size = new Size(604, 65);
            lblExplanation.TabIndex = 0;
            lblExplanation.Text = "lblExplanation";
            // 
            // tbRepulsion
            // 
            tbRepulsion.AutoSize = false;
            tbRepulsion.LargeChange = 1000000;
            tbRepulsion.Location = new Point(196, 122);
            tbRepulsion.Maximum = 8000000;
            tbRepulsion.Minimum = 1000000;
            tbRepulsion.Name = "tbRepulsion";
            tbRepulsion.Size = new Size(420, 32);
            tbRepulsion.SmallChange = 1000000;
            tbRepulsion.TabIndex = 1;
            tbRepulsion.TickFrequency = 1000000;
            tbRepulsion.Value = 1000000;
            tbRepulsion.Scroll += tbRepulsion_Scroll;
            // 
            // chkAutoZoom
            // 
            chkAutoZoom.AutoSize = true;
            chkAutoZoom.BackColor = Color.Transparent;
            chkAutoZoom.Checked = true;
            chkAutoZoom.CheckState = CheckState.Checked;
            chkAutoZoom.Location = new Point(12, 203);
            chkAutoZoom.Name = "chkAutoZoom";
            chkAutoZoom.Size = new Size(125, 24);
            chkAutoZoom.TabIndex = 3;
            chkAutoZoom.Text = "chkAutoZoom";
            chkAutoZoom.UseVisualStyleBackColor = false;
            // 
            // tmrRelocator
            // 
            tmrRelocator.Interval = 50;
            tmrRelocator.Tick += tmrRelocator_Tick;
            // 
            // lblRepulsion
            // 
            lblRepulsion.AutoSize = true;
            lblRepulsion.Location = new Point(36, 128);
            lblRepulsion.Name = "lblRepulsion";
            lblRepulsion.Size = new Size(91, 20);
            lblRepulsion.TabIndex = 8;
            lblRepulsion.Text = "lblRepulsion";
            // 
            // btnToggleRepulsion
            // 
            btnToggleRepulsion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnToggleRepulsion.AutoSize = true;
            btnToggleRepulsion.Location = new Point(463, 276);
            btnToggleRepulsion.Name = "btnToggleRepulsion";
            btnToggleRepulsion.Size = new Size(152, 30);
            btnToggleRepulsion.TabIndex = 10;
            btnToggleRepulsion.Text = "btnToggleRepulsion";
            btnToggleRepulsion.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnToggleRepulsion.UseVisualStyleBackColor = true;
            btnToggleRepulsion.Click += btnToggleRepulsion_Click;
            // 
            // RepositionTablesOptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 318);
            Controls.Add(btnToggleRepulsion);
            Controls.Add(lblRepulsion);
            Controls.Add(chkAutoZoom);
            Controls.Add(tbRepulsion);
            Controls.Add(lblExplanation);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RepositionTablesOptionsForm";
            StartPosition = FormStartPosition.CenterParent;
            FormClosing += ExpandDiagramOptionsForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)tbRepulsion).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplanation;
        private TrackBar tbRepulsion;
        private System.Windows.Forms.Timer tmrRelocator;
        private CheckBox chkAutoZoom;
        private Label lblRepulsion;
        private Button btnToggleRepulsion;
    }
}
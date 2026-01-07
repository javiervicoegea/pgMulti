using System.Windows.Forms;

namespace PgMulti.Forms
{
    partial class DiagramPreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramPreviewForm));
            pnlCanvas = new Panel();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // pnlCanvas
            // 
            pnlCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlCanvas.BackColor = Color.White;
            pnlCanvas.BorderStyle = BorderStyle.Fixed3D;
            pnlCanvas.Location = new Point(19, 19);
            pnlCanvas.Margin = new Padding(10);
            pnlCanvas.Name = "pnlCanvas";
            pnlCanvas.Size = new Size(594, 420);
            pnlCanvas.TabIndex = 0;
            pnlCanvas.Paint += pnlCanvas_Paint;
            pnlCanvas.MouseDown += pnlCanvas_MouseDown;
            pnlCanvas.MouseLeave += pnlCanvas_MouseLeave;
            pnlCanvas.MouseMove += pnlCanvas_MouseMove;
            pnlCanvas.MouseUp += pnlCanvas_MouseUp;
            pnlCanvas.Resize += pnlCanvas_Resize;
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(393, 459);
            btnOk.Margin = new Padding(10);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 29);
            btnOk.TabIndex = 1;
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(513, 459);
            btnCancel.Margin = new Padding(10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 29);
            btnCancel.TabIndex = 2;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // DiagramPreviewForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(632, 507);
            Controls.Add(pnlCanvas);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DiagramPreviewForm";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Panel pnlCanvas;
    }
}

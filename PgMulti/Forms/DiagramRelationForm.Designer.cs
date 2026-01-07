using System.Windows.Forms;

namespace PgMulti.Forms
{
    partial class DiagramRelationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramRelationForm));
            lblRelationId = new Label();
            txtRelationId = new TextBox();
            lblRelationParentTable = new Label();
            cbRelationParentTable = new ComboBox();
            lblRelationParentColumns = new Label();
            odcsRelationParentColumns = new OrderedDiagramColumnSelectorControl();
            lblRelationParentType = new Label();
            cbRelationParentType = new ComboBox();
            lblRelationChildTable = new Label();
            cbRelationChildTable = new ComboBox();
            lblRelationChildColumns = new Label();
            odcsRelationChildColumns = new OrderedDiagramColumnSelectorControl();
            lblRelationChildType = new Label();
            cbRelationChildType = new ComboBox();
            lblRelationOnUpdate = new Label();
            cbRelationOnUpdate = new ComboBox();
            lblRelationOnDelete = new Label();
            cbRelationOnDelete = new ComboBox();
            btnCancel = new Button();
            btnOk = new Button();
            gbParent = new GroupBox();
            gbChild = new GroupBox();
            gbParent.SuspendLayout();
            gbChild.SuspendLayout();
            SuspendLayout();
            // 
            // lblRelationId
            // 
            lblRelationId.AutoSize = true;
            lblRelationId.Location = new Point(17, 17);
            lblRelationId.Margin = new Padding(8);
            lblRelationId.Name = "lblRelationId";
            lblRelationId.Size = new Size(94, 20);
            lblRelationId.TabIndex = 0;
            lblRelationId.Text = "lblRelationId";
            // 
            // txtRelationId
            // 
            txtRelationId.Location = new Point(246, 14);
            txtRelationId.Name = "txtRelationId";
            txtRelationId.Size = new Size(400, 27);
            txtRelationId.TabIndex = 0;
            // 
            // lblRelationParentTable
            // 
            lblRelationParentTable.AutoSize = true;
            lblRelationParentTable.Location = new Point(11, 41);
            lblRelationParentTable.Margin = new Padding(8);
            lblRelationParentTable.Name = "lblRelationParentTable";
            lblRelationParentTable.Size = new Size(157, 20);
            lblRelationParentTable.TabIndex = 0;
            lblRelationParentTable.Text = "lblRelationParentTable";
            // 
            // cbRelationParentTable
            // 
            cbRelationParentTable.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbRelationParentTable.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbRelationParentTable.Location = new Point(229, 38);
            cbRelationParentTable.Name = "cbRelationParentTable";
            cbRelationParentTable.Size = new Size(212, 28);
            cbRelationParentTable.TabIndex = 0;
            cbRelationParentTable.SelectedIndexChanged += cbRelationParentTable_SelectedIndexChanged;
            // 
            // lblRelationParentColumns
            // 
            lblRelationParentColumns.AutoSize = true;
            lblRelationParentColumns.Location = new Point(11, 77);
            lblRelationParentColumns.Margin = new Padding(8);
            lblRelationParentColumns.Name = "lblRelationParentColumns";
            lblRelationParentColumns.Size = new Size(179, 20);
            lblRelationParentColumns.TabIndex = 0;
            lblRelationParentColumns.Text = "lblRelationParentColumns";
            // 
            // odcsRelationParentColumns
            // 
            odcsRelationParentColumns.BackColor = Color.Transparent;
            odcsRelationParentColumns.DiagramTable = null;
            odcsRelationParentColumns.Location = new Point(229, 77);
            odcsRelationParentColumns.Name = "odcsRelationParentColumns";
            odcsRelationParentColumns.OnlyKeyColumns = true;
            odcsRelationParentColumns.Size = new Size(400, 150);
            odcsRelationParentColumns.TabIndex = 1;
            // 
            // lblRelationParentType
            // 
            lblRelationParentType.AutoSize = true;
            lblRelationParentType.Location = new Point(11, 236);
            lblRelationParentType.Margin = new Padding(8);
            lblRelationParentType.Name = "lblRelationParentType";
            lblRelationParentType.Size = new Size(153, 20);
            lblRelationParentType.TabIndex = 0;
            lblRelationParentType.Text = "lblRelationParentType";
            // 
            // cbRelationParentType
            // 
            cbRelationParentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRelationParentType.Location = new Point(229, 233);
            cbRelationParentType.Name = "cbRelationParentType";
            cbRelationParentType.Size = new Size(50, 28);
            cbRelationParentType.TabIndex = 2;
            // 
            // lblRelationChildTable
            // 
            lblRelationChildTable.AutoSize = true;
            lblRelationChildTable.Location = new Point(11, 41);
            lblRelationChildTable.Margin = new Padding(8);
            lblRelationChildTable.Name = "lblRelationChildTable";
            lblRelationChildTable.Size = new Size(213, 28);
            lblRelationChildTable.TabIndex = 0;
            lblRelationChildTable.Text = "lblRelationChildTable";
            // 
            // cbRelationChildTable
            // 
            cbRelationChildTable.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbRelationChildTable.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbRelationChildTable.Location = new Point(229, 38);
            cbRelationChildTable.Name = "cbRelationChildTable";
            cbRelationChildTable.Size = new Size(212, 36);
            cbRelationChildTable.TabIndex = 0;
            cbRelationChildTable.SelectedIndexChanged += cbRelationChildTable_SelectedIndexChanged;
            // 
            // lblRelationChildColumns
            // 
            lblRelationChildColumns.AutoSize = true;
            lblRelationChildColumns.Location = new Point(11, 77);
            lblRelationChildColumns.Margin = new Padding(8);
            lblRelationChildColumns.Name = "lblRelationChildColumns";
            lblRelationChildColumns.Size = new Size(244, 28);
            lblRelationChildColumns.TabIndex = 0;
            lblRelationChildColumns.Text = "lblRelationChildColumns";
            // 
            // odcsRelationChildColumns
            // 
            odcsRelationChildColumns.BackColor = Color.Transparent;
            odcsRelationChildColumns.DiagramTable = null;
            odcsRelationChildColumns.Location = new Point(229, 77);
            odcsRelationChildColumns.Name = "odcsRelationChildColumns";
            odcsRelationChildColumns.OnlyKeyColumns = false;
            odcsRelationChildColumns.Size = new Size(400, 157);
            odcsRelationChildColumns.TabIndex = 1;
            // 
            // lblRelationChildType
            // 
            lblRelationChildType.AutoSize = true;
            lblRelationChildType.Location = new Point(11, 236);
            lblRelationChildType.Margin = new Padding(8);
            lblRelationChildType.Name = "lblRelationChildType";
            lblRelationChildType.Size = new Size(208, 28);
            lblRelationChildType.TabIndex = 0;
            lblRelationChildType.Text = "lblRelationChildType";
            // 
            // cbRelationChildType
            // 
            cbRelationChildType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRelationChildType.Location = new Point(229, 233);
            cbRelationChildType.Name = "cbRelationChildType";
            cbRelationChildType.Size = new Size(50, 36);
            cbRelationChildType.TabIndex = 2;
            // 
            // lblRelationOnUpdate
            // 
            lblRelationOnUpdate.AutoSize = true;
            lblRelationOnUpdate.Location = new Point(17, 663);
            lblRelationOnUpdate.Margin = new Padding(8);
            lblRelationOnUpdate.Name = "lblRelationOnUpdate";
            lblRelationOnUpdate.Size = new Size(82, 20);
            lblRelationOnUpdate.TabIndex = 0;
            lblRelationOnUpdate.Text = "On update:";
            // 
            // cbRelationOnUpdate
            // 
            cbRelationOnUpdate.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRelationOnUpdate.Location = new Point(246, 660);
            cbRelationOnUpdate.Name = "cbRelationOnUpdate";
            cbRelationOnUpdate.Size = new Size(100, 28);
            cbRelationOnUpdate.TabIndex = 3;
            // 
            // lblRelationOnDelete
            // 
            lblRelationOnDelete.AutoSize = true;
            lblRelationOnDelete.Location = new Point(17, 697);
            lblRelationOnDelete.Margin = new Padding(8);
            lblRelationOnDelete.Name = "lblRelationOnDelete";
            lblRelationOnDelete.Size = new Size(77, 20);
            lblRelationOnDelete.TabIndex = 0;
            lblRelationOnDelete.Text = "On delete:";
            // 
            // cbRelationOnDelete
            // 
            cbRelationOnDelete.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRelationOnDelete.Location = new Point(246, 694);
            cbRelationOnDelete.Name = "cbRelationOnDelete";
            cbRelationOnDelete.Size = new Size(100, 28);
            cbRelationOnDelete.TabIndex = 4;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(566, 746);
            btnCancel.Margin = new Padding(10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 29);
            btnCancel.TabIndex = 6;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(446, 746);
            btnOk.Margin = new Padding(10);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 29);
            btnOk.TabIndex = 5;
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // gbParent
            // 
            gbParent.Controls.Add(lblRelationParentColumns);
            gbParent.Controls.Add(odcsRelationParentColumns);
            gbParent.Controls.Add(lblRelationParentTable);
            gbParent.Controls.Add(cbRelationParentTable);
            gbParent.Controls.Add(lblRelationParentType);
            gbParent.Controls.Add(cbRelationParentType);
            gbParent.Location = new Point(17, 56);
            gbParent.Name = "gbParent";
            gbParent.Size = new Size(649, 273);
            gbParent.TabIndex = 1;
            gbParent.TabStop = false;
            gbParent.Text = "gbParent";
            // 
            // gbChild
            // 
            gbChild.Controls.Add(lblRelationChildTable);
            gbChild.Controls.Add(cbRelationChildTable);
            gbChild.Controls.Add(cbRelationChildType);
            gbChild.Controls.Add(lblRelationChildColumns);
            gbChild.Controls.Add(odcsRelationChildColumns);
            gbChild.Controls.Add(lblRelationChildType);
            gbChild.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            gbChild.Location = new Point(17, 348);
            gbChild.Name = "gbChild";
            gbChild.Size = new Size(649, 273);
            gbChild.TabIndex = 2;
            gbChild.TabStop = false;
            gbChild.Text = "gbChild";
            // 
            // DiagramRelationForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(682, 787);
            Controls.Add(gbChild);
            Controls.Add(gbParent);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(lblRelationId);
            Controls.Add(txtRelationId);
            Controls.Add(lblRelationOnUpdate);
            Controls.Add(cbRelationOnUpdate);
            Controls.Add(lblRelationOnDelete);
            Controls.Add(cbRelationOnDelete);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "DiagramRelationForm";
            StartPosition = FormStartPosition.CenterParent;
            gbParent.ResumeLayout(false);
            gbParent.PerformLayout();
            gbChild.ResumeLayout(false);
            gbChild.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnOk;
        private Label lblRelationId;
        private TextBox txtRelationId;
        private Label lblRelationParentTable;
        private ComboBox cbRelationParentTable;
        private Label lblRelationParentColumns;
        private ComboBox cbRelationParentType;
        private Label lblRelationParentType;
        private OrderedDiagramColumnSelectorControl odcsRelationParentColumns;
        private Label lblRelationChildTable;
        private ComboBox cbRelationChildTable;
        private Label lblRelationChildColumns;
        private OrderedDiagramColumnSelectorControl odcsRelationChildColumns;
        private Label lblRelationChildType;
        private ComboBox cbRelationChildType;
        private Label lblRelationOnUpdate;
        private ComboBox cbRelationOnUpdate;
        private Label lblRelationOnDelete;
        private ComboBox cbRelationOnDelete;
        private GroupBox gbParent;
        private GroupBox gbChild;
    }
}

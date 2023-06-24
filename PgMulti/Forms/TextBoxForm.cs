namespace PgMulti
{
    public partial class TextBoxForm : Form
    {
        public bool IsNull;
        public string? Value;

        public TextBoxForm(bool isNull, string? v, bool editable)
        {
            InitializeComponent();
            InitializeText();

            IsNull = isNull;
            Value = v;

            tsbNull.Checked = isNull;
            tsbNull_Click(null, null);

            if (editable)
            {
                txtText.ReadOnly = false;
                tsToolbar.Visible = true;
            }
            else
            {
                txtText.ReadOnly = true;
                tsToolbar.Visible = false;
            }
        }

        private void txtText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void TextBoxForm_Shown(object sender, EventArgs e)
        {
            txtText.Focus();
        }

        private void tsbNull_Click(object? sender, EventArgs? e)
        {
            if (tsbNull.Checked)
            {
                txtText.Text = "[NULL]";
            }
            else
            {
                txtText.Text = Value;
            }

            txtText.Enabled = !tsbNull.Checked;
        }

        private void tsbOk_Click(object sender, EventArgs e)
        {
            if (tsbNull.Checked)
            {
                IsNull = true;
                Value = null;
            }
            else
            {
                IsNull = false;
                Value = txtText.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            IsNull = true;
            Value = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.tsbNull.Text = Properties.Text.null_value;
            this.tsbNull.ToolTipText = Properties.Text.select_to_set_null_value;
            this.tsbOk.Text = Properties.Text.btn_ok;
            this.tsbCancel.Text = Properties.Text.btn_cancel;
            this.Text = Properties.Text.text;
        }
        #endregion
    }
}

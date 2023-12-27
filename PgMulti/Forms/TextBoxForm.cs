using FastColoredTextBoxNS;

namespace PgMulti
{
    public partial class TextBoxForm : Form
    {
        public string? Value;

        public TextBoxForm(string? v, bool editable)
        {
            InitializeComponent();
            InitializeText();

            Value = v;

            tsbNull.Checked = v == null;
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

        private void tsbWordWrap_Click(object? sender, EventArgs? e)
        {
            txtText.WordWrap = tsbWordWrap.Checked;
        }

        private void tsbOpenFile_Click(object? sender, EventArgs? e)
        {
            ofdOpenFile.FileName = "";

            if (ofdOpenFile.ShowDialog(this) != DialogResult.OK) return;

            txtText.Text = File.ReadAllText(ofdOpenFile.FileName);
        }

        private void tsbSaveFile_Click(object? sender, EventArgs? e)
        {
            sfdSaveFile.FileName = "";
            if (sfdSaveFile.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                File.WriteAllText(sfdSaveFile.FileName, txtText.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{sfdSaveFile.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Value = null;
            }
            else
            {
                Value = txtText.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            Value = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.tsbWordWrap.ToolTipText = Properties.Text.text_wrap;
            this.tsbOpenFile.ToolTipText = Properties.Text.open;
            this.tsbSaveFile.ToolTipText = Properties.Text.save_as;
            this.tsbNull.Text = Properties.Text.null_value;
            this.tsbNull.ToolTipText = Properties.Text.select_to_set_null_value;
            this.tsbOk.Text = Properties.Text.btn_ok;
            this.tsbCancel.Text = Properties.Text.btn_cancel;
            this.Text = Properties.Text.text;
            this.ofdOpenFile.Filter = Properties.Text.txt_file_filter;
            this.ofdOpenFile.Title = Properties.Text.select_open_file;
            this.sfdSaveFile.Filter = Properties.Text.txt_file_filter;
            this.sfdSaveFile.Title = Properties.Text.select_save_file;
        }
        #endregion
    }
}

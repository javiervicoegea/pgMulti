using PgMulti.AppData;

namespace PgMulti
{
    public partial class PasswordForm : Form
    {
        private string? _Password = null;

        public PasswordForm()
        {
            InitializeComponent();
            InitializeText();
            DialogResult = DialogResult.Cancel;
        }

        public string? Password
        {
            get
            {
                return _Password;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword1.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword1.Focus();
                return;
            }

            if (txtPassword1.Text != txtPassword2.Text)
            {
                MessageBox.Show(Properties.Text.warning_password_mismatch, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword1.Focus();
                return;
            }

            string? msg;
            if (!Data.ValidateNewPassword(txtPassword1.Text, out msg))
            {
                MessageBox.Show(msg!, Properties.Text.warning_invalid_password, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword1.Focus();
                return;
            }

            _Password = txtPassword1.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.btnOk.Text = Properties.Text.btn_ok;
            this.lblPassword2.Text = $"{Properties.Text.repeat_password}:";
            this.lblPassword1.Text = $"{Properties.Text.new_password}:";
            this.Text = Properties.Text.change_pass;
        }
        #endregion
    }
}

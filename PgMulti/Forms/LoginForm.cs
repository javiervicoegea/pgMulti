using PgMulti.AppData;

namespace PgMulti
{
    public partial class LoginForm : Form
    {
        private string? _Password = null;

        public LoginForm()
        {
            InitializeComponent();
            InitializeText();
        }

        public string? Password
        {
            get
            {
                return _Password;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "v" + Application.ProductVersion;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Text.reset_confirm_message, Properties.Text.reset_confirm_title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            SetupForm f = new SetupForm();
            f.ShowDialog();

            if (f.Password != null)
            {
                _Password = f.Password;
                Close();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Focus();
                return;
            }

            if (!Data.ValidateCurrentPassword(txtPassword.Text))
            {
                MessageBox.Show(Properties.Text.warning_invalid_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Focus();
                txtPassword.SelectAll();
                return;
            }

            _Password = txtPassword.Text;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.lblPassword.Text = $"{Properties.Text.password}:";
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.btnReset.Text = Properties.Text.btn_reset;
            this.Text = Properties.Text.login;
        }
        #endregion
    }
}

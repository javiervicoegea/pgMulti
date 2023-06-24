using PgMulti.AppData;

namespace PgMulti
{
    public partial class SetupForm : Form
    {
        private string? _Password = null;

        public SetupForm()
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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

            try
            {
                Data.CreateDB(txtPassword1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(msg!, string.Format(Properties.Text.error_creating_db,Data.PathDB, ex.Message), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Password = txtPassword1.Text;

            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.btnOk.Text = Properties.Text.btn_ok;
            this.lblPassword1.Text = $"{Properties.Text.password}:";
            this.lblPassword2.Text = $"{Properties.Text.repeat_password}:";
            this.Text = Properties.Text.setup_config;
        }
        #endregion
    }
}

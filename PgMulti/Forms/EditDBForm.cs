using PgMulti.AppData;

namespace PgMulti
{
    public partial class EditDBForm : Form
    {
        private Data _Data;
        private Group _Group;
        private DB? _DB;

        public EditDBForm(Data d, Group g, DB? db)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _Group = g;
            _DB = db;
        }

        public DB? DB
        {
            get { return _DB; }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtDBAlias.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_alias, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDBAlias.Focus();
                return false;
            }

            if (_Data.AllDBs.Any(db => db.Alias.ToLower() == txtDBAlias.Text.ToLower() && !db.Equals(_DB)))
            {
                MessageBox.Show(Properties.Text.warning_duplicate_alias, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDBAlias.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_server, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtServer.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDBName.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_db, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDBName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_port, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPort.Focus();
                return false;
            }

            ushort port;

            if (!ushort.TryParse(txtPort.Text, out port) || port < 1 || port > 65535)
            {
                MessageBox.Show(Properties.Text.warning_invalid_port, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPort.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_user, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUser.Focus();
                return false;
            }

            if (_DB == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Focus();
                return false;
            }

            if (_DB == null)
            {
                _DB = new DB(_Data, _Group);
            }

            _DB.Alias = txtDBAlias.Text;
            _DB.Server = txtServer.Text;
            _DB.DBName = txtDBName.Text;
            _DB.Port = port;
            _DB.User = txtUser.Text;
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                _DB.Password = txtPassword.Text;
            }

            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            string? msg;
            if (_DB!.Test(out msg))
            {
                MessageBox.Show(Properties.Text.test_successful_message, Properties.Text.test_successful_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(msg, Properties.Text.test_error_title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _DB = null;

            Close();
        }

        private void frmEditDB_Load(object sender, EventArgs e)
        {
            if (_DB != null)
            {
                txtDBAlias.Text = _DB.Alias;
                txtServer.Text = _DB.Server;
                txtPort.Text = _DB.Port.ToString();
                txtDBName.Text = _DB.DBName;
                txtUser.Text = _DB.User;
            }
        }

        #region TextI18n
        private void InitializeText()
        {
            this.lblDBAlias.Text = $"{Properties.Text.name}:";
            this.lblServer.Text = $"{Properties.Text.server}:";
            this.label3.Text = $"{Properties.Text.port}:";
            this.lblUser.Text = $"{Properties.Text.user}:";
            this.lblPassword.Text = $"{Properties.Text.password}:";
            this.btnTest.Text = Properties.Text.btn_test;
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.lblDbName.Text = $"{Properties.Text.db}:";
            this.Text = $"{Properties.Text.edit_db}:";
        }
        #endregion
    }
}

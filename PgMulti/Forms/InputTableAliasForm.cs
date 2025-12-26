using PgMulti.Properties;
using System.Diagnostics;

namespace PgMulti
{
    public partial class InputTableAliasForm : Form
    {
        private string _DefaultAlias;
        private Point _Point;

        public InputTableAliasForm(string fqTableName, string defaultAlias, Point p)
        {
            InitializeComponent();
            InitializeText();

            _DefaultAlias = defaultAlias;
            _Point = p;

            Text = string.Format(Properties.Text.input_alias, fqTableName);
            txtInput.Text = defaultAlias;
        }

        public string TableAlias
        {
            get
            {
                if (DialogResult == DialogResult.OK && txtInput.Text != "")
                {
                    return txtInput.Text;
                }
                else
                {
                    return _DefaultAlias;
                }
            }
        }

        private void Accept()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InputTableAliasForm_Load(object sender, EventArgs e)
        {
            Top = _Point.Y;
            Left = _Point.X;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Accept();
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Accept();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Cancel();
            }
        }

        #region TextI18n
        private void InitializeText()
        {
            btnOk.Text = Properties.Text.btn_ok;
        }
        #endregion
    }
}

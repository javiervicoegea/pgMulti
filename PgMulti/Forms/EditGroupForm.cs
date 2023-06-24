using PgMulti.AppData;

namespace PgMulti
{
    public partial class EditGroupForm : Form
    {
        private Data _Data;
        private Group? _ParentGroup;
        private Group? _Group;

        public EditGroupForm(Data d, Group? parentGroup, Group? g)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _ParentGroup = parentGroup;
            _Group = g;
        }

        public Group? Group
        {
            get { return _Group; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_alias, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            if (_Group == null)
            {
                _Group = new Group(_Data, _ParentGroup);
            }

            _Group.Name = txtName.Text;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _Group = null;

            Close();
        }

        private void EditGroupForm_Load(object sender, EventArgs e)
        {
            if (_Group != null)
            {
                txtName.Text = _Group.Name;
            }
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.btnOk.Text = Properties.Text.btn_ok;
            this.lblName.Text = $"{Properties.Text.name}:";
            this.Text = Properties.Text.new_group;
        }
        #endregion
    }
}

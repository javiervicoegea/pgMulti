using PgMulti.Properties;

namespace PgMulti
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeText();
        }

        private void frmAcercaDe_Load(object sender, EventArgs e)
        {
            llUrl.Text = AppSettings.Default.ProjectUrl;
            lblTitle.Text += Application.ProductVersion;
            txtAttributions.SelectionStart = 0;
            txtAttributions.SelectionLength = 0;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.lblAuthor.Text = $"{Properties.Text.author}: Javier Vico Egea";
            this.lblAttributions.Text = $"{Properties.Text.attributions}:";
            this.Text = Properties.Text.about;
        }
        #endregion
    }
}

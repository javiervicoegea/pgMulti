using PgMulti.Properties;
using System.Diagnostics;

namespace PgMulti
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeText();
        }

        private void llUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(AppSettings.Default.ProjectUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Properties.Text.unable_to_open_url, AppSettings.Default.ProjectUrl, ex.Message), Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

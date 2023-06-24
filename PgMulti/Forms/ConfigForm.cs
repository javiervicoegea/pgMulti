using PgMulti.AppData;

namespace PgMulti
{
    public partial class ConfigForm : Form
    {
        private Data _Data;
        public ConfigForm(Data d)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            DialogResult = DialogResult.Cancel;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            chkKeepServerSelection.Checked = _Data.Config.KeepServerSelection;

            if (_Data.Config.AutocompleteDelay == 0)
            {
                chkAutocomplete.Checked = false;
            }
            else
            {
                chkAutocomplete.Checked = true;
            }

            txtAutocompleteDelay.Enabled = chkAutocomplete.Checked;
            txtAutocompleteDelay.Text = _Data.Config.AutocompleteDelay.ToString();

            chkMergeTables.Checked = _Data.Config.MergeTables;

            cbTransactionMode.SelectedIndex = (int)_Data.Config.TransactionMode;
            cbTransactionLevel.SelectedIndex = (int)_Data.Config.TransactionLevel;

            txtMaxRows.Text = _Data.Config.MaxRows.ToString();

            cbLanguage.Items.AddRange(AppLanguage.AvailableLanguages.ToArray());
            cbLanguage.SelectedItem = AppLanguage.CurrentLanguage;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int delay;
            if (chkAutocomplete.Checked)
            {
                if (!int.TryParse(txtAutocompleteDelay.Text, out delay) || delay < 0)
                {
                    MessageBox.Show(Properties.Text.warning_invalid_autocomplete_delay, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (delay == 0) delay = 1;
            }
            else
            {
                delay = 0;
            }

            int maxRows;
            if (!int.TryParse(txtMaxRows.Text, out maxRows) || maxRows <= 0)
            {
                MessageBox.Show(Properties.Text.warning_invalid_max_rows, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _Data.Config.KeepServerSelection = chkKeepServerSelection.Checked;
            _Data.Config.AutocompleteDelay = delay;
            _Data.Config.MergeTables = chkMergeTables.Checked;
            _Data.Config.TransactionMode = (Config.TransactionModeEnum)cbTransactionMode.SelectedIndex;
            _Data.Config.TransactionLevel = (Config.TransactionLevelEnum)cbTransactionLevel.SelectedIndex;
            _Data.Config.MaxRows = maxRows;

            _Data.Config.Save();
            _Data.ResetConfigCache();

            AppLanguage.CurrentLanguage = (AppLanguage)cbLanguage.SelectedItem;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkAutocomplete_CheckedChanged(object sender, EventArgs e)
        {
            txtAutocompleteDelay.Enabled = chkAutocomplete.Checked;
        }

        private void cbTransactionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTransactionLevel.Enabled = cbTransactionMode.SelectedIndex > 0;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.chkKeepServerSelection.Text = Properties.Text.keep_db_selection;
            this.chkAutocomplete.Text = Properties.Text.enable_autocomplete;
            this.lblAutocompleteDelay.Text = $"{Properties.Text.delay} (ms):";
            this.chkMergeTables.Text = Properties.Text.mergeTables;
            this.lblManualTransactionModeTitle.Text = Properties.Text.manual_transactions;
            this.cbTransactionMode.Items.AddRange(new object[] {
            Properties.Text.manual_transactions,
            Properties.Text.auto_single_transactions,
            Properties.Text.auto_coordinated_transactions});
            this.tabPage1.Text = Properties.Text.general;
            this.lblMaxRowsTitle.Text = Properties.Text.max_rows;
            this.lblMaxRows.Text = $"{Properties.Text.limit}:";
            this.tabPage2.Text = Properties.Text.transactions;
            this.lblTransactionLevel.Text = $"{Properties.Text.level}:";
            this.lblTransactionMode.Text = $"{Properties.Text.mode}:";
            this.lblAutoCoordinatedTransactionModeTitle.Text = Properties.Text.auto_coordinated_transactions;
            this.lblAutoSingleTransactionModeTitle.Text = Properties.Text.auto_single_transactions;
            this.lblLanguage.Text = Properties.Text.language;
            this.Text = Properties.Text.app_options;

            this.lblKeepServerSelection.Text = Properties.Text.keep_db_selection_explanation;
            this.lblAutocomplete.Text = Properties.Text.autocomplete_explanation;
            this.lblMergeTables.Text = Properties.Text.merge_tables_explanation;
            this.lblMaxRowsInfo.Text = Properties.Text.max_rows_explanation;
            this.lblAutoCoordinatedTransactionModeInfo.Text = Properties.Text.auto_coordinated_transaction_mode_explanation;
            this.lblAutoSingleTransactionModeInfo.Text = Properties.Text.auto_single_transaction_mode_explanation;
            this.lblManualTransactionModeInfo.Text = Properties.Text.manual_transaction_mode_explanation;
        }
        #endregion
    }
}

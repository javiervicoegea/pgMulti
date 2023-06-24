using PgMulti.AppData;

namespace PgMulti
{
    public partial class ConfirmSqlForm : Form
    {
        public ResultEnum Result = ResultEnum.Cancel;

        private Data _Data;
        private List<Tuple<DB, string>> _Statements;

        public ConfirmSqlForm(Data d, List<Tuple<DB, string>> statements)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _Statements = statements;
            fctbSql.SetParser(_Data.PGLanguageData);

            tslSummary.Text = $"{string.Format(Properties.Text.script_count_message, _Statements.Count)}:";
            for (int i = 0; i < _Statements.Count; i++)
            {
                Tuple<DB, string> t = _Statements[i];
                ToolStripMenuItem tsmiStatement = new ToolStripMenuItem();

                tsmiStatement.Image = Properties.Resources.error;
                tsmiStatement.Text = string.Format(Properties.Text.script_index_message, i + 1, _Statements.Count, t.Item1.Alias);
                tsmiStatement.Click += new EventHandler(tsmiStatement_Click);
                tsmiStatement.Tag = t;
                tsddbScript.DropDownItems.Add(tsmiStatement);
            }

            tsmiStatement_Click(tsddbScript.DropDownItems[0], null);
        }
        private void tsmiStatement_Click(object? sender, EventArgs? e)
        {
            ToolStripMenuItem tsmiComando = (ToolStripMenuItem)sender!;
            Tuple<DB, string> t = (Tuple<DB, string>)tsmiComando.Tag;
            fctbSql.Text = t.Item2;
            tsddbScript.Text = tsmiComando.Text;
        }

        private void tsbRunAll_Click(object sender, EventArgs e)
        {
            Result = ResultEnum.Run;
            Close();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            Result = ResultEnum.Cancel;
            Close();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            Result = ResultEnum.Edit;
            Close();
        }

        public enum ResultEnum
        {
            Run,
            Cancel,
            Edit
        }

        #region TextI18n
        private void InitializeText()
        {
            this.tsbRunAll.Text = Properties.Text.run_all;
            this.tsbRunAll.ToolTipText = Properties.Text.run_all_extended;
            this.tsbCancel.Text = Properties.Text.btn_cancel;
            this.tsbEdit.Text = Properties.Text.edit;
            this.Text = Properties.Text.confirm_execution;
        }
        #endregion
    }
}

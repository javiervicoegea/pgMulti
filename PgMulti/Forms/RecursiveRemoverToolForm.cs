using PgMulti.AppData;
using PgMulti.DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PgMulti.Forms
{
    public partial class RecursiveRemoverToolForm : Form
    {
        public PgMulti.RecursiveRemover.RecursiveRemover? RecursiveRemover;
        private Table _Table;

        public RecursiveRemoverToolForm(Table t)
        {
            InitializeComponent();
            InitializeText();

            _Table = t;
            txtRootTableName.Text = SqlSyntax.PostgreSqlGrammar.IdToString(t.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(t.Id);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            RecursiveRemover = new PgMulti.RecursiveRemover.RecursiveRemover(txtSchemaName.Text, _Table, txtDeleteTuplesWhereClause.Text, txtPreserveTuplesWhereClause.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RecursiveRemover = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtDeleteTuplesWhereClause.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_delete_filter, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDeleteTuplesWhereClause.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPreserveTuplesWhereClause.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_preserve_filter, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPreserveTuplesWhereClause.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSchemaName.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_schema, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSchemaName.Focus();
                return false;
            }

            return true;
        }

        private void InitializeText()
        {
            this.lblRootTableName.Text = Properties.Text.table_name;
            this.lblDeleteTuplesWhereClause.Text = Properties.Text.delete_tuples_where_clause;
            this.lblPreserveTuplesWhereClause.Text = Properties.Text.preserve_tuples_where_clause;
            this.lblSchemaName.Text = Properties.Text.recursive_remover_schema;

            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
        }
    }
}

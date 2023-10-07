using PgMulti.Diagrams;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace PgMulti.Forms
{
    public partial class TableForm : Form
    {
        private DataTable _Columns;
        private DiagramTable _DiagramTable;
        private Regex _RegexTypeNameParams = new Regex(@"^(.+)\s*(\(\s*(\d+)\s*(\,\s*(\d+)\s*)?\))$");
        private Regex _RegexTypeParamValues = new Regex(@"[\(\,]\s*(n)\s*[\,\)]");

        public DiagramTable DiagramTable
        {
            get
            {
                return _DiagramTable;
            }
        }

        public TableForm(Diagram d)
        {
            InitializeComponent();
            InitializeText();

            this.Text = Properties.Text.new_table;
            _DiagramTable = new DiagramTable(d, "public", "", new List<DiagramColumn>(), new List<DiagramTableRelation>());
            _Columns = ShowDiagramTable();
        }

        public TableForm(Diagram d, DiagramTable dt)
        {
            InitializeComponent();
            InitializeText();

            this.Text = string.Format(Properties.Text.edit_table_x, dt.SchemaName + "." + dt.TableName);
            _DiagramTable = dt;
            _Columns = ShowDiagramTable();
        }

        private DataTable ShowDiagramTable()
        {
            txtTableName.Text = _DiagramTable.TableName;
            txtSchemaName.Text = _DiagramTable.SchemaName;

            DataTable columns = new DataTable();
            columns.Columns.Add("original_name", typeof(string));
            columns.Columns.Add("name", typeof(string));
            columns.Columns.Add("type_name", typeof(string));
            columns.Columns.Add("type_initials", typeof(string));
            columns.Columns.Add("pk", typeof(bool));
            columns.Columns.Add("is_identity", typeof(bool));
            columns.Columns.Add("default", typeof(string));
            columns.Columns.Add("not_null", typeof(bool));

            foreach (DiagramColumn dc in _DiagramTable.Columns)
            {
                DataRow dr = columns.NewRow();

                dr["original_name"] = dc.ColumnName;
                dr["name"] = dc.ColumnName;
                dr["type_name"] = dc.TypeName + (dc.TypeParams == null ? "" : " " + dc.TypeParams);
                dr["type_initials"] = dc.TypeInitials;
                dr["pk"] = dc.PrimaryKey;
                dr["is_identity"] = dc.IsIdentity;
                dr["not_null"] = dc.NotNull;

                columns.Rows.Add(dr);
            }

            gvColumns.DataSource = columns;

            return columns;
        }

        private void gvColumns_SelectionChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count == 1)
            {
                DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];

                txtColumnName.Enabled = true;
                cbColumnType.Enabled = true;
                txtColumnTypeInitials.Enabled = true;
                chkPrimaryKey.Enabled = true;
                chkIdentity.Enabled = true;
                txtColumnDefault.Enabled = true;
                chkNotNull.Enabled = true;

                txtColumnName.Text = (string)dr["name"];
                cbColumnType.Text = (string)dr["type_name"];
                txtColumnTypeInitials.Text = (string)dr["type_initials"];
                chkPrimaryKey.Checked = (bool)dr["pk"];
                chkIdentity.Checked = (bool)dr["is_identity"];
                txtColumnDefault.Text = (dr["default"] == DBNull.Value ? "" : (string)dr["default"]);
                chkNotNull.Checked = (bool)dr["not_null"];
            }
            else
            {
                txtColumnName.Enabled = false;
                cbColumnType.Enabled = false;
                txtColumnTypeInitials.Enabled = false;
                chkPrimaryKey.Enabled = false;
                chkIdentity.Enabled = false;
                txtColumnDefault.Enabled = false;
                chkNotNull.Enabled = false;

                txtColumnName.Text = "";
                cbColumnType.Text = "";
                txtColumnTypeInitials.Text = "";
                chkPrimaryKey.Checked = false;
                chkIdentity.Checked = false;
                txtColumnDefault.Text = "";
                chkNotNull.Checked = false;
            }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            DataRow dr = _Columns.NewRow();

            dr["original_name"] = DBNull.Value;
            dr["name"] = "";
            dr["type_name"] = "";
            dr["type_initials"] = "";
            dr["pk"] = false;
            dr["is_identity"] = false;
            dr["not_null"] = false;

            _Columns.Rows.Add(dr);
            gvColumns.Rows[gvColumns.Rows.Count - 1].Selected = true;
            txtColumnName.Focus();
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count == 1)
            {
                _Columns.Rows.RemoveAt(gvColumns.SelectedRows[0].Index);
            }
        }

        private void txtColumnName_TextChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["name"] = txtColumnName.Text;
        }

        private void cbColumnType_TextChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            
            dr["type_name"] = cbColumnType.Text;
        }

        string? _OriginalCbColumnTypeValue = null;

        private void cbColumnType_Enter(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            _OriginalCbColumnTypeValue = cbColumnType.Text;
        }

        private void cbColumnType_Leave(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            if (_OriginalCbColumnTypeValue != cbColumnType.Text)
            {
                string typeName = cbColumnType.Text;
                string? typeParams;

                ParseTypeName(ref typeName, out typeParams);

                cbColumnType.Text = typeName + typeParams;

                string? initials = DiagramColumn.GetTypeInitials(typeName, typeParams);
                txtColumnTypeInitials.Text = initials;
            }
        }

        private void cbColumnType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            Match m = _RegexTypeParamValues.Match(cbColumnType.Text);
            if (m.Success)
            {
                e.Cancel = true;
                cbColumnType.Select(m.Groups[1].Index, m.Groups[1].Length);
            }
        }

        private void txtColumnTypeInitials_TextChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["type_initials"] = txtColumnTypeInitials.Text;
        }

        private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["pk"] = chkPrimaryKey.Checked;
        }

        private void chkIdentity_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["is_identity"] = chkIdentity.Checked;
        }

        private void txtColumnDefault_TextChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["default"] = (string.IsNullOrWhiteSpace(txtColumnDefault.Text) ? DBNull.Value : txtColumnDefault.Text);
        }

        private void chkNotNull_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["not_null"] = chkNotNull.Checked;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_table_name, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tc.SelectedTab = tcGeneral;
                txtTableName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSchemaName.Text))
            {
                MessageBox.Show(Properties.Text.warning_empty_schema, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tc.SelectedTab = tcGeneral;
                txtTableName.Focus();
                return false;
            }
            for (int i = 0; i < _Columns.Rows.Count; i++)
            {
                DataRow dr = _Columns.Rows[i];

                if (string.IsNullOrWhiteSpace((string)dr["name"]))
                {
                    MessageBox.Show(Properties.Text.warning_empty_column_name, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    txtColumnName.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace((string)dr["type_name"]))
                {
                    MessageBox.Show(Properties.Text.warning_empty_type_name, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    cbColumnType.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace((string)dr["type_initials"]))
                {
                    MessageBox.Show(Properties.Text.warning_empty_type_initials, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    txtColumnTypeInitials.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            // Check if there is another table with the same name
            foreach (DiagramTable dti in _DiagramTable.Diagram.Tables)
            {
                if (dti != _DiagramTable && dti.TableName.ToUpper().Trim() == _DiagramTable.TableName.ToUpper().Trim() && dti.SchemaName.ToUpper().Trim() == _DiagramTable.SchemaName.ToUpper().Trim())
                {
                    MessageBox.Show(Properties.Text.warning_table_already_exists, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcGeneral;
                    txtTableName.Focus();
                    return;
                }
            }

            // Check if there are columns and relations to remove

            List<DiagramColumn> columnsToRemove = new List<DiagramColumn>();
            foreach (DiagramColumn dci in _DiagramTable.Columns)
            {
                foreach (DataRow dri in _Columns.Rows)
                {
                    if (dci.ColumnName == (string)dri["original_name"])
                    {
                        columnsToRemove.Add(dci);
                        break;
                    }
                }
            }

            List<DiagramTableRelation> relationsToRemove = new List<DiagramTableRelation>();
            foreach (DiagramTableRelation dtri in _DiagramTable.Relations)
            {
                bool remove = false;
                foreach (DiagramColumn dci in columnsToRemove)
                {
                    if (dtri.ParentTable == _DiagramTable)
                    {
                        foreach (DiagramColumn dtrci in dtri.ParentTableColumns)
                        {
                            if (dci == dtrci)
                            {
                                remove = true;
                                break;
                            }
                        }
                    }
                    if (!remove && dtri.ChildTable == _DiagramTable)
                    {
                        foreach (DiagramColumn dtrci in dtri.ChildTableColumns)
                        {
                            if (dci == dtrci)
                            {
                                remove = true;
                                break;
                            }
                        }
                    }
                    if (remove) break;
                }

                if (remove)
                {
                    relationsToRemove.Add(dtri);
                }
            }

            // Confirm relation deletion
            if (relationsToRemove.Count > 0)
            {
                if (MessageBox.Show(
                    string.Format(Properties.Text.warning_relations_to_remove, string.Join(", ", relationsToRemove)),
                    Properties.Text.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
                {
                    return;
                }
            }

            foreach (DiagramColumn dci in columnsToRemove) _DiagramTable.Columns.Remove(dci);
            foreach (DiagramTableRelation dtri in relationsToRemove) _DiagramTable.Relations.Remove(dtri);

            List<DiagramColumn> columns = new List<DiagramColumn>();

            foreach (DataRow dr in _Columns.Rows)
            {
                string typeName = (string)dr["type_name"];
                string? typeParams;

                ParseTypeName(ref typeName, out typeParams);

                string? defaultValue = (dr["default"] == DBNull.Value ? null : (string)dr["default"]);

                DiagramColumn dc;

                if (dr["original_name"] == DBNull.Value)
                {
                    dc = new DiagramColumn((string)dr["name"], typeName, typeParams, defaultValue, (bool)dr["is_identity"], (bool)dr["pk"], (bool)dr["not_null"], (string)dr["type_initials"]);
                    columns.Add(dc);
                }
                else
                {
                    dc = _DiagramTable.Columns.First(dci => dci.ColumnName == (string)dr["original_name"]);
                    dc.ColumnName = (string)dr["name"];
                    dc.TypeName = typeName;
                    dc.TypeParams = typeParams;
                    dc.DefaultValue = defaultValue;
                    dc.IsIdentity = (bool)dr["is_identity"];
                    dc.PrimaryKey = (bool)dr["pk"];
                    dc.NotNull = (bool)dr["not_null"];
                    dc.TypeInitials = (string)dr["type_initials"];
                }
            }

            _DiagramTable.TableName = txtTableName.Text;
            _DiagramTable.SchemaName = txtSchemaName.Text;
            _DiagramTable.Columns = columns;
            _DiagramTable.RefreshDimensions();

            DialogResult = DialogResult.OK;
        }

        private void ParseTypeName(ref string typeName, out string? typeParams)
        {
            Match m = _RegexTypeNameParams.Match(typeName);

            if (m.Success)
            {
                typeName = m.Groups[1].Value;
                typeParams = "(" + m.Groups[3].Value + (m.Groups[5].Value == "" ? "" : "," + m.Groups[5].Value) + ")";
            }
            else
            {
                typeParams = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.tc.TabPages[0].Text = Properties.Text.general;
            this.tc.TabPages[1].Text = Properties.Text.columns;
            this.lblTableName.Text = Properties.Text.table_name + ":";
            this.lblSchemaName.Text = Properties.Text.schema_name + ":";
            this.tsbAdd.Text = Properties.Text.new_column;
            this.tsbRemove.Text = Properties.Text.remove_column;
            this.gvColumns.Columns[0].HeaderText = Properties.Text.name;
            this.gvColumns.Columns[1].HeaderText = Properties.Text.type_name;
            this.gvColumns.Columns[2].HeaderText = Properties.Text.pk;
            this.gvColumns.Columns[3].HeaderText = Properties.Text.not_null;
            this.lblColumnName.Text = Properties.Text.column_name + ":";
            this.lblColumnType.Text = Properties.Text.type_name + ":";
            this.lblColumnTypeInitials.Text = Properties.Text.type_initials + ":";
            this.lblPrimaryKey.Text = Properties.Text.pk + ":";
            this.lblIdentity.Text = Properties.Text.identity + ":";
            this.lblColumnDefault.Text = Properties.Text.column_default + ":";
            this.lblNotNull.Text = Properties.Text.not_null + ":";
        }
        #endregion

        private bool IsActive = true;
        private void gvColumns_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (IsActive)
            {
                Match m = _RegexTypeParamValues.Match(cbColumnType.Text);
                if (m.Success)
                {
                    IsActive = false;
                    cbColumnType.Focus();
                    cbColumnType.Select(m.Groups[1].Index, m.Groups[1].Length);
                    IsActive = true;

                    e.Cancel = true;
                }
            }
        }

        private void gvColumns_Enter(object sender, EventArgs e)
        {
            //IsActive = true;
        }

        private void gvColumns_Leave(object sender, EventArgs e)
        {
            //IsActive = false;
        }
    }
}

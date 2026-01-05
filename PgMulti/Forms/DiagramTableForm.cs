using PgMulti.Diagrams;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PgMulti.Forms
{
    public partial class DiagramTableForm : Form
    {
        private DataTable _Columns;
        private DiagramTable _DiagramTable;
        private Regex _RegexTypeNameParams = new Regex(@"^(.+)\s*(\(\s*(\d+)\s*(\,\s*(\d+)\s*)?\))$");
        private Regex _RegexTypeParamValues = new Regex(@"[\(\,]\s*(n)\s*[\,\)]");

        public DiagramTableForm(Diagram d) : this(new DiagramTable(d, "public", "", new List<DiagramColumn>(), new List<DiagramRelation>()))
        {
            InitializeComponent();
            InitializeText();

            Text = Properties.Text.new_table;
        }

        public DiagramTableForm(DiagramTable dt)
        {
            InitializeComponent();
            InitializeText();

            Text = string.Format(Properties.Text.edit_table_x, dt.SchemaName + "." + dt.TableName);
            _DiagramTable = dt;

            // General tab
            txtTableName.Text = _DiagramTable.TableName;
            txtSchemaName.Text = _DiagramTable.SchemaName;

            // Columns tab
            _Columns = new DataTable();
            _Columns.Columns.Add("original_name", typeof(string));
            _Columns.Columns.Add("name", typeof(string));
            _Columns.Columns.Add("type_name", typeof(string));
            _Columns.Columns.Add("type_initials", typeof(string));
            _Columns.Columns.Add("pk", typeof(bool));
            _Columns.Columns.Add("is_identity", typeof(bool));
            _Columns.Columns.Add("default", typeof(string));
            _Columns.Columns.Add("not_null", typeof(bool));

            foreach (DiagramColumn dc in _DiagramTable.Columns)
            {
                DataRow dr = _Columns.NewRow();

                dr["original_name"] = dc.ColumnName;
                dr["name"] = dc.ColumnName;
                dr["type_name"] = dc.TypeName + (dc.TypeParams == null ? "" : " " + dc.TypeParams);
                dr["type_initials"] = dc.TypeInitials;
                dr["pk"] = dc.PrimaryKey;
                dr["is_identity"] = dc.IsIdentity;
                dr["not_null"] = dc.NotNull;

                _Columns.Rows.Add(dr);
            }

            gvColumns.DataSource = _Columns;
        }

        public DiagramTable DiagramTable
        {
            get
            {
                return _DiagramTable;
            }
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

        #region "Column events"

        private bool gvColumns_RowValidating_Ignore = false;
        private void gvColumns_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (gvColumns_RowValidating_Ignore) return;
            Match m = _RegexTypeParamValues.Match(cbColumnType.Text);
            if (m.Success)
            {
                gvColumns_RowValidating_Ignore = true;
                cbColumnType.Focus();
                cbColumnType.Select(m.Groups[1].Index, m.Groups[1].Length);
                gvColumns_RowValidating_Ignore = false;

                e.Cancel = true;
            }
        }

        private void gvColumns_SelectionChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count == 1)
            {
                DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];

                txtColumnName.Enabled = true;
                cbColumnType.Enabled = true;
                txtColumnTypeInitials.Enabled = true;
                chkColumnPrimaryKey.Enabled = true;
                chkColumnIdentity.Enabled = true;
                txtColumnDefault.Enabled = true;
                chkColumnNotNull.Enabled = true;

                txtColumnName.Text = dr.Field<string?>("name");
                cbColumnType.Text = dr.Field<string?>("type_name");
                txtColumnTypeInitials.Text = dr.Field<string?>("type_initials");
                chkColumnPrimaryKey.Checked = (bool)dr["pk"];
                chkColumnIdentity.Checked = (bool)dr["is_identity"];
                txtColumnDefault.Text = (dr["default"] == DBNull.Value ? "" : dr.Field<string?>("default"));
                chkColumnNotNull.Checked = (bool)dr["not_null"];
            }
            else
            {
                txtColumnName.Enabled = false;
                cbColumnType.Enabled = false;
                txtColumnTypeInitials.Enabled = false;
                chkColumnPrimaryKey.Enabled = false;
                chkColumnIdentity.Enabled = false;
                txtColumnDefault.Enabled = false;
                chkColumnNotNull.Enabled = false;

                txtColumnName.Text = "";
                cbColumnType.Text = "";
                txtColumnTypeInitials.Text = "";
                chkColumnPrimaryKey.Checked = false;
                chkColumnIdentity.Checked = false;
                txtColumnDefault.Text = "";
                chkColumnNotNull.Checked = false;
            }
        }

        private void tsbColumnAdd_Click(object sender, EventArgs e)
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

        private void tsbColumnRemove_Click(object sender, EventArgs e)
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

        private void chkColumnPrimaryKey_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["pk"] = chkColumnPrimaryKey.Checked;
        }

        private void chkColumnIdentity_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["is_identity"] = chkColumnIdentity.Checked;
        }

        private void txtColumnDefault_TextChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["default"] = (string.IsNullOrWhiteSpace(txtColumnDefault.Text) ? DBNull.Value : txtColumnDefault.Text);
        }

        private void chkColumnNotNull_CheckedChanged(object sender, EventArgs e)
        {
            if (gvColumns.SelectedRows.Count != 1) return;
            DataRow dr = _Columns.Rows[gvColumns.SelectedRows[0].Index];
            dr["not_null"] = chkColumnNotNull.Checked;
        }
        #endregion

        //#region "Relations events"

        //private DiagramRelation.RelationTypeOptions? DataTableValueToRelationTypeOptions(string? v)
        //{
        //    if (v == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return Enum.Parse<DiagramRelation.RelationTypeOptions>(v);
        //    }
        //}

        //private void gvRelations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    DataRow dr = _Relations.Rows[e.RowIndex];

        //    switch (gvRelations.Columns[e.ColumnIndex].Name)
        //    {
        //        case "gvcRelationTable":
        //            DiagramTable? dt;

        //            if (dr.Field<string?>("parent_schema_name") == _DiagramTable.SchemaName && dr.Field<string?>("parent_table_name") == _DiagramTable.TableName)
        //            {
        //                dt = _DiagramTable.Diagram.Tables.FirstOrDefault(i => i.SchemaName == dr.Field<string?>("child_schema_name") && i.TableName == dr.Field<string?>("child_table_name"));
        //            }
        //            else if (dr.Field<string?>("child_schema_name") == _DiagramTable.SchemaName && dr.Field<string?>("child_table_name") == _DiagramTable.TableName)
        //            {
        //                dt = _DiagramTable.Diagram.Tables.FirstOrDefault(i => i.SchemaName == dr.Field<string?>("parent_schema_name") && i.TableName == dr.Field<string?>("parent_table_name"));
        //            }
        //            else
        //            {
        //                dt = null;
        //            }

        //            if (dt == null)
        //            {
        //                e.Value = "";
        //            }
        //            else
        //            {
        //                e.Value = $"{dt.SchemaName}.{dt.TableName}";
        //            }
        //            e.FormattingApplied = true;
        //            break;
        //        case "gvcRelationType":
        //            DiagramRelation.RelationTypeOptions? parentType = DataTableValueToRelationTypeOptions(dr.Field<string?>("parent_type"));
        //            DiagramRelation.RelationTypeOptions? childType = DataTableValueToRelationTypeOptions(dr.Field<string?>("child_type"));

        //            if (dr.Field<string?>("parent_schema_name") == _DiagramTable.SchemaName && dr.Field<string?>("parent_table_name") == _DiagramTable.TableName)
        //            {
        //                e.Value = (parentType.HasValue ? DiagramRelation.RelationTypeOptionsToString(parentType.Value) : "?") + " - " + (childType.HasValue ? DiagramRelation.RelationTypeOptionsToString(childType.Value) : "?");
        //            }
        //            else if (dr.Field<string?>("child_schema_name") == _DiagramTable.SchemaName && dr.Field<string?>("child_table_name") == _DiagramTable.TableName)
        //            {
        //                e.Value = (childType.HasValue ? DiagramRelation.RelationTypeOptionsToString(childType.Value) : "?") + " - " + (parentType.HasValue ? DiagramRelation.RelationTypeOptionsToString(parentType.Value) : "?");
        //            }
        //            else
        //            {
        //                e.Value = "";
        //            }
        //            e.FormattingApplied = true;
        //            break;
        //    }
        //}

        //private bool _IgnoreChangeEvents = false;
        //private void gvRelations_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (gvRelations.SelectedRows.Count == 1)
        //    {
        //        DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //        pnlRelation.Visible = true;

        //        _IgnoreChangeEvents = true;
        //        txtRelationId.Text = dr.Field<string?>("id");
        //        DiagramTable? parentTable = _DiagramTable.Diagram.Tables.FirstOrDefault(i => i.SchemaName == dr.Field<string?>("parent_schema_name") && i.TableName == dr.Field<string?>("parent_table_name"));
        //        cbRelationParentTable.SelectedItem = parentTable;
        //        odcsRelationParentColumns.DiagramTable = parentTable;
        //        odcsRelationParentColumns.SelectedColumnsStringValue = dr.Field<string?>("parent_columns");
        //        cbRelationParentType.SelectedItem = cbRelationParentType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value.ToString() == dr.Field<string>("parent_type"));
        //        DiagramTable? childTable = _DiagramTable.Diagram.Tables.FirstOrDefault(i => i.SchemaName == dr.Field<string?>("child_schema_name") && i.TableName == dr.Field<string?>("child_table_name"));
        //        cbRelationChildTable.SelectedItem = childTable;
        //        odcsRelationChildColumns.DiagramTable = childTable;
        //        odcsRelationChildColumns.SelectedColumnsStringValue = dr.Field<string?>("child_columns");
        //        cbRelationChildType.SelectedItem = cbRelationChildType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value.ToString() == dr.Field<string>("child_type"));
        //        cbRelationOnUpdate.SelectedItem = cbRelationOnUpdate.Items.Cast<PropagationOptionListItem>().First(i => i.Value.ToString() == dr.Field<string>("on_update"));
        //        cbRelationOnDelete.SelectedItem = cbRelationOnDelete.Items.Cast<PropagationOptionListItem>().First(i => i.Value.ToString() == dr.Field<string>("on_delete"));
        //        _IgnoreChangeEvents = false;

        //        cbRelationParentTable.Enabled = true;
        //        cbRelationChildTable.Enabled = true;
        //    }
        //    else
        //    {
        //        pnlRelation.Visible = false;
        //    }
        //}

        //private void tsbRelationAdd_Click(object sender, EventArgs e)
        //{
        //    DataRow dr = _Relations.NewRow();

        //    dr["id"] = "";
        //    dr["parent_schema_name"] = _DiagramTable.SchemaName;
        //    dr["parent_table_name"] = _DiagramTable.TableName;
        //    dr["parent_columns"] = string.Join(',', _DiagramTable.Columns.Where(i => i.PrimaryKey).Select(i => i.ColumnName));
        //    dr["parent_type"] = DiagramRelation.RelationTypeOptions.One.ToString();
        //    dr["child_schema_name"] = DBNull.Value;
        //    dr["child_table_name"] = DBNull.Value;
        //    dr["child_columns"] = DBNull.Value;
        //    dr["child_type"] = DiagramRelation.RelationTypeOptions.Many.ToString();
        //    dr["on_update"] = DiagramRelation.PropagationOptions.Restrict.ToString();
        //    dr["on_delete"] = DiagramRelation.PropagationOptions.Restrict.ToString();

        //    _Relations.Rows.Add(dr);
        //    gvRelations.Rows[gvRelations.Rows.Count - 1].Selected = true;
        //    txtRelationId.Focus();

        //    cbRelationParentTable.Enabled = true;
        //    cbRelationChildTable.Enabled = true;
        //}

        //private void tsbRelationRemove_Click(object sender, EventArgs e)
        //{
        //    if (gvRelations.SelectedRows.Count == 1)
        //    {
        //        _Relations.Rows.RemoveAt(gvRelations.SelectedRows[0].Index);
        //    }
        //}

        //private void txtRelationId_TextChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];
        //    dr["id"] = txtRelationId.Text;
        //}

        //private void cbRelationParentTable_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    DiagramTable? dt = (DiagramTable?)cbRelationParentTable.SelectedItem;
        //    if (dt == null)
        //    {
        //        dr["parent_schema_name"] = DBNull.Value;
        //        dr["parent_table_name"] = DBNull.Value;

        //    }
        //    else
        //    {
        //        dr["parent_schema_name"] = dt.SchemaName;
        //        dr["parent_table_name"] = dt.TableName;
        //    }
        //    gvRelations.Invalidate();

        //    odcsRelationParentColumns.DiagramTable = dt;
        //    odcsRelationParentColumns.SelectedColumnsStringValue = (dt == null ? "" : string.Join(',', dt.Columns.Where(i => i.PrimaryKey).Select(i => i.ColumnName)));

        //    if (dt != _DiagramTable)
        //    {
        //        cbRelationChildTable.SelectedItem = _DiagramTable;
        //        odcsRelationChildColumns.DiagramTable = _DiagramTable;
        //    }
        //    cbRelationChildTable.Enabled = dt == _DiagramTable;

        //}

        //private void odcsRelationParentColumns_SelectedColumnsChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    dr["parent_columns"] = odcsRelationParentColumns.SelectedColumnsStringValue;
        //}

        //private void cbRelationParentType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    RelationTypeOptionListItem? o = (RelationTypeOptionListItem?)cbRelationParentType.SelectedItem;
        //    if (o == null)
        //    {
        //        dr["parent_type"] = DBNull.Value;
        //    }
        //    else
        //    {
        //        dr["parent_type"] = o.Value.ToString();
        //    }
        //    gvRelations.Invalidate();
        //}

        //private void cbRelationChildTable_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    DiagramTable? dt = (DiagramTable?)cbRelationChildTable.SelectedItem;
        //    if (dt == null)
        //    {
        //        dr["child_schema_name"] = DBNull.Value;
        //        dr["child_table_name"] = DBNull.Value;
        //    }
        //    else
        //    {
        //        dr["child_schema_name"] = dt.SchemaName;
        //        dr["child_table_name"] = dt.TableName;
        //    }
        //    gvRelations.Invalidate();

        //    odcsRelationChildColumns.DiagramTable = dt;

        //    if (dt != _DiagramTable)
        //    {
        //        cbRelationParentTable.SelectedItem = _DiagramTable;
        //        odcsRelationParentColumns.DiagramTable = _DiagramTable;
        //        odcsRelationParentColumns.SelectedColumnsStringValue = string.Join(',', _DiagramTable.Columns.Where(i => i.PrimaryKey).Select(i => i.ColumnName));
        //    }
        //    cbRelationParentTable.Enabled = dt == _DiagramTable;
        //}

        //private void odcsRelationChildColumns_SelectedColumnsChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    dr["child_columns"] = odcsRelationChildColumns.SelectedColumnsStringValue;
        //}

        //private void cbRelationChildType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    RelationTypeOptionListItem? o = (RelationTypeOptionListItem?)cbRelationChildType.SelectedItem;
        //    if (o == null)
        //    {
        //        dr["child_type"] = DBNull.Value;
        //    }
        //    else
        //    {
        //        dr["child_type"] = o.Value.ToString();
        //    }
        //    gvRelations.Invalidate();
        //}

        //private void cbRelationOnUpdate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    PropagationOptionListItem? o = (PropagationOptionListItem?)cbRelationOnUpdate.SelectedItem;
        //    if (o == null)
        //    {
        //        dr["on_update"] = DBNull.Value;
        //    }
        //    else
        //    {
        //        dr["on_update"] = o.Value.ToString();
        //    }
        //}

        //private void cbRelationOnDelete_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_IgnoreChangeEvents) return;
        //    if (gvRelations.SelectedRows.Count != 1) return;
        //    DataRow dr = _Relations.Rows[gvRelations.SelectedRows[0].Index];

        //    PropagationOptionListItem? o = (PropagationOptionListItem?)cbRelationOnDelete.SelectedItem;
        //    if (o == null)
        //    {
        //        dr["on_delete"] = DBNull.Value;
        //    }
        //    else
        //    {
        //        dr["on_delete"] = o.Value.ToString();
        //    }
        //}

        //#endregion

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, lblTableName.Text), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tc.SelectedTab = tcGeneral;
                txtTableName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSchemaName.Text))
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, lblTableName.Text), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tc.SelectedTab = tcGeneral;
                txtTableName.Focus();
                return false;
            }

            for (int i = 0; i < _Columns.Rows.Count; i++)
            {
                DataRow dr = _Columns.Rows[i];

                if (string.IsNullOrWhiteSpace(dr.Field<string?>("name")))
                {
                    MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, lblColumnName.Text), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    txtColumnName.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(dr.Field<string?>("type_name")))
                {
                    MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, lblColumnType.Text), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    cbColumnType.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(dr.Field<string?>("type_initials")))
                {
                    MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, lblColumnTypeInitials.Text), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcColumns;
                    gvColumns.Rows[i].Selected = true;
                    txtColumnTypeInitials.Focus();
                    return false;
                }
            }

            return true;
        }

        public List<DiagramColumn> GetProvisionalColumnsFromStringDataValue(DiagramTable dt, string v)
        {
            if (dt == _DiagramTable)
            {
                List<DiagramColumn> l = new List<DiagramColumn>();
                foreach (string s in v.Split(','))
                {
                    DataRow? dr = _Columns.Rows.Cast<DataRow>().FirstOrDefault(i => i.Field<string>("name") == s);
                    if (dr == null) continue;

                    l.Add(GetProvisionalColumn(dr));
                }

                return l;
            }
            else
            {
                return dt.GetColumnsFromStringDataValue(v);
            }
        }

        private DiagramColumn GetProvisionalColumn(DataRow dr)
        {
            string typeName = dr.Field<string>("type_name")!;
            string? typeParams;

            ParseTypeName(ref typeName, out typeParams);

            string? defaultValue = (dr["default"] == DBNull.Value ? null : dr.Field<string?>("default"));

            return new DiagramColumn(dr.Field<string>("name")!, typeName, typeParams, defaultValue, (bool)dr["is_identity"], (bool)dr["pk"], (bool)dr["not_null"], dr.Field<string>("type_initials")!);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            // Check if there is another table with the same name
            foreach (DiagramTable dti in _DiagramTable.Diagram.Tables)
            {
                if (dti != _DiagramTable && dti.TableName.ToUpper().Trim() == _DiagramTable.TableName.ToUpper().Trim() && dti.SchemaName.ToUpper().Trim() == _DiagramTable.SchemaName.ToUpper().Trim())
                {
                    MessageBox.Show(this, Properties.Text.warning_table_already_exists, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tc.SelectedTab = tcGeneral;
                    txtTableName.Focus();
                    return;
                }
            }

            // Check if there are columns and relations to remove
            List<DiagramColumn> columnsToRemove = new List<DiagramColumn>();
            foreach (DiagramColumn dci in _DiagramTable.Columns)
            {
                bool found = false;
                foreach (DataRow dri in _Columns.Rows)
                {
                    if (dri.RowState != DataRowState.Deleted && dci.ColumnName == (string)dri["original_name"])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) columnsToRemove.Add(dci);
            }

            List<DiagramRelation> relationsToRemove = new List<DiagramRelation>();
            foreach (DiagramRelation dtri in _DiagramTable.Relations)
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
            foreach (DiagramRelation dtri in relationsToRemove) _DiagramTable.Relations.Remove(dtri);

            List<DiagramColumn> columns = new List<DiagramColumn>();

            foreach (DataRow dr in _Columns.Rows)
            {
                string typeName = dr.Field<string>("type_name")!;
                string? typeParams;

                ParseTypeName(ref typeName, out typeParams);

                string? defaultValue = (dr["default"] == DBNull.Value ? null : dr.Field<string?>("default"));

                DiagramColumn dc;

                if (dr["original_name"] == DBNull.Value)
                {
                    dc = new DiagramColumn(dr.Field<string>("name")!, typeName, typeParams, defaultValue, (bool)dr["is_identity"], (bool)dr["pk"], (bool)dr["not_null"], dr.Field<string>("type_initials")!);
                }
                else
                {
                    dc = _DiagramTable.Columns.First(dci => dci.ColumnName == dr.Field<string?>("original_name"));
                    dc.ColumnName = dr.Field<string>("name")!;
                    dc.TypeName = typeName;
                    dc.TypeParams = typeParams;
                    dc.DefaultValue = defaultValue;
                    dc.IsIdentity = (bool)dr["is_identity"];
                    dc.PrimaryKey = (bool)dr["pk"];
                    dc.NotNull = (bool)dr["not_null"];
                    dc.TypeInitials = dr.Field<string>("type_initials")!;
                }

                columns.Add(dc);
            }

            _DiagramTable.TableName = txtTableName.Text;
            _DiagramTable.SchemaName = txtSchemaName.Text;
            _DiagramTable.Columns = columns;
            _DiagramTable.RefreshDimensions();

            DialogResult = DialogResult.OK;
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

            this.tsbAddColumn.Text = Properties.Text.new_column;
            this.tsbRemoveColumn.Text = Properties.Text.remove_column;
            this.gvColumns.Columns[0].HeaderText = Properties.Text.name;
            this.gvColumns.Columns[1].HeaderText = Properties.Text.type_name;
            this.gvColumns.Columns[2].HeaderText = Properties.Text.pk;
            this.gvColumns.Columns[3].HeaderText = Properties.Text.not_null;
            this.lblColumnName.Text = Properties.Text.column_name + ":";
            this.lblColumnType.Text = Properties.Text.type_name + ":";
            this.lblColumnTypeInitials.Text = Properties.Text.type_initials + ":";
            this.lblColumnPrimaryKey.Text = Properties.Text.pk + ":";
            this.lblColumnIdentity.Text = Properties.Text.identity + ":";
            this.lblColumnDefault.Text = Properties.Text.column_default + ":";
            this.lblColumnNotNull.Text = Properties.Text.not_null + ":";
        }
        #endregion

        public class RelationTypeOptionListItem
        {
            public readonly DiagramRelation.RelationTypeOptions Value;
            public RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return DiagramRelation.RelationTypeOptionsToString(Value);
            }
        }

        public class PropagationOptionListItem
        {
            public readonly DiagramRelation.PropagationOptions Value;
            public PropagationOptionListItem(DiagramRelation.PropagationOptions v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return DiagramRelation.PropagationOptionsToString(Value);
            }
        }
    }
}

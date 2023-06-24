using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.DataStructure;
using Npgsql;
using Npgsql.Schema;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PgMulti.AppData;

namespace PgMulti.Tasks
{
    public abstract class Query
    {
        public const int MaxSqlDescLength = 200;

        public bool MaxRowsReached = false;
        public readonly int Index;
        public readonly string Sql;
        public readonly DataTable DataTable;
        public readonly List<QueryColumn> Columns;

        protected Data _Data;
        protected Table? _Table;
        protected bool _Editable;

        private Dictionary<DataRow, Dictionary<int, bool>>? _Editions = null;
        internal Dictionary<DataRow, DataRow> _InsertedRowsMapping = new Dictionary<DataRow, DataRow>();

        public Query(Data d, int index, string sql)
        {
            _Data = d;
            Index = index;
            Sql = sql;
            DataTable = new DataTable();
            Columns = new List<QueryColumn>();
            _Table = null;
            _Editable = false;
        }

        public string Description
        {
            get
            {
                string descSql = Sql.Replace("\r\n", " ").Replace("\n", " ");
                return string.Format(Properties.Text.statement_n, Index + 1)
                        + (MaxRowsReached ? " " + string.Format(Properties.Text.rows_limit_warning_short, _Data.Config.MaxRows) : "")
                        + ": " + (descSql.Length > MaxSqlDescLength ? descSql.Substring(0, MaxSqlDescLength - 3) + "..." : descSql);
            }
        }

        public bool Editable
        {
            get
            {
                return _Editable;
            }
        }

        public Table? Table
        {
            get
            {
                return _Table;
            }
        }

        internal void _SetEditedCell(DataRow drCurrent, int columnIndex)
        {
            if (_Editions == null)
            {
                _Editions = new Dictionary<DataRow, Dictionary<int, bool>>();
            }

            if (!_Editions.ContainsKey(drCurrent))
            {
                _Editions[drCurrent] = new Dictionary<int, bool>();
            }

            _Editions[drCurrent][columnIndex] = true;
        }

        public bool GetEditedCell(DataRow dr, int columnIndex)
        {
            if (_Editions == null) return false;
            if (!_Editions.ContainsKey(dr)) return false;
            if (!_Editions[dr].ContainsKey(columnIndex)) return false;

            return true;
        }

        public abstract void SetEditedCell(DataRow drCurrent, int columnIndex);
        public abstract void SetDeletedRow(DataRow drCurrent);
        public abstract void CommitChanges();


        public virtual void _CommitChanges()
        {
            _Editions = null;
            _InsertedRowsMapping.Clear();
            _Editable = false;
            DataTable.AcceptChanges();
        }

        public virtual void ShowInGridView(DataGridView gv, ToolStripButton tsbDeleteRow, ToolStripDropDownButton tsddbInsertRow)
        {
            gv.Tag = null;
            gv.DataSource = DataTable;

            for (int i = 0; i < Columns.Count; i++)
            {
                QueryColumn qc = Columns[i];
                DataGridViewColumn dgvc=gv.Columns[i];
                dgvc.HeaderText = qc.Title;
                dgvc.Tag = qc;
                dgvc.Resizable = DataGridViewTriState.True;

                if (qc.Column!=null && qc.Column.IsBoolean && !qc.Column.NotNull && dgvc is DataGridViewCheckBoxColumn)
                {
                    DataGridViewCheckBoxColumn dgvcbc = (DataGridViewCheckBoxColumn)dgvc;
                    dgvcbc.ThreeState = true;
                }
            }

            tsddbInsertRow.DropDownItems.Clear();


            if (Editable)
            {
                gv.ReadOnly = false;
                gv.AllowUserToDeleteRows = true;
                tsbDeleteRow.Visible = true;
                tsddbInsertRow.Visible = true;

                for (int i = 0; i < Columns.Count; i++)
                {
                    if (Columns[i].Editable)
                    {
                        gv.Columns[i].ReadOnly = false;
                    }
                    else if (Columns[i].Column != null && Columns[i].Column!.PK)
                    {
                        gv.Columns[i].ReadOnly = false;
                        for (int j = 0; j < gv.RowCount; j++)
                        {
                            if (((DataRowView)gv.Rows[j].DataBoundItem).Row.RowState != DataRowState.Added)
                            {
                                gv[i, j].ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        gv.Columns[i].ReadOnly = true;
                    }

                }
            }
            else
            {
                gv.ReadOnly = true;
                gv.AllowUserToDeleteRows = false;
                tsbDeleteRow.Visible = false;
                tsddbInsertRow.Visible = false;
            }

            gv.Tag = this;
        }

        public List<Tuple<DB, string>>? GenerateSql()
        {
            if (!Editable) return null;

            List<QueryColumn> columnasPK = Columns.Where(ci => ci.Column != null && ci.Column.PK).ToList();

            Dictionary<DB, StringBuilder> statements = new Dictionary<DB, StringBuilder>();
            for (int i = 0; i < DataTable.Rows.Count; i++)
            {
                DataRow dr = DataTable.Rows[i];
                switch (dr.RowState)
                {
                    case DataRowState.Added:
                    case DataRowState.Deleted:
                    case DataRowState.Modified:
                        break;
                    default:
                        continue;
                }

                DB db = GetDBOfRow(dr);
                StringBuilder sb;
                if (statements.ContainsKey(db))
                {
                    sb = statements[db];
                }
                else
                {
                    sb = new StringBuilder();
                    statements[db] = sb;
                }

                switch (dr.RowState)
                {
                    case DataRowState.Added:
                        sb.Append("INSERT INTO " + Table!.IdSchema + "." + Table!.Id);

                        List<string> cols = new List<string>();
                        List<string> vals = new List<string>();
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            if (dc.ColumnName == "__") continue;
                            QueryColumn c = Columns[int.Parse(dc.ColumnName.Substring(1))];
                            if (c.Editable || c.Column != null && dr[c.Index] != null && dr[c.Index] != DBNull.Value)
                            {
                                cols.Add(c.Column!.Id);
                                vals.Add(c.Column.GetSqlLiteralValue(dr[c.Index]));
                            }
                        }
                        sb.Append(" (" + string.Join(",", cols) + ") VALUES (");
                        sb.AppendLine(string.Join(",", vals) + ");");
                        break;
                    case DataRowState.Deleted:
                        sb.Append("DELETE FROM " + Table!.IdSchema + "." + Table!.Id);
                        BuildQueryWhereClause(sb, dr, columnasPK);
                        sb.AppendLine(";");
                        break;
                    case DataRowState.Modified:
                        sb.Append("UPDATE " + Table!.IdSchema + "." + Table!.Id + " SET ");

                        List<string> sets = new List<string>();
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            if (dc.ColumnName == "__") continue;
                            QueryColumn c = Columns[int.Parse(dc.ColumnName.Substring(1))];
                            if (c.Column != null && GetEditedCell(dr, c.Index))
                            {
                                sets.Add(c.Column.Id + "=" + c.Column.GetSqlLiteralValue(dr[c.Index]));
                            }
                        }
                        sb.Append(string.Join(",", sets));
                        BuildQueryWhereClause(sb, dr, columnasPK);
                        sb.AppendLine(";");
                        break;
                    default:
                        throw new Exception();
                }
            }

            List<Tuple<DB, string>> l = new List<Tuple<DB, string>>();
            foreach (var item in statements)
            {
                l.Add(new Tuple<DB, string>(item.Key, item.Value.ToString()));
            }

            return l;
        }

        private void BuildQueryWhereClause(StringBuilder sb, DataRow dr, List<QueryColumn> pkColumns)
        {
            sb.Append(" WHERE ");

            for (int i = 0; i < pkColumns.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(" AND ");
                }

                QueryColumn c = pkColumns[i];
                object v;

                if (dr.RowState == DataRowState.Deleted)
                {
                    v = dr[c.Index, DataRowVersion.Original];
                }
                else
                {
                    v = dr[c.Index];
                }

                sb.Append(c.Column!.Id + "=" + c.Column.GetSqlLiteralValue(v));
            }
        }

        public DataRow? FindRowsWithSamePK(DataRow drAnotherTable)
        {
            List<QueryColumn> pkColumns = Columns.Where(ci => ci.Column != null && ci.Column.PK).ToList();
            object[] ids = new object[pkColumns.Count];

            for (int i = 0; i < pkColumns.Count; i++)
            {
                QueryColumn c = pkColumns[i];

                if (drAnotherTable.RowState == DataRowState.Deleted)
                {
                    ids[i] = drAnotherTable[c.Index, DataRowVersion.Original];
                }
                else
                {
                    ids[i] = drAnotherTable[c.Index];
                }
            }

            for (int i = 0; i < DataTable.Rows.Count; i++)
            {
                DataRow dri = DataTable.Rows[i];
                bool matchingRow = true;
                for (int j = 0; j < pkColumns.Count; j++)
                {
                    QueryColumn c = pkColumns[j];
                    object v;
                    if (dri.RowState == DataRowState.Deleted)
                    {
                        v = dri[c.Index, DataRowVersion.Original];
                    }
                    else
                    {
                        v = dri[c.Index];
                    }

                    if (v != ids[j])
                    {
                        matchingRow = false;
                        break;
                    }
                }

                if (matchingRow)
                {
                    return dri;
                }
            }

            return null;
        }

        public abstract DB GetDBOfRow(DataRow dr);

        public class QueryColumn
        {
            public string Title;
            public Column? Column;
            public int Index;

            public bool Editable
            {
                get
                {
                    return Column != null && !Column.PK;
                }
            }

            public QueryColumn(int index, string titulo)
            {
                Title = titulo.ToLower();
                Column = null;
                Index = index;
            }
        }
    }
}

using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.DataStructure;
using Npgsql;
using Npgsql.Schema;
using System.Data;
using PgMulti.AppData;

namespace PgMulti.Tasks
{
    public class QueryIntegrator : Query
    {

        private List<QueryExecutorSql> _IntegratedQueries = new List<QueryExecutorSql>();
        internal Dictionary<DataRow, QueryExecutorSql> _InsertedRowsQueriesMapping = new Dictionary<DataRow, QueryExecutorSql>();

        public QueryIntegrator(Data d, int index, string sql) : base(d, index, sql)
        {
        }

        public List<QueryExecutorSql> IntegratedQueries
        {
            get
            {
                return _IntegratedQueries;
            }
        }

        public override void _CommitChanges()
        {
            base._CommitChanges();
            _InsertedRowsQueriesMapping.Clear();
        }

        public void Integrate(Data d, QueryExecutorSql ces)
        {
            _IntegratedQueries.Add(ces);
            ces._QueryIntegrator = this;

            Dictionary<int, int> mapping = new Dictionary<int, int>();

            if (Columns.Count == 0)
            {
                _Table = ces.Table;

                for (int i = 0; i < ces.Columns.Count; i++)
                {
                    QueryColumn c = ces.Columns[i];
                    Columns.Add(c);
                    DataTable.Columns.Add("_" + i.ToString());
                    mapping[i] = i;
                }

                DataTable.Columns.Add("__");

                _Editable = ces.Editable;

                foreach (QueryColumn qc in ces.Columns)
                {
                    if (qc.Column != null && qc.Column.IsBoolean)
                    {
                        DataColumn dc = DataTable.Columns["_" + qc.Index]!;
                        dc.DataType = typeof(bool);
                        if (qc.Column.NotNull)
                        {
                            dc.AllowDBNull = false;
                        }
                        else
                        {
                            dc.AllowDBNull = true;
                        }
                    }
                }
            }
            else
            {
                if (Columns.Count != ces.Columns.Count) throw new IncompatibleQueryException();
                if (_Editable != ces.Editable) throw new IncompatibleQueryException();
                for (int i = 0; i < Columns.Count; i++)
                {
                    QueryColumn c1 = Columns[i];
                    QueryColumn c2 = ces.Columns[i];

                    if (
                        c1.Title == c2.Title
                        && (c1.Column == null) == (c2.Column == null)
                        && (
                            c1.Column == null
                            || (
                                c1.Column.Id == c2.Column!.Id
                                && c1.Column.IdTable == c2.Column.IdTable
                                && c1.Column.IdSchema == c2.Column.IdSchema
                            )
                        )
                    )
                    {
                        mapping[i] = i;
                    }
                    else
                    {
                        bool found = false;

                        for (int j = 0; j < ces.Columns.Count; j++)
                        {
                            c2 = ces.Columns[j];

                            if (
                                !mapping.Values.Contains(j)
                                && c1.Title == c2.Title
                                && (c1.Column == null) == (c2.Column == null)
                                && (
                                    c1.Column == null
                                    || (
                                        c1.Column.Id == c2.Column!.Id
                                        && c1.Column.IdTable == c2.Column.IdTable
                                        && c1.Column.IdSchema == c2.Column.IdSchema
                                    )
                                )
                            )
                            {
                                found = true;
                                mapping[i] = j;
                                break;
                            }
                        }

                        if (!found)
                        {
                            throw new IncompatibleQueryException();
                        }
                    }
                }
            }

            for (int i = 0; i < ces.DataTable.Rows.Count && DataTable.Rows.Count < d.Config.MaxRows; i++)
            {
                DataRow dr1 = ces.DataTable.Rows[i];
                DataRow dr2 = DataTable.NewRow();

                for (int j = 0; j < Columns.Count; j++)
                {
                    dr2[j] = dr1[mapping[j]];
                }
                dr2[Columns.Count] = ces.DB.Alias;

                DataTable.Rows.Add(dr2);
            }

            if (DataTable.Rows.Count == d.Config.MaxRows) MaxRowsReached = true;

            DataTable.AcceptChanges();
        }

        public override DB GetDBOfRow(DataRow dr)
        {
            string dbAlias;

            if (dr.RowState == DataRowState.Deleted)
            {
                dbAlias = (string)dr[dr.Table.Columns.Count - 1, DataRowVersion.Original];
            }
            else
            {
                dbAlias = (string)dr[dr.Table.Columns.Count - 1];
            }

            return _IntegratedQueries.First(ces => ces.DB.Alias == dbAlias).DB;
        }

        public override void ShowInGridView(DataGridView gv, ToolStripButton tsbDeleteRow, ToolStripDropDownButton tsddbInsertRow)
        {
            base.ShowInGridView(gv, tsbDeleteRow, tsddbInsertRow);

            gv.Columns[Columns.Count].HeaderText = Properties.Text.db_field_name;
            gv.Columns[Columns.Count].Tag = null;
            gv.Columns[Columns.Count].Resizable = DataGridViewTriState.True;
            gv.Columns[Columns.Count].ReadOnly = true;

            foreach (QueryExecutorSql ces in _IntegratedQueries)
            {
                ToolStripMenuItem tsmiInsertRow = new ToolStripMenuItem();
                tsmiInsertRow.Image = Properties.Resources.tva_db;
                tsmiInsertRow.Text = string.Format(Properties.Text.insert_in, ces.DB.Alias);
                tsmiInsertRow.Tag = new Tuple<DataGridView, QueryExecutorSql>(gv, ces);
                tsmiInsertRow.Click += tsmiInsertRow_Click;

                tsddbInsertRow.DropDownItems.Add(tsmiInsertRow);
            }
        }

        private void tsmiInsertRow_Click(object? sender, EventArgs e)
        {
            Tuple<DataGridView, QueryExecutorSql> t = (Tuple<DataGridView, QueryExecutorSql>)((ToolStripMenuItem)sender!).Tag!;
            DataGridView gv = t.Item1;
            QueryExecutorSql ces = t.Item2;

            InsertRow(ces);
            gv.Focus();
            gv.FirstDisplayedScrollingRowIndex = gv.RowCount - 1;
        }

        public override void SetEditedCell(DataRow drCurrent, int columnIndex)
        {
            _SetEditedCell(drCurrent, columnIndex);

            string dbAlias = drCurrent.Field<string>(Columns.Count)!;
            QueryExecutorSql c = _IntegratedQueries.First(ci => ci.DB.Alias == dbAlias);

            DataRow? drOtra = null;
            if (_InsertedRowsMapping.ContainsKey(drCurrent))
            {
                drOtra = _InsertedRowsMapping[drCurrent];
            }
            else
            {
                drOtra = c.FindRowsWithSamePK(drCurrent);
            }
            if (drOtra == null) throw new Exception();

            drOtra[columnIndex] = drCurrent[columnIndex];
            c._SetEditedCell(drOtra, columnIndex);
        }

        public override void SetDeletedRow(DataRow drCurrent)
        {
            QueryExecutorSql c;
            if (drCurrent.RowState == DataRowState.Detached)
            {
                c = _InsertedRowsQueriesMapping[drCurrent];
            }
            else
            {
                string dbAlias = drCurrent.Field<string>(Columns.Count, DataRowVersion.Original)!;
                c = _IntegratedQueries.First(ci => ci.DB.Alias == dbAlias);
            }

            DataRow? drOther = null;
            if (_InsertedRowsMapping.ContainsKey(drCurrent))
            {
                drOther = _InsertedRowsMapping[drCurrent];
            }
            else
            {
                drOther = c.FindRowsWithSamePK(drCurrent);
            }
            if (drOther == null) throw new Exception();

            drOther.Delete();
        }

        public void InsertRow(QueryExecutorSql ces)
        {
            DataRow drCurrent = DataTable.NewRow();
            drCurrent[Columns.Count] = ces.DB.Alias;
            DataTable.Rows.Add(drCurrent);

            DataRow drOther = ces.DataTable.NewRow();
            ces.DataTable.Rows.Add(drOther);

            _InsertedRowsMapping[drCurrent] = drOther;
            _InsertedRowsQueriesMapping[drCurrent] = ces;
            ces._InsertedRowsMapping[drOther] = drCurrent;
        }

        public override void CommitChanges()
        {
            _CommitChanges();

            foreach (QueryExecutorSql c in _IntegratedQueries)
            {
                c._CommitChanges();
            }
        }

        public class IncompatibleQueryException : Exception
        {

        }
    }
}

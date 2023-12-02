using Irony.Parsing;
using PgMulti.DataStructure;
using Npgsql;
using Npgsql.Schema;
using System.Data;
using System.Text;
using PgMulti.AppData;
using PgMulti.SqlSyntax;

namespace PgMulti.Tasks
{
    public class QueryExecutorSql : Query
    {

        private PgTaskExecutorSqlTables _TaskExecutor;
        internal QueryIntegrator? _QueryIntegrator = null;

        public QueryExecutorSql(Data d, PgTaskExecutorSqlTables tes, int index, string sql) : base(d, index, sql)
        {
            _Data = d;
            _TaskExecutor = tes;
        }

        public DB DB
        {
            get
            {
                return _TaskExecutor.DB;
            }
        }

        public void Load(NpgsqlDataReader drd)
        {
            var cols = drd.GetColumnSchema();
            for (int i = 0; i < cols.Count; i++)
            {
                NpgsqlDbColumn dc = cols[i];
                Columns.Add(new QueryColumn(i, dc.ColumnName));
                DataTable.Columns.Add("_" + i);
            }

            CalculateEditable();

            foreach (QueryColumn qc in Columns)
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

            while (drd.Read() && DataTable.Rows.Count < _Data.Config.MaxRows)
            {
                DataRow dr = DataTable.NewRow();
                for (int i = 0; i < DataTable.Columns.Count; i++)
                {
                    QueryColumn qc = Columns[i];

                    if (qc.Column != null && qc.Column.IsBoolean)
                    {
                        string? v = drd[i] == DBNull.Value ? null : (string)drd[i];
                        if (v == null)
                        {
                            dr[i] = DBNull.Value;
                        }
                        else if (v == "t")
                        {
                            dr[i] = true;
                        }
                        else if (v == "f")
                        {
                            dr[i] = false;
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        dr[i] = drd[i];
                    }
                }
                DataTable.Rows.Add(dr);
            }
            if (DataTable.Rows.Count == _Data.Config.MaxRows) MaxRowsReached = true;

            DataTable.AcceptChanges();
        }

        public override DB GetDBOfRow(DataRow dr)
        {
            return DB;
        }

        private void CalculateEditable()
        {
            Parser p = new Parser(_Data.PGLanguageData);

            ParseTree pt = p.Parse(Sql);
            if (pt.HasErrors())
            {
                return;
            }

            AstNode root = AstNode.ProcesarParseTree(pt);
            if (root["stmtList"]!.Children.Where(n => n.Name=="stmt").Count() != 1 || root["stmtList"]!["stmt"]![0].Name != "selectStmt")
            {
                return;
            }

            AstNode nSelectStmt = root["stmtList"]!["stmt"]![0];

            if (nSelectStmt[0] == null || nSelectStmt[0]["selectBaseClauses"] == null) return;

            AstNode nSelectBaseClauses = nSelectStmt[0]["selectBaseClauses"]!;

            if (
                    nSelectBaseClauses["intoClauseOpt"] != null
                    || nSelectBaseClauses["groupClauseOpt"] != null
                    || nSelectBaseClauses["fromClauseOpt"] == null
                    || nSelectBaseClauses["fromClauseOpt"]!["fromItemList"]!["joinChainOpt"] != null
                    || nSelectBaseClauses["fromClauseOpt"]!["fromItemList"]!["fromItem"]![0]["id"] == null
                )
            {
                return;
            }

            AstNode nFromItem = nSelectBaseClauses["fromClauseOpt"]!["fromItemList"]!["fromItem"]!;
            PostgreSqlIdParser idParser = new PostgreSqlIdParser(_Data.PGLanguageData);
            PostgreSqlId? id = idParser.TryParse(nFromItem[0]["id"]!);

            if (id == null || id.Values.Length > 2) return;

            string idTable = id.Values.Last();
            string idSchema = id.Values.Length == 1 ? "public" : id.Values[0];

            Schema? schema = DB.Schemas.FirstOrDefault(ei => ei.Id == idSchema);
            if (schema == null)
            {
                return;
            }

            _Table = schema.Tables.FirstOrDefault(ti => ti.Id == idTable);
            if (_Table == null)
            {
                return;
            }

            foreach (AstNode nSelItem in nSelectBaseClauses["selList"]!.Children)
            {
                if (nSelItem["aliasOpt"] != null && _Table.Columns.Any(ci => ci.Id == SqlSyntax.PostgreSqlGrammar.IdFromString(nSelItem["aliasOpt"]!["id_simple"]!.SingleLineText)))
                {
                    return;
                }
            }

            foreach (QueryColumn cc in Columns)
            {
                cc.Column = _Table.Columns.FirstOrDefault(ci => ci.Id == cc.Title);
            }

            List<Column> colsPK = _Table.Columns.Where(ci => ci.PK).ToList();
            foreach (Column cpk in colsPK)
            {
                if (!Columns.Any(ci => ci.Column == cpk))
                {
                    return;
                }
            }

            _Editable = colsPK.Count > 0;
        }

        public override void SetEditedCell(DataRow drActual, int columnIndex)
        {
            _SetEditedCell(drActual, columnIndex);

            if (_QueryIntegrator != null)
            {
                DataRow? drOtra = null;
                if (_InsertedRowsMapping.ContainsKey(drActual))
                {
                    drOtra = _InsertedRowsMapping[drActual];
                }
                else
                {
                    drOtra = _QueryIntegrator.FindRowsWithSamePK(drActual);
                }
                if (drOtra == null) throw new Exception();

                drOtra[columnIndex] = drActual[columnIndex];
                _QueryIntegrator._SetEditedCell(drOtra, columnIndex);
            }
        }

        public override void SetDeletedRow(DataRow drCurrent)
        {
            if (_QueryIntegrator != null)
            {
                DataRow? drOther;
                if (_InsertedRowsMapping.ContainsKey(drCurrent))
                {
                    drOther = _InsertedRowsMapping[drCurrent];
                }
                else
                {
                    drOther = _QueryIntegrator.FindRowsWithSamePK(drCurrent);
                }
                if (drOther == null) throw new Exception();

                drOther.Delete();
            }
        }

        public void InsertRow()
        {
            DataRow drCurrent = DataTable.NewRow();

            foreach (DataColumn dc in DataTable.Columns)
            {
                if (!dc.AllowDBNull)
                {
                    if (dc.DefaultValue == DBNull.Value)
                    {
                        drCurrent[dc.ColumnName] = Activator.CreateInstance(dc.DataType);
                    }
                    else
                    {
                        drCurrent[dc.ColumnName] = dc.DefaultValue;
                    }
                }
            }

            DataTable.Rows.Add(drCurrent);

            if (_QueryIntegrator != null)
            {
                DataRow drOtra = _QueryIntegrator.DataTable.NewRow();

                for (int i = 0; i < Columns.Count; i++)
                {
                    drOtra[i] = drCurrent[i];
                }
                drOtra[Columns.Count] = DB.Alias;

                _QueryIntegrator.DataTable.Rows.Add(drOtra);

                _InsertedRowsMapping[drCurrent] = drOtra;
                _QueryIntegrator._InsertedRowsMapping[drOtra] = drCurrent;
                _QueryIntegrator._InsertedRowsQueriesMapping[drOtra] = this;
            }
        }

        public override void CommitChanges()
        {
            _CommitChanges();

            if (_QueryIntegrator != null)
            {
                _QueryIntegrator._CommitChanges();
            }
        }
    }
}

using Irony.Parsing;
using PgMulti.DataStructure;
using Npgsql;
using Npgsql.Schema;
using System.Data;
using System.Text;
using PgMulti.AppData;
using PgMulti.SqlSyntax;
using System.Text.RegularExpressions;
using System.Globalization;
using PgMulti.DataAccess;
using Microsoft.VisualBasic.Logging;
using NpgsqlTypes;
using System.Collections;

namespace PgMulti.Tasks
{
    public class QueryExecutorSql : Query
    {
        private PgTaskExecutorSqlTables _TaskExecutor;
        internal QueryIntegrator? _QueryIntegrator = null;

        public QueryExecutorSql(Data d, PgTaskExecutorSqlTables tes, int index, string sql) : base(d, tes, index, sql)
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


        public string? Load(NpgsqlDataReader drd)
        {
            string? log = null;

            var cols = drd.GetColumnSchema();
            for (int i = 0; i < cols.Count; i++)
            {
                NpgsqlDbColumn dc = cols[i];
                Columns.Add(new QueryColumn(i, dc.ColumnName));
                DataColumn dtc = new DataColumn("_" + i, dc.DataType!);

                dtc.DataType = Column.GetDataTableTypeMapping(dc.DataType);

                DataTable.Columns.Add(dtc);
            }

            CalculateEditable();

            foreach (QueryColumn qc in Columns)
            {
                DataColumn dc = DataTable.Columns["_" + qc.Index]!;

                if (qc.Column != null)
                {
                    if (qc.Column.IsBoolean)
                    {
                        dc.DataType = typeof(bool);
                    }
                    else if (qc.Column.IsShort)
                    {
                        dc.DataType = typeof(short);
                    }
                    else if (qc.Column.IsInt)
                    {
                        dc.DataType = typeof(int);
                    }
                    else if (qc.Column.IsLong)
                    {
                        dc.DataType = typeof(long);
                    }
                    else if (qc.Column.IsFloat)
                    {
                        dc.DataType = typeof(float);
                    }
                    else if (qc.Column.IsDouble)
                    {
                        dc.DataType = typeof(double);
                    }
                    else if (qc.Column.IsDecimal)
                    {
                        dc.DataType = typeof(decimal);
                    }

                    if (qc.Column.NotNull)
                    {
                        dc.AllowDBNull = false;
                    }
                    else
                    {
                        dc.AllowDBNull = true;
                    }
                }

                qc.PostgreSqlTypeName = drd.GetPostgresType(qc.Index).Name;
            }

            CultureInfo? monetaryCultureInfo = null;
            if (Columns.Any(qc => qc.PostgreSqlTypeName == "money"))
            {
                string lcMonetary;
                monetaryCultureInfo = DB.GetMonetaryCultureInfo(out lcMonetary);
                log = string.Format(string.Format(Properties.Text.money_culture_used, monetaryCultureInfo.Name, lcMonetary));
            }

            while (drd.Read() && DataTable.Rows.Count < _Data.Config.MaxRows)
            {
                DataRow dr = DataTable.NewRow();

                foreach (QueryColumn qc in Columns)
                {
                    DataColumn dc = DataTable.Columns["_" + qc.Index]!;
                    object o = Column.ConvertValue(drd, qc.Index, dc.DataType, qc.PostgreSqlTypeName!, monetaryCultureInfo);

                    dr[qc.Index] = o;
                }
                DataTable.Rows.Add(dr);
            }
            if (DataTable.Rows.Count == _Data.Config.MaxRows) MaxRowsReached = true;

            DataTable.AcceptChanges();

            return log;
        }

        private static Type[] numericTypesHierarchy = new Type[] { typeof(string), typeof(double), typeof(decimal), typeof(long), typeof(int), typeof(short) };
        private Type combineType(Type? t1, Type t2)
        {
            if (t1 == null) return t2;
            if (t1 == typeof(bool) && t2 == typeof(bool)) return typeof(bool);

            int n1 = Array.IndexOf(numericTypesHierarchy, t1);
            int n2 = Array.IndexOf(numericTypesHierarchy, t2);

            if (n1 == -1 || n2 == -1) return typeof(string);

            return numericTypesHierarchy[Math.Min(n1, n2)];
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

            AstNode root = AstNode.ProcessParseTree(pt);
            if (root["stmtList"]!.Children.Count != 1 || root["stmtList"]!["stmtAndSemi"]!["stmt"]![0].Name != "selectStmt")
            {
                return;
            }

            AstNode nSelectStmt = root["stmtList"]!["stmtAndSemi"]!["stmt"]![0];

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
                        drCurrent[dc.ColumnName] = dc.DataType == typeof(string) ? "" : Activator.CreateInstance(dc.DataType);
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

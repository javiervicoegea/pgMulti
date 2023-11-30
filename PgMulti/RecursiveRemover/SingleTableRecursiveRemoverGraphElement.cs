using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class SingleTableRecursiveRemoverGraphElement : RecursiveRemoverGraphElement
    {
        protected Table _Table { get; }

        public SingleTableRecursiveRemoverGraphElement(string schemaName, RecursiveRemover recursiveRemover, Table table) : base(schemaName, recursiveRemover)
        {
            _Table = table;
        }

        public override List<Table> Tables
        {
            get
            {
                List<Table> l = new List<Table>();
                l.Add(_Table);
                return l;
            }
        }

        public override bool ContainsTable(Table t)
        {
            return _Table == t;
        }

        public override void AddTablesToList(List<Table> list)
        {
            if (!list.Contains(_Table)) list.Add(_Table);
        }

        public override void WriteCollectTuplesSqlCommands(StringBuilder sb, bool delete)
        {
            sb.AppendLine("-- TABLE " + _Table.IdSchema + "." + _Table.Id + ":\r\n");
            _WriteCreateTableSqlCommand(sb, 3, _Table, delete);
            _WriteInsertSqlCommand(sb, delete);
        }

        public override void WriteDeleteTuplesSqlCommands(StringBuilder sb)
        {
            sb.AppendLine("-- TABLE " + _Table.IdSchema + "." + _Table.Id + ":\r\n");

            sb.AppendLine("   DELETE FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(_Table.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(_Table.Id) + " t");
            sb.AppendLine("   WHERE EXISTS(SELECT 1 FROM " + RecursiveRemover.GetCollectTableName(SchemaName, _Table, true) + " r WHERE " + string.Join(" AND ", _Table.Columns.Where(c => c.PK).Select(c => "r." + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id) + " = t." + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id))) + ");\r\n");
        }

        protected virtual void _WriteInsertSqlCommand(StringBuilder sb, bool delete)
        {
            foreach (TableRelation tr in _Table.Relations.Where(tri => tri.ChildTable == _Table && RecursiveRemover.Graph.Nodes.Any(ni => ni.Value.ContainsTable(tri.ParentTable!))))
            {
                Tuple<string, string>[] fkColumnMatch = new Tuple<string, string>[tr.ParentColumns.Length];

                for (int i = 0; i < tr.ParentColumns.Length; i++)
                {
                    fkColumnMatch[i] = new Tuple<string, string>(tr.ParentColumns[i], tr.ChildColumns[i]);
                }

                sb.AppendLine("--- FK " + _Table.IdSchema + "." + _Table.Id + "." + tr.Id + ":\r\n");
                sb.AppendLine("    INSERT INTO " + GetCollectTableName(_Table, delete));
                sb.AppendLine("    (" + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => SqlSyntax.PostgreSqlGrammar.IdToString(c.Id))) + ")");
                sb.AppendLine("    SELECT " + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => "t." + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id)).ToArray()));
                sb.AppendLine("    FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(_Table!.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(_Table!.Id) + " t");
                sb.AppendLine("    WHERE EXISTS");
                sb.AppendLine("    (");
                sb.AppendLine("        SELECT 1");
                sb.AppendLine("        FROM " + GetCollectTableName(tr.ParentTable!, delete) + " r");
                sb.AppendLine("        WHERE " + string.Join(" AND ", fkColumnMatch.Select(mi => "r." + mi.Item1 + " = t." + mi.Item2)));
                sb.AppendLine("    )");
                sb.AppendLine("    ON CONFLICT DO NOTHING;\r\n");
            }
        }

        public override string ToString()
        {
            return _Table.ToString();
        }
    }
}

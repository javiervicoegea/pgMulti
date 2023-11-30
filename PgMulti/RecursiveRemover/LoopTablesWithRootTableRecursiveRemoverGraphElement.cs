using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class LoopTablesWithRootTableRecursiveRemoverGraphElement : LoopTablesRecursiveRemoverGraphElement
    {
        private Table _RootTable;
        private string _DeleteWhereClause;
        private string _PreserveWhereClause;

        public LoopTablesWithRootTableRecursiveRemoverGraphElement(string schemaName, RecursiveRemover recursiveRemover, List<Table> tables, Table rootTable, string rootDeleteWhereClause, string rootPreserveTableWhereClause) : base(schemaName, recursiveRemover, tables)
        {
            _RootTable = rootTable;
            _DeleteWhereClause = rootDeleteWhereClause;
            _PreserveWhereClause = rootPreserveTableWhereClause;
        }

        protected override void _WriteRootInsertSqlCommand(Table t, StringBuilder sb, bool delete, string stepTuplesTableName)
        {
            if (_RootTable == t)
            {
                sb.AppendLine("---- Custom filter for initial table:\r\n");
                sb.AppendLine("     INSERT INTO " + stepTuplesTableName);
                sb.AppendLine("     (" + string.Join(",", _RootTable.Columns.Where(c => c.PK).Select(c => SqlSyntax.PostgreSqlGrammar.IdToString(c.Id))) + ")");
                sb.AppendLine("     SELECT " + string.Join(",", _RootTable.Columns.Where(c => c.PK).Select(c => "t." + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id)).ToArray()));
                sb.AppendLine("     FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(_RootTable!.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(_RootTable!.Id) + " t");
                sb.AppendLine("     WHERE " + (delete ? _DeleteWhereClause : _PreserveWhereClause));
                sb.AppendLine("     ON CONFLICT DO NOTHING;\r\n");
            }
        }

    }
}

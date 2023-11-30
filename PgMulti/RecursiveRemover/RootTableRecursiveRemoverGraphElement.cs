using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class RootTableRecursiveRemoverGraphElement : SingleTableRecursiveRemoverGraphElement
    {
        private string _DeleteWhereClause;
        private string _PreserveWhereClause;

        public RootTableRecursiveRemoverGraphElement(string schemaName, RecursiveRemover recursiveRemover, Table table, string rootDeleteWhereClause, string rootPreserveTableWhereClause) : base(schemaName, recursiveRemover, table)
        {
            _DeleteWhereClause = rootDeleteWhereClause;
            _PreserveWhereClause = rootPreserveTableWhereClause;
        }

        protected override void _WriteInsertSqlCommand(StringBuilder sb, bool delete)
        {
            sb.AppendLine("--- Custom filter for initial table:\r\n");
            sb.AppendLine("    INSERT INTO " + GetCollectTableName(_Table, delete));
            sb.AppendLine("    (" + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => SqlSyntax.PostgreSqlGrammar.IdToString(c.Id))) + ")");
            sb.AppendLine("    SELECT " + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => "t." + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id)).ToArray()));
            sb.AppendLine("    FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(_Table!.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(_Table!.Id) + " t");
            sb.AppendLine("    WHERE " + (delete ? _DeleteWhereClause : _PreserveWhereClause));
            sb.AppendLine("    ON CONFLICT DO NOTHING;\r\n");
        }
    }
}

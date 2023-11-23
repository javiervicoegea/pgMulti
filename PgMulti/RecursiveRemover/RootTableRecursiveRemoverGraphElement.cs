using PgMulti.DataStructure;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class RootTableRecursiveRemoverGraphElement : SingleTableRecursiveRemoverGraphElement
    {
        private string WhereClause;

        public RootTableRecursiveRemoverGraphElement(Table table, string whereClause) : base(table) { WhereClause = whereClause; }

        protected override void _WriteInsertSqlCommand(StringBuilder sb)
        {
            sb.AppendLine("--- Custom filter for initial table:\r\n");
            sb.AppendLine("    INSERT INTO recursiveremover." + _Table.IdSchema + "_" + _Table.Id);
            sb.AppendLine("    (" + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => c.Id)) + ")");
            sb.AppendLine("    SELECT " + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => "t." + c.Id).ToArray()));
            sb.AppendLine("    FROM " + _Table!.IdSchema + "." + _Table!.Id + " t");
            sb.AppendLine("    WHERE " + WhereClause);
            sb.AppendLine("    ON CONFLICT DO NOTHING;");
        }
    }
}

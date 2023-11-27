using PgMulti.DataStructure;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class RootTableRecursiveRemoverGraphElement : SingleTableRecursiveRemoverGraphElement
    {
        private string _DeleteWhereClause;
        private string _PreserveWhereClause;

        public RootTableRecursiveRemoverGraphElement(string schemaName, Table table, string rootDeleteWhereClause, string rootPreserveTableWhereClause) : base(schemaName, table)
        {
            _DeleteWhereClause = rootDeleteWhereClause;
            _PreserveWhereClause = rootPreserveTableWhereClause;
        }

        protected override void _WriteInsertSqlCommand(StringBuilder sb, bool delete)
        {
            sb.AppendLine("--- Custom filter for initial table:\r\n");
            sb.AppendLine("    INSERT INTO " + GetCollectTableName(_Table, delete));
            sb.AppendLine("    (" + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => c.Id)) + ")");
            sb.AppendLine("    SELECT " + string.Join(",", _Table.Columns.Where(c => c.PK).Select(c => "t." + c.Id).ToArray()));
            sb.AppendLine("    FROM " + _Table!.IdSchema + "." + _Table!.Id + " t");
            sb.AppendLine("    WHERE " + (delete ? _DeleteWhereClause : _PreserveWhereClause));
            sb.AppendLine("    ON CONFLICT DO NOTHING;\r\n");
        }
    }
}

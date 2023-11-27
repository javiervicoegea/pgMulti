using PgMulti.DataStructure;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public class LoopTablesWithRootTableRecursiveRemoverGraphElement: LoopTablesRecursiveRemoverGraphElement
    {
        private Table _RootTable;
        private string _DeleteWhereClause;
        private string _PreserveWhereClause;

        public LoopTablesWithRootTableRecursiveRemoverGraphElement(string schemaName, List<Table> tables, Table rootTable, string rootDeleteWhereClause, string rootPreserveTableWhereClause) : base(schemaName, tables)
        {
            _RootTable = rootTable;
            _DeleteWhereClause = rootDeleteWhereClause;
            _PreserveWhereClause = rootPreserveTableWhereClause;
        }

        public override Table? RootTable { get => _RootTable; }

        protected override void _WriteRootInsertSqlCommand(Table t, StringBuilder sb, bool delete, string stepTuplesTableName) 
        {
            if (_RootTable == t)
            {
                sb.AppendLine("---- Custom filter for initial table:\r\n");
                sb.AppendLine("     INSERT INTO " + stepTuplesTableName);
                sb.AppendLine("     (" + string.Join(",", _RootTable.Columns.Where(c => c.PK).Select(c => c.Id)) + ")");
                sb.AppendLine("     SELECT " + string.Join(",", _RootTable.Columns.Where(c => c.PK).Select(c => "t." + c.Id).ToArray()));
                sb.AppendLine("     FROM " + _RootTable!.IdSchema + "." + _RootTable!.Id + " t");
                sb.AppendLine("     WHERE " + (delete ? _DeleteWhereClause : _PreserveWhereClause));
                sb.AppendLine("     ON CONFLICT DO NOTHING;\r\n");
            }
        }

    }
}

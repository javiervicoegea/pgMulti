using PgMulti.DataStructure;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public abstract class RecursiveRemoverGraphElement
    {
        public abstract List<Table> Tables { get; }
        public abstract List<TableRelation> ParentRelations { get; }
        public abstract List<TableRelation> ChildRelations { get; }

        public abstract bool ContainsTable(Table t);

        public abstract void AddTablesToList(List<Table> list);

        public abstract void WriteCollectTuplesSqlCommands(StringBuilder sb);

        public bool ContainsElement(RecursiveRemoverGraphElement e)
        {
            foreach (Table t in e.Tables)
            {
                if (!ContainsTable(t)) return false;
            }

            return true;
        }

        protected virtual void _WriteCreateTableSqlCommand(StringBuilder sb, int indentation, Table t)
        {
            string indentationString = new string(' ', indentation);
            sb.AppendLine(indentationString + "CREATE TABLE recursiveremover." + t.IdSchema + "_" + t.Id);
            sb.AppendLine(indentationString + "(");
            sb.AppendLine(indentationString + string.Join(",\r\n", t.Columns.Where(c => c.PK).Select(c => "    " + c.Id + " " + c.Type + c.TypeParams + " PRIMARY KEY").ToArray()));
            sb.AppendLine(indentationString + ");\r\n");
        }
    }
}

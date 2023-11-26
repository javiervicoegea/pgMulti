using PgMulti.DataStructure;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public abstract class RecursiveRemoverGraphElement
    {
        public abstract List<Table> Tables { get; }
        public abstract List<TableRelation> ParentRelations { get; }
        public abstract List<TableRelation> ChildRelations { get; }

        public readonly string SchemaName;

        protected RecursiveRemoverGraphElement(string schemaName)
        {
            SchemaName = schemaName;
        }

        public abstract bool ContainsTable(Table t);

        public abstract void AddTablesToList(List<Table> list);

        public abstract void WriteCollectTuplesSqlCommands(StringBuilder sb, bool delete);

        public abstract void WriteDeleteTuplesSqlCommands(StringBuilder sb);

        public bool ContainsElement(RecursiveRemoverGraphElement e)
        {
            foreach (Table t in e.Tables)
            {
                if (!ContainsTable(t)) return false;
            }

            return true;
        }

        protected string GetCollectTableName(Table t, bool delete)
        {
            return RecursiveRemover.GetCollectTableName(SchemaName, t, delete);
        }

        protected virtual void _WriteCreateTableSqlCommand(StringBuilder sb, int indentation, Table t, bool delete)
        {
            RecursiveRemover.WriteCreateCollectTableSqlCommand(sb, indentation, t, GetCollectTableName(t, delete));
        }
    }
}

using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System.Text;

namespace PgMulti.RecursiveRemover
{
    public abstract class RecursiveRemoverGraphElement
    {
        public abstract List<Table> Tables { get; }

        public readonly string SchemaName;
        public readonly RecursiveRemover RecursiveRemover;

        protected RecursiveRemoverGraphElement(string schemaName, RecursiveRemover recursiveRemover)
        {
            SchemaName = schemaName;
            RecursiveRemover = recursiveRemover;
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

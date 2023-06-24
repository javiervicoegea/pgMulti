using Npgsql;

namespace PgMulti.DataStructure
{
    public class Table
    {
        private string _Id;
        private string _IdSchema;
        private List<Column> _Columns;
        private Schema? _Schema;
        private List<TableRelation> _Relations;
        private List<TableIndex> _Indexes;
        private List<Trigger> _Triggers;

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public List<Column> Columns { get => _Columns; }
        public Schema? Schema { get => _Schema; internal set => _Schema = value; }
        public List<TableRelation> Relations { get => _Relations; }
        public List<TableIndex> Indexes { get => _Indexes; }
        public List<Trigger> Triggers { get => _Triggers; }

        internal Table(NpgsqlDataReader drd)
        {
            _Columns = new List<Column>();
            _Relations = new List<TableRelation>();
            _Indexes = new List<TableIndex>();
            _Triggers = new List<Trigger>();

            _IdSchema = drd.Ref<string>("schemaname")!.ToLower();
            _Id = drd.Ref<string>("tablename")!.ToLower();
        }
    }
}

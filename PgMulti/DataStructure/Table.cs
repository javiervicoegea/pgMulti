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

        internal Table(NpgsqlDataReader drd) : this(drd.Ref<string>("schemaname")!, drd.Ref<string>("tablename")!)
        {
        }

        internal Table(Schema s, string id) : this(s.Id, id)
        {
            _Schema = s;
        }

        private Table(string schemaId, string tableId)
        {
            _Columns = new List<Column>();
            _Relations = new List<TableRelation>();
            _Indexes = new List<TableIndex>();
            _Triggers = new List<Trigger>();

            _IdSchema = schemaId;
            _Id = tableId;
        }

        public void CreateInDB(NpgsqlConnection connection, NpgsqlTransaction t)
        {
            NpgsqlCommand cmdCreateTable = new NpgsqlCommand();
            cmdCreateTable.CommandText = $"CREATE TABLE {IdSchema}.{Id} ({
                    string.Join(",", Columns.Select(c => 
                        c.Id + " " + c.Type 
                        + (string.IsNullOrWhiteSpace(c.TypeParams)?"":" " + c.TypeParams) 
                        + (c.NotNull?" NOT NULL":"") 
                        + (string.IsNullOrWhiteSpace(c.DefaultValue)?"":" DEFAULT" + c.DefaultValue) 
                        + (c.IsIdentity?" GENERATED " + (c.IsGeneratedAlways?"ALWAYS":"BY DEFAULT") + " AS IDENTITY":""))
                    )
                })";
            cmdCreateTable.Connection = connection;
            cmdCreateTable.Transaction = t;
            cmdCreateTable.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return IdSchema + "." + Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Table)) return false;

            Table objTable = (Table)obj;

            return objTable.IdSchema == IdSchema && objTable.Id == Id;
        }

        public override int GetHashCode()
        {
            return (_IdSchema + "." + _Id).GetHashCode();
        }
    }
}

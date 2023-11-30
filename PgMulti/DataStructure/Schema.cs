using Npgsql;
using PgMulti.AppData;

namespace PgMulti.DataStructure
{
    public class Schema
    {
        private string _Id;
        private List<Table> _Tables;
        private List<Function> _Functions;
        private DB _DB;

        public string Id { get => _Id; internal set => _Id = value; }
        public List<Table> Tables { get => _Tables; internal set => _Tables = value; }
        public List<Function> Functions { get => _Functions; internal set => _Functions = value; }
        public DB DB { get => _DB; internal set => _DB = value; }

        internal Schema(NpgsqlDataReader drd, DB db)
        {
            _DB = db;
            _Tables = new List<Table>();
            _Functions = new List<Function>();
            _Id = drd.Ref<string>("nspname")!;
        }
    }
}

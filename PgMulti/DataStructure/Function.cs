using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using PgMulti.SqlSyntax;
using System.Xml;

namespace PgMulti.DataStructure
{
    public class Function
    {
        private string _Id;
        private string _IdSchema;
        private string _Arguments;
        private string _Returns;
        private string _SourceCode;

        private Schema? _Schema;
        private List<Trigger> _Triggers;

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public string Arguments { get => _Arguments; internal set => _Arguments = value; }
        public string Returns { get => _Returns; internal set => _Returns = value; }
        public string SourceCode { get => _SourceCode; internal set => _SourceCode = value; }

        public Schema? Schema { get => _Schema; internal set => _Schema = value; }
        public List<Trigger> Triggers { get => _Triggers; }

        internal Function(NpgsqlDataReader drd)
        {
            _Triggers = new List<Trigger>();

            _Id = drd.Ref<string>("proname")!;
            _IdSchema = drd.Ref<string>("nspname")!;
            _Arguments = drd.Ref<string>("arguments")!.ToLower();
            _Returns = drd.Ref<string>("returns")!.ToLower();
            _SourceCode = drd.Ref<string>("source_code")!;
        }
    }
}

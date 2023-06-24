using Npgsql;
using System.Text.RegularExpressions;

namespace PgMulti.DataStructure
{
    public class TableRelation
    {
        private string _IdParentTable;
        private string _IdParentSchema;
        private string _IdChildTable;
        private string _IdChildSchema;
        private Table? _ParentTable;
        private Table? _ChildTable;
        private string _Id;
        private string _Definition;

        public string IdParentTable { get => _IdParentTable; internal set => _IdParentTable = value; }
        public string IdParentSchema { get => _IdParentSchema; internal set => _IdParentSchema = value; }
        public string IdChildTable { get => _IdChildTable; internal set => _IdChildTable = value; }
        public string IdChildSchema { get => _IdChildSchema; internal set => _IdChildSchema = value; }
        public Table? ParentTable { get => _ParentTable; internal set => _ParentTable = value; }
        public Table? ChildTable { get => _ChildTable; internal set => _ChildTable = value; }
        public string Id { get => _Id; internal set => _Id = value; }
        public string Definition { get => _Definition; internal set => _Definition = value; }

        public readonly string[] ParentColumns;
        public readonly string[] ChildColumns;

        public readonly string OnDelete;
        public readonly string OnUpdate;

        internal TableRelation(NpgsqlDataReader drd)
        {
            _IdParentTable = drd.Ref<string>("parent_table")!.ToLower();
            _IdParentSchema = drd.Ref<string>("parent_schema")!.ToLower();
            _IdChildTable = drd.Ref<string>("child_table")!.ToLower();
            _IdChildSchema = drd.Ref<string>("child_schema")!.ToLower();
            _Id = drd.Ref<string>("fk")!.ToLower();
            _Definition = drd.Ref<string>("def")!;

            Match m;

            m = Regex.Match(Definition, @"^FOREIGN KEY \(([^\)]+)\) REFERENCES [^\(]+\(([^\)]+)\)");
            ParentColumns = m.Groups[2].Value.Split(',');
            ChildColumns = m.Groups[1].Value.Split(',');

            m = Regex.Match(Definition, @"ON DELETE ((NO ACTION|RESTRICT|CASCADE|SET (NULL|DEFAULT))( \([^\)]+\))?)");
            if (m.Success)
            {
                OnDelete = m.Groups[1].Value;
            }
            else
            {
                OnDelete = "NO ACTION";
            }

            m = Regex.Match(Definition, @"ON UPDATE ((NO ACTION|RESTRICT|CASCADE|SET (NULL|DEFAULT))( \([^\)]+\))?)");
            if (m.Success)
            {
                OnUpdate = m.Groups[1].Value;
            }
            else
            {
                OnUpdate = "NO ACTION";
            }
        }
    }
}

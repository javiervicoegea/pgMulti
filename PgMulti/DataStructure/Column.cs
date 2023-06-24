using Npgsql;

namespace PgMulti.DataStructure
{
    public class Column
    {
        private string _Id;
        private string _IdTable;
        private string _IdSchema;
        private string _Type;
        private string? _TypeParams;
        private string? _DefaultValue;
        private bool _IsIdentity;
        private bool _PK;
        private bool _NotNull;
        private int _Position;
        private Table? _Table;

        private string[] BooleanTypes = { "bool", "boolean" };

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdTable { get => _IdTable; internal set => _IdTable = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public string Type { get => _Type; internal set => _Type = value; }
        public string? TypeParams { get => _TypeParams; internal set => _TypeParams = value; }
        public string? DefaultValue { get => _DefaultValue; internal set => _DefaultValue = value; }
        public bool IsIdentity{ get => _IsIdentity; internal set => _IsIdentity = value; }
        public bool PK { get => _PK; internal set => _PK = value; }
        public bool NotNull { get => _NotNull; internal set => _NotNull = value; }
        public int Position { get => _Position; internal set => _Position = value; }
        public Table? Table { get => _Table; internal set => _Table = value; }

        public bool IsBoolean { get => BooleanTypes.Contains(Type); }

        internal Column(NpgsqlDataReader drd)
        {
            _PK = false;
            _IdSchema = drd.Ref<string>("table_schema")!.ToLower();
            _IdTable = drd.Ref<string>("table_name")!.ToLower();
            _Id = drd.Ref<string>("column_name")!.ToLower();
            _DefaultValue = drd.Ref<string>("column_default");
            _IsIdentity = drd.Val<bool>("is_identity")!.Value;
            _NotNull = drd.Ref<string>("is_nullable")! == "NO";
            _Type = drd.Ref<string>("data_type")!;
            _Position = drd.Val<int>("ordinal_position")!.Value;

            _TypeParams = null;
            if (_Type == "numeric")
            {
                _TypeParams += $"({drd.Val<int>("numeric_precision")!},{drd.Val<int>("numeric_scale")!})";
            }

            int? cml = drd.Val<int>("character_maximum_length");
            if (cml.HasValue && cml.Value > 0)
            {
                _TypeParams += $"({cml.Value})";
            }
        }

        public string Info
        {
            get
            {
                return Type.ToLower() + (TypeParams == null ? "" : TypeParams) + " " + (NotNull ? "not null" : "null") + " " + DefaultValue;
            }
        }

        public string GetSqlLiteralValue(object v)
        {
            if (v == null || v == DBNull.Value) return "null";

            switch (Type)
            {
                case "bit":
                case "bit varying":
                case "varbit":
                case "bytea":
                    return "decode('" + v.ToString() + "','hex')::" + Type;
                case "decimal":
                case "real":
                case "float":
                case "smallint":
                case "integer":
                case "int":
                case "interval":
                case "double precision":
                case "bigint":
                case "numeric":
                case "money":
                    return v.ToString()!;
                case "boolean":
                case "bool":
                    return v.ToString()!.ToLower();
                case "character":
                case "character varying":
                case "char":
                case "varchar":
                case "text":
                    string s = v.ToString()!;
                    if (s.Contains('\'') || s.Contains('\n') || s.Contains('\r'))
                    {
                        s = s.Replace("\'", "\\'");
                        s = s.Replace("\n", "\\n");
                        s = s.Replace("\r", "\\r");

                        return "E'" + s + "'";
                    }
                    else
                    {
                        return "'" + s + "'";
                    }
                case "serial":
                case "bigserial":
                case "smallserial":
                    throw new NotSupportedException();
                case "datetime":
                case "date":
                case "time":
                case "time without time zone":
                case "time with time zone":
                case "timestamp":
                case "timestamp without time zone":
                case "timestamp with time zone":
                case "inet":
                case "cidr":
                case "json":
                case "tsquery":
                case "tsvector":
                case "xml":
                case "point":
                case "regconfig":
                case "regclass":
                case "regnamespace":
                    return "'" + v.ToString() + "'::" + Type;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

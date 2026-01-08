using PgMulti.DataStructure;
using System.Text;
using System.Xml;

namespace PgMulti.Diagrams
{
    public class DiagramColumn
    {
        private DiagramTable _DiagramTable;
        private string _ColumnName;
        private string _TypeName;
        private string? _TypeParams;
        private string? _DefaultValue;
        private bool _IsIdentity;
        private bool _PrimaryKey;
        private bool _ForeignKey;
        private bool _NotNull;
        private string _TypeInitials;

        public static string GetTypeInitials(string typeName, string? typeParams)
        {
            string ti;
            switch (typeName)
            {
                case "boolean":
                case "bool":
                    return "bool";
                case "serial":
                case "serial4":
                    return "ser";
                case "smallserial":
                case "serial2":
                    return "sser";
                case "bigserial":
                case "serial8":
                    return "bser";
                case "integer":
                case "int":
                case "int4":
                    return "int";
                case "smallint":
                case "int2":
                    return "sint";
                case "bigint":
                case "int8":
                    return "bint";
                case "float4":
                case "real":
                    return "real";
                case "float":
                case "float8":
                case "double precision":
                    return "dble";
                case "decimal":
                    return "dmal";
                case "money":
                    return "mney";
                case "numeric":
                    ti = "n";
                    if (typeParams != null)
                    {
                        ti += typeParams;
                    }
                    return ti;
                case "interval":
                    return "ival";
                case "char":
                case "character":
                    ti = "c";
                    if (typeParams != null)
                    {
                        ti += typeParams;
                    }
                    return ti;
                case "varchar":
                case "character varying":
                    ti = "cv";
                    if (typeParams != null)
                    {
                        ti += typeParams;
                    }
                    return ti;
                case "text":
                    return "text";
                case "time with time zone":
                    return "tmz";
                case "time":
                case "time without time zone":
                    return "tm";
                case "date":
                    return "date";
                case "datetime":
                    return "dtm";
                case "timestamp with time zone":
                    return "tsz";
                case "timestamp":
                case "timestamp without time zone":
                    return "tsmp";
                case "bytea":
                    return "btea";
                case "bit":
                    return "bit";
                case "varbit":
                case "bit varying":
                    return "bitv";
                case "inet":
                    return "inet";
                case "cidr":
                    return "cidr";
                case "json":
                    return "json";
                case "jsonb":
                    return "jsnb";
                case "xml":
                    return "xml";
                case "tsquery":
                    return "tsqy";
                case "tsvector":
                    return "tsvr";
                case "point":
                    return "pt";
                case "regconfig":
                    return "rcig";
                case "regclass":
                    return "rcss";
                case "regnamespace":
                    return "rns";
                default:
                    return (typeName.Length > 4 ? typeName.Substring(0, 4) : typeName) + (typeParams == null ? "" : typeParams);
            }
        }

        public DiagramColumn(DiagramTable dt, string columnName, string typeName, string? typeParams, string? defaultValue, bool isIdentity, bool primaryKey, bool notNull, string typeInitials)
        {
            if (dt == null || columnName == null || typeName == null || typeInitials == null) throw new ArgumentException();

            _DiagramTable = dt;

            _ColumnName = columnName;
            _TypeName = typeName;
            _TypeParams = typeParams;
            _DefaultValue = defaultValue;
            _IsIdentity = isIdentity;
            _PrimaryKey = primaryKey;
            _NotNull = notNull;
            _TypeInitials = typeInitials;
        }

        public DiagramColumn(DiagramTable dt, Column c)
        {
            if (dt == null || c == null) throw new ArgumentException();

            _DiagramTable = dt;

            _ColumnName = c.Id;
            _TypeName = c.Type;
            _TypeParams = c.TypeParams;
            _DefaultValue = c.DefaultValue;
            _IsIdentity = c.IsIdentity;
            _PrimaryKey = c.PK;
            _NotNull = c.NotNull;
            _TypeInitials = GetTypeInitials(c.Type, c.TypeParams);
        }

        public DiagramColumn(DiagramTable dt, XmlElement xeColumn)
        {
            if (dt == null || xeColumn == null) throw new ArgumentException();

            _DiagramTable = dt;

            string? v;
            bool b;

            v = xeColumn.GetAttribute("column_name");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            _ColumnName = v;

            v = xeColumn.GetAttribute("type_name");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            _TypeName = v;

            v = xeColumn.GetAttribute("type_params");
            if (string.IsNullOrEmpty(v)) v = null;

            _TypeParams = v;

            v = xeColumn.GetAttribute("default_value");
            if (string.IsNullOrEmpty(v)) v = null;

            _DefaultValue = v;

            v = xeColumn.GetAttribute("is_identity");
            if (!string.IsNullOrEmpty(v) && bool.TryParse(v, out b) && b)
            {
                _IsIdentity = true;
            }
            else
            {
                _IsIdentity = false;
            }

            v = xeColumn.GetAttribute("type_initials");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            _TypeInitials = v;

            v = xeColumn.GetAttribute("primary_key");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            if (!bool.TryParse(v, out _PrimaryKey)) throw new BadFormatException();

            v = xeColumn.GetAttribute("not_null");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            if (!bool.TryParse(v, out _NotNull)) throw new BadFormatException();
        }

        public string ColumnName { get => _ColumnName; set => _ColumnName = value; }
        public string TypeName { get => _TypeName; set => _TypeName = value; }
        public string? TypeParams { get => _TypeParams; set => _TypeParams = value; }
        public string? DefaultValue { get => _DefaultValue; set => _DefaultValue = value; }
        public bool IsIdentity { get => _IsIdentity; set => _IsIdentity = value; }
        public bool PrimaryKey { get => _PrimaryKey; set => _PrimaryKey = value; }
        public bool ForeignKey { get => _ForeignKey; internal set => _ForeignKey = value; }
        public bool NotNull { get => _NotNull; set => _NotNull = value; }
        public string TypeInitials { get => _TypeInitials; set => _TypeInitials = value; }

        public XmlElement ToXml(XmlDocument xd)
        {
            XmlElement xeColumn = xd.CreateElement("column");
            xeColumn.SetAttribute("column_name", ColumnName);
            xeColumn.SetAttribute("type_name", TypeName);
            if (TypeParams != null) xeColumn.SetAttribute("type_params", TypeParams);
            if (DefaultValue != null) xeColumn.SetAttribute("default_value", DefaultValue);
            xeColumn.SetAttribute("is_identity", IsIdentity.ToString());
            xeColumn.SetAttribute("type_initials", TypeInitials);
            xeColumn.SetAttribute("primary_key", PrimaryKey.ToString());
            xeColumn.SetAttribute("not_null", NotNull.ToString());

            return xeColumn;
        }

        public void WriteSqlClauseFullDefinition(StringBuilder sb)
        {
            if (sb == null) throw new ArgumentException();

            sb.Append($"{SqlSyntax.PostgreSqlGrammar.IdToString(ColumnName)} {TypeName}{(TypeParams == null ? "" : TypeParams)} {(NotNull ? "NOT NULL" : "NULL")}{(DefaultValue == null ? "" : $" DEFAULT {DefaultValue}")}{(IsIdentity ? " GENERATED ALWAYS AS IDENTITY" : "")}");
            if (PrimaryKey && _DiagramTable.Columns.Count(i => i.PrimaryKey) == 1)
            {
                sb.Append(" PRIMARY KEY");
            }
        }

        public void WriteSqlSentenceAlter(StringBuilder sb, Column c)
        {
            if (sb == null || c == null) throw new ArgumentException();

            if (TypeName != c.Type || TypeParams != c.TypeParams) WriteSqlClauseAlter(sb, $"TYPE {TypeName}{(TypeParams == null ? "" : TypeParams)}");
            if (DefaultValue != c.DefaultValue) WriteSqlClauseAlter(sb, DefaultValue == null ? "DROP DEFAULT" : $"SET DEFAULT {DefaultValue}");
            if (IsIdentity != c.IsIdentity) WriteSqlClauseAlter(sb, IsIdentity ? "ADD GENERATED ALWAYS AS IDENTITY" : "DROP IDENTITY");
            if (NotNull != c.NotNull) WriteSqlClauseAlter(sb, NotNull ? "SET NOT NULL" : "DROP NOT NULL");
        }

        public void WriteSqlClauseAlter(StringBuilder sb, string sql)
        {
            if (sb == null || sql == null) throw new ArgumentException();

            sb.AppendLine($"ALTER TABLE {SqlSyntax.PostgreSqlGrammar.IdToString(_DiagramTable.SchemaName)}.{SqlSyntax.PostgreSqlGrammar.IdToString(_DiagramTable.TableName)} ALTER COLUMN {SqlSyntax.PostgreSqlGrammar.IdToString(ColumnName)} {sql};");
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (base.Equals(obj)) return true;

            if (obj is Column)
            {
                Column other = (Column)obj;
                return other.Id == ColumnName && other.Type == TypeName && other.TypeParams == TypeParams;
            }
            else if (obj is DiagramColumn)
            {
                DiagramColumn other = (DiagramColumn)obj;
                return other.ColumnName == ColumnName && other.TypeName == TypeName && other.TypeParams == TypeParams;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ColumnName.GetHashCode();
        }

        public override string ToString()
        {
            return ColumnName;
        }
    }
}

using Npgsql;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PgMulti.DataStructure
{
    public class Column
    {
        public readonly bool IsBoolean = false;
        public readonly bool IsShort = false;
        public readonly bool IsInt = false;
        public readonly bool IsLong = false;
        public readonly bool IsFloat = false;
        public readonly bool IsDouble = false;
        public readonly bool IsDecimal = false;
        public readonly bool IsDate = false;
        public readonly bool IsDateTime = false;

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdTable { get => _IdTable; internal set => _IdTable = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public string Type { get => _Type; internal set => _Type = value; }
        public string? TypeParams { get => _TypeParams; internal set => _TypeParams = value; }
        public string? DefaultValue { get => _DefaultValue; internal set => _DefaultValue = value; }
        public bool IsIdentity { get => _IsIdentity; internal set => _IsIdentity = value; }
        public bool IsGeneratedAlways { get => _IsGeneratedAlways; internal set => _IsGeneratedAlways = value; }
        public bool PK { get => _PK; internal set => _PK = value; }
        public bool NotNull { get => _NotNull; internal set => _NotNull = value; }
        public int Position { get => _Position; internal set => _Position = value; }
        public Table? Table { get => _Table; internal set => _Table = value; }
        public int? Precission { get => _Precission; }
        public int? Scale { get => _Scale; }


        private string _Id;
        private string _IdTable;
        private string _IdSchema;
        private string _Type;
        private string? _TypeParams;
        private string? _DefaultValue;
        private bool _IsIdentity;
        private bool _IsGeneratedAlways;
        private bool _PK;
        private bool _NotNull;
        private int _Position;
        private Table? _Table;
        private int? _Precission = null;
        private int? _Scale = null;

        private static string[] BooleanTypes = { "bool", "boolean" };
        private static string[] ShortTypes = { "smallint", "int2", "serial2" };
        private static string[] IntTypes = { "int", "integer", "int4", "serial4", "serial" };
        private static string[] LongTypes = { "bigint", "int8", "serial8", "bigserial" };
        private static string[] FloatTypes = { "real", "float4" };
        private static string[] DoubleTypes = { "double precision", "float8" };
        private static string[] DecimalTypes = { "money", "numeric", "decimal" };
        private static string[] DateTypes = { "date" };
        private static string[] DateTimeTypes = { "datetime", "timestamp", "timestamp with time zone", "timestamp without time zone" };
        private static string[] NumericTypes = { "numeric", "decimal" };

        internal Column(NpgsqlDataReader drd)
        {
            _PK = false;
            _IdSchema = drd.Ref<string>("table_schema")!;
            _IdTable = drd.Ref<string>("table_name")!;
            _Id = drd.Ref<string>("column_name")!;
            _DefaultValue = drd.Ref<string>("column_default");
            _IsIdentity = drd.Val<bool>("is_identity")!.Value;
            _IsGeneratedAlways = drd.Val<bool>("is_generatedalways")!.Value;
            _NotNull = drd.Ref<string>("is_nullable")! == "NO";
            _Type = drd.Ref<string>("data_type")!;
            _Position = drd.Val<int>("ordinal_position")!.Value;

            _TypeParams = null;
            if (NumericTypes.Contains(_Type))
            {
                _Precission = drd.Val<int>("numeric_precision")!;
                _Scale = drd.Val<int>("numeric_scale")!;
                _TypeParams = $"({_Precission},{_Scale})";
            }
            else
            {
                int? cml = drd.Val<int>("character_maximum_length");
                if (cml.HasValue && cml.Value > 0)
                {
                    _TypeParams = $"({cml.Value})";
                }
            }


            if (BooleanTypes.Contains(_Type))
            {
                IsBoolean = true;
            }
            else if (ShortTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Scale!.Value == 0 && Precission!.Value <= 4))
            {
                IsShort = true;
            }
            else if (IntTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Scale!.Value == 0 && Precission!.Value > 4 && Precission!.Value <= 9))
            {
                IsInt = true;
            }
            else if (LongTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Scale!.Value == 0 && Precission!.Value > 9 && Precission!.Value <= 18))
            {
                IsLong = true;
            }
            else if (FloatTypes.Contains(_Type))
            {
                IsFloat = true;
            }
            else if (DoubleTypes.Contains(_Type))
            {
                IsDouble = true;
            }
            else if (DecimalTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && (Scale!.Value > 0 || Precission!.Value > 18)))
            {
                IsDecimal = true;
            }
            else if (DateTypes.Contains(_Type))
            {
                IsDate = true;
            }
            else if (DateTimeTypes.Contains(_Type))
            {
                IsDateTime = true;
            }
        }

        public static Type GetDotNetType(string pgType)
        {
            if (BooleanTypes.Contains(pgType))
            {
                return typeof(bool);
            }
            else if (ShortTypes.Contains(pgType))
            {
                return typeof(short);
            }
            else if (IntTypes.Contains(pgType))
            {
                return typeof(int);
            }
            else if (LongTypes.Contains(pgType))
            {
                return typeof(long);
            }
            else if (FloatTypes.Contains(pgType))
            {
                return typeof(float);
            }
            else if (DoubleTypes.Contains(pgType))
            {
                return typeof(double);
            }
            else if (DecimalTypes.Contains(pgType))
            {
                return typeof(decimal);
            }
            else
            {
                return typeof(string);
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

            if (IsBoolean)
            {
                return v.ToString()!.ToLower();
            }
            else if (IsShort)
            {
                return ((short)v).ToString(CultureInfo.InvariantCulture);
            }
            else if (IsInt)
            {
                return ((int)v).ToString(CultureInfo.InvariantCulture);
            }
            else if (IsLong)
            {
                return ((long)v).ToString(CultureInfo.InvariantCulture);
            }
            else if (IsFloat)
            {
                return ((float)v).ToString(CultureInfo.InvariantCulture);
            }
            else if (IsDouble)
            {
                return ((double)v).ToString(CultureInfo.InvariantCulture);
            }
            else if (Type == "money")
            {
                return ((decimal)v).ToString(CultureInfo.InvariantCulture) + "::money";
            }
            else if (IsDecimal)
            {
                return ((decimal)v).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                switch (Type)
                {
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
                    default:
                        return "'" + v.ToString() + "'::" + Type;
                }
            }

        }
    }
}

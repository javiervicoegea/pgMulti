using Npgsql;
using Npgsql.Schema;
using NpgsqlTypes;
using System;
using System.Collections;
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
        public int? Size { get => _Size; }


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
        private int? _Size = null;

        public static string[] BooleanTypes = { "bool", "boolean" };
        public static string[] ShortTypes = { "smallint", "int2", "serial2" };
        public static string[] IntTypes = { "int", "integer", "int4", "serial4", "serial" };
        public static string[] LongTypes = { "bigint", "int8", "serial8", "bigserial" };
        public static string[] FloatTypes = { "real", "float4" };
        public static string[] DoubleTypes = { "double precision", "float8" };
        public static string[] DecimalTypes = { "money", "numeric", "decimal" };
        public static string[] DateTypes = { "date" };
        public static string[] DateTimeTypes = { "datetime", "timestamp", "timestamp with time zone", "timestamp without time zone" };
        public static string[] NumericTypes = { "numeric", "decimal" };
        public static Type[] NumericDotNetTypes = new Type[] { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(Decimal) };

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

                if (_Precission.HasValue && _Precission.Value == 0)
                {
                    _Precission = null;
                    _Scale = null;
                }
                else if (_Precission.HasValue && !_Scale.HasValue)
                {
                    _Scale = 0;
                }
                else if (!_Precission.HasValue)
                {
                    _Scale = null;
                }

                if (_Precission.HasValue) _TypeParams = $"({_Precission},{_Scale})";
            }
            else
            {
                _Size = drd.Val<int>("character_maximum_length");
                if (_Size.HasValue && _Size.Value > 0)
                {
                    _TypeParams = $"({_Size.Value})";
                }
            }

            if (BooleanTypes.Contains(_Type))
            {
                IsBoolean = true;
            }
            else if (ShortTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value <= 4))
            {
                IsShort = true;
            }
            else if (IntTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value > 4 && Precission.Value <= 9))
            {
                IsInt = true;
            }
            else if (LongTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value > 9 && Precission.Value <= 18))
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
            else if (DecimalTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && (!Precission.HasValue || Scale!.Value > 0 || Precission.Value > 18)))
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

        internal Column(Table t, NpgsqlDbColumn c)
        {
            _Table = t;

            _PK = false;
            _IdSchema = t.Schema!.Id;
            _IdTable = t.Id;
            _Id = c.ColumnName;
            _DefaultValue = null;
            _IsIdentity = false;
            _IsGeneratedAlways = false;
            _NotNull = false;
            _Type = c.PostgresType.Name;
            _Position = c.ColumnOrdinal!.Value;

            _TypeParams = null;
            if (NumericTypes.Contains(_Type))
            {
                _Precission = c.NumericPrecision;
                _Scale = c.NumericScale;

                if (_Precission.HasValue && _Precission.Value == 0)
                {
                    _Precission = null;
                    _Scale = null;
                }
                else if (_Precission.HasValue && !_Scale.HasValue)
                {
                    _Scale = 0;
                }
                else if (!_Precission.HasValue)
                {
                    _Scale = null;
                }

                if (_Precission.HasValue) _TypeParams = $"({_Precission},{_Scale})";
            }
            else
            {
                _Size = c.ColumnSize;

                if (_Size.HasValue && _Size.Value > 0)
                {
                    _TypeParams = $"({_Size.Value})";
                }
            }

            if (BooleanTypes.Contains(_Type))
            {
                IsBoolean = true;
            }
            else if (ShortTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value <= 4))
            {
                IsShort = true;
            }
            else if (IntTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value > 4 && Precission.Value <= 9))
            {
                IsInt = true;
            }
            else if (LongTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && Precission.HasValue && Scale!.Value == 0 && Precission.Value > 9 && Precission.Value <= 18))
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
            else if (DecimalTypes.Contains(_Type) || (NumericTypes.Contains(_Type) && (!Precission.HasValue || Scale!.Value > 0 || Precission.Value > 18)))
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

        public static Type GetDataTableTypeMapping(Type? type)
        {
            if (type != null && NumericDotNetTypes.Contains(type))
            {
                return type;
            }
            else
            {
                return typeof(string);
            }
        }

        public static object ConvertValue(NpgsqlDataReader drd, int index, Type dotNetType, string postgreSqlTypeName, CultureInfo? monetaryCultureInfo)
        {
            object? sourceObject;

            if (drd[index] == DBNull.Value) return DBNull.Value;

            switch (postgreSqlTypeName)
            {
                case "inet":
                    sourceObject = drd.GetFieldValue<NpgsqlInet?>(index);
                    break;
                case "bit":
                    sourceObject = drd.GetFieldValue<BitArray?>(index);
                    break;
                default:
                    sourceObject = drd[index];
                    break;
            }

            if (sourceObject == null) return DBNull.Value;

            return ConvertValue(sourceObject, dotNetType, postgreSqlTypeName, monetaryCultureInfo);
        }

        public static object ConvertValue(object sourceObject, Type dotNetType, string npgsqlTypeName, CultureInfo? monetaryCultureInfo)
        {
            object destinationObject;

            if (sourceObject == DBNull.Value)
            {
                destinationObject = sourceObject;
            }
            else if (!Column.IsSupportedType(npgsqlTypeName))
            {
                return "?";
            }
            else if (sourceObject is string)
            {
                string s = (string)sourceObject;

                if (dotNetType == typeof(bool))
                {
                    if (s == "t")
                    {
                        destinationObject = true;
                    }
                    else if (s == "f")
                    {
                        destinationObject = false;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                else if (dotNetType == typeof(short))
                {
                    destinationObject = short.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (dotNetType == typeof(int))
                {
                    destinationObject = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (dotNetType == typeof(long))
                {
                    destinationObject = long.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (dotNetType == typeof(float))
                {
                    destinationObject = float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (dotNetType == typeof(double))
                {
                    destinationObject = double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (dotNetType == typeof(decimal))
                {
                    if (npgsqlTypeName == "money")
                    {
                        if (monetaryCultureInfo == null) throw new ArgumentException("Missing monetaryCultureInfo");
                        destinationObject = decimal.Parse(s, System.Globalization.NumberStyles.Currency, monetaryCultureInfo!);
                    }
                    else
                    {
                        destinationObject = decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    destinationObject = sourceObject;
                }
            }
            else if (sourceObject is NpgsqlInet && dotNetType == typeof(string))
            {
                NpgsqlInet ip = (NpgsqlInet)sourceObject;

                destinationObject = ip.Address.ToString() + "/" + ip.Netmask.ToString();
            }
            else if (dotNetType == typeof(string))
            {
                if (sourceObject is DateTime)
                {
                    DateTime dt = (DateTime)sourceObject;
                    if (dt.Date == dt)
                    {
                        destinationObject = dt.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        if (dt.TimeOfDay.Seconds == 0 && dt.TimeOfDay.Milliseconds == 0)
                        {
                            destinationObject = dt.ToString("yyyy-MM-dd HH:mm");
                        }
                        else
                        {
                            destinationObject = dt.ToString("yyyy-MM-dd HH:mm:ss.FFF");
                        }
                    }
                }
                else if (sourceObject is byte[])
                {
                    destinationObject = Convert.ToBase64String((byte[])sourceObject);
                }
                else if (sourceObject is BitArray)
                {
                    destinationObject = string.Join("", ((BitArray)sourceObject).Cast<bool>().Select(b => b ? "1" : "0"));
                }
                else if (sourceObject is bool)
                {
                    destinationObject = ((bool)sourceObject).ToString().ToLowerInvariant();
                }
                else
                {
                    destinationObject = sourceObject;
                }
            }
            else
            {
                destinationObject = sourceObject;
            }

            return destinationObject;
        }

        public static bool IsSupportedType(string? type)
        {
            // ToDo: A comprehensive enumeration of the types actually admitted is required
            return type != null && type != "ARRAY" && !type.Contains("[");
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
                    case "bit varying":
                    case "bit":
                    case "varbit":
                        return "B'" + (string)v + "'::" + Type + (TypeParams == null ? "" : TypeParams);
                    case "bytea":
                        return "decode('" + (string)v + "', 'base64')::" + Type;
                    default:
                        return "'" + v.ToString() + "'::" + Type;
                }
            }

        }

        public string GetSqlParameterExpression(string parameterName)
        {
            if (IsBoolean || IsShort || IsInt || IsLong || IsFloat || IsDouble || IsDecimal)
            {
                return ":" + parameterName;
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
                        return ":" + parameterName;
                    default:
                        return ":" + parameterName + "::" + Type;
                }
            }
        }
    }
}

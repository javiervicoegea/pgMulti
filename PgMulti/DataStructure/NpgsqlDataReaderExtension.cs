using Npgsql;
using System.Data;

namespace PgMulti.DataStructure
{
    public static class NpgsqlDataReaderExtension
    {
        public static T? Val<T>(this NpgsqlDataReader r, string n) where T : struct
        {
            var t = r.GetValue(n);
            if (t == DBNull.Value) return default(T);
            return (T)t;
        }

        public static T? Ref<T>(this NpgsqlDataReader r, string n) where T : class
        {
            var t = r.GetValue(n);
            if (t == DBNull.Value) return null;
            return (T)t;
        }

        /*
        public static T? Def<T>(this NpgsqlDataReader r, string n)
        {
            var t = r.GetValue(n);
            if (t == DBNull.Value) return default(T);
            return ((INullable)t).IsNull ? default(T) : (T)t;
        }

        public static T? Val<T>(this NpgsqlDataReader r, string n) where T : struct
        {
            var t = r.GetValue(n);
            if (t == DBNull.Value) return null;
            return ((INullable)t).IsNull ? (T?)null : (T)t;
        }

        public static T? Ref<T>(this NpgsqlDataReader r, string n) where T : class
        {
            var t = r.GetValue(n);
            if (t == DBNull.Value) return null;
            return ((INullable)t).IsNull ? null : (T)t;
        }
        */
    }
}

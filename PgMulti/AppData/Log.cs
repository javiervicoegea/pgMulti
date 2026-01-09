using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace PgMulti.AppData
{
    public class Log
    {
        public DateTime Timestamp;
        public string? SqlText;
        public readonly List<int> DBIds;

        private const int MaxList = 20;

        private Data _Data;
        private int _Id;

        public static List<Log> List(Data d, int? lastLogId, Filter f)
        {
            using (Connection c = d.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT h.id,h.timestamp,h.txt,group_concat(dh.dbid) dbids");
                sb.Append(" FROM logs h INNER JOIN dbs_logs dh ON dh.logid=h.id");

                if (lastLogId.HasValue)
                {
                    sb.Append(" AND h.id<:lastLogId");
                    cmd.Parameters.AddWithValue("lastLogId", lastLogId.Value);
                }

                List<string> sqlClauses = new List<string>();
                f.ApplyFilter(sqlClauses, cmd.Parameters);
                if (sqlClauses.Count > 0) sb.Append(" WHERE " + string.Join(" AND ", sqlClauses));

                sb.Append(" GROUP BY h.id,h.timestamp,h.txt");
                sb.Append(" ORDER BY h.id DESC");
                sb.Append(" LIMIT :max");

                cmd.Parameters.AddWithValue("max", MaxList);

                cmd.CommandText = sb.ToString();

                SqliteDataReader drd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(drd);

                List<Log> l = new List<Log>();
                foreach (DataRow dr in dt.Rows)
                {
                    l.Add(new Log(d, dr));
                }

                return l;
            }
        }

        public Log(Data d)
        {
            _Data = d;

            _Id = -1;
            Timestamp = DateTime.Now;
            SqlText = null;
            DBIds = new List<int>();
        }

        public Log(Data d, DataRow dr)
        {
            _Data = d;

            _Id = (int)dr.Field<long>("id");
            Timestamp = new DateTime(dr.Field<long>("timestamp"));
            SqlText = dr.Field<string>("txt")!;
            DBIds = new List<int>();
            foreach (string s in dr.Field<string>("dbids")!.Split(','))
            {
                DBIds.Add(int.Parse(s));
            }
        }

        public int Id
        {
            get
            {
                return _Id;
            }
        }

        public string DBsDescription
        {
            get
            {
                List<string> l = new List<string>();
                foreach (int id in DBIds)
                {
                    foreach (DB db in _Data.AllDBs)
                    {
                        if (db.Id == id)
                        {
                            l.Add(db.Alias);
                            break;
                        }
                    }
                }

                return string.Join(", ", l);
            }
        }

        public void Save()
        {
            if (_Id != -1) throw new NotSupportedException();
            if (SqlText == null) throw new NotSupportedException();

            using (Connection c = _Data.OpenConnection())
            using (Transaction t = c.Begin())
            {
                SqliteCommand cmd = c.CreateCommand();

                cmd.CommandText = "INSERT INTO logs (timestamp,txt) VALUES (:timestamp,:txt) RETURNING id";

                cmd.Parameters.AddWithValue("timestamp", Timestamp.Ticks);
                cmd.Parameters.AddWithValue("txt", SqlText);

                _Id = (int)(long)cmd.ExecuteScalar()!;

                foreach (int idDB in DBIds)
                {
                    cmd = c.CreateCommand();
                    cmd.CommandText = "INSERT INTO dbs_logs (logid,dbid) VALUES (:logid,:dbid)";

                    cmd.Parameters.AddWithValue("logid", Id);
                    cmd.Parameters.AddWithValue("dbid", idDB);

                    cmd.ExecuteNonQuery();
                }

                t.Commit();
            }
        }

        public static int DeleteOldest(Data d, int percentage, int minCount)
        {
            using (Connection c = d.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                cmd.CommandText = "SELECT COUNT(*) FROM logs";
                int n = (int)(long)cmd.ExecuteScalar()!;

                cmd.CommandText = "SELECT MIN(id) FROM logs";
                int minId = (int)(long)cmd.ExecuteScalar()!;
                minId = minId + Math.Max(minCount, (percentage * n) / 100);

                cmd.CommandText = "DELETE FROM logs WHERE id<:minId";

                cmd.Parameters.AddWithValue("id", minId);

                cmd.ExecuteNonQuery();

                cmd.CommandText = "VACUUM";
                cmd.ExecuteNonQuery();

                return n;
            }
        }

        public class Filter
        {
            public DB? DB = null;
            public DateTime? FromTimestamp = null;
            public DateTime? ToTimestamp = null;
            public string? Text = null;

            public void ApplyFilter(List<string> sqlClauses, SqliteParameterCollection parms)
            {
                if (DB != null)
                {
                    sqlClauses.Add("dh.dbid=:dbId");
                    parms.AddWithValue("dbId", DB.Id);
                }
                if (FromTimestamp.HasValue)
                {
                    sqlClauses.Add("h.timestamp>=:fromTimestamp");
                    parms.AddWithValue("fromTimestamp", FromTimestamp.Value.Ticks);
                }
                if (ToTimestamp.HasValue)
                {
                    sqlClauses.Add("h.timestamp<=:toTimestamp");
                    parms.AddWithValue("toTimestamp", ToTimestamp.Value.Date == ToTimestamp.Value ? ToTimestamp.Value.AddDays(1) : ToTimestamp.Value.Ticks);
                }
                if (!string.IsNullOrEmpty(Text))
                {
                    int i = 0;
                    foreach (string word in Text.Split(' ').Select(i => i.Trim()).Where(i => i != ""))
                    {
                        sqlClauses.Add("h.txt LIKE '%' || :word_" + i + " || '%'");
                        parms.AddWithValue("word_" + i, word);
                        i++;
                    }
                }
            }
        }
    }
}

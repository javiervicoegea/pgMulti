using PgMulti.DataAccess;
using System.Data;
using Microsoft.Data.Sqlite;
using Irony.Parsing;
using PgMulti.SqlSyntax;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using PgMulti.QueryEditor;
using System.Globalization;

namespace PgMulti.AppData
{
    public class Data
    {
        private const long MaxDBFileSize = 1024 * 1024 * 100; // 100 MB
        private const int PreserveClosedTabsCount = 30;

        private Group _RootGroup;
        private List<DB> _AllDBs;
        private PostgreSqlGrammar _PGGrammar;
        private PostgreSqlSimpleCommandsGrammar _PGSimpleCommandsGrammar;
        private ConnectionData _ConnectionData;
        private const string FileName = "pgMulti.sqlite";
        private static string? _Path = null;
        private Config? _Config = null;
        private string _Password;

        private LanguageData? _PGLanguageData = null;
        private LanguageData? _PGSimpleLanguageData = null;

        public Data(string password)
        {
            _AllDBs = new List<DB>();
            _Password = password;

            CultureInfo cu = AppLanguage.CurrentLanguage.CultureInfo;

            _PGGrammar = new PostgreSqlGrammar(cu);
            _PGSimpleCommandsGrammar = new PostgreSqlSimpleCommandsGrammar(cu);

            SqliteConnectionStringBuilder b = new SqliteConnectionStringBuilder()
            {
                DataSource = PathDB,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            };

            _ConnectionData = new ConnectionData(b.ToString());
            _RootGroup = ReloadStructure();
        }

        public static long DBFileSize
        {
            get
            {
                return new FileInfo(PathDB).Length;
            }
        }

        internal static bool ExistDB
        {
            get
            {
                return File.Exists(PathDB);
            }
        }

        public static void CreateDB(string password)
        {
            if (File.Exists(PathDB)) File.Delete(PathDB);

            SqliteConnectionStringBuilder b = new SqliteConnectionStringBuilder()
            {
                DataSource = PathDB,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            };

            using (SqliteConnection con = new SqliteConnection(b.ToString()))
            {
                con.Open();

                SqliteCommand cmd = con.CreateCommand();
                cmd.CommandText = Properties.Resources.CreateSqliteDb_sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static bool ValidateCurrentPassword(string password)
        {
            if (!File.Exists(PathDB)) return false;

            SqliteConnectionStringBuilder b = new SqliteConnectionStringBuilder()
            {
                DataSource = PathDB,
                Mode = SqliteOpenMode.ReadOnly,
                Password = password
            };

            try
            {
                using (SqliteConnection con = new SqliteConnection(b.ToString()))
                {
                    con.Open();
                }
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 26)
                {
                    return false;
                }
                throw new Exception("Error validating password", ex);
            }

            return true;
        }

        public static bool ValidateNewPassword(string pass, out string? msg)
        {
            if (pass == null)
            {
                msg = Properties.Text.warning_password_null;
                return false;
            }

            if (pass.Length < 6)
            {
                msg = Properties.Text.warning_password_too_short;
                return false;
            }

            msg = null;
            return true;
        }

        internal static string PathDB
        {
            get
            {
                if (_Path == null)
                {
                    _Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
                }
                return _Path;
            }
        }

        public Config Config
        {
            get
            {
                if (_Config == null)
                {
                    using (Connection c = OpenConnection())
                    {
                        SqliteCommand cmd = c.CreateCommand();
                        cmd.CommandText = "SELECT * FROM config";
                        DataTable table = new DataTable();
                        table.Load(cmd.ExecuteReader());
                        _Config = new Config(this, table.Rows[0]);
                    }
                }

                return _Config;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                using (Connection c = OpenConnection())
                {
                    c.ChangePassword(value);
                }

                SqliteConnectionStringBuilder b = new SqliteConnectionStringBuilder()
                {
                    DataSource = PathDB,
                    Mode = SqliteOpenMode.ReadWrite,
                    Password = value
                };
                _ConnectionData.ConnectionString = b.ToString();

                _Password = value;
            }
        }

        public PostgreSqlGrammar PGGrammar
        {
            get
            {
                return _PGGrammar;
            }
        }

        public PostgreSqlSimpleCommandsGrammar PGSimpleCommandsGrammar
        {
            get
            {
                return _PGSimpleCommandsGrammar;
            }
        }

        public LanguageData PGLanguageData
        {
            get
            {
                if (_PGLanguageData == null)
                {
                    _PGLanguageData = new LanguageData(_PGGrammar);
                }

                return _PGLanguageData;
            }
        }

        public LanguageData PGSimpleLanguageData
        {
            get
            {
                if (_PGSimpleLanguageData == null)
                {
                    _PGSimpleLanguageData = new LanguageData(_PGSimpleCommandsGrammar);
                }

                return _PGSimpleLanguageData;
            }
        }

        public Group RootGroup
        {
            get
            {
                return _RootGroup;
            }
        }

        public List<DB> AllDBs
        {
            get
            {
                return _AllDBs;
            }
        }

        public Group ReloadStructure()
        {
            _AllDBs = new List<DB>();

            using (Connection c = OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "SELECT g.* FROM groups g ORDER BY g.position";

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                Dictionary<int, Group> dict = new Dictionary<int, Group>();

                foreach (DataRow dr in dt.Rows)
                {
                    Group g = new Group(this, dr);
                    dict[g.Id] = g;
                }

                bool alreadyHasRoot = false;
                foreach (Group g in dict.Values)
                {
                    if (g.IdParentGroup.HasValue)
                    {
                        g.ParentGroup = dict[g.IdParentGroup.Value];
                        g.ParentGroup.ChildGroups.Add(g);
                    }
                    else
                    {
                        if (alreadyHasRoot) throw new NotSupportedException("Structure has more than one root group");
                        _RootGroup = g;
                        alreadyHasRoot = true;
                    }
                }


                cmd = c.CreateCommand();
                cmd.CommandText = "SELECT d.* FROM dbs d ORDER BY d.position";

                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                foreach (DataRow dr in dt.Rows)
                {
                    Group g = dict[(int)dr.Field<long>("groupid")];
                    DB db = new DB(this, dr, g);
                    g.DBs.Add(db);
                    _AllDBs.Add(db);
                }
            }

            return _RootGroup;
        }

        public void CheckAppDbFileSize()
        {
            while (DBFileSize > MaxDBFileSize)
            {
                int n = Log.DeleteOldest(this, 20, 100);
                if (n < 100) return;
            }
        }


        public List<EditorTab> ListOpenEditorTabs(MainForm mainForm)
        {
            using (Connection c = OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "SELECT * FROM editortabs WHERE closedAt IS NULL ORDER BY position";

                SqliteDataReader drd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(drd);

                List<EditorTab> l = new List<EditorTab>();
                foreach (DataRow dr in dt.Rows)
                {
                    EditorTab et = new EditorTab(this, dr, mainForm);
                    l.Add(et);
                }
                return l;
            }
        }
        public List<ClosedEditorTab> ListClosedEditorTabs()
        {
            using (Connection c = OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "SELECT * FROM editortabs WHERE closedAt IS NOT NULL ORDER BY closedAt DESC";

                SqliteDataReader drd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(drd);

                List<ClosedEditorTab> l = new List<ClosedEditorTab>();
                foreach (DataRow dr in dt.Rows)
                {
                    l.Add(new ClosedEditorTab(this, dr));
                }
                return l;
            }
        }

        public ClosedEditorTab? GetLastClosedEditorTab()
        {
            using (Connection c = OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "SELECT * FROM editortabs WHERE closedAt IS NOT NULL ORDER BY closedAt DESC LIMIT 1";

                SqliteDataReader drd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(drd);

                List<ClosedEditorTab> l = new List<ClosedEditorTab>();
                ClosedEditorTab? cet;
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dt.Rows[0];
                    cet = new ClosedEditorTab(this, dr);
                }
                else
                {
                    cet = null;
                }
                return cet;
            }
        }

        public void DeleteOldestTabs()
        {
            using (Connection c = OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "DELETE FROM editortabs WHERE closedAt IS NOT NULL AND id NOT IN (SELECT et2.id FROM editortabs et2 WHERE et2.closedAt IS NOT NULL ORDER BY et2.closedAt DESC LIMIT :preserveClosedTabsCount)";
                cmd.Parameters.AddWithValue("preserveClosedTabsCount", PreserveClosedTabsCount);

                cmd.ExecuteNonQuery();
            }
        }

        public Connection OpenConnection()
        {
            return new Connection(_ConnectionData);
        }

        public void ResetConfigCache()
        {
            _Config = null;
        }

        public static string? AutoEllipsis(string? s, int maxLength = 100)
        {
            if (s == null) return null;
            if (s.Length < maxLength) return s;
            return s.Substring(0, maxLength - 3) + "...";
        }
    }
}

using PgMulti.DataAccess;
using PgMulti.DataStructure;
using Npgsql;
using System.Data;
using Microsoft.Data.Sqlite;

namespace PgMulti.AppData
{
    public class DB
    {
        public int Id;
        public string Alias;
        public string Server;
        public ushort Port;
        public string DBName;
        public string User;
        public string Password;
        public int IdGroup;
        public short Position;

        private Data _Data;
        private List<Schema>? _Schemas = null;
        private List<Schema>? _SearchPathSchemas = null;
        private Group _Group;

        public DB(Data d, Group g)
        {
            _Data = d;

            Id = -1;
            Alias = Properties.Text.new_group;
            Server = "";
            Port = 5432;
            DBName = "";
            User = "";
            Password = "";
            IdGroup = g.Id;
            _Group = g;
            Position = -1;
        }
        public DB(Data d, DataRow dr, Group g)
        {
            _Data = d;

            Id = (int)dr.Field<long>("id");
            Alias = dr.Field<string>("alias")!;
            Server = dr.Field<string>("server")!;
            Port = (ushort)dr.Field<long>("port")!;
            DBName = dr.Field<string>("db")!;
            User = dr.Field<string>("user")!;
            Password = dr.Field<string>("pass")!;
            IdGroup = (int)dr.Field<long>("groupid");
            Position = (short)dr.Field<long>("position");
            _Group = g;
        }

        public string ConnectionString
        {
            get
            {
                return $"Host={Server};Port={Port};Database={DBName};Username={User};Password={Password};Application Name=pgMulti {Application.ProductVersion};Connection Idle Lifetime=60;Keepalive=30;Include Error Detail=true;Command Timeout=36000";
            }
        }

        public NpgsqlConnection Connection
        {
            get
            {
                return new NpgsqlConnection(ConnectionString);
            }
        }

        public Group Group
        {
            get
            {
                return _Group;
            }
        }

        public List<Schema> Schemas
        {
            get
            {
                if (_Schemas == null)
                {
                    StructureBuilder? eb = StructureBuilder.CreateStructureBuilder(_Data, this);
                    if (eb == null)
                    {
                        _Schemas = new List<Schema>();
                    }
                    else
                    {
                        //try
                        //{
                            eb.Build();
                            _Schemas = eb.Schemas;
                        //}
                        //catch (Exception)
                        //{
                        //    _Schemas = new List<Schema>();
                        //}
                    }
                }

                return _Schemas;
            }
        }

        public List<Schema> SearchPathSchemas
        {
            get
            {
                if (_SearchPathSchemas == null)
                {
                    List<Schema> list = new List<Schema>();

                    try
                    {
                        string searchPath;

                        using (NpgsqlConnection c = Connection)
                        {
                            c.Open();
                            NpgsqlCommand cmd = c.CreateCommand();
                            cmd.CommandText = "SHOW search_path";
                            searchPath = (string)cmd.ExecuteScalar()!;
                        }

                        foreach (string idSchema in searchPath.Split(','))
                        {
                            string idSchema2 = SqlSyntax.PostgreSqlGrammar.IdFromString(idSchema);

                            foreach (Schema s in Schemas)
                            {
                                if (s.Id == idSchema2)
                                {
                                    list.Add(s);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception) { }

                    _SearchPathSchemas = list;
                }

                return _SearchPathSchemas;
            }
        }

        public void Save()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                if (Id == -1)
                {
                    cmd.CommandText = "INSERT INTO dbs (alias,server,port,db,user,pass,position,groupid) VALUES (:alias,:server,:port,:db,:user,:pass,:position,:idgroup) RETURNING id";
                    cmd.Parameters.AddWithValue("alias", Alias);
                    cmd.Parameters.AddWithValue("server", Server);
                    cmd.Parameters.AddWithValue("port", (int)Port);
                    cmd.Parameters.AddWithValue("db", DBName);
                    cmd.Parameters.AddWithValue("user", User);
                    cmd.Parameters.AddWithValue("pass", Password);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("idgroup", IdGroup);

                    Id = (int)(long)cmd.ExecuteScalar()!;
                }
                else
                {
                    cmd.CommandText = "UPDATE dbs SET alias=:alias,server=:server,port=:port,db=:db,user=:user,pass=:pass,position=:position,groupid=:idgroup WHERE id=:id";
                    cmd.Parameters.AddWithValue("alias", Alias);
                    cmd.Parameters.AddWithValue("server", Server);
                    cmd.Parameters.AddWithValue("port", (int)Port);
                    cmd.Parameters.AddWithValue("db", DBName);
                    cmd.Parameters.AddWithValue("user", User);
                    cmd.Parameters.AddWithValue("pass", Password);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.AddWithValue("idgroup", IdGroup);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MoveTo(Group targetGroup, int targetPosition)
        {
            using (Connection c = _Data.OpenConnection())
            using (Transaction t = c.Begin())
            {
                SqliteCommand cmd;

                // I close the gap in the original position

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE groups SET position=position-1 WHERE parentgroupid=:idgroup AND position>:position";
                cmd.Parameters.AddWithValue("idgroup", IdGroup);
                cmd.Parameters.AddWithValue("position", Position);
                cmd.ExecuteNonQuery();

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE dbs SET position=position-1 WHERE groupid=:idgroup AND position>:position";
                cmd.Parameters.AddWithValue("idgroup", IdGroup);
                cmd.Parameters.AddWithValue("position", Position);
                cmd.ExecuteNonQuery();

                // Then I open a new gap in the target position

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE groups SET position=position+1 WHERE parentgroupid=:idgroup AND position>=:position";
                cmd.Parameters.AddWithValue("idgroup", targetGroup.Id);
                cmd.Parameters.AddWithValue("position", targetPosition);
                cmd.ExecuteNonQuery();

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE dbs SET position=position+1 WHERE groupid=:idgroup AND position>=:position";
                cmd.Parameters.AddWithValue("idgroup", targetGroup.Id);
                cmd.Parameters.AddWithValue("position", targetPosition);
                cmd.ExecuteNonQuery();

                // Finally, I move the group

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE dbs SET groupid=:targetgroupid,position=:targetposition WHERE id=:id";
                cmd.Parameters.AddWithValue("targetgroupid", targetGroup == null ? DBNull.Value : targetGroup.Id);
                cmd.Parameters.AddWithValue("targetposition", targetPosition);
                cmd.Parameters.AddWithValue("id", Id);
                cmd.ExecuteNonQuery();

                t.Commit();
            }
        }

        public void Delete()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE dbs SET position=position-1 WHERE position>:position AND groupid=:idGrupo;DELETE FROM dbs WHERE id=:id";
                cmd.Parameters.AddWithValue("position", Position);
                cmd.Parameters.AddWithValue("idGrupo", IdGroup);
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public bool Test(out string? msg)
        {
            NpgsqlConnection c = Connection;
            try
            {
                c.Open();

                NpgsqlCommand cmd = c.CreateCommand();
                cmd.CommandText = "SELECT 1";
                if ((int)cmd.ExecuteScalar()! != 1) throw new NotSupportedException("Error ejecutando comando");

                c.Close();
            }
            catch (Exception nex)
            {
                msg = nex.Message;
                return false;
            }
            finally
            {
                c.Close();
            }

            msg = null;
            return true;
        }

        public void ResetSchemas()
        {
            _Schemas = null;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is DB && ((DB)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}

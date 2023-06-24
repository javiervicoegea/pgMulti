using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;

namespace PgMulti.AppData
{
    public class Group
    {
        public int Id;
        public int? IdParentGroup;
        public string Name;
        public short Position;

        private Data _Data;
        private Group? _ParentGroup = null;
        private List<Group> _ChildGroups;
        private List<DB> _DBs;

        public Group(Data d, Group? parentGroup)
        {
            _Data = d;
            Id = -1;
            Name = Properties.Text.new_group;
            Position = -1;
            _ChildGroups = new List<Group>();
            _DBs = new List<DB>();
            IdParentGroup = (parentGroup == null ? null : parentGroup.Id);
            _ParentGroup = parentGroup;
        }

        public Group(Data d, DataRow dr)
        {
            _Data = d;

            Id = (int)dr.Field<long>("id");
            IdParentGroup = (int?)dr.Field<long?>("parentgroupid");
            Name = dr.Field<string>("name")!;
            Position = (short)dr.Field<long>("position")!;
            _ChildGroups = new List<Group>();
            _DBs = new List<DB>();
        }

        public List<DB> DBs
        {
            get
            {
                return _DBs;
            }
        }

        public List<Group> ChildGroups
        {
            get
            {
                return _ChildGroups;
            }
        }

        public Group? ParentGroup
        {
            get
            {
                return _ParentGroup;
            }

            internal set
            {
                _ParentGroup = value;
            }
        }

        public int Count
        {
            get
            {
                return DBs.Count + ChildGroups.Count;
            }
        }

        public bool ContainedIn(Group other)
        {
            Group? g = this;
            while (g != null)
            {
                if (g == other) return true;

                g = g.ParentGroup;
            }

            return false;
        }

        public void Save()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                if (Id == -1)
                {
                    cmd.CommandText = "INSERT INTO groups (name,position,parentgroupid) VALUES (:name,:position,:idparentgroup) RETURNING id";
                    cmd.Parameters.AddWithValue("name", Name);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("idparentgroup", IdParentGroup.HasValue ? IdParentGroup.Value : DBNull.Value);

                    Id = (int)(long)cmd.ExecuteScalar()!;
                }
                else
                {
                    cmd.CommandText = "UPDATE groups SET name=:name,position=:position,parentgroupid=:idparentgroup WHERE id=:id";
                    cmd.Parameters.AddWithValue("name", Name);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("id", Id);
                    cmd.Parameters.AddWithValue("idparentgroup", IdParentGroup.HasValue ? IdParentGroup.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void MoveTo(Group? targetGroup, int targetPosition)
        {
            using (Connection c = _Data.OpenConnection())
            using (Transaction t = c.Begin())
            {
                SqliteCommand cmd = c.CreateCommand();

                // I close the gap in the original position

                if (IdParentGroup.HasValue)
                {
                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE groups SET position=position-1 WHERE parentgroupid=:idparentgroup AND position>:position";
                    cmd.Parameters.AddWithValue("idparentgroup", IdParentGroup.Value);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.ExecuteNonQuery();

                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE dbs SET position=position-1 WHERE groupid=:idparentgroup AND position>:position";
                    cmd.Parameters.AddWithValue("idparentgroup", IdParentGroup.Value);
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE groups SET position=position-1 WHERE parentgroupid IS NULL AND position>:position";
                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.ExecuteNonQuery();
                }

                // Then I open a new gap in the target position

                if (targetGroup != null)
                {
                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE groups SET position=position+1 WHERE parentgroupid=:idparentgroup AND position>=:position";
                    cmd.Parameters.AddWithValue("idparentgroup", targetGroup.Id);
                    cmd.Parameters.AddWithValue("position", targetPosition);
                    cmd.ExecuteNonQuery();

                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE dbs SET position=position+1 WHERE groupid=:idparentgroup AND position>=:position";
                    cmd.Parameters.AddWithValue("idparentgroup", targetGroup.Id);
                    cmd.Parameters.AddWithValue("position", targetPosition);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = c.CreateCommand();
                    cmd.CommandText = "UPDATE groups SET position=position+1 WHERE parentgroupid IS NULL AND position>=:position";
                    cmd.Parameters.AddWithValue("position", targetPosition);
                    cmd.ExecuteNonQuery();
                }

                // Finally, I move the group

                cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE groups SET parentgroupid=:targetgroupid,position=:targetposition WHERE id=:id";
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
                cmd.CommandText = "UPDATE groups SET position=position-1 WHERE position>:position;DELETE FROM groups WHERE id=:id";
                cmd.Parameters.AddWithValue("position", Position);
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

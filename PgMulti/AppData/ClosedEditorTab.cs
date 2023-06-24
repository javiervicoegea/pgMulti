using FastColoredTextBoxNS;
using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using PgMulti.Tasks;
using PgMulti.AppData;

namespace PgMulti.QueryEditor
{
    public class ClosedEditorTab
    {
        private int _Id;
        private string _Text;
        private string _Name;
        private string? _Path;
        private DateTime _ClosedAt;

        private Data _Data;

        public ClosedEditorTab(Data d, DataRow dr)
        {
            _Data = d;

            _Id = (int)dr.Field<long>("id");
            _Text = dr.Field<string>("text")!;
            _Name = dr.Field<string>("name")!;
            _Path = dr.Field<string?>("path");
            _ClosedAt = new DateTime(dr.Field<long>("closedAt"));
        }

        public int Id { get => _Id; }
        public string Text { get => _Text; }
        public string Name { get => _Name; }
        public string? Path { get => _Path; }
        public DateTime ClosedAt { get => _ClosedAt; }

        public void Reopen(int position)
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE editortabs SET closedAt=NULL,position=:position WHERE id=:id";
                cmd.Parameters.AddWithValue("position", position);
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

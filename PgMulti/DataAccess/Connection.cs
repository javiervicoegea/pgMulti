using System.Data;
using Microsoft.Data.Sqlite;

namespace PgMulti.DataAccess
{
    public class Connection : IDisposable
    {
        private ConnectionData _ConnectionData;

        public Connection(ConnectionData dc)
        {
            _ConnectionData = dc;
            _ConnectionData.ConnectionNestingLevel++;

            if (_ConnectionData.ConnectionNestingLevel == 0)
            {
                if (_ConnectionData.TransactionNestingLevel != -1 || _ConnectionData.Transaction != null) throw new Exception("There is no current transaction");
                if (_ConnectionData.Connection != null) throw new Exception("Another connection is already opened");
                _ConnectionData.Connection = new SqliteConnection(_ConnectionData.ConnectionString);
                _ConnectionData.Connection.Open();
            }
        }

        public SqliteCommand CreateCommand()
        {
            SqliteCommand cmd = _ConnectionData.Connection!.CreateCommand();
            cmd.Transaction = _ConnectionData.Transaction;

            return cmd;
        }

        public void ChangePassword(string newPassword)
        {
            SqliteCommand command = CreateCommand();
            command.CommandText = "SELECT quote($newPassword);";
            command.Parameters.AddWithValue("$newPassword", newPassword);
            string quotedNewPassword = (string)command.ExecuteScalar()!;

            command.CommandText = "PRAGMA rekey = " + quotedNewPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();
        }

        public Transaction Begin(IsolationLevel l = IsolationLevel.ReadCommitted)
        {
            return new Transaction(_ConnectionData, l);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ConnectionData.ConnectionNestingLevel < 0 || _ConnectionData.Connection == null) throw new Exception("There is no opened connection");

                _ConnectionData.ConnectionNestingLevel--;

                if (_ConnectionData.ConnectionNestingLevel == -1)
                {
                    if (_ConnectionData.TransactionNestingLevel != -1) throw new Exception("Another transaction is already in process");
                    _ConnectionData.Connection.Close();
                    _ConnectionData.Connection.Dispose();
                    _ConnectionData.Connection = null;
                }
            }
        }
    }
}

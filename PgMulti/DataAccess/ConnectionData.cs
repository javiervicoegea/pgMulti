using System.Data;
using Microsoft.Data.Sqlite;

namespace PgMulti.DataAccess
{
    public class ConnectionData
    {
        internal string ConnectionString;
        internal SqliteConnection? Connection;
        internal SqliteTransaction? Transaction;
        internal int ConnectionNestingLevel;
        internal int TransactionNestingLevel;
        internal IsolationLevel? IsolationLevel;

        public ConnectionData(string cs)
        {
            ConnectionString = cs;
            Connection = null;
            Transaction = null;
            ConnectionNestingLevel = -1;
            TransactionNestingLevel = -1;
            IsolationLevel = null;
        }
    }
}

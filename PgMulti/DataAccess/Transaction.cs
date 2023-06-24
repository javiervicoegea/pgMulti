using System.Data;

namespace PgMulti.DataAccess
{
    public class Transaction : IDisposable
    {
        private ConnectionData _ConnectionData;

        public Transaction(ConnectionData dc, IsolationLevel l)
        {
            _ConnectionData = dc;

            if (_ConnectionData.ConnectionNestingLevel == -1 || _ConnectionData.Connection == null) throw new Exception("There is no opened connection");
            _ConnectionData.TransactionNestingLevel++;

            if (_ConnectionData.TransactionNestingLevel == 0)
            {
                if (_ConnectionData.Transaction != null) throw new Exception("Another transaction is already in process");
                _ConnectionData.Transaction = _ConnectionData.Connection.BeginTransaction(l);
                _ConnectionData.IsolationLevel = l;
            }
            else
            {
                if (_ConnectionData.IsolationLevel != l) throw new Exception("Another transaction is already in process with distinct IsolationLevel");
            }
        }

        public void Commit()
        {
            if (_ConnectionData.TransactionNestingLevel < 0 || _ConnectionData.Transaction == null) throw new Exception("There is no current transaction");
            if (_ConnectionData.TransactionNestingLevel == 0)
            {
                _ConnectionData.Transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_ConnectionData.TransactionNestingLevel < 0 || _ConnectionData.Transaction == null) throw new Exception("There is no current transaction");
            if (_ConnectionData.TransactionNestingLevel == 0)
            {
                _ConnectionData.Transaction.Rollback();
            }
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
                if (_ConnectionData.TransactionNestingLevel < 0 || _ConnectionData.Transaction == null) throw new Exception("There is no current transaction");
                _ConnectionData.TransactionNestingLevel--;

                if (_ConnectionData.TransactionNestingLevel == -1)
                {
                    if (_ConnectionData.ConnectionNestingLevel < 0) throw new Exception("There is no opened connection");
                    _ConnectionData.Transaction.Dispose();
                    _ConnectionData.Transaction = null;
                }
            }
        }
    }
}

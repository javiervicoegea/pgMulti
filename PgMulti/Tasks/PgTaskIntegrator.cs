using PgMulti.AppData;
using static PgMulti.Tasks.QueryIntegrator;

namespace PgMulti.Tasks
{
    public class PgTaskIntegrator : PgTask
    {
        private List<PgTaskExecutorSqlTables> _ExecutorTasks;
        private int _AvailableStatementCount;
        private bool _Symmetric;
        private Thread? _Thread = null;
        private Mutex _Mutex;
        private string? _CoordinatedTransactionId = null;

        public PgTaskIntegrator(Data d, OnUpdate onUpdate, string sql, bool symmetric)
            : base(d, onUpdate, sql)
        {
            _ExecutorTasks = new List<PgTaskExecutorSqlTables>();
            _StatementCount = -1;
            _CurrentStatementIndex = -1;
            _AvailableStatementCount = -1;
            _Symmetric = symmetric;
            _Mutex = new Mutex(false);
        }

        internal string CoordinatedTransactionId
        {
            get
            {
                if (_CoordinatedTransactionId == null)
                {
                    _CoordinatedTransactionId = $"{Application.ProductName}_{DateTime.Now:HHmmss}";
                }

                return _CoordinatedTransactionId;
            }
        }

        internal bool AreAllTransactionsPrepared
        {
            get
            {
                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    if (!tes.PreparedCommit) return false;
                }

                return true;
            }
        }

        internal void Integrate(PgTaskExecutorSqlTables tes)
        {
            _ExecutorTasks.Add(tes);
        }

        internal void OnTesStatementCountReady(PgTaskExecutorSqlTables tes)
        {
            _Mutex.WaitOne();
            try
            {
                if (!_Symmetric)
                {
                    _StatementCount = Math.Max(_StatementCount, tes.StatementCount);
                }
                else if (_StatementCount == -1)
                {
                    _StatementCount = tes.StatementCount;
                }
                else if (_StatementCount != tes.StatementCount)
                {
                    throw new Exception();
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        internal void OnTesStatementCompleted()
        {
            _Mutex.WaitOne();
            try
            {
                int newCurrentStatementIndex = int.MaxValue;
                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    newCurrentStatementIndex = Math.Min(newCurrentStatementIndex, tes.CurrentStatementIndex);
                }

                if (newCurrentStatementIndex != _AvailableStatementCount)
                {
                    if (newCurrentStatementIndex > 0) StringBuilderAppendIndentedLine(string.Format(Properties.Text.completed_statement_n_in_all_tasks, newCurrentStatementIndex), true, LogStyle.Query);


                    if (_Symmetric)
                    {
                        _AvailableStatementCount = newCurrentStatementIndex;

                        if (_Thread == null)
                        {
                            _Thread = new Thread(new ThreadStart(Run));
                            _Thread.Start();
                        }
                    }
                    else
                    {
                        _AvailableStatementCount = newCurrentStatementIndex;
                        _CurrentStatementIndex = _AvailableStatementCount;
                    }
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        internal void OnTesWaitingCoordinatedCommit(PgTaskExecutorSqlTables tes)
        {
            StringBuilderAppendIndentedLine(string.Format(Properties.Text.waiting_coordinated_commit, tes.DB.Alias), true);
        }

        internal void OnTesStateChanged()
        {
            _Mutex.WaitOne();
            try
            {
                StateEnum? newState = null;
                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    if (!newState.HasValue || newState.Value > tes.State)
                    {
                        newState = tes.State;
                    }
                }

                if (newState.HasValue && newState.Value != State)
                {
                    _State = newState.Value;

                    if (_State == StateEnum.Finished)
                    {
                        _TotalDuration = DateTime.Now.Subtract(_StartTimestamp!.Value);
                        if (_Exception == null)
                        {
                            StringBuilderAppendIndentedLine(Properties.Text.all_tasks_finished, true, LogStyle.TaskSuccessfullyCompleted);
                        }
                        else
                        {
                            StringBuilderAppendIndentedLine(Properties.Text.tasks_finished_with_error, true, LogStyle.TaskFailed);
                        }
                        StringBuilderAppendSummaryLine($"\r\n{Properties.Text.total_duration}: {EllapsedTimeDescription(_TotalDuration!.Value, true)}", _Exception == null ? LogStyle.TaskSuccessfullyCompleted : LogStyle.TaskFailed);
                    }

                    _OnUpdate(this);
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        internal void OnTesException(PgTaskExecutorSqlTables tes)
        {
            StringBuilderAppendIndentedLine($"{string.Format(Properties.Text.error_in_task, tes.DB.Alias)}:\r\n" + tes.Exception!.Message.Replace("\r\n\r\n", "\r\n"), true, LogStyle.Error);
            _Exception = tes.Exception;
        }

        public override void Start()
        {
            _Mutex.WaitOne();
            try
            {
                _StartTimestamp = DateTime.Now;

                StringBuilderAppendIndentedLine($"{Properties.Text.starting_tasks}:", true);

                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    StringBuilderAppendIndentedLine("- " + tes.DB.Alias, false);
                }

                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    tes.Start();
                }

                base.Start();
            }
            finally { _Mutex.ReleaseMutex(); }
        }


        public override void Cancel()
        {
            if (_Canceled) return;

            base.Cancel();

            StringBuilderAppendIndentedLine(Properties.Text.canceling_all_tasks, true, LogStyle.Error);
            foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
            {
                tes.Cancel();
            }
        }

        protected override void Run()
        {
            bool exit = false;
            while (!exit)
            {
                QueryIntegrator? qi = null;
                foreach (PgTaskExecutorSqlTables tes in _ExecutorTasks)
                {
                    QueryExecutorSql? ces = (QueryExecutorSql?)tes.Queries.FirstOrDefault(cii => cii.Index == _CurrentStatementIndex);
                    if (ces == null) break;

                    if (qi == null)
                    {
                        qi = new QueryIntegrator(_Data, _CurrentStatementIndex, ces.Sql);
                    }

                    try
                    {
                        qi.Integrate(_Data, ces);
                    }
                    catch (IncompatibleQueryException iqex)
                    {
                        _Exception = iqex;
                        StringBuilderAppendIndentedLine(string.Format(Properties.Text.incompatible_query, _CurrentStatementIndex + 1), true, LogStyle.Error);
                        qi = null;
                        break;
                    }
                }

                if (qi != null)
                {
                    _Queries.Add(qi);
                    StringBuilderAppendIndentedLine($"{string.Format(Properties.Text.total_rows_integrated_in_statement_n, _CurrentStatementIndex + 1)}: " + qi.DataTable.Rows.Count + (qi.MaxRowsReached ? " " + string.Format(Properties.Text.rows_limit_warning, _Data.Config.MaxRows) : ""), true);
                }

                _Mutex.WaitOne();
                try
                {
                    _CurrentStatementIndex++;
                    if (_CurrentStatementIndex >= _AvailableStatementCount)
                    {
                        exit = true;
                        _Thread = null;
                    }
                }
                finally { _Mutex.ReleaseMutex(); }
            }
        }

        public override string ToString()
        {
            return _CreationTimestamp.ToShortTimeString() + $" - {Properties.Text.task_integrator}: " + StateDescription;
        }
    }
}

using Irony.Parsing;
using Microsoft.VisualBasic;
using Npgsql;
using PgMulti.AppData;
using System.Data;
using System.Diagnostics;

namespace PgMulti.Tasks
{
    public class PgTaskExecutorSqlTables : PgTaskExecutorSql
    {
        private DB _DB;
        private PgTaskIntegrator? _TaskIntegrator;
        private bool _PreparedCommit = false;

        public PgTaskExecutorSqlTables(
            Data d, DB db, OnUpdate onUpdate, string sql,
            Config.TransactionModeEnum transactionMode, Config.TransactionLevelEnum transactionLevel,
            LanguageData sld, PgTaskIntegrator? ti
        ) : base(d, onUpdate, sql, transactionMode, transactionLevel, sld)
        {
            _DB = db;
            _TaskIntegrator = ti;
        }


        public DB DB
        {
            get
            {
                return _DB;
            }
        }

        internal bool PreparedCommit
        {
            get
            {
                return _PreparedCommit;
            }
        }

        public override void Start()
        {
            base.Start();
            if (_TaskIntegrator != null) _TaskIntegrator.OnTesStateChanged();
        }

        public override void Cancel()
        {
            if (_Cancel) return;

            base.Cancel();

            if (_TaskIntegrator != null && _TransactionMode == Config.TransactionModeEnum.AutoCoordinated) _TaskIntegrator.Cancel();
        }

        protected override void Run()
        {
            try
            {
                List<Tuple<int, string, string>> statements;
                try
                {
                    statements = ListStatements();
                    _StatementCount = statements.Count;
                }
                catch (Exception ex)
                {
                    StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: " + ex.Message, true);
                    throw new AlreadyLoggedException(ex.Message, ex);
                }

                if (_TaskIntegrator != null) _TaskIntegrator.OnTesStatementCountReady(this);

                if (_TransactionMode != Config.TransactionModeEnum.Manual)
                {
                    foreach (Tuple<int, string, string> stmt in statements)
                    {
                        if (stmt.Item3 == "BEGIN" || stmt.Item3 == "COMMIT" || stmt.Item3 == "ROLLBACK")
                        {
                            StringBuilderAppendIndentedLine(Properties.Text.error_no_manual_transaction_allowed_in_auto_transactions, true);
                            throw new AlreadyLoggedException(Properties.Text.error_no_manual_transaction_allowed_in_auto_transactions);
                        }
                    }
                }

                NpgsqlConnection c = _DB.Connection;
                c.Notice += Connection_Notice;

                using (c)
                {
                    _Notices = new List<PostgresNotice>();
                    _NpgsqlCommand = c.CreateCommand();

                    c.Open();
                    StringBuilderAppendIndentedLine(Properties.Text.connection_opened, true);

                    if (_TransactionMode != Config.TransactionModeEnum.Manual)
                    {
                        string nivel;
                        switch (_TransactionLevel)
                        {
                            case Config.TransactionLevelEnum.ReadCommited:
                                nivel = "READ COMMITTED";
                                break;
                            case Config.TransactionLevelEnum.RepeatableRead:
                                nivel = "REPEATABLE READ";
                                break;
                            case Config.TransactionLevelEnum.Serializable:
                                nivel = "SERIALIZABLE";
                                break;
                            default:
                                throw new NotSupportedException();
                        }


                        _NpgsqlCommand.CommandText = "BEGIN ISOLATION LEVEL " + nivel;
                        _NpgsqlCommand.ExecuteNonQuery();
                        StringBuilderAppendIndentedLine(string.Format(Properties.Text.automatic_begin_transaction, nivel), true);
                    }

                    try
                    {
                        for (_CurrentStatementIndex = 0; _CurrentStatementIndex < statements.Count; _CurrentStatementIndex++)
                        {
                            if (_TaskIntegrator != null) _TaskIntegrator.OnTesStatementCompleted();

                            Tuple<int, string, string> stmt = statements[_CurrentStatementIndex];
                            int linea = stmt.Item1;
                            string sql = stmt.Item2;

                            _StringBuilder.AppendLine();
                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.query_counter, _CurrentStatementIndex + 1, StatementCount, linea + 1) + $":\r\n{sql}", true);
                            _StringBuilder.AppendLine();

                            _NpgsqlCommand.CommandText = sql;

                            int affectedRows;
                            DataTable? dt = null;
                            DateTime start = DateTime.Now;

                            _NpgsqlCommand.AllResultTypesAreUnknown = true;

                            using (NpgsqlDataReader drd = _NpgsqlCommand.ExecuteReader())
                            {
                                affectedRows = drd.RecordsAffected;
                                if (affectedRows == -1)
                                {
                                    QueryExecutorSql ces = new QueryExecutorSql(_Data, this, _CurrentStatementIndex, sql);
                                    ces.Load(drd);
                                    dt = ces.DataTable;

                                    if (dt.Columns.Count > 0)
                                    {
                                        StringBuilderAppendIndentedLine($"{Properties.Text.total_rows}: " + dt.Rows.Count + (ces.MaxRowsReached ? " " + string.Format(Properties.Text.rows_limit_warning, _Data.Config.MaxRows) : ""), false);

                                        _Queries.Add(ces);
                                    }
                                }

                                if (affectedRows != -1)
                                {
                                    StringBuilderAppendIndentedLine(string.Format(Properties.Text.n_affected_rows, affectedRows), false);
                                }
                            }


                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.completed_in, EllapsedTimeDescription(DateTime.Now.Subtract(start), true)), false);


                            foreach (PostgresNotice n in _Notices)
                            {
                                StringBuilderAppendIndentedLine(n.Severity + ": " + n.MessageText + " - " + n.Detail, false);
                            }
                        }

                        if (_TaskIntegrator != null)
                        {
                            _TaskIntegrator.OnTesStatementCompleted();
                            _TaskIntegrator.OnTesStateChanged();
                        }

                        switch (_TransactionMode)
                        {
                            case Config.TransactionModeEnum.Manual:
                                break;
                            case Config.TransactionModeEnum.AutoSingle:
                                _NpgsqlCommand.CommandText = "COMMIT";
                                _NpgsqlCommand.ExecuteNonQuery();

                                _StringBuilder.AppendLine();
                                StringBuilderAppendIndentedLine(Properties.Text.commited_single_auto_transaction, true);
                                break;
                            case Config.TransactionModeEnum.AutoCoordinated:
                                string idTransaccion = _TaskIntegrator!.CoordinatedTransactionId + "_" + Guid.NewGuid().ToString();

                                _NpgsqlCommand.CommandText = $"PREPARE TRANSACTION '{idTransaccion}'";
                                _NpgsqlCommand.ExecuteNonQuery();
                                _PreparedCommit = true;

                                _StringBuilder.AppendLine();
                                StringBuilderAppendIndentedLine(string.Format(Properties.Text.prepared_coordinated_auto_transaction, idTransaccion), true);
                                StringBuilderAppendIndentedLine(Properties.Text.waiting_tasks, false);
                                _TaskIntegrator.OnTesWaitingCoordinatedCommit(this);

                                while (!_Cancel && !_TaskIntegrator.AreAllTransactionsPrepared)
                                {
                                    Thread.Sleep(500);
                                }

                                if (_Cancel)
                                {
                                    _NpgsqlCommand.CommandText = $"ROLLBACK PREPARED '{idTransaccion}'";
                                    _NpgsqlCommand.ExecuteNonQuery();

                                    _StringBuilder.AppendLine();
                                    StringBuilderAppendIndentedLine(Properties.Text.rollbacked_coordinated_auto_transaction, true);
                                    _Exception = new Exception(Properties.Text.rollbacked_prepared_transaction);
                                }
                                else
                                {
                                    _NpgsqlCommand.CommandText = $"COMMIT PREPARED '{idTransaccion}'";
                                    _NpgsqlCommand.ExecuteNonQuery();

                                    _StringBuilder.AppendLine();
                                    StringBuilderAppendIndentedLine(Properties.Text.commited_coordinated_auto_transaction, true);
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                    catch (Exception ex)
                    {
                        Exception? exi = ex;
                        while (exi != null)
                        {
                            StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {exi.Message}", false);

                            if (exi is NpgsqlException)
                            {
                                NpgsqlException nex = (NpgsqlException)exi;
                                if (!string.IsNullOrWhiteSpace(nex.SqlState)) StringBuilderAppendIndentedLine($"{Properties.Text.pg_error_code}: {nex.SqlState}", false);

                                if (exi is PostgresException)
                                {
                                    PostgresException pex = (PostgresException)exi;
                                    if (!string.IsNullOrEmpty(pex.Detail))
                                    {
                                        StringBuilderAppendIndentedLine($"{Properties.Text.pg_exception_detail}: {pex.Detail}", false);
                                    }
                                }
                            }
                            exi = exi.InnerException;
                        }

                        if (_TransactionMode != Config.TransactionModeEnum.Manual)
                        {
                            _NpgsqlCommand.CommandText = "ROLLBACK";
                            _NpgsqlCommand.ExecuteNonQuery();

                            _StringBuilder.AppendLine();
                            StringBuilderAppendIndentedLine(Properties.Text.rollbacked_single_auto_transaction, true);
                        }
                        throw new AlreadyLoggedException(ex.Message, ex);
                    }
                }
            }
            catch (AlreadyLoggedException ex2)
            {
                _Exception = ex2;
                if (_TaskIntegrator != null) _TaskIntegrator.OnTesException(this);
                if (_TaskIntegrator != null && _TransactionMode == Config.TransactionModeEnum.AutoCoordinated) _TaskIntegrator.Cancel();
            }
            catch (Exception ex2)
            {
                StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {ex2.Message}", true);

                _Exception = ex2;
                if (_TaskIntegrator != null) _TaskIntegrator.OnTesException(this);
                if (_TaskIntegrator != null && _TransactionMode == Config.TransactionModeEnum.AutoCoordinated) _TaskIntegrator.Cancel();
            }
            finally
            {
                _NpgsqlCommand = null;
                StringBuilderAppendIndentedLine(Properties.Text.closed_connection, true);

                _TotalDuration = DateTime.Now.Subtract(_StartTimestamp!.Value);
                _StringBuilder.AppendLine();
                _StringBuilder.AppendLine($"{Properties.Text.total_duration}: {EllapsedTimeDescription(_TotalDuration!.Value, true)}");

                _State = StateEnum.Finished;

                _OnUpdate(this);
                if (_TaskIntegrator != null) _TaskIntegrator.OnTesStateChanged();
            }
        }

        public override string ToString()
        {
            return _CreationTimestamp.ToShortTimeString() + $" - {string.Format(Properties.Text.task_in, _DB.Alias)}: " + StateDescription;
        }
    }
}

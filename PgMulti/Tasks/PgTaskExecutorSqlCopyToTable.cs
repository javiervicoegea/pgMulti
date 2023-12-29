using Irony.Parsing;
using Microsoft.VisualBasic.Logging;
using Npgsql;
using Npgsql.Schema;
using PgMulti.AppData;
using PgMulti.DataStructure;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PgMulti.Tasks
{
    public class PgTaskExecutorSqlCopyToTable : PgTaskExecutorSql
    {
        private List<DB> _DBs;
        private int _CurrentDBIndex = -1;
        private ReadOnlyCollection<NpgsqlDbColumn>? _SourceColumns = null;
        private Table? _DestinationTable = null;
        private Dictionary<int, int>? _ColumnMapping = null;
        private Schema? _DestinationNewTableSchema = null;
        private string? _DestinationNewTableName = null;

        public PgTaskExecutorSqlCopyToTable(
            Data d, List<DB> dbs, OnUpdate onUpdate, string sql,
            Config.TransactionModeEnum modoTransacciones, Config.TransactionLevelEnum nivelTransacciones,
            LanguageData sld
        ) : base(d, onUpdate, sql, modoTransacciones, nivelTransacciones, sld)
        {
            _DBs = dbs;
        }


        public List<DB> DBs
        {
            get
            {
                return _DBs;
            }
        }

        public int CurrentDBIndex
        {
            get
            {
                return _CurrentDBIndex;
            }
        }

        public ReadOnlyCollection<NpgsqlDbColumn>? SourceColumns
        {
            get
            {
                return _SourceColumns;
            }
        }

        public void SetDestinationTableAndMapping(Table destinationTable, Dictionary<int, int> columnMapping)
        {
            if (columnMapping.Any(p => !destinationTable.Columns.Any(c => c.Position == p.Key) || !_SourceColumns!.Any(sc => sc.ColumnOrdinal!.Value == p.Value))) throw new ArgumentException();

            _DestinationTable = destinationTable;
            _ColumnMapping = columnMapping;
        }

        public void SetNewTable(Schema destinationNewTableSchema, string destinationNewTableName)
        {
            _DestinationNewTableSchema = destinationNewTableSchema;
            _DestinationNewTableName = destinationNewTableName;
        }

        protected override void Run()
        {
            NpgsqlConnection? destinationConnection = null;
            NpgsqlTransaction? destinationTransaction = null;
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
                    StringBuilderAppendSummaryLine($"{Properties.Text.error_processing_task}: " + ex.Message, LogStyle.Error);
                    throw new AlreadyLoggedException(ex.Message, ex);
                }

                _StatementCount = statements.Count;
                _CurrentDBIndex = 0;


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

                foreach (DB db in DBs)
                {
                    NpgsqlConnection c = db.Connection;
                    c.Notice += Connection_Notice;
                    using (c)
                    {
                        _Notices = new List<PostgresNotice>();
                        _NpgsqlCommand = c.CreateCommand();

                        c.Open();
                        StringBuilderAppendIndentedLine(string.Format(Properties.Text.connection_opened_to, db.Alias), true);

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
                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.automatic_begin_transaction, nivel), false);
                        }

                        //try
                        //{
                        for (_CurrentStatementIndex = 0; _CurrentStatementIndex < statements.Count && !_Canceled; _CurrentStatementIndex++)
                        {
                            Tuple<int, string, string> stmt = statements[_CurrentStatementIndex];
                            int linea = stmt.Item1;
                            string sql = stmt.Item2;

                            StringBuilderAppendEmptyLine();
                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.query_counter, _CurrentStatementIndex + 1, StatementCount, linea + 1) + ":", true, LogStyle.Query);
                            StringBuilderAppendIndentedLine(sql, false);
                            StringBuilderAppendEmptyLine();

                            _NpgsqlCommand.CommandText = sql;

                            int affectedRows;
                            DateTime start = DateTime.Now;

                            _NpgsqlCommand.AllResultTypesAreUnknown = true;

                            using (NpgsqlDataReader drd = _NpgsqlCommand.ExecuteReader())
                            {
                                affectedRows = drd.RecordsAffected;
                                if (affectedRows == -1)
                                {
                                    ReadOnlyCollection<NpgsqlDbColumn> sourceColumns = drd.GetColumnSchema();

                                    if (_SourceColumns == null)
                                    {
                                        _SourceColumns = sourceColumns;
                                    }
                                    else
                                    {
                                        bool compatible = true;
                                        foreach (NpgsqlDbColumn column in _SourceColumns)
                                        {
                                            if (!sourceColumns.Any(ci => ci.ColumnName == column.ColumnName && ci.PostgresType != column.PostgresType))
                                            {
                                                compatible = false;
                                                break;
                                            }
                                        }

                                        if (!compatible)
                                        {
                                            StringBuilderAppendIndentedLine(Properties.Text.ignoring_incompatible_query, false);
                                            continue;
                                        }
                                    }

                                    if (_SourceColumns.Count > 0)
                                    {
                                        if ((_DestinationNewTableSchema == null || _DestinationNewTableName == null) && (_DestinationTable == null || _ColumnMapping == null))
                                        {
                                            StringBuilderAppendIndentedLine(Properties.Text.waiting_for_destination_table, true);

                                            while (_DestinationTable == null && _DestinationNewTableName == null && !_Canceled)
                                            {
                                                Thread.Sleep(1000);
                                            }

                                            if (_Canceled) throw new Exception(Properties.Text.task_canceled_by_user);
                                        }

                                        NpgsqlCommand insertCommand = new NpgsqlCommand();
                                        if (destinationConnection == null)
                                        {
                                            DB destinationDB;
                                            if (_DestinationNewTableSchema == null)
                                            {
                                                destinationDB = _DestinationTable!.Schema!.DB;
                                            }
                                            else
                                            {
                                                destinationDB = _DestinationNewTableSchema.DB;
                                            }

                                            destinationConnection = destinationDB.Connection;
                                            destinationConnection.Open();
                                            StringBuilderAppendEmptyLine();
                                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.dest_connection_opened_to, destinationDB.Alias), true, LogStyle.TaskIsRunning);
                                            destinationTransaction = destinationConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                                            StringBuilderAppendIndentedLine(Properties.Text.dest_transaction_started, false);

                                            if (_DestinationNewTableSchema != null && _DestinationNewTableName != null)
                                            {
                                                _DestinationTable = new Table(_DestinationNewTableSchema, _DestinationNewTableName);
                                                _DestinationNewTableSchema.Tables.Add(_DestinationTable);
                                                _ColumnMapping = new Dictionary<int, int>();

                                                foreach (NpgsqlDbColumn sc in _SourceColumns)
                                                {
                                                    _DestinationTable.Columns.Add(new Column(_DestinationTable, sc));
                                                    _ColumnMapping[sc.ColumnOrdinal!.Value] = sc.ColumnOrdinal!.Value;
                                                }

                                                _DestinationTable.CreateInDB(destinationConnection, destinationTransaction);
                                            }

                                            StringBuilderAppendIndentedLine(string.Format(Properties.Text.inserting_rows_on, _DestinationTable!.Schema!.Id + "." + _DestinationTable!.Id), false, LogStyle.TaskIsRunning);
                                            StringBuilderAppendEmptyLine();
                                        }

                                        List<Tuple<int, int, NpgsqlDbColumn, Column, NpgsqlParameter, Type>> parameters = new List<Tuple<int, int, NpgsqlDbColumn, Column, NpgsqlParameter, Type>>();
                                        foreach (KeyValuePair<int, int> item in _ColumnMapping!)
                                        {
                                            NpgsqlDbColumn sourceColumn = _SourceColumns.First(sc => sc.ColumnOrdinal!.Value == item.Value);
                                            Column destinationColumn = _DestinationTable!.Columns.First(c => c.Position == item.Key);
                                            NpgsqlParameter p = new NpgsqlParameter("_" + item.Key, sourceColumn.NpgsqlDbType);
                                            Type sourceType = Column.GetDotNetType(sourceColumn.PostgresType.Name);

                                            insertCommand.Parameters.Add(p);

                                            parameters.Add(new Tuple<int, int, NpgsqlDbColumn, Column, NpgsqlParameter, Type>(item.Key, item.Value, sourceColumn, destinationColumn, p, sourceType));
                                        }

                                        insertCommand.CommandText = $"INSERT INTO {_DestinationTable!.IdSchema}.{_DestinationTable!.Id} ({string.Join(",", parameters.Select(t => t.Item4.Id))}) VALUES ({string.Join(",", parameters.Select(t => t.Item4.GetSqlParameterExpression(t.Item5.ParameterName)))})";
                                        insertCommand.Connection = destinationConnection;
                                        insertCommand.Transaction = destinationTransaction;

                                        CultureInfo? monetaryCultureInfo = null;
                                        if (parameters.Any(t => t.Item3.PostgresType.Name == "money"))
                                        {
                                            string lcMonetary;
                                            monetaryCultureInfo = QueryExecutorSql.GetMonetaryCultureInfo(db, out lcMonetary);
                                            StringBuilderAppendIndentedLine(string.Format(string.Format(Properties.Text.money_culture_used, monetaryCultureInfo.Name, lcMonetary)), false);
                                        }

                                        int savedRows = 0;
                                        while (drd.Read() && !_Canceled)
                                        {
                                            if (savedRows > 0 && savedRows % 1000 == 0)
                                            {
                                                StringBuilderAppendIndentedLine($"{Properties.Text.saving_row}: {savedRows}", false);
                                            }

                                            foreach (Tuple<int, int, NpgsqlDbColumn, Column, NpgsqlParameter, Type> item in parameters!)
                                            {
                                                object o;

                                                o = QueryExecutorSql.ParseValue(drd[item.Item2], item.Item6, item.Item3.PostgresType.Name, monetaryCultureInfo);

                                                insertCommand.Parameters["_" + item.Item1].Value = o;
                                            }

                                            int n = insertCommand.ExecuteNonQuery();
                                            if (n != 1) throw new Exception();

                                            savedRows++;
                                        }

                                        StringBuilderAppendIndentedLine($"{Properties.Text.saved_rows}: {savedRows}", false);
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
                                StringBuilderAppendIndentedLine($"{n.Severity}: {n.MessageText} - {n.Detail}", false);
                            }
                        }

                        if (destinationTransaction != null)
                        {
                            destinationTransaction.Commit();
                            StringBuilderAppendEmptyLine();
                            StringBuilderAppendIndentedLine(Properties.Text.dest_transaction_commited, true);
                        }

                        switch (_TransactionMode)
                        {
                            case Config.TransactionModeEnum.Manual:
                                break;
                            case Config.TransactionModeEnum.AutoSingle:
                            case Config.TransactionModeEnum.AutoCoordinated:
                                _NpgsqlCommand.CommandText = "COMMIT";
                                _NpgsqlCommand.ExecuteNonQuery();

                                StringBuilderAppendEmptyLine();
                                StringBuilderAppendIndentedLine(Properties.Text.commited_single_auto_transaction, true);
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Exception? exi = ex;
                        //    while (exi != null)
                        //    {
                        //        StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {exi.Message}", false, LogStyle.Error);

                        //        if (exi is NpgsqlException)
                        //        {
                        //            NpgsqlException nex = (NpgsqlException)exi;
                        //            if (!string.IsNullOrWhiteSpace(nex.SqlState))
                        //            {
                        //                StringBuilderAppendEmptyLine();
                        //                StringBuilderAppendIndentedLine($"{Properties.Text.pg_error_code}: {nex.SqlState}", false, LogStyle.Error);
                        //            }

                        //            if (nex is PostgresException)
                        //            {
                        //                PostgresException pex = (PostgresException)nex;
                        //                if (!string.IsNullOrEmpty(pex.Detail))
                        //                {
                        //                    StringBuilderAppendEmptyLine();
                        //                    StringBuilderAppendIndentedLine($"{Properties.Text.pg_exception_detail}: {pex.Detail}", false, LogStyle.Error);
                        //                }
                        //            }
                        //        }

                        //        exi = exi.InnerException;
                        //    }

                        //    if (_TransactionMode != Config.TransactionModeEnum.Manual)
                        //    {
                        //        _NpgsqlCommand.CommandText = "ROLLBACK";
                        //        _NpgsqlCommand.ExecuteNonQuery();

                        //        StringBuilderAppendEmptyLine();
                        //        StringBuilderAppendIndentedLine(Properties.Text.rollbacked_single_auto_transaction, true);
                        //    }
                        //    throw new AlreadyLoggedException(ex.Message, ex);
                        //}
                    }

                    _CurrentDBIndex++;

                    if (_Canceled) throw new Exception(Properties.Text.task_canceled_by_user);
                }
            }
            catch (AlreadyLoggedException ex2)
            {
                _Exception = ex2;
            }
            //catch (Exception ex2)
            //{
            //    StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {ex2.Message}", true, LogStyle.Error);

            //    _Exception = ex2;
            //}
            finally
            {
                if (destinationConnection != null) destinationConnection.Dispose();

                _NpgsqlCommand = null;
                StringBuilderAppendIndentedLine(Properties.Text.closed_connection, true);

                _TotalDuration = DateTime.Now.Subtract(_StartTimestamp!.Value);
                StringBuilderAppendEmptyLine();
                StringBuilderAppendSummaryLine($"{Properties.Text.total_duration}: {EllapsedTimeDescription(_TotalDuration!.Value, true)}", _Exception == null ? LogStyle.TaskSuccessfullyCompleted : LogStyle.TaskFailed);

                _State = StateEnum.Finished;

                _OnUpdate(this);
            }
        }

        public override string ToString()
        {
            return _CreationTimestamp.ToShortTimeString() + $" - {Properties.Text.copy_to_table_task}: {StateDescription}";
        }
    }
}

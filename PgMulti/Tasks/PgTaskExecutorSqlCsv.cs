﻿using CsvHelper;
using Irony.Parsing;
using Npgsql;
using Npgsql.Schema;
using PgMulti.AppData;
using System.Collections.ObjectModel;
using System.Text;

namespace PgMulti.Tasks
{
    public class PgTaskExecutorSqlCsv : PgTaskExecutorSql
    {
        private List<DB> _DBs;
        private string _FileName;
        private int _CurrentDBIndex = -1;

        public PgTaskExecutorSqlCsv(
            Data d, List<DB> dbs, OnUpdate onUpdate, string sql,
            Config.TransactionModeEnum modoTransacciones, Config.TransactionLevelEnum nivelTransacciones,
            LanguageData sld, string fileName
        ) : base(d, onUpdate, sql, modoTransacciones, nivelTransacciones, sld)
        {
            _DBs = dbs;
            _FileName = fileName;
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
                    StringBuilderAppendSummaryLine($"{Properties.Text.error_processing_task}: " + ex.Message, LogStyle.Error);
                    throw new AlreadyLoggedException(ex.Message, ex);
                }

                _StatementCount = statements.Count;
                _CurrentDBIndex = 0;

                CsvHelper.Configuration.CsvConfiguration conf = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture);
                conf.Delimiter = ";";

                using (Stream s = File.Open(_FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter sw = new StreamWriter(s, Encoding.GetEncoding(1252)))
                using (CsvWriter cw = new CsvWriter(sw, conf))
                {
                    StringBuilderAppendIndentedLine(string.Format(Properties.Text.saving_rows_on_file, _FileName), true, LogStyle.TaskIsRunning);

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

                    ReadOnlyCollection<NpgsqlDbColumn>? cols = null;
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
                                StringBuilderAppendIndentedLine(string.Format(Properties.Text.automatic_begin_transaction, nivel), true);
                            }

                            try
                            {
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
                                            ReadOnlyCollection<NpgsqlDbColumn> colsDrd = drd.GetColumnSchema();
                                            if (colsDrd.Count > 0)
                                            {
                                                Dictionary<int, int> mapping = new Dictionary<int, int>();

                                                if (cols == null)
                                                {
                                                    cols = colsDrd;

                                                    for (int i = 0; i < cols.Count; i++)
                                                    {
                                                        NpgsqlDbColumn dc = cols[i];

                                                        cw.WriteField(dc.ColumnName);
                                                        mapping[i] = i;
                                                    }

                                                    if (_DBs.Count > 1)
                                                    {
                                                        cw.WriteField(Properties.Text.db_field_name);
                                                    }

                                                    cw.NextRecord();
                                                }
                                                else
                                                {
                                                    if (cols.Count != colsDrd.Count)
                                                        throw new Exception(Properties.Text.incompatible_queries);

                                                    for (int i = 0; i < cols.Count; i++)
                                                    {
                                                        if (cols[i].ColumnName == colsDrd[i].ColumnName && cols[i].DataType == colsDrd[i].DataType)
                                                        {
                                                            mapping[i] = i;
                                                        }
                                                        else
                                                        {
                                                            bool found = false;
                                                            for (int j = 0; j < colsDrd.Count; j++)
                                                            {
                                                                if (!mapping.Values.Contains(j) && cols[i].ColumnName == colsDrd[j].ColumnName && cols[i].DataType == colsDrd[j].DataType)
                                                                {
                                                                    found = true;
                                                                    mapping[i] = j;
                                                                    break;
                                                                }
                                                            }
                                                            if (!found)
                                                            {
                                                                throw new Exception(Properties.Text.incompatible_queries);
                                                            }
                                                        }
                                                    }
                                                }

                                                int savedRows = 0;
                                                while (drd.Read() && !_Canceled)
                                                {
                                                    if (savedRows > 0 && savedRows % 1000 == 0)
                                                    {
                                                        StringBuilderAppendIndentedLine($"{Properties.Text.saving_row}: {savedRows}", false);
                                                    }

                                                    for (int i = 0; i < cols.Count; i++)
                                                    {
                                                        NpgsqlDbColumn dc = cols[i];

                                                        if (drd.IsDBNull(mapping[i]))
                                                        {
                                                            cw.WriteField(null);
                                                        }
                                                        else
                                                        {
                                                            cw.WriteField(drd.GetString(mapping[i]));
                                                        }
                                                    }

                                                    if (_DBs.Count > 1)
                                                    {
                                                        cw.WriteField(db.Alias);
                                                    }

                                                    cw.NextRecord();

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
                            }
                            catch (Exception ex)
                            {
                                Exception? exi = ex;
                                while (exi != null)
                                {
                                    StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {exi.Message}", false, LogStyle.Error);

                                    if (exi is NpgsqlException)
                                    {
                                        NpgsqlException nex = (NpgsqlException)exi;
                                        if (!string.IsNullOrWhiteSpace(nex.SqlState))
                                        {
                                            StringBuilderAppendEmptyLine();
                                            StringBuilderAppendIndentedLine($"{Properties.Text.pg_error_code}: {nex.SqlState}", false, LogStyle.Error);
                                        }

                                        if (nex is PostgresException)
                                        {
                                            PostgresException pex = (PostgresException)nex;
                                            if (!string.IsNullOrEmpty(pex.Detail))
                                            {
                                                StringBuilderAppendEmptyLine();
                                                StringBuilderAppendIndentedLine($"{Properties.Text.pg_exception_detail}: {pex.Detail}", false, LogStyle.Error);
                                            }
                                        }
                                    }

                                    exi = exi.InnerException;
                                }

                                if (_TransactionMode != Config.TransactionModeEnum.Manual)
                                {
                                    _NpgsqlCommand.CommandText = "ROLLBACK";
                                    _NpgsqlCommand.ExecuteNonQuery();

                                    StringBuilderAppendEmptyLine();
                                    StringBuilderAppendIndentedLine(Properties.Text.rollbacked_single_auto_transaction, true);
                                }
                                throw new AlreadyLoggedException(ex.Message, ex);
                            }
                        }

                        _CurrentDBIndex++;

                        if (_Canceled) throw new Exception(Properties.Text.task_canceled_by_user);
                    }
                }
            }
            catch (AlreadyLoggedException ex2)
            {
                _Exception = ex2;
            }
            catch (Exception ex2)
            {
                StringBuilderAppendIndentedLine($"{Properties.Text.error_processing_task}: {ex2.Message}", true, LogStyle.Error);

                _Exception = ex2;
            }
            finally
            {
                _NpgsqlCommand = null;
                StringBuilderAppendIndentedLine(Properties.Text.closed_connection, true);

                _TotalDuration = DateTime.Now.Subtract(_StartTimestamp!.Value);
                StringBuilderAppendEmptyLine();
                StringBuilderAppendSummaryLine($"{Properties.Text.total_duration}: {EllapsedTimeDescription(_TotalDuration!.Value, true)}", _Exception == null ? LogStyle.TaskSuccessfullyCompleted: LogStyle.TaskFailed);

                _State = StateEnum.Finished;

                _OnUpdate(this);
            }
        }

        public override string ToString()
        {
            return _CreationTimestamp.ToShortTimeString() + $" - {Properties.Text.csv_task}: {StateDescription}";
        }
    }
}

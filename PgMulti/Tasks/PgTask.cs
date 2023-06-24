using PgMulti.AppData;
using System.Text;
using System.Text.RegularExpressions;

namespace PgMulti.Tasks
{
    public abstract class PgTask
    {
        public delegate void OnUpdate(PgTask t);

        protected Data _Data;
        protected string _Sql;
        protected Exception? _Exception = null;
        protected DateTime _CreationTimestamp;
        protected DateTime? _StartTimestamp;
        protected TimeSpan? _TotalDuration;
        protected StateEnum _State = StateEnum.Init;
        protected OnUpdate _OnUpdate;
        protected StringBuilder _StringBuilder;
        protected List<Query> _Queries;
        protected int _StatementCount = -1;
        protected int _CurrentStatementIndex = -1;
        protected bool _Cancel = false;

        public PgTask(Data d, OnUpdate onUpdate, string sql)
        {
            _Data = d;
            _OnUpdate = onUpdate;
            _CreationTimestamp = DateTime.Now;
            _Sql = sql;
            _Queries = new List<Query>();
            _StringBuilder = new StringBuilder();
        }

        public int StatementCount
        {
            get
            {
                return _StatementCount;
            }
        }

        public int CurrentStatementIndex
        {
            get
            {
                return _CurrentStatementIndex;
            }
        }

        public List<Query> Queries
        {
            get
            {
                return _Queries;
            }
        }

        public Exception? Exception
        {
            get
            {
                return _Exception;
            }
        }

        public TimeSpan? TotalDuration
        {
            get
            {
                return _TotalDuration;
            }
        }

        public DateTime? StartTimestamp
        {
            get
            {
                return _StartTimestamp;
            }
        }

        public string Log
        {
            get
            {
                return _StringBuilder.ToString();
            }
        }

        public StateEnum State
        {
            get
            {
                return _State;
            }
        }

        public string Sql
        {
            get
            {
                return _Sql;
            }
        }

        public virtual void Start()
        {
            _StartTimestamp = DateTime.Now;
            _State = StateEnum.Running;
            _OnUpdate(this);
        }

        public virtual void Cancel()
        {
            _Cancel = true;
        }

        protected abstract void Run();

        protected string StateDescription
        {
            get
            {
                switch (_State)
                {
                    case StateEnum.Init:
                        return Properties.Text. initializing;
                    case StateEnum.Running:
                        return Properties.Text.running + " (" + EllapsedTimeDescription(DateTime.Now.Subtract(_StartTimestamp!.Value), false) + ")";
                    case StateEnum.Finished:
                        if (_Exception == null)
                        {
                            return Properties.Text.finished_successfully + " (" + EllapsedTimeDescription(_TotalDuration!.Value, true) + ")";
                        }
                        else
                        {
                            return Properties.Text.finished_error + " (" + EllapsedTimeDescription(_TotalDuration!.Value, true) + ")";
                        }
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        protected string EllapsedTimeDescription(TimeSpan ts, bool ms)
        {
            if (ts.TotalHours > 1)
            {
                return ts.Hours + "h " + ts.Minutes + " min " + ts.Seconds + " s";
            }
            else if (ts.TotalMinutes > 1)
            {
                return ts.Minutes + " min " + ts.Seconds + " s";
            }
            else if (ts.TotalSeconds > 1)
            {
                return ts.Seconds + " s " + (ms ? ts.Milliseconds + " ms" : "");
            }
            else
            {
                return ts.Milliseconds + " ms";
            }
        }

        protected virtual void StringBuilderAppendIndentedLine(string s, bool includeTimestamp)
        {
            if (includeTimestamp)
            {
                _StringBuilder.Append($"[{DateTime.Now.ToShortTimeString()}] ");
            }
            else
            {
                _StringBuilder.Append(TimestampGap);
            }
            _StringBuilder.AppendLine(s.Replace("\n", "\n" + TimestampGap));
        }

        private string? _TimestampGap = null;
        private string TimestampGap
        {
            get
            {
                if (_TimestampGap == null)
                {
                    _TimestampGap = Regex.Replace($"[{DateTime.Now.ToShortTimeString()}] ", ".", " ");
                }

                return _TimestampGap;
            }
        }

        public enum StateEnum
        {
            Init,
            Running,
            Finished
        }
    }
}

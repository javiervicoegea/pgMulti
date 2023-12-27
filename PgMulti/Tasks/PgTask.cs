using FastColoredTextBoxNS;
using PgMulti.AppData;
using System.Security.Cryptography.X509Certificates;
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
        protected List<Query> _Queries;
        protected int _StatementCount = -1;
        protected int _CurrentStatementIndex = -1;
        protected bool _Canceled = false;

        private StringBuilder _StringBuilder;

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

        private string Log
        {
            get
            {
                lock (_StringBuilder)
                {
                    return _StringBuilder.ToString();
                }
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

        public bool Canceled
        {
            get
            {
                return _Canceled;
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
            _Canceled = true;
        }

        protected abstract void Run();

        protected string StateDescription
        {
            get
            {
                switch (_State)
                {
                    case StateEnum.Init:
                        return Properties.Text.initializing;
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

        public void PrintLog(FastColoredTextBox tb, Dictionary<LogStyle, FastColoredTextBoxNS.Style> styles)
        {

            Regex r = new Regex(@"<style name=""([^\""]+)"">([^<>]*)<\/style>([^<>]*)");

            string s = Log;
            Match m = r.Match(s);

            if (m.Success)
            {
                if (m.Index > 0)
                {
                    FastColoredTextBoxAppendText(tb, s.Substring(0, m.Index));
                }

                while (m.Success)
                {
                    if (m.Groups[2].Value != "")
                    {
                        FastColoredTextBoxAppendText(tb, m.Groups[2].Value, styles[Enum.Parse<LogStyle>(m.Groups[1].Value)]);
                    }
                    if (m.Groups[3].Value != "")
                    {
                        FastColoredTextBoxAppendText(tb, m.Groups[3].Value);
                    }

                    m = m.NextMatch();
                }
            }
            else
            {
                if (s != "")
                {
                    FastColoredTextBoxAppendText(tb, s);
                }
            }
        }

        private void FastColoredTextBoxAppendText(FastColoredTextBox tb, string text, Style? style = null)
        {
            tb.AppendText(text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\""), style);
        }

        private string EncodeText(string s)
        {
            return s.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        private string ApplyStyle(string s, LogStyle style)
        {
            return $"<style name=\"{style.ToString()}\">{s}</style>";
        }

        protected void StringBuilderAppendIndentedLine(string s, bool includeTimestamp, LogStyle? style = null)
        {
            lock (_StringBuilder)
            {
                if (includeTimestamp)
                {
                    _StringBuilder.Append(ApplyStyle($"[{DateTime.Now.ToShortTimeString()}]", LogStyle.Timestamp) + " ");
                }
                else
                {
                    _StringBuilder.Append(TimestampGap);
                }

                if (style.HasValue)
                {
                    string[] lines = EncodeText(s).Split("\r\n");
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i > 0) _StringBuilder.Append(TimestampGap);

                        _StringBuilder.AppendLine(ApplyStyle(lines[i], style.Value));
                    }
                }
                else
                {
                    _StringBuilder.AppendLine(EncodeText(s).Replace("\n", "\n" + TimestampGap));
                }
            }
        }

        protected void StringBuilderAppendSummaryLine(string s, LogStyle? style = null)
        {
            s = EncodeText(s);

            if (style.HasValue)
            {
                s = ApplyStyle(s, style.Value);
            }

            lock (_StringBuilder)
            {
                _StringBuilder.AppendLine(s);
            }
        }

        protected void StringBuilderAppendEmptyLine()
        {
            lock (_StringBuilder)
            {
                _StringBuilder.AppendLine();
            }
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

        public enum LogStyle
        {
            Timestamp,
            Query,
            TaskIsRunning,
            TaskSuccessfullyCompleted,
            TaskFailed,
            Error
        }
    }
}

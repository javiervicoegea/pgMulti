using FastColoredTextBoxNS;
using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using PgMulti.SqlSyntax;
using System.Diagnostics;
using System.Text;
using static System.Windows.Forms.LinkLabel;

namespace PgMulti.Tasks
{
    public abstract class PgTaskExecutorSql : PgTask
    {
        protected NpgsqlCommand? _NpgsqlCommand;
        protected List<PostgresNotice>? _Notices;
        protected Config.TransactionModeEnum _TransactionMode;
        protected Config.TransactionLevelEnum _TransactionLevel;
        protected LanguageData _PGSimpleLanguageData;
        protected Thread _Thread;

        public PgTaskExecutorSql(
            Data d, OnUpdate onUpdate, string sql,
            Config.TransactionModeEnum transactionMode, Config.TransactionLevelEnum transactionLevel,
            LanguageData sld
        ) : base(d, onUpdate, sql)
        {
            _PGSimpleLanguageData = sld;
            _TransactionMode = transactionMode;
            _TransactionLevel = transactionLevel;
            _Thread = new Thread(new ThreadStart(Run));
            _Thread.Name = "PgTaskExecutorSql";
        }

        protected void Connection_Notice(object sender, NpgsqlNoticeEventArgs e)
        {
            _Notices!.Add(e.Notice);
        }

        public override void Start()
        {
            base.Start();
            _Thread.Start();
        }

        public override void Cancel()
        {
            if (_Canceled) return;

            base.Cancel();

            if (_NpgsqlCommand != null) _NpgsqlCommand.Cancel();
        }

        protected List<Tuple<int, string, string>> ListStatements()
        {
            Parser parser = new Parser(_PGSimpleLanguageData);
            string txt = _Sql;
            ParseTree parseTree = parser.Parse(txt);

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                int indexInsertPointToken = parseTree.Tokens.Count - 1;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "EOF") indexInsertPointToken--;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "line_comment") indexInsertPointToken--;

                Token startToken = parseTree.Tokens[0];
                Token insertPointToken = parseTree.Tokens[indexInsertPointToken];
                Token endToken = parseTree.Tokens[parseTree.Tokens.Count - 1];

                int start = startToken.Location.Position;
                int insertPoint = insertPointToken.Location.Position + insertPointToken.Length;
                int end = endToken.Location.Position + endToken.Length;

                txt = txt.Substring(start,insertPoint-start) + ";" + txt.Substring(insertPoint, end - insertPoint);

                parseTree = parser.Parse(txt);
            }


            if (parseTree.Status == ParseTreeStatus.Error)
            {
                throw new Exception(Properties.Text.basic_parse_error + "\r\n" + string.Format(Properties.Text.line_column, parseTree.ParserMessages[0].Location.Line + 1, parseTree.ParserMessages[0].Location.Column + 1) + ": " + parseTree.ParserMessages[0].Message);
            }

            AstNode astRoot = AstNode.ProcessParseTree(parseTree);

            List<Tuple<int, string, string>> statements = new List<Tuple<int, string, string>>();

            foreach (AstNode na in astRoot.Children[0].Children)
            {
                string stmtType = na.RecursiveTokens[0].Token!.Text.ToUpper();

                if (stmtType == ";") continue;

                Tuple<int, string, string> stmt = new Tuple<int, string, string>(na.RecursiveTokens[0].StartLine, na.SingleLineText, stmtType);

                statements.Add(stmt);
            }

            return statements;
        }

        public class AlreadyLoggedException : Exception
        {
            public AlreadyLoggedException(string msg) : base(msg) { }
            public AlreadyLoggedException(string msg, Exception ex) : base(msg, ex) { }
        }
    }
}

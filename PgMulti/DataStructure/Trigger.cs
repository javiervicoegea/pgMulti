using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using PgMulti.SqlSyntax;
using System.Xml;

namespace PgMulti.DataStructure
{
    public class Trigger
    {
        private string _Id;
        private string _IdTable;
        private string _IdSchema;
        private string _IdFunction;
        private string _IdSchemaFunction;
        private string _Momentum;
        private string _Action;
        private string _Repetition;

        private Table? _Table;
        private Function? _Function;

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdTable { get => _IdTable; internal set => _IdTable = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public string IdFunction { get => _IdFunction; internal set => _IdFunction = value; }
        public string IdSchemaFunction { get => _IdSchemaFunction; internal set => _IdSchemaFunction = value; }
        public string Momentum { get => _Momentum; internal set => _Momentum = value; }
        public string Action { get => _Action; internal set => _Action = value; }
        public string Repetition { get => _Repetition; internal set => _Repetition = value; }

        public Table? Table { get => _Table; internal set => _Table = value; }
        public Function? Function { get => _Function; internal set => _Function = value; }

        internal Trigger(NpgsqlDataReader drd, Parser parser)
        {
            _Id = drd.Ref<string>("tgname")!.ToLower();
            _IdSchema = drd.Ref<string>("nspname")!.ToLower();
            _IdTable = drd.Ref<string>("relname")!.ToLower();

            string def = drd.Ref<string>("triggerdef")!.ToLower();
            ParseTree parseTree = parser.Parse(def + ";");
            AstNode nCreateTriggerStmt;

            try
            {
                nCreateTriggerStmt = AstNode.ProcesarParseTree(parseTree)[0][0][0][0];
            }
            catch (Exception ex)
            {
                throw new NotSupportedTriggerSqlDefinition(def, ex);
            }

            _Momentum = nCreateTriggerStmt["createTriggerMomentumClause"]!.SingleLineText;

            _Action = nCreateTriggerStmt["createTriggerActionClause"]!.SingleLineText;

            _Repetition = nCreateTriggerStmt["createTriggerRepetitionClause"]!.SingleLineText;

            AstNode nIdFunction = nCreateTriggerStmt["createTriggerExecuteClause"]!["id"]!;
            if (nIdFunction.Count == 3)
            {
                _IdFunction = nIdFunction[2].SingleLineText;
                _IdSchemaFunction = nIdFunction[0].SingleLineText;
            }
            else
            {
                throw new Exception($"Function {nIdFunction.SingleLineText} of trigger {_IdSchema}.{_Id} is missing schema name");
            }
        }

        public class NotSupportedTriggerSqlDefinition : Exception
        {
            public NotSupportedTriggerSqlDefinition(string definition, Exception innerException) : base("Not supported trigger definition '" + definition + "'", innerException) { }
        }
    }
}

using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using PgMulti.SqlSyntax;
using System.Xml;

namespace PgMulti.DataStructure
{
    public class TableIndex
    {
        private string _Id;
        private string _IdTable;
        private string _IdSchema;
        private bool _Unique;
        private string _OrderList;
        private string _Using;
        private string? _Filter;

        private Table? _Table;

        public static Parser CreateParser(LanguageData languageData)
        {
            return new Parser(languageData, languageData.Grammar.SnippetRoots.First(nt => nt.Name == "createIndexStmt"));
        }

        public string Id { get => _Id; internal set => _Id = value; }
        public string IdTable { get => _IdTable; internal set => _IdTable = value; }
        public string IdSchema { get => _IdSchema; internal set => _IdSchema = value; }
        public bool Unique { get => _Unique; internal set => _Unique = value; }
        public string OrderList { get => _OrderList; internal set => _OrderList = value; }
        public string Using { get => _Using; internal set => _Using = value; }
        public string? Filter { get => _Filter; internal set => _Filter = value; }

        public Table? Table { get => _Table; internal set => _Table = value; }

        internal TableIndex(NpgsqlDataReader drd, Parser parser)
        {
            _Id = drd.Ref<string>("indexname")!;
            _IdSchema = drd.Ref<string>("schemaname")!;
            _IdTable = drd.Ref<string>("tablename")!;

            string def = drd.Ref<string>("indexdef")!;
            ParseTree parseTree = parser.Parse(def);
            AstNode nCreateIndexStmt = AstNode.ProcessParseTree(parseTree);

            _OrderList = nCreateIndexStmt["orderList"]!.SingleLineText;

            _Unique = nCreateIndexStmt["uniqueOpt"] != null;

            _Using = nCreateIndexStmt["usingIndexClauseOpt"]!.SingleLineText;

            AstNode? nWhereClauseOpt = nCreateIndexStmt["whereClauseOpt"];
            if (nWhereClauseOpt == null)
            {
                _Filter = null;
            }
            else
            {
                _Filter = nWhereClauseOpt.SingleLineText;
            }
        }
    }
}

using CsvHelper;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.SqlSyntax
{
    public class PostgreSqlIdParser
    {
        private Parser _Parser;

        public PostgreSqlIdParser(LanguageData languageData)
        {
            _Parser = new Parser(languageData, languageData.Grammar.SnippetRoots.First(nt => nt.Name == "id"));
        }

        public string Sql2CleanDefinition(AstNode nId)
        {
            PostgreSqlId? id = TryParse(nId);
            if (id == null) return "";
            return id.ToString();
        }

        public string Sql2CleanDefinition(string s)
        {
            PostgreSqlId? id = TryParse(s);
            if (id == null) return "";
            return id.ToString();
        }

        public PostgreSqlId? TryParse(AstNode nId)
        {
            return new PostgreSqlId(nId);
        }

        public PostgreSqlId? TryParse(string s)
        {
            ParseTree parseTree = _Parser.Parse(s);
            if (parseTree.Status != ParseTreeStatus.Parsed) return null;

            AstNode nId = AstNode.ProcessParseTree(parseTree);
            return TryParse(nId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Irony.Parsing;

namespace PgMulti.SqlSyntax
{
    [Language("PostgreSQL", "15", "PostgreSQL simple commands grammar")]
    public class PostgreSqlSimpleCommandsGrammar : Grammar
    {
        public PostgreSqlSimpleCommandsGrammar(CultureInfo cu) : base(false)
        {
            DefaultCulture = cu;

            //SQL is case insensitive
            //Terminals
            var comment = new CommentTerminal("comment", "/*", "*/");
            var lineComment = new CommentTerminal("line_comment", "--", "\n", "\r\n");
            NonGrammarTerminals.Add(comment);
            NonGrammarTerminals.Add(lineComment);

            var number = new NumberLiteral("number");
            number.DefaultIntTypes = new TypeCode[] { TypeCode.Int32, TypeCode.Int64 };

            var string_literal = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote | StringOptions.NoEscapes | StringOptions.AllowsLineBreak);
            var escaped_string_literal = new StringLiteral("escaped_string", "E'", "'", StringOptions.AllowsDoubledQuote | StringOptions.AllowsAllEscapes | StringOptions.AllowsLineBreak);
            var dollar_string_tag = new StringLiteral("dollar_string_tag", "$");
            var dollar_variable = new IdentifierTerminal("dollar_variable");
            dollar_variable.AllFirstChars = "$";
            dollar_variable.AllChars = Irony.Strings.DecimalDigits;
            var id_simple = CreateIdentifier("id_simple");
            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var semi = ToTerm(";");

            //Non-terminals
            var stmt = new NonTerminal("stmt");
            var stmtList = new NonTerminal("stmtList");
            var stmtContent = new NonTerminal("stmtContent");
            var stmtContentPart = new NonTerminal("stmtContentPart");
            var word = new NonTerminal("word");
            var root = new NonTerminal("root");
            var dollarString = new NonTerminal("dollarString");
            var dollarStringContent = new NonTerminal("dollarStringContent");

            //BNF Rules
            Root = root;

            root.Rule = stmtList;
            stmtList.Rule = MakeStarRule(stmtList, stmt);
            stmt.Rule = stmtContent + semi;
            stmtContent.Rule = MakeStarRule(stmtContent, stmtContentPart);
            stmtContentPart.Rule = word | dollarString;
            word.Rule = number | string_literal | escaped_string_literal | id_simple | dollar_variable | dot | comma | "*" | "/" | "%" | "+" | "-" | "=" | ":=" | ">" | "<" | ">=" | "<=" | "<>" | "!=" | "!<" | "!>" | "^" | "&" | "|" | "(" | ")" | "[" | "]" | "::" | "~" | "!~" | "@@" | "..";
            
            dollarString.Rule = dollar_string_tag + dollarStringContent + dollar_string_tag;
            dollarStringContent.Rule = MakePlusRule(dollarStringContent, word | semi);
        }

        private IdentifierTerminal CreateIdentifier(string name)
        {
            var id = new IdentifierTerminal(name);
            id.AllFirstChars = Irony.Strings.AllLatinLetters + "_ÑÁÉÍÓÚÝÄËÏÖÜÀÈÌÒÙÂÊÎÔÛñáéíóúýäëïöüÿàèìòùâêîôû";
            id.AllChars = Irony.Strings.AllLatinLetters + Irony.Strings.DecimalDigits + "_$ÑÁÉÍÓÚÝÄËÏÖÜÀÈÌÒÙÂÊÎÔÛñáéíóúýäëïöüÿàèìòùâêîôû";

            StringLiteral term = new StringLiteral(name + "_qouted");
            term.AddStartEnd("\"", StringOptions.AllowsDoubledQuote | StringOptions.NoEscapes);
            term.SetOutputTerminal(this, id);
            return id;
        }

        public override bool IsWhitespaceOrDelimiter(char ch)
        {
            if (base.IsWhitespaceOrDelimiter(ch)) return true;

            switch (ch)
            {
                case '*':
                case '/':
                case '%':
                case '+':
                case '-':
                case '=':
                case '>':
                case '<':
                case '!':
                case '^':
                case '&':
                case '|':
                case ':':
                case '~':
                    return true;
                default:
                    return false;
            }
        }

    }
}

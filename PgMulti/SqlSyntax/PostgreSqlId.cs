using CsvHelper;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.SqlSyntax
{
    public class PostgreSqlId
    {
        public readonly string[] Values;

        public PostgreSqlId(string[] values)
        {
            Values = values;
        }

        public PostgreSqlId(AstNode nId)
        {
            Values = nId.Children.Where(n => n.Name == "id_simple").Select(n => PostgreSqlGrammar.IdFromString(n.SingleLineText)).ToArray();
            if (Values.Length == 0) throw new ArgumentException();
        }

        public string ToSqlString()
        {
            return string.Join(".", Values.Select(v => PostgreSqlGrammar.IdToString(v)));
        }

        public override string ToString()
        {
            return string.Join(".", Values);
        }
    }
}

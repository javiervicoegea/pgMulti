using FastColoredTextBoxNS;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemId : AutocompleteItemCustom
    {
        string? _IdNamespace;
        string _Id;
        private PostgreSqlIdParser _IdParser;

        public AutocompleteItemId(string? idNamespace, string id, int imageIndex, Font f, PostgreSqlIdParser idParser, string? tooltipTitle = null, string? tooltip = null)
            : base(id, imageIndex, id, tooltipTitle, tooltip, f)
        {
            _IdNamespace = idNamespace;
            _Id = id;
            _IdParser = idParser;
        }

        public override CompareResult Compare(string fragmentText)
        {
            string lastIdSimple;
            PostgreSqlId? pid = _IdParser.TryParse(fragmentText);

            if (pid == null)
            {
                lastIdSimple = "";
            }
            else
            {
                lastIdSimple = pid.Values.Last();
            }

            if (lastIdSimple == "") return CompareResult.Visible;
            if (_Id.StartsWith(lastIdSimple, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            if (_Id.Contains(lastIdSimple, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.Visible;

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            if (_IdNamespace == null)
            {
                return SqlSyntax.PostgreSqlGrammar.IdToString(_Id);
            }
            else
            {
                return SqlSyntax.PostgreSqlGrammar.IdToString(_IdNamespace) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(_Id);
            }
        }
    }
}

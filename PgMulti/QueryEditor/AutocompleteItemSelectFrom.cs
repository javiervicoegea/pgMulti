using FastColoredTextBoxNS;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemSelectFrom : AutocompleteItemCustom
    {
        private AstNode _NSelectBaseClauses;
        private string _FromText;

        public AutocompleteItemSelectFrom(AstNode nSelectBaseClauses, string fqTable, string alias)
            : base(alias + ".*", 1, fqTable, null, null)
        {
            _FromText = " FROM " + fqTable + " " + alias;
            _NSelectBaseClauses = nSelectBaseClauses;
            NeverAutoSelectOnSymbol = true;
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (fragmentText == "") return CompareResult.Visible;
            if (MenuText.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            if (MenuText.Contains(fragmentText.ToLower()))
                return CompareResult.Visible;

            return CompareResult.Hidden;
        }

        protected override void DoAutocomplete(FastColoredTextBoxNS.Range fragment)
        {
            AstNode? n = _NSelectBaseClauses["intoClauseOpt"];

            if (n == null)
            {
                n = _NSelectBaseClauses["selList"];
            }

            Place pFrom = new Place(n!.EndColumn, n.EndLine);

            var tb = fragment.tb;

            tb.BeginAutoUndo();
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            tb.Selection.Start = pFrom;
            tb.Selection.End = pFrom;
            tb.InsertText(_FromText + ";");
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            tb.Selection.Start = fragment.Start;
            tb.Selection.End = fragment.End;
            tb.InsertText(Text);
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            tb.EndAutoUndo();
            tb.Focus();
        }
    }
}

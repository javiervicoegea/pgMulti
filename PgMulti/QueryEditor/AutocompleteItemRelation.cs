using FastColoredTextBoxNS;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemRelation : AutocompleteItemCustom
    {
        private AstNode _AstNode;
        private string _ConditionText;
        private bool _CondicionEnWhere;
        private PostgreSqlIdParser _IdParser;

        public AutocompleteItemRelation(AstNode n, string currentTableAlias, string fqForeignTable, string fromText, string conditionText, bool conditionInWhere, bool isManyToOneRelation, PostgreSqlIdParser idParser)
            : base(
                  fromText, isManyToOneRelation ? 3 : 2, fqForeignTable + " [" + currentTableAlias + "]",
                  string.Format(Properties.Text.relation_type, isManyToOneRelation ? "n:1" : "1:n"),
                  fromText + (conditionInWhere ? " WHERE " : " ON ") + conditionText)
        {
            _AstNode = n;
            _ConditionText = conditionText;
            _CondicionEnWhere = conditionInWhere;
            _IdParser = idParser;
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (Font == null) Font = Parent.PreselectedFont;

            if (fragmentText == "") return CompareResult.Visible;

            string cleanId = _IdParser.Sql2CleanDefinition(fragmentText);

            if (MenuText.StartsWith(cleanId, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            if (MenuText.Contains(cleanId))
                return CompareResult.Visible;

            return CompareResult.Hidden;
        }

        protected override void DoAutocomplete(FastColoredTextBoxNS.Range fragment)
        {
            AstNode nFromItem = _AstNode.Parent!.Parent!.Parent!;
            AstNode nFromItemList = nFromItem.Parent!;

            Place pIniFrom;
            Place pFinFrom;

            if (nFromItemList.Name == "join")
            {
                pIniFrom = new Place(nFromItem.StartColumn, nFromItem.StartLine);
                pFinFrom = new Place(nFromItemList.EndColumn, nFromItemList.EndLine);
                nFromItemList = nFromItemList.Parent!.Parent!;
            }
            else
            {
                pIniFrom = new Place(nFromItem.StartColumn, nFromItem.StartLine);
                pFinFrom = new Place(nFromItem.EndColumn, nFromItem.EndLine);
            }

            if (nFromItemList.Name != "fromItemList") throw new Exception();

            string textoFrom = Text;
            string? textoWhere = null;
            Place? pCondicionWhere = null;

            if (_CondicionEnWhere)
            {
                AstNode fromOrUsingClauseOpt = nFromItemList.Parent!;
                AstNode? whereClauseOpt = fromOrUsingClauseOpt.Parent!["whereClauseOpt"];

                if (whereClauseOpt == null)
                {
                    pCondicionWhere = new Place(fromOrUsingClauseOpt.Parent.EndColumn, fromOrUsingClauseOpt.Parent.EndLine);
                    textoWhere = " WHERE " + _ConditionText + " ";
                }
                else
                {
                    AstNode nWhere = whereClauseOpt["WHERE"]!;
                    pCondicionWhere = new Place(nWhere.EndColumn, nWhere.EndLine);
                    textoWhere = " " + _ConditionText + " AND";
                }
            }
            else
            {
                textoFrom += " ON " + _ConditionText;
            }

            var tb = fragment.tb;

            tb.BeginAutoUndo();
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            if (textoWhere != null && pCondicionWhere != null)
            {
                tb.Selection.Start = pCondicionWhere.Value;
                tb.Selection.End = pCondicionWhere.Value;
                tb.InsertText(textoWhere);
                tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));
            }

            tb.Selection.Start = pIniFrom;
            tb.Selection.End = pFinFrom;
            tb.InsertText(textoFrom);
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            tb.EndAutoUndo();
            tb.Focus();
        }
    }
}

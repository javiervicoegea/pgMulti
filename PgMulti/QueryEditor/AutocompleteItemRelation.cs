using FastColoredTextBoxNS;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemRelation : AutocompleteItemCustom
    {
        private AstNode _AstNode;
        private string _ConditionText;
        private bool _IsConditionInWhere;
        private PostgreSqlIdParser _IdParser;

        private string _AutoCompleteTextPattern;
        private string _DefaultTableAlias;
        private string _FqForeignTable;

        public AutocompleteItemRelation(AstNode n, string currentTableAlias, string fqForeignTable, string autoCompleteTextPattern, string defaultTableAlias, string conditionText, bool isConditionInWhere, bool isManyToOneRelation, PostgreSqlIdParser idParser)
            : base(
                  string.Format(autoCompleteTextPattern, PostgreSqlGrammar.IdToString(defaultTableAlias)), isManyToOneRelation ? 3 : 2, fqForeignTable + " [" + currentTableAlias + "]",
                  string.Format(Properties.Text.relation_type, isManyToOneRelation ? "n:1" : "1:n"),
                  string.Format(autoCompleteTextPattern, PostgreSqlGrammar.IdToString(defaultTableAlias)) + (isConditionInWhere ? " WHERE " : " ON ") + string.Format(conditionText, PostgreSqlGrammar.IdToString(defaultTableAlias)))
        {
            _AstNode = n;
            _ConditionText = conditionText;
            _IsConditionInWhere = isConditionInWhere;
            _IdParser = idParser;

            _AutoCompleteTextPattern = autoCompleteTextPattern;
            _DefaultTableAlias = defaultTableAlias;
            _FqForeignTable = fqForeignTable;
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

            Place pStartFrom;
            Place pEndFrom;

            if (nFromItemList.Name == "join")
            {
                pStartFrom = new Place(nFromItem.StartColumn, nFromItem.StartLine);
                pEndFrom = new Place(nFromItemList.EndColumn, nFromItemList.EndLine);
                nFromItemList = nFromItemList.Parent!.Parent!;
            }
            else
            {
                pStartFrom = new Place(nFromItem.StartColumn, nFromItem.StartLine);
                pEndFrom = new Place(nFromItem.EndColumn, nFromItem.EndLine);
            }

            if (nFromItemList.Name != "fromItemList") throw new Exception();

            string fromText = _AutoCompleteTextPattern;
            string? whereText = null;
            Place? pWhereCondition = null;

            if (_IsConditionInWhere)
            {
                AstNode fromOrUsingClauseOpt = nFromItemList.Parent!;
                AstNode? whereClauseOpt = fromOrUsingClauseOpt.Parent!["whereClauseOpt"];

                if (whereClauseOpt == null)
                {
                    pWhereCondition = new Place(fromOrUsingClauseOpt.Parent.EndColumn, fromOrUsingClauseOpt.Parent.EndLine);
                    whereText = " WHERE " + _ConditionText + " ";
                }
                else
                {
                    AstNode nWhere = whereClauseOpt["WHERE"]!;
                    pWhereCondition = new Place(nWhere.EndColumn, nWhere.EndLine);
                    whereText = " " + _ConditionText + " AND";
                }
            }
            else
            {
                fromText += " ON " + _ConditionText;
            }

            Parent.Close();
            InputTableAliasForm f = new InputTableAliasForm(_FqForeignTable, _DefaultTableAlias, new Point(Parent.Left, Parent.Top));
            f.ShowDialog(Parent.FastColoredTextBox.ParentForm);

            if (f.DialogResult != DialogResult.OK) return;

            string tableAlias = f.TableAlias;

            whereText = whereText == null ? null : string.Format(whereText, PostgreSqlGrammar.IdToString(tableAlias));
            fromText = string.Format(fromText, PostgreSqlGrammar.IdToString(tableAlias));

            var tb = fragment.tb;

            tb.BeginAutoUndo();
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            if (whereText != null && pWhereCondition != null)
            {
                tb.Selection.Start = pWhereCondition.Value;
                tb.Selection.End = pWhereCondition.Value;
                tb.InsertText(whereText);
                tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));
            }

            tb.Selection.Start = pStartFrom;
            tb.Selection.End = pEndFrom;
            tb.InsertText(fromText);
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));

            tb.EndAutoUndo();
            tb.Focus();
        }
    }
}

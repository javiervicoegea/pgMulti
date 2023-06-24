using FastColoredTextBoxNS;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemTip : AutocompleteItemCustom
    {
        public AutocompleteItemTip(string text, string menuText, int imageIndex, string? tooltipTitle = null, string? tooltip = null)
            : base(text, imageIndex, menuText, tooltipTitle, tooltip)
        {
            NeverAutoSelectOnSymbol = true;
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (Font == null) Font = Parent.TipFont;

            return CompareResult.VisibleAndSelected;
        }
    }
}

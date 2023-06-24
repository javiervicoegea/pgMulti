using FastColoredTextBoxNS;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemCustom : AutocompleteItem
    {
        //public bool NoAutoSelectIfEmpty = false;
        public bool NeverAutoSelectOnSymbol = false;

        public AutocompleteItemCustom(string text, int imageIndex, string menuText, string? tooltipTitle, string? tooltip)
            : base(text, imageIndex, menuText, tooltipTitle, tooltip)
        {
        }
    }
}

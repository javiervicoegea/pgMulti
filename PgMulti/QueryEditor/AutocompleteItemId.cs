using FastColoredTextBoxNS;

namespace PgMulti.QueryEditor
{
    public class AutocompleteItemId : AutocompleteItemCustom
    {
        string _Id;
        string? _FirstPart = null;

        public AutocompleteItemId(string id, int imageIndex, bool autoSelectOnSymbol, Font f, string? tooltipTitle = null, string? tooltip = null)
            : base(id, imageIndex, id, tooltipTitle, tooltip)
        {
            Font = f;
            _Id = id.ToLower();
            //NoAutoSelectIfEmpty = !autoSelectOnSymbol;
        }

        public override CompareResult Compare(string fragmentText)
        {
            int i = fragmentText.LastIndexOf('.');

            string lastPart;
            if (i == -1)
            {
                _FirstPart = null;
                lastPart = fragmentText.ToLower();
            }
            else
            {
                _FirstPart = fragmentText.Substring(0, i);
                lastPart = fragmentText.Substring(i + 1).ToLower();
            }

            if (lastPart == "") return CompareResult.Visible;
            if (_Id.StartsWith(lastPart, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            if (_Id.Contains(lastPart.ToLower()))
                return CompareResult.Visible;

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            if (_FirstPart == null)
            {
                return Text;
            }
            else
            {
                return _FirstPart + "." + Text;
            }
        }
    }
}

using PgMulti.QueryEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Forms
{
    public interface IEditorTabForm
    {
        public TextBox txtSearchText { get; }
        public TextBox txtReplaceText { get; }
        public CheckBox chkSearchMatchCase { get; }
        public CheckBox chkSearchMatchWholeWords { get; }
        public CheckBox chkSearchRegex { get; }
        public CheckBox chkSearchWithinSelectedText { get; }
        public Button btnGoNextSearchResult { get; }
        public Button btnReplaceCurrent { get; }
        public Button btnReplaceAll { get; }
        public Label lblSearchResultsSummary { get; }
        public ToolStripDropDownButton tsddbErrors { get; }

        public void ShowSearchAndReplace();
        public void HideSearchAndReplace();
        public bool IsCurrentFctbTab(CustomFctb fctb);
  }
}

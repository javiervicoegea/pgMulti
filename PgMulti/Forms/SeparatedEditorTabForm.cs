using FastColoredTextBoxNS;
using PgMulti.AppData;
using PgMulti.Forms;
using PgMulti.QueryEditor;
using System.Globalization;
using System.Windows.Forms;

namespace PgMulti
{
    public partial class SeparatedEditorTabForm : Form, IEditorTabForm
    {
        private EditorTab _EditorTab;

        public SeparatedEditorTabForm(EditorTab et)
        {
            InitializeComponent();
            InitializeText();

            _EditorTab = et;

            Text = et.TabPage.Text;
            toolStripContainer.ContentPanel.Controls.Add(_EditorTab.Fctb);
            toolStripContainer.ContentPanel.Controls.Add(_EditorTab.Fctb.HScrollBar);
            toolStripContainer.ContentPanel.Controls.Add(_EditorTab.Fctb.VScrollBar);
        }

        TextBox IEditorTabForm.txtSearchText => txtSearchText;
        TextBox IEditorTabForm.txtReplaceText => txtReplaceText;
        CheckBox IEditorTabForm.chkSearchMatchCase => chkSearchMatchCase;
        CheckBox IEditorTabForm.chkSearchMatchWholeWords => chkSearchMatchWholeWords;
        CheckBox IEditorTabForm.chkSearchRegex => chkSearchRegex;
        CheckBox IEditorTabForm.chkSearchWithinSelectedText => chkSearchWithinSelectedText;
        Button IEditorTabForm.btnGoNextSearchResult => btnGoNextSearchResult;
        Button IEditorTabForm.btnReplaceCurrent => btnReplaceCurrent;
        Button IEditorTabForm.btnReplaceAll => btnReplaceAll;
        Label IEditorTabForm.lblSearchResultsSummary => lblSearchResultsSummary;
        ToolStripDropDownButton IEditorTabForm.tsddbErrors => tsddbErrors;

        public void RefreshPosition()
        {
            Place p = _EditorTab.Fctb.PositionToPlace(_EditorTab.Fctb.SelectionStart);
            tslPosition.Text = string.Format(Properties.Text.line_column, p.iLine + 1, p.iChar + 1);
        }

        public void ShowSearchAndReplace()
        {
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel1.Show();
            txtSearchText.Focus();
            txtSearchText.SelectionStart = 0;
            txtSearchText.SelectionLength = txtSearchText.Text.Length;

            if (_EditorTab.Fctb.Selection.Length > 0)
            {
                txtSearchText.SelectedText = _EditorTab.Fctb.SelectedText;
            }
            tsbSearchAndReplace.Checked = true;
        }

        public void HideSearchAndReplace()
        {
            txtSearchText.Text = "";
            splitContainer.Panel1Collapsed = true;
            splitContainer.Panel1.Hide();

            if (_EditorTab.Fctb.SearchRange != null)
            {
                _EditorTab.Fctb.SearchRange = null;
            }

            _EditorTab.UpdateSearchResults();
            _EditorTab.Fctb.DoHighlighting();
            _EditorTab.Fctb.Focus();
            tsbSearchAndReplace.Checked = false;
        }

        public bool IsCurrentFctbTab(CustomFctb fctb)
        {
            return _EditorTab.Fctb == fctb;
        }

        private void SeparatedEditorTabForm_Load(object sender, EventArgs e)
        {
            Width = (Screen.PrimaryScreen!.Bounds.Width * 2) / 3;
            Height = (Screen.PrimaryScreen!.Bounds.Height * 2) / 3;
            Left = (Screen.PrimaryScreen!.Bounds.Width - Width) / 2;
            Top = (Screen.PrimaryScreen!.Bounds.Height - Height) / 2;
        }

        private void CustomFctbForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _EditorTab.ReturnToMainForm();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            _EditorTab.ShowSave(this);
        }

        private void tsbSaveAs_Click(object sender, EventArgs e)
        {
            _EditorTab.ShowSaveAs(this);
        }

        private void tsbSearchAndReplace_Click(object sender, EventArgs e)
        {
            if (tsbSearchAndReplace.Checked)
            {
                ShowSearchAndReplace();
            }
            else
            {
                HideSearchAndReplace();
            }
        }

        private void tsbGoTo_Click(object sender, EventArgs e)
        {
            _EditorTab.Fctb.ShowGoToDialog();
        }

        private void tsbFormat_Click(object? sender, EventArgs? e)
        {
            _EditorTab.Format();
        }

        private void txtSearchText_TextChanged(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
        }

        private void chkSearchMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
        }

        private void chkSearchMatchWholeWords_CheckedChanged(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
        }

        private void chkSearchRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
        }

        private void chkSearchWithinSelectedText_CheckedChanged(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchRange() | _EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
            btnUpdateSearchSelectedText.Visible = chkSearchWithinSelectedText.Checked;
        }

        private void txtSearchText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnGoNextSearchResult_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Escape)
            {
                HideSearchAndReplace();
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _EditorTab.UpdateSearchResults();
            _EditorTab.Fctb.DoHighlighting();

            if (_EditorTab.Fctb.SearchMatches != null && _EditorTab.Fctb.SearchMatches.Count > 0)
            {
                _EditorTab.GoNextSearchResult();
            }
        }

        private void btnGoNextSearchResult_Click(object sender, EventArgs e)
        {
            _EditorTab.GoNextSearchResult();
        }

        private void btnUpdateSearchSelectedText_Click(object sender, EventArgs e)
        {
            if (_EditorTab.UpdateSearchRange() | _EditorTab.UpdateSearchResults())
            {
                _EditorTab.Fctb.DoHighlighting();
            }
        }

        private void btnReplaceCurrent_Click(object sender, EventArgs e)
        {
            _EditorTab.ReplaceCurrent();
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            _EditorTab.ReplaceAll();
        }


        #region TextI18n
        private void InitializeText()
        {
            this.tsbSave.Text = Properties.Text.save;
            this.tsbSaveAs.Text = Properties.Text.save_as;
            this.tsbSearchAndReplace.Text = Properties.Text.search_for_and_replace;
            this.tsbGoTo.Text = Properties.Text.goto_sc;
            this.tsbFormat.Text = Properties.Text.format_sc;
            this.tsddbErrors.Text = Properties.Text.no_errors;
            this.lblSearch.Text = Properties.Text.search_text;
            this.lblReplace.Text = Properties.Text.replace_text;
            this.chkSearchMatchCase.Text = Properties.Text.match_case;
            this.chkSearchMatchWholeWords.Text = Properties.Text.match_whole_words;
            this.chkSearchRegex.Text = Properties.Text.regex;
            this.chkSearchWithinSelectedText.Text = Properties.Text.search_only_within_selected_text;
            this.btnSearch.Text = Properties.Text.search;
            this.btnGoNextSearchResult.Text = Properties.Text.go_next;
            this.btnUpdateSearchSelectedText.Text = Properties.Text.update_selected_text;
            this.btnReplaceCurrent.Text = Properties.Text.replace_current;
            this.btnReplaceAll.Text = Properties.Text.replace_all;
        }
        #endregion
    }
}

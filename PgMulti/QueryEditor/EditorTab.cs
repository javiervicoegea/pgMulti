using FastColoredTextBoxNS;
using PgMulti.DataAccess;
using Microsoft.Data.Sqlite;
using System.Data;
using PgMulti.Tasks;
using PgMulti.AppData;
using System.Windows.Forms;
using Irony.Parsing;
using PgMulti.SqlSyntax;
using System.Text;
using PgMulti.Forms;

namespace PgMulti.QueryEditor
{
    public class EditorTab
    {
        public bool PendingSaveDB = true;
        public string? LocalPath;

        private const string _AutoSelectSymbols = "()=/&%$!*;:><[]{}@ .,-+";

        private short _Position;
        private int _Id = -1;

        private Data _Data;
        private MainForm _MainForm;
        private TabPage _TabPage;
        private CustomFctb _Fctb;
        private AutocompleteMenu _AutocompleteMenu;

        private SeparatedEditorTabForm? _SeparatedEditorTabForm;
        private Button? _ReturnButton;

        private List<PgTask> _Tasks = new List<PgTask>();

        public EditorTab(Data d, MainForm mainForm, CreateEditorTabOptions o) : this(d, mainForm, false)
        {
            if (o.Text != null)
            {
                _Fctb.Text = o.Text;
            }

            if (o.Format)
            {
                Format();
            }

            if (o.Title != null)
            {
                _TabPage.Text = o.Title;
            }
            if (o.Path != null)
            {
                LocalPath = o.Path;
                _TabPage.ToolTipText = LocalPath;
            }

            PendingSaveDB = true;
            _MainForm.EnableTimerSaveTabs();

            InitEvents();
            if (o.Text != null) _Fctb.DoHighlighting();
            if (o.PendingFileSave) SetPendingFileSave();
        }

        public EditorTab(Data d, DataRow dr, MainForm frmPrincipal) : this(d, frmPrincipal, false)
        {
            LocalPath = dr.Field<string?>("path");
            _Position = (short)dr.Field<long>("position");
            _Id = (int)dr.Field<long>("id");
            _Fctb.Text = dr.Field<string>("text");
            _Fctb.ClearUndo();
            _TabPage.Text = dr.Field<string>("name");
            _TabPage.ToolTipText = LocalPath;

            PendingSaveDB = false;

            InitEvents();
            _Fctb.DoHighlighting();
        }

        public EditorTab(Data d, ClosedEditorTab cet, MainForm frmPrincipal, int position) : this(d, frmPrincipal, false)
        {
            _Id = cet.Id;
            LocalPath = cet.Path;
            _Fctb.Text = cet.Text;
            _TabPage.Text = cet.Name;
            _TabPage.ToolTipText = LocalPath;

            cet.Reopen(position);

            PendingSaveDB = false;

            InitEvents();
            _Fctb.DoHighlighting();
        }

        private EditorTab(Data d, MainForm mainForm, bool _)
        {
            _Data = d;
            _MainForm = mainForm;

            _Fctb = new CustomFctb(true);
            _Fctb.SetParser(_Data!.PGLanguageData);
            _Fctb.CaretBlinking = false;
            _Fctb.AutoScrollMinSize = new Size(669, 645);
            _Fctb.BackBrush = null;
            _Fctb.CharHeight = 15;
            _Fctb.CharWidth = 7;
            _Fctb.Cursor = Cursors.IBeam;
            _Fctb.DisabledColor = Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            _Fctb.Dock = DockStyle.Fill;
            _Fctb.Font = new Font("Cascadia Code", _Data!.Config.FontSize, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            _Fctb.IsReplaceMode = false;
            _Fctb.Location = new Point(0, 0);
            _Fctb.Name = "_Fctb";
            _Fctb.SelectionColor = Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            _Fctb.Size = new Size(668, 380);
            _Fctb.TabIndex = 0;
            _Fctb.Text = "";
            _Fctb.Zoom = 100;
            _Fctb.AcceptsTab = true;
            _Fctb.AcceptsReturn = true;
            _Fctb.AutoIndent = true;
            _Fctb.AutoIndentExistingLines = false;
            _Fctb.Paddings = new Padding(20);
            _Fctb.AutoCompleteBrackets = true;
            _Fctb.AutoIndentChars = false;


            _TabPage = new TabPage(Properties.Text.new_doc_title);

            _MainForm.SqlEditorTabControl.SuspendDrawing();
            _TabPage.Controls.Add(_Fctb);
            _TabPage.Controls.Add(_Fctb.HScrollBar);
            _TabPage.Controls.Add(_Fctb.VScrollBar);
            _MainForm.SqlEditorTabControl.TabPages.Insert(_MainForm.SqlEditorTabControl.TabPages.Count - 1, _TabPage);
            _MainForm.SqlEditorTabControl.ResumeDrawing();

            _TabPage.Tag = this;

            _AutocompleteMenu = new AutocompleteMenu(_Fctb);
            _AutocompleteMenu.ForeColor = Color.FromArgb(30, 30, 30);
            _AutocompleteMenu.BackColor = Color.FromArgb(247, 249, 254);
            _AutocompleteMenu.SelectedColor = Color.FromArgb(196, 213, 255);
            _AutocompleteMenu.SearchPattern = @"[\w\.\""]";
            _AutocompleteMenu.AllowTabKey = true;
            _AutocompleteMenu.Items.SetAutocompleteItems(new PGAutocompleteEnumerable(_AutocompleteMenu, _Fctb, mainForm, _Data.PGLanguageData, _Data.PGSimpleLanguageData));
            _AutocompleteMenu.MinFragmentLength = 0;
            _AutocompleteMenu.ImageList = mainForm.ilAutocompleteMenu;
            _AutocompleteMenu.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            _AutocompleteMenu.TipFont = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            _AutocompleteMenu.PreselectedFont = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            _AutocompleteMenu.MinimumSize = new Size(400, 100);
            _AutocompleteMenu.MaximumSize = new Size(500, 400);
            _AutocompleteMenu.MaxTooltipWidth = 500;
            _AutocompleteMenu.ToolTipDuration = 30000;
            _AutocompleteMenu.AppearInterval = (_Data.Config.AutocompleteDelay == 0 ? int.MaxValue : _Data!.Config.AutocompleteDelay);
        }

        public List<PgTask> Tasks
        {
            get
            {
                return _Tasks;
            }
        }

        public IEditorTabForm EditorTabForm
        {
            get
            {
                if (_SeparatedEditorTabForm == null)
                {
                    return _MainForm;
                }
                else
                {
                    return _SeparatedEditorTabForm;
                }
            }
        }

        public bool ShownInSeparatedWindow
        {
            get
            {
                return _SeparatedEditorTabForm != null;
            }
        }

        private ContextMenuStrip? _CmsFctb = null;
        private ContextMenuStrip CmsFctb
        {
            get
            {
                if (_CmsFctb == null)
                {
                    _CmsFctb = CreateCmsFctb();
                }

                return _CmsFctb;
            }
        }

        public void OpenEditorInNewWindow()
        {
            _SeparatedEditorTabForm = new SeparatedEditorTabForm(this);

            _ReturnButton = new Button();
            _ReturnButton.Text = Properties.Text.return_editor_to_main_form;
            _ReturnButton.Dock = DockStyle.Fill;
            _ReturnButton.Margin = new Padding(100);
            _ReturnButton.BackColor = SystemColors.ControlDarkDark;
            _ReturnButton.ForeColor = SystemColors.ControlLightLight;
            _ReturnButton.Click += _ReturnButton_Click;
            TabPage.Controls.Add(_ReturnButton);

            _SeparatedEditorTabForm.Show();
            _MainForm.ShowTabAsInSeparatedWindow(true);
            _MainForm.SeparatedEditorTabForms.Add(_SeparatedEditorTabForm);
            Fctb.Focus();
        }

        public void ReturnToMainForm()
        {
            TabPage.Controls.Remove(_ReturnButton);
            TabPage.Controls.Add(Fctb);
            TabPage.Controls.Add(Fctb.HScrollBar);
            TabPage.Controls.Add(Fctb.VScrollBar);

            _MainForm.ShowTabAsInSeparatedWindow(false);
            _MainForm.SeparatedEditorTabForms.Remove(_SeparatedEditorTabForm!);
            _SeparatedEditorTabForm = null;
            _ReturnButton = null;
            Fctb.Focus();
        }
        public bool ShowSave(Form ownerForm)
        {
            if (LocalPath == null)
            {
                return ShowSaveAs(ownerForm);
            }
            else
            {
                try
                {
                    File.WriteAllText(LocalPath, Fctb.Text);
                    if (TabPage.Text.EndsWith(" *"))
                    {
                        TabPage.Text = TabPage.Text.Substring(0, TabPage.Text.Length - 2);

                        if (_SeparatedEditorTabForm != null) _SeparatedEditorTabForm.Text = TabPage.Text;
                    }
                    PendingSaveDB = true;
                    _MainForm.EnableTimerSaveTabs();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ownerForm, Properties.Text.error_saving_file + $":\r\n{LocalPath}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public bool ShowSaveAs(Form ownerForm)
        {
            SaveFileDialog sfdSql = new SaveFileDialog();
            sfdSql.DefaultExt = "sql";
            sfdSql.FilterIndex = 0;
            sfdSql.Filter = Properties.Text.sql_file_filter;
            sfdSql.Title = Properties.Text.select_save_file;
            sfdSql.FileName = TabPage.Text;
            if (sfdSql.FileName.EndsWith(" *")) sfdSql.FileName = sfdSql.FileName.Substring(0, sfdSql.FileName.Length - 2);
            if (!sfdSql.FileName.Contains(".")) sfdSql.FileName += ".sql";

            if (sfdSql.ShowDialog(ownerForm) != DialogResult.OK) return false;

            try
            {
                File.WriteAllText(sfdSql.FileName, Fctb.Text);
                SetFileName(sfdSql.FileName);
                PendingSaveDB = true;
                LocalPath = sfdSql.FileName;
                _MainForm.EnableTimerSaveTabs();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ownerForm, Properties.Text.error_saving_file + $":\r\n{sfdSql.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void Format()
        {
            Parser parserGlobal = new Parser(_Data!.PGSimpleLanguageData);

            string globalText;
            int prevIndentation = 0;
            if (string.IsNullOrEmpty(Fctb.SelectedText))
            {
                globalText = Fctb.Text;
            }
            else
            {
                int startLine;
                int endLine;

                if (Fctb.Selection.Start.iLine < Fctb.Selection.End.iLine)
                {
                    startLine = Fctb.Selection.Start.iLine;
                    endLine = Fctb.Selection.End.iLine;
                }
                else
                {
                    startLine = Fctb.Selection.End.iLine;
                    endLine = Fctb.Selection.Start.iLine;
                }

                Fctb.Selection.Start = new Place(0, startLine);
                Fctb.Selection.End = new Place(Fctb.Lines[endLine].Length, endLine);

                globalText = Fctb.SelectedText;

                foreach (char c in globalText)
                {
                    bool exitFor = false;
                    switch (c)
                    {
                        case ' ':
                            prevIndentation++;
                            break;
                        case '\t':
                            prevIndentation += 4;
                            break;
                        case '\r':
                        case '\n':
                            prevIndentation = 0;
                            break;
                        default:
                            exitFor = true;
                            break;
                    }

                    if (exitFor) break;
                }
            }

            if (string.IsNullOrWhiteSpace(globalText)) return;

            ParseTree parseTreeGlobal = parserGlobal.Parse(globalText);
            bool addedSemicolon = false;
            if (parseTreeGlobal.Status == ParseTreeStatus.Error)
            {
                globalText += "\r\n;";
                parseTreeGlobal = parserGlobal.Parse(globalText);
                addedSemicolon = true;
                if (parseTreeGlobal.Status == ParseTreeStatus.Error)
                {
                    return;
                }
            }


            AstNode globalRootAstNode = AstNode.ProcessParseTree(parseTreeGlobal);

            if (globalRootAstNode.Children.Count == 0) return;

            int pos = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < globalRootAstNode.Children[0].Children.Count; i++)
            {
                int end;

                if (i < globalRootAstNode.Children[0].Children.Count - 1)
                {
                    end = globalRootAstNode.Children[0].Children[i + 1].RecursiveTokens[0].Token!.Location.Position;
                }
                else
                {
                    end = globalText.Length;
                }

                string stmtText = globalText.Substring(pos, end - pos);

                Parser parserQuery = new Parser(_Data!.PGLanguageData);
                ParseTree parseTreeQuery = parserQuery.Parse(stmtText);
                if (parseTreeQuery.Status == ParseTreeStatus.Error)
                {
                    for (int j = 0; j < prevIndentation; j++)
                    {
                        sb.Append(" ");
                    }

                    if (addedSemicolon && i == globalRootAstNode.Children[0].Children.Count - 1)
                    {
                        stmtText = stmtText.Substring(0, stmtText.Length - 3);
                    }

                    sb.Append(stmtText);
                }
                else
                {
                    AstNode queryRootAstNode = AstNode.ProcessParseTree(parseTreeQuery);
                    queryRootAstNode.Format(sb, parseTreeQuery, prevIndentation);

                    sb.AppendLine();
                }

                pos = end;
            }

            string formattedText = sb.ToString();

            Fctb.BeginAutoUndo();

            if (string.IsNullOrEmpty(Fctb.SelectedText))
            {
                Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(Fctb.TextSource));
                Fctb.Selection.Start = new Place(0, 0);
                Fctb.Selection.End = new Place(Fctb.Lines[Fctb.LinesCount - 1].Length, Fctb.LinesCount - 1);
            }

            Fctb.InsertText(formattedText);
            Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(Fctb.TextSource));

            Fctb.EndAutoUndo();
            Fctb.Focus();
        }

        public bool UpdateSearchResults()
        {
            string pattern = EditorTabForm.txtSearchText.Text;

            List<FastColoredTextBoxNS.Range> matches;

            if (pattern == "")
            {
                matches = new List<FastColoredTextBoxNS.Range>();
            }
            else
            {
                matches = Fctb.FindAll(pattern, EditorTabForm.chkSearchMatchCase.Checked, EditorTabForm.chkSearchMatchWholeWords.Checked, EditorTabForm.chkSearchRegex.Checked);
            }

            if (
                    (matches.Count == 0 && Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0)
                    || (matches.Count > 0 && (Fctb.SearchMatches == null || !matches.SequenceEqual(Fctb.SearchMatches)))
                )
            {
                Fctb.SearchMatches = matches;
                UpdateSearchResultsSummary();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateSearchResultsSummary()
        {
            EditorTabForm.btnGoNextSearchResult.Enabled = Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0;
            EditorTabForm.btnReplaceCurrent.Enabled = Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0;
            EditorTabForm.btnReplaceAll.Enabled = Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0;
            EditorTabForm.lblSearchResultsSummary.Text = string.Format(Properties.Text.number_of_search_results_found, Fctb.SearchMatches == null ? 0 : Fctb.SearchMatches.Count) + "\r\n"
                + (Fctb.SearchRange == null ? Properties.Text.searching_the_entire_text : Properties.Text.searching_only_within_selected_text);
        }

        public bool UpdateSearchRange()
        {
            FastColoredTextBoxNS.Range? searchRange;

            if (EditorTabForm.chkSearchWithinSelectedText.Checked)
            {
                searchRange = Fctb.Selection.Clone();
                searchRange.Normalize();
            }
            else
            {
                searchRange = null;
            }

            if (Fctb.SearchRange != searchRange)
            {
                Fctb.SearchRange = searchRange;

                EditorTabForm.lblSearchResultsSummary.Text = string.Format(Properties.Text.number_of_search_results_found, Fctb.SearchMatches == null ? 0 : Fctb.SearchMatches.Count) + "\r\n"
                    + (searchRange == null ? Properties.Text.searching_the_entire_text : Properties.Text.searching_only_within_selected_text);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void GoNextSearchResult()
        {
            if (Fctb.SearchMatches == null || Fctb.SearchMatches.Count == 0) return;

            Place p = Fctb.Selection.End > Fctb.Selection.Start ? Fctb.Selection.End : Fctb.Selection.Start;
            FastColoredTextBoxNS.Range? r = Fctb.SearchMatches!.FirstOrDefault(ri => ri.Start > p);
            if (r == null)
            {
                r = Fctb.SearchMatches!.First();
            }

            Fctb.Selection = r;
            Fctb.DoSelectionVisible();
            Fctb.Focus();
            Fctb.Invalidate();
            return;
        }

        public void ReplaceCurrent()
        {
            if (Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0)
            {
                if (Fctb.Selection.Length > 0 && Fctb.SearchMatches.Any(sm => Fctb.Selection.Equals(sm)))
                {
                    string replaceText = EditorTabForm.txtReplaceText.Text;
                    if (EditorTabForm.chkSearchRegex.Checked)
                    {
                        System.Text.RegularExpressions.Regex r = Fctb.GetRegex(EditorTabForm.txtSearchText.Text, EditorTabForm.chkSearchMatchCase.Checked, EditorTabForm.chkSearchMatchWholeWords.Checked, EditorTabForm.chkSearchRegex.Checked);

                        replaceText = r.Replace(Fctb.SelectedText, replaceText);
                    }

                    Fctb.InsertText(replaceText);

                    if (UpdateSearchResults())
                    {
                        Fctb.DoHighlighting();
                    }

                    if (Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0)
                    {
                        GoNextSearchResult();
                    }
                    else
                    {
                        Fctb.Focus();
                    }
                }
                else
                {
                    GoNextSearchResult();
                }
            }
        }

        public void ReplaceAll()
        {
            if (Fctb.SearchMatches != null && Fctb.SearchMatches.Count > 0)
            {
                Fctb.Selection.BeginUpdate();
                Fctb.BeginAutoUndo();
                try
                {
                    if (EditorTabForm.chkSearchRegex.Checked)
                    {
                        System.Text.RegularExpressions.Regex r = Fctb.GetRegex(EditorTabForm.txtSearchText.Text, EditorTabForm.chkSearchMatchCase.Checked, EditorTabForm.chkSearchMatchWholeWords.Checked, EditorTabForm.chkSearchRegex.Checked);

                        for (int i = Fctb.SearchMatches.Count - 1; i >= 0; i--)
                        {
                            FastColoredTextBoxNS.Range ri = Fctb.SearchMatches[i];

                            string replaceText = r.Replace(ri.Text, EditorTabForm.txtReplaceText.Text);

                            Fctb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(Fctb.TextSource, new List<FastColoredTextBoxNS.Range> { ri }, replaceText));
                        }
                    }
                    else
                    {
                        Fctb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(Fctb.TextSource, Fctb.SearchMatches, EditorTabForm.txtReplaceText.Text));
                    }


                    if (Fctb.SearchRange == null)
                    {
                        Fctb.Selection.Start = new Place(0, 0);
                    }
                    else
                    {
                        Fctb.Selection.Start = Fctb.SearchRange.Start;
                    }
                }
                finally
                {
                    Fctb.EndAutoUndo();
                    Fctb.Selection.EndUpdate();
                }

                Fctb.DoSelectionVisible();
                Fctb.Focus();
                Fctb.Invalidate();

                if (UpdateSearchResults())
                {
                    Fctb.DoHighlighting();
                }
            }
        }

        public void RefreshErrors()
        {
            EditorTabForm.tsddbErrors.DropDownItems.Clear();
            if (Fctb.ParseTree != null && Fctb.ParseTree.Status == ParseTreeStatus.Error)
            {
                foreach (Irony.LogMessage msg in Fctb.ParseTree.ParserMessages)
                {
                    ToolStripMenuItem tsmiError = new ToolStripMenuItem();

                    tsmiError.Image = Properties.Resources.error;
                    tsmiError.Text = string.Format(Properties.Text.line_column, msg.Location.Line + 1, msg.Location.Column + 1) + ": " + msg.Message;
                    tsmiError.Click += new EventHandler(tsmiError_Click);
                    tsmiError.Tag = new Tuple<CustomFctb, Irony.LogMessage>(Fctb, msg);
                    EditorTabForm.tsddbErrors.DropDownItems.Add(tsmiError);
                }

                EditorTabForm.tsddbErrors.Text = string.Format(Properties.Text.error_count, Fctb.ParseTree.ParserMessages.Count);
                EditorTabForm.tsddbErrors.Image = Properties.Resources.error;
                EditorTabForm.tsddbErrors.Enabled = true;
            }
            else
            {
                EditorTabForm.tsddbErrors.Text = Properties.Text.no_errors;
                EditorTabForm.tsddbErrors.Image = Properties.Resources.ok;
                EditorTabForm.tsddbErrors.Enabled = false;
            }
        }

        public bool Close(bool force)
        {
            if (_SeparatedEditorTabForm != null)
            {
                ReturnToMainForm();
                if (!force) return false;
            }

            if (string.IsNullOrWhiteSpace(Fctb.Text))
            {
                Delete();
            }
            else
            {
                Save();
                SetClosed();
                _Data!.DeleteOldestTabs();
            }

            _MainForm.CloseTabComplete(this);

            return true;
        }

        private void InitEvents()
        {
            _Fctb.ResetTextChangedDelayedEvent();

            _Fctb.TextChangedDelayed += fctbSql_TextChangedDelayed;
            _Fctb.Enter += fctbSql_Enter;
            _Fctb.Leave += fctbSql_Leave;
            _Fctb.KeyDown += fctbSql_KeyDown;
            _Fctb.MouseUp += fctbSql_MouseUp;
            _Fctb.ParseTreeUpdated += fctbSql_ParseTreeUpdated;
            _Fctb.SecondaryFormShowed += fctbSql_SecondaryFormShowed;
            _Fctb.SecondaryFormClosed += fctbSql_SecondaryFormClosed;
            _Fctb.AutoIndentNeeded += fctbSql_AutoIndentNeeded;
            _Fctb.KeyPressing += fctbSql_KeyPressing;

            _AutocompleteMenu.ProcessKeyDown += _AutocompleteMenu_ProcessKeyDown;
            _AutocompleteMenu.ProcessKeyPressing += _AutocompleteMenu_ProcessKeyPressing;
        }

        private ContextMenuStrip CreateCmsFctb()
        {
            ToolStripMenuItem tscmiBack = new ToolStripMenuItem();
            ToolStripMenuItem tscmiForward = new ToolStripMenuItem();
            ToolStripMenuItem tscmiUndo = new ToolStripMenuItem();
            ToolStripMenuItem tscmiRedo = new ToolStripMenuItem();
            ToolStripMenuItem tscmiCut = new ToolStripMenuItem();
            ToolStripMenuItem tscmiCopy = new ToolStripMenuItem();
            ToolStripMenuItem tscmiPaste = new ToolStripMenuItem();
            ToolStripMenuItem tscmiSearchAndReplace = new ToolStripMenuItem();
            ToolStripMenuItem tscmiGoTo = new ToolStripMenuItem();
            ToolStripMenuItem tscmiFormat = new ToolStripMenuItem();

            // 
            // tscmiBack
            // 
            tscmiBack.Image = Properties.Resources.atras;
            tscmiBack.Name = "tscmiBack";
            tscmiBack.Size = new Size(73, 26);
            tscmiBack.Click += tsmiBack_Click;
            // 
            // tscmiForward
            // 
            tscmiForward.Image = Properties.Resources.adelante;
            tscmiForward.Name = "tscmiForward";
            tscmiForward.Size = new Size(73, 26);
            tscmiForward.Click += tsmiForward_Click;
            // 
            // tscmiUndo
            // 
            tscmiUndo.Image = Properties.Resources.undo;
            tscmiUndo.Name = "tscmiUndo";
            tscmiUndo.Size = new Size(73, 26);
            tscmiUndo.Click += tsmiUndo_Click;
            // 
            // tscmiRedo
            // 
            tscmiRedo.Image = Properties.Resources.redo;
            tscmiRedo.Name = "tscmiRedo";
            tscmiRedo.Size = new Size(73, 26);
            tscmiRedo.Click += tsmiRedo_Click;
            // 
            // tscmiCut
            // 
            tscmiCut.Image = Properties.Resources.cortar;
            tscmiCut.Name = "tscmiCut";
            tscmiCut.Size = new Size(73, 26);
            tscmiCut.Click += tsmiCut_Click;
            // 
            // tscmiCopy
            // 
            tscmiCopy.Image = Properties.Resources.copiar;
            tscmiCopy.Name = "tscmiCopy";
            tscmiCopy.Size = new Size(73, 26);
            tscmiCopy.Click += tsmiCopy_Click;
            // 
            // tscmiPaste
            // 
            tscmiPaste.Image = Properties.Resources.pegar;
            tscmiPaste.Name = "tscmiPaste";
            tscmiPaste.Size = new Size(73, 26);
            tscmiPaste.Click += tsmiPaste_Click;
            // 
            // tscmiSearchAndReplace
            // 
            tscmiSearchAndReplace.Image = Properties.Resources.buscar;
            tscmiSearchAndReplace.Name = "tscmiSearchAndReplace";
            tscmiSearchAndReplace.Size = new Size(73, 26);
            tscmiSearchAndReplace.Click += tsmiSearchAndReplace_Click;
            // 
            // tscmiGoTo
            // 
            tscmiGoTo.Image = Properties.Resources.linea;
            tscmiGoTo.Name = "tscmiGoTo";
            tscmiGoTo.Size = new Size(73, 26);
            tscmiGoTo.Click += tsmiGoTo_Click;
            // 
            // tscmiFormat
            // 
            tscmiFormat.Image = Properties.Resources.autoformato;
            tscmiFormat.Name = "tscmiFormat";
            tscmiFormat.Size = new Size(73, 26);
            tscmiFormat.Click += tsmiFormat_Click;

            tscmiBack.Text = Properties.Text.back;
            tscmiForward.Text = Properties.Text.forward;
            tscmiUndo.Text = Properties.Text.undo_sc;
            tscmiRedo.Text = Properties.Text.redo_sc;
            tscmiCut.Text = Properties.Text.cut_sc;
            tscmiCopy.Text = Properties.Text.copy_sc;
            tscmiPaste.Text = Properties.Text.paste_sc;
            tscmiSearchAndReplace.Text = Properties.Text.search_for_and_replace;
            tscmiGoTo.Text = Properties.Text.goto_sc;
            tscmiFormat.Text = Properties.Text.format_sc;

            ContextMenuStrip cmsFctb = new ContextMenuStrip();
            cmsFctb.SuspendLayout();

            cmsFctb.ImageScalingSize = new Size(20, 20);
            cmsFctb.Items.AddRange(new ToolStripItem[] { tscmiBack, tscmiForward, tscmiUndo, tscmiRedo, tscmiCut, tscmiCopy, tscmiPaste, tscmiSearchAndReplace, tscmiGoTo, tscmiFormat });
            cmsFctb.Name = "cmsFctb";
            cmsFctb.Size = new Size(74, 264);

            cmsFctb.ResumeLayout(false);

            return cmsFctb;
        }

        private void SetPendingFileSave()
        {
            if (!TabPage.Text.EndsWith(" *")) TabPage.Text += " *";

            if (_SeparatedEditorTabForm != null) _SeparatedEditorTabForm.Text = TabPage.Text;

            if (!_MainForm.Text.EndsWith(" *")) _MainForm.Text += " *";
            PendingSaveDB = true;
            _MainForm.EnableTimerSaveTabs();

            UpdateSearchResults();
        }

        private void _ReturnButton_Click(object? sender, EventArgs e)
        {
            ReturnToMainForm();
        }

        private void _AutocompleteMenu_ProcessKeyDown(object? sender, ProcessKeyDownEventArgs e)
        {
            string fragmentText = _AutocompleteMenu.Fragment.Text.ToLower();

            if (fragmentText.Length == 0 && !e.ExplicitlySelected)
            {
                e.Select = false;
                return;
            }

            string itemText = e.Item.MenuText.ToLower();

            int pos = fragmentText.IndexOf('.');
            if (pos != -1) fragmentText = fragmentText.Substring(pos + 1);

            if (!itemText.StartsWith(fragmentText))
            {
                e.Select = false;
                return;
            }

            if (e.KeyData == Keys.Enter)
            {
                e.Select = true;
                e.Handled = true;
                return;
            }
        }

        private void _AutocompleteMenu_ProcessKeyPressing(object? sender, ProcessKeyPressingEventArgs e)
        {
            string fragmentText = _AutocompleteMenu.Fragment.Text.ToLower();

            if (fragmentText.Length == 0 && !e.ExplicitlySelected)
            {
                e.Select = false;
                return;
            }

            string itemText = e.Item.MenuText.ToLower();

            int pos = fragmentText.IndexOf('.');
            if (pos != -1) fragmentText = fragmentText.Substring(pos + 1);

            if (!itemText.StartsWith(fragmentText))
            {
                e.Select = false;
                return;
            }

            if (!(e.Item is AutocompleteItemCustom) || ((AutocompleteItemCustom)e.Item).NeverAutoSelectOnSymbol)
            {
                e.Select = false;
                return;
            }

            if (e.KeyChar == '*' && fragmentText.Length == 0)
            {
                e.Select = false;
                return;
            }

            e.Select = _AutoSelectSymbols.Contains(e.KeyChar) && !itemText.Contains(e.KeyChar);
        }

        public TabPage TabPage
        {
            get
            {
                return _TabPage;
            }
        }

        public short Position
        {
            get
            {
                return _Position;
            }
            set
            {
                if (_Position == value) return;
                PendingSaveDB = true;
                _Position = value;
            }
        }

        public int Id { get => _Id; }

        public AutocompleteMenu AutocompleteMenu
        {
            get
            {
                return _AutocompleteMenu;
            }
        }

        public CustomFctb Fctb
        {
            get
            {
                return _Fctb;
            }
        }

        public void Save()
        {
            if (!PendingSaveDB) return;

            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();

                if (Id == -1)
                {
                    cmd.CommandText = "INSERT INTO editortabs (position,text,name,path) VALUES (:position,:text,:name,:path) RETURNING id";

                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("text", Fctb.Text);
                    cmd.Parameters.AddWithValue("name", _TabPage.Text);
                    cmd.Parameters.AddWithValue("path", LocalPath == null ? DBNull.Value : LocalPath);

                    _Id = (int)(long)cmd.ExecuteScalar()!;
                }
                else
                {
                    cmd.CommandText = "UPDATE editortabs SET position=:position,text=:text,name=:name,path=:path WHERE id=:id";

                    cmd.Parameters.AddWithValue("position", Position);
                    cmd.Parameters.AddWithValue("text", Fctb.Text);
                    cmd.Parameters.AddWithValue("name", _TabPage.Text);
                    cmd.Parameters.AddWithValue("path", LocalPath == null ? DBNull.Value : LocalPath);
                    cmd.Parameters.AddWithValue("id", Id);

                    cmd.ExecuteNonQuery();
                }
            }

            PendingSaveDB = false;
        }

        public void SetClosed()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "UPDATE editortabs SET closedAt=:closedAt,position=NULL WHERE id=:id";
                cmd.Parameters.AddWithValue("closedAt", DateTime.Now.Ticks);
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            using (Connection c = _Data.OpenConnection())
            {
                SqliteCommand cmd = c.CreateCommand();
                cmd.CommandText = "DELETE FROM editortabs WHERE id=:id";
                cmd.Parameters.AddWithValue("id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public void SqlEditorProcessKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && _MainForm.CanRun)
            {
                _MainForm.Run(this);
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.D | Keys.Control))
            {
                Format();
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.L | Keys.Control))
            {
                _MainForm.ToggleFilterCurrentEditorTabTasks();
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Control | Keys.F) || e.KeyData == (Keys.Control | Keys.R))
            {
                EditorTabForm.ShowSearchAndReplace();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.F3)
            {
                GoNextSearchResult();
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Alt | Keys.F) || e.KeyData == (Keys.Control | Keys.H))
            {
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Escape)
            {
                EditorTabForm.HideSearchAndReplace();
                e.Handled = true;
            }
        }

        public void Navigate(bool forward)
        {
            if (forward)
            {
                Fctb.NavigateForward();
            }
            else
            {
                Fctb.NavigateBackward();
            }
        }

        public void SqlEditorProcessClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
            {
                Navigate(false);
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                Navigate(true);
            }
            else if (e.Button == MouseButtons.Right)
            {
                CmsFctb.Show(Fctb, e.X, e.Y);
            }
        }

        public void SqlEditorProcessParseTreeUpdated()
        {
            if (EditorTabForm.IsCurrentFctbTab(Fctb))
            {
                RefreshErrors();
            }
        }

        private void fctbSql_KeyPressing(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                if (_Fctb.Selection.Start == _Fctb.Selection.End)
                {
                    System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[_Fctb.Selection.Start.iLine].Substring(0, _Fctb.Selection.Start.iChar), @"^\s+$");

                    if (m.Success)
                    {
                        int n = m.Length % _Fctb.TabLength;
                        if (n == 0)
                        {
                            n = _Fctb.TabLength;
                        }

                        _Fctb.BeginAutoUndo();
                        _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                        _Fctb.Selection.End = new Place(_Fctb.Selection.End.iChar - n, _Fctb.Selection.End.iLine);
                        _Fctb.InsertText("");
                        _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                        _Fctb.EndAutoUndo();

                        e.Handled = true;
                    }
                }
            }
        }

        private void fctbSql_TextChangedDelayed(object? sender, EventArgs e)
        {
            SetPendingFileSave();
        }

        private void fctbSql_Enter(object? sender, EventArgs e)
        {
            _MainForm.EnableTimerPosition(true);
        }

        private void fctbSql_Leave(object? sender, EventArgs e)
        {
            _MainForm.EnableTimerPosition(false);
        }

        private void fctbSql_KeyDown(object? sender, KeyEventArgs e)
        {
            SqlEditorProcessKey(e);
        }

        private void fctbSql_MouseUp(object? sender, MouseEventArgs e)
        {
            SqlEditorProcessClick(e);
        }

        private void fctbSql_ParseTreeUpdated(object? sender, EventArgs e)
        {
            SqlEditorProcessParseTreeUpdated();
        }

        private void fctbSql_SecondaryFormClosed(object? sender, SecondaryFormEventArgs e)
        {
            _MainForm.SecondaryForms.Remove(e.Form);
        }

        private void fctbSql_SecondaryFormShowed(object? sender, SecondaryFormEventArgs e)
        {
            _MainForm.SecondaryForms.Add(e.Form);
        }

        private void fctbSql_AutoIndentNeeded(object? sender, AutoIndentEventArgs e)
        {
            if (e.IsCurrentLine)
            {
                string currentLineText = e.LineText.Trim();
                if (currentLineText == "" || currentLineText == ")")
                {
                    string previousText = _Fctb.GetRange(new Place(0, 0), new Place(0, e.iLine)).Text;
                    bool semiColonAdded;
                    bool dollarStringTagAdded;
                    List<AstNode>? stmts = ListStatements(previousText, out semiColonAdded, out dollarStringTagAdded);
                    if (stmts == null) return;

                    List<AstNode> tokens = stmts[stmts.Count - 1].RecursiveTokens;

                    int sentenceIndent = -1;
                    bool isNewSentenceAfterSemiColon;

                    if (dollarStringTagAdded)
                    {
                        AstNode dollarStringContent = stmts[stmts.Count - 1]["stmtContent"]!.Children.First(an => an[0].Name == "dollarString")[0]["dollarStringContent"]!;
                        List<AstNode> dollarStringContentTokens = dollarStringContent.RecursiveTokens;

                        if (dollarStringContentTokens[dollarStringContentTokens.Count - 1].Token!.Text == ";")
                        {
                            isNewSentenceAfterSemiColon = true;

                            for (int i = dollarStringContentTokens.Count - 2; i >= 0; i--)
                            {
                                string tokenText = dollarStringContentTokens[i].Token!.Text.ToUpper();

                                if (tokenText == ";" || tokenText == "DECLARE" || tokenText == "THEN" || tokenText == "ELSE" || tokenText == "LOOP"
                                     || (tokenText == "BEGIN" && _Fctb.Lines[dollarStringContentTokens[i].Token!.Location.Line].ToUpper().EndsWith(tokenText))) // Because BEGIN can also be part of a stmt, but we cannot have complete syntax validation here
                                {
                                    sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[dollarStringContentTokens[i + 1].Token!.Location.Line], @"^\s*").Length;
                                    break;
                                }
                            }

                            if (sentenceIndent == -1)
                            {
                                sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[dollarStringContentTokens[0].Token!.Location.Line], @"^\s*").Length;
                            }
                        }
                        else
                        {
                            isNewSentenceAfterSemiColon = false;
                        }
                    }
                    else
                    {
                        isNewSentenceAfterSemiColon = !semiColonAdded;

                        if (isNewSentenceAfterSemiColon)
                        {
                            int lastStmtStartLine = tokens[0].Token!.Location.Line;
                            sentenceIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[lastStmtStartLine], @"^\s*").Length;
                        }
                    }


                    if (isNewSentenceAfterSemiColon)
                    {
                        e.AbsoluteIndentation = sentenceIndent;
                    }
                    else
                    {
                        int lastTokenIndex;
                        if (dollarStringTagAdded)
                        {
                            lastTokenIndex = tokens.Count - 4;
                        }
                        else
                        {
                            lastTokenIndex = tokens.Count - 2;
                        }
                        Token previousToken = tokens[lastTokenIndex].Token!;
                        string previousTokenText = previousToken.Text.ToUpper();

                        if (previousTokenText == "(")
                        {
                            if (currentLineText == ")")
                            {
                                int previousIndent = System.Text.RegularExpressions.Regex.Match(_Fctb.Lines[previousToken.Location.Line], @"^\s*").Length;
                                string replacementText;

                                if (_Fctb.Lines[previousToken.Location.Line].Trim() == "(")
                                {
                                    replacementText = "(\r\n" + new String(' ', previousIndent + e.TabLength) + "\r\n" + new String(' ', previousIndent) + ")";
                                }
                                else
                                {
                                    replacementText = "\r\n" + new String(' ', previousIndent) + "(\r\n" + new String(' ', previousIndent + e.TabLength) + "\r\n" + new String(' ', previousIndent) + ")";
                                }

                                _Fctb.BeginAutoUndo();
                                _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                                _Fctb.Selection.Start = new Place(previousToken.Location.Column, previousToken.Location.Line);
                                _Fctb.Selection.End = new Place(_Fctb.Lines[e.iLine].Length, e.iLine);
                                _Fctb.InsertText(replacementText);
                                _Fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(_Fctb.TextSource));

                                _Fctb.EndAutoUndo();

                                Place finalCaretPosition;
                                if (_Fctb.Lines[previousToken.Location.Line].Trim() == "(")
                                {
                                    finalCaretPosition = new Place(_Fctb.Lines[e.iLine].Length, e.iLine);
                                }
                                else
                                {
                                    finalCaretPosition = new Place(_Fctb.Lines[e.iLine + 1].Length, e.iLine + 1);
                                }

                                _Fctb.Selection.Start = finalCaretPosition;
                                _Fctb.Selection.End = finalCaretPosition;
                                e.AbsoluteIndentation = 0;
                            }
                            else
                            {
                                e.Shift = e.TabLength;
                            }
                        }
                        else if (dollarStringTagAdded &&
                                (
                                    previousTokenText == "DECLARE"
                                    || (previousTokenText == "BEGIN" && _Fctb.Lines[previousToken.Location.Line].ToUpper().EndsWith("BEGIN")) // Because BEGIN can also be part of a stmt, but we cannot have complete syntax validation here
                                    || previousTokenText == "THEN"
                                    || previousTokenText == "ELSE"
                                    || previousTokenText == "LOOP"
                                    || previousToken.Terminal.Name == "dollar_string_tag"
                                )
                            )
                        {
                            e.Shift = e.TabLength;
                        }
                    }
                }

            }

        }

        public void Undo()
        {
            Fctb.Undo();
        }

        public void Redo()
        {
            Fctb.Redo();
        }

        public void Cut()
        {
            Fctb.Cut();
        }

        public void Copy()
        {
            Fctb.Copy();
        }

        public void Paste()
        {
            Fctb.Paste();
        }

        public void ShowGoTo()
        {
            Fctb.ShowGoToDialog();
        }

        private void tsmiUndo_Click(object? sender, EventArgs e)
        {
            Undo();
        }

        private void tsmiRedo_Click(object? sender, EventArgs e)
        {
            Redo();
        }

        private void tsmiBack_Click(object? sender, EventArgs? e)
        {
            Navigate(false);
        }

        private void tsmiForward_Click(object? sender, EventArgs? e)
        {
            Navigate(true);
        }

        private void tsmiCut_Click(object? sender, EventArgs e)
        {
            Cut();
        }

        private void tsmiCopy_Click(object? sender, EventArgs e)
        {
            Copy();
        }

        private void tsmiPaste_Click(object? sender, EventArgs e)
        {
            Paste();
        }

        private void tsmiFormat_Click(object? sender, EventArgs? e)
        {
            Format();
        }

        private void tsmiSearchAndReplace_Click(object? sender, EventArgs e)
        {
            EditorTabForm.ShowSearchAndReplace();
        }

        private void tsmiGoTo_Click(object? sender, EventArgs e)
        {
            ShowGoTo();
        }

        private void tsmiError_Click(object? sender, EventArgs? e)
        {
            ToolStripMenuItem errorDeEjemploToolStripMenuItem = (ToolStripMenuItem)sender!;
            Tuple<CustomFctb, Irony.LogMessage> tag = (Tuple<CustomFctb, Irony.LogMessage>)errorDeEjemploToolStripMenuItem.Tag;
            CustomFctb fctbSql = tag.Item1;
            Irony.LogMessage msg = tag.Item2;

            var loc = msg.Location;
            var place = new Place(loc.Column, loc.Line);
            var r = new FastColoredTextBoxNS.Range(fctbSql, place, place);

            fctbSql.Selection = r;
            fctbSql.DoSelectionVisible();
        }

        protected List<AstNode>? ListStatements(string txt, out bool semiColonAdded, out bool dollarStringTagAdded)
        {
            Parser parser = new Parser(_Data!.PGSimpleLanguageData);
            ParseTree parseTree = parser.Parse(txt);

            dollarStringTagAdded = false;

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                int indexInsertPointToken = parseTree.Tokens.Count - 1;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "EOF") indexInsertPointToken--;
                if (parseTree.Tokens[indexInsertPointToken].Terminal.Name == "line_comment") indexInsertPointToken--;

                Token insertPointToken = parseTree.Tokens[indexInsertPointToken];
                Token endToken = parseTree.Tokens[parseTree.Tokens.Count - 1];

                int insertPoint = insertPointToken.Location.Position + insertPointToken.Length;
                int end = endToken.Location.Position + endToken.Length;

                txt = txt.Substring(0, insertPoint) + ";" + txt.Substring(insertPoint, end - insertPoint);
                semiColonAdded = true;

                parseTree = parser.Parse(txt);

                if (parseTree.Status == ParseTreeStatus.Error)
                {
                    string? stringTag = parseTree.Tokens.Select(tk => tk.Text).FirstOrDefault(tt => System.Text.RegularExpressions.Regex.Match(tt, @"^\$.*\$$").Success);

                    if (stringTag != null)
                    {
                        txt = txt.Substring(0, insertPoint) + " x " + stringTag + txt.Substring(insertPoint, end - insertPoint);
                        dollarStringTagAdded = true;

                        parseTree = parser.Parse(txt);
                    }
                }
            }
            else
            {
                semiColonAdded = false;
            }


            if (parseTree.Status == ParseTreeStatus.Error) return null;

            AstNode astRoot = AstNode.ProcessParseTree(parseTree);

            if (astRoot.Children.Count == 0) return null;

            return astRoot.Children[0].Children;
        }

        private void SetFileName(string fileName)
        {
            TabPage.Text = Path.GetFileName(fileName);
            TabPage.ToolTipText = fileName;

            if (_SeparatedEditorTabForm != null) _SeparatedEditorTabForm.Text = TabPage.Text;
        }

        public class CreateEditorTabOptions
        {
            public string? Title = null;
            public string? Text = null;
            public string? Path = null;
            public bool Focus = false;
            public bool Format = false;
            public bool PendingFileSave = true;
        }
    }
}

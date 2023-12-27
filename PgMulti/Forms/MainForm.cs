using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.DataStructure;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using PgMulti.AppData;
using PgMulti.QueryEditor;
using PgMulti.SqlSyntax;
using PgMulti.Tasks;
using System.Globalization;
using System.Net;
using PgMulti.Export;
using PgMulti.Forms;
using System.Windows.Forms;
using PgMulti.Diagrams;
using Newtonsoft.Json;
using PgMulti.Properties;
using System;
using System.Linq;
using Irony;
using static PgMulti.Tasks.PgTask;

namespace PgMulti
{
    public partial class MainForm : Form
    {
        #region "General"
        private Data? _Data;
        private Node _NRoot;
        private Mutex _Mutex;
        private bool _AutomaticScroll = true;
        private MainFormTreeModel _TreeModel;
        private List<Form> _SecondaryForms = new List<Form>();
        private Dictionary<LogStyle, FastColoredTextBoxNS.Style> _FctbResultStyles;


        private static Font _CheckedNodeFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private static Font _NodeFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private static Brush _SelectedNodeBackgroundBrush = new SolidBrush(Color.FromName("Highlight"));
        private static Color _CheckedNodeTextColor = Color.FromArgb(255, 0, 0, 0);
        private static Brush _FullCheckedNodeBackgroundBrush = new SolidBrush(Color.FromArgb(255, 150, 211, 95));
        private static Brush _PartialCheckedNodeBackgroundBrush = new SolidBrush(Color.FromArgb(255, 228, 239, 101));

        public MainForm()
        {
            InitializeComponent();
            InitializeText();
            mm.CanOverflow = true;
            _TreeModel = new MainFormTreeModel();
            tvaConnections.Model = _TreeModel;
            _NRoot = new Node(Properties.Text.all_databases);
            _NRoot.Image = Properties.Resources.tva_grupo;
            _TreeModel.Nodes.Add(_NRoot);
            _Mutex = new Mutex(false);
            ntb.DrawText += ntb_DrawText;
            ncb.CheckStateChanged += ncb_CheckStateChanged;
            ncb.IsVisibleValueNeeded += ncb_IsVisibleValueNeeded;

            _FctbResultStyles = new Dictionary<LogStyle, Style>();
            _FctbResultStyles[LogStyle.Timestamp] = new TextStyle(Brushes.Black, null, FontStyle.Bold);
            _FctbResultStyles[LogStyle.Query] = new TextStyle(Brushes.White, Brushes.Black, FontStyle.Bold);
            _FctbResultStyles[LogStyle.TaskIsRunning] = new TextStyle(Brushes.White, Brushes.DarkBlue, FontStyle.Bold);
            _FctbResultStyles[LogStyle.TaskSuccessfullyCompleted] = new TextStyle(Brushes.White, Brushes.DarkGreen, FontStyle.Bold);
            _FctbResultStyles[LogStyle.TaskFailed] = new TextStyle(Brushes.White, Brushes.DarkRed, FontStyle.Bold);
            _FctbResultStyles[LogStyle.Error] = new TextStyle(Brushes.DarkRed, null, FontStyle.Bold);
        }



        public List<DB> SelectedDBs
        {
            get
            {
                List<DB> l = new List<DB>();

                Stack<Node> stack = new Stack<Node>();
                stack.Push(_NRoot);

                while (stack.Count > 0)
                {
                    Node tn = stack.Pop();

                    if (tn.IsChecked && tn.Tag is DB)
                    {
                        l.Add((DB)tn.Tag);
                    }

                    foreach (Node tni in tn.Nodes) stack.Push(tni);
                }

                return l;
            }
        }

        internal List<Form> SecondaryForms { get => _SecondaryForms; }
        internal TradeWright.UI.Forms.TabControlExtra SqlEditorTabControl { get => tcSql; }

        #endregion

        #region "Form"
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Data.ExistDB)
            {
                LoginForm f = new LoginForm();
                f.ShowDialog(this);

                if (f.Password == null)
                {
                    Application.Exit();
                    return;
                }

                _Data = new Data(f.Password);
            }
            else
            {
                SetupForm f = new SetupForm();
                f.ShowDialog(this);

                if (f.Password == null)
                {
                    Application.Exit();
                    return;
                }

                _Data = new Data(f.Password);
            }

            //tvaConnections.Root.Children[0].Expanded += root_Expanded;
            fctbExecutedSql.SetParser(_Data.PGLanguageData);

            RefreshConnectionsTreeControl(false);
            UpdateServersButtons();
            ExpandServersTree();

            foreach (EditorTab si in _Data.ListOpenEditorTabs(this)) { }

            if (tcSql.TabCount == 1)
            {
                CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
            }
            else
            {
                tcSql.SelectedIndex = 0;
            }

            RefreshTransactionsConfig();
            UpdateRunButton(null);
            CheckUpdates();
        }

        private void RefreshTransactionsConfig()
        {
            tsmiTransactionModeManual.Image = null;
            tsmiTransactionModeAutoSingle.Image = null;
            tsmiTransactionModeAutoCoordinated.Image = null;
            tsmiTransactionLevelReadCommitted.Image = null;
            tsmiTransactionLevelRepeatableRead.Image = null;
            tsmiTransactionLevelSerializable.Image = null;

            switch (_Data!.Config.TransactionMode)
            {
                case Config.TransactionModeEnum.Manual:
                    tsmiTransactionModeManual.Image = Properties.Resources.check;
                    break;
                case Config.TransactionModeEnum.AutoSingle:
                    tsmiTransactionModeAutoSingle.Image = Properties.Resources.check;
                    break;
                case Config.TransactionModeEnum.AutoCoordinated:
                    tsmiTransactionModeAutoCoordinated.Image = Properties.Resources.check;
                    break;
                default:
                    throw new NotSupportedException();
            }

            switch (_Data!.Config.TransactionLevel)
            {
                case Config.TransactionLevelEnum.ReadCommited:
                    tsmiTransactionLevelReadCommitted.Image = Properties.Resources.check;
                    break;
                case Config.TransactionLevelEnum.RepeatableRead:
                    tsmiTransactionLevelRepeatableRead.Image = Properties.Resources.check;
                    break;
                case Config.TransactionLevelEnum.Serializable:
                    tsmiTransactionLevelSerializable.Image = Properties.Resources.check;
                    break;
                default:
                    throw new NotSupportedException();
            }

            switch (_Data!.Config.TransactionMode)
            {
                case Config.TransactionModeEnum.Manual:
                    switch (_Data!.Config.TransactionLevel)
                    {
                        case Config.TransactionLevelEnum.ReadCommited:
                            tsddbTransactions.Image = Properties.TransactionIcons.ManualReadCommited;
                            break;
                        case Config.TransactionLevelEnum.RepeatableRead:
                            tsddbTransactions.Image = Properties.TransactionIcons.ManualRepeatableRead;
                            break;
                        case Config.TransactionLevelEnum.Serializable:
                            tsddbTransactions.Image = Properties.TransactionIcons.ManualSerializable;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                case Config.TransactionModeEnum.AutoSingle:
                    switch (_Data!.Config.TransactionLevel)
                    {
                        case Config.TransactionLevelEnum.ReadCommited:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticIndependentReadCommited;
                            break;
                        case Config.TransactionLevelEnum.RepeatableRead:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticIndependentRepeatableRead;
                            break;
                        case Config.TransactionLevelEnum.Serializable:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticIndependentSerializable;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                case Config.TransactionModeEnum.AutoCoordinated:
                    switch (_Data!.Config.TransactionLevel)
                    {
                        case Config.TransactionLevelEnum.ReadCommited:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticCoordinatedReadCommited;
                            break;
                        case Config.TransactionLevelEnum.RepeatableRead:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticCoordinatedRepeatableRead;
                            break;
                        case Config.TransactionLevelEnum.Serializable:
                            tsddbTransactions.Image = Properties.TransactionIcons.AutomaticCoordinatedSerializable;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Data == null) return;

            foreach (PgTask t in ListTasks(false))
            {
                if (t.State != PgTask.StateEnum.Finished)
                {
                    MessageBox.Show(Properties.Text.warning_runnig_tasks, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                    return;
                }
            }

            try
            {
                SaveTabs();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(Properties.Text.error_saving + "\r\n" + ex.Message, Properties.Text.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }

            TabPage tp = tcSql.TabPages[0];
            EditorTab si = (EditorTab)tp.Tag!;

            _Data!.Config.FontSize = (int)Math.Round(si.Fctb.Font.Size);
            _Data!.Config.Save();
        }

        private FormWindowState _LastWindowState = FormWindowState.Normal;
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && _LastWindowState != FormWindowState.Minimized)
            {
                foreach (Form form in _SecondaryForms)
                {
                    form.WindowState = FormWindowState.Minimized;
                }
            }
            else if (_LastWindowState == FormWindowState.Minimized)
            {
                foreach (Form form in _SecondaryForms)
                {
                    form.WindowState = FormWindowState.Normal;
                }
            }

            _LastWindowState = WindowState;
        }

        private void tsmiImportConnections_Click(object sender, EventArgs e)
        {
            ofdImportConfig.FileName = "pgMultiConnections.pgcx";
            if (ofdImportConfig.ShowDialog(this) != DialogResult.OK) return;

            ExportConnectionsFile ecf = new ExportConnectionsFile();
            try
            {
                ecf.LoadFile(ofdImportConfig.FileName);
            }
            catch (Export.BadFormatException)
            {
                MessageBox.Show(Properties.Text.warning_bad_format_export_file, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_opening_file + $":\r\n{ofdImportConfig.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ExportImportConnectionsForm eicf = new ExportImportConnectionsForm(_Data!, ecf, true);
            if (eicf.ShowDialog(this) == DialogResult.OK)
            {

                RefreshConnectionsTreeControl();
                UpdateServersButtons();
                ExpandServersTree();
            }
        }

        private void tsmiExportConnections_Click(object sender, EventArgs e)
        {
            ExportConnectionsFile ecf = new ExportConnectionsFile();
            ecf.LoadConfig(_Data!);

            ExportImportConnectionsForm eicf = new ExportImportConnectionsForm(_Data!, ecf, false);
            eicf.ShowDialog(this);
        }

        private void tsbOpenDiagram_Click(object sender, EventArgs e)
        {
            ofdOpenDiagram.FileName = "*.pgdx";
            if (ofdOpenDiagram.ShowDialog(this) != DialogResult.OK) return;

            Diagram dg;
            try
            {
                dg = Diagram.LoadFile(ofdOpenDiagram.FileName);
            }
            catch (Diagrams.BadFormatException)
            {
                MessageBox.Show(Properties.Text.warning_bad_format_diagram_file, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_opening_file + $":\r\n{ofdOpenDiagram.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DiagramForm df = new DiagramForm(_Data!, dg, ofdOpenDiagram.FileName, SelectedDBs.FirstOrDefault());
            df.Show();
        }

        private DiagramForm? CreateDiagram(DB? preselectedDB)
        {
            sfdSaveDiagram.FileName = "pgMultiDiagram.pgdx";
            if (sfdSaveDiagram.ShowDialog(this) != DialogResult.OK) return null;

            Diagram dg = new Diagram();
            try
            {
                dg.SaveFile(sfdSaveDiagram.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{sfdSaveDiagram.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            DiagramForm df = new DiagramForm(_Data!, dg, sfdSaveDiagram.FileName, preselectedDB == null ? SelectedDBs.FirstOrDefault() : preselectedDB);
            df.Show();

            return df;
        }

        private void tsbNewDiagram_Click(object sender, EventArgs e)
        {
            CreateDiagram(null);
        }

        private void tsmiUpdates_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo((string)tsmiUpdates.Tag) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Properties.Text.unable_to_open_url, (string)tsmiUpdates.Tag, ex.Message), Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CheckUpdates()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "pgMulti");
                HttpResponseMessage response = await client.GetAsync(AppSettings.Default.LatestReleaseInfoUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic? json = JsonConvert.DeserializeObject(responseBody);

                if (json != null)
                {
                    int[] currentVersion = Application.ProductVersion.Split('+').First().Split('.').Select(s => int.Parse(s)).ToArray();
                    int[] latestReleaseVersion = ((string)json.tag_name).Split('.').Select(s => int.Parse(s)).ToArray();

                    bool obsolete = false;

                    for (int i = 0; i < currentVersion.Length && i < latestReleaseVersion.Length; i++)
                    {
                        if (currentVersion[i] < latestReleaseVersion[i])
                        {
                            obsolete = true;
                            break;
                        }
                        else if (currentVersion[i] > latestReleaseVersion[i])
                        {
                            obsolete = false;
                            break;
                        }
                    }

                    if (obsolete)
                    {
                        tsmiUpdates.Visible = true;
                        tsmiUpdates.Text = Properties.Text.update_available + ": " + Application.ProductVersion.Split('+').First() + " >> " + (string)json.tag_name;
                        tsmiUpdates.Image = global::PgMulti.Properties.Resources.updates_found;
                        tsmiUpdates.Tag = (string)json.html_url;
                    }
                }
            }
            catch (Exception)
            {
                tsmiUpdates.Visible = true;
                tsmiUpdates.Text = Properties.Text.check_updates;
                tsmiUpdates.Tag = AppSettings.Default.ProjectUrl;
            }
        }

        /// <summary>
        /// Trick to avoid having to click twice on the toolbar buttons when the window does not have the focus.
        /// https://stackoverflow.com/questions/13836363/why-two-time-click-is-required-to-click-toolstripmenuitem
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_PARENTNOTIFY = 0x0210;
            if (m.Msg == WM_PARENTNOTIFY)
            {
                if (!Focused)
                    Activate();
            }
            base.WndProc(ref m);
        }

        #endregion

        #region "Connections tree control"

        private void ResetStructureCache()
        {
            foreach (DB db in _Data!.AllDBs)
            {
                db.ResetSchemas();
            }
        }

        private void RefreshConnectionsTreeControl(bool reloadStructure = true)
        {
            if (reloadStructure) _Data!.ReloadStructure();
            tvaConnections.BeginUpdate();
            _NRoot.Nodes.Clear();
            ClearSelectedNodesTreeView();

            Queue<Tuple<Group, Node>> queue = new Queue<Tuple<Group, Node>>();

            _NRoot.Tag = _Data!.RootGroup;
            queue.Enqueue(new Tuple<Group, Node>(_Data!.RootGroup, _NRoot));

            while (queue.Count > 0)
            {
                Tuple<Group, Node> t = queue.Dequeue();
                Group g = t.Item1;
                Node ng = t.Item2;

                List<Tuple<int, object>> items = new List<Tuple<int, object>>();

                foreach (DB db in g.DBs)
                {
                    items.Add(new Tuple<int, object>(db.Position, db));
                }

                foreach (Group gi in g.ChildGroups)
                {
                    items.Add(new Tuple<int, object>(gi.Position, gi));
                }

                items = items.OrderBy(t => t.Item1).ToList();

                foreach (Tuple<int, object> childItem in items)
                {
                    if (childItem.Item2 is DB)
                    {
                        DB db = (DB)childItem.Item2;
                        Node tnBD = new Node(db.Alias);
                        tnBD.Tag = db;
                        tnBD.Image = Properties.Resources.tva_db;

                        ng.Nodes.Add(tnBD);
                    }
                    else if (childItem.Item2 is Group)
                    {
                        Group childGroup = (Group)childItem.Item2;
                        Node nChildGroup = new Node(childGroup.Name);
                        nChildGroup.Tag = childGroup;
                        nChildGroup.Image = Properties.Resources.tva_grupo;
                        ng.Nodes.Add(nChildGroup);

                        queue.Enqueue(new Tuple<Group, Node>(childGroup, nChildGroup));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            Stack<Node> stack = new Stack<Node>();

            stack.Push(_NRoot);

            while (stack.Count > 0)
            {
                Node n = stack.Pop();

                if (n != _NRoot && !(n.Tag is Group)) continue;

                _TreeModel.OnStructureChanged(new TreePathEventArgs(_TreeModel.GetPath(n)));

                foreach (Node childNode in n.Nodes)
                {
                    stack.Push(childNode);
                }
            }

            tvaConnections.EndUpdate();
            UpdateRunButton(null);
        }

        private TreeNodeAdv? _ConnectionTreeControlSelectPath(int idGroup)
        {
            // Find Group

            Stack<Group> stack = new Stack<Group>();

            stack.Push(_Data!.RootGroup);

            Group? g = null;
            while (stack.Count > 0)
            {
                Group gi = stack.Pop();

                if (gi.Id == idGroup)
                {
                    g = gi;
                    break;
                }

                foreach (Group gj in gi.ChildGroups)
                {
                    stack.Push(gj);
                }
            }

            if (g == null)
            {
                return null;
            }
            else
            {
                // If found, get path to it

                Group? gi = g;
                Stack<Group> path = new Stack<Group>();

                while (gi != null)
                {
                    path.Push(gi);
                    gi = gi.ParentGroup;
                }

                // Expand recursively each node of the path

                TreeNodeAdv tna = tvaConnections.Root;

                while (path.Count > 0)
                {
                    Group gj = path.Pop();
                    tna = tna.Children.First(tnai => ((Node)tnai.Tag).Tag is Group && ((Group)((Node)tnai.Tag).Tag).Id == gj.Id);
                    tna.Expand();
                }

                // Return last tree node

                return tna;
            }
        }

        private void ConnectionTreeControlSelectDB(int idParentGroup, int idDB)
        {
            TreeNodeAdv? tna = _ConnectionTreeControlSelectPath(idParentGroup);

            if (tna == null) return;

            tna = tna.Children.First(tnai => ((Node)tnai.Tag).Tag is DB && ((DB)((Node)tnai.Tag).Tag).Id == idDB);
            tvaConnections.SelectedNode = tna;

            UpdateServersButtons();
        }

        private void ConnectionTreeControlSelectGroup(int? idParentGroup, int idGroup)
        {
            TreeNodeAdv? tna;

            if (idParentGroup.HasValue)
            {
                tna = _ConnectionTreeControlSelectPath(idParentGroup.Value);
                if (tna == null) return;

                tna = tna.Children.First(tnai => ((Node)tnai.Tag).Tag is Group && ((Group)((Node)tnai.Tag).Tag).Id == idGroup);
            }
            else
            {
                tna = _ConnectionTreeControlSelectPath(idGroup);
                if (tna == null) return;
            }

            tvaConnections.SelectedNode = tna;

            UpdateServersButtons();
        }

        private void UpdateServersButtons(bool hideAll = false)
        {
            bool tsbNewGroupVisible = false;
            bool tsbNewDBVisible = false;
            bool tsbEditVisible = false;
            bool tsbRemoveVisible = false;
            bool tsbUpVisible = false;
            bool tsbDownVisible = false;
            bool tsbExploreTableVisible = false;
            bool tsbRecursiveRemoveVisible = false;
            bool tsbCreateTableDiagramVisible = false;
            bool tsbUpEnabled = false;
            bool tsbDownEnabled = false;
            bool tssNewVisible = false;
            bool tssEditVisible = false;
            bool tssUpDownVisible = false;
            bool tssCopyTextVisible = false;

            if (!hideAll)
            {
                Node? tn = (tvaConnections.SelectedNode == null ? null : (Node)tvaConnections.SelectedNode.Tag);

                if (tn != null)
                {
                    if (tn == _NRoot)
                    {
                        tsbNewGroupVisible = true;
                        tsbNewDBVisible = true;
                        tssNewVisible = true;
                    }
                    else if (tn!.Tag is Group)
                    {
                        tsbNewGroupVisible = true;
                        tsbNewDBVisible = true;
                        tssNewVisible = true;
                        tsbRemoveVisible = true;
                        tsbEditVisible = true;
                        tssEditVisible = true;
                    }
                    else if (tn!.Tag is DB)
                    {
                        tsbRemoveVisible = true;
                        tsbEditVisible = true;
                        tssEditVisible = true;
                    }
                    else if (tn!.Tag is Function || tn!.Tag is Tuple<Function, string>)
                    {
                        tsbEditVisible = true;
                        tssEditVisible = true;
                    }

                    if (tn != _NRoot && (tn!.Tag is Group || tn!.Tag is DB))
                    {
                        tsbUpVisible = true;
                        tsbDownVisible = true;
                        tssUpDownVisible = true;

                        if (tn.PreviousNode != null)
                        {
                            tsbUpEnabled = true;
                        }

                        if (tn.NextNode != null)
                        {
                            tsbDownEnabled = true;
                        }
                    }

                    if (tn.Tag is Table)
                    {
                        tsbExploreTableVisible = true;
                        tsbRecursiveRemoveVisible = true;
                        tsbCreateTableDiagramVisible = true;
                        tssEditVisible = true;
                    }

                    if (tn != _NRoot)
                    {
                        tssCopyTextVisible = true;
                    }
                }
            }

            tsbNewGroup.Visible = tsbNewGroupVisible;
            tscmiNewGroup.Visible = tsbNewGroupVisible;
            tsbNewDB.Visible = tsbNewDBVisible;
            tscmiNewDB.Visible = tsbNewDBVisible;
            tsbEdit.Visible = tsbEditVisible;
            tscmiEdit.Visible = tsbEditVisible;
            tsbRemove.Visible = tsbRemoveVisible;
            tscmiRemove.Visible = tsbRemoveVisible;
            tsbUp.Visible = tsbUpVisible;
            tscmiUp.Visible = tsbUpVisible;
            tsbDown.Visible = tsbDownVisible;
            tscmiDown.Visible = tsbDownVisible;
            tsbUp.Enabled = tsbUpEnabled;
            tscmiUp.Enabled = tsbUpEnabled;
            tsbDown.Enabled = tsbDownEnabled;
            tscmiDown.Enabled = tsbDownEnabled;
            tsbExploreTable.Visible = tsbExploreTableVisible;
            tscmiExploreTable.Visible = tsbExploreTableVisible;
            tsbRecursiveRemove.Visible = tsbRecursiveRemoveVisible;
            tscmiRecursiveRemove.Visible = tsbRecursiveRemoveVisible;
            tsbCreateTableDiagram.Visible = tsbCreateTableDiagramVisible;
            tscmiCreateTableDiagram.Visible = tsbCreateTableDiagramVisible;
            tscmiCopyText.Visible = tssCopyTextVisible;

            tssNew.Visible = tssNewVisible;
            tssEdit.Visible = tssEditVisible;
            tssUpDown.Visible = tssUpDownVisible;
        }

        private void ExpandServersTree()
        {
            tvaConnections.Root.Children[0].Expand(true);
            foreach (TreeNodeAdv tna in tvaConnections.Root.Children[0].Children)
            {
                if (((Node)tna.Tag).Tag is Group)
                {
                    tna.Expand(true);
                }
            }
        }

        private void ncb_IsVisibleValueNeeded(object? sender, NodeControlValueEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n == _NRoot || n.Tag is Group || n.Tag is DB)
            {
                e.Value = true;
            }
            else
            {
                e.Value = false;
            }
        }

        private void ntb_DrawText(object? sender, DrawEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            e.Font = _NodeFont;

            switch (((Node)e.Node.Tag).CheckState)
            {
                case CheckState.Checked:
                    e.Font = _CheckedNodeFont;
                    break;
                case CheckState.Unchecked:
                    break;
                case CheckState.Indeterminate:
                    e.Font = _CheckedNodeFont;
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (_TvaConnectionsHasFocus)
            {
                if (e.Node.IsSelected && e.BackgroundBrush != null)
                {
                    return;
                }
            }

            if ((n.Tag is DB) || !e.Node.IsExpanded)
            {

                switch (n.CheckState)
                {
                    case CheckState.Checked:
                        e.TextColor = _CheckedNodeTextColor;
                        e.BackgroundBrush = _FullCheckedNodeBackgroundBrush;
                        break;
                    case CheckState.Unchecked:
                        break;
                    case CheckState.Indeterminate:
                        e.TextColor = _CheckedNodeTextColor;
                        e.BackgroundBrush = _PartialCheckedNodeBackgroundBrush;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        //private void root_Expanded(object? sender, TreeViewAdvEventArgs e)
        //{
        //    foreach (TreeNodeAdv tna in tvaConnections.Root.Children[0].Children)
        //    {
        //        tna.Expand(true);
        //    }
        //}

        private void tsbNewGroup_Click(object sender, EventArgs e)
        {
            TreeNodeAdv tnaParent = tvaConnections.SelectedNode;
            Node nParent = (Node)tnaParent.Tag;
            Group parentGroup = ((Group)nParent.Tag);
            EditGroupForm f = new EditGroupForm(_Data!, parentGroup, null);
            f.ShowDialog(this);

            if (f.Group == null) return;

            f.Group.Position = (short)nParent.Nodes.Count;

            f.Group.Save();

            RefreshConnectionsTreeControl();
            ConnectionTreeControlSelectGroup(f.Group.IdParentGroup, f.Group.Id);
        }

        private void tsbNewDB_Click(object sender, EventArgs e)
        {
            TreeNodeAdv tnaParent = tvaConnections.SelectedNode;
            Node nGroup = (Node)tnaParent.Tag;
            Group g = (Group)nGroup.Tag;

            EditDBForm f = new EditDBForm(_Data!, g, null);
            f.ShowDialog(this);

            if (f.DB == null) return;

            f.DB.Position = (short)nGroup.Nodes.Count;

            f.DB.Save();

            RefreshConnectionsTreeControl();
            ConnectionTreeControlSelectDB(f.DB.IdGroup, f.DB.Id);
        }

        private void tsbExploreTable_Click(object sender, EventArgs e)
        {
            TreeNodeAdv tn = tvaConnections.SelectedNode;
            Node n = (Node)tn.Tag;
            Table t = (Table)n.Tag;
            string sql = "SELECT * FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(t.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(t.Id) + ";";

            EditorTab.CreateEditorTabOptions o = new EditorTab.CreateEditorTabOptions();
            o.Title = string.Format(Properties.Text.explore_table_x, t.Id);
            o.Text = sql;
            o.Focus = true;

            EditorTab et = CreateEditorTab(o);

            Log h = new Log(_Data!);

            h.SqlText = sql;

            PgTaskExecutorSqlTables tes = new PgTaskExecutorSqlTables(_Data!, t.Schema!.DB, new PgTask.OnUpdate(Task_OnUpdate), sql, Config.TransactionModeEnum.Manual, Config.TransactionLevelEnum.ReadCommited, _Data!.PGSimpleLanguageData, null);
            h.DBIds.Add(t.Schema!.DB.Id);

            tes.Start();
            et.LastTask = tes;

            h.Save();
            _Data.CheckAppDbFileSize();
        }

        private void tsbRecursiveRemove_Click(object sender, EventArgs e)
        {
            Node tn = (Node)tvaConnections.SelectedNode.Tag;

            if (tn.Tag is Table)
            {
                RecursiveRemoverToolForm f = new RecursiveRemoverToolForm((Table)tn.Tag);

                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    PgMulti.RecursiveRemover.RecursiveRemover rr = f.RecursiveRemover!;

                    StringBuilder sb;
                    EditorTab.CreateEditorTabOptions o;


                    sb = new StringBuilder();

                    rr.WriteCollectTuplesScript(sb);

                    o = new EditorTab.CreateEditorTabOptions();
                    o.Title = Properties.Text.collect_tuples_script_name;
                    o.Text = sb.ToString();
                    o.Focus = true;

                    CreateEditorTab(o);


                    sb = new StringBuilder();

                    rr.WriteDeleteScript(sb);

                    o = new EditorTab.CreateEditorTabOptions();
                    o.Title = Properties.Text.delete_tuples_script_name;
                    o.Text = sb.ToString();
                    o.Focus = false;

                    CreateEditorTab(o);

                    ClearSelectedNodesTreeView();
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void tsbCreateTableDiagram_Click(object sender, EventArgs e)
        {
            TreeNodeAdv tn = tvaConnections.SelectedNode;
            Node n = (Node)tn.Tag;
            Table t = (Table)n.Tag;

            DiagramForm? df = CreateDiagram(t.Schema!.DB);
            if (df == null) return;

            df.AddTables(new List<Table>() { t });
            ExpandDiagramPanel educ = df.OpenExpandDiagramPanel();

            educ.EnableAll();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            Node tn = (Node)tvaConnections.SelectedNode.Tag;

            if (tn.Tag is Group && tn != _NRoot)
            {
                Group g = (Group)tn.Tag;
                EditGroupForm f = new EditGroupForm(_Data!, g.ParentGroup, g);
                f.ShowDialog(this);

                if (f.Group == null) return;

                f.Group.Save();

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectGroup(f.Group.IdParentGroup, f.Group.Id);
            }
            else if (tn.Tag is DB)
            {
                DB db = (DB)tn.Tag;
                EditDBForm f = new EditDBForm(_Data!, db.Group, db);
                f.ShowDialog(this);

                if (f.DB == null) return;

                f.DB.Save();

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectDB(f.DB.IdGroup, f.DB.Id);
            }
            else if (tn!.Tag is Function)
            {
                EditFunction((Function)tn!.Tag);
            }
            else if (tn!.Tag is Tuple<Function, string>)
            {
                EditFunction(((Tuple<Function, string>)tn!.Tag).Item1);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            Node tn = (Node)tvaConnections.SelectedNode.Tag;
            Group? parentGroup;

            if (tn.Tag is Group && tn != _NRoot)
            {
                Group g = (Group)tn.Tag;
                parentGroup = g.ParentGroup;

                if (MessageBox.Show(string.Format(Properties.Text.confirm_remove_group, g.Name), Properties.Text.confirm_remove, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;
                g.Delete();

                RefreshConnectionsTreeControl();
            }
            else if (tn.Tag is DB)
            {
                DB db = (DB)tn.Tag;
                parentGroup = db.Group;

                if (MessageBox.Show(string.Format(Properties.Text.confirm_remove_db, db.Alias), Properties.Text.confirm_remove, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;
                db.Delete();

                RefreshConnectionsTreeControl();
            }
            else
            {
                throw new NotSupportedException();
            }

            if (parentGroup == null)
            {
                UpdateServersButtons();
                ExpandServersTree();
            }
            else
            {
                ConnectionTreeControlSelectGroup(parentGroup.IdParentGroup, parentGroup.Id);
            }
        }

        private void tsbUp_Click(object sender, EventArgs e)
        {
            Node tn = (Node)tvaConnections.SelectedNode.Tag;
            Node tnPrev = (Node)tvaConnections.SelectedNode.PreviousNode.Tag;

            if (tn.Tag is Group && tn != _NRoot)
            {
                Group g = (Group)tn.Tag;

                g.Position -= 1;
                g.Save();

                if (tnPrev.Tag is Group && tnPrev != _NRoot)
                {
                    Group gPrev = (Group)tnPrev.Tag;
                    gPrev.Position += 1;
                    gPrev.Save();
                }
                else if (tnPrev.Tag is DB)
                {
                    DB dbPrev = (DB)tnPrev.Tag;
                    dbPrev.Position += 1;
                    dbPrev.Save();
                }
                else
                {
                    throw new NotSupportedException();
                }

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectGroup(g.IdParentGroup, g.Id);
            }
            else if (tn.Tag is DB)
            {
                DB db = (DB)tn.Tag;
                db.Position -= 1;
                db.Save();

                if (tnPrev.Tag is Group && tnPrev != _NRoot)
                {
                    Group gPrev = (Group)tnPrev.Tag;
                    gPrev.Position += 1;
                    gPrev.Save();
                }
                else if (tnPrev.Tag is DB)
                {
                    DB dbPrev = (DB)tnPrev.Tag;
                    dbPrev.Position += 1;
                    dbPrev.Save();
                }
                else
                {
                    throw new NotSupportedException();
                }

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectDB(db.IdGroup, db.Id);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            Node tn = (Node)tvaConnections.SelectedNode.Tag;
            Node tnNext = (Node)tvaConnections.SelectedNode.NextNode.Tag;

            if (tn.Tag is Group && tn != _NRoot)
            {
                Group g = (Group)tn.Tag;

                g.Position += 1;
                g.Save();

                if (tnNext.Tag is Group && tnNext != _NRoot)
                {
                    Group gNext = (Group)tnNext.Tag;
                    gNext.Position -= 1;
                    gNext.Save();
                }
                else if (tnNext.Tag is DB)
                {
                    DB dbNext = (DB)tnNext.Tag;
                    dbNext.Position -= 1;
                    dbNext.Save();
                }
                else
                {
                    throw new NotSupportedException();
                }

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectGroup(g.IdParentGroup, g.Id);
            }
            else if (tn.Tag is DB)
            {
                DB db = (DB)tn.Tag;
                db.Position += 1;
                db.Save();

                if (tnNext.Tag is Group && tnNext != _NRoot)
                {
                    Group gNext = (Group)tnNext.Tag;
                    gNext.Position -= 1;
                    gNext.Save();
                }
                else if (tnNext.Tag is DB)
                {
                    DB dbNext = (DB)tnNext.Tag;
                    dbNext.Position -= 1;
                    dbNext.Save();
                }
                else
                {
                    throw new NotSupportedException();
                }

                RefreshConnectionsTreeControl();
                ConnectionTreeControlSelectDB(db.IdGroup, db.Id);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private bool _IgnoreTvaServersSelectionChanged = false;
        private void tvaServers_SelectionChanged(object sender, EventArgs e)
        {
            if (_IgnoreTvaServersSelectionChanged) return;
            UpdateServersButtons();
        }

        private void tvaServers_DoubleClick(object sender, EventArgs e)
        {
            Node? tn = (tvaConnections.SelectedNode == null ? null : (Node)tvaConnections.SelectedNode.Tag);

            if (tn != null && tn!.Tag is Tuple<Function, string> && ((Tuple<Function, string>)tn!.Tag).Item2 == "source_code")
            {
                EditFunction(((Tuple<Function, string>)tn!.Tag).Item1);
            }
        }

        private void EditFunction(Function f)
        {
            EditorTab.CreateEditorTabOptions o = new EditorTab.CreateEditorTabOptions();
            o.Title = f.Id;
            o.Text = $"CREATE OR REPLACE FUNCTION {f.IdSchema}.{f.Id} ({f.Arguments}) RETURNS {f.Returns} AS $$\r\n{f.SourceCode}\r\n$$ language plpgsql";
            o.Focus = true;
            CreateEditorTab(o);
        }

        private void UpdateNodeCheck(Node nEditedNode)
        {
            if (nEditedNode == null) throw new ArgumentException();

            bool v = nEditedNode.IsChecked;

            Stack<Node> pila = new Stack<Node>();

            foreach (Node tni in nEditedNode.Nodes) pila.Push(tni);

            while (pila.Count > 0)
            {
                Node tni = pila.Pop();
                if (!(tni.Tag is DB) && !(tni.Tag is Group)) continue;

                tni.IsChecked = v;

                foreach (Node tnj in tni.Nodes) pila.Push(tnj);
            }

            Node tn = nEditedNode.Parent;
            while (tn != null)
            {
                CheckState? cs = null;
                foreach (Node tni in tn.Nodes)
                {
                    if (!cs.HasValue)
                    {
                        cs = tni.CheckState;
                    }
                    else if (cs.Value != tni.CheckState)
                    {
                        cs = CheckState.Indeterminate;
                        break;
                    }
                }

                tn.CheckState = cs!.Value;

                tn = tn.Parent;
            }

            List<DB> dbs = new List<DB>();
            UpdateNodeCounter(_NRoot, dbs);
            UpdateRunButton(dbs);
        }

        private void ncb_CheckStateChanged(object? sender, TreePathEventArgs e)
        {
            Node n = _TreeModel.FindNode(e.Path)!;
            UpdateNodeCheck(n);
        }

        private int UpdateNodeCounter(Node tn, List<DB> dbs)
        {
            if (tn == null || !(tn.Tag is Group || tn == _NRoot)) throw new ArgumentException();

            int n = 0;
            foreach (Node tni in tn.Nodes)
            {
                if (tni.Tag is DB)
                {
                    if (tni.IsChecked)
                    {
                        n++;
                        dbs.Add((DB)tni.Tag);
                    }
                }
                else
                {
                    n += UpdateNodeCounter(tni, dbs);
                }
            }

            string nombre = (tn == _NRoot ? Properties.Text.all_databases : ((Group)tn.Tag).Name);
            if (n > 0)
            {
                tn.Text = nombre + " (" + n + ")";
            }
            else
            {
                tn.Text = nombre;
            }

            return n;
        }

        private void ClearSelectedNodesTreeView()
        {
            Stack<Node> pila = new Stack<Node>();
            pila.Push(_NRoot);

            while (pila.Count > 0)
            {
                Node tn = pila.Pop();

                tn.IsChecked = false;

                if (tn == _NRoot)
                {
                    tn.Text = Properties.Text.all_databases;
                }
                else if (tn.Tag is Group)
                {
                    tn.Text = ((Group)tn.Tag).Name;
                }
                else
                {
                    continue;
                }

                foreach (Node tni in tn.Nodes) pila.Push(tni);
            }
            tvaConnections.ClearSelection();
            tvaConnections.Refresh();
            UpdateRunButton(null);
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            ResetStructureCache();
            RefreshConnectionsTreeControl();
            UpdateServersButtons();
            ExpandServersTree();
        }

        private void tscmiNewGroup_Click(object sender, EventArgs e)
        {
            tsbNewGroup_Click(sender, e);
        }

        private void tscmiNewDB_Click(object sender, EventArgs e)
        {
            tsbNewDB_Click(sender, e);
        }

        private void tscmiExploreTable_Click(object sender, EventArgs e)
        {
            tsbExploreTable_Click(sender, e);
        }

        private void tscmiRecursiveRemove_Click(object sender, EventArgs e)
        {
            tsbRecursiveRemove_Click(sender, e);
        }

        private void tscmiCreateTableDiagram_Click(object sender, EventArgs e)
        {
            tsbCreateTableDiagram_Click(sender, e);
        }

        private void tscmiCopyText_Click(object sender, EventArgs e)
        {
            TreeNodeAdv tn = tvaConnections.SelectedNode;
            Clipboard.SetText(tn.ToString());
        }

        private void tscmiEdit_Click(object sender, EventArgs e)
        {
            tsbEdit_Click(sender, e);
        }

        private void tscmiRemove_Click(object sender, EventArgs e)
        {
            tsbRemove_Click(sender, e);
        }

        private void tscmiUp_Click(object sender, EventArgs e)
        {
            tsbUp_Click(sender, e);
        }

        private void tscmiDown_Click(object sender, EventArgs e)
        {
            tsbDown_Click(sender, e);
        }

        private void tscmiRefresh_Click(object sender, EventArgs e)
        {
            tsbRefresh_Click(sender, e);
        }

        private void tsbCollapseAll_Click(object sender, EventArgs e)
        {
            tvaConnections.Root.Children[0].Expand(true);

            foreach (TreeNodeAdv tnaGrupo in tvaConnections.Root.Children[0].Children)
            {
                tnaGrupo.Collapse(false);
            }
        }

        private void tvaConnections_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNodeAdv[] items = (TreeNodeAdv[])e.Item!;
            if (items.Length != 1) return;

            TreeNodeAdv tna = items[0];
            Node n = (Node)tna.Tag;

            if ((!(n.Tag is DB) && !(n.Tag is Group)) || n == _NRoot)
            {
                return;
            }

            tvaConnections.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        private void tvaConnections_DragOver(object sender, DragEventArgs e)
        {
            Tuple<TreeNodeAdv, int, Node>? t = CheckNodeDrop(tvaConnections.DropPosition, e);

            if (t == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            else
            {
                e.Effect = e.AllowedEffect;
            }
        }

        private Tuple<TreeNodeAdv, int, Node>? CheckNodeDrop(DropPosition dp, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(typeof(TreeNodeAdv[])) && dp.Node != null)
            {
                TreeNodeAdv[] treeNodes = (TreeNodeAdv[])e.Data!.GetData(typeof(TreeNodeAdv[]))!;
                if (treeNodes.Length != 1)
                {
                    return null;
                }

                TreeNodeAdv treeNode = treeNodes[0];
                TreeNodeAdv referenceTreeNode = dp.Node;

                int targetPosition;
                TreeNodeAdv targetParentTreeNode;

                switch (dp.Position)
                {
                    case NodePosition.Inside:
                        targetParentTreeNode = referenceTreeNode;
                        targetPosition = referenceTreeNode.Children.Count;
                        break;
                    case NodePosition.After:
                        targetParentTreeNode = referenceTreeNode.Parent;
                        targetPosition = targetParentTreeNode.Children.IndexOf(referenceTreeNode) + 1;
                        break;
                    case NodePosition.Before:
                        targetParentTreeNode = referenceTreeNode.Parent;
                        targetPosition = targetParentTreeNode.Children.IndexOf(referenceTreeNode);
                        break;
                    default:
                        throw new NotSupportedException();
                }

                Node targetParentNode = (Node)targetParentTreeNode.Tag;

                if (targetParentNode == null || (targetParentNode != _NRoot && !(targetParentNode.Tag is Group)))
                {
                    return null;
                }

                Node node = (Node)treeNode.Tag;
                if (node.Tag is Group)
                {
                    if (CheckRecursiveParent(targetParentTreeNode, treeNode))
                    {
                        return null;
                    }
                }

                if (treeNode.Parent == targetParentTreeNode)
                {
                    int currentIndex = treeNode.Parent.Children.IndexOf(treeNode);
                    if (targetPosition > currentIndex) targetPosition--;

                    if (currentIndex == targetPosition)
                    {
                        return null;
                    }
                }

                return new Tuple<TreeNodeAdv, int, Node>(targetParentTreeNode, targetPosition, node);
            }

            return null;
        }

        private bool CheckRecursiveParent(TreeNodeAdv child, TreeNodeAdv parent)
        {
            while (child != null)
            {
                if (parent == child)
                    return true;
                else
                    child = child.Parent;
            }
            return false;
        }

        private void tvaConnections_DragDrop(object sender, DragEventArgs e)
        {
            Tuple<TreeNodeAdv, int, Node>? t = CheckNodeDrop(tvaConnections.DropPosition, e);

            if (t != null)
            {
                Group targetGroup = (Group)((Node)t.Item1.Tag).Tag;
                int targetPosition = t.Item2;
                if (t.Item3.Tag is Group)
                {
                    Group g = (Group)t.Item3.Tag;
                    g.MoveTo(targetGroup, targetPosition);

                    RefreshConnectionsTreeControl();
                    ConnectionTreeControlSelectGroup(targetGroup.Id, g.Id);
                }
                else if (t.Item3.Tag is DB)
                {
                    DB db = (DB)t.Item3.Tag;
                    db.MoveTo(targetGroup, targetPosition);

                    RefreshConnectionsTreeControl();
                    ConnectionTreeControlSelectDB(targetGroup.Id, db.Id);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        private bool _TvaConnectionsHasFocus = false;
        private void tvaConnections_Enter(object sender, EventArgs e)
        {
            _TvaConnectionsHasFocus = true;
            tvaConnections.SelectionMode = TreeSelectionMode.Single;
        }

        private void tvaConnections_Leave(object sender, EventArgs e)
        {
            _TvaConnectionsHasFocus = false;
            UpdateServersButtons(true);
            tvaConnections.SelectionMode = TreeSelectionMode.Multi;
            tvaConnections.ClearSelection();

            Stack<TreeNodeAdv> pila = new Stack<TreeNodeAdv>();
            pila.Push(tvaConnections.Root);
        }

        #endregion

        #region "SQL editor & tab pages"
        public EditorTab CreateEditorTab(EditorTab.CreateEditorTabOptions o)
        {
            EditorTab et = new EditorTab(_Data!, this, o);

            tmrSaveTabs.Enabled = true;

            if (o.Focus)
            {
                tcSql.SelectedTab = et.TabPage;
                et.Fctb.Focus();
            }

            return et;
        }

        internal void EnableTimerSaveTabs()
        {
            tmrSaveTabs.Enabled = true;
        }

        internal void EnableTimerPosition(bool v)
        {
            tmrPosition.Enabled = v;
        }

        internal void SqlEditorProcessKey(CustomFctb sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && tsbRun.Enabled)
            {
                tsbRun_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.D | Keys.Control))
            {
                tsmiFormat_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.L | Keys.Control))
            {
                tsbCurrentTabLastTask_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyData == (Keys.Control | Keys.F) || e.KeyData == (Keys.Control | Keys.R))
            {
                ShowSearchAndReplace();
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
                CustomFctb tb = (CustomFctb)sender!;
                if (tb.SearchMatches != null && tb.SearchMatches.Count > 0)
                {
                    HideSearchAndReplace();
                }
                e.Handled = true;
            }
        }

        internal void SqlEditorProcessClick(CustomFctb sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
            {
                tsmiBack_Click(null, null);
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                tsmiForward_Click(null, null);
            }
            else if (e.Button == MouseButtons.Right)
            {
                cmsFctb.Show((Control)sender!, e.X, e.Y);
            }
        }

        internal void SqlEditorProcessParseTreeUpdated(CustomFctb sender)
        {
            if (tcSql.SelectedTab != null && tcSql.SelectedTab != tpNewTab && tcSql.SelectedTab.Controls[0] == sender)
            {
                RefreshErrors(sender);
            }
        }

        private void RefreshErrors(CustomFctb fctbSql)
        {
            tsddbErrors.DropDownItems.Clear();
            if (fctbSql.ParseTree != null && fctbSql.ParseTree.Status == ParseTreeStatus.Error)
            {
                foreach (Irony.LogMessage msg in fctbSql.ParseTree.ParserMessages)
                {
                    ToolStripMenuItem tsmiError = new ToolStripMenuItem();

                    tsmiError.Image = Properties.Resources.error;
                    tsmiError.Text = string.Format(Properties.Text.line_column, msg.Location.Line + 1, msg.Location.Column + 1) + ": " + msg.Message;
                    tsmiError.Click += new EventHandler(tsmiError_Click);
                    tsmiError.Tag = new Tuple<CustomFctb, Irony.LogMessage>(fctbSql, msg);
                    tsddbErrors.DropDownItems.Add(tsmiError);
                }

                tsddbErrors.Text = string.Format(Properties.Text.error_count, fctbSql.ParseTree.ParserMessages.Count);
                tsddbErrors.Image = Properties.Resources.error;
                tsddbErrors.Enabled = true;
            }
            else
            {
                tsddbErrors.Text = Properties.Text.no_errors;
                tsddbErrors.Image = Properties.Resources.ok;
                tsddbErrors.Enabled = false;
            }
        }

        private void fctbSql_Leave(object? sender, EventArgs e)
        {
            tmrPosition.Enabled = false;
        }

        private void tcSql_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pos = -1;

            TabControl tc = (TabControl)sender;
            if (tc.SelectedTab != null && tc.SelectedTab != tpNewTab)
            {
                EditorTab? s = (EditorTab?)tc.SelectedTab.Tag;

                if (s != null && s.LastTask != null)
                {
                    pos = lbResult.Items.IndexOf(s.LastTask);
                }

                CustomFctb fctbSql = (CustomFctb)tc.SelectedTab.Controls[0];
                fctbSql.Focus();
                RefreshErrors(fctbSql);
            }

            /*
            lbResult.SelectedIndices.Clear();
            lbResult.SelectedIndices.Add(pos);
            */
        }

        private void tcSql_TabClosing(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !CloseTab(tcSql.SelectedTab);
        }

        private bool CloseTab(TabPage tp)
        {
            EditorTab et = ((EditorTab)tp.Tag!);

            if (string.IsNullOrWhiteSpace(et.Fctb.Text))
            {
                et.Delete();
            }
            else
            {
                et.Save();
                et.SetClosed();
                _Data!.DeleteOldestTabs();
            }

            if (tcSql.SelectedTab == tp) tcSql.SelectedIndex = Math.Max(tcSql.SelectedIndex - 1, 0);
            tcSql.TabPages.Remove(tp);

            if (tcSql.TabPages.Count == 1)
            {
                CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
            }

            return true;
        }

        private bool SaveTab(TabPage tp)
        {
            EditorTab si = (EditorTab)tp.Tag!;
            string? rutaLocal = si.LocalPath;

            if (rutaLocal == null)
            {
                return SaveTabAs(tp);
            }
            else
            {
                try
                {
                    File.WriteAllText(rutaLocal, tp.Controls[0].Text);
                    if (tp.Text.EndsWith(" *")) tp.Text = tp.Text.Substring(0, tp.Text.Length - 2);
                    si.PendingSaveDB = true;
                    tmrSaveTabs.Enabled = true;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{rutaLocal}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool SaveTabAs(TabPage tp)
        {
            sfdSql.FileName = tp.Text;
            if (sfdSql.FileName.EndsWith(" *")) sfdSql.FileName = sfdSql.FileName.Substring(0, sfdSql.FileName.Length - 2);
            if (!sfdSql.FileName.Contains(".")) sfdSql.FileName += ".sql";

            if (sfdSql.ShowDialog(this) != DialogResult.OK) return false;

            try
            {
                File.WriteAllText(sfdSql.FileName, tp.Controls[0].Text);
                tp.Text = Path.GetFileName(sfdSql.FileName);
                tp.ToolTipText = sfdSql.FileName;
                EditorTab si = (EditorTab)tp.Tag!;
                si.PendingSaveDB = true;
                si.LocalPath = sfdSql.FileName;
                tmrSaveTabs.Enabled = true;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{sfdSql.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private TabPage? _MouseDownTabPage = null;
        private void tcSql_MouseDown(object sender, MouseEventArgs e)
        {
            var mousePosition = new Point(e.X, e.Y);
            int index = tcSql.GetActiveIndex(mousePosition);

            _MouseDownTabPage = null;

            if (index != -1)
            {
                TabPage tp = tcSql.TabPages[index];

                if (tp == tpNewTab)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
                    }
                }
                else
                {
                    _MouseDownTabPage = tp;
                }
            }
        }

        private void tcSql_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                var mousePosition = new Point(e.X, e.Y);
                int index = tcSql.GetActiveIndex(mousePosition);

                if (index != -1)
                {
                    TabPage tp = tcSql.TabPages[index];

                    if (tp == _MouseDownTabPage)
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            cmsTabs.Tag = tp;
                            tsmiCloseTab.Visible = tp != tpNewTab;
                            tsmiCloseAllTabsExceptThisOne.Visible = tp != tpNewTab;

                            EditorTab et = ((EditorTab)tp.Tag!);
                            tsmiCopyPath.Visible = et.LocalPath != null;
                            tsmiOpenFolder.Visible = et.LocalPath != null;

                            cmsTabs.Show(Cursor.Position);
                        }
                        else if (e.Button == MouseButtons.Middle)
                        {
                            if (tp != tpNewTab)
                            {
                                CloseTab(tp);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }

                }
            }

            _MouseDownTabPage = null;
        }

        private void tcSql_ReorderedTabs(object sender, EventArgs e)
        {
            tmrSaveTabs.Enabled = true;
        }

        private void tsmiCloseTab_Click(object sender, EventArgs e)
        {
            TabPage tp = (TabPage)cmsTabs.Tag!;
            CloseTab(tp);
        }

        private void tsmiCloseAllTabs_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tcSql.TabPages)
            {
                if (tp == tpNewTab) continue;
                ((EditorTab)tp.Tag!).SetClosed();

                tcSql.TabPages.Remove(tp);
            }

            CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
        }

        private void tsmiCloseAllTabsExceptThisOne_Click(object sender, EventArgs e)
        {
            TabPage tp = (TabPage)cmsTabs.Tag!;
            foreach (TabPage tpi in tcSql.TabPages)
            {
                if (tpi == tpNewTab || tpi == tp) continue;
                ((EditorTab)tpi.Tag!).SetClosed();

                tcSql.TabPages.Remove(tpi);
            }
        }

        private void tsmiClosedTabsLog_Click(object sender, EventArgs e)
        {
            ShowLogForm(true);
        }

        private void tsmiCopyPath_Click(object sender, EventArgs e)
        {
            EditorTab et = ((EditorTab)((TabPage)cmsTabs.Tag!).Tag!);

            System.Windows.Forms.Clipboard.SetText(et.LocalPath!);
        }

        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            EditorTab et = ((EditorTab)((TabPage)cmsTabs.Tag!).Tag!);

            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + et.LocalPath! + "\"");
        }

        private void tsmiReopenLastClosedTab_Click(object sender, EventArgs e)
        {
            ClosedEditorTab? cet = _Data!.GetLastClosedEditorTab();
            if (cet == null) return;

            EditorTab et = new EditorTab(_Data!, cet, this, tcSql.TabCount - 1);
            tcSql.SelectedTab = et.TabPage;
        }

        #endregion

        #region "SQL editor toolbar & context menu"

        private void UpdateRunButton(List<DB>? dbs)
        {
            tsmiRun.Enabled = false;
            tsbRun.Enabled = false;
            tsmiExportCsv.Enabled = false;
            tsbExportCsv.Enabled = false;
            tsmiCopyToTable.Enabled = false;

            if (dbs == null || dbs.Count == 0)
            {
                tsbRun.Text = "";
                tsmiRun.Text = string.Format(Properties.Text.run_on, 0);
            }
            else
            {
                tsmiRun.Enabled = true;
                tsbRun.Enabled = true;
                tsmiExportCsv.Enabled = true;
                tsbExportCsv.Enabled = true;
                tsmiCopyToTable.Enabled = true;

                List<Group> wholeGroups = new List<Group>();
                List<DB> singleDBs = new List<DB>();

                Stack<Group> stack1 = new Stack<Group>();
                stack1.Push(_Data!.RootGroup);

                while (stack1.Count > 0)
                {
                    Group g1 = stack1.Pop();

                    Stack<Group> stack2 = new Stack<Group>();
                    stack2.Push(g1);

                    bool allSelected1 = true;
                    while (stack2.Count > 0)
                    {
                        Group g2 = stack2.Pop();

                        bool allSelected2 = true;
                        foreach (DB db1 in g2.DBs)
                        {
                            bool found = false;
                            foreach (DB db2 in dbs)
                            {
                                if (db1 == db2)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                allSelected2 = false;
                                break;
                            }
                        }

                        if (allSelected2)
                        {
                            foreach (Group childGroup in g2.ChildGroups)
                            {
                                stack2.Push(childGroup);
                            }
                        }
                        else
                        {
                            allSelected1 = false;
                            break;
                        }
                    }

                    if (allSelected1)
                    {
                        wholeGroups.Add(g1);
                    }
                    else
                    {
                        foreach (Group childGroup in g1.ChildGroups)
                        {
                            stack1.Push(childGroup);
                        }

                        singleDBs.AddRange(g1.DBs.Where(dbi => dbs.Contains(dbi)));
                    }
                }

                string txt;
                if (singleDBs.Count > 0)
                {
                    txt = string.Join(", ", singleDBs.Select(dbi => dbi.Alias).ToArray());
                }
                else
                {
                    txt = "";
                }
                if (wholeGroups.Count > 0 && singleDBs.Count > 0)
                {
                    txt += ", ";
                }
                if (wholeGroups.Count > 0)
                {
                    txt += string.Join(", ", wholeGroups.Select(gi => gi.ParentGroup == null ? Properties.Text.all_databases : gi.Name).ToArray());
                }

                txt = Data.AutoEllipsis(txt)!;

                tsbRun.Text = "[" + dbs.Count + "] " + txt;
                tsmiRun.Text = string.Format(Properties.Text.run_on, dbs.Count) + ": " + txt;
            }
        }

        private void ShowLogForm(bool closedTabsMode)
        {
            LogForm f = new LogForm(_Data!, closedTabsMode);
            f.ShowDialog(this);

            if (f.DialogResult == DialogResult.OK)
            {
                if (f.SelectedLog != null)
                {
                    EditorTab.CreateEditorTabOptions o = new EditorTab.CreateEditorTabOptions();
                    o.Text = f.SelectedLog.SqlText;
                    o.Focus = true;
                    CreateEditorTab(o);
                }
                else if (f.SelectedClosedTab != null)
                {
                    EditorTab et = new EditorTab(_Data!, f.SelectedClosedTab, this, tcSql.TabCount - 1);
                    tcSql.SelectedTab = et.TabPage;
                }
            }
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
        private void tsbRun_Click(object? sender, EventArgs? e)
        {
            EditorTab s = (EditorTab)tcSql.SelectedTab.Tag!;
            string sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).SelectedText;
            if (sql == "")
            {
                sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).Text;
            }
            else if (_Data!.Config.ShowWarningSelectedText)
            {
                if (MessageBox.Show(this, Properties.Text.warning_run_selected_text, Properties.Text.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    _Data.Config.ShowWarningSelectedText = false;
                    _Data.Config.Save();
                }
                else
                {
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                MessageBox.Show(Properties.Text.warning_empty_query, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<DB> dbs = SelectedDBs;

            if (dbs.Count == 0)
            {
                MessageBox.Show(Properties.Text.warning_no_selected_dbs, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Log h = new Log(_Data!);

            h.SqlText = sql;

            PgTaskIntegrator? ti = null;
            if (dbs.Count > 1 && (_Data!.Config.MergeTables || _Data!.Config.TransactionMode == Config.TransactionModeEnum.AutoCoordinated))
            {
                ti = new PgTaskIntegrator(_Data, new PgTask.OnUpdate(Task_OnUpdate), sql, true);
            }

            Config.TransactionModeEnum modoTransacciones = _Data!.Config.TransactionMode;
            if (dbs.Count == 1 && modoTransacciones == Config.TransactionModeEnum.AutoCoordinated)
            {
                modoTransacciones = Config.TransactionModeEnum.AutoSingle;
            }

            List<PgTaskExecutorSqlTables> tess = new List<PgTaskExecutorSqlTables>();
            foreach (DB db in dbs)
            {
                PgTaskExecutorSqlTables tes = new PgTaskExecutorSqlTables(_Data!, db, new PgTask.OnUpdate(Task_OnUpdate), sql, modoTransacciones, _Data!.Config.TransactionLevel, _Data!.PGSimpleLanguageData, ti);
                tess.Add(tes);
                h.DBIds.Add(db.Id);
            }

            if (ti == null)
            {
                foreach (PgTaskExecutorSqlTables tes in tess)
                {
                    tes.Start();
                    s.LastTask = tes;
                }
            }
            else
            {
                foreach (PgTaskExecutorSqlTables tes in tess)
                {
                    ti.Integrate(tes);
                }

                ti.Start();
                s.LastTask = ti;
            }

            h.Save();
            _Data.CheckAppDbFileSize();

            if (!_Data!.Config.KeepServerSelection)
            {
                ClearSelectedNodesTreeView();
            }

            ResetStructureCache();

            tsmiRun.Enabled = false;
            tsbRun.Enabled = false;
            tsmiExportCsv.Enabled = false;
            tsbExportCsv.Enabled = false;
            tsmiCopyToTable.Enabled = false;
            tmrReenableRunButton.Enabled = true;
        }

        private void tsmiRun_Click(object sender, EventArgs e)
        {
            tsbRun_Click(sender, e);
        }

        private void tsbHistory_Click(object sender, EventArgs e)
        {
            ShowLogForm(false);
        }

        private void tsmiHistory_Click(object sender, EventArgs e)
        {
            tsbHistory_Click(sender, e);
        }

        private void tmrPosition_Tick(object sender, EventArgs e)
        {
            CustomFctb fctbSql = (CustomFctb)tcSql.SelectedTab.Controls[0];
            Place p = fctbSql.PositionToPlace(fctbSql.SelectionStart);
            tslPosition.Text = string.Format(Properties.Text.line_column, p.iLine + 1, p.iChar + 1);
        }

        private void SaveTabs()
        {
            tmrSaveTabs.Enabled = false;

            for (int i = 0; i < tcSql.TabCount; i++)
            {
                TabPage tp = tcSql.TabPages[i];
                if (tp == tpNewTab) continue;
                EditorTab si = (EditorTab)tp.Tag!;
                si.Position = (short)i;
                si.Save();
            }

            if (Text.EndsWith(" *")) Text = Text.Substring(0, Text.Length - 2);
        }

        private void tmrSaveTabs_Tick(object sender, EventArgs e)
        {
            try
            {
                SaveTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_saving + "\r\n" + ex.Message, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tsmiSearchAndReplace_Click(object sender, EventArgs e)
        {
            ShowSearchAndReplace();
        }

        private void tsbSearchAndReplace_Click(object sender, EventArgs e)
        {
            tsmiSearchAndReplace_Click(sender, e);
        }

        private void tsmiGoTo_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.ShowGoToDialog();
        }

        private void tsbGoTo_Click(object sender, EventArgs e)
        {
            tsmiGoTo_Click(sender, e);
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.Cut();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.Copy();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.Paste();
        }

        private void tsmiFormat_Click(object? sender, EventArgs? e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;

            Parser parserGlobal = new Parser(_Data!.PGSimpleLanguageData);

            string globalText;
            int prevIndentation = 0;
            if (string.IsNullOrEmpty(fctb.SelectedText))
            {
                globalText = fctb.Text;
            }
            else
            {
                int startLine;
                int endLine;

                if (fctb.Selection.Start.iLine < fctb.Selection.End.iLine)
                {
                    startLine = fctb.Selection.Start.iLine;
                    endLine = fctb.Selection.End.iLine;
                }
                else
                {
                    startLine = fctb.Selection.End.iLine;
                    endLine = fctb.Selection.Start.iLine;
                }

                fctb.Selection.Start = new Place(0, startLine);
                fctb.Selection.End = new Place(fctb.Lines[endLine].Length, endLine);

                globalText = fctb.SelectedText;

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

            fctb.BeginAutoUndo();

            if (string.IsNullOrEmpty(fctb.SelectedText))
            {
                fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(fctb.TextSource));
                fctb.Selection.Start = new Place(0, 0);
                fctb.Selection.End = new Place(fctb.Lines[fctb.LinesCount - 1].Length, fctb.LinesCount - 1);
            }

            fctb.InsertText(formattedText);
            fctb.TextSource.Manager.ExecuteCommand(new SelectCommand(fctb.TextSource));

            fctb.EndAutoUndo();
            fctb.Focus();
        }

        private void tsbFormat_Click(object? sender, EventArgs? e)
        {
            tsmiFormat_Click(sender, e);
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            ofdSql.FileName = "*.sql";
            if (ofdSql.ShowDialog(this) != DialogResult.OK) return;


            try
            {
                string txt = File.ReadAllText(ofdSql.FileName);

                TabPage? replaceTab = null;
                if (string.IsNullOrWhiteSpace(tcSql.SelectedTab.Controls[0].Text) && (tcSql.SelectedTab.Text == Properties.Text.new_doc_title || tcSql.SelectedTab.Text == Properties.Text.new_doc_title + " *"))
                {
                    replaceTab = tcSql.SelectedTab;
                }

                CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Text = txt, Title = Path.GetFileName(ofdSql.FileName), Focus = true, Path = ofdSql.FileName, PendingFileSave = false });

                if (replaceTab != null)
                {
                    CloseTab(replaceTab);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_opening_file + $":\r\n{ofdSql.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            tsmiOpen_Click(sender, e);
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            SaveTab(tcSql.SelectedTab);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveTab(tcSql.SelectedTab);
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            SaveTabAs(tcSql.SelectedTab);
        }

        private void tsmiSaveAll_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tcSql.TabPages)
            {
                if (tp != tpNewTab && tp.Text.EndsWith(" *"))
                {
                    tcSql.SelectedTab = tp;
                    SaveTab(tp);
                }
            }
        }

        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            tsmiSaveAll_Click(sender, e);
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            CloseTab(tcSql.SelectedTab);
        }

        private void tsmiCloseAll_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tcSql.TabPages)
            {
                if (tp == tpNewTab) continue;
                ((EditorTab)tp.Tag!).SetClosed();

                tcSql.TabPages.Remove(tp);
            }

            CreateEditorTab(new EditorTab.CreateEditorTabOptions() { Focus = true, PendingFileSave = false });
        }

        private void tsmiChangePassword_Click(object sender, EventArgs e)
        {
            PasswordForm f = new PasswordForm();
            f.ShowDialog(this);

            if (f.DialogResult == DialogResult.OK && f.Password != _Data!.Password)
            {
                _Data.Password = f.Password!;
            }
        }

        private void tsmiUndo_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.Undo();
        }

        private void tsmiRedo_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.Redo();
        }

        private void tsmiBack_Click(object? sender, EventArgs? e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.NavigateBackward();
        }

        private void tsmiForward_Click(object? sender, EventArgs? e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.NavigateForward();
        }

        private void tsmiIncreaseFont_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.ChangeFontSize(2);
            _Data!.Config.FontSize = (int)Math.Round(fctb.Font.Size);
            _Data!.Config.Save();
        }

        private void tsmiReduceFont_Click(object sender, EventArgs e)
        {
            CustomFctb fctb = ((EditorTab)tcSql.SelectedTab.Tag!).Fctb;
            fctb.ChangeFontSize(-2);
            _Data!.Config.FontSize = (int)Math.Round(fctb.Font.Size);
            _Data!.Config.Save();
        }

        private void tsmMoreOptions_Click(object sender, EventArgs e)
        {
            ConfigForm f = new ConfigForm(_Data!);
            f.ShowDialog(this);

            if (f.DialogResult == DialogResult.OK)
            {
                foreach (TabPage tp in tcSql.TabPages)
                {
                    if (tp == tpNewTab) continue;
                    ((EditorTab)tp.Tag!).AutocompleteMenu.AppearInterval = (_Data!.Config.AutocompleteDelay == 0 ? int.MaxValue : _Data!.Config.AutocompleteDelay);
                }

                RefreshLanguage();
                RefreshTransactionsConfig();
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.ShowDialog(this);
        }

        private void tsmiNewDiagram_Click(object sender, EventArgs e)
        {
            tsbNewDiagram_Click(sender, e);
        }

        private void tsmiOpenDiagram_Click(object sender, EventArgs e)
        {
            tsbOpenDiagram_Click(sender, e);
        }

        private void tsbExportCsv_Click(object sender, EventArgs e)
        {
            EditorTab s = (EditorTab)tcSql.SelectedTab.Tag!;
            string sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).SelectedText;
            if (sql == "")
            {
                sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).Text;
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                MessageBox.Show(Properties.Text.warning_empty_query, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<DB> dbs = SelectedDBs;

            if (dbs.Count == 0)
            {
                MessageBox.Show(Properties.Text.warning_no_selected_dbs, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sfdCsv.FileName = tcSql.SelectedTab.Text;
            if (sfdCsv.FileName.EndsWith(" *")) sfdCsv.FileName = sfdCsv.FileName.Substring(0, sfdCsv.FileName.Length - 2);
            if (!sfdCsv.FileName.Contains(".")) sfdCsv.FileName += ".csv";

            if (sfdCsv.ShowDialog(this) != DialogResult.OK) return;

            Log h = new Log(_Data!);

            h.SqlText = sql;

            Config.TransactionModeEnum modoTransacciones = _Data!.Config.TransactionMode;
            if (dbs.Count == 1 && modoTransacciones == Config.TransactionModeEnum.AutoCoordinated)
            {
                modoTransacciones = Config.TransactionModeEnum.AutoSingle;
            }

            foreach (DB db in dbs)
            {
                h.DBIds.Add(db.Id);
            }

            PgTaskExecutorSqlCsv t = new PgTaskExecutorSqlCsv(_Data!, dbs, new PgTask.OnUpdate(Task_OnUpdate), sql, modoTransacciones, _Data!.Config.TransactionLevel, _Data!.PGSimpleLanguageData, sfdCsv.FileName);
            t.Start();

            s.LastTask = t;

            h.Save();
            _Data.CheckAppDbFileSize();

            if (!_Data!.Config.KeepServerSelection)
            {
                ClearSelectedNodesTreeView();
            }

            tsmiRun.Enabled = false;
            tsbRun.Enabled = false;
            tsmiExportCsv.Enabled = false;
            tsbExportCsv.Enabled = false;
            tsmiCopyToTable.Enabled = false;
            tmrReenableRunButton.Enabled = true;
        }

        private void tsmiExportCsv_Click(object sender, EventArgs e)
        {
            tsbExportCsv_Click(sender, e);
        }

        private void tsmiCopyToTable_Click(object sender, EventArgs e)
        {
            EditorTab s = (EditorTab)tcSql.SelectedTab.Tag!;
            string sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).SelectedText;
            if (sql == "")
            {
                sql = ((CustomFctb)tcSql.SelectedTab.Controls[0]).Text;
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                MessageBox.Show(Properties.Text.warning_empty_query, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<DB> dbs = SelectedDBs;

            if (dbs.Count == 0)
            {
                MessageBox.Show(Properties.Text.warning_no_selected_dbs, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Log h = new Log(_Data!);

            h.SqlText = sql;

            Config.TransactionModeEnum modoTransacciones = _Data!.Config.TransactionMode;
            if (dbs.Count == 1 && modoTransacciones == Config.TransactionModeEnum.AutoCoordinated)
            {
                modoTransacciones = Config.TransactionModeEnum.AutoSingle;
            }

            foreach (DB db in dbs)
            {
                h.DBIds.Add(db.Id);
            }

            PgTaskExecutorSqlCopyToTable t = new PgTaskExecutorSqlCopyToTable(_Data!, dbs, new PgTask.OnUpdate(Task_OnUpdate), sql, modoTransacciones, _Data!.Config.TransactionLevel, _Data!.PGSimpleLanguageData);
            t.Start();

            InsertIntoTableForm f = new InsertIntoTableForm(_Data, t);
            f.Show(this);

            s.LastTask = t;

            h.Save();
            _Data.CheckAppDbFileSize();

            if (!_Data!.Config.KeepServerSelection)
            {
                ClearSelectedNodesTreeView();
            }

            tsmiRun.Enabled = false;
            tsbRun.Enabled = false;
            tsmiExportCsv.Enabled = false;
            tsbExportCsv.Enabled = false;
            tsmiCopyToTable.Enabled = false;
            tmrReenableRunButton.Enabled = true;
        }

        private void tsmiTransactionModeManual_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionMode = Config.TransactionModeEnum.Manual;
            RefreshTransactionsConfig();
        }

        private void tsmiTransactionModeAutoSingle_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionMode = Config.TransactionModeEnum.AutoSingle;
            RefreshTransactionsConfig();
        }

        private void tsmiTransactionModeAutoCoordinated_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionMode = Config.TransactionModeEnum.AutoCoordinated;
            RefreshTransactionsConfig();
        }

        private void tsmiTransactionLevelReadCommitted_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionLevel = Config.TransactionLevelEnum.ReadCommited;
            RefreshTransactionsConfig();
        }

        private void tsmiTransactionLevelRepeatableRead_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionLevel = Config.TransactionLevelEnum.RepeatableRead;
            RefreshTransactionsConfig();
        }

        private void tsmiTransactionLevelSerializable_Click(object sender, EventArgs e)
        {
            _Data!.Config.TransactionLevel = Config.TransactionLevelEnum.Serializable;
            RefreshTransactionsConfig();
        }

        private void tmrReenableRunButton_Tick(object sender, EventArgs e)
        {
            List<DB> dbs = new List<DB>();
            UpdateNodeCounter(_NRoot, dbs);
            UpdateRunButton(dbs);

            tmrReenableRunButton.Enabled = false;
        }

        #endregion

        #region "Tasks & results"
        void Task_OnUpdate(PgTask t)
        {
            lbResult.Invoke((MethodInvoker)delegate
            {
                RefreshTaskListItem(t);
            });
        }

        private void RefreshTaskListItem(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                int pos = lbResult.Items.IndexOf(t);

                if (pos == -1)
                {
                    lbResult.Items.Insert(0, t);
                    tsbStopSelected.Visible = true;
                    tsbRemoveSelected.Visible = true;

                    _IgnoreLbResult_SelectedIndexChanged = true;
                    lbResult.SelectedIndices.Clear();
                    lbResult.SelectedIndices.Add(0);
                    _IgnoreLbResult_SelectedIndexChanged = false;
                    fctbResult.Clear();
                    gvTable.Tag = null;
                    gvTable.DataSource = null;
                    tsddbTables.Text = Properties.Text.no_results;
                    tsddbTables.DropDownItems.Clear();
                    RefreshSelectedResult();
                }
                else if (lbResult.SelectedIndices.Count == 1 && pos == lbResult.SelectedIndex)
                {
                    RefreshSelectedResult();
                }


                switch (t.State)
                {
                    case PgTask.StateEnum.Init:
                        break;
                    case PgTask.StateEnum.Running:
                        tmrResult.Enabled = true;
                        tsbStopAll.Visible = true;
                        break;
                    case PgTask.StateEnum.Finished:
                        tsbRemoveAll.Visible = true;

                        bool algunoSinTerminar = false;
                        foreach (PgTask ti in lbResult.Items)
                        {
                            if (ti.State != PgTask.StateEnum.Finished)
                            {
                                algunoSinTerminar = true;
                                break;
                            }
                        }
                        if (!algunoSinTerminar)
                        {
                            tsbStopAll.Visible = false;
                        }

                        break;
                    default:
                        throw new NotSupportedException();
                }

                lbResult.Refresh();
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private bool _IgnoreLbResult_SelectedIndexChanged = false;
        private void lbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_IgnoreLbResult_SelectedIndexChanged) return;
            lbResult.Refresh();
            fctbResult.Clear();
            gvTable.Tag = null;
            gvTable.DataSource = null;
            tsddbTables.Text = Properties.Text.no_results;
            tsddbTables.DropDownItems.Clear();
            RefreshSelectedResult();
        }

        private void RefreshSelectedResult()
        {
            _Mutex.WaitOne();
            try
            {
                _GvTable_IgnoreEvents = true;
                if (lbResult.SelectedIndices.Count > 0)
                {
                    List<PgTask> l = ListTasks(true);
                    tsbStopSelected.Visible = l.Any(ti => ti.State == PgTask.StateEnum.Running);
                    tsbRemoveSelected.Visible = l.Any(ti => ti.State == PgTask.StateEnum.Finished);
                }
                else
                {
                    tsbStopSelected.Visible = false;
                    tsbRemoveSelected.Visible = false;
                }

                if (lbResult.SelectedIndices.Count != 1)
                {
                    gvTable.Tag = null;
                    gvTable.DataSource = null;
                    fctbResult.Clear();
                    fctbExecutedSql.Text = "";
                    fctbExecutedSql.ClearUndo();
                    tsbEditExecutedSql.Enabled = false;
                    tsddbTables.Visible = false;
                }
                else
                {
                    PgTask t;
                    t = (PgTask)lbResult.SelectedItem!;

                    int scrollAnt = fctbResult.VerticalScroll.Value;
                    FastColoredTextBoxNS.Range selAnt = fctbResult.Selection.Clone();

                    fctbResult.BeginUpdate();
                    fctbResult.Selection.BeginUpdate();
                    fctbResult.Clear();

                    t.PrintLog(fctbResult, _FctbResultStyles);

                    if (t.State == PgTask.StateEnum.Finished)
                    {
                        tmrFitGridColumns.Enabled = true;

                        if (t.Exception == null)
                        {
                            //fctbResult.ForeColor = Color.DarkGreen;
                        }
                        else
                        {
                            //fctbResult.ForeColor = Color.DarkRed;
                        }

                        if (t.Queries.Count > 0 && t.Exception == null)
                        {
                            tcResult.SelectedIndex = 1;
                        }
                        else
                        {
                            tcResult.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        //fctbResult.ForeColor = Color.DarkBlue;
                        fctbResult.AppendText("\r\n\r\n");
                        fctbResult.AppendText($"{Properties.Text.executing_since}: {t.StartTimestamp!.Value:g}", _FctbResultStyles[LogStyle.TaskIsRunning]);
                    }

                    bool tsddbTablesPrevEmpty = tsddbTables.DropDown.Items.Count == 0;
                    for (int i = tsddbTables.DropDown.Items.Count; i < t.Queries.Count; i++)
                    {
                        Query c = t.Queries[i];
                        ToolStripItem tsi = tsddbTables.DropDownItems.Add(Data.AutoEllipsis(c.Description, 250), global::PgMulti.Properties.Resources.tabla);
                        tsi.Tag = c;
                    }
                    tsddbTables.Visible = true;
                    if (tsddbTablesPrevEmpty && tsddbTables.DropDown.Items.Count > 0)
                    {
                        tsddbTables.Text = Data.AutoEllipsis(t.Queries[0].Description, 150);
                        t.Queries[0].ShowInGridView(gvTable, tsbDeleteRows, tsddbInsertRow);
                    }

                    fctbExecutedSql.Text = t.Sql;
                    fctbExecutedSql.ClearUndo();
                    tsbEditExecutedSql.Enabled = true;

                    if (_AutomaticScroll)
                    {
                        fctbResult.GoEnd();
                    }
                    else
                    {
                        fctbResult.Selection = selAnt;
                        fctbResult.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, fctbResult.VerticalScroll.Value, scrollAnt, ScrollOrientation.VerticalScroll), true);
                    }

                    fctbResult.Selection.EndUpdate();
                    fctbResult.ClearUndo();
                    fctbResult.EndUpdate();

                    fctbExecutedSql.SelectionStart = 0;
                    fctbExecutedSql.SelectionLength = 0;
                }
                _GvTable_IgnoreEvents = false;
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private void tsddbTables_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Query c = (Query)e.ClickedItem!.Tag;
            _GvTable_IgnoreEvents = true;
            c.ShowInGridView(gvTable, tsbDeleteRows, tsddbInsertRow);
            _GvTable_IgnoreEvents = false;
            tsddbTables.Text = e.ClickedItem.Text;
        }

        private void lbResult_DrawItem(object sender, DrawItemEventArgs e)
        {
            _Mutex.WaitOne();
            try
            {
                if (e.Index == -1) return;
                PgTask? t = (PgTask)lbResult.Items[e.Index];

                e.DrawBackground();

                Color textColor;
                Color backColor;
                Color? progressColor = null;

                switch (t.State)
                {
                    case PgTask.StateEnum.Init:
                        textColor = Color.DarkGray;
                        backColor = Color.White;
                        break;
                    case PgTask.StateEnum.Running:
                        textColor = Color.DarkBlue;
                        backColor = Color.White;
                        progressColor = Color.LightGreen;
                        break;
                    case PgTask.StateEnum.Finished:
                        if (t.Exception == null)
                        {
                            textColor = Color.DarkGreen;
                            backColor = Color.White;
                        }
                        else
                        {
                            textColor = Color.DarkRed;
                            backColor = Color.White;
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }

                if (lbResult.SelectedIndices.Contains(e.Index))
                {
                    Color tmp = backColor;
                    backColor = textColor;
                    textColor = tmp;

                    if (t.State == PgTask.StateEnum.Running)
                    {
                        progressColor = Color.DarkGreen;
                    }
                }

                using (var backBrush = new SolidBrush(backColor))
                using (var foreBrush = new SolidBrush(textColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);

                    if (t.State == PgTask.StateEnum.Running)
                    {
                        using (var progressBrush = new SolidBrush(progressColor!.Value))
                        {
                            Rectangle r = e.Bounds;
                            if (t is PgTaskExecutorSqlCsv)
                            {
                                PgTaskExecutorSqlCsv tcsv = (PgTaskExecutorSqlCsv)t;
                                if (tcsv.DBs.Count > 1)
                                {
                                    r.Width = (tcsv.CurrentDBIndex * r.Width) / tcsv.DBs.Count;
                                }
                                else
                                {
                                    r.Width = (t.CurrentStatementIndex * r.Width) / t.StatementCount;
                                }
                            }
                            else
                            {
                                r.Width = (t.CurrentStatementIndex * r.Width) / t.StatementCount;
                            }
                            e.Graphics.FillRectangle(progressBrush, r);
                        }
                    }

                    e.Graphics.DrawString(t.ToString(), lbResult.Font, foreBrush,
                                      e.Bounds, StringFormat.GenericTypographic);
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private void lbResult_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            _Mutex.WaitOne();
            try
            {
                if (e.Index == -1) return;
                e.ItemHeight = lbResult.Font.Height;
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private void lbResult_Resize(object sender, EventArgs e)
        {
            lbResult.Invalidate();
        }

        private void gvTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void gvTable_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            DataGridViewCell cell = gvTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ToolTipText = "";
            if (cell.Value == DBNull.Value) return;
            string text = Data.AutoEllipsis(cell.Value.ToString(), 300)!;
            if (text == "") return;
            e.ToolTipText = text;
        }

        private void tsbRemoveSelected_Click(object sender, EventArgs e)
        {
            RemoveResults(true);
        }

        private void tsbRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveResults(false);
        }

        private void RemoveResults(bool soloSeleccionados)
        {
            _Mutex.WaitOne();
            try
            {
                foreach (PgTask t in ListTasks(soloSeleccionados))
                {
                    if (t.State != PgTask.StateEnum.Finished) continue;

                    lbResult.Items.Remove(t);
                }

                if (lbResult.Items.Count == 0)
                {
                    tsbRemoveAll.Visible = false;
                }

                if (lbResult.SelectedItems.Count == 0)
                {
                    tsbRemoveSelected.Visible = false;
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private void tsbStopSelected_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Text.confirm_stop_selected, Properties.Text.error, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            StopTasks(true);
        }

        private void tsbDetenerTodos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Text.confirm_stop_all, Properties.Text.error, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            StopTasks(false);
        }

        private void StopTasks(bool selectedOnly)
        {
            foreach (PgTask t in ListTasks(selectedOnly))
            {
                if (t.State != PgTask.StateEnum.Running) continue;

                t.Cancel();
            }
        }

        private void tsbCurrentTabLastTask_Click(object? sender, EventArgs? e)
        {
            int pos = -1;
            if (tcSql.SelectedTab != null && tcSql.SelectedTab != tpNewTab)
            {
                EditorTab? s = (EditorTab?)tcSql.SelectedTab.Tag;

                if (s != null && s.LastTask != null)
                {
                    pos = lbResult.Items.IndexOf(s.LastTask);
                }
            }

            lbResult.SelectedIndices.Clear();
            lbResult.SelectedIndices.Add(pos);
        }

        private void tmrResult_Tick(object sender, EventArgs e)
        {
            bool anyRunning = false;

            foreach (PgTask t in ListTasks(false))
            {
                if (t.State != PgTask.StateEnum.Running) continue;

                RefreshTaskListItem(t);
                anyRunning = true;
            }

            if (!anyRunning)
            {
                tmrResult.Enabled = false;
            }
        }

        private void tsbEditExecutedSql_Click(object sender, EventArgs e)
        {
            EditorTab.CreateEditorTabOptions o = new EditorTab.CreateEditorTabOptions();
            o.Text = fctbExecutedSql.Text;
            o.Focus = true;
            CreateEditorTab(o);
        }

        private List<PgTask> ListTasks(bool selectedOnly)
        {
            _Mutex.WaitOne();
            try
            {
                List<PgTask> l = new List<PgTask>();

                if (selectedOnly)
                {
                    foreach (object o in lbResult.SelectedItems)
                    {
                        l.Add((PgTask)o);
                    }
                }
                else
                {
                    foreach (object o in lbResult.Items)
                    {
                        l.Add((PgTask)o);
                    }
                }

                return l;
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        private void tsddbAutoScroll_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _AutomaticScroll = ((string)e.ClickedItem!.Tag == "auto");
            tsddbAutoScroll.Image = e.ClickedItem!.Image;
            tsddbAutoScroll.Text = e.ClickedItem!.Text;
        }

        private void tmrFitGridColumns_Tick(object sender, EventArgs e)
        {
            tmrFitGridColumns.Enabled = false;
            gvTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            foreach (DataGridViewColumn c in gvTable.Columns)
            {
                if (c.Tag != null)
                {
                    Query.QueryColumn qc = (Query.QueryColumn)c.Tag;
                    if (qc.Column != null && qc.Column.IsBoolean)
                    {
                        c.Width = Math.Min(c.Width + 40, 700);
                    }
                    else
                    {
                        c.Width = Math.Min(c.Width + 18, 700);
                    }
                }
            }
        }

        private void gvTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != -1 && gvTable.Columns[e.ColumnIndex].Tag != null && ((Query.QueryColumn)gvTable.Columns[e.ColumnIndex].Tag!).EditableOnEdit && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.PaintContent(e.ClipBounds);

                Rectangle r;
                r = e.CellBounds;
                r.X += r.Width - 34;
                r.Y = (r.Height - 16) / 2 + 2;
                r.Width = 16;
                r.Height = 16;
                e.Graphics.DrawImage(Properties.Resources.editar, r);

                e.Handled = true;
            }
        }

        private void gvTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                gvTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            else
            {
                gvTable.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
        }

        private void tsddbInsertRow_Click(object sender, EventArgs e)
        {
            Query q = (Query)gvTable.Tag!;

            if (q is QueryExecutorSql)
            {
                QueryExecutorSql ces = (QueryExecutorSql)q;
                ces.InsertRow();
                gvTable.FirstDisplayedScrollingRowIndex = gvTable.RowCount - 1;
            }
        }

        private void tsbApplyTableChanges_Click(object sender, EventArgs e)
        {
            if (gvTable.DataSource == null) return;
            if (gvTable.IsCurrentCellDirty || gvTable.IsCurrentRowDirty)
            {
                Validate();
            }

            Query q = (Query)gvTable.Tag!;

            List<Tuple<DB, string>>? ts = q.GenerateSql();

            if (ts != null && ts.Count > 0)
            {
                ConfirmSqlForm f = new ConfirmSqlForm(_Data!, ts);
                f.ShowDialog(this);

                switch (f.Result)
                {
                    case ConfirmSqlForm.ResultEnum.Run:
                        if (ts.Count > 1)
                        {
                            StringBuilder sbIntegratedSql = new StringBuilder();
                            foreach (Tuple<DB, string> t in ts)
                            {
                                DB db = t.Item1;
                                string sql = t.Item2;

                                sbIntegratedSql.AppendLine(string.Format(Properties.Text.executed_in_comment, db.Alias));
                                sbIntegratedSql.AppendLine();
                                sbIntegratedSql.AppendLine(sql);
                            }

                            PgTaskIntegrator ti = new PgTaskIntegrator(_Data!, new PgTask.OnUpdate(Task_OnUpdate), sbIntegratedSql.ToString(), false);

                            foreach (Tuple<DB, string> t in ts)
                            {
                                DB db = t.Item1;
                                string sql = t.Item2;

                                PgTaskExecutorSqlTables tes = new PgTaskExecutorSqlTables(
                                    _Data!, db, new PgTask.OnUpdate(Task_OnUpdate), sql,
                                    _Data!.Config.TransactionMode == Config.TransactionModeEnum.AutoCoordinated ? Config.TransactionModeEnum.AutoCoordinated : Config.TransactionModeEnum.AutoSingle,
                                    Config.TransactionLevelEnum.ReadCommited, _Data!.PGSimpleLanguageData, ti);

                                ti.Integrate(tes);

                                Log h = new Log(_Data!);
                                h.SqlText = sql;
                                h.DBIds.Add(db.Id);
                                h.Save();
                            }

                            ti.Start();
                        }
                        else
                        {
                            DB db = ts[0].Item1;
                            string sql = ts[0].Item2;


                            PgTaskExecutorSqlTables tes = new PgTaskExecutorSqlTables(_Data!, db, new PgTask.OnUpdate(Task_OnUpdate), sql, Config.TransactionModeEnum.AutoSingle, Config.TransactionLevelEnum.ReadCommited, _Data!.PGSimpleLanguageData, null);
                            tes.Start();

                            Log h = new Log(_Data!);
                            h.SqlText = sql;
                            h.DBIds.Add(db.Id);
                            h.Save();
                        }
                        _Data!.CheckAppDbFileSize();
                        q.CommitChanges();

                        break;
                    case ConfirmSqlForm.ResultEnum.Cancel:
                        break;
                    case ConfirmSqlForm.ResultEnum.Edit:
                        foreach (Tuple<DB, string> t in ts)
                        {
                            EditorTab.CreateEditorTabOptions o = new EditorTab.CreateEditorTabOptions();
                            o.Title = string.Format(Properties.Text.modifications_in, t.Item1.Alias);
                            o.Text = t.Item2;
                            o.Focus = true;
                            CreateEditorTab(o);
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }

            }
        }

        private bool _GvTable_IgnoreEvents = false;
        private void gvTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_GvTable_IgnoreEvents) return;
            if (gvTable.Tag == null) return;
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;

            Query q = (Query)gvTable.Tag!;
            DataRow drCurrent = ((DataRowView)gvTable.Rows[e.RowIndex].DataBoundItem).Row;
            q.SetEditedCell(drCurrent, e.ColumnIndex);
        }

        private void gvTable_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_GvTable_IgnoreEvents) return;
            if (gvTable.Tag == null) return;
            if (e.RowIndex == -1) return;
            if (gvTable.Rows.Count == 0) return;

            Query q = (Query)gvTable.Tag!;
            DataRow drCurrent = ((DataRowView)gvTable.Rows[e.RowIndex].DataBoundItem).Row;
            q.SetDeletedRow(drCurrent);
        }

        private void gvTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, gvTable.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), this.Font, SystemBrushes.ControlText, headerBounds, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private void tsbDeleteRows_Click(object sender, EventArgs e)
        {
            if (gvTable.SelectedRows.Count == 0)
            {
                MessageBox.Show(Properties.Text.no_selected_rows, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow r in gvTable.SelectedRows)
            {
                Query q = (Query)gvTable.Tag!;
                int rowIndex = r.Index;
                _GvTable_IgnoreEvents = true;
                DataRow drCurrent = ((DataRowView)gvTable.Rows[rowIndex].DataBoundItem).Row;
                drCurrent.Delete();
                q.SetDeletedRow(drCurrent);
                _GvTable_IgnoreEvents = false;
            }
        }

        private string? GetSelectedCellValue(out DataGridViewCell? cell, out Query? q, out Query.QueryColumn? col, out DataRow? drCurrent)
        {
            cell = null;
            col = null;
            q = null;
            drCurrent = null;

            if (gvTable.SelectedCells.Count != 1) return null;
            cell = gvTable.SelectedCells[0];

            if (cell.RowIndex == -1 || cell.ColumnIndex == -1) return null;

            if (gvTable.RowCount <= cell.RowIndex) return null;

            drCurrent = ((DataRowView)gvTable.Rows[cell.RowIndex].DataBoundItem).Row;
            object v = drCurrent[cell.ColumnIndex];
            q = (Query)gvTable.Tag!;
            col = q.Columns[cell.ColumnIndex];

            if (v == null || v == DBNull.Value)
            {
                return null;
            }
            else
            {
                return v.ToString()!;
            }
        }

        private void tsbCopyCellText_Click(object sender, EventArgs e)
        {
            DataGridViewCell? cell;
            Query.QueryColumn? col;
            Query? q;
            DataRow? drCurrent;

            string? txt = GetSelectedCellValue(out cell, out q, out col, out drCurrent);
            if (cell == null || q == null || col == null || drCurrent == null) return;

            Clipboard.SetText(txt == null ? "" : txt);
        }

        private void tsbLoadCellBinaryValueFromFile_Click(object sender, EventArgs e)
        {
            ofdBinaryCell.FileName = "";

            if (ofdBinaryCell.ShowDialog(this) != DialogResult.OK) return;

            byte[] ba = File.ReadAllBytes(ofdBinaryCell.FileName);

            Query q = (Query)gvTable.Tag!;

            foreach (DataGridViewCell cell in gvTable.SelectedCells)
            {
                if (cell.RowIndex == -1 || cell.ColumnIndex == -1) continue;
                if (gvTable.RowCount <= cell.RowIndex) continue;
                Query.QueryColumn col = q.Columns[cell.ColumnIndex];

                if (q.Editable && col.EditableOnEdit)
                {
                    DataRow drCurrent = ((DataRowView)gvTable.Rows[cell.RowIndex].DataBoundItem).Row;
                    switch (col.Type)
                    {
                        case "bytea":
                            {
                                StringBuilder sb = new StringBuilder(ba.Length * 2);
                                foreach (byte b in ba)
                                {
                                    sb.AppendFormat("{0:x2}", b);
                                }

                                drCurrent[cell.ColumnIndex] = @"\x" + sb.ToString();
                                q.SetEditedCell(drCurrent, cell.ColumnIndex);
                            }
                            break;
                        case "bit varying":
                        case "bit":
                        case "varbit":
                            {
                                StringBuilder sb = new StringBuilder(ba.Length * 8);
                                foreach (byte b in ba)
                                {
                                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                                }

                                drCurrent[cell.ColumnIndex] = @"\x" + sb.ToString();
                                q.SetEditedCell(drCurrent, cell.ColumnIndex);
                            }
                            break;
                    }
                }
            }
        }

        private void tsbSaveCellBinaryValueInFile_Click(object sender, EventArgs e)
        {
            Query.QueryColumn? col;
            string? s = GetSelectedCellValue(out _, out _, out col, out _);
            if (s == null || col == null) return;

            byte[] b;

            try
            {
                switch (col.Type)
                {
                    case "bytea":
                        if (s.Length < 4 || !s.StartsWith(@"\x") || s.Length % 2 != 0)
                        {
                            MessageBox.Show(Properties.Text.invalid_hex_value, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        b = Enumerable.Range(0, s.Length - 4)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(s.Substring(x + 2, 2), 16))
                                 .ToArray();
                        break;
                    case "bit varying":
                    case "bit":
                    case "varbit":
                        if (s.Length < 8 || s.Length % 8 != 0)
                        {
                            MessageBox.Show(Properties.Text.invalid_hex_value, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        b = Enumerable.Range(0, s.Length - 8)
                                 .Where(x => x % 8 == 0)
                                 .Select(x => Convert.ToByte(s.Substring(x, 8), 2))
                                 .ToArray();
                        break;
                    default:
                        return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.Text.invalid_hex_value, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sfdBinaryCell.FileName = "";
            if (sfdBinaryCell.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                File.WriteAllBytes(sfdBinaryCell.FileName, b);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{sfdBinaryCell.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbTextEditor_Click(object sender, EventArgs e)
        {
            DataGridViewCell? cell;
            Query.QueryColumn? col;
            Query? q;
            DataRow? drCurrent;

            string? txt = GetSelectedCellValue(out cell, out q, out col, out drCurrent);
            if (cell == null || q == null || col == null || drCurrent == null) return;

            /* Changed TextBox for FastColoredTextBox, which supports larger texts
            if (txt != null && txt.Length > 1024 * 1024)
            {
                MessageBox.Show(Properties.Text.text_too_long, Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            */

            TextBoxForm f = new TextBoxForm(txt, q.Editable && col.EditableOnEdit);
            f.ShowDialog(this);

            if (f.DialogResult == DialogResult.OK)
            {
                if (f.Value == null)
                {
                    drCurrent[cell.ColumnIndex] = DBNull.Value;
                }
                else
                {
                    drCurrent[cell.ColumnIndex] = f.Value;
                }
                q.SetEditedCell(drCurrent, cell.ColumnIndex);
            }
        }

        private void tsbSetNull_Click(object sender, EventArgs e)
        {
            Query q = (Query)gvTable.Tag!;

            foreach (DataGridViewCell cell in gvTable.SelectedCells)
            {
                if (cell.RowIndex == -1 || cell.ColumnIndex == -1) continue;
                if (gvTable.RowCount <= cell.RowIndex) continue;

                Query.QueryColumn col = q.Columns[cell.ColumnIndex];
                if (q.Editable && col.EditableOnEdit && !col.Column!.NotNull)
                {
                    DataRow drCurrent = ((DataRowView)gvTable.Rows[cell.RowIndex].DataBoundItem).Row;
                    drCurrent[cell.ColumnIndex] = DBNull.Value;
                    q.SetEditedCell(drCurrent, cell.ColumnIndex);
                }
            }
        }

        private void gvTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex == -1 || e.ColumnIndex == -1) return;
            DataGridViewCell cell = gvTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Query q = (Query)gvTable.Tag!;

            gvTable.ClearSelection();
            gvTable.CurrentCell = cell;
            cell.Selected = true;

            tsbCopyCellText.Enabled = false;
            tsbLoadCellBinaryValueFromFile.Visible = false;
            tsbSaveCellBinaryValueInFile.Visible = false;
            tsbTextEditor.Enabled = false;
            tsbSetNull.Enabled = false;

            if (cell.ColumnIndex < q.Columns.Count && cell.RowIndex < gvTable.RowCount)
            {
                Query.QueryColumn col = q.Columns[cell.ColumnIndex];

                if (cell.Value != DBNull.Value)
                {
                    tsbCopyCellText.Enabled = true;

                    if (col.Type == "bytea" || col.Type == "varbit" || col.Type == "bit" || col.Type == "bit varying")
                    {
                        tsbSaveCellBinaryValueInFile.Visible = true;
                    }

                    if (q.Editable && col.EditableOnEdit && !col.Column!.NotNull)
                    {
                        tsbSetNull.Enabled = true;
                    }
                }

                if (col.Type == "bytea" || col.Type == "varbit" || col.Type == "bit" || col.Type == "bit varying" && q.Editable && col.EditableOnEdit)
                {
                    tsbLoadCellBinaryValueFromFile.Visible = true;
                }

                tsbTextEditor.Enabled = true;
            }
        }


        private void fctbExecutedSql_SecondaryFormClosed(object? sender, SecondaryFormEventArgs e)
        {
            _SecondaryForms.Remove(e.Form);
        }

        private void fctbExecutedSql_SecondaryFormShowed(object? sender, SecondaryFormEventArgs e)
        {
            _SecondaryForms.Add(e.Form);
        }

        private void fctbResult_SecondaryFormClosed(object? sender, SecondaryFormEventArgs e)
        {
            _SecondaryForms.Remove(e.Form);
        }

        private void fctbResult_SecondaryFormShowed(object? sender, SecondaryFormEventArgs e)
        {
            _SecondaryForms.Add(e.Form);
        }

        #endregion

        #region "Search and replace"
        private void ShowSearchAndReplace()
        {
            tcLeftPanel.SelectedTab = tpSearchAndReplace;
            txtSearchText.Focus();
            txtSearchText.SelectionStart = 0;
            txtSearchText.SelectionLength = txtSearchText.Text.Length;

            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            if (tb.Selection.Length > 0)
            {
                txtSearchText.SelectedText = tb.SelectedText;
            }
        }

        private void HideSearchAndReplace()
        {
            txtSearchText.Text = "";
            tcLeftPanel.SelectedTab = tpConnections;

            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            if (tb.SearchRange != null)
            {
                tb.SearchRange = null;
            }

            UpdateSearchResults();
            UpdateSearchHighlighting();
        }

        private bool UpdateSearchRange()
        {
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            FastColoredTextBoxNS.Range? searchRange;

            if (chkSearchWithinSelectedText.Checked)
            {
                searchRange = tb.Selection.Clone();
                searchRange.Normalize();
            }
            else
            {
                searchRange = null;
            }

            if (tb.SearchRange != searchRange)
            {
                tb.SearchRange = searchRange;

                lblSearchResultsSummary.Text = string.Format(Properties.Text.number_of_search_results_found, tb.SearchMatches == null ? 0 : tb.SearchMatches.Count) + "\r\n"
                    + (searchRange == null ? Properties.Text.searching_the_entire_text : Properties.Text.searching_only_within_selected_text);

                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool UpdateSearchResults()
        {
            string pattern = txtSearchText.Text;
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);

            List<FastColoredTextBoxNS.Range> matches;

            if (pattern == "")
            {
                matches = new List<FastColoredTextBoxNS.Range>();
            }
            else
            {

                matches = tb.FindAll(pattern, chkSearchMatchCase.Checked, chkSearchMatchWholeWords.Checked, chkSearchRegex.Checked);
            }

            if (
                    (matches.Count == 0 && tb.SearchMatches != null && tb.SearchMatches.Count > 0)
                    || (matches.Count > 0 && (tb.SearchMatches == null || !matches.SequenceEqual(tb.SearchMatches)))
                )
            {
                tb.SearchMatches = matches;
                btnGoNextSearchResult.Enabled = matches.Count > 0;
                btnReplaceCurrent.Enabled = matches.Count > 0;
                btnReplaceAll.Enabled = matches.Count > 0;
                lblSearchResultsSummary.Text = string.Format(Properties.Text.number_of_search_results_found, matches.Count) + "\r\n"
                    + (tb.SearchRange == null ? Properties.Text.searching_the_entire_text : Properties.Text.searching_only_within_selected_text);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateSearchHighlighting()
        {
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            tb.DoHighlighting();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateSearchResults();
            UpdateSearchHighlighting();

            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            if (tb.SearchMatches != null && tb.SearchMatches.Count > 0)
            {
                GoNextSearchResult();
            }
        }

        private void GoNextSearchResult()
        {
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            if (tb.SearchMatches == null || tb.SearchMatches.Count == 0) return;

            Place p = tb.Selection.End > tb.Selection.Start ? tb.Selection.End : tb.Selection.Start;
            FastColoredTextBoxNS.Range? r = tb.SearchMatches!.FirstOrDefault(ri => ri.Start > p);
            if (r == null)
            {
                r = tb.SearchMatches!.First();
            }

            tb.Selection = r;
            tb.DoSelectionVisible();
            tb.Focus();
            tb.Invalidate();
            return;
        }

        private void btnGoNextSearchResult_Click(object sender, EventArgs e)
        {
            GoNextSearchResult();
        }

        private void btnUpdateSearchSelectedText_Click(object sender, EventArgs e)
        {
            if (UpdateSearchRange() | UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
        }

        private void btnReplaceCurrent_Click(object sender, EventArgs e)
        {
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);
            if (tb.SearchMatches != null && tb.SearchMatches.Count > 0)
            {
                if (tb.Selection.Length > 0 && tb.SearchMatches.Any(sm => tb.Selection.Equals(sm)))
                {
                    string replaceText = txtReplaceText.Text;
                    if (chkSearchRegex.Checked)
                    {
                        System.Text.RegularExpressions.Regex r = tb.GetRegex(txtSearchText.Text, chkSearchMatchCase.Checked, chkSearchMatchWholeWords.Checked, chkSearchRegex.Checked);

                        replaceText = r.Replace(tb.SelectedText, replaceText);
                    }

                    tb.InsertText(replaceText);

                    if (UpdateSearchResults())
                    {
                        UpdateSearchHighlighting();
                    }

                    if (tb.SearchMatches != null && tb.SearchMatches.Count > 0)
                    {
                        GoNextSearchResult();
                    }
                    else
                    {
                        tb.Focus();
                    }
                }
                else
                {
                    GoNextSearchResult();
                }
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            CustomFctb tb = ((CustomFctb)tcSql.SelectedTab.Controls[0]);

            if (tb.SearchMatches != null && tb.SearchMatches.Count > 0)
            {
                tb.Selection.BeginUpdate();
                tb.BeginAutoUndo();
                try
                {
                    if (chkSearchRegex.Checked)
                    {
                        System.Text.RegularExpressions.Regex r = tb.GetRegex(txtSearchText.Text, chkSearchMatchCase.Checked, chkSearchMatchWholeWords.Checked, chkSearchRegex.Checked);

                        for (int i = tb.SearchMatches.Count - 1; i >= 0; i--)
                        {
                            FastColoredTextBoxNS.Range ri = tb.SearchMatches[i];

                            string replaceText = r.Replace(ri.Text, txtReplaceText.Text);

                            tb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(tb.TextSource, new List<FastColoredTextBoxNS.Range> { ri }, replaceText));
                        }
                    }
                    else
                    {
                        tb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(tb.TextSource, tb.SearchMatches, txtReplaceText.Text));
                    }


                    if (tb.SearchRange == null)
                    {
                        tb.Selection.Start = new Place(0, 0);
                    }
                    else
                    {
                        tb.Selection.Start = tb.SearchRange.Start;
                    }
                }
                finally
                {
                    tb.EndAutoUndo();
                    tb.Selection.EndUpdate();
                }

                tb.DoSelectionVisible();
                tb.Focus();
                tb.Invalidate();

                if (UpdateSearchResults())
                {
                    UpdateSearchHighlighting();
                }
            }
        }

        private void txtSearchText_TextChanged(object sender, EventArgs e)
        {
            if (UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
        }

        private void chkSearchMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
        }

        private void chkSearchMatchWholeWords_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
        }

        private void chkSearchRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
        }

        private void chkSearchWithinSelectedText_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateSearchRange() | UpdateSearchResults())
            {
                UpdateSearchHighlighting();
            }
            btnUpdateSearchSelectedText.Visible = chkSearchWithinSelectedText.Checked;
        }

        private void txtSearchText_Enter(object sender, EventArgs e)
        {
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

        #endregion

        #region TextI18n
        private void RefreshLanguage()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == AppLanguage.CurrentLanguage.Id) return;

            CultureInfo cu = AppLanguage.CurrentLanguage.CultureInfo;
            Application.CurrentCulture = cu;
            CultureInfo.DefaultThreadCurrentCulture = cu;
            CultureInfo.DefaultThreadCurrentUICulture = cu;
            Thread.CurrentThread.CurrentCulture = cu;
            Thread.CurrentThread.CurrentUICulture = cu;
            _Data!.PGLanguageData.Grammar.DefaultCulture = cu;
            _Data!.PGSimpleLanguageData.Grammar.DefaultCulture = cu;
            Irony.Resources.Culture = cu;

            InitializeText();
            _NRoot.Text = Properties.Text.all_databases;
            tvaConnections.Refresh();

        }

        private void InitializeText()
        {
            this.tscmiNewGroup.Text = Properties.Text.new_group;
            this.tscmiNewDB.Text = Properties.Text.add_db;
            this.tscmiExploreTable.Text = Properties.Text.explore_table;
            this.tscmiRecursiveRemove.Text = Properties.Text.recursive_remove;
            this.tscmiCreateTableDiagram.Text = Properties.Text.create_table_diagram;
            this.tscmiCopyText.Text = Properties.Text.copy_text;
            this.tscmiEdit.Text = Properties.Text.edit;
            this.tscmiRemove.Text = Properties.Text.remove;
            this.tscmiUp.Text = Properties.Text.up;
            this.tscmiDown.Text = Properties.Text.down;
            this.tscmiRefresh.Text = Properties.Text.refresh;
            this.tsbNewGroup.Text = Properties.Text.new_group;
            this.tsbNewDB.Text = Properties.Text.add_db;
            this.tsbExploreTable.Text = Properties.Text.explore_table;
            this.tsbRecursiveRemove.Text = Properties.Text.recursive_remove;
            this.tsbCreateTableDiagram.Text = Properties.Text.create_table_diagram;
            this.tsbEdit.Text = Properties.Text.edit;
            this.tsbRemove.Text = Properties.Text.remove;
            this.tsbUp.Text = Properties.Text.up;
            this.tsbDown.Text = Properties.Text.down;
            this.tsbCollapseAll.Text = Properties.Text.collapse_all;
            this.tsbRefresh.Text = Properties.Text.refresh;
            this.tsmiFile.Text = Properties.Text.file;
            this.tsmiNew.Text = Properties.Text._new;
            this.tsmiOpen.Text = Properties.Text.open;
            this.tsbOpen.Text = Properties.Text.open;
            this.tsmiSave.Text = Properties.Text.save;
            this.tsbSave.Text = Properties.Text.save;
            this.tsmiSaveAs.Text = Properties.Text.save_as;
            this.tsmiSaveAll.Text = Properties.Text.save_all;
            this.tsbSaveAll.Text = Properties.Text.save_all;
            this.tsmiClose.Text = Properties.Text.close;
            this.tsmiCloseAll.Text = Properties.Text.close_all;
            this.tsmiEdit.Text = Properties.Text.edit;
            this.tsmiBack.Text = Properties.Text.back;
            this.tsmiForward.Text = Properties.Text.forward;
            this.tsmiUndo.Text = Properties.Text.undo_sc;
            this.tsmiRedo.Text = Properties.Text.redo_sc;
            this.tsmiCut.Text = Properties.Text.cut_sc;
            this.tsmiCopy.Text = Properties.Text.copy_sc;
            this.tsmiPaste.Text = Properties.Text.paste_sc;
            this.tsmiSearchAndReplace.Text = Properties.Text.search_for_and_replace;
            this.tsmiGoTo.Text = Properties.Text.goto_sc;
            this.tsmiFormat.Text = Properties.Text.format_sc;
            this.tsbSearchAndReplace.Text = Properties.Text.search_for_and_replace;
            this.tsbGoTo.Text = Properties.Text.goto_sc;
            this.tsbFormat.Text = Properties.Text.format_sc;
            this.tsmiOptions.Text = Properties.Text.options;
            this.tsmiIncreaseFont.Text = Properties.Text.increase_font;
            this.tsmiReduceFont.Text = Properties.Text.reduce_font;
            this.tsmiChangePassword.Text = Properties.Text.change_pass;
            this.tsmiImportConnections.Text = Properties.Text.import_databases;
            this.tsmiExportConnections.Text = Properties.Text.export_databases;
            this.tsmiMoreOptions.Text = Properties.Text.more_options;
            this.tsmiRunMenu.Text = Properties.Text.run;
            this.tsbExportCsv.Text = Properties.Text.export_csv;
            this.tsmiExportCsv.Text = Properties.Text.export_csv;
            this.tsddbErrors.Text = Properties.Text.no_errors;
            this.tsbHistory.Text = Properties.Text.history;
            this.tsmiHistory.Text = Properties.Text.history;
            this.tsmiAbout.Text = Properties.Text.about;
            this.tsbRemoveSelected.Text = Properties.Text.remove_selected;
            this.tsbRemoveAll.Text = Properties.Text.remove_completed;
            this.tsbStopSelected.Text = Properties.Text.stop_selected;
            this.tsbStopAll.Text = Properties.Text.stop_all;
            this.tsbCurrentTabLastTask.Text = Properties.Text.current_tab_last_task + " (ctrl + L)";
            this.tpResult.Text = Properties.Text.result;
            this.tsddbAutoScroll.Text = Properties.Text.auto_scroll;
            this.tsmiAutoScroll.Text = Properties.Text.auto_scroll;
            this.tsmiManualScroll.Text = Properties.Text.manual_scroll;
            this.tpTable.Text = Properties.Text.table;
            this.tsbTextEditor.Text = Properties.Text.show_text_window;
            this.tsbCopyCellText.Text = Properties.Text.copy_text;
            this.tsbLoadCellBinaryValueFromFile.Text = Properties.Text.load_binary_file;
            this.tsbSaveCellBinaryValueInFile.Text = Properties.Text.save_binary_file;
            this.tsbSetNull.Text = Properties.Text.set_null;
            this.tsddbTables.Text = Properties.Text.no_results;
            this.tsddbInsertRow.Text = Properties.Text.insert_row;
            this.tsbDeleteRows.Text = Properties.Text.delete_rows;
            this.tsbApplyTableChanges.Text = Properties.Text.apply_changes;
            this.tpExecutedSql.Text = Properties.Text.executed_query;
            this.tsbEditExecutedSql.Text = Properties.Text.edit;
            this.tsmiCloseTab.Text = Properties.Text.close_this_tab;
            this.tsmiCloseAllTabs.Text = Properties.Text.close_all_tabs;
            this.tsmiCloseAllTabsExceptThisOne.Text = Properties.Text.close_all_tabs_except_this_one;
            this.tsmiClosedTabsLog.Text = Properties.Text.closed_tabs_log;
            this.tsmiCopyPath.Text = Properties.Text.copy_path;
            this.tsmiOpenFolder.Text = Properties.Text.open_folder;
            this.tsmiReopenLastClosedTab.Text = Properties.Text.reopen_last_closed_tab;
            this.tscmiBack.Text = Properties.Text.back;
            this.tscmiForward.Text = Properties.Text.forward;
            this.tscmiUndo.Text = Properties.Text.undo_sc;
            this.tscmiRedo.Text = Properties.Text.redo_sc;
            this.tscmiCut.Text = Properties.Text.cut_sc;
            this.tscmiCopy.Text = Properties.Text.copy_sc;
            this.tscmiPaste.Text = Properties.Text.paste_sc;
            this.tscmiSearchAndReplace.Text = Properties.Text.search_for_and_replace;
            this.tscmiGoTo.Text = Properties.Text.goto_sc;
            this.tscmiFormat.Text = Properties.Text.format_sc;
            this.ofdSql.Filter = Properties.Text.sql_file_filter;
            this.ofdSql.Title = Properties.Text.select_open_file;
            this.sfdSql.Filter = Properties.Text.sql_file_filter;
            this.sfdSql.Title = Properties.Text.select_save_file;
            this.sfdCsv.Filter = Properties.Text.csv_file_filter;
            this.sfdCsv.Title = Properties.Text.select_save_file;
            this.tpNewTab.ToolTipText = Properties.Text._new;
            this.tsddbTransactions.Text = Properties.Text.transactions;
            this.tsmiTransactionModeManual.Text = Properties.Text.manual_transactions;
            this.tsmiTransactionModeAutoSingle.Text = Properties.Text.auto_single_transactions;
            this.tsmiTransactionModeAutoCoordinated.Text = Properties.Text.auto_coordinated_transactions;
            this.ofdImportConfig.Filter = Properties.Text.pgcx_file_filter;
            this.ofdImportConfig.Title = Properties.Text.select_open_file;
            this.ofdOpenDiagram.Filter = Properties.Text.pgdx_file_filter;
            this.ofdOpenDiagram.Title = Properties.Text.select_open_file;
            this.sfdSaveDiagram.Filter = Properties.Text.pgdx_file_filter;
            this.sfdSaveDiagram.Title = Properties.Text.select_save_file;
            this.tsbNewDiagram.Text = Properties.Text.new_diagram;
            this.tsbOpenDiagram.Text = Properties.Text.open_diagram;
            this.tsmiDiagrams.Text = Properties.Text.diagrams;
            this.tsmiNewDiagram.Text = Properties.Text.new_diagram;
            this.tsmiOpenDiagram.Text = Properties.Text.open_diagram;
            this.tpConnections.Text = Properties.Text.db_list;
            this.tpSearchAndReplace.Text = Properties.Text.search_for_and_replace;
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
            this.ofdBinaryCell.Filter = Properties.Text.all_file_filter;
            this.ofdBinaryCell.Title = Properties.Text.select_open_file;
            this.sfdBinaryCell.Filter = Properties.Text.all_file_filter;
            this.sfdBinaryCell.Title = Properties.Text.select_save_file;
            this.tsmiCopyToTable.Text = Properties.Text.run_and_copy_to_table;
        }
        #endregion
    }
}
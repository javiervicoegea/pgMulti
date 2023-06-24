using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using PgMulti.AppData;
using PgMulti.DataStructure;
using System.Windows.Forms;

namespace PgMulti.Forms
{
    public partial class SelectTablesForm : Form
    {
        private Data? _Data;
        private TreeModel _TreeModelConnections;
        private TreeModel _TreeModelTables;
        private Font groupFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private Font itemFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        private List<Tuple<string, string>> _PreselectedTablesIds;
        private List<Table>? _SelectedTables = null;

        public SelectTablesForm(Data d, List<Tuple<string, string>> preselectedTablesIds)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _PreselectedTablesIds = preselectedTablesIds;

            _TreeModelConnections = new TreeModel();
            _TreeModelTables = new TreeModel();

            tvaConnections.Model = _TreeModelConnections;
            tvaTables.Model = _TreeModelTables;

            ntbConnections.DrawText += ntbConnections_DrawText;

            ntbTables.DrawText += ntbTables_DrawText;
            ncbTables.CheckStateChanged += ncbTables_CheckStateChanged;
            ncbTables.IsVisibleValueNeeded += ncbTables_IsVisibleValueNeeded;

            DialogResult = DialogResult.Cancel;
        }

        public List<Table>? SelectedTables { get { return _SelectedTables; } }

        private void SelectTablesForm_Load(object sender, EventArgs e)
        {
            tvaConnections.BeginUpdate();
            Queue<Tuple<Node, Group>> queue = new Queue<Tuple<Node, Group>>();
            queue.Enqueue(new Tuple<Node, Group>(_TreeModelConnections.Root, _Data!.RootGroup));

            while (queue.Count > 0)
            {
                Tuple<Node, Group> tuple = queue.Dequeue();
                Node n = tuple.Item1;
                Group g = tuple.Item2;

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
                        Node dbNode = new Node(db.Alias);
                        dbNode.Tag = db;
                        dbNode.Image = Properties.Resources.tva_db;

                        n.Nodes.Add(dbNode);
                    }
                    else if (childItem.Item2 is Group)
                    {
                        Group childGroup = (Group)childItem.Item2;
                        Node groupNode = new Node(childGroup.Name);

                        groupNode.Tag = childGroup;
                        groupNode.Image = Properties.Resources.tva_grupo;

                        n.Nodes.Add(groupNode);
                        queue.Enqueue(new Tuple<Node, Group>(groupNode, childGroup));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            _TreeModelConnections.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            tvaConnections.EndUpdate();
        }

        private void tvaConnections_SelectionChanged(object sender, EventArgs e)
        {
            tvaTables.BeginUpdate();

            Node rootNode = _TreeModelTables.Root;

            rootNode.Nodes.Clear();

            if (tvaConnections.SelectedNode != null && ((Node)tvaConnections.SelectedNode.Tag).Tag is DB)
            {
                DB db = (DB)((Node)tvaConnections.SelectedNode.Tag).Tag;

                foreach (Schema schema in db.Schemas)
                {
                    Node schemaNode = new Node(schema.Id);
                    schemaNode.Tag = schema;
                    schemaNode.Image = Properties.Resources.tva_schema;
                    rootNode.Nodes.Add(schemaNode);

                    bool allPreselected = true;
                    foreach (Table table in schema.Tables.OrderBy(ti => ti.Id))
                    {
                        Node tableNode = new Node(table.Id);
                        tableNode.Tag = table;
                        tableNode.Image = Properties.Resources.tva_table;
                        schemaNode.Nodes.Add(tableNode);

                        bool preselected = false;
                        foreach (Tuple<string, string> tableId in _PreselectedTablesIds)
                        {
                            if (table.IdSchema == tableId.Item1 && table.Id == tableId.Item2)
                            {
                                preselected = true;
                                break;
                            }
                        }

                        tableNode.IsChecked = preselected;
                        if (!preselected) allPreselected = false;
                    }

                    schemaNode.IsChecked = (allPreselected && schema.Tables.Count > 0);
                }
            }

            _TreeModelTables.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));
            tvaTables.EndUpdate();
        }

        private void ncbTables_IsVisibleValueNeeded(object? sender, NodeControlValueEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n.Tag is Schema || n.Tag is Table)
            {
                e.Value = true;
            }
            else
            {
                e.Value = false;
            }
        }

        private void ntbConnections_DrawText(object? sender, DrawEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n.Tag is Group)
            {
                e.Font = groupFont;
            }
            else
            {
                e.Font = itemFont;
            }
        }

        private void ntbTables_DrawText(object? sender, DrawEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n.Tag is Schema)
            {
                e.Font = groupFont;
            }
            else
            {
                e.Font = itemFont;
            }
        }

        private void UpdateNodeCheck(Node nEditedNode)
        {
            if (nEditedNode == null) throw new ArgumentException();

            if (nEditedNode.Tag is Schema)
            {
                tvaTables.AllNodes.First(tnai => tnai.Tag == nEditedNode).Expand();
                _TreeModelTables.OnStructureChanged(new TreePathEventArgs(_TreeModelTables.GetPath(nEditedNode)));
            }

            bool v = nEditedNode.IsChecked;

            Stack<Node> stack = new Stack<Node>();

            foreach (Node ni in nEditedNode.Nodes) stack.Push(ni);

            while (stack.Count > 0)
            {
                Node ni = stack.Pop();

                if (!(ni.Tag is Schema) && !(ni.Tag is Table)) continue;
                ni.IsChecked = v;


                foreach (Node tnj in ni.Nodes) stack.Push(tnj);
            }

            if (nEditedNode.Tag is Table)
            {
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
            }
        }

        private void ncbTables_CheckStateChanged(object? sender, TreePathEventArgs e)
        {
            Node n = _TreeModelTables.FindNode(e.Path)!;
            UpdateNodeCheck(n);
        }

        private void tsbOk_Click(object sender, EventArgs e)
        {
            _SelectedTables = new List<Table>();

            foreach (Node schemaNode in _TreeModelTables.Nodes)
            {
                foreach (Node tableNode in schemaNode.Nodes)
                {
                    if (tableNode.IsChecked)
                    {
                        _SelectedTables.Add((Table)tableNode.Tag);
                    }
                }
            }

            if (_SelectedTables.Count == 0)
            {
                MessageBox.Show(this, Properties.Text.warning_no_table_selected, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.Text = Properties.Text.select_tables;
            this.tsbOk.Text = Properties.Text.btn_ok;
            this.tsbCancel.Text = Properties.Text.btn_cancel;
        }
        #endregion
    }
}

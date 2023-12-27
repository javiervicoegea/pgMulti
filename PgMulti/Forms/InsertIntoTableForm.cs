using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Npgsql.Schema;
using PgMulti.AppData;
using PgMulti.DataStructure;
using PgMulti.Tasks;
using System.Data;
using static PgMulti.Forms.InsertIntoTableForm;

namespace PgMulti.Forms
{
    public partial class InsertIntoTableForm : Form
    {
        private Data? _Data;
        private PgTaskExecutorSqlCopyToTable _Task;
        private InsertIntoTableFormTreeModel _TreeModel;
        private Font groupFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private Font itemFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private List<ComboBox>? _ComboBoxes = null;
        private Table? _SelectedTable = null;

        public InsertIntoTableForm(Data d, PgTaskExecutorSqlCopyToTable t)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _Task = t;

            _TreeModel = new InsertIntoTableFormTreeModel();

            tvaTables.Model = _TreeModel;
            ntbTables.DrawText += ntbTables_DrawText;
            DialogResult = DialogResult.Cancel;
        }

        private void InsertIntoTableForm_Load(object sender, EventArgs e)
        {
            tvaTables.BeginUpdate();
            Queue<Tuple<Node, Group>> queue = new Queue<Tuple<Node, Group>>();
            queue.Enqueue(new Tuple<Node, Group>(_TreeModel.Root, _Data!.RootGroup));

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

            _TreeModel.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            tvaTables.EndUpdate();

            while (_Task.SourceColumns == null || _Task.Canceled)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            if (_Task.Canceled)
            {
                Close();
                return;
            }




        }

        private void InsertIntoTableForm_FormClosed(object sender, EventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                _Task.Cancel();
            }
        }

        private void tvaTables_SelectionChanged(object sender, EventArgs e)
        {
            TreeNodeAdv? tna = tvaTables.SelectedNode;

            foreach (Control c in splitContainer1.Panel2.Controls)
            {
                splitContainer1.Panel2.Controls.Remove(c);
            }


            if (tna != null && tna.Tag != null && tna.Tag is Aga.Controls.Tree.Node && ((Aga.Controls.Tree.Node)tna.Tag).Tag != null && ((Aga.Controls.Tree.Node)tna.Tag).Tag is Table)
            {
                _SelectedTable = (Table)((Aga.Controls.Tree.Node)tna.Tag).Tag;
                _ComboBoxes = new List<ComboBox>();

                TableLayoutPanel tlpColumnMapping = new TableLayoutPanel();

                tlpColumnMapping.SuspendLayout();


                tlpColumnMapping.ColumnCount = 2;
                tlpColumnMapping.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tlpColumnMapping.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tlpColumnMapping.Dock = DockStyle.Fill;
                tlpColumnMapping.Padding = new Padding(10);
                tlpColumnMapping.RowCount = _SelectedTable.Columns.Count + 1;
                tlpColumnMapping.Size = new Size(1006, 612);
                tlpColumnMapping.TabIndex = 0;
                tlpColumnMapping.AutoScroll = true;

                int rowIndex = 0;
                foreach (Column c in _SelectedTable.Columns)
                {
                    tlpColumnMapping.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    Label lbl = new Label(); ;
                    ComboBox cb = new ComboBox();

                    lbl.Dock = DockStyle.Fill;
                    lbl.TextAlign = ContentAlignment.TopRight;
                    lbl.Text = $"{c.Id} ({c.Type}):";
                    lbl.Padding = new Padding(7);

                    cb.Tag = c;
                    cb.Items.Add(new SourceColumnListItem(null));
                    cb.SelectedIndex = 0;
                    cb.Width = 600;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;

                    foreach (NpgsqlDbColumn sc in _Task.SourceColumns!)
                    {
                        SourceColumnListItem scli = new SourceColumnListItem(sc);
                        cb.Items.Add(scli);

                        if (sc.ColumnName == c.Id)
                        {
                            cb.SelectedItem = scli;
                        }
                    }

                    _ComboBoxes.Add(cb);

                    tlpColumnMapping.Controls.Add(lbl, 0, rowIndex);
                    tlpColumnMapping.Controls.Add(cb, 1, rowIndex);

                    rowIndex++;
                }

                tlpColumnMapping.RowStyles.Add(new RowStyle());

                tlpColumnMapping.ResumeLayout(false);
                tlpColumnMapping.PerformLayout();

                splitContainer1.Panel2.Controls.Add(tlpColumnMapping);
            }
            else
            {
                _SelectedTable = null;
                _ComboBoxes = null;
            }
        }

        private void ntbTables_DrawText(object? sender, DrawEventArgs e)
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_SelectedTable == null || _ComboBoxes == null || _ComboBoxes.Any(cb => cb.SelectedItem == null))
            {
                MessageBox.Show(this, Properties.Text.warning_no_table_selected, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Dictionary<int, int> cm = new Dictionary<int, int>();

            for (int i = 0; i < _ComboBoxes.Count; i++)
            {
                ComboBox cb = _ComboBoxes[i];
                Column dc = (Column)cb.Tag!;
                NpgsqlDbColumn? sc = ((SourceColumnListItem)cb.SelectedItem).SourceColumn;

                if (sc != null)
                {
                    cm[i] = _Task.SourceColumns!.IndexOf(sc);
                }
            }

            _Task.SetDestinationTableAndMapping(_SelectedTable, cm);

            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region TextI18n
        private void InitializeText()
        {
            this.Text = Properties.Text.select_destination_table;
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
        }
        #endregion

        internal class SourceColumnListItem
        {
            private NpgsqlDbColumn? _SourceColumn;

            internal SourceColumnListItem(NpgsqlDbColumn? sc)
            {
                _SourceColumn = sc;
            }

            internal NpgsqlDbColumn? SourceColumn
            {
                get
                {
                    return _SourceColumn;
                }
            }

            public override string ToString()
            {
                if (_SourceColumn == null)
                {
                    return $"[{Properties.Text.leave_unassigned}]";
                }
                else
                {
                    return $"{_SourceColumn.ColumnName} ({_SourceColumn.DataTypeName})";
                }
            }
        }
    }
}

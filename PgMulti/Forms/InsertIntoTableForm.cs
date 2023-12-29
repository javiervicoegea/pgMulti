using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Npgsql.Schema;
using PgMulti.AppData;
using PgMulti.DataStructure;
using PgMulti.Tasks;
using System.Data;
using static PgMulti.AppData.InsertIntoTableFormTreeModel;

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
        private Schema? _NewTableSchema = null;
        private TextBox? txtTableName = null;

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

            _NewTableSchema = null;
            _SelectedTable = null;
            _ComboBoxes = null;
            txtTableName = null;

            List<NpgsqlDbColumn> scs = _Task.SourceColumns!.OrderByDescending(sc => sc.IsKey).ThenBy(sc => sc.ColumnName).ToList();

            if (tna != null && tna.Tag != null && tna.Tag is Node)
            {
                if (((Node)tna.Tag).Tag is NewTable)
                {
                    _NewTableSchema = ((NewTable)((Node)tna.Tag).Tag).Schema;

                    TableLayoutPanel tlpNewTable = new TableLayoutPanel();

                    tlpNewTable.SuspendLayout();


                    tlpNewTable.ColumnCount = 3;
                    tlpNewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpNewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpNewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpNewTable.Dock = DockStyle.Fill;
                    tlpNewTable.Padding = new Padding(10);
                    tlpNewTable.RowCount = scs.Count + 3;
                    tlpNewTable.TabIndex = 0;
                    tlpNewTable.AutoScroll = true;

                    int rowIndex = 0;

                    tlpNewTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));

                    Label lblTableName = new Label(); ;
                    txtTableName = new TextBox();

                    lblTableName.Dock = DockStyle.Fill;
                    lblTableName.TextAlign = ContentAlignment.TopRight;
                    lblTableName.Text = Properties.Text.table_name + ":";
                    lblTableName.Padding = new Padding(7);

                    txtTableName.Width = 600;

                    tlpNewTable.Controls.Add(lblTableName, 0, rowIndex);
                    tlpNewTable.Controls.Add(txtTableName, 1, rowIndex);

                    rowIndex++;

                    tlpNewTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                    Label lblColumns = new Label(); ;

                    lblColumns.Dock = DockStyle.Fill;
                    lblColumns.TextAlign = ContentAlignment.TopRight;
                    lblColumns.Text = Properties.Text.columns + ":";
                    lblColumns.Padding = new Padding(7);

                    tlpNewTable.Controls.Add(lblColumns, 0, rowIndex);

                    Label lblColumnHeader;

                    lblColumnHeader = new Label(); ;
                    lblColumnHeader.Dock = DockStyle.Fill;
                    lblColumnHeader.TextAlign = ContentAlignment.TopLeft;
                    lblColumnHeader.Text = Properties.Text.column_name;
                    lblColumnHeader.Padding = new Padding(7);
                    lblColumnHeader.Font = new Font(lblColumnHeader.Font, FontStyle.Bold);
                    tlpNewTable.Controls.Add(lblColumnHeader, 1, rowIndex);

                    lblColumnHeader = new Label(); ;
                    lblColumnHeader.Dock = DockStyle.Fill;
                    lblColumnHeader.TextAlign = ContentAlignment.TopLeft;
                    lblColumnHeader.Text = Properties.Text.type_name;
                    lblColumnHeader.Padding = new Padding(7);
                    lblColumnHeader.Font = new Font(lblColumnHeader.Font, FontStyle.Bold);
                    tlpNewTable.Controls.Add(lblColumnHeader, 2, rowIndex);

                    rowIndex++;

                    foreach (NpgsqlDbColumn c in scs)
                    {
                        tlpNewTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                        Label lblColumnName = new Label();
                        lblColumnName.Dock = DockStyle.Fill;
                        lblColumnName.TextAlign = ContentAlignment.TopLeft;
                        lblColumnName.Text = c.ColumnName;
                        lblColumnName.Padding = new Padding(7);
                        tlpNewTable.Controls.Add(lblColumnName, 1, rowIndex);

                        Label lblColumnType = new Label();
                        lblColumnType.Dock = DockStyle.Fill;
                        lblColumnType.TextAlign = ContentAlignment.TopLeft;
                        lblColumnType.Text = c.PostgresType.Name + (c.NumericPrecision.HasValue ? $" ({c.NumericPrecision.Value},{(c.NumericScale.HasValue ? c.NumericScale.Value : 0)})" : (c.ColumnSize.HasValue ? $" ({c.ColumnSize.Value})" : ""));
                        lblColumnType.Padding = new Padding(7);
                        tlpNewTable.Controls.Add(lblColumnType, 2, rowIndex);

                        rowIndex++;
                    }

                    tlpNewTable.RowStyles.Add(new RowStyle());

                    tlpNewTable.ResumeLayout(false);
                    tlpNewTable.PerformLayout();

                    splitContainer1.Panel2.Controls.Add(tlpNewTable);
                }
                else if (((Node)tna.Tag).Tag is Table)
                {
                    _SelectedTable = (Table)((Node)tna.Tag).Tag;
                    _ComboBoxes = new List<ComboBox>();

                    TableLayoutPanel tlpColumnMapping = new TableLayoutPanel();

                    tlpColumnMapping.SuspendLayout();


                    tlpColumnMapping.ColumnCount = 3;
                    tlpColumnMapping.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpColumnMapping.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpColumnMapping.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                    tlpColumnMapping.Dock = DockStyle.Fill;
                    tlpColumnMapping.Padding = new Padding(10);
                    tlpColumnMapping.RowCount = _SelectedTable.Columns.Count + 2;
                    tlpColumnMapping.TabIndex = 0;
                    tlpColumnMapping.AutoScroll = true;

                    int rowIndex = 0;

                    tlpColumnMapping.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                    Label lblColumnHeader;

                    lblColumnHeader = new Label(); ;
                    lblColumnHeader.Dock = DockStyle.Fill;
                    lblColumnHeader.TextAlign = ContentAlignment.TopLeft;
                    lblColumnHeader.Text = Properties.Text.dest_column;
                    lblColumnHeader.Padding = new Padding(7);
                    lblColumnHeader.Font = new Font(lblColumnHeader.Font, FontStyle.Bold);
                    tlpColumnMapping.Controls.Add(lblColumnHeader, 0, rowIndex);

                    lblColumnHeader = new Label(); ;
                    lblColumnHeader.Dock = DockStyle.Fill;
                    lblColumnHeader.TextAlign = ContentAlignment.TopLeft;
                    lblColumnHeader.Text = Properties.Text.dest_type;
                    lblColumnHeader.Padding = new Padding(7);
                    lblColumnHeader.Font = new Font(lblColumnHeader.Font, FontStyle.Bold);
                    tlpColumnMapping.Controls.Add(lblColumnHeader, 1, rowIndex);

                    lblColumnHeader = new Label(); ;
                    lblColumnHeader.Dock = DockStyle.Fill;
                    lblColumnHeader.TextAlign = ContentAlignment.TopLeft;
                    lblColumnHeader.Text = Properties.Text.source_column;
                    lblColumnHeader.Padding = new Padding(7);
                    lblColumnHeader.Font = new Font(lblColumnHeader.Font, FontStyle.Bold);
                    tlpColumnMapping.Controls.Add(lblColumnHeader, 2, rowIndex);

                    rowIndex++;

                    foreach (Column c in _SelectedTable.Columns.OrderByDescending(ci => ci.PK).ThenBy(sc => sc.Id))
                    {
                        tlpColumnMapping.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

                        Label lblColumnName = new Label(); ;
                        lblColumnName.Dock = DockStyle.Fill;
                        lblColumnName.TextAlign = ContentAlignment.TopLeft;
                        lblColumnName.Text = c.Id;
                        lblColumnName.Padding = new Padding(7);
                        tlpColumnMapping.Controls.Add(lblColumnName, 0, rowIndex);

                        Label lblColumnType = new Label(); ;
                        lblColumnType.Dock = DockStyle.Fill;
                        lblColumnType.TextAlign = ContentAlignment.TopLeft;
                        lblColumnType.Text = c.Type + (string.IsNullOrWhiteSpace(c.TypeParams) ? "" : " " + c.TypeParams);
                        lblColumnType.Padding = new Padding(7);
                        tlpColumnMapping.Controls.Add(lblColumnType, 1, rowIndex);

                        ComboBox cb = new ComboBox();
                        cb.Tag = c;
                        cb.Items.Add(new SourceColumnListItem(null));
                        cb.SelectedIndex = 0;
                        cb.Width = 600;
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;

                        foreach (NpgsqlDbColumn sc in scs)
                        {
                            SourceColumnListItem scli = new SourceColumnListItem(sc);
                            cb.Items.Add(scli);

                            if (sc.ColumnName == c.Id)
                            {
                                cb.SelectedItem = scli;
                            }
                        }

                        _ComboBoxes.Add(cb);

                        tlpColumnMapping.Controls.Add(cb, 2, rowIndex);

                        rowIndex++;
                    }

                    tlpColumnMapping.RowStyles.Add(new RowStyle());

                    tlpColumnMapping.ResumeLayout(false);
                    tlpColumnMapping.PerformLayout();

                    splitContainer1.Panel2.Controls.Add(tlpColumnMapping);
                }
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
            if (_NewTableSchema == null && (_SelectedTable == null || _ComboBoxes == null || _ComboBoxes.Any(cb => cb.SelectedItem == null)))
            {
                MessageBox.Show(this, Properties.Text.warning_no_table_selected, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_NewTableSchema == null)
            {
                Dictionary<int, int> cm = new Dictionary<int, int>();

                for (int i = 0; i < _ComboBoxes!.Count; i++)
                {
                    ComboBox cb = _ComboBoxes[i];
                    Column dc = (Column)cb.Tag!;
                    NpgsqlDbColumn? sc = ((SourceColumnListItem)cb.SelectedItem).SourceColumn;

                    if (sc != null)
                    {
                        cm[dc.Position] = sc.ColumnOrdinal!.Value;
                    }
                }

                _Task.SetDestinationTableAndMapping(_SelectedTable!, cm); 
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtTableName!.Text))
                {
                    MessageBox.Show(this, Properties.Text.warning_empty_table_name, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _Task.SetNewTable(_NewTableSchema, txtTableName!.Text);
            }

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
                    return $"{_SourceColumn.ColumnName} - {_SourceColumn.PostgresType.Name}" + (_SourceColumn.NumericPrecision.HasValue ? $" ({_SourceColumn.NumericPrecision.Value},{(_SourceColumn.NumericScale.HasValue ? _SourceColumn.NumericScale.Value : 0)})" : (_SourceColumn.ColumnSize.HasValue ? $" ({_SourceColumn.ColumnSize.Value})" : ""));
                }
            }
        }
    }
}

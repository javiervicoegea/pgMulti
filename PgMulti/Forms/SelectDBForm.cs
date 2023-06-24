using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using PgMulti.AppData;
using PgMulti.DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PgMulti.Forms
{
    public partial class SelectDBForm : Form
    {
        private Data? _Data;
        private TreeModel _TreeModelConnections;
        private Font groupFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private Font itemFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        private DB? _PreselectedDB;
        private DB? _SelectedDB = null;

        public SelectDBForm(Data d, DB? preselectedDB)
        {
            InitializeComponent();
            InitializeText();

            _Data = d;
            _PreselectedDB = preselectedDB;

            _TreeModelConnections = new TreeModel();

            tvaConnections.Model = _TreeModelConnections;

            ntbConnections.DrawText += ntbConnections_DrawText;

            DialogResult = DialogResult.Cancel;
        }

        public DB? SelectedDB { get { return _SelectedDB; } }

        private void SelectDBForm_Load(object sender, EventArgs e)
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

        private void tsbOk_Click(object sender, EventArgs e)
        {
            if (tvaConnections.SelectedNode == null)
            {
                MessageBox.Show(this, Properties.Text.warning_no_db_selected, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _SelectedDB = (DB)((Node)tvaConnections.SelectedNode.Tag).Tag;

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

using System.Collections.ObjectModel;
using Aga.Controls.Tree;
using PgMulti.DataStructure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PgMulti.AppData
{
    public class InsertIntoTableFormTreeModel : ITreeModel
    {
        private Node _root;
        public Node Root
        {
            get { return _root; }
        }

        public Collection<Node> Nodes
        {
            get { return _root.Nodes; }
        }

        public InsertIntoTableFormTreeModel()
        {
            _root = new Node();
        }

        public TreePath GetPath(Node node)
        {
            if (node == _root)
                return TreePath.Empty;
            else
            {
                Stack<object> stack = new Stack<object>();
                while (node != _root)
                {
                    stack.Push(node);
                    node = node.Parent;
                }
                return new TreePath(stack.ToArray());
            }
        }

        public Node? FindNode(TreePath path)
        {
            if (path.IsEmpty())
                return _root;
            else
                return FindNode(_root, path, 0);
        }

        private Node? FindNode(Node root, TreePath path, int level)
        {
            foreach (Node node in root.Nodes)
                if (node == path.FullPath[level])
                {
                    if (level == path.FullPath.Length - 1)
                        return node;
                    else
                        return FindNode(node, path, level + 1);
                }
            return null;
        }

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            Node? node = FindNode(treePath);
            if (node != null)
            {
                if (node.Tag is DB)
                {
                    DB db = (DB)node.Tag;
                    foreach (Schema e in db.Schemas.OrderBy(ei => ei.Id != "public").ThenBy(ei => ei.Id))
                    {
                        Node n = new Node(e.Id);
                        n.Image = Properties.Resources.tva_schema;
                        node.Nodes.Add(n);
                        n.Tag = e;
                        yield return n;
                    }
                }
                else if (node.Tag is Schema)
                {
                    Schema schema = (Schema)node.Tag;

                    Node nNewTable = new Node($"[{Properties.Text.new_table}]");
                    nNewTable.Image = Properties.Resources.tva_table;
                    node.Nodes.Add(nNewTable);
                    nNewTable.Tag = new NewTable(schema);
                    yield return nNewTable;

                    foreach (Table t in schema.Tables.OrderBy(ti => ti.Id))
                    {
                        Node n = new Node(t.Id);
                        n.Image = Properties.Resources.tva_table;
                        node.Nodes.Add(n);
                        n.Tag = t;
                        yield return n;
                    }
                }
                else
                {
                    foreach (Node n in node.Nodes)
                    {
                        yield return n;
                    }
                }
            }
            else
            {
                yield break;
            }
        }

        public bool IsLeaf(TreePath treePath)
        {
            Node? node = FindNode(treePath);
            if (node != null)
            {
                if (node.Tag is Table)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("treePath");
            }
        }

        public event EventHandler<TreeModelEventArgs>? NodesChanged;
        internal void OnNodesChanged(TreeModelEventArgs args)
        {
            if (NodesChanged != null)
                NodesChanged(this, args);
        }

        public event EventHandler<TreePathEventArgs>? StructureChanged;
        public void OnStructureChanged(TreePathEventArgs args)
        {
            if (StructureChanged != null)
                StructureChanged(this, args);
        }

        public event EventHandler<TreeModelEventArgs>? NodesInserted;
        internal void OnNodeInserted(Node parent, int index, Node node)
        {
            if (NodesInserted != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesInserted(this, args);
            }

        }

        public event EventHandler<TreeModelEventArgs>? NodesRemoved;
        internal void OnNodeRemoved(Node parent, int index, Node node)
        {
            if (NodesRemoved != null)
            {
                TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
                NodesRemoved(this, args);
            }
        }

        public class NewTable
        {
            public readonly Schema Schema;
            public NewTable(Schema s)
            {
                Schema = s;
            }
        }
    }
}

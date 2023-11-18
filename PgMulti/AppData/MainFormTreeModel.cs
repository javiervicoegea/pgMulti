using System.Collections.ObjectModel;
using Aga.Controls.Tree;
using PgMulti.DataStructure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PgMulti.AppData
{
    public class MainFormTreeModel : ITreeModel
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

        public MainFormTreeModel()
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
                    foreach (Table t in schema.Tables.OrderBy(ti => ti.Id))
                    {
                        Node n = new Node(t.Id);
                        n.Image = Properties.Resources.tva_table;
                        node.Nodes.Add(n);
                        n.Tag = t;
                        yield return n;
                    }

                    if (schema.Functions.Count > 0)
                    {
                        Node n = new Node(Properties.Text.functions);
                        n.Image = Properties.Resources.tva_function;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Schema, string>(schema, "functions");
                        yield return n;
                    }
                }
                else if (node.Tag is Table)
                {
                    Table table = (Table)node.Tag;
                    Node n;

                    n = new Node(Properties.Text.columns);
                    n.Image = Properties.Resources.tva_columns;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Table, string>(table, "columns");
                    yield return n;

                    n = new Node(Properties.Text.relations);
                    n.Image = Properties.Resources.tva_relations;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Table, string>(table, "relations");
                    yield return n;

                    if (table.Indexes.Count > 0)
                    {
                        n = new Node(Properties.Text.indexes);
                        n.Image = Properties.Resources.tva_index;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Table, string>(table, "indexes");
                        yield return n;
                    }

                    if (table.Triggers.Count > 0)
                    {
                        n = new Node(Properties.Text.triggers);
                        n.Image = Properties.Resources.tva_trigger;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Table, string>(table, "triggers");
                        yield return n;
                    }
                }
                else if (node.Tag is Tuple<Table, string>)
                {
                    Tuple<Table, string> tuple = (Tuple<Table, string>)node.Tag;
                    Table table = tuple.Item1;

                    switch (tuple.Item2)
                    {
                        case "columns":
                            foreach (Column c in table.Columns.OrderBy(ci => !ci.PK).ThenBy<Column, string>(ci => ci.Id)) //.ThenBy(ci => ci.Orden))
                            {
                                Node n = new Node(c.Id);

                                if (c.PK)
                                {
                                    n.Image = Properties.Resources.tva_key;
                                }
                                else
                                {
                                    switch (c.Type)
                                    {
                                        case "smallint":
                                        case "integer":
                                        case "bigint":
                                        case "int":
                                        case "int2":
                                        case "int4":
                                        case "int8":
                                        case "serial":
                                        case "smallserial":
                                        case "bigserial":
                                        case "serial2":
                                        case "serial4":
                                        case "serial8":
                                            n.Image = Properties.Resources.tva_int;
                                            break;
                                        case "money":
                                            n.Image = Properties.Resources.tva_money;
                                            break;
                                        case "decimal":
                                        case "real":
                                        case "float":
                                        case "numeric":
                                        case "double precision":
                                            n.Image = Properties.Resources.tva_number;
                                            break;
                                        case "bit":
                                        case "bit varying":
                                        case "bytea":
                                        case "varbit":
                                            n.Image = Properties.Resources.tva_binary;
                                            break;
                                        case "boolean":
                                        case "bool":
                                            n.Image = Properties.Resources.tva_bool;
                                            break;
                                        case "date":
                                        case "datetime":
                                        case "timestamp":
                                        case "timestamp with time zone":
                                        case "timestamp without time zone":
                                            n.Image = Properties.Resources.tva_date;
                                            break;
                                        case "time":
                                        case "time with time zone":
                                        case "time without time zone":
                                            n.Image = Properties.Resources.tva_time;
                                            break;
                                        case "text":
                                        case "character":
                                        case "character varying":
                                        case "char":
                                        case "varchar":
                                            n.Image = Properties.Resources.tva_text;
                                            break;
                                        default:
                                            n.Image = Properties.Resources.tva_dato;
                                            break;
                                    }
                                }

                                node.Nodes.Add(n);
                                n.Tag = c;
                                yield return n;
                            }
                            break;
                        case "relations":
                            foreach (TableRelation rt in table.Relations.OrderBy(ri => ri.IdParentTable == table.Id ? ri.ChildTable!.Id : ri.ParentTable!.Id))
                            {
                                Node n;

                                if (rt.IdParentTable == table.Id)
                                {
                                    n = new Node(rt.ChildTable!.Id + " (" + rt.Id + ")");
                                    n.Image = Properties.Resources.tva_1n;
                                    n.Tag = new Tuple<TableRelation, Table>(rt, rt.ChildTable)!;
                                }
                                else
                                {
                                    n = new Node(rt.ParentTable!.Id + " (" + rt.Id + ")");
                                    n.Image = Properties.Resources.tva_n1;
                                    n.Tag = new Tuple<TableRelation, Table>(rt, rt.ParentTable)!;
                                }

                                node.Nodes.Add(n);
                                yield return n;
                            }
                            break;
                        case "indexes":
                            foreach (TableIndex ind in table.Indexes.OrderBy(indi => indi.Id))
                            {
                                Node n;

                                n = new Node((table.IdSchema == ind.IdSchema ? "" : ind.IdSchema + ".") + ind.Id);
                                n.Image = Properties.Resources.tva_index;
                                n.Tag = ind;

                                node.Nodes.Add(n);
                                yield return n;
                            }
                            break;
                        case "triggers":
                            foreach (Trigger trigger in table.Triggers.OrderBy(ti => ti.Id))
                            {
                                Node n;

                                n = new Node((table.IdSchema == trigger.IdSchema ? "" : trigger.IdSchema + ".") + trigger.Id);
                                n.Image = Properties.Resources.tva_trigger;
                                n.Tag = trigger;

                                node.Nodes.Add(n);
                                yield return n;
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else if (node.Tag is Tuple<Schema, string>)
                {
                    Tuple<Schema, string> tuple = (Tuple<Schema, string>)node.Tag;
                    Schema schema = tuple.Item1;

                    switch (tuple.Item2)
                    {
                        case "functions":
                            foreach (Function f in schema.Functions.OrderBy(fi => fi.Id))
                            {
                                Node n;

                                n = new Node(f.Id);
                                n.Image = Properties.Resources.tva_function;
                                n.Tag = f;

                                node.Nodes.Add(n);
                                yield return n;
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else if (node.Tag is Tuple<TableRelation, Table>)
                {
                    Tuple<TableRelation, Table> tuple = (Tuple<TableRelation, Table>)node.Tag;
                    TableRelation rt = tuple.Item1;
                    Table tabla = tuple.Item2;

                    Node n;
                    bool sameSchema = rt.IdParentSchema == rt.IdChildSchema;


                    if (rt.IdParentTable == tabla.Id)
                    {
                        n = new Node((sameSchema ? "" : rt.IdParentSchema + ".") + rt.ParentTable!.Id + $" ({Properties.Text.parent_table})");
                        n.Image = Properties.Resources.tva_table;
                        n.Tag = rt.ParentTable!;
                    }
                    else
                    {
                        n = new Node((sameSchema ? "" : rt.IdChildSchema + ".") + rt.ChildTable!.Id + $" ({Properties.Text.child_table})");
                        n.Image = Properties.Resources.tva_table;
                        n.Tag = rt.ChildTable!;
                    }

                    node.Nodes.Add(n);
                    yield return n;

                    n = new Node((sameSchema ? "" : rt.IdChildSchema + ".") + rt.IdChildTable + "(" + string.Join(",", rt.ChildColumns) + ") → " + (sameSchema ? "" : rt.IdParentSchema + ".") + rt.IdParentTable + "(" + string.Join(",", rt.ParentColumns) + ")");
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableRelation, string>(rt, "definition");
                    yield return n;

                    n = new Node("on delete " + rt.OnDelete.ToLower());
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableRelation, string>(rt, "on_delete");
                    yield return n;

                    n = new Node("on update " + rt.OnUpdate.ToLower());
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableRelation, string>(rt, "on_update");
                    yield return n;

                    n = new Node(rt.Deferrable ? "deferrable" : "not deferrable");
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableRelation, string>(rt, "deferrable");
                    yield return n;

                    n = new Node(rt.InitiallyDeferred ? "initially deferred" : "initially immediate");
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableRelation, string>(rt, "initially deferred");
                    yield return n;
                }
                else if (node.Tag is Tuple<Function, string>)
                {
                    Tuple<Function, string> tuple = (Tuple<Function, string>)node.Tag;
                    Function func = tuple.Item1;

                    switch (tuple.Item2)
                    {
                        case "triggers":
                            foreach (Trigger trigger in func.Triggers.OrderBy(ti => ti.Id))
                            {
                                Node n;

                                n = new Node((func.IdSchema == trigger.IdSchema ? "" : trigger.IdSchema + ".") + trigger.Id);
                                n.Image = Properties.Resources.tva_trigger;
                                n.Tag = trigger;

                                node.Nodes.Add(n);
                                yield return n;
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else if (node.Tag is Column)
                {
                    Column column = (Column)node.Tag;
                    Node n;

                    if (column.PK)
                    {
                        n = new Node("primary key");
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Column, string>(column, "pk");
                        yield return n;
                    }

                    n = new Node(column.Type + (column.TypeParams == null ? "" : column.TypeParams));
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Column, string>(column, "type");
                    yield return n;

                    n = new Node(column.NotNull ? "not null" : "null");
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Column, string>(column, "not_null");
                    yield return n;

                    if (column.DefaultValue != null)
                    {
                        n = new Node("default: " + column.DefaultValue);
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Column, string>(column, "default");
                        yield return n;
                    }

                    if (column.IsIdentity)
                    {
                        n = new Node("identity");
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Column, string>(column, "identity");
                        yield return n;
                    }
                }
                else if (node.Tag is TableIndex)
                {
                    TableIndex ind = (TableIndex)node.Tag;
                    Node n;

                    n = new Node(ind.OrderList);
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableIndex, string>(ind, "order_list");
                    yield return n;

                    if (ind.Unique)
                    {
                        n = new Node("unique");
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<TableIndex, string>(ind, "unique");
                        yield return n;
                    }

                    n = new Node(ind.Using);
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<TableIndex, string>(ind, "using");
                    yield return n;

                    if (ind.Filter != null)
                    {
                        n = new Node(ind.Filter);
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<TableIndex, string>(ind, "filter");
                        yield return n;
                    }
                }
                else if (node.Tag is Function)
                {
                    Function f = (Function)node.Tag;
                    Node n;

                    if (!string.IsNullOrEmpty(f.Arguments))
                    {
                        n = new Node($"{Properties.Text.function_arguments}: {f.Arguments}");
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Function, string>(f, "arguments");
                        yield return n;
                    }

                    if (!string.IsNullOrEmpty(f.Returns))
                    {
                        n = new Node($"{Properties.Text.function_returns}: {f.Returns}");
                        n.Image = Properties.Resources.tva_element;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Function, string>(f, "returns");
                        yield return n;
                    }

                    n = new Node($"[{Properties.Text.source_code}]");
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Function, string>(f, "source_code");
                    yield return n;

                    if (f.Triggers.Count > 0)
                    {
                        n = new Node(Properties.Text.triggers);
                        n.Image = Properties.Resources.tva_trigger;
                        node.Nodes.Add(n);
                        n.Tag = new Tuple<Function, string>(f, "triggers");
                        yield return n;
                    }
                }
                else if (node.Tag is Trigger)
                {
                    Trigger trigger = (Trigger)node.Tag;
                    Node n;

                    n = new Node(trigger.Momentum);
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Trigger, string>(trigger, "momentum");
                    yield return n;

                    n = new Node(trigger.Action);
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Trigger, string>(trigger, "action");
                    yield return n;

                    n = new Node(trigger.Repetition);
                    n.Image = Properties.Resources.tva_element;
                    node.Nodes.Add(n);
                    n.Tag = new Tuple<Trigger, string>(trigger, "repetition");
                    yield return n;

                    n = new Node((trigger.IdSchema == trigger.Table!.IdSchema ? "" : trigger.Table!.IdSchema + ".") + trigger.Table!.Id);
                    n.Image = Properties.Resources.tva_table;
                    node.Nodes.Add(n);
                    n.Tag = trigger.Table!;
                    yield return n;

                    n = new Node((trigger.IdSchema == trigger.Function!.IdSchema ? "" : trigger.Function!.IdSchema + ".") + trigger.Function!.Id);
                    n.Image = Properties.Resources.tva_function;
                    node.Nodes.Add(n);
                    n.Tag = trigger.Function!;
                    yield return n;
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
                if (node.Tag is Tuple<Column, string>)
                {
                    return true;
                }
                else if (node.Tag is Tuple<TableRelation, string>)
                {
                    return true;
                }
                else if (node.Tag is Tuple<TableIndex, string>)
                {
                    return true;
                }
                else if (node.Tag is Tuple<Function, string>)
                {
                    return ((Tuple<Function, string>)node.Tag!).Item2 != "triggers";
                }
                else if (node.Tag is Group)
                {
                    return node.Nodes.Count == 0;
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
    }
}

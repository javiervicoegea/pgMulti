using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover
{
    public class RecursiveRemover
    {
        public Table RootTable;
        public Graph<RecursiveRemoverGraphElement> Graph;

        public RecursiveRemover(Table rootTable, string rootTableWhereClause)
        {
            RootTable = rootTable;
            Node<RecursiveRemoverGraphElement> rootNode = new Node<RecursiveRemoverGraphElement>(new RootTableRecursiveRemoverGraphElement(rootTable, rootTableWhereClause));

            List<Node<RecursiveRemoverGraphElement>> nodes = new List<Node<RecursiveRemoverGraphElement>>();
            List<Arrow<RecursiveRemoverGraphElement>> arrows = new List<Arrow<RecursiveRemoverGraphElement>>();

            Stack<Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>>> stack = new Stack<Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>>>();
            stack.Push(new Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>>(new List<Node<RecursiveRemoverGraphElement>>(), rootNode));

            nodes.Add(rootNode);

            while (stack.Count > 0)
            {
                Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>> currentTuple = stack.Pop();
                List<Node<RecursiveRemoverGraphElement>> currentParents = currentTuple.Item1;
                Node<RecursiveRemoverGraphElement> currentNode = currentTuple.Item2;

                List<Node<RecursiveRemoverGraphElement>> childParents = new List<Node<RecursiveRemoverGraphElement>>(currentParents);
                childParents.Add(currentNode);

                foreach (TableRelation tr in currentNode.Value.ChildRelations)
                {
                    int index = childParents.FindIndex(n => n.Value.ContainsTable(tr.ChildTable!));

                    if (index == -1)
                    {
                        Node<RecursiveRemoverGraphElement> childNode = new Node<RecursiveRemoverGraphElement>(new SingleTableRecursiveRemoverGraphElement(tr.ChildTable!));
                        Arrow<RecursiveRemoverGraphElement> outgoingArrow = new Arrow<RecursiveRemoverGraphElement>(currentNode, childNode);
                        currentNode.OutgoingArrows.Add(outgoingArrow);
                        childNode.IncomingArrows.Add(outgoingArrow);

                        nodes.Add(childNode);
                        arrows.Add(outgoingArrow);

                        Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>> childTuple = new Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>>(childParents, childNode);

                        stack.Push(childTuple);
                    }
                    else
                    {
                        // Loop found

                        List<Table> loopTables = new List<Table>();

                        for (int i = index; i < childParents.Count; i++)
                        {
                            Node<RecursiveRemoverGraphElement> loopInnerNode = childParents[i];
                            loopInnerNode.Value.AddTablesToList(loopTables);
                        }

                        Node<RecursiveRemoverGraphElement> loopNode = new Node<RecursiveRemoverGraphElement>(new LoopTablesRecursiveRemoverGraphElement(loopTables));

                        for (int i = index; i < childParents.Count; i++)
                        {
                            Node<RecursiveRemoverGraphElement> loopInnerNode = childParents[i];

                            foreach (Arrow<RecursiveRemoverGraphElement> arrow in loopInnerNode.IncomingArrows)
                            {
                                if (loopNode.Value.ContainsElement(arrow.Source.Value))
                                {
                                    // Inner arrow
                                    arrows.Remove(arrow);
                                }
                                else
                                {
                                    arrow.Target = loopNode;
                                    loopNode.IncomingArrows.Add(arrow);
                                }
                            }

                            foreach (Arrow<RecursiveRemoverGraphElement> arrow in loopInnerNode.OutgoingArrows)
                            {
                                if (loopNode.Value.ContainsElement(arrow.Target.Value))
                                {
                                    // Inner arrow
                                    arrows.Remove(arrow);
                                }
                                else
                                {
                                    arrow.Source = loopNode;
                                    loopNode.OutgoingArrows.Add(arrow);
                                }
                            }

                            nodes.Remove(loopInnerNode);
                        }

                        nodes.Add(loopNode);
                    }
                }
            }

            Graph = new Graph<RecursiveRemoverGraphElement>(nodes, arrows);
        }

        public void WriteCollectTuplesSqlCommands(StringBuilder sb)
        {
            sb.AppendLine("/* SCRIPT TO COLLECT TUPLES TO DELETE */\r\n");

            sb.AppendLine("CREATE SCHEMA recursiveremover;\r\n");

            foreach (Node<RecursiveRemoverGraphElement> n in Graph.CreateTraverseGraphEnumerable(true))
            {

            }
        }
    }
}

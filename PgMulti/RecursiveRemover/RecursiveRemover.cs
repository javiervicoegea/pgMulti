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
        public readonly Table RootTable;
        public readonly Graph<RecursiveRemoverGraphElement> Graph;
        public readonly string CollectTablesSchemaName;

        public RecursiveRemover(string collectTablesSchemaName, Table rootTable, string rootDeleteWhereClause, string rootPreserveTableWhereClause)
        {
            CollectTablesSchemaName = collectTablesSchemaName;
            RootTable = rootTable;

            Node<RecursiveRemoverGraphElement> rootNode = new Node<RecursiveRemoverGraphElement>(new RootTableRecursiveRemoverGraphElement(collectTablesSchemaName, rootTable, rootDeleteWhereClause, rootPreserveTableWhereClause));

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
                        // It is not a loop

                        int index2 = nodes.FindIndex(n => n.Value.ContainsTable(tr.ChildTable!));

                        Node<RecursiveRemoverGraphElement> childNode;

                        if (index2 == -1)
                        {
                            // New node

                            childNode = new Node<RecursiveRemoverGraphElement>(new SingleTableRecursiveRemoverGraphElement(collectTablesSchemaName, tr.ChildTable!));

                            nodes.Add(childNode);

                            Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>> childTuple = new Tuple<List<Node<RecursiveRemoverGraphElement>>, Node<RecursiveRemoverGraphElement>>(childParents, childNode);

                            stack.Push(childTuple);
                        }
                        else
                        {
                            // Already existing node

                            childNode = nodes[index2];
                        }

                        Arrow<RecursiveRemoverGraphElement> incomingArrow = new Arrow<RecursiveRemoverGraphElement>(childNode, currentNode);
                        currentNode.IncomingArrows.Add(incomingArrow);
                        childNode.OutgoingArrows.Add(incomingArrow);

                        arrows.Add(incomingArrow);
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

                        Node<RecursiveRemoverGraphElement> loopNode = new Node<RecursiveRemoverGraphElement>(new LoopTablesRecursiveRemoverGraphElement(collectTablesSchemaName, loopTables));

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

        public void WriteCollectTuplesScript(StringBuilder sb)
        {
            sb.AppendLine("/* SCRIPT TO COLLECT TUPLES TO DELETE */\r\n");

            sb.AppendLine("CREATE SCHEMA " + CollectTablesSchemaName + ";\r\n");

            foreach (Node<RecursiveRemoverGraphElement> n in Graph.CreateTraverseGraphEnumerable(false))
            {
                n.Value.WriteCollectTuplesSqlCommands(sb, true);
            }

            sb.AppendLine("/* SCRIPT TO COLLECT TUPLES TO PRESERVE */\r\n");

            foreach (Node<RecursiveRemoverGraphElement> n in Graph.CreateTraverseGraphEnumerable(false))
            {
                n.Value.WriteCollectTuplesSqlCommands(sb, false);
            }

            sb.AppendLine("/* SCRIPT TO CALCULATE CONFLICTS AND STATISTICS */\r\n");

            sb.AppendLine("CREATE TABLE " + CollectTablesSchemaName + ".statistics");
            sb.AppendLine("(");
            sb.AppendLine("    schemaName TEXT NOT NULL,");
            sb.AppendLine("    tableName TEXT NOT NULL,");
            sb.AppendLine("    currentTuples INTEGER NOT NULL,");
            sb.AppendLine("    tuplesToDelete INTEGER NOT NULL,");
            sb.AppendLine("    tuplesToPreserve INTEGER NOT NULL,");
            sb.AppendLine("    conflicts INTEGER NOT NULL,");
            sb.AppendLine("    CONSTRAINT statistics_pkey PRIMARY KEY (schemaName,tableName)");
            sb.AppendLine(");\r\n");

            foreach (Table t in Graph.Nodes.SelectMany(ni => ni.Value.Tables).OrderBy(ti => ti.IdSchema).ThenBy(ti => ti.Id))
            {
                string deleteTuplesTableName = GetCollectTableName(CollectTablesSchemaName, t, true);
                string preserveTuplesTableName = GetCollectTableName(CollectTablesSchemaName, t, false);
                string conflictsTableName = GetConflictsTableName(CollectTablesSchemaName, t);
                string[] pkColumns = t.Columns.Where(c => c.PK).Select(c => c.Id).ToArray();

                sb.AppendLine("-- TABLE " + t.IdSchema + "." + t.Id + ":\r\n");

                WriteCreateCollectTableSqlCommand(sb, 3, t, conflictsTableName);

                sb.AppendLine("   INSERT INTO " + conflictsTableName);
                sb.AppendLine("   (" + string.Join(",", pkColumns) + ")");
                sb.AppendLine("   SELECT " + string.Join(",", pkColumns.Select(s => "d." + s).ToArray()));
                sb.AppendLine("   FROM " + deleteTuplesTableName + " d");
                sb.AppendLine("   WHERE EXISTS");
                sb.AppendLine("   (");
                sb.AppendLine("       SELECT 1");
                sb.AppendLine("       FROM " + preserveTuplesTableName + " p");
                sb.AppendLine("       WHERE " + string.Join(" AND ", pkColumns.Select(s => "d." + s + " = p." + s)));
                sb.AppendLine("   );\r\n");

                sb.AppendLine("   INSERT INTO " + CollectTablesSchemaName + ".statistics");
                sb.AppendLine("   (schemaName, tableName, currentTuples, tuplesToDelete, tuplesToPreserve, conflicts)");
                sb.AppendLine("   VALUES");
                sb.AppendLine("   (");
                sb.AppendLine("       '" + t.IdSchema + "',");
                sb.AppendLine("       '" + t.Id + "',");
                sb.AppendLine("       (SELECT COUNT(*) FROM " + t.IdSchema + "." + t.Id + "),");
                sb.AppendLine("       (SELECT COUNT(*) FROM " + deleteTuplesTableName + "),");
                sb.AppendLine("       (SELECT COUNT(*) FROM " + preserveTuplesTableName + "),");
                sb.AppendLine("       (SELECT COUNT(*) FROM " + conflictsTableName + ")");
                sb.AppendLine("   );\r\n");
            }
        }

        public void WriteDeleteScript(StringBuilder sb)
        {
            sb.AppendLine("/*");
            sb.AppendLine("   SCRIPT TO DELETE TUPLES\r\n");
            sb.AppendLine("   CAUTION: Do not run this script without reviewing and resolving any detected conflicts and reading the documentation.");
            sb.AppendLine("   This script may cause unwanted deletion of data.");
            sb.AppendLine("*/\r\n");

            foreach (Node<RecursiveRemoverGraphElement> n in Graph.CreateTraverseGraphEnumerable(true))
            {
                n.Value.WriteDeleteTuplesSqlCommands(sb);
            }

            sb.AppendLine("-- Finally, the schema created by this tool can be dropped:\r\n");

            sb.AppendLine("-- DROP SCHEMA " + CollectTablesSchemaName + " CASCADE;\r\n");
        }

        internal static string GetCollectTableName(string schemaName, Table t, bool delete)
        {
            return schemaName + "." + (delete ? "delete_" : "preserve_") + t.IdSchema + "_" + t.Id;
        }

        internal static string GetConflictsTableName(string schemaName, Table t)
        {
            return schemaName + ".conflicts_" + t.IdSchema + "_" + t.Id;
        }

        internal static void WriteCreateCollectTableSqlCommand(StringBuilder sb, int indentation, Table t, string collectTableName)
        {
            string indentationString = new string(' ', indentation);
            sb.AppendLine(indentationString + "CREATE TABLE " + collectTableName);
            sb.AppendLine(indentationString + "(");
            sb.AppendLine(indentationString + string.Join(",\r\n", t.Columns.Where(c => c.PK).Select(c => "    " + c.Id + " " + c.Type + c.TypeParams + " PRIMARY KEY").ToArray()));
            sb.AppendLine(indentationString + ");\r\n");
        }
    }
}

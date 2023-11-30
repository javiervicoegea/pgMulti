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

            Node<RecursiveRemoverGraphElement> rootNode = new Node<RecursiveRemoverGraphElement>(new RootTableRecursiveRemoverGraphElement(collectTablesSchemaName, this, rootTable, rootDeleteWhereClause, rootPreserveTableWhereClause));

            Dictionary<Table, Node<RecursiveRemoverGraphElement>> nodes = new Dictionary<Table, Node<RecursiveRemoverGraphElement>>();
            List<Arrow<RecursiveRemoverGraphElement>> arrows = new List<Arrow<RecursiveRemoverGraphElement>>();

            Stack<List<Table>> stack = new Stack<List<Table>>();
            stack.Push(new List<Table>() { rootTable });

            nodes[rootTable] = rootNode;

            while (stack.Count > 0)
            {
                List<Table> childTableParents = stack.Pop();

                Table currentTable = childTableParents.Last();

                foreach (TableRelation tr in currentTable.Relations.Where(tri => tri.ParentTable == currentTable))
                {
                    Table childTable = tr.ChildTable!;
                    int index = childTableParents.FindIndex(t => nodes[t].Value.ContainsTable(childTable));

                    if (index == -1)
                    {
                        // It is not closing a new loop

                        Node<RecursiveRemoverGraphElement> childNode;

                        if (nodes.ContainsKey(childTable))
                        {
                            // Already existing node

                            childNode = nodes[childTable];
                        }
                        else
                        {
                            // New node

                            childNode = new Node<RecursiveRemoverGraphElement>(new SingleTableRecursiveRemoverGraphElement(collectTablesSchemaName, this, childTable));

                            nodes[childTable] = childNode;
                        }

                        List<Table> grandsonTableParents = new List<Table>(childTableParents);
                        grandsonTableParents.Add(childTable);

                        stack.Push(grandsonTableParents);

                        Node<RecursiveRemoverGraphElement> currentNode = nodes[currentTable];

                        if (!currentNode.IncomingNodes.Contains(childNode))
                        {
                            Arrow<RecursiveRemoverGraphElement> incomingArrow = new Arrow<RecursiveRemoverGraphElement>(childNode, currentNode);
                            currentNode.IncomingArrows.Add(incomingArrow);
                            childNode.OutgoingArrows.Add(incomingArrow);

                            arrows.Add(incomingArrow);
                        }
                    }
                    else
                    {
                        // New loop closure found

                        List<Table> loopTables = new List<Table>();
                        List<Node<RecursiveRemoverGraphElement>> innerLoopNodes = new List<Node<RecursiveRemoverGraphElement>>();

                        for (int i = index; i < childTableParents.Count; i++)
                        {
                            Node<RecursiveRemoverGraphElement> innerLoopNode = nodes[childTableParents[i]];
                            innerLoopNode.Value.AddTablesToList(loopTables);
                            innerLoopNodes.Add(innerLoopNode);
                        }

                        Node<RecursiveRemoverGraphElement> loopNode;

                        if (loopTables.Contains(rootTable))
                        {
                            loopNode = new Node<RecursiveRemoverGraphElement>(new LoopTablesWithRootTableRecursiveRemoverGraphElement(collectTablesSchemaName, this, loopTables, rootTable, rootDeleteWhereClause, rootPreserveTableWhereClause));
                        }
                        else
                        {
                            loopNode = new Node<RecursiveRemoverGraphElement>(new LoopTablesRecursiveRemoverGraphElement(collectTablesSchemaName, this, loopTables));
                        }

                        foreach (Table t in loopTables)
                        {
                            nodes[t] = loopNode;
                        }

                        foreach (Node<RecursiveRemoverGraphElement> innerLoopNode in innerLoopNodes)
                        {
                            foreach (Arrow<RecursiveRemoverGraphElement> arrow in innerLoopNode.IncomingArrows)
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

                            foreach (Arrow<RecursiveRemoverGraphElement> arrow in innerLoopNode.OutgoingArrows)
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
                        }
                    }
                }
            }

            Graph = new Graph<RecursiveRemoverGraphElement>(new List<Node<RecursiveRemoverGraphElement>>(nodes.Values.Distinct()), arrows);
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
                string[] pkColumns = t.Columns.Where(c => c.PK).Select(c => SqlSyntax.PostgreSqlGrammar.IdToString(c.Id)).ToArray();

                sb.AppendLine("-- TABLE " + SqlSyntax.PostgreSqlGrammar.IdToString(t.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(t.Id) + ":\r\n");

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
                sb.AppendLine("       (SELECT COUNT(*) FROM " + SqlSyntax.PostgreSqlGrammar.IdToString(t.IdSchema) + "." + SqlSyntax.PostgreSqlGrammar.IdToString(t.Id) + "),");
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
            return schemaName + "." + SqlSyntax.PostgreSqlGrammar.IdToString((delete ? "delete_" : "preserve_") + t.IdSchema + "_" + t.Id);
        }

        internal static string GetConflictsTableName(string schemaName, Table t)
        {
            return schemaName + "." + SqlSyntax.PostgreSqlGrammar.IdToString("conflicts_" + t.IdSchema + "_" + t.Id);
        }

        internal static void WriteCreateCollectTableSqlCommand(StringBuilder sb, int indentation, Table t, string collectTableName)
        {
            string indentationString = new string(' ', indentation);
            sb.AppendLine(indentationString + "CREATE TABLE " + collectTableName);
            sb.AppendLine(indentationString + "(");
            sb.AppendLine(indentationString + string.Join(",\r\n", t.Columns.Where(c => c.PK).Select(c => "    " + SqlSyntax.PostgreSqlGrammar.IdToString(c.Id) + " " + c.Type + c.TypeParams + " PRIMARY KEY").ToArray()));
            sb.AppendLine(indentationString + ");\r\n");
        }

        internal string GetStepTableTuplesTableName(Table t, bool delete)
        {
            string id = (delete ? "d_" : "p_") + t.IdSchema + "_" + t.Id + "_st";
            if (id.Length > 60)
            {
                id = id.Substring(0, 50) + "_" + GetTableUniqueId(id) + "_st";
            }
            return SqlSyntax.PostgreSqlGrammar.IdToString(id);
        }

        internal string GetStepRelationTuplesTableName(TableRelation tr, bool delete)
        {
            string id = (delete ? "d" : "p") + "fk_" + tr.ChildTable!.IdSchema + "_" + tr.ChildTable!.Id + "_" + tr.Id + "_st";
            if (id.Length > 60)
            {
                id = id.Substring(0, 50) + "_" + GetTableUniqueId(id) + "_st";
            }
            return SqlSyntax.PostgreSqlGrammar.IdToString(id);
        }

        private int _TableUniqueId = 0;
        private Dictionary<string, string> _AssignedTableUniqueId = new Dictionary<string, string>();
        private string GetTableUniqueId(string originalId)
        {
            string assignedId;

            if (_AssignedTableUniqueId.ContainsKey(originalId))
            {
                assignedId = _AssignedTableUniqueId[originalId];
            }
            else
            {
                assignedId = (_TableUniqueId++).ToString();
                _AssignedTableUniqueId[originalId] = assignedId;
            }
            return assignedId;
        }

    }
}

using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using PgMulti.SqlSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover
{
    public class LoopTablesRecursiveRemoverGraphElement : RecursiveRemoverGraphElement
    {
        private List<Table> _Tables { get; }

        public LoopTablesRecursiveRemoverGraphElement(string schemaName, RecursiveRemover recursiveRemover, List<Table> tables) : base(schemaName, recursiveRemover)
        {
            _Tables = tables;
        }

        public override List<Table> Tables
        {
            get
            {
                return _Tables;
            }
        }

        public override bool ContainsTable(Table t)
        {
            return _Tables.Contains(t);
        }

        public override void AddTablesToList(List<Table> list)
        {
            foreach (Table t in _Tables)
            {
                if (!list.Contains(t)) list.Add(t);
            }
        }

        protected virtual void _WriteRootInsertSqlCommand(Table t, StringBuilder sb, bool delete, string stepTuplesTableName) { }

        public override void WriteCollectTuplesSqlCommands(StringBuilder sb, bool delete)
        {
            sb.AppendLine("-- LOOP FOR TABLES " + string.Join(", ", _Tables.Select(t => t.IdSchema + "." + t.Id)) + ":\r\n");

            foreach (Table t in _Tables)
            {
                string collectTuplesTableName = GetCollectTableName(t, delete);
                string stepTuplesTableName = RecursiveRemover.GetStepTableTuplesTableName(t, delete);

                sb.AppendLine("--- Loop table " + PostgreSqlGrammar.IdToString(t.IdSchema) + "." + PostgreSqlGrammar.IdToString(t.Id) + ":\r\n");

                _WriteCreateTableSqlCommand(sb, 4, t, delete);

                string tablePKColumnsDef = "    " + string.Join(",\r\n        ", t.Columns.Where(c => c.PK).Select(c => "    " + PostgreSqlGrammar.IdToString(c.Id) + " " + c.Type + c.TypeParams).ToArray()) + ",\r\n"
                    + "        PRIMARY KEY (" + string.Join(",", t.Columns.Where(c => c.PK).Select(c => PostgreSqlGrammar.IdToString(c.Id)).ToArray()) + ")";


                sb.AppendLine("    CREATE TEMPORARY TABLE " + stepTuplesTableName);
                sb.AppendLine("    (");
                sb.AppendLine("    " + tablePKColumnsDef);
                sb.AppendLine("    );\r\n");

                sb.AppendLine("---- Temporary tables for foreign key references to child tables within the loop:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);

                    sb.AppendLine("     CREATE TEMPORARY TABLE " + stepRelationTuplesTableName);
                    sb.AppendLine("     (");
                    sb.AppendLine("     " + tablePKColumnsDef);
                    sb.AppendLine("     );\r\n");
                }

                _WriteRootInsertSqlCommand(t, sb, delete, stepTuplesTableName);

                sb.AppendLine("---- Tuples of parent tables external to the loop:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && !_Tables.Contains(tri.ParentTable!) && RecursiveRemover.Graph.Nodes.Any(ni => ni.Value.ContainsTable(tri.ParentTable!))))
                {
                    string collectTuplesParentTableName = GetCollectTableName(tr.ParentTable!, delete);
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);
                    Tuple<string, string>[] fkColumnMatch = new Tuple<string, string>[tr.ParentColumns.Length];

                    for (int i = 0; i < tr.ParentColumns.Length; i++)
                    {
                        fkColumnMatch[i] = new Tuple<string, string>(tr.ParentColumns[i], tr.ChildColumns[i]);
                    }

                    sb.AppendLine("----- FK " + tr.Id + " to " + tr.ParentTable!.IdSchema + "." + tr.ParentTable!.Id + ":\r\n");
                    sb.AppendLine("      INSERT INTO " + stepTuplesTableName);
                    sb.AppendLine("      (" + string.Join(",", t.Columns.Where(c => c.PK).Select(c => PostgreSqlGrammar.IdToString(c.Id))) + ")");
                    sb.AppendLine("      SELECT " + string.Join(",", t.Columns.Where(c => c.PK).Select(c => "t." + PostgreSqlGrammar.IdToString(c.Id)).ToArray()));
                    sb.AppendLine("      FROM " + PostgreSqlGrammar.IdToString(t!.IdSchema) + "." + PostgreSqlGrammar.IdToString(t!.Id) + " t");
                    sb.AppendLine("      WHERE EXISTS");
                    sb.AppendLine("      (");
                    sb.AppendLine("          SELECT 1");
                    sb.AppendLine("          FROM " + collectTuplesParentTableName + " r");
                    sb.AppendLine("          WHERE " + string.Join(" AND ", fkColumnMatch.Select(mi => "r." + PostgreSqlGrammar.IdToString(mi.Item1) + " = t." + PostgreSqlGrammar.IdToString(mi.Item2))));
                    sb.AppendLine("      )");
                    sb.AppendLine("      ON CONFLICT DO NOTHING;\r\n");
                }

                sb.AppendLine("---- Transfer tuples to FKs within the loop:\r\n");

                sb.AppendLine("     INSERT INTO " + collectTuplesTableName);
                sb.AppendLine("     SELECT *");
                sb.AppendLine("     FROM " + stepTuplesTableName + ";\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);

                    sb.AppendLine("----- FK " + tr.Id + " from " + tr.ChildTable!.IdSchema + "." + tr.ChildTable!.Id + ":\r\n");
                    sb.AppendLine("      INSERT INTO " + stepRelationTuplesTableName);
                    sb.AppendLine("      SELECT *");
                    sb.AppendLine("      FROM " + stepTuplesTableName + ";\r\n");
                }

                sb.AppendLine("      TRUNCATE TABLE " + stepTuplesTableName + ";\r\n");
            }

            sb.AppendLine("--- Iterate the loop to get all the tuples:\r\n");

            sb.AppendLine("    CREATE FUNCTION " + SchemaName + ".exploreloop() RETURNS VOID AS $$");
            sb.AppendLine("        DECLARE");
            sb.AppendLine("            n INTEGER;");
            sb.AppendLine("            i INTEGER;");
            sb.AppendLine("        BEGIN");
            sb.AppendLine("            LOOP");
            sb.AppendLine("                n := 0;\r\n");

            foreach (Table t in _Tables)
            {
                string collectTuplesTableName = GetCollectTableName(t, delete);
                string stepTuplesTableName = RecursiveRemover.GetStepTableTuplesTableName(t, delete);
                string tablePKColumns = "(" + string.Join(",", t.Columns.Where(c => c.PK).Select(c => PostgreSqlGrammar.IdToString(c.Id)).ToArray()) + ")";

                sb.AppendLine("                -- Table " + t.IdSchema + "." + t.Id + ":\r\n");

                sb.AppendLine("                   i := 0;\r\n");

                sb.AppendLine("                --- Parent tables:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && _Tables.Contains(tri.ParentTable!)))
                {
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);
                    Tuple<string, string>[] fkColumnMatch = new Tuple<string, string>[tr.ParentColumns.Length];

                    for (int i = 0; i < tr.ParentColumns.Length; i++)
                    {
                        fkColumnMatch[i] = new Tuple<string, string>(tr.ParentColumns[i], tr.ChildColumns[i]);
                    }

                    sb.AppendLine("                ---- FK " + tr.Id + " to " + tr.ParentTable!.IdSchema + "." + tr.ParentTable!.Id + ":\r\n");

                    sb.AppendLine("                     INSERT INTO " + stepTuplesTableName);
                    sb.AppendLine("                     " + tablePKColumns + "");
                    sb.AppendLine("                     SELECT " + string.Join(",", t.Columns.Where(c => c.PK).Select(c => "t." + PostgreSqlGrammar.IdToString(c.Id)).ToArray()) + "");
                    sb.AppendLine("                     FROM " + PostgreSqlGrammar.IdToString(t.IdSchema) + "." + PostgreSqlGrammar.IdToString(t.Id) + " t");
                    sb.AppendLine("                     WHERE EXISTS");
                    sb.AppendLine("                     (");
                    sb.AppendLine("                         SELECT 1");
                    sb.AppendLine("                         FROM " + stepRelationTuplesTableName + " r");
                    sb.AppendLine("                         WHERE " + string.Join(" AND ", fkColumnMatch.Select(mi => "r." + PostgreSqlGrammar.IdToString(mi.Item1) + " = t." + PostgreSqlGrammar.IdToString(mi.Item2))));
                    sb.AppendLine("                     )");
                    sb.AppendLine("                     AND NOT EXISTS");
                    sb.AppendLine("                     (");
                    sb.AppendLine("                         SELECT 1");
                    sb.AppendLine("                         FROM " + collectTuplesTableName + " r");
                    sb.AppendLine("                         WHERE " + string.Join(" AND ", t.Columns.Where(c => c.PK).Select(c => "r." + PostgreSqlGrammar.IdToString(c.Id) + " = t." + PostgreSqlGrammar.IdToString(c.Id))));
                    sb.AppendLine("                     )");
                    sb.AppendLine("                     ON CONFLICT DO NOTHING;\r\n");

                    sb.AppendLine("                     TRUNCATE TABLE " + stepRelationTuplesTableName + ";\r\n");
                }

                sb.AppendLine("                   SELECT COUNT(*) INTO i");
                sb.AppendLine("                   FROM " + stepTuplesTableName + ";\r\n");

                sb.AppendLine("                   INSERT INTO " + collectTuplesTableName);
                sb.AppendLine("                   SELECT *");
                sb.AppendLine("                   FROM " + stepTuplesTableName + ";\r\n");

                sb.AppendLine("                --- Child tables:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);

                    sb.AppendLine("                ---- FK " + tr.Id + " from " + tr.ChildTable!.IdSchema + "." + tr.ChildTable!.Id + ":\r\n");

                    sb.AppendLine("                     INSERT INTO " + stepRelationTuplesTableName);
                    sb.AppendLine("                     SELECT *");
                    sb.AppendLine("                     FROM " + stepTuplesTableName + ";\r\n");
                }

                sb.AppendLine("                   TRUNCATE TABLE " + stepTuplesTableName + ";\r\n");

                sb.AppendLine("                   n := n + i;\r\n");
            }

            sb.AppendLine("                n := n + i;\r\n");

            sb.AppendLine("                EXIT WHEN n = 0;\r\n");

            sb.AppendLine("            END LOOP;");
            sb.AppendLine("        END;");
            sb.AppendLine("    $$ LANGUAGE plpgsql;\r\n");

            sb.AppendLine("    SELECT " + SchemaName + ".exploreloop();\r\n");

            sb.AppendLine("--- Drop function and temporary tables:\r\n");

            sb.AppendLine("    DROP FUNCTION " + SchemaName + ".exploreloop;\r\n");

            foreach (Table t in _Tables)
            {
                string stepTuplesTableName = RecursiveRemover.GetStepTableTuplesTableName(t, delete);

                sb.AppendLine("    DROP TABLE " + stepTuplesTableName + ";\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    string stepRelationTuplesTableName = RecursiveRemover.GetStepRelationTuplesTableName(tr, delete);

                    sb.AppendLine("    DROP TABLE " + stepRelationTuplesTableName + ";\r\n");
                }
            }
        }

        public override void WriteDeleteTuplesSqlCommands(StringBuilder sb)
        {
            sb.AppendLine("-- LOOP FOR TABLES " + string.Join(", ", _Tables.Select(t => t.IdSchema + "." + t.Id)) + ":\r\n");

            sb.AppendLine("--- Transform loop inner FKs into ON DELETE NO ACTION DEFERRABLE where necessary:\r\n");

            foreach (Table t in _Tables)
            {
                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && _Tables.Contains(tri.ParentTable!)))
                {
                    if (tr.OnDelete != "NO ACTION" || !tr.Deferrable)
                    {
                        string newDefinition = tr.Definition;

                        newDefinition = Regex.Replace(newDefinition, @" ON DELETE ((NO ACTION|RESTRICT|CASCADE|SET (NULL|DEFAULT))( \([^\)]+\))?)", "");
                        newDefinition = Regex.Replace(newDefinition, @" NOT DEFERRABLE", "");

                        sb.AppendLine("    ALTER TABLE " + PostgreSqlGrammar.IdToString(tr.ChildTable!.IdSchema) + "." + PostgreSqlGrammar.IdToString(tr.ChildTable!.Id) + " DROP CONSTRAINT " + PostgreSqlGrammar.IdToString(tr.Id) + ";\r\n");
                        sb.AppendLine("    ALTER TABLE " + PostgreSqlGrammar.IdToString(tr.ChildTable!.IdSchema) + "." + PostgreSqlGrammar.IdToString(tr.ChildTable!.Id) + " ADD CONSTRAINT " + PostgreSqlGrammar.IdToString(tr.Id));
                        sb.AppendLine("    " + newDefinition + " DEFERRABLE;\r\n");
                    }
                }
            }

            sb.AppendLine("--- Execute removal of loop tuples in a single transaction:\r\n");

            sb.AppendLine("    BEGIN;\r\n");

            sb.AppendLine("    SET CONSTRAINTS ALL DEFERRED;\r\n");

            foreach (Table t in _Tables)
            {
                sb.AppendLine("    DELETE FROM " + PostgreSqlGrammar.IdToString(t.IdSchema) + "." + PostgreSqlGrammar.IdToString(t.Id) + " t");
                sb.AppendLine("    WHERE EXISTS(SELECT 1 FROM " + RecursiveRemover.GetCollectTableName(SchemaName, t, true) + " r WHERE " + string.Join(" AND ", t.Columns.Where(c => c.PK).Select(c => "r." + PostgreSqlGrammar.IdToString(c.Id) + " = t." + PostgreSqlGrammar.IdToString(c.Id))) + ");\r\n");
            }

            sb.AppendLine("    COMMIT;\r\n");

            sb.AppendLine("--- Restore original definition of loop internal FKs:\r\n");

            foreach (Table t in _Tables)
            {
                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && _Tables.Contains(tri.ParentTable!)))
                {
                    if (tr.OnDelete != "NO ACTION" || !tr.Deferrable)
                    {
                        sb.AppendLine("    ALTER TABLE " + PostgreSqlGrammar.IdToString(tr.ChildTable!.IdSchema) + "." + PostgreSqlGrammar.IdToString(tr.ChildTable!.Id) + " DROP CONSTRAINT " + PostgreSqlGrammar.IdToString(tr.Id) + ";\r\n");
                        sb.AppendLine("    ALTER TABLE " + PostgreSqlGrammar.IdToString(tr.ChildTable!.IdSchema) + "." + PostgreSqlGrammar.IdToString(tr.ChildTable!.Id) + " ADD CONSTRAINT " + PostgreSqlGrammar.IdToString(tr.Id));
                        sb.AppendLine("    " + tr.Definition + ";\r\n");
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Loop (" + string.Join(", ", _Tables.Select(t => t.IdSchema + "." + t.Id)) + ")";
        }
    }
}

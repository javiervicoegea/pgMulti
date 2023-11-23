using PgMulti.DataStructure;
using PgMulti.RecursiveRemover.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover
{
    public class LoopTablesRecursiveRemoverGraphElement : RecursiveRemoverGraphElement
    {
        private List<Table> _Tables { get; }
        private List<TableRelation> _ParentRelations;
        private List<TableRelation> _ChildRelations;

        public LoopTablesRecursiveRemoverGraphElement(List<Table> tables)
        {
            _Tables = tables;
            _ParentRelations = new List<TableRelation>();
            _ChildRelations = new List<TableRelation>();

            foreach (Table t in _Tables)
            {
                foreach (TableRelation tr in t.Relations)
                {
                    if (!ContainsTable(tr.ParentTable!))
                    {
                        _ParentRelations.Add(tr);
                    }
                    else if (!ContainsTable(tr.ChildTable!))
                    {
                        _ChildRelations.Add(tr);
                    }
                }
            }
        }

        public override List<Table> Tables
        {
            get
            {
                return _Tables;
            }
        }

        public override List<TableRelation> ParentRelations
        {
            get
            {
                return _ParentRelations;
            }
        }

        public override List<TableRelation> ChildRelations
        {
            get
            {
                return _ChildRelations;
            }
        }

        public override bool ContainsTable(Table t)
        {
            return _Tables.Contains(t);
        }

        public override void AddTablesToList(List<Table> list)
        {
            list.AddRange(_Tables);
        }

        public override void WriteCollectTuplesSqlCommands(StringBuilder sb)
        {
            sb.AppendLine("-- LOOP FOR TABLES " + string.Join(", ", _Tables.Select(t => t.IdSchema + "." + t.Id)) + ":\r\n");

            foreach (Table t in _Tables)
            {
                sb.AppendLine("--- Table " + t.IdSchema + "." + t.Id + ":\r\n");

                _WriteCreateTableSqlCommand(sb, 4, t);

                string tableColumns = string.Join(",\r\n", t.Columns.Where(c => c.PK).Select(c => "    " + c.Id + " " + c.Type + c.TypeParams + " PRIMARY KEY").ToArray());

                sb.AppendLine("    CREATE TEMPORARY TABLE " + t.IdSchema + "_" + t.Id + "_steptuples");
                sb.AppendLine("    (");
                sb.AppendLine("    " + tableColumns);
                sb.AppendLine("    );\r\n");

                sb.AppendLine("---- Temporary tables for foreign key references to child tables within the loop:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    sb.AppendLine("     CREATE TEMPORARY TABLE " + t.IdSchema + "_" + t.Id + "_steptuples_to_" + tr.ChildTable!.IdSchema + "_" + tr.ChildTable!.Id + "_" + tr.Id);
                    sb.AppendLine("     (");
                    sb.AppendLine("     " + tableColumns);
                    sb.AppendLine("     );\r\n");
                }

                sb.AppendLine("---- Tuples of parent tables external to the loop:\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && !_Tables.Contains(tri.ParentTable!)))
                {
                    Tuple<string, string>[] fkColumnMatch = new Tuple<string, string>[tr.ParentColumns.Length];

                    for (int i = 0; i < tr.ParentColumns.Length; i++)
                    {
                        fkColumnMatch[i] = new Tuple<string, string>(tr.ParentColumns[i], tr.ChildColumns[i]);
                    }

                    sb.AppendLine("----- FK " + t.IdSchema + "." + t.Id + "." + tr.Id + ":\r\n");
                    sb.AppendLine("      INSERT INTO " + t.IdSchema + "_" + t.Id + "_steptuples");
                    sb.AppendLine("      (" + string.Join(",", t.Columns.Where(c => c.PK).Select(c => c.Id)) + ")");
                    sb.AppendLine("      SELECT " + string.Join(",", t.Columns.Where(c => c.PK).Select(c => "t." + c.Id).ToArray()));
                    sb.AppendLine("      FROM " + t!.IdSchema + "." + t!.Id + " t");
                    sb.AppendLine("      WHERE EXISTS");
                    sb.AppendLine("      (");
                    sb.AppendLine("          SELECT 1");
                    sb.AppendLine("          FROM recursiveremover." + tr.ParentTable!.IdSchema + "." + tr.ParentTable!.Id + " r");
                    sb.AppendLine("          WHERE " + string.Join(" AND ", fkColumnMatch.Select(mi => "r." + mi.Item1 + " = t." + mi.Item2)));
                    sb.AppendLine("      )");
                    sb.AppendLine("      ON CONFLICT DO NOTHING;\r\n");
                }

                sb.AppendLine("---- Transfer tuples to fks within the loop:\r\n");

                sb.AppendLine("      INSERT INTO recursiveremover." + t.IdSchema + "_" + t.Id);
                sb.AppendLine("      SELECT *");
                sb.AppendLine("      FROM " + t.IdSchema + "_" + t.Id + "_steptuples;\r\n");

                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                    sb.AppendLine("      INSERT INTO " + t.IdSchema + "_" + t.Id + "_steptuples_to_" + tr.ChildTable!.IdSchema + "_" + tr.ChildTable!.Id + "_" + tr.Id);
                    sb.AppendLine("      SELECT *");
                    sb.AppendLine("      FROM " + t.IdSchema + "_" + t.Id + "_steptuples;\r\n");
                }

                sb.AppendLine("      TRUNCATE TABLE " + t.IdSchema + "_" + t.Id + "_steptuples;\r\n");
            }

            sb.AppendLine("--- Iterate the loop to get all the tuples:\r\n");

            sb.AppendLine("    CREATE FUNCTION recursiveremover.exploreloop() RETURNS VOID AS $$");
            sb.AppendLine("        DECLARE");
            sb.AppendLine("            n INTEGER;");
            sb.AppendLine("            i INTEGER;");
            sb.AppendLine("        BEGIN");
            sb.AppendLine("            LOOP");
            sb.AppendLine("                n := 0;");

            foreach (Table t in _Tables)
            {
                sb.AppendLine("                -- Table " + t.IdSchema + "." + t.Id + ":\r\n");
                
                sb.AppendLine("                --- Parent tables:\r\n");
                foreach (TableRelation tr in t.Relations.Where(tri => tri.ChildTable == t && _Tables.Contains(tri.ParentTable!)))
                {
                }

                sb.AppendLine("                --- Child tables:\r\n");
                foreach (TableRelation tr in t.Relations.Where(tri => tri.ParentTable == t && _Tables.Contains(tri.ChildTable!)))
                {
                }
            }

            sb.AppendLine("                n := n + i;");
            sb.AppendLine("                EXIT WHEN n = 0;");
            sb.AppendLine("            END LOOP;");
            sb.AppendLine("        END;");
            sb.AppendLine("    $$ LANGUAGE plpgsql;");
        }
    }
}

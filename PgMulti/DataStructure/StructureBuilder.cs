using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using System.Data;
using System.Text;

namespace PgMulti.DataStructure
{
    public class StructureBuilder
    {
        public readonly DB _DB;
        protected List<Schema> _Schemas;
        protected Data _Data;

        protected Dictionary<string, Schema> _DictSchemas = new Dictionary<string, Schema>();
        protected Dictionary<Tuple<string, string>, Table> _DictTables = new Dictionary<Tuple<string, string>, Table>();
        protected Dictionary<Tuple<string, string, string>, Column> _DictColumns = new Dictionary<Tuple<string, string, string>, Column>();
        protected List<string> _HiddenSchemas = new List<string>();
        protected Parser _Parser;

        public static StructureBuilder? CreateStructureBuilder(Data d, DB db)
        {
            using (NpgsqlConnection c = db.Connection)
            {
                NpgsqlCommand cmd = c.CreateCommand();

                try
                {
                    c.Open();
                }
                catch (Exception)
                {
                    return null;
                }

                cmd.CommandText = "SHOW server_version";
                int v = int.Parse(((string)cmd.ExecuteScalar()!).Split('.')[0]);

                if (v >= 15)
                {
                    return new StructureBuilder(d, db);
                }
                else
                {
                    return new StructureBuilderOldVersion(d, db);
                }
            }
        }

        protected StructureBuilder(Data d, DB db)
        {
            _DB = db;
            _Schemas = new List<Schema>();
            _Data = d;
            _Parser = new Parser(_Data.PGLanguageData);
        }

        public List<Schema> Schemas
        {
            get
            {
                return _Schemas;
            }
        }

        public virtual void Build()
        {
            using (NpgsqlConnection c = _DB.Connection)
            {

                c.Open();

                ClearSearchPath(c);
                BuildSchemas(c);
                BuildTables(c);
                BuildColumns(c);
                BuildPKs(c);
                BuildFKs(c);
                BuildIndexes(c);
                BuildFunctions(c);
                BuildTriggers(c);
            }
        }

        protected virtual void ClearSearchPath(NpgsqlConnection c)
        {
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SET search_path=''";
            cmd.ExecuteNonQuery();
        }

        protected virtual void BuildSchemas(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT nspname FROM pg_catalog.pg_namespace";
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Schema schema = new Schema(drd, _DB);

                    if (schema.Id.StartsWith("pg_") || schema.Id == "information_schema")
                    {
                        _HiddenSchemas.Add(schema.Id);
                    }
                    else
                    {
                        _DictSchemas[schema.Id] = schema;
                        _Schemas.Add(schema);
                    }
                }
            }
        }

        protected virtual void BuildTables(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT schemaname,tablename FROM pg_catalog.pg_tables WHERE schemaname <> ALL (:hiddenSchemas);";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Table table = new Table(drd);
                    _DictTables[new Tuple<string, string>(table.IdSchema, table.Id)] = table;
                    Schema schema = _DictSchemas[table.IdSchema];
                    schema.Tables.Add(table);
                    table.Schema = schema;
                }
            }
        }

        protected virtual void BuildColumns(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT table_schema,table_name,column_name,ordinal_position,column_default,is_identity::bool is_identity,is_nullable,data_type,character_maximum_length,numeric_precision,numeric_scale FROM information_schema.columns WHERE table_schema <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Column column = new Column(drd);
                    Tuple<string, string> tk = new Tuple<string, string>(column.IdSchema, column.IdTable);
                    if (!_DictTables.ContainsKey(tk)) continue; // if there is no table then it is a view
                    _DictColumns[new Tuple<string, string, string>(column.IdSchema, column.IdTable, column.Id)] = column;
                    Table table = _DictTables[tk];
                    table.Columns.Add(column);
                    column.Table = table;
                }
            }
        }

        protected virtual void BuildPKs(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT kcu.table_schema,kcu.table_name,kcu.column_name FROM information_schema.table_constraints tco INNER JOIN information_schema.key_column_usage kcu ON kcu.constraint_name=tco.constraint_name AND kcu.constraint_schema=tco.constraint_schema AND kcu.constraint_name=tco.constraint_name WHERE tco.constraint_type='PRIMARY KEY' AND kcu.table_schema <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Column columna = _DictColumns[new Tuple<string, string, string>(drd.GetString("table_schema").ToLower(), drd.GetString("table_name").ToLower(), drd.GetString("column_name").ToLower())];
                    columna.PK = true;
                }
            }
        }

        protected virtual void BuildFKs(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT ns1.nspname parent_schema,c1.relname parent_table,ns2.nspname child_schema,c2.relname child_table,confrelid::regclass AS table_name2,conname AS fk,pg_get_constraintdef(cons.oid) def FROM pg_constraint cons INNER JOIN pg_catalog.pg_class AS c1 ON c1.oid=cons.confrelid INNER JOIN pg_catalog.pg_namespace AS ns1 ON c1.relnamespace = ns1.oid INNER JOIN pg_catalog.pg_class AS c2 ON c2.oid=cons.conrelid INNER JOIN pg_catalog.pg_namespace AS ns2 ON c2.relnamespace = ns2.oid WHERE contype = 'f' AND ns1.nspname <> ALL (:hiddenSchemas) AND ns2.nspname <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    TableRelation tr = new TableRelation(drd);
                    tr.ParentTable = _DictTables[new Tuple<string, string>(tr.IdParentSchema, tr.IdParentTable)];
                    tr.ChildTable = _DictTables[new Tuple<string, string>(tr.IdChildSchema, tr.IdChildTable)];
                    tr.ParentTable.Relations.Add(tr);
                    if (tr.ParentTable != tr.ChildTable) tr.ChildTable.Relations.Add(tr);
                }
            }
        }

        protected virtual void BuildIndexes(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "select indexname,schemaname,tablename,indexdef from pg_indexes WHERE schemaname <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    TableIndex ind = new TableIndex(drd, _Parser);
                    ind.Table = _DictTables[new Tuple<string, string>(ind.IdSchema, ind.IdTable)];
                    ind.Table.Indexes.Add(ind);
                }
            }
        }

        protected virtual void BuildFunctions(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT n.nspname,p.proname,pg_catalog.pg_get_function_arguments(p.oid) as arguments,pg_catalog.pg_get_function_result(p.oid) as returns,COALESCE(pg_catalog.pg_get_function_sqlbody(p.oid), p.prosrc) as source_code FROM pg_catalog.pg_proc p LEFT JOIN pg_catalog.pg_namespace n ON n.oid = p.pronamespace WHERE n.nspname <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Function f = new Function(drd);
                    f.Schema = _DictSchemas[f.IdSchema];
                    f.Schema.Functions.Add(f);
                }
            }
        }

        protected virtual void BuildTriggers(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT ns.nspname,c.relname,t.tgname,pg_catalog.pg_get_triggerdef(t.oid,true) triggerdef FROM pg_catalog.pg_trigger t INNER JOIN pg_catalog.pg_class AS c ON c.oid = t.tgrelid INNER JOIN pg_catalog.pg_namespace AS ns ON c.relnamespace = ns.oid WHERE NOT t.tgisinternal AND t.tgenabled != 'D' AND ns.nspname <> ALL (:hiddenSchemas)";
            cmd.Parameters.AddWithValue("hiddenSchemas", _HiddenSchemas);
            using (drd = cmd.ExecuteReader())
            {
                while (drd.Read())
                {
                    Trigger tg;

                    try
                    {
                        tg = new Trigger(drd, _Parser);
                    }
                    catch (Trigger.NotSupportedTriggerSqlDefinition)
                    {
                        continue;
                    }

                    if (!_DictSchemas.ContainsKey(tg.IdSchemaFunction)) continue;

                    Schema sf = _DictSchemas[tg.IdSchemaFunction];
                    Function? f = sf.Functions.FirstOrDefault(fi => fi.Id == tg.IdFunction);
                    if (f == null) throw new Exception($"Function {tg.IdSchemaFunction}.{tg.IdFunction} of trigger {tg.IdSchema}.{tg.Id} not found!");

                    tg.Table = _DictTables[new Tuple<string, string>(tg.IdSchema, tg.IdTable)];
                    tg.Table.Triggers.Add(tg);

                    tg.Function = f;
                    f.Triggers.Add(tg);
                }
            }
        }
    }
}

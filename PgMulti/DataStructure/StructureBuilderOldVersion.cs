using Irony.Parsing;
using Npgsql;
using PgMulti.AppData;
using System.Data;
using System.Text;

namespace PgMulti.DataStructure
{
    internal class StructureBuilderOldVersion : StructureBuilder
    {

        internal StructureBuilderOldVersion(Data d, DB db) : base(d, db) { }

        protected override void BuildFunctions(NpgsqlConnection c)
        {
            NpgsqlDataReader drd;
            NpgsqlCommand cmd = c.CreateCommand();

            cmd.CommandText = "SELECT n.nspname,p.proname,pg_catalog.pg_get_function_arguments(p.oid) as arguments,pg_catalog.pg_get_function_result(p.oid) as returns,p.prosrc as source_code FROM pg_catalog.pg_proc p LEFT JOIN pg_catalog.pg_namespace n ON n.oid = p.pronamespace WHERE n.nspname <> ALL (:hiddenSchemas)";
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
    }
}

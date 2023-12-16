using Irony.Parsing;
using Npgsql;
using PgMulti.SqlSyntax;
using System.Text.RegularExpressions;

namespace PgMulti.DataStructure
{
    public class TableRelation
    {
        private string _IdParentTable;
        private string _IdParentSchema;
        private string _IdChildTable;
        private string _IdChildSchema;
        private Table? _ParentTable;
        private Table? _ChildTable;
        private string _Id;
        private string _Definition;
        private bool _InitiallyDeferred;
        private bool _Deferrable;

        public static Parser CreateParser(LanguageData languageData)
        {
            return new Parser(languageData, languageData.Grammar.SnippetRoots.First(nt => nt.Name == "alterTableAddConstraint"));
        }

        public string IdParentTable { get => _IdParentTable; internal set => _IdParentTable = value; }
        public string IdParentSchema { get => _IdParentSchema; internal set => _IdParentSchema = value; }
        public string IdChildTable { get => _IdChildTable; internal set => _IdChildTable = value; }
        public string IdChildSchema { get => _IdChildSchema; internal set => _IdChildSchema = value; }
        public Table? ParentTable { get => _ParentTable; internal set => _ParentTable = value; }
        public Table? ChildTable { get => _ChildTable; internal set => _ChildTable = value; }
        public string Id { get => _Id; internal set => _Id = value; }
        public string Definition { get => _Definition; internal set => _Definition = value; }
        public bool InitiallyDeferred { get => _InitiallyDeferred; internal set => _InitiallyDeferred = value; }
        public bool Deferrable { get => _Deferrable; internal set => _Deferrable = value; }

        public readonly string[] ParentColumns;
        public readonly string[] ChildColumns;

        public readonly string OnDelete;
        public readonly string OnUpdate;

        internal TableRelation(NpgsqlDataReader drd, Parser parser)
        {
            _IdParentTable = drd.Ref<string>("parent_table")!;
            _IdParentSchema = drd.Ref<string>("parent_schema")!;
            _IdChildTable = drd.Ref<string>("child_table")!;
            _IdChildSchema = drd.Ref<string>("child_schema")!;
            _Id = drd.Ref<string>("fk")!;
            _Definition = drd.Ref<string>("def")!;

            ParseTree parseTree = parser.Parse(_Definition);
            AstNode nAlterTableAddConstraint = AstNode.ProcessParseTree(parseTree);

            ChildColumns = nAlterTableAddConstraint["tableConstraintDef"]!["tableConstraintDefClause"]!["fkTableConstraint"]!["idlistPar"]!["idSimpleList"]!.Children.Where(ni => ni.Name== "id_simple").Select(ni => SqlSyntax.PostgreSqlGrammar.IdFromString(ni.SingleLineText)).ToArray();
            ParentColumns = nAlterTableAddConstraint["tableConstraintDef"]!["tableConstraintDefClause"]!["fkTableConstraint"]!["fkConstraint"]!["idlistPar"]!["idSimpleList"]!.Children.Where(ni => ni.Name == "id_simple").Select(ni => SqlSyntax.PostgreSqlGrammar.IdFromString(ni.SingleLineText)).ToArray();

            OnDelete = "NO ACTION";
            OnUpdate = "NO ACTION";
            _Deferrable = false;
            _InitiallyDeferred = false;
            
            AstNode? nFkTableConstraintOpt = nAlterTableAddConstraint["tableConstraintDef"]!["tableConstraintDefClause"]!["fkTableConstraint"]!["fkConstraint"]!["fkTableConstraintOpt"];
            if (nFkTableConstraintOpt != null)
            {
                AstNode? nOnActionClauseListOpt = nFkTableConstraintOpt["onActionClauseListOpt"];
                if (nOnActionClauseListOpt != null)
                {
                    AstNode? nOnActionClauseListItemDelete = nOnActionClauseListOpt.Children.FirstOrDefault(ni => ni.Name == "onActionClauseListItem" && ni[1].SingleLineText.ToUpperInvariant() == "DELETE");
                    if (nOnActionClauseListItemDelete != null)
                    {
                        OnDelete = nOnActionClauseListItemDelete[2].SingleLineText.ToUpperInvariant();
                    }

                    AstNode? nOnActionClauseListItemUpdate = nOnActionClauseListOpt.Children.FirstOrDefault(ni => ni.Name == "onActionClauseListItem" && ni[1].SingleLineText.ToUpperInvariant() == "UPDATE");
                    if (nOnActionClauseListItemUpdate != null)
                    {
                        OnDelete = nOnActionClauseListItemUpdate[2].SingleLineText.ToUpperInvariant();
                    }
                }

                AstNode? nDeferrable = nFkTableConstraintOpt["deferrable"];
                if (nDeferrable != null && nDeferrable.SingleLineText.ToUpperInvariant() == "DEFERRABLE")
                {
                    _Deferrable = true;
                }

                AstNode? nInitiallyDeferred = nFkTableConstraintOpt["initiallyDeferred"]?["deferred"];
                if (nInitiallyDeferred != null && nInitiallyDeferred.SingleLineText.ToUpperInvariant() == "DEFERRED")
                {
                    _InitiallyDeferred = true;
                }
            }
        }

        public override string ToString()
        {
            return Id + " (" + _ChildTable!.ToString() + " -> " + _ParentTable!.ToString() + ")";
        }
    }
}

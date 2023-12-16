using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.AppData;
using PgMulti.DataStructure;
using PgMulti.SqlSyntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PgMulti.QueryEditor
{
    public class PGAutocompleteEnumerable : IEnumerable<AutocompleteItem>
    {
        private AutocompleteMenu _AutocompleteMenu;
        private FastColoredTextBox _Fctb;
        private MainForm _MainForm;
        private LanguageData _PGLanguageData;
        private LanguageData _PGSimpleLanguageData;
        private PostgreSqlIdParser _IdParser;

        public PGAutocompleteEnumerable(AutocompleteMenu autocompleteMenu, FastColoredTextBox fctb, MainForm mainForm, LanguageData ld, LanguageData sld)
        {
            _AutocompleteMenu = autocompleteMenu;
            _Fctb = fctb;
            _MainForm = mainForm;
            _PGLanguageData = ld;
            _PGSimpleLanguageData = sld;
            _IdParser = new PostgreSqlIdParser(ld);
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            _DBLoaded = false;

            Place currentFragmentStart = _AutocompleteMenu.Fragment.Start;
            Place currentFragmentEnd = _AutocompleteMenu.Fragment.End;

            if (new FastColoredTextBoxNS.Range(_Fctb, new Place(0, currentFragmentStart.iLine), currentFragmentStart).Text.Contains("--")) yield break;

            FastColoredTextBoxNS.Range r = new FastColoredTextBoxNS.Range(_Fctb, currentFragmentStart, currentFragmentStart);
            Place startQuery;
            while (r.Start.iChar > 0 || r.Start.iLine > 0)
            {
                if (r.CharBeforeStart == ';')
                {
                    break;
                }

                r.GoLeft();
            }

            startQuery = r.Start;
            string beforeCurrentSql = new FastColoredTextBoxNS.Range(_Fctb, startQuery, currentFragmentEnd).Text;

            r = new FastColoredTextBoxNS.Range(_Fctb, currentFragmentEnd, currentFragmentEnd);
            Place endQuery;
            while (r.Start.iLine < _Fctb.LinesCount - 1 || r.Start.iChar < _Fctb[r.Start.iLine].Count)
            {
                if (r.CharAfterStart == ';')
                {
                    break;
                }

                r.GoRight();
            }
            endQuery = r.End;
            string afterCurrentSql = new FastColoredTextBoxNS.Range(_Fctb, currentFragmentEnd, endQuery).Text;

            r = new FastColoredTextBoxNS.Range(_Fctb, currentFragmentStart, currentFragmentStart);
            Place endLastWordQuery;
            while (r.Start.iChar > startQuery.iChar || r.Start.iLine > startQuery.iLine)
            {
                if (!Regex.Match(r.CharBeforeStart.ToString(), @"[\w\""]").Success)
                {
                    break;
                }

                r.GoLeft();
            }
            endLastWordQuery = r.Start;
            string lastCompletedWordsBeforeCurrentSql = new FastColoredTextBoxNS.Range(_Fctb, startQuery, endLastWordQuery).Text;

            string currentFragment = _AutocompleteMenu.Fragment.Text;

            int insertedChars = 0;
            if (currentFragment == "" || currentFragment.EndsWith("."))
            {
                beforeCurrentSql += "x";
                insertedChars = 1;
            }

            string currentSql = beforeCurrentSql + afterCurrentSql;

            if (DB == null && currentFragment.Length > 0)
            {
                yield return new AutocompleteItemTip(currentFragment, Properties.Text.tip_title_no_selected_db, 15, Properties.Text.tip, Properties.Text.tip_content_no_selected_db);
            }

            Parser parser = new Parser(_PGLanguageData);

            bool anyId = false;

            if (DB != null)
            {
                using (IEnumerator<AutocompleteItem> ie = _GetEnumeratorStructure(parser, startQuery, currentFragmentStart, insertedChars, currentSql, beforeCurrentSql, afterCurrentSql, currentFragment))
                {
                    while (ie.MoveNext())
                    {
                        yield return ie.Current;
                        anyId = true;
                    }
                }

                parser = new Parser(_PGLanguageData);
            }

            using (IEnumerator<AutocompleteItem> ie = _GetEnumeratorKeywords(parser, lastCompletedWordsBeforeCurrentSql, currentFragment, anyId))
                while (ie.MoveNext())
                    yield return ie.Current;
        }

        private IEnumerator<AutocompleteItem> _GetEnumeratorStructure(Parser parser, Place posicionInicial, Place currentPlace, int insertedChars, string currentSql, string beforeCurrentSql, string afterCurrentSql, string currentFragment)
        {
            if (
                    (
                        Regex.Match(beforeCurrentSql, @"^\s*GRANT\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        || Regex.Match(beforeCurrentSql, @"^\s*REVOKE\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success

                    )
                    && Regex.Match(beforeCurrentSql, @"\s+SCHEMA\s+[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                )
            {
                // List schemas

                foreach (Schema schema in DB!.Schemas.OrderBy(e => e.Id))
                {
                    yield return new AutocompleteItemId(null, schema.Id, 0, _AutocompleteMenu.PreselectedFont, _IdParser);
                }

                yield break;
            }

            if (

                        (
                            Regex.Match(beforeCurrentSql, @"^\s*GRANT\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                            || Regex.Match(beforeCurrentSql, @"^\s*REVOKE\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success

                        )
                        && Regex.Match(beforeCurrentSql, @"\s+ON\s+(TABLE\s+)?[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success

                    || Regex.Match(beforeCurrentSql, @"^\s*ALTER\s+TABLE\s+[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    || Regex.Match(beforeCurrentSql, @"^\s*(ALTER|CREATE)\s+TABLE\s+.*REFERENCES\s+[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                )
            {
                // List tables

                using (IEnumerator<AutocompleteItem> ie = _GetEnumeratorAllTables(currentFragment))
                {
                    while (ie.MoveNext())
                    {
                        yield return ie.Current;
                    }
                }

                yield break;
            }

            string? tableColumns = null;
            List<string>? alreadyColumns = null;

            Match m = Regex.Match(beforeCurrentSql, @"^\s*ALTER\s+TABLE\s+([\w\""\.]+)\s+(ALTER|DROP)\s+COLUMN\s+[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (m.Success)
            {
                tableColumns = m.Groups[1].Value;
            }
            else
            {
                m = Regex.Match(beforeCurrentSql, @"^\s*(ALTER|CREATE)\s+TABLE\s+([\w\""\.]+)(\s|\()", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (m.Success)
                {
                    tableColumns = m.Groups[2].Value;

                    m = Regex.Match(beforeCurrentSql, @"\s+FOREIGN\s+KEY\s*\((([\w\""]+\s*\,\s*)*)[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (m.Success)
                    {
                        alreadyColumns = m.Groups[1].Value.Split(',').Select(s => s.Trim()).ToList();
                    }
                    else
                    {
                        m = Regex.Match(beforeCurrentSql, @"\s+REFERENCES\s+([\w\""\.]+)\s*\((([\w\""]+\s*\,\s*)*)[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        if (m.Success)
                        {
                            tableColumns = m.Groups[1].Value;
                            alreadyColumns = m.Groups[2].Value.Split(',').Select(s => s.Trim()).ToList();
                        }
                    }
                }
            }

            if (m.Success)
            {
                // List columns

                Table? table = _GetTableFromFqId(tableColumns!);
                if (table == null) yield break;

                foreach (DataStructure.Column column in table.Columns.OrderBy(c => !c.PK).ThenBy<DataStructure.Column, string>(c => c.Id))
                {
                    if (alreadyColumns != null && alreadyColumns.Contains(column.Id)) continue;
                    yield return new AutocompleteItemId(null, column.Id, _GetImageIndexForColumn(column), _AutocompleteMenu.PreselectedFont, _IdParser, column.Info);
                }

                yield break;
            }

            ParseTree parseTree = parser.Parse(currentSql + "\r\n;");


            if (parseTree.Status == ParseTreeStatus.Error)
            {
                // Correct parenthesis
                Parser sp = new Parser(_PGSimpleLanguageData);
                ParseTree spt = sp.Parse(currentSql + "\r\n;");

                if (spt.Status == ParseTreeStatus.Parsed)
                {
                    int nparentesis = 0;
                    foreach (Token t in spt.Tokens)
                    {
                        if (t.Text == "(")
                        {
                            nparentesis++;
                        }
                        else if (t.Text == ")")
                        {
                            nparentesis--;
                        }
                    }
                    if (nparentesis != 0)
                    {
                        for (int i = 0; i < nparentesis; i++)
                        {
                            afterCurrentSql = ")" + afterCurrentSql;
                        }

                        insertedChars += nparentesis;
                        currentSql = beforeCurrentSql + afterCurrentSql;

                        parseTree = parser.Parse(currentSql + "\r\n;");
                    }
                }
            }

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                // Diferent corrections according to statement type and writting position

                if (
                        Regex.Match(beforeCurrentSql, @"JOIN\s+[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^\s+ON\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = " ON x" + afterCurrentSql;
                    insertedChars += 5;

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"UPDATE\s+[\w\""]+(\s*\.\s*[\w\""]+)?$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^\s+SET\s+[\w\""]+\s*\=", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = " SET x=x" + afterCurrentSql;
                    insertedChars += 8;

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"UPDATE\s+[\w\""]+(\s*\.\s*[\w\""]+)?\s+([\w\""]+\s+)?SET\s+.+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        &&
                        (
                            Regex.Match(beforeCurrentSql, @"SET\s+[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                            || Regex.Match(beforeCurrentSql, @"\,\s*[\w\""]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        )
                        && !Regex.Match(afterCurrentSql, @"^\s*\=", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = "=x" + afterCurrentSql;
                    insertedChars += 2;

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"INSERT\s+INTO\s+[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^\s*\([\w\""]+(\s*\,\s*[\w\""]+)*\)\s*VALUES\s*\(", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = " (x) VALUES (x)" + afterCurrentSql;
                    insertedChars += 15;

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"INSERT\s+INTO\s+[\w\""\.]+\s*\([\w\""]+(\s*\,\s*[\w\""]+)*$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^(\s*\,\s*[\w\""]+)*\)\s*VALUES\s*\(", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = ") VALUES (x" + afterCurrentSql;
                    insertedChars += 11;

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"WITH\s+[\w\""\.]+\s+AS\s*\(", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && Regex.Match(afterCurrentSql, @"\)$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql += "\r\nSELECT";

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"^\s*CREATE\s+(UNIQUE\s+)?INDEX\s+(CONCURRENTLY\s+)?((IF\s+NOT\s+EXISTS\s+)?[\w\""])?\s+ON\s+(ONLY\s+)?[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^\s*\(", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = "(x)";

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
                else if (
                        Regex.Match(beforeCurrentSql, @"^\s*CREATE\s+(.*\s+)?TRIGGER\s+.*\s+ON\s+[\w\""\.]+$", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                        && !Regex.Match(afterCurrentSql, @"^\s+FOR\s+.*\s+EXECUTE", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success
                    )
                {
                    afterCurrentSql = " FOR EACH ROW EXECUTE FUNCTION x()";

                    currentSql = beforeCurrentSql + afterCurrentSql;

                    parseTree = parser.Parse(currentSql + ";");
                }
            }

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                // Truncate text as last try

                afterCurrentSql = "";

                currentSql = beforeCurrentSql + afterCurrentSql;

                parseTree = parser.Parse(currentSql + ";");
            }

            if (parseTree.Status == ParseTreeStatus.Error)
            {
                yield break;
            }

            AstNode rootAst = AstNode.ProcessParseTree(parseTree);

            if (rootAst.RecursiveTokens.Count == 0) yield break;

            rootAst.RevisePosition(posicionInicial.iLine, posicionInicial.iChar, currentPlace.iLine, currentPlace.iChar, insertedChars);

            AstNode? currentNode = null;
            foreach (AstNode nToken in rootAst.RecursiveTokens)
            {
                if (nToken.ContainsPosition(currentPlace.iLine, currentPlace.iChar))
                {
                    currentNode = nToken;
                    break;
                }
            }

            if (currentNode == null) yield break;

            if (currentNode.Parent!.Name == "id" && currentNode.Parent.Parent!.Name == "term")
            {
                // Autocomplete Id in expression (where, select, on, etc)

                string? currentFragmentAlias = null;

                int dotPosition = currentFragment.IndexOf('.');
                if (dotPosition != -1)
                {
                    currentFragmentAlias = _IdParser.Sql2CleanDefinition(currentFragment.Substring(0, dotPosition));

                    if (currentFragment.IndexOf('.', dotPosition + 1) != -1) yield break;
                }

                Dictionary<string, Tuple<string, bool>> tables = _ListFromClauseTables(currentNode, true, false);

                AstNode? nStmt = currentNode.GetRecursiveParentNamedAs("stmt");
                if (nStmt == null) nStmt = currentNode.GetRecursiveParentNamedAs("sqlStmt");
                if (nStmt != null) nStmt = nStmt.Children[0];

                if (currentFragmentAlias == null && (nStmt == null || nStmt.Name != "createIndexStmt"))
                {
                    foreach (string alias in tables.Keys.OrderBy(k => k))
                    {
                        yield return new AutocompleteItemId(null, alias, 1, _AutocompleteMenu.PreselectedFont, _IdParser);
                    }
                }

                List<string> tablesWhereSearchForColumns = new List<string>();

                if (currentFragmentAlias == null)
                {
                    tablesWhereSearchForColumns.AddRange(tables.Values.Select(v => v.Item1).Distinct());
                }
                else if (tables.ContainsKey(currentFragmentAlias))
                {
                    tablesWhereSearchForColumns.Add(tables[currentFragmentAlias].Item1);
                }
                else
                {
                    yield break;
                }

                if (DB == null)
                {
                    yield break;
                }

                foreach (string tablaFqId in tablesWhereSearchForColumns)
                {
                    Table? table = _GetTableFromFqId(tablaFqId);
                    if (table == null) continue;

                    foreach (DataStructure.Column column in table.Columns.OrderBy(c => !c.PK).ThenBy<DataStructure.Column, string>(c => c.Id))
                    {
                        yield return new AutocompleteItemId(currentFragmentAlias, column.Id, _GetImageIndexForColumn(column), _AutocompleteMenu.PreselectedFont, _IdParser, column.Info);
                    }
                }
            }
            else if (currentNode.Name == "id_simple" && currentNode.Parent.Name == "assignment")
            {
                // Autocomplete Id to assign in UPDATE's SET clause

                string tableFqId = currentNode.Parent!.Parent!.Parent![currentNode.Parent!.Parent!.Parent!["UPDATE"]!.Index + 1].SingleLineText;

                Table? table = _GetTableFromFqId(tableFqId);
                if (table == null) yield break;

                foreach (DataStructure.Column column in table.Columns.OrderBy(c => !c.PK).ThenBy<DataStructure.Column, string>(c => c.Id))
                {
                    bool alreadyAssigned = false;
                    foreach (AstNode n in currentNode.Parent.Parent.Children)
                    {
                        if (n == currentNode.Parent || n.Name != "assignment") continue;
                        if (n.Children[0].Token!.Text == column.Id)
                        {
                            alreadyAssigned = true;
                            break;
                        }
                    }
                    if (alreadyAssigned) continue;

                    yield return new AutocompleteItemId(null, column.Id, _GetImageIndexForColumn(column), _AutocompleteMenu.PreselectedFont, _IdParser, column.Info);
                }
            }
            else if (currentNode.Name == "id_simple" && currentNode.Parent.Parent!.Parent!.Parent!.Name == "insertStmt")
            {
                // Autocomplete Id to assign in INSERT

                string tableFqId = currentNode.Parent.Parent.Parent.Parent[currentNode.Parent.Parent.Parent.Parent["INTO"]!.Index + 1].SingleLineText;

                Table? table = _GetTableFromFqId(tableFqId);
                if (table == null) yield break;

                foreach (DataStructure.Column column in table.Columns.OrderBy(c => !c.PK).ThenBy<DataStructure.Column, string>(c => c.Id))
                {
                    bool alreadyAssigned = false;
                    foreach (AstNode n in currentNode.Parent.Children)
                    {
                        if (n == currentNode || n.Name != "id_simple") continue;
                        if (n.Token!.Text == column.Id)
                        {
                            alreadyAssigned = true;
                            break;
                        }
                    }
                    if (alreadyAssigned) continue;

                    yield return new AutocompleteItemId(null, column.Id, _GetImageIndexForColumn(column), _AutocompleteMenu.PreselectedFont, _IdParser, column.Info);
                }
            }
            else if (currentNode.Parent.Name == "id" && (currentNode.Parent.Parent!.Name == "tableId" || (currentNode.Parent.Parent.Parent != null && currentNode.Parent.Parent.Parent.Name == "fromItem")))
            {
                // Autocomplete Ids in FROM / USING or main table in statements like UPDATE, INSERT, DELETE, etc

                if (DB == null)
                {
                    yield break;
                }

                if (currentNode.Parent.Parent.Parent != null && currentNode.Parent.Parent.Parent.Name == "fromItem")
                {
                    bool crossJoinOrFirstTable = currentNode.Parent.Parent.Parent.Index == 0 || currentNode.Parent.Parent.Parent.Parent![currentNode.Parent.Parent.Parent.Index - 1].Name == ",";

                    Dictionary<string, Tuple<string, bool>> sameLevelTables = _ListFromClauseTables(currentNode, false, !crossJoinOrFirstTable);

                    if (sameLevelTables.Count > 1)
                    {
                        // Related tables only

                        Dictionary<string, Tuple<string, bool>> allTables = _ListFromClauseTables(currentNode, true, false);

                        foreach (KeyValuePair<string, Tuple<string, bool>> kvpTabla in sameLevelTables)
                        {
                            Table? table = _GetTableFromFqId(kvpTabla.Value.Item1);
                            if (table == null) continue;

                            foreach (TableRelation tr in table.Relations)
                            {
                                Table relatedTable;
                                string[] curTableColumns;
                                string[] relatedTableColumns;
                                bool n1;

                                if (tr.ParentTable == table)
                                {
                                    relatedTable = tr.ChildTable!;
                                    curTableColumns = tr.ParentColumns;
                                    relatedTableColumns = tr.ChildColumns;
                                    n1 = false;
                                }
                                else
                                {
                                    relatedTable = tr.ParentTable!;
                                    curTableColumns = tr.ChildColumns;
                                    relatedTableColumns = tr.ParentColumns;
                                    n1 = true;
                                }

                                string aliasRel = _CreateTableAlias(allTables.Keys, relatedTable.Id);
                                string condRel = PostgreSqlGrammar.IdToString(kvpTabla.Key) + "." + PostgreSqlGrammar.IdToString(curTableColumns[0]) + "=" + PostgreSqlGrammar.IdToString(aliasRel) + "." + PostgreSqlGrammar.IdToString(relatedTableColumns[0]);

                                for (int i = 1; i < curTableColumns.Length; i++)
                                {
                                    condRel += " AND " + PostgreSqlGrammar.IdToString(kvpTabla.Key) + "." + PostgreSqlGrammar.IdToString(curTableColumns[i])
                                        + "=" + PostgreSqlGrammar.IdToString(aliasRel) + "." + PostgreSqlGrammar.IdToString(relatedTableColumns[i]);
                                }

                                string fqTablaRel = relatedTable.IdSchema + "." + relatedTable.Id;
                                string fromText = PostgreSqlGrammar.IdToString(relatedTable.IdSchema) + "." + PostgreSqlGrammar.IdToString(relatedTable.Id) + " " + aliasRel;

                                yield return new AutocompleteItemRelation(currentNode, kvpTabla.Key, fqTablaRel, fromText, condRel, crossJoinOrFirstTable || kvpTabla.Value.Item2, n1, _IdParser);
                            }
                        }
                    }
                }

                using (IEnumerator<AutocompleteItem> ie = _GetEnumeratorAllTables(currentFragment))
                    while (ie.MoveNext())
                        yield return ie.Current;
            }
        }

        public IEnumerator<AutocompleteItem> _GetEnumeratorAllTables(string currentFragment)
        {
            // Any table
            string? currentFragmentSchema = null;

            int posPunto = currentFragment.IndexOf('.');
            if (posPunto != -1)
            {
                currentFragmentSchema = currentFragment.Substring(0, posPunto);

                if (currentFragment.IndexOf('.', posPunto + 1) != -1) yield break;
            }

            List<Schema> currentFragmentSearchPath;

            if (currentFragmentSchema == null)
            {
                // List schemas

                foreach (Schema esquema in DB!.Schemas.OrderBy(e => e.Id))
                {
                    yield return new AutocompleteItemId(null, esquema.Id, 0, _AutocompleteMenu.PreselectedFont, _IdParser);
                }
                currentFragmentSearchPath = DB!.SearchPathSchemas;
            }
            else
            {
                currentFragmentSearchPath = new List<Schema>();
                Schema? currentSchema = DB!.Schemas.FirstOrDefault(ei => ei.Id == PostgreSqlGrammar.IdFromString(currentFragmentSchema));
                if (currentSchema != null) currentFragmentSearchPath.Add(currentSchema);
            }

            foreach (Schema s in currentFragmentSearchPath.OrderBy(e => e.Id))
            {
                foreach (Table table in s.Tables.OrderBy(t => t.Id))
                {
                    yield return new AutocompleteItemId(s.Id, table.Id, 1, _AutocompleteMenu.PreselectedFont, _IdParser);
                }
            }
        }

        private string _CreateTableAlias(IEnumerable<string> prevAlias, string tableName)
        {
            string alias = tableName.Substring(0, 2);
            if (prevAlias.Contains(alias) || _PGLanguageData.Grammar.KeyTerms.ContainsKey(alias))
            {
                int i = 2;
                while (prevAlias.Contains(alias + i))
                {
                    i++;
                }
                alias = alias + i;
            }

            return alias;
        }

        private DB? _DB = null;
        private bool _DBLoaded = false;
        private DB? DB
        {
            get
            {
                if (!_DBLoaded)
                {
                    List<DB> dbs = _MainForm.SelectedDBs;
                    if (dbs.Count > 0)
                    {
                        _DB = dbs[0];
                    }
                    else
                    {
                        _DB = null;
                    }
                    _DBLoaded = true;
                }

                return _DB;
            }
        }

        private Table? _GetTableFromFqId(string tableFqId)
        {
            int dotPosition = tableFqId.IndexOf('.');
            if (dotPosition == -1)
            {
                string tableId;
                tableId = tableFqId;

                List<Table> tables = new List<Table>();
                foreach (Schema s in DB!.SearchPathSchemas)
                {
                    tables.AddRange(s.Tables.Where(ti => ti.Id == tableId));
                }

                if (tables.Count == 1)
                {
                    return tables[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string schemaId;
                schemaId = tableFqId.Substring(0, dotPosition);
                string tableId;
                tableId = tableFqId.Substring(dotPosition + 1);

                Schema? schema = DB!.Schemas.FirstOrDefault(ei => ei.Id == schemaId);
                if (schema == null) return null;

                return schema.Tables.FirstOrDefault(ti => ti.Id == tableId);
            }
        }

        private Dictionary<string, Tuple<string, bool>> _ListFromClauseTables(AstNode currentNode, bool upwards, bool excludeMain)
        {
            Dictionary<string, Tuple<string, bool>> tables = new Dictionary<string, Tuple<string, bool>>();


            // Search for tables

            List<AstNode> fromItems = new List<AstNode>();
            AstNode? n = currentNode;

            while (n != null)
            {
                while (n.Parent != null)
                {
                    if (n.Name == "selectBaseClauses" || n.Parent.Name == "stmt" || n.Parent.Name == "sqlStmt")
                    {
                        break;
                    }
                    n = n.Parent;
                }

                if (n.Parent == null) break;

                if (!excludeMain)
                {
                    switch (n.Name)
                    {
                        case "deleteStmt":
                        case "updateStmt":
                            _ProcessTableAlias(n["tableId"]!["id"], n["aliasOpt"], tables, true);
                            break;
                        case "createIndexStmt":
                            _ProcessTableAlias(n["tableId"]!["id"], null, tables, true);
                            break;
                    }
                }

                AstNode? fromClauseOpt = null;

                if (n.Name == "selectStmt")
                {
                    if (n["selectBody"]!["selectBaseClauses"] != null)
                    {
                        fromClauseOpt = n["selectBody"]!["selectBaseClauses"]!["fromClauseOpt"];
                    }
                }
                else
                {
                    fromClauseOpt = n["fromClauseOpt"];
                    if (fromClauseOpt == null) fromClauseOpt = n["usingClauseOpt"];
                }

                if (fromClauseOpt != null)
                {
                    AstNode fromItemList = fromClauseOpt["fromItemList"]!;
                    fromItems.Add(fromItemList["fromItem"]!);
                    AstNode? joinChainOpt = fromItemList["joinChainOpt"];
                    if (joinChainOpt != null)
                    {
                        foreach (AstNode joinChainOptItem in joinChainOpt.Children)
                        {
                            fromItems.Add(joinChainOptItem["fromItem"]!);
                        }
                    }
                }

                if (!upwards) break;

                n = n.Parent;
            }

            foreach (AstNode fromItem in fromItems)
            {
                _ProcessTableAlias(fromItem[0]["id"], fromItem["aliasOpt"], tables, false);
            }

            return tables;
        }

        private void _ProcessTableAlias(AstNode? nodeId, AstNode? nodeAliasOpt, Dictionary<string, Tuple<string, bool>> tables, bool main)
        {
            if (nodeId == null) return;
            PostgreSqlId? id = _IdParser.TryParse(nodeId);
            if (id == null) return;

            string table = id.ToString();
            string alias;

            if (nodeAliasOpt == null)
            {
                alias = id.Values.Last();
            }
            else
            {
                alias = PostgreSqlGrammar.IdFromString(nodeAliasOpt["id_simple"]!.SingleLineText);
            }

            tables[alias] = new Tuple<string, bool>(table, main);
        }

        private IEnumerator<AutocompleteItem> _GetEnumeratorKeywords(Parser parser, string lastCompletedWordsBeforeCurrentSql, string currentFragment, bool anyId)
        {
            if (!_AutocompleteMenu.Forcing && currentFragment.Length == 0) yield break;

            parser.Parse(lastCompletedWordsBeforeCurrentSql);
            if (parser.NextAcceptableTerminals == null) yield break;
            List<string> l = new List<string>(parser.NextAcceptableTerminals);

            if (l.Contains("boolLit"))
            {
                l.Remove("boolLit");
                l.Add("true");
                l.Add("false");
            }

            if (!anyId && currentFragment.Length > 0 && l.Count > 1 && currentFragment.Length < 3 && l.Contains("id_simple") && DB != null && !l.Contains(currentFragment.ToUpper()))
            {
                yield return new AutocompleteItemTip(currentFragment, currentFragment, 16);
            }

            foreach (string s in l.OrderBy(si => si.ToLower()))
            {
                switch (s)
                {
                    case "id_simple":
                    case "string":
                    case "escaped_string":
                    case "number":
                    case "(":
                    case ")":
                        continue;
                    case "dollar_string_tag":
                        yield return new AutocompleteItemCustom("$$", 13, "$$", null, null, _AutocompleteMenu.Font);
                        break;
                    default:
                        yield return new AutocompleteItemCustom(s, 13, s, null, null, _AutocompleteMenu.Font);
                        break;
                }
            }
        }

        private int _GetImageIndexForColumn(DataStructure.Column c)
        {
            if (c.PK)
            {
                return 14;
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
                        return 8;
                    case "money":
                        return 9;
                    case "decimal":
                    case "real":
                    case "float":
                    case "numeric":
                    case "double precision":
                        return 10;
                    case "bit":
                    case "bit varying":
                    case "bytea":
                    case "varbit":
                        return 4;
                    case "boolean":
                    case "bool":
                        return 5;
                    case "date":
                    case "datetime":
                    case "timestamp":
                    case "timestamp with time zone":
                    case "timestamp without time zone":
                        return 6;
                    case "time":
                    case "time with time zone":
                    case "time without time zone":
                        return 12;
                    case "text":
                    case "character":
                    case "character varying":
                    case "char":
                    case "varchar":
                        return 11;
                    default:
                        return 7;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

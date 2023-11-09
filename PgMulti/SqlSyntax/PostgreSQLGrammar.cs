using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using FastColoredTextBoxNS;
using Irony.Parsing;
using Microsoft.VisualBasic.ApplicationServices;
using PgMulti.DataAccess;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PgMulti.SqlSyntax
{
    [Language("PostgreSQL", "15", "PostgreSQL grammar")]
    public class PostgreSQLGrammar : Grammar
    {
        public PostgreSQLGrammar(CultureInfo cu) : base(false)
        {
            DefaultCulture = cu;

            //SQL is case insensitive
            //Terminals
            var comment = new CommentTerminal("comment", "/*", "*/");
            var lineComment = new CommentTerminal("line_comment", "--", "\n", "\r\n");
            NonGrammarTerminals.Add(comment);
            NonGrammarTerminals.Add(lineComment);

            var NULL = new ConstantTerminal("NULL");
            NULL.Add("NULL", null);

            var boolLit = new ConstantTerminal("boolLit");
            boolLit.Add("true", true);
            boolLit.Add("false", false);

            var number = new NumberLiteral("number");
            number.DefaultIntTypes = new TypeCode[] { TypeCode.Int32, TypeCode.Int64 };

            var string_literal = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote | StringOptions.NoEscapes | StringOptions.AllowsLineBreak);
            var escaped_string_literal = new StringLiteral("escaped_string", "E'", "'", StringOptions.AllowsDoubledQuote | StringOptions.AllowsAllEscapes | StringOptions.AllowsLineBreak);
            var dollar_string_tag = new StringLiteral("dollar_string_tag", "$");
            var id_simple = CreateIdentifier("id_simple");
            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var semi = ToTerm(";");
            var CREATE = ToTerm("CREATE");
            var NOT = ToTerm("NOT");
            var IN = ToTerm("IN");
            var UNIQUE = ToTerm("UNIQUE");
            var WITH = ToTerm("WITH");
            var TABLE = ToTerm("TABLE");
            var ALTER = ToTerm("ALTER");
            var ADD = ToTerm("ADD");
            var COLUMN = ToTerm("COLUMN");
            var DROP = ToTerm("DROP");
            var CONSTRAINT = ToTerm("CONSTRAINT");
            var INDEX = ToTerm("INDEX");
            var ON = ToTerm("ON");
            var KEY = ToTerm("KEY");
            var PRIMARY = ToTerm("PRIMARY");
            var INSERT = ToTerm("INSERT");
            var INTO = ToTerm("INTO");
            var UPDATE = ToTerm("UPDATE");
            var SET = ToTerm("SET");
            var VALUES = ToTerm("VALUES");
            var DELETE = ToTerm("DELETE");
            var SELECT = ToTerm("SELECT");
            var FROM = ToTerm("FROM");
            var AS = ToTerm("AS");
            var COUNT = ToTerm("COUNT");
            var JOIN = ToTerm("JOIN");
            var BY = ToTerm("BY");
            var USING = ToTerm("USING");
            var TO = ToTerm("TO");
            var DEFAULT = ToTerm("DEFAULT");
            var FULL = ToTerm("FULL");
            var TRUNCATE = ToTerm("TRUNCATE");
            var REFERENCES = ToTerm("REFERENCES");
            var DISTINCT = ToTerm("DISTINCT");
            var SEQUENCE = ToTerm("SEQUENCE");
            var FUNCTION = ToTerm("FUNCTION");
            var IF = ToTerm("IF");
            var END = ToTerm("END");
            var WHEN = ToTerm("WHEN");

            //Non-terminals
            var id = new NonTerminal("id");
            var stmt = new NonTerminal("stmt");
            var createTableStmt = new NonTerminal("createTableStmt");
            var createTableTablespaceClauseOpt = new NonTerminal("createTableTablespaceClauseOpt");
            var createTableWithClauseOpt = new NonTerminal("createTableWithClauseOpt");
            var createTableWithList = new NonTerminal("createTableWithList");
            var createTableWithItem = new NonTerminal("createTableWithItem");
            var createIndexStmt = new NonTerminal("createIndexStmt");
            var createRoleStmt = new NonTerminal("createRoleStmt");
            var createRoleOptions = new NonTerminal("createRoleOptions");
            var alterRoleOptions = new NonTerminal("alterRoleOptions");
            var alterRoleOption = new NonTerminal("alterRoleOption");
            var createRoleOption = new NonTerminal("createRoleOption");
            var alterStmt = new NonTerminal("alterStmt");
            var dropTableStmt = new NonTerminal("dropTableStmt");
            var dropIndexStmt = new NonTerminal("dropIndexStmt");
            var selectStmt = new NonTerminal("selectStmt");
            var selectBody = new NonTerminal("selectBody");
            var selectBaseClauses = new NonTerminal("selectBaseClauses");
            var selectCombineClauseOpt = new NonTerminal("selectCombineClauseOpt");
            var insertStmt = new NonTerminal("insertStmt");
            var insertReturningClauseOpt = new NonTerminal("insertReturningClauseOpt");
            var insertOnConflictClauseOpt = new NonTerminal("insertOnConflictClauseOpt");
            var updateStmt = new NonTerminal("updateStmt");
            var deleteStmt = new NonTerminal("deleteStmt");
            var fieldDef = new NonTerminal("fieldDef");
            var createTableDefList = new NonTerminal("createTableDefList");
            var nullColumnConstraint = new NonTerminal("nullColumnConstraint");
            var fkColumnConstraint = new NonTerminal("fkColumnConstraint");
            var typeName = new NonTerminal("typeName");
            var typeSpec = new NonTerminal("typeSpec");
            var typeParamsOpt = new NonTerminal("typeParamsOpt");
            var constraintDef = new NonTerminal("constraintDef");
            var idSimpleList = new NonTerminal("idSimpleList");
            var idlistPar = new NonTerminal("idlistPar");
            var uniqueOpt = new NonTerminal("uniqueOpt");
            var orderList = new NonTerminal("orderList");
            var orderMember = new NonTerminal("orderMember");
            var orderDirOpt = new NonTerminal("orderDirOpt");
            var withClauseOpt = new NonTerminal("withClauseOpt");
            var alterTable = new NonTerminal("alterTable");
            var insertData = new NonTerminal("insertData");
            var assignList = new NonTerminal("assignList");
            var whereClauseOpt = new NonTerminal("whereClauseOpt");
            var assignment = new NonTerminal("assignment");
            var expression = new NonTerminal("expression");
            var exprList = new NonTerminal("exprList");
            var selRestrOpt = new NonTerminal("selRestrOpt");
            var selList = new NonTerminal("selList");
            var intoClauseOpt = new NonTerminal("intoClauseOpt");
            var fromClauseOpt = new NonTerminal("fromClauseOpt");
            var groupClauseOpt = new NonTerminal("groupClauseOpt");
            var havingClauseOpt = new NonTerminal("havingClauseOpt");
            var orderClauseOpt = new NonTerminal("orderClauseOpt");
            var selItem = new NonTerminal("selItem");
            var asOpt = new NonTerminal("asOpt");
            var aliasOpt = new NonTerminal("aliasOpt");
            var tuple = new NonTerminal("tuple");
            var joinChainOpt = new NonTerminal("joinChainOpt");
            var joinKindOpt = new NonTerminal("joinKindOpt");
            var term = new NonTerminal("term");
            var unExpr = new NonTerminal("unExpr");
            var unOp = new NonTerminal("unOp");
            var binExpr = new NonTerminal("binExpr");
            var binOp = new NonTerminal("binOp");
            var betweenExpr = new NonTerminal("betweenExpr");
            var inExpr = new NonTerminal("inExpr");
            var parSelectStmtExpr = new NonTerminal("parSelectStmtExpr");
            var notOpt = new NonTerminal("notOpt");
            var funCall = new NonTerminal("funCall");
            var stmtListOpt = new NonTerminal("stmtListOpt");
            var funArgs = new NonTerminal("funArgs");
            var dropSequenceStmt = new NonTerminal("dropSequenceStmt");
            var dropFunctionStmt = new NonTerminal("dropFunctionStmt");

            var cteClauseOpt = new NonTerminal("cteClauseOpt");
            var cteClauseList = new NonTerminal("cteClauseList");
            var fromItemList = new NonTerminal("fromItemList");
            var fromItem = new NonTerminal("fromItem");
            var setStmt = new NonTerminal("setStmt");
            var truncateStmt = new NonTerminal("truncateStmt");
            var limitClauseOpt = new NonTerminal("limitClauseOpt");
            var offsetClauseOpt = new NonTerminal("offsetClauseOpt");
            var onActionClauseListOpt = new NonTerminal("onActionClauseListOpt");
            var onActionClauseListItem = new NonTerminal("onActionClauseListItem");
            var tablespaceClauseOpt = new NonTerminal("tablespaceClauseOpt");
            var grantStmt = new NonTerminal("grantStmt");
            var grantObjectTable = new NonTerminal("grantObjectTable");
            var grantObjectSchema = new NonTerminal("grantObjectSchema");
            var createTriggerStmt = new NonTerminal("createTriggerStmt");
            var createSequenceStmt = new NonTerminal("createSequenceStmt");
            var caseExpression = new NonTerminal("caseExpression");
            var caseWhenElseList = new NonTerminal("caseWhenElseList");
            var caseWhenList = new NonTerminal("caseWhenList");
            var caseWhen = new NonTerminal("caseWhen");
            var caseElse = new NonTerminal("caseElse");
            var columnConstraint = new NonTerminal("columnConstraint");
            var columnConstraintListOpt = new NonTerminal("columnConstraintListOpt");
            var semiOpt = new NonTerminal("semiOpt");
            var stmtList = new NonTerminal("stmtList");
            var binOpNot = new NonTerminal("binOpNot");
            var setTransactionStmt = new NonTerminal("setTransactionStmt");
            var valuesList = new NonTerminal("valuesList");
            var winFunOpt = new NonTerminal("winFunOpt");
            var createExtensionStmt = new NonTerminal("createExtensionStmt");
            var opIn = new NonTerminal("opIn");
            var stmtContent = new NonTerminal("stmtContent");
            var root = new NonTerminal("root");
            var binOpIsDistinct = new NonTerminal("binOpIsDistinct");
            var binOpNotIsDistinct = new NonTerminal("binOpNotIsDistinct");
            var extractExpr = new NonTerminal("extractExpr");
            var extractField = new NonTerminal("extractField");
            var isolationLevel = new NonTerminal("isolationLevel");
            var join = new NonTerminal("join");
            var usingClauseOpt = new NonTerminal("usingClauseOpt");
            var idList = new NonTerminal("idList");
            var idColumna = new NonTerminal("idColumna");
            var fkTableConstraint = new NonTerminal("fkTableConstraint");
            var fkTableConstraintOpt = new NonTerminal("fkTableConstraintOpt");
            var revokeStmt = new NonTerminal("revokeStmt");
            var usingIndexClauseOpt = new NonTerminal("usingIndexClauseOpt");
            var createTriggerMomentumClause = new NonTerminal("createTriggerMomentumClause");
            var createTriggerActionClause = new NonTerminal("createTriggerActionClause");
            var createTriggerRepetitionClause = new NonTerminal("createTriggerRepetitionClause");
            var createTriggerWhenClauseOpt = new NonTerminal("createTriggerWhenClauseOpt");
            var createTriggerExecuteClause = new NonTerminal("createTriggerExecuteClause");
            var createSchemaStmt = new NonTerminal("createSchemaStmt");
            var dropTriggerStmt = new NonTerminal("dropTriggerStmt");
            var dropSchemaStmt = new NonTerminal("dropSchemaStmt");
            var createFunctionStmt = new NonTerminal("createFunctionStmt");
            var createFunctionArgs = new NonTerminal("createFunctionArgs");
            var createFunctionArg = new NonTerminal("createFunctionArg");
            var createFunctionArgMode = new NonTerminal("createFunctionArgMode");
            var createFunctionArgDefault = new NonTerminal("createFunctionArgDefault");
            var createFunctionReturns = new NonTerminal("createFunctionReturns");
            var createFunctionReturnsTableColumns = new NonTerminal("createFunctionReturnsTableColumns");
            var createFunctionClauses = new NonTerminal("createFunctionClauses");
            var createFunctionClause = new NonTerminal("createFunctionClause");
            var dollarStringBodyFunction = new NonTerminal("dollarStringBodyFunction");
            var castExpr = new NonTerminal("castExpr");
            var notNull = new NonTerminal("notNull");
            var showStmt = new NonTerminal("showStmt");

            var plStmtList = new NonTerminal("plStmtList");
            var plStmt = new NonTerminal("plStmt");
            var plStmtContent = new NonTerminal("plStmtContent");
            var plBlockCodeStmt = new NonTerminal("plBlockCodeStmt");
            var plDeclareClauseOpt = new NonTerminal("plDeclareClauseOpt");
            var plDeclareStmts = new NonTerminal("plDeclareStmts");
            var plBeginClause = new NonTerminal("plBeginClause");
            var plExceptionClauseOpt = new NonTerminal("plExceptionClause");
            var plExceptionClauseStruct = new NonTerminal("plExceptionClauseStruct");
            var plExceptionClauseConditions = new NonTerminal("plExceptionClauseConditions");
            var plAssignmentStmt = new NonTerminal("plAssignmentStmt");
            var plReturnStmt = new NonTerminal("plReturnStmt");
            var plIfStmt = new NonTerminal("plIfStmt");
            var plCaseStmt = new NonTerminal("plCaseStmt");
            var plCaseWhenElseList = new NonTerminal("plCaseWhenElseList");
            var plCaseWhenList = new NonTerminal("plCaseWhenList");
            var plForStmt = new NonTerminal("plForStmt");
            var plWhileLoopStmt = new NonTerminal("plWhileLoopStmt");
            var plLoopStmt = new NonTerminal("plLoopStmt");
            var plLoopStmtList = new NonTerminal("plLoopStmtList");
            var plLoopStmtListStmt = new NonTerminal("plLoopStmtListStmt");
            var plExitStmt = new NonTerminal("plExitStmt");
            var plRaiseStmt = new NonTerminal("plRaiseStmt");
            var plExpressionList = new NonTerminal("plExpressionList");

            //BNF Rules
            Root = root;

            root.Rule = stmtList;
            stmtList.Rule = MakeStarRule(stmtList, stmt | Empty + semi);

            //ID
            id.Rule = id_simple | id_simple + dot + id_simple;
            idColumna.Rule = id_simple + dot + id_simple | id_simple + dot + id_simple + dot + id_simple;
            stmtContent.Rule = createTableStmt | createIndexStmt | createExtensionStmt | createRoleStmt | alterStmt
                      | dropTableStmt | dropIndexStmt | dropSequenceStmt | dropFunctionStmt
                      | selectStmt | insertStmt | updateStmt | deleteStmt
                      | "GO" | "BEGIN" + (Empty | isolationLevel) | "COMMIT" | "ROLLBACK" | setStmt | setTransactionStmt
                      | truncateStmt | grantStmt | revokeStmt | createTriggerStmt | createSequenceStmt | createSchemaStmt
                      | dropTriggerStmt | dropSchemaStmt | showStmt | createFunctionStmt;
            stmt.Rule = stmtContent + semi;

            stmt.ErrorRule = SyntaxError + semi; //skip all until semicolon

            setStmt.Rule = SET + (Empty | "SESSION" | "BEGIN") + id_simple + (TO | "=") + (exprList | DEFAULT);
            setTransactionStmt.Rule = SET + "TRANSACTION" + isolationLevel;
            isolationLevel.Rule = ToTerm("ISOLATION") + "LEVEL" + ("SERIALIZABLE" | ToTerm("REPEATABLE") + "READ" | ToTerm("READ") + "COMMITTED" | ToTerm("READ") + "UNCOMMITTED");

            showStmt.Rule = "SHOW" + id_simple;

            truncateStmt.Rule = TRUNCATE + (Empty | "TABLE") + (Empty | "ONLY") + id;

            grantStmt.Rule = ToTerm("GRANT") +
                (
                    "ALL" + ("PRIVILEGES" | Empty)
                    | SELECT | INSERT | UPDATE | DELETE | TRUNCATE | REFERENCES
                    | "TRIGGER" | "USAGE" | CREATE | "CONNECT" | "TEMPORARY" | "TEMP"
                    | SET | ALTER + "SYSTEM"
                ) + "ON" +
                (
                    "SCHEMA" + id_simple
                    | grantObjectTable
                    | grantObjectSchema
                    | "ALL" + (ToTerm("TABLES") | "SEQUENCES") + IN + "SCHEMA" + id_simple
                    | "DATABASE" + id_simple
                ) + TO + id_simple;

            grantObjectTable.Rule = (Empty | "TABLE") + id;
            grantObjectSchema.Rule = "SEQUENCE" + id;

            revokeStmt.Rule = ToTerm("REVOKE") +
                (
                    "ALL" + ("PRIVILEGES" | Empty)
                    | SELECT | INSERT | UPDATE | DELETE | TRUNCATE | REFERENCES
                    | "TRIGGER" | "USAGE" | CREATE | "CONNECT" | "TEMPORARY" | "TEMP"
                    | SET | ALTER + "SYSTEM"
                ) + "ON" +
                (
                    "SCHEMA" + id_simple
                    | "TABLE" + id
                    | "SEQUENCE" + id
                    | "ALL" + (ToTerm("TABLES") | "SEQUENCES") + IN + "SCHEMA" + id_simple
                    | "DATABASE" + id_simple
                ) + FROM + id_simple;

            createSequenceStmt.Rule = CREATE + SEQUENCE + id + (Empty | "OWNED" + BY + idColumna);
            createExtensionStmt.Rule = CREATE + "EXTENSION" + id_simple;
            createSchemaStmt.Rule = CREATE + "SCHEMA" + id_simple;

            //Create trigger
            createTriggerStmt.Rule = CREATE + (ToTerm("OR") + "REPLACE" | Empty) + "TRIGGER" + id_simple + createTriggerMomentumClause + createTriggerActionClause + ON + id + createTriggerRepetitionClause + createTriggerWhenClauseOpt + createTriggerExecuteClause;
            createTriggerMomentumClause.Rule = ToTerm("AFTER") | "BEFORE" | ToTerm("INSTEAD") + "OF";
            createTriggerActionClause.Rule = MakePlusRule(createTriggerActionClause, ToTerm("OR"), INSERT | UPDATE | DELETE | TRUNCATE);
            createTriggerRepetitionClause.Rule = "FOR" + ("EACH" | Empty) + (ToTerm("ROW") | "STATEMENT");
            createTriggerWhenClauseOpt.Rule = Empty | "WHEN" + ToTerm("(") + expression + ToTerm(")");
            createTriggerExecuteClause.Rule = ToTerm("EXECUTE") + (ToTerm("PROCEDURE") | "FUNCTION") + id + ToTerm("(") + ")";

            //Create function
            createFunctionStmt.Rule =
                CREATE + (ToTerm("OR") + "REPLACE" | Empty) + "FUNCTION" + id
                + "(" + createFunctionArgs + ")" + createFunctionReturns
                + createFunctionClauses;
            createFunctionArgs.Rule = MakeStarRule(createFunctionArgs, comma, createFunctionArg);
            createFunctionArg.Rule = createFunctionArgMode + id_simple + typeName + typeParamsOpt + createFunctionArgDefault;
            createFunctionArgMode.Rule = ToTerm("IN") | "OUT" | "INOUT" | "VARIADIC" | Empty;
            createFunctionArgDefault.Rule = ((DEFAULT | "=") + expression) | Empty;
            createFunctionReturns.Rule = "RETURNS" + (typeName + typeParamsOpt | "TRIGGER") | ToTerm("RETURNS") + "TABLE" + "(" + createFunctionReturnsTableColumns + ")" | Empty;
            createFunctionReturnsTableColumns.Rule = MakeStarRule(createFunctionReturnsTableColumns, comma, id_simple + typeName + typeParamsOpt);
            createFunctionClauses.Rule = MakePlusRule(createFunctionClauses, createFunctionClause);
            createFunctionClause.Rule =
                "LANGUAGE" + (string_literal | id_simple)
                | "IMMUTABLE" | "STABLE" | "VOLATILE"
                | ("NOT" | Empty) + "LEAKPROOF"
                | "COST" + number
                | "ROWS" + number
                | AS + (string_literal | escaped_string_literal | dollarStringBodyFunction);
            dollarStringBodyFunction.Rule = dollar_string_tag + plBlockCodeStmt + (semi | Empty) + dollar_string_tag;
            plBlockCodeStmt.Rule = plDeclareClauseOpt + plBeginClause + plExceptionClauseOpt + END;
            plDeclareClauseOpt.Rule = Empty | "DECLARE" + plDeclareStmts;
            plDeclareStmts.Rule = MakeStarRule(plDeclareStmts, id_simple + (Empty | "CONSTANT") + typeName + typeParamsOpt + (Empty | notNull) + (Empty | (DEFAULT | ToTerm(":=") | "=") + expression) + semi);
            plBeginClause.Rule = "BEGIN" + plStmtList;
            plStmtList.Rule = MakeStarRule(plStmtList, plStmt);

            plStmt.Rule = stmt | plStmtContent + semi;
            plStmtContent.Rule = plBlockCodeStmt | plAssignmentStmt | plReturnStmt | plIfStmt | plCaseStmt | plForStmt | plWhileLoopStmt | plLoopStmt | plRaiseStmt;
            plAssignmentStmt.Rule = id_simple + (ToTerm("=") | ToTerm(":=")) + expression;
            plReturnStmt.Rule = "RETURN" + expression;
            plIfStmt.Rule = IF + expression + "THEN" + plStmtList + (Empty | "ELSIF" + expression + "THEN" + plStmtList) + (Empty | "ELSE" + plStmtList) + END + IF;
            plCaseStmt.Rule = "CASE" + (Empty | expression) + plCaseWhenElseList + END + "CASE";
            plCaseWhenElseList.Rule = plCaseWhenList + (Empty | "ELSE" + plStmtList);
            plCaseWhenList.Rule = MakePlusRule(plCaseWhenList, WHEN + plExpressionList + "THEN" + plStmtList);
            plForStmt.Rule = "FOR" + id_simple + IN + ("REVERSE" | Empty) + expression + ".." + expression + (Empty | BY + expression) + "LOOP" + plLoopStmtList + END + "LOOP";
            plWhileLoopStmt.Rule = "WHILE" + expression + plLoopStmt;
            plLoopStmt.Rule = "LOOP" + plLoopStmtList + END + "LOOP";
            plLoopStmtList.Rule = MakeStarRule(plLoopStmtList, plLoopStmtListStmt);
            plLoopStmtListStmt.Rule = plStmt | plExitStmt + semi;
            plExitStmt.Rule = "EXIT" + (Empty | WHEN + expression);
            plRaiseStmt.Rule = "RAISE" + (Empty | "DEBUG" | "LOG" | "INFO" | "NOTICE" | "WARNING" | "EXCEPTION") + plExpressionList + (Empty | USING + assignList);
            plExpressionList.Rule = MakePlusRule(plExpressionList, comma, expression);

            plExceptionClauseOpt.Rule = Empty | "EXCEPTION" + plExceptionClauseStruct;
            plExceptionClauseStruct.Rule = MakePlusRule(plExceptionClauseStruct, WHEN + plExceptionClauseConditions + "THEN" + plStmtList);
            plExceptionClauseConditions.Rule = MakePlusRule(plExceptionClauseConditions, ToTerm("OR"), id_simple);

            //functionCode.Rule = MakePlusRule(functionCode, number | string_literal | escaped_string_literal | id_simple | dot | comma | semi | "*" | "/" | "%" | "+" | "-" | "=" | ">" | "<" | ">=" | "<=" | "<>" | "!=" | "!<" | "!>" | "^" | "&" | "|" | "(" | ")" | "[" | "]" | "::" | "~" | "@@" | ":=");

            //Create table
            createTableStmt.Rule = CREATE + TABLE
                + (Empty | IF + NOT + "EXISTS") + id
                + (
                    "(" + createTableDefList + ")" + createTableWithClauseOpt + createTableTablespaceClauseOpt
                    | createTableWithClauseOpt + createTableTablespaceClauseOpt + AS + selectStmt
                );
            createTableDefList.Rule = MakePlusRule(createTableDefList, comma, fieldDef | constraintDef);
            fieldDef.Rule = id_simple + (Empty | "TYPE") + typeName + typeParamsOpt + (Empty | "COLLATE" + id) + columnConstraintListOpt;
            columnConstraintListOpt.Rule = MakeStarRule(columnConstraintListOpt, columnConstraint);
            columnConstraint.Rule =
                (Empty | CONSTRAINT + id_simple) + (
                    nullColumnConstraint
                    | fkColumnConstraint
                    | PRIMARY + KEY
                    | UNIQUE
                    | "DEFAULT" + expression
                    | ToTerm("CHECK") + "(" + expression + ")"
                    | ToTerm("GENERATED") + ("ALWAYS" | BY + DEFAULT) + AS + "IDENTITY"
                );
            nullColumnConstraint.Rule = NULL | notNull;
            fkColumnConstraint.Rule = REFERENCES + id + "(" + id_simple + ")" + onActionClauseListOpt;
            typeName.Rule = ToTerm("BIT") + (Empty | "VARYING") | "VARBIT" | "DATE"
                | "TIME" + (Empty | (ToTerm("WITHOUT") | "WITH") + "TIME" + "ZONE")
                | "TIMESTAMP" + (Empty | (ToTerm("WITHOUT") | "WITH") + "TIME" + "ZONE")
                | "DECIMAL" | "REAL" | "FLOAT" | "FLOAT4" | "FLOAT8"
                | "SMALLINT" | "INTEGER" | "INT" | "INTERVAL" | "CHARACTER" + (Empty | ToTerm("VARYING")) | "DATETIME"
                | "INT2" | "INT4" | "INT8" | "SERIAL2" | "SERIAL4" | "SERIAL8"
                | ToTerm("DOUBLE") + "PRECISION" | "CHAR" | "VARCHAR" | "BYTEA" | "TEXT" | "SERIAL" | "BIGSERIAL"
                | "BIGINT" | "BOOLEAN" | "BOOL" | "INET" | "CIDR" | "JSON" | "JSONB" | "MONEY" | "NUMERIC"
                | "SMALLSERIAL" | "TSQUERY" | "TSVECTOR" | "XML" | "POINT" | "REGCONFIG" | "REGCLASS" | "REGNAMESPACE";

            typeParamsOpt.Rule = "(" + number + ")" | "(" + number + comma + number + ")" | Empty;
            constraintDef.Rule = CONSTRAINT + id + (
                    PRIMARY + KEY + idlistPar
                    | UNIQUE + idlistPar | notNull + idlistPar
                    | fkTableConstraint
                );
            fkTableConstraint.Rule = "FOREIGN" + KEY + idlistPar + REFERENCES + id + idlistPar + fkTableConstraintOpt;
            fkTableConstraintOpt.Rule = (Empty | "MATCH" + (FULL | "SIMPLE")) + onActionClauseListOpt + (Empty | NOT + "DEFERRABLE" + "INITIALLY" + "IMMEDIATE");
            idlistPar.Rule = "(" + idSimpleList + ")";
            idSimpleList.Rule = MakePlusRule(idSimpleList, comma, id_simple);
            idList.Rule = MakePlusRule(idList, comma, id);
            onActionClauseListItem.Rule = ON + (UPDATE | DELETE | INSERT) + (SET + NULL | "RESTRICT" | "CASCADE" | ToTerm("NO") + "ACTION");
            onActionClauseListOpt.Rule = MakeStarRule(onActionClauseListOpt, onActionClauseListItem);
            createTableWithClauseOpt.Rule = Empty | WITH + "(" + createTableWithList + ")";
            createTableWithList.Rule = MakeStarRule(createTableWithList, comma, createTableWithItem);
            createTableWithItem.Rule = id_simple + (Empty | "=" + expression);
            createTableTablespaceClauseOpt.Rule = Empty | "TABLESPACE" + id_simple;

            //Create Index
            createIndexStmt.Rule = CREATE + uniqueOpt + INDEX + id + ON + id + usingIndexClauseOpt + "(" + orderList + ")" + withClauseOpt + whereClauseOpt + tablespaceClauseOpt;
            uniqueOpt.Rule = Empty | UNIQUE;
            orderList.Rule = MakePlusRule(orderList, comma, orderMember);
            orderMember.Rule = expression + orderDirOpt + (Empty | ToTerm("NULLS") + (ToTerm("FIRST") | "LAST"));
            orderDirOpt.Rule = Empty | "ASC" | "DESC" | id_simple;
            usingIndexClauseOpt.Rule = Empty | "USING" + id;
            withClauseOpt.Rule = Empty | WITH + PRIMARY | WITH + "Disallow" + NULL | WITH + "Ignore" + NULL;
            tablespaceClauseOpt.Rule = Empty | "TABLESPACE" + id_simple;

            createRoleStmt.Rule = CREATE + (ToTerm("ROLE") | "USER") + id_simple + (Empty | (Empty | WITH) + createRoleOptions);
            createRoleOptions.Rule = MakePlusRule(createRoleOptions, createRoleOption);

            createRoleOption.Rule =
                alterRoleOption
                | IN + "ROLE" + idSimpleList
                | "ROLE" + idSimpleList
                | "ADMIN" + idSimpleList;

            //Alter 
            alterStmt.Rule = ALTER 
                + (
                    TABLE + (Empty | IF + "EXISTS") + (Empty | "ONLY") + id + alterTable
                    | INDEX + (Empty | IF + "EXISTS") + id + "RENAME" + TO + id
                    | SEQUENCE + (Empty | IF + "EXISTS") + id
                        + (
                            "OWNED" + BY + idColumna
                            | AS + typeName + (Empty | "MAXVALUE" + number)
                        )
                    | (ToTerm("ROLE") | "USER") + (
                        (id_simple | "CURRENT_ROLE" | "CURRENT_USER" | "SESSION_USER") + (WITH | Empty) + alterRoleOptions
                        | id_simple + "RENAME" + TO + id_simple
                        | (id_simple | "CURRENT_ROLE" | "CURRENT_USER" | "SESSION_USER" | "ALL") + (Empty | IN + "DATABASE" + id_simple)
                            + (
                                SET + id_simple + ((TO | "=") + ("DEFAULT" | expression) | FROM + "CURRENT")
                                | "RESET" + (id_simple | "ALL")
                            )
                        )
                    | "SCHEMA" + id_simple + "RENAME" + TO + id_simple
                );

            alterRoleOptions.Rule = MakePlusRule(createRoleOptions, createRoleOption);

            alterRoleOption.Rule =
                ToTerm("SUPERUSER") | "NOSUPERUSER" | "CREATEDB" | "NOCREATEDB" | "CREATEROLE"
                | "NOCREATEROLE" | "INHERIT" | "NOINHERIT" | "LOGIN" | "NOLOGIN" | "REPLICATION"
                | "NOREPLICATION" | "BYPASSRLS" | "NOBYPASSRLS" | ToTerm("CONNECTION") + "LIMIT" + number
                | ToTerm("ENCRYPTED") + "PASSWORD" + expression
                | "PASSWORD" + (NULL | expression)
                | ToTerm("VALID") + "UNTIL" + expression;

            alterTable.Rule =
                ADD + (Empty | COLUMN) + (Empty | IF + NOT + "EXISTS") + fieldDef
                | ADD + constraintDef
                | ALTER + COLUMN + id_simple + (SET + (notNull | DEFAULT + expression) | DROP + (DEFAULT | notNull) | ("TYPE" | ToTerm("SET") + "DATA" + "TYPE") + typeName + typeParamsOpt)
                | DROP + COLUMN + id_simple + (Empty | "CASCADE")
                | DROP + CONSTRAINT + id_simple
                | ToTerm("RENAME") + TO + id_simple
                | "RENAME" + COLUMN + id_simple + TO + id_simple
                | ToTerm("SET") + "SCHEMA" + id_simple
                | ToTerm("ENABLE") + "TRIGGER" + id_simple
                | ToTerm("DISABLE") + "TRIGGER" + id_simple
                | "OWNER" + TO + id_simple;

            //Drop stmts
            dropTableStmt.Rule = DROP + TABLE + (Empty | IF + "EXISTS") + id;
            dropIndexStmt.Rule = DROP + INDEX + (Empty | IF + "EXISTS") + id;
            dropSequenceStmt.Rule = DROP + SEQUENCE + (Empty | IF + "EXISTS") + id;
            dropFunctionStmt.Rule = DROP + FUNCTION + (Empty | IF + "EXISTS") + id + (Empty | "(" + createFunctionArgs + ")");
            dropTriggerStmt.Rule = DROP + "TRIGGER" + (Empty | IF + "EXISTS") + id_simple + ON + id + ("CASCADE" | Empty);
            dropSchemaStmt.Rule = DROP + "SCHEMA" + (Empty | IF + "EXISTS") + id_simple + ("CASCADE" | Empty);

            //Insert stmt
            insertStmt.Rule = cteClauseOpt + INSERT + INTO + id + idlistPar + insertData + insertOnConflictClauseOpt + insertReturningClauseOpt;
            insertOnConflictClauseOpt.Rule = Empty | ON + "CONFLICT" + ON + "CONSTRAINT" + id_simple + ToTerm("DO") + ("NOTHING" | UPDATE + SET + assignList);
            insertReturningClauseOpt.Rule = Empty | "RETURNING" + idSimpleList;
            insertData.Rule = selectStmt | VALUES + valuesList;
            valuesList.Rule = MakePlusRule(valuesList, comma, "(" + exprList + ")");

            //Update stmt
            updateStmt.Rule = cteClauseOpt + UPDATE + id + aliasOpt + SET + assignList + fromClauseOpt + whereClauseOpt;
            assignList.Rule = MakePlusRule(assignList, comma, assignment);
            assignment.Rule = id_simple + "=" + expression;

            //Delete stmt
            deleteStmt.Rule = cteClauseOpt + DELETE + FROM + id + aliasOpt + usingClauseOpt + whereClauseOpt;

            //Select stmt
            selectStmt.Rule = cteClauseOpt + selectBody + selectCombineClauseOpt + orderClauseOpt + limitClauseOpt + offsetClauseOpt;
            selectBody.Rule = selectBaseClauses | "(" + selectStmt + ")";
            selectBaseClauses.Rule = SELECT + selRestrOpt + selList + intoClauseOpt + fromClauseOpt + whereClauseOpt + groupClauseOpt + havingClauseOpt;
            selectCombineClauseOpt.Rule = Empty | ((ToTerm("UNION") | "INTERSECT" | "EXCEPT") + ("ALL" | Empty) + selectBody);
            cteClauseOpt.Rule = Empty | WITH + cteClauseList;
            cteClauseList.Rule = MakeStarRule(cteClauseList, comma, id + AS + "(" + (selectStmt | insertStmt | updateStmt | deleteStmt) + ")");
            selRestrOpt.Rule = Empty | "ALL" | DISTINCT;
            selList.Rule = MakeStarRule(selList, comma, selItem);
            selItem.Rule = expression + aliasOpt | "*" | id_simple + dot + "*";
            aliasOpt.Rule = Empty | asOpt + id_simple;
            asOpt.Rule = Empty | AS;
            intoClauseOpt.Rule = Empty | INTO + id;
            fromItemList.Rule = fromItem + joinChainOpt;
            fromItem.Rule = (id | parSelectStmtExpr | funCall) + aliasOpt;
            usingClauseOpt.Rule = Empty | USING + fromItemList;
            fromClauseOpt.Rule = Empty | FROM + fromItemList;
            joinChainOpt.Rule = MakeStarRule(joinChainOpt, join);
            join.Rule = joinKindOpt + JOIN + fromItem + ON + expression | comma + fromItem;
            joinKindOpt.Rule = Empty | "INNER" | "LEFT" + (Empty | "OUTER") | "RIGHT" + (Empty | "OUTER");
            whereClauseOpt.Rule = Empty | "WHERE" + expression;
            groupClauseOpt.Rule = Empty | "GROUP" + BY + idList + havingClauseOpt;
            havingClauseOpt.Rule = Empty | "HAVING" + expression;
            orderClauseOpt.Rule = Empty | "ORDER" + BY + orderList;
            limitClauseOpt.Rule = Empty | "LIMIT" + expression;
            offsetClauseOpt.Rule = Empty | "OFFSET" + expression;

            //Expression
            exprList.Rule = MakePlusRule(exprList, comma, expression);
            expression.Rule = term | unExpr | binExpr | inExpr;// | betweenExpr; //-- BETWEEN doesn't work - yet; brings a few parsing conflicts 
            term.Rule = NULL | boolLit | id | string_literal | escaped_string_literal | number
                | ToTerm("EXISTS") + parSelectStmtExpr | ToTerm("ARRAY") + parSelectStmtExpr | funCall
                | tuple | parSelectStmtExpr | caseExpression
                | term + "::" + typeName + typeParamsOpt
                | term + "[" + expression + "]" | extractExpr | castExpr
                | ToTerm("SUBSTRING") + "(" + expression + FROM + expression + "FOR" + expression + ")";
            tuple.Rule = "(" + exprList + ")";
            parSelectStmtExpr.Rule = "(" + selectStmt + ")";
            unExpr.Rule = unOp + term;
            unOp.Rule = NOT | "+" | "-" | "~";
            binExpr.Rule = expression + binOp + expression;
            binOp.Rule = ToTerm("+") | "-" | "*" | "/" | "%" //arithmetic
                       | "&" | "|" | "^"                     //bit
                       | "=" | ">" | "<" | ">=" | "<=" | "<>" | "!=" | "!<" | "!>"
                       | "AND" | "OR" | "LIKE" | binOpNot + "LIKE" | "ILIKE" | "@@"
                       | binOpNot + "ILIKE" | "||" | "~" | "!~" | binOpIsDistinct;

            binOpIsDistinct.Rule = "IS" + (Empty | (binOpNotIsDistinct | Empty) + DISTINCT + "FROM");
            binOpNotIsDistinct.Rule = CustomActionHere(ResolveNotIsDistinctConflict) + NOT;
            binOpNot.Rule = CustomActionHere(ResolveNotConflict) + NOT;

            betweenExpr.Rule = expression + notOpt + "BETWEEN" + expression + "AND" + expression;
            notOpt.Rule = Empty | NOT;
            funCall.Rule = (COUNT | "AVG" | "MIN" | "MAX" | "SUM" | id) + "(" + funArgs + ")" + winFunOpt;
            winFunOpt.Rule = Empty | ToTerm("OVER") + "(" + (Empty | ToTerm("PARTITION") + "BY" + exprList) + (Empty | ToTerm("ORDER") + "BY" + orderList) + ")";
            funArgs.Rule = "*" | exprList | Empty;
            inExpr.Rule = expression + opIn + (tuple | parSelectStmtExpr);
            opIn.Rule = IN | binOpNot + IN;

            caseExpression.Rule = "CASE" + (Empty | expression) + caseWhenElseList + END;
            caseWhenElseList.Rule = caseWhenList + (Empty | caseElse);
            caseWhenList.Rule = MakePlusRule(caseWhenList, caseWhen);
            caseWhen.Rule = WHEN + expression + "THEN" + expression;
            caseElse.Rule = "ELSE" + expression;
            extractExpr.Rule = ToTerm("EXTRACT") + "(" + extractField + "FROM" + term + ")";
            extractField.Rule = ToTerm("CENTURY") | "DAY" | "DECADE" | "DOW" | "DOY" | "EPOCH"
                | "HOUR" | "ISODOW" | "ISOYEAR" | "JULIAN" | "MICROSECONDS" | "MILLENNIUM"
                | "MILLISECONDS" | "MINUTE" | "MONTH" | "QUARTER" | "SECOND" | "TIMEZONE"
                | "TIMEZONE_HOUR" | "TIMEZONE_MINUTE" | "WEEK" | "YEAR";
            castExpr.Rule = ToTerm("CAST") + "(" + expression + "AS" + typeName + ")";

            notNull.Rule = NOT + NULL;

            //Operators
            RegisterOperators(10, "*", "/", "%");
            RegisterOperators(9, "+", "-");
            RegisterOperators(8, "=", ">", "<", ">=", "<=", "<>", "!=", "!<", "!>", "LIKE", "IN");
            RegisterOperators(7, "^", "&", "|");
            RegisterOperators(6, NOT);
            RegisterOperators(5, "AND");
            RegisterOperators(4, "OR");
            RegisterOperators(3, ":=");

            MarkPunctuation(",", "(", ")", "[", "]", ":=", "=");
            MarkPunctuation(AS, semi, dot, comma);
            //Note: we cannot declare binOp as transient because it includes operators "NOT LIKE", "NOT IN" consisting of two tokens. 
            // Transient non-terminals cannot have more than one non-punctuation child nodes.
            // Instead, we set flag InheritPrecedence on binOp , so that it inherits precedence value from it's children, and this precedence is used
            // in conflict resolution when binOp node is sitting on the stack
            //base.MarkTransient(stmt, term, asOpt, aliasOpt, stmtListOpt, semiOpt, expression, unOp, tuple);
            binOp.SetFlag(TermFlags.InheritPrecedence);
            //IN.Priority = TerminalPriority.High;
            DISTINCT.Priority = TerminalPriority.High;
        }//constructor

        private void ResolveNotConflict(ParsingContext context, CustomParserAction customAction)
        {
            var scanner = context.Parser.Scanner;

            ParserAction? action;

            if (context.CurrentParserInput.Term.Name == "NOT")
            {
                scanner.BeginPreview();

                Token preview = scanner.GetToken();
                string? previewSym = null;

                if (preview.Terminal != Eof)
                {
                    previewSym = preview.Terminal.Name;
                }

                scanner.EndPreview(true); //keep previewed tokens; important to keep ">>" matched to two ">" symbols, not one combined symbol (see method below)

                if (previewSym == "NULL")
                {
                    action = customAction.ReduceActions.First();
                }
                else
                {
                    action = customAction.ShiftActions.FirstOrDefault(a => a.Term.Name == context.CurrentParserInput.Term.Name);
                }
            }
            else
            {
                action = customAction.ShiftActions.FirstOrDefault(a => a.Term.Name == context.CurrentParserInput.Term.Name);
            }

            //foreach (ShiftParserAction pai in customAction.ShiftActions)
            //{
            //    Debug.WriteLine(pai.Term.Name + " - " + pai.Term.Flags + " - " + (pai.Term.Flags & TermFlags.IsKeyword));
            //}

            if (action != null) action.Execute(context);
        }

        private void ResolveNotIsDistinctConflict(ParsingContext context, CustomParserAction customAction)
        {
            var scanner = context.Parser.Scanner;

            ParserAction? action;

            if (context.CurrentParserInput.Term.Name == "NOT")
            {
                scanner.BeginPreview();

                string? previewSym = null;

                Token preview = scanner.GetToken();
                if (preview.Terminal != Eof)
                {
                    previewSym = preview.Terminal.Name;
                }

                scanner.EndPreview(true); //keep previewed tokens; important to keep ">>" matched to two ">" symbols, not one combined symbol (see method below)

                if (previewSym != "DISTINCT")
                {
                    action = customAction.ReduceActions.First();
                }
                else
                {
                    action = customAction.ShiftActions.FirstOrDefault(a => a.Term.Name == context.CurrentParserInput.Term.Name);
                }
            }
            else
            {
                action = customAction.ShiftActions.FirstOrDefault(a => a.Term.Name == context.CurrentParserInput.Term.Name);
            }

            //foreach (ShiftParserAction pai in customAction.ShiftActions)
            //{
            //    Debug.WriteLine(pai.Term.Name + " - " + pai.Term.Flags + " - " + (pai.Term.Flags & TermFlags.IsKeyword));
            //}

            if (action != null) action.Execute(context);
        }

        private IdentifierTerminal CreateIdentifier(string name)
        {
            var id = new IdentifierTerminal(name);
            StringLiteral term = new StringLiteral(name + "_qouted");
            term.AddStartEnd("\"", StringOptions.NoEscapes);
            term.SetOutputTerminal(this, id);
            return id;
        }

        public override bool IsWhitespaceOrDelimiter(char ch)
        {
            if (base.IsWhitespaceOrDelimiter(ch)) return true;

            switch (ch)
            {
                case '*':
                case '/':
                case '%':
                case '+':
                case '-':
                case '=':
                case '>':
                case '<':
                case '!':
                case '^':
                case '&':
                case '|':
                case ':':
                    return true;
                default:
                    return false;
            }
        }

    }
}

using FastColoredTextBoxNS;
using Irony.Parsing;
using System.Text;
using System.Text.RegularExpressions;

namespace PgMulti.SqlSyntax
{
    public class AstNode
    {
        private const int AutoBreakLineLength = 50;
        private AstNode? _Parent = null;
        private List<AstNode> _Children = new List<AstNode>();
        private string _Name;
        private Token? _Token;
        private bool _IsStatementHeader = false;
        private int _Index = -1;
        private int _StartLine = -1;
        private int _StartColumn = -1;
        private int _EndLine = -1;
        private int _EndColumn = -1;
        private FormatDefinition? _PrevFormatDefinition = null;
        private FormatDefinition? _NextFormatDefinition = null;

        public static AstNode ProcesarParseTree(ParseTree tree)
        {
            if (tree.Status != ParseTreeStatus.Parsed || tree.Root == null)
            {
                throw new ArgumentException();
            }

            Stack<AstNode> astNodeStack = new Stack<AstNode>();
            Stack<ParseTreeNode> nodeStack = new Stack<ParseTreeNode>();
            //Stack<List<ParseTreeNode>> superiorNodesStack = new Stack<List<ParseTreeNode>>();

            AstNode root = new AstNode(null, tree.Root);

            astNodeStack.Push(root);
            nodeStack.Push(tree.Root);
            //superiorNodesStack.Push(new List<ParseTreeNode>());

            while (nodeStack.Count > 0)
            {
                AstNode astNode = astNodeStack.Pop();
                ParseTreeNode node = nodeStack.Pop();
                //List<ParseTreeNode> superiorNodes = superiorNodesStack.Pop();

                if (node.Token != null)
                {
                    Token t = node.Token;
                }
                else
                {
                    foreach (ParseTreeNode ptni in node.ChildNodes)
                    {
                        AstNode childNode = new AstNode(astNode, ptni);
                        astNode.Children.Add(childNode);
                        astNodeStack.Push(childNode);
                        nodeStack.Push(ptni);

                        //List<ParseTreeNode> childSuperiorNodes = new List<ParseTreeNode>();
                        //foreach (ParseTreeNode ptnj in superiorNodes)
                        //{
                        //    childSuperiorNodes.Add(ptnj);
                        //}
                        //childSuperiorNodes.Add(node);
                        //superiorNodesStack.Push(childSuperiorNodes);
                    }
                }
            }

            if (root.Children.Count > 0)
            {
                foreach (AstNode n in root.Children[0].Children)
                {
                    Stack<AstNode> stack = new Stack<AstNode>();
                    stack.Push(n);
                    int i = 0;
                    while (stack.Count > 0 && i < 1)
                    {
                        AstNode ni = stack.Pop();
                        if (ni.Token != null)
                        {
                            ni._IsStatementHeader = true;
                            i++;
                        }
                        else
                        {
                            for (int j = ni.Children.Count - 1; j >= 0; j--)
                            {
                                stack.Push(ni.Children[j]);
                            }
                        }
                    }
                }
            }

            root._PostProcess();

            return root;
        }

        public AstNode(AstNode? parent, ParseTreeNode ptn)
        {
            _Parent = parent;
            if (ptn.Token == null)
            {
                _Name = ptn.Term.Name;
                _Token = null;
            }
            else
            {
                _Name = ptn.Token.Terminal.Name;
                _Token = ptn.Token;
                _StartLine = _Token.Location.Line;
                _StartColumn = _Token.Location.Column;
                _EndLine = _Token.Location.Line;
                _EndColumn = _Token.Location.Column + _Token.Length;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public Token? Token
        {
            get
            {
                return _Token;
            }
        }

        public int Index
        {
            get
            {
                return _Index;
            }
        }

        public int StartLine
        {
            get
            {
                return _StartLine;
            }
        }

        public int StartColumn
        {
            get
            {
                return _StartColumn;
            }
        }

        public int EndLine
        {
            get
            {
                return _EndLine;
            }
        }

        public int EndColumn
        {
            get
            {
                return _EndColumn;
            }
        }

        private List<AstNode>? _RecursiveTokens = null;
        public List<AstNode> RecursiveTokens
        {
            get
            {
                if (_RecursiveTokens == null)
                {
                    _RecursiveTokens = new List<AstNode>();
                    Stack<AstNode> stack = new Stack<AstNode>();

                    stack.Push(this);

                    while (stack.Count > 0)
                    {
                        AstNode na = stack.Pop();
                        if (na.Token == null)
                        {
                            for (int i = na.Children.Count - 1; i >= 0; i--)
                            {

                                stack.Push(na.Children[i]);
                            }
                        }
                        else
                        {
                            _RecursiveTokens.Add(na);
                        }
                    }
                }

                return _RecursiveTokens;
            }
        }

        public string SingleLineText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                Token? tokenAnt = null;
                foreach (AstNode nToken in RecursiveTokens)
                {
                    if (tokenAnt != null && tokenAnt.Location.Position + tokenAnt.Length < nToken.Token!.Location.Position)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(nToken.Token!.Text);
                    tokenAnt = nToken.Token;
                }

                return sb.ToString();
            }
        }

        private bool IsNoSpacesSymbol(string txt)
        {
            const string Symbols = ".,;()[]{}:=<>+-/*%^&!|~;";

            return txt.All(c => Symbols.Contains(c));
        }

        public AstNode? Parent
        {
            get
            {
                return _Parent;
            }
        }

        public List<AstNode> Children
        {
            get
            {
                return _Children;
            }
        }

        public AstNode? this[string name]
        {
            get
            {
                foreach (AstNode child in Children)
                {
                    if (child.Name == name)
                    {
                        return child;
                    }
                }

                return null;
            }
        }

        public AstNode this[int i]
        {
            get
            {
                return Children[i];
            }
        }

        public int Count
        {
            get
            {
                return Children.Count;
            }
        }

        public bool IsStatementHeader
        {
            get
            {
                return _IsStatementHeader;
            }
        }

        private Dictionary<Token, AstNode>? _TokenDict = null;
        public Dictionary<Token, AstNode> TokenDict
        {
            get
            {
                if (_TokenDict == null)
                {
                    _TokenDict = new Dictionary<Token, AstNode>();
                    foreach (AstNode nodoAst in RecursiveTokens)
                    {
                        _TokenDict[nodoAst.Token!] = nodoAst;
                    }
                }
                return _TokenDict;
            }
        }

        public FormatDefinition? PrevFormatDefinition { get => _PrevFormatDefinition; private set => _PrevFormatDefinition = value; }
        public FormatDefinition? NextFormatDefinition { get => _NextFormatDefinition; private set => _NextFormatDefinition = value; }

        public AstNode? GetRecursiveParentNth(int n)
        {
            AstNode? p = this;
            for (int i = 0; i < n && p != null; i++)
            {
                p = p.Parent;
            }

            return p;
        }

        public AstNode? GetRecursiveParentNamedAs(string name)
        {
            AstNode? p = this;
            while (p != null)
            {
                if (p.Name == name) return p;
                p = p.Parent;
            }

            return null;
        }

        public bool ContainsPosition(int line, int column)
        {
            return (line > _StartLine || line == _StartLine && column >= StartColumn) && (line < _EndLine || line == _EndLine && column < EndColumn);
        }

        private void _PostProcess()
        {
            Stack<AstNode> pathStack = new Stack<AstNode>();
            Stack<AstNode> processStack = new Stack<AstNode>();
            pathStack.Push(this);
            processStack.Push(this);

            while (pathStack.Count > 0)
            {
                AstNode n = pathStack.Pop();

                foreach (AstNode ni in n.Children)
                {
                    pathStack.Push(ni);
                    processStack.Push(ni);
                }
            }

            while (processStack.Count > 0)
            {
                AstNode n = processStack.Pop();

                if (n.Children.Count == 0 && n.Token == null)
                {
                    n._RemoveEmptyNode();
                }
                else if (n.Children.Count > 0)
                {
                    n._StartLine = n.Children[0]._StartLine;
                    n._StartColumn = n.Children[0]._StartColumn;
                    n._EndLine = n.Children[n.Children.Count - 1]._EndLine;
                    n._EndColumn = n.Children[n.Children.Count - 1]._EndColumn;
                }
            }

            // Enumerate nodes
            pathStack.Push(this);

            while (pathStack.Count > 0)
            {
                AstNode n = pathStack.Pop();

                for (int i = 0; i < n.Children.Count; i++)
                {
                    AstNode ni = n.Children[i];
                    ni._Index = i;
                    pathStack.Push(ni);
                }
            }
        }

        public void RevisePosition(int lineaIni, int columnaIni, int lineaInsercion, int columnaInsercion, int caracteresInsertados)
        {
            Stack<AstNode> stack = new Stack<AstNode>();

            stack.Push(this);

            while (stack.Count > 0)
            {
                AstNode na = stack.Pop();

                na._StartLine += lineaIni;
                if (na._StartLine == lineaIni)
                {
                    na._StartColumn += columnaIni;
                }
                na._EndLine += lineaIni;
                if (na._EndLine == lineaIni)
                {
                    na._EndColumn += columnaIni;
                }

                if (na._StartLine > lineaInsercion || na._StartLine == lineaInsercion && na._StartColumn > columnaInsercion)
                {
                    if (na.Token != null && na._StartLine == lineaInsercion && na._StartColumn < columnaInsercion + caracteresInsertados)
                    {
                        // Leaf nodes keep fake position to allow to get editor caret token node
                    }
                    else
                    {
                        if (na._StartLine == lineaInsercion)
                        {
                            na._StartColumn -= caracteresInsertados;
                        }
                        if (na._EndLine == lineaInsercion)
                        {
                            na._EndColumn -= caracteresInsertados;
                        }
                    }
                }
                else if (na.ContainsPosition(lineaInsercion, columnaInsercion))
                {
                    if (na.Token != null)
                    {
                        // Leaf nodes keep fake position to allow to get editor caret token 
                    }
                    else if (na._EndLine == lineaInsercion)
                    {
                        na._EndColumn = Math.Max(na._StartColumn, na._EndColumn - caracteresInsertados);
                    }
                }

                if (na.Token == null)
                {
                    for (int i = na.Children.Count - 1; i >= 0; i--)
                    {
                        stack.Push(na.Children[i]);
                    }
                }
            }
        }

        private void _RemoveEmptyNode()
        {
            if (_Parent != null)
            {
                _Parent.Children.Remove(this);
                _Parent = null;
            }
        }

        public override string ToString()
        {
            return Name + " (" + (Token == null ? "non-terminal" : "terminal") + ")";
        }

        public void Format(StringBuilder sb, ParseTree parseTree, int prevIndentation)
        {
            Token[] comments = parseTree.Tokens.Where(tk => tk.Category == TokenCategory.Comment).OrderBy(tk => tk.Location.Position).ToArray();
            int nextCommentIndex = 0;

            _DefineFormats();

            FormatDefinition f = new FormatDefinition();
            f.NoSpace = true;
            f.Indentation = prevIndentation;

            sb.Append(_GetIndent(f.Indentation));

            FormatDefinition? nextFormat = null;
            Stack<Tuple<AstNode, FormatDefinition?>> pila = new Stack<Tuple<AstNode, FormatDefinition?>>();

            pila.Push(new Tuple<AstNode, FormatDefinition?>(this, null));

            int prevTokenLine = 0;
            int lineLength = 0;
            bool lastCommentIsLineComment = false;

            while (pila.Count > 0)
            {
                Tuple<AstNode, FormatDefinition?> t = pila.Pop();
                AstNode na = t.Item1;
                nextFormat = t.Item2;

                if (na.PrevFormatDefinition != null) f.Combinar(na.PrevFormatDefinition);
                if (na.NextFormatDefinition != null)
                {
                    if (nextFormat == null)
                    {
                        nextFormat = na.NextFormatDefinition;
                    }
                    else
                    {
                        nextFormat.Combinar(na.NextFormatDefinition);
                    }
                }

                if (na.Children.Count > 0)
                {
                    for (int i = na.Children.Count - 1; i >= 0; i--)
                    {
                        FormatDefinition? fi = null;
                        if (i == na.Children.Count - 1) fi = nextFormat;
                        pila.Push(new Tuple<AstNode, FormatDefinition?>(na.Children[i], fi));
                    }
                }
                else if (na.Token != null)
                {
                    string? indentationString = null;

                    while (nextCommentIndex < comments.Length && comments[nextCommentIndex].Location.Position < na.Token.Location.Position)
                    {
                        Token comment = comments[nextCommentIndex];
                        if (indentationString == null) indentationString = _GetIndent(f.Indentation);
                        if (prevTokenLine == comment.Location.Line)
                        {
                            sb.Append(" ");
                            lineLength++;
                        }
                        else
                        {
                            if (f.DoubleCR) sb.AppendLine();
                            sb.Append("\r\n" + indentationString);
                            prevTokenLine = comment.Location.Line;
                            lineLength = f.Indentation;
                        }

                        lastCommentIsLineComment = false;
                        switch (comment.Terminal.Name)
                        {
                            case "line_comment":
                                sb.Append(comment.Text);
                                lineLength += comment.Text.Length;
                                lastCommentIsLineComment = true;
                                break;
                            case "comment":
                                int retPos = comment.Text.LastIndexOf("\r\n");
                                if (retPos == -1)
                                {
                                    sb.Append(comment.Text);
                                    lineLength += comment.Text.Length;
                                }
                                else
                                {
                                    sb.Append(comment.Text.Replace("\r\n", "\r\n" + indentationString));
                                    lineLength = f.Indentation + comment.Text.Length - retPos - 2;
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        nextCommentIndex++;
                    }


                    if (f.CR || f.DoubleCR || lastCommentIsLineComment)
                    {
                        sb.AppendLine();
                        lineLength = 0;

                        if (f.DoubleCR) sb.AppendLine();

                        if (indentationString == null) indentationString = _GetIndent(f.Indentation);
                        sb.Append(indentationString);
                        lineLength += f.Indentation;
                        lastCommentIsLineComment = false;
                    }
                    else if (!f.NoSpace)
                    {
                        sb.Append(" ");
                        lineLength++;
                    }

                    string tokenText;

                    if ((na.Token.Terminal.Flags & TermFlags.IsKeyword) != 0)
                    {
                        if (na.GetRecursiveParentNamedAs("typeNameAndParams") != null)
                        {
                            tokenText = na.Token.Text.ToLowerInvariant();
                        }
                        else
                        {
                            tokenText = na.Token.Text.ToUpperInvariant();
                        }
                    }
                    else
                    {
                        tokenText = na.Token.Text;
                    }

                    sb.Append(tokenText);
                    lineLength += tokenText.Length;

                    f.CR = false;
                    f.DoubleCR = false;
                    f.NoCR = false;
                    f.NoDoubleCR = false;
                    f.NoSpace = false;
                    f.CRIfLong = false;
                    if (nextFormat != null) f.Combinar(nextFormat);

                    if (f.CRIfLong && lineLength > AutoBreakLineLength)
                    {
                        f.CR = true;
                    }

                    prevTokenLine = na.Token.Location.Line;
                }
                else
                {
                    throw new NotSupportedException("Tree cannot contain empty nodes");
                }
            }

            while (nextCommentIndex < comments.Length)
            {
                Token comment = comments[nextCommentIndex];

                if (prevTokenLine == comment.Location.Line)
                {
                    sb.Append(" ");
                }
                else
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append(_GetIndent(f.Indentation));
                    prevTokenLine = comment.Location.Line;
                }

                sb.Append(comment.Text);
                lastCommentIsLineComment = (comment.Terminal.Name == "line_comment");

                nextCommentIndex++;
            }


            if (lastCommentIsLineComment || (nextFormat != null && (nextFormat.CR || nextFormat.DoubleCR)))
            {
                sb.AppendLine();
            }
        }

        private string _GetIndent(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                sb.Append(" ");
            }

            return sb.ToString();
        }

        private void _DefineFormats()
        {
            Stack<AstNode> stack = new Stack<AstNode>();
            stack.Push(this);

            while (stack.Count > 0)
            {
                AstNode n = stack.Pop();

                for (int i = 0; i < n.Children.Count; i++)
                {
                    AstNode ni = n.Children[i];
                    ni._Index = i;
                    stack.Push(ni);
                }

                switch (n.Name)
                {
                    case "join":
                        if (n[0].Token != null && n[0].Token!.Text == ",")
                        {
                            n[0]._NextFormatDefinition = new FormatDefinition();
                            n[0]._NextFormatDefinition!.CR = true;
                        }
                        else
                        {
                            n._PrevFormatDefinition = new FormatDefinition();
                            n._PrevFormatDefinition.CR = true;
                        }
                        break;
                    case "fromClauseOpt":
                    case "usingClauseOpt":
                    case "whereClauseOpt":
                    case "groupClauseOpt":
                    case "havingClauseOpt":
                    case "orderClauseOpt":
                    case "limitClauseOpt":
                    case "offsetClauseOpt":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        break;
                    case "insertData":
                        if (n[0].Token != null && n[0].Token!.Text == "VALUES")
                        {
                            n[0]._NextFormatDefinition = new FormatDefinition();
                            n[0]._NextFormatDefinition!.CR = true;
                        }
                        else
                        {
                            n._PrevFormatDefinition = new FormatDefinition();
                            n._PrevFormatDefinition.CR = true;
                        }
                        break;
                    case "assignList":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        n._PrevFormatDefinition.Indentation = 4;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.CR = true;
                        n._NextFormatDefinition.Indentation = -4;
                        break;
                    case "insertStmt":
                        if (n["idlistParOpt"] != null && n["idlistParOpt"]!["idlistPar"] != null)
                        {
                            AstNode nIdlistPar = n["idlistParOpt"]!["idlistPar"]!;
                            if (nIdlistPar != null)
                            {
                                nIdlistPar._PrevFormatDefinition = new FormatDefinition();
                                nIdlistPar._PrevFormatDefinition.CR = true;
                            }
                        }

                        break;
                    case "joinChainOpt":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        n._PrevFormatDefinition.Indentation = 4;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.CR = false;
                        n._NextFormatDefinition.Indentation = -4;
                        break;
                    case "expression":
                        switch (n.Parent!.Name)
                        {
                            case "whereClauseOpt":
                                n._PrevFormatDefinition = new FormatDefinition();
                                n._PrevFormatDefinition.Indentation = 4;

                                n._NextFormatDefinition = new FormatDefinition();
                                n._NextFormatDefinition.Indentation = -4;
                                break;
                            case "caseWhen":
                            case "caseElse":
                                if (n.Index == n.Parent.Count - 1)
                                {
                                    n._NextFormatDefinition = new FormatDefinition();
                                    n._NextFormatDefinition.CR = true;
                                }
                                break;
                        }
                        break;
                    case "alterTable":
                    case "createTriggerExecuteClause":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        n._PrevFormatDefinition.Indentation = 4;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.Indentation = -4;
                        break;
                    case "dollar_string_tag":
                        if (n.Index == 0)
                        {
                            n._NextFormatDefinition = new FormatDefinition();
                            n._NextFormatDefinition.CR = true;
                            n._NextFormatDefinition.Indentation = 4;
                        }
                        else
                        {
                            n._PrevFormatDefinition = new FormatDefinition();
                            n._PrevFormatDefinition.CR = true;
                            n._PrevFormatDefinition.Indentation = -4;

                            n._NextFormatDefinition = new FormatDefinition();
                            n._NextFormatDefinition.CR = true;
                        }
                        break;
                    case "createFunctionClause":
                    case "insertOnConflictClauseOpt":
                    case "fkTableConstraint":
                    case "fkTableConstraintOpt":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        break;
                    case "plStmt":
                    case "plLoopStmtListStmt":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.DoubleCR = true;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.DoubleCR = true;
                        break;
                    case "plStmtList":
                    case "plLoopStmtList":
                    case "plCaseWhenElseList":
                    case "plDeclareStmts":
                    case "plExceptionClauseStruct":
                    case "caseWhenElseList":
                    case "createFunctionClauses":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;
                        n._PrevFormatDefinition.Indentation = 4;
                        n._PrevFormatDefinition.NoDoubleCR = true;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.CR = true;
                        n._NextFormatDefinition.Indentation = -4;
                        n._NextFormatDefinition.NoDoubleCR = true;
                        break;
                    case "caseExpression":
                        n._PrevFormatDefinition = new FormatDefinition();
                        n._PrevFormatDefinition.CR = true;

                        n._NextFormatDefinition = new FormatDefinition();
                        n._NextFormatDefinition.CR = true;
                        break;
                    case "selList":
                        if (n.SingleLineText.Length > AutoBreakLineLength)
                        {
                            n._PrevFormatDefinition = new FormatDefinition();
                            n._PrevFormatDefinition.CR = true;
                            n._PrevFormatDefinition.Indentation = 4;

                            n._NextFormatDefinition = new FormatDefinition();
                            n._NextFormatDefinition.CR = true;
                            n._NextFormatDefinition.Indentation = -4;
                        }
                        break;
                    default:
                        if (n.Token != null)
                        {
                            switch (n.Token.Text.ToUpper())
                            {
                                case "(":
                                    n._NextFormatDefinition = new FormatDefinition();
                                    if (n.Parent!.Name == "cteClauseOpt"
                                        || n.Parent.Name == "parSelectStmtExpr"
                                        || n.Parent.Parent!.Name == "createTableStmt"
                                        || CalculateParenthesisExpressionLength(n, true) > AutoBreakLineLength
                                        )
                                    {
                                        n._PrevFormatDefinition = new FormatDefinition();
                                        n._PrevFormatDefinition.CR = true;

                                        n._NextFormatDefinition.CR = true;
                                    }
                                    else if (n.Parent!.Name == "funCall" || n.Parent!.Name == "createTriggerExecuteClause")
                                    {
                                        n._PrevFormatDefinition = new FormatDefinition();
                                        n._PrevFormatDefinition.NoSpace = true;

                                        n._NextFormatDefinition.NoSpace = true;
                                    }
                                    else
                                    {
                                        n._NextFormatDefinition.NoSpace = true;
                                    }
                                    n._NextFormatDefinition.Indentation = 4;

                                    break;
                                case ")":
                                    n._PrevFormatDefinition = new FormatDefinition();
                                    if (n.Parent!.Name == "cteClauseOpt"
                                        || n.Parent.Name == "parSelectStmtExpr"
                                        || CalculateParenthesisExpressionLength(n, false) > AutoBreakLineLength
                                        )
                                    {
                                        n._PrevFormatDefinition.CR = true;

                                        n._NextFormatDefinition = new FormatDefinition();
                                        n._NextFormatDefinition.CR = true;
                                    }
                                    else if (n.Parent!.Parent!.Name == "createTableStmt")
                                    {
                                        n._PrevFormatDefinition.CR = true;
                                        n._PrevFormatDefinition.Indentation = -4;
                                    }
                                    else if (n.Parent!.Name == "funCall")
                                    {
                                        n._PrevFormatDefinition.NoSpace = true;
                                    }
                                    else
                                    {
                                        n._PrevFormatDefinition.NoSpace = true;
                                    }
                                    n._PrevFormatDefinition.Indentation = -4;

                                    break;
                                case ";":
                                    n._PrevFormatDefinition = new FormatDefinition();
                                    n._PrevFormatDefinition.NoSpace = true;
                                    n._PrevFormatDefinition.NoCR = true;

                                    n._NextFormatDefinition = new FormatDefinition();
                                    n._NextFormatDefinition.CR = true;

                                    break;
                                case ",":
                                    n._PrevFormatDefinition = new FormatDefinition();
                                    n._PrevFormatDefinition.NoSpace = true;
                                    n._PrevFormatDefinition.NoCR = true;

                                    n._NextFormatDefinition = new FormatDefinition();
                                    n._NextFormatDefinition.NoSpace = true;
                                    n._NextFormatDefinition.CRIfLong = true;

                                    switch (n.Parent!.Name)
                                    {
                                        case "createTableDefList":
                                        case "valuesList":
                                        case "assignList":
                                            n._NextFormatDefinition.CR = true;
                                            break;
                                    }
                                    break;
                                case ".":
                                    n._PrevFormatDefinition = new FormatDefinition();
                                    n._PrevFormatDefinition.NoSpace = true;

                                    n._NextFormatDefinition = new FormatDefinition();
                                    n._NextFormatDefinition.NoSpace = true;

                                    break;
                                case "AND":
                                case "OR":
                                    if (n.Parent!.Name == "binOp")
                                    {
                                        n._PrevFormatDefinition = new FormatDefinition();
                                        n._PrevFormatDefinition.CR = true;
                                    }
                                    break;
                                case "RETURNING":
                                    n._PrevFormatDefinition = new FormatDefinition();
                                    n._PrevFormatDefinition.CR = true;
                                    break;
                                case "SET":
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private int CalculateParenthesisExpressionLength(AstNode n, bool openParenthesis)
        {
            List<AstNode> nodes = new List<AstNode>();

            if (openParenthesis)
            {
                for (int i = n.Index; i < n.Parent!.Count; i++)
                {
                    AstNode p = n.Parent[i];
                    nodes.Add(p);
                    if (p.Token != null && p.Token.Text == ")") break;
                }
            }
            else
            {
                for (int i = n.Index; i >= 0; i--)
                {
                    AstNode p = n.Parent![i];
                    nodes.Add(p);
                    if (p.Token != null && p.Token.Text == "(") break;
                }
            }

            int count = 0;
            foreach (AstNode ni in nodes)
            {
                count += 1 + ni.SingleLineText.Length;
            }

            return count;
        }

        public class FormatDefinition
        {
            public int Indentation = 0;
            public bool CR = false;
            public bool DoubleCR = false;
            public bool NoCR = false;
            public bool NoDoubleCR = false;
            public bool NoSpace = false;
            public bool CRIfLong = false;

            public void Combinar(FormatDefinition ef)
            {
                Indentation += ef.Indentation;
                NoCR |= ef.NoCR;
                NoDoubleCR |= ef.NoDoubleCR;
                DoubleCR |= ef.DoubleCR;
                DoubleCR &= !NoCR & !NoDoubleCR;
                CR |= ef.CR;
                CR &= !NoCR & !DoubleCR;
                NoSpace |= ef.NoSpace;
                CRIfLong |= ef.CRIfLong;
            }
        }
    }
}

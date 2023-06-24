using System.Diagnostics;
using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class CustomFctb : FastColoredTextBox
    {
        public event EventHandler<StyleNeededEventArgs>? StyleNeeded;
        public event EventHandler<EventArgs>? ParseTreeUpdated;

        public Style WavyStyle = new WavyLineStyle(255, Color.Red);
        public Style MaroonBoldStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Bold);
        public Style KeywordUpperCaseStyle = new UpperCaseTextStyle(Brushes.Blue, null, FontStyle.Regular);
        public Style KeywordBoldUpperCaseStyle = new UpperCaseTextStyle(Brushes.Blue, null, FontStyle.Bold);

        private Parser? parser;
        private ParseTree? _ParseTree = null;

        public ParseTree? ParseTree
        {
            get
            {
                return _ParseTree;
            }
        }

        public CustomFctb()
        {
        }

        public virtual void SetParser(LanguageData language)
        {
            SetParser(new Parser(language));
        }

        public virtual void SetParser(Parser? parser)
        {
            this.parser = parser;
            ClearStylesBuffer();
            AddStyle(WavyStyle);
            AddStyle(MaroonBoldStyle);

            SyntaxHighlighter.InitStyleSchema(Language.Custom);
            SyntaxHighlighter.StringStyle = SyntaxHighlighter.BrownStyle;
            SyntaxHighlighter.CommentStyle = SyntaxHighlighter.GreenStyle;
            SyntaxHighlighter.NumberStyle = SyntaxHighlighter.MagentaStyle;
            SyntaxHighlighter.FunctionsStyle = MaroonBoldStyle;
            SyntaxHighlighter.VariableStyle = SyntaxHighlighter.BlackStyle;
            SyntaxHighlighter.TypesStyle = SyntaxHighlighter.BrownStyle;

            InitBraces();
            OnTextChanged(Range);
        }

        public override void OnTextChangedDelayed(FastColoredTextBoxNS.Range changedRange)
        {
            DoHighlighting();
            base.OnTextChangedDelayed(changedRange);
        }

        public virtual void DoHighlighting()
        {
            if (parser == null)
                return;

            Place? insertPoint = null;
            string txt = Text;

            try
            {
                _ParseTree = parser.Parse(txt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
            if (_ParseTree.Status == ParseTreeStatus.Error)
            {
                int indexInsertPointToken = _ParseTree.Tokens.Count - 1;
                if (_ParseTree.Tokens[indexInsertPointToken].Terminal.Name == "EOF") indexInsertPointToken--;
                while (_ParseTree.Tokens[indexInsertPointToken].Terminal.Name == "line_comment") indexInsertPointToken--;

                Token insertPointToken = _ParseTree.Tokens[indexInsertPointToken];
                Token lastToken = _ParseTree.Tokens[_ParseTree.Tokens.Count - 1];
                Place start = new Place(0, 0); //new Place(_ParseTree.Tokens[0].Location.Column, _ParseTree.Tokens[0].Location.Line);
                insertPoint = new Place(insertPointToken.Location.Column + insertPointToken.Text.Length, insertPointToken.Location.Line);
                Place end = new Place(lastToken.Location.Column + lastToken.Text.Length, lastToken.Location.Line);
                txt = GetRange(start, insertPoint.Value).Text + ";" + GetRange(insertPoint.Value, end).Text;

                try
                {
                    _ParseTree = parser.Parse(txt);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return;
                }
            }

            ClearStyle(StyleIndex.All);

            AstNode? raizAst = null;
            if (_ParseTree.Status == ParseTreeStatus.Error)
            {
                foreach (Irony.LogMessage msg in _ParseTree.ParserMessages)
                {
                    var loc = msg.Location;
                    var place = new Place(loc.Column, loc.Line);
                    var r = new FastColoredTextBoxNS.Range(this, place, place);
                    var f = r.GetFragment(@"[\w]");
                    if (f.IsEmpty)
                    {
                        f = r.GetFragment(@"[\S]");
                    }
                    f.SetStyle(WavyStyle);
                }
            }
            else if (_ParseTree.Status == ParseTreeStatus.Parsed)
            {
                raizAst = AstNode.ProcesarParseTree(_ParseTree);
            }


            foreach (Token t in _ParseTree.Tokens)
            {
                var arg = new StyleNeededEventArgs(t);
                OnStyleNeeded(arg);

                if (arg.Cancel)
                    continue;

                FastColoredTextBoxNS.Range tr = GetTokenRange(t);

                if (arg.Style != null)
                {
                    tr.SetStyle(arg.Style);
                    continue;
                }

                AstNode? nodoAst = (raizAst == null || !raizAst.TokenDict.ContainsKey(t)) ? null : raizAst.TokenDict[t];

                switch (t.Terminal.Name)
                {
                    case "NULL":
                        if (nodoAst != null && nodoAst.Parent != null && nodoAst.Parent.Name == "term")
                        {
                            tr.SetStyle(SyntaxHighlighter.NumberStyle);
                        }
                        else
                        {
                            tr.SetStyle(KeywordUpperCaseStyle);
                        }

                        break;
                    case "boolLit":
                    case "number":
                        tr.SetStyle(SyntaxHighlighter.NumberStyle);
                        break;
                    case "string":
                    case "escaped_string":
                        tr.SetStyle(SyntaxHighlighter.StringStyle);
                        break;
                    case "comment":
                    case "line_comment":
                        if (insertPoint.HasValue && tr.End.iLine == insertPoint.Value.iLine && tr.End.iChar >= insertPoint.Value.iChar)
                        {
                            tr = GetRange(new Place(tr.Start.iChar, tr.Start.iLine), new Place(tr.End.iChar - 1, tr.End.iLine));
                        }
                        tr.SetStyle(SyntaxHighlighter.CommentStyle);
                        break;
                    case "id_simple":
                        if (nodoAst != null && nodoAst.Parent != null && nodoAst.Parent.Parent != null && nodoAst.Parent.Parent.Name == "funCall")
                        {
                            tr.SetStyle(SyntaxHighlighter.FunctionsStyle);
                        }
                        else
                        {
                            tr.SetStyle(SyntaxHighlighter.VariableStyle);
                        }
                        break;
                    case "":
                        tr.SetStyle(SyntaxHighlighter.VariableStyle);
                        break;
                    default:
                        if (nodoAst != null && (nodoAst.Name == "typeParamsOpt" || nodoAst.GetRecursiveParentNamedAs("typeName") != null))
                        {
                            tr.SetStyle(SyntaxHighlighter.TypesStyle);
                        }
                        else if (t.Terminal.GetType().Name == "KeyTerm")
                        {
                            if ((t.Terminal.Flags & TermFlags.IsKeyword) != 0)
                            {
                                if (nodoAst != null && nodoAst.IsStatementHeader)
                                {
                                    tr.SetStyle(KeywordBoldUpperCaseStyle);
                                }
                                else
                                {
                                    tr.SetStyle(KeywordUpperCaseStyle);
                                }
                            }
                        }
                        break;
                }
            }

            if (ParseTreeUpdated != null) ParseTreeUpdated(this, new EventArgs());
        }

        public virtual void OnStyleNeeded(StyleNeededEventArgs e)
        {
            if (StyleNeeded != null)
                StyleNeeded(this, e);
        }

        public FastColoredTextBoxNS.Range GetTokenRange(Token t)
        {
            var loc = t.Location;

            var place = new Place(loc.Column, loc.Line);
            var r = new FastColoredTextBoxNS.Range(this, place, place);

            foreach (var c in t.Text)
                if (c != '\r')
                    r.GoRight(true);

            return r;
        }

        protected virtual void InitBraces()
        {
            LeftBracket = '\x0';
            RightBracket = '\x0';
            LeftBracket2 = '\x0';
            RightBracket2 = '\x0';

            var braces = parser!.Language.Grammar.KeyTerms
              .Select(pair => pair.Value)
              .Where(term => term.Flags.IsSet(TermFlags.IsOpenBrace))
              .Where(term => term.IsPairFor != null && term.IsPairFor is KeyTerm)
              .Where(term => term.Text.Length == 1)
              .Where(term => ((KeyTerm)term.IsPairFor).Text.Length == 1)
              .Take(2);
            if (braces.Any())
            {
                var brace = braces.First();
                LeftBracket = brace.Text.First();
                RightBracket = ((KeyTerm)brace.IsPairFor).Text.First();

                if (braces.Count() > 1)
                {
                    brace = braces.Last();
                    LeftBracket2 = brace.Text.First();
                    RightBracket2 = ((KeyTerm)brace.IsPairFor).Text.First();
                }
            }
            else
            {
                LeftBracket = '(';
                RightBracket = ')';
            }
        }
    }

    public class StyleNeededEventArgs : EventArgs
    {
        public readonly Token Token;
        public bool Cancel { get; set; }
        public Style? Style { get; set; }

        public StyleNeededEventArgs(Token t)
        {
            Token = t;
        }
    }
}

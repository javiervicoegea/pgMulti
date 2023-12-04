using System.Diagnostics;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using Irony.Parsing;
using PgMulti.SqlSyntax;

namespace PgMulti.QueryEditor
{
    public class CustomFctb : FastColoredTextBox
    {
        public event EventHandler<StyleNeededEventArgs>? StyleNeeded;
        public event EventHandler<EventArgs>? ParseTreeUpdated;

        public Style WavyLineStyle;
        public Style FunctionsTextStyle;
        public Style StringTextStyle;
        public Style CommentTextStyle;
        public Style NumberTextStyle;
        public Style VariableTextStyle;
        public Style KeywordCaseTextStyle;
        public Style KeywordHeaderCaseTextStyle;
        public Style TypesCaseStyle;
        public Style SearchMatchTextStyle;

        private Parser? parser;
        private ParseTree? _ParseTree = null;
        private List<FastColoredTextBoxNS.Range>? _SearchMatches = null;

        public ParseTree? ParseTree
        {
            get
            {
                return _ParseTree;
            }
        }

        public List<FastColoredTextBoxNS.Range>? SearchMatches
        {
            get
            {
                return _SearchMatches;
            }
            set
            {
                _SearchMatches = value;
            }
        }

        public CustomFctb()
        {
            ClearStylesBuffer();
            AddStyle(WavyLineStyle);
            AddStyle(FunctionsTextStyle);
            AddStyle(StringTextStyle);
            AddStyle(CommentTextStyle);
            AddStyle(NumberTextStyle);
            AddStyle(VariableTextStyle);
            AddStyle(KeywordCaseTextStyle);
            AddStyle(KeywordHeaderCaseTextStyle);
            AddStyle(TypesCaseStyle);
            AddStyle(SearchMatchTextStyle);

            WavyLineStyle = new WavyLineStyle(255, Color.Red);
            FunctionsTextStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Bold);
            StringTextStyle = SyntaxHighlighter.BrownStyle;
            CommentTextStyle = SyntaxHighlighter.GreenStyle;
            NumberTextStyle = SyntaxHighlighter.MagentaStyle;
            VariableTextStyle = SyntaxHighlighter.BlackStyle;
            KeywordCaseTextStyle = new CaseTextStyle(Brushes.Blue, null, FontStyle.Regular, true);
            KeywordHeaderCaseTextStyle = new CaseTextStyle(Brushes.Blue, null, FontStyle.Bold, true);
            TypesCaseStyle = new CaseTextStyle(Brushes.Brown, null, FontStyle.Italic, false);
            SearchMatchTextStyle = new TextStyle(Brushes.Black, Brushes.Yellow, FontStyle.Bold);

            //SyntaxHighlighter.InitStyleSchema(Language.Custom);
            //SyntaxHighlighter.StringStyle = SyntaxHighlighter.BrownStyle;
            //SyntaxHighlighter.CommentStyle = SyntaxHighlighter.GreenStyle;
            //SyntaxHighlighter.NumberStyle = SyntaxHighlighter.MagentaStyle;
            //SyntaxHighlighter.FunctionsStyle = FunctionsStyle;
            //SyntaxHighlighter.VariableStyle = SyntaxHighlighter.BlackStyle;
        }

        public virtual void SetParser(LanguageData language)
        {
            this.parser = new Parser(language);

            InitBraces();
            OnTextChanged(Range);
        }

        public override void OnTextChangedDelayed(FastColoredTextBoxNS.Range changedRange)
        {
            base.OnTextChangedDelayed(changedRange);
            DoHighlighting();
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
                    f.SetStyle(WavyLineStyle);
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
                            tr.SetStyle(NumberTextStyle);
                        }
                        else
                        {
                            tr.SetStyle(KeywordCaseTextStyle);
                        }

                        break;
                    case "boolLit":
                    case "number":
                        tr.SetStyle(NumberTextStyle);
                        break;
                    case "string":
                    case "escaped_string":
                        tr.SetStyle(StringTextStyle);
                        break;
                    case "comment":
                    case "line_comment":
                        if (insertPoint.HasValue && tr.End.iLine == insertPoint.Value.iLine && tr.End.iChar >= insertPoint.Value.iChar)
                        {
                            tr = GetRange(new Place(tr.Start.iChar, tr.Start.iLine), new Place(tr.End.iChar - 1, tr.End.iLine));
                        }
                        tr.SetStyle(CommentTextStyle);
                        break;
                    case "id_simple":
                        if (nodoAst != null && nodoAst.Parent != null && nodoAst.Parent.Parent != null && nodoAst.Parent.Parent.Name == "funCall")
                        {
                            tr.SetStyle(FunctionsTextStyle);
                        }
                        else
                        {
                            tr.SetStyle(VariableTextStyle);
                        }
                        break;
                    case "":
                        tr.SetStyle(VariableTextStyle);
                        break;
                    default:
                        if (nodoAst != null && nodoAst.GetRecursiveParentNamedAs("typeNameAndParams") != null)
                        {
                            tr.SetStyle(TypesCaseStyle);
                        }
                        else if (t.Terminal.GetType().Name == "KeyTerm")
                        {
                            if ((t.Terminal.Flags & TermFlags.IsKeyword) != 0)
                            {
                                if (nodoAst != null && nodoAst.IsStatementHeader)
                                {
                                    tr.SetStyle(KeywordHeaderCaseTextStyle);
                                }
                                else
                                {
                                    tr.SetStyle(KeywordCaseTextStyle);
                                }
                            }
                        }
                        break;
                }
            }

            if (_SearchMatches != null)
            {
                foreach (FastColoredTextBoxNS.Range r in _SearchMatches)
                {
                    r.ClearStyle(StyleIndex.All);
                    r.SetStyle(SearchMatchTextStyle);
                }
            }

            if (ParseTreeUpdated != null) ParseTreeUpdated(this, new EventArgs());
        }

        public List<FastColoredTextBoxNS.Range> FindAll(string pattern, bool matchCase, bool matchWholeWords, bool regex, FastColoredTextBoxNS.Range wholeSearchRange)
        {
            RegexOptions opt = matchCase ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (!regex)
                pattern = Regex.Escape(pattern);
            if (matchWholeWords)
                pattern = "\\b" + pattern + "\\b";

            try
            {
                return wholeSearchRange.GetRangesByLines(pattern, opt).ToList();
            }
            catch (RegexParseException)
            {

                return new List<FastColoredTextBoxNS.Range>();
            }
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

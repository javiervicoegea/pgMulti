﻿using FastColoredTextBoxNS;

namespace PgMulti.QueryEditor
{
    public class UpperCaseTextStyle : TextStyle
    {

        public UpperCaseTextStyle(Brush foreBrush, Brush? backgroundBrush, FontStyle fontStyle) : base(foreBrush, backgroundBrush, fontStyle)
        {
        }

        public override void Draw(Graphics gr, Point position, FastColoredTextBoxNS.Range range)
        {
            //draw background
            if (BackgroundBrush != null)
                gr.FillRectangle(BackgroundBrush, position.X, position.Y, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth, range.tb.CharHeight);
            //draw chars
            using (var f = new Font(range.tb.Font, FontStyle))
            {
                Line line = range.tb[range.Start.iLine];
                float dx = range.tb.CharWidth;
                float y = position.Y + range.tb.LineInterval / 2;
                float x = position.X - range.tb.CharWidth / 3;

                if (ForeBrush == null)
                    ForeBrush = new SolidBrush(range.tb.ForeColor);

                if (range.tb.ImeAllowed)
                {
                    //IME mode
                    for (int i = range.Start.iChar; i < range.End.iChar; i++)
                    {
                        SizeF size = FastColoredTextBox.GetCharSize(f, line[i].c);

                        var gs = gr.Save();
                        float k = size.Width > range.tb.CharWidth + 1 ? range.tb.CharWidth / size.Width : 1;
                        gr.TranslateTransform(x, y + (1 - k) * range.tb.CharHeight / 2);
                        gr.ScaleTransform(k, (float)Math.Sqrt(k));
                        gr.DrawString(line[i].c.ToString().ToUpper(), f, ForeBrush, 0, 0, stringFormat);
                        gr.Restore(gs);
                        x += dx;
                    }
                }
                else
                {
                    //classic mode 
                    for (int i = range.Start.iChar; i < range.End.iChar; i++)
                    {
                        //draw char
                        gr.DrawString(line[i].c.ToString().ToUpper(), f, ForeBrush, x, y, stringFormat);
                        x += dx;
                    }
                }
            }
        }
    }
}

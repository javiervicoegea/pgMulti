using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.QueryEditor
{

    public class CustomVerticalScrollBar : Control
    {
        private int @value;

        private CustomFctb tb;

        public int Value
        {
            get { return value; }
            set
            {
                if (this.value == value)
                    return;
                this.value = value;
                //Invalidate();
                Refresh();
                OnScroll();
            }
        }

        private int maximum = 100;
        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; Invalidate(); }
        }

        private int _ThumbSize = 10;
        public int ThumbSize
        {
            get { return _ThumbSize; }
            set { _ThumbSize = value; Invalidate(); }
        }

        private int _FirstLineY = 0;
        public int FirstLineY
        {
            get { return _FirstLineY; }
            set { _FirstLineY = value; Invalidate(); }
        }

        private float _LineHeight = 5.0f;
        public float LineHeight
        {
            get { return _LineHeight; }
            set { _LineHeight = value; Invalidate(); }
        }



        private Color thumbColor = Color.Gray;
        public Color ThumbColor
        {
            get { return thumbColor; }
            set { thumbColor = value; Invalidate(); }
        }

        private Color errorColor = Color.Gray;
        public Color ErrorColor
        {
            get { return errorColor; }
            set { errorColor = value; Invalidate(); }
        }

        private Color commentColor = Color.Gray;
        public Color CommentColor
        {
            get { return commentColor; }
            set { commentColor = value; Invalidate(); }
        }

        private Color searchMatchColor = Color.Gray;
        public Color SearchMatchColor
        {
            get { return searchMatchColor; }
            set { searchMatchColor = value; Invalidate(); }
        }

        private Color borderColor = Color.Silver;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        public event ScrollEventHandler? Scroll = null;

        public CustomVerticalScrollBar(CustomFctb tb)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            this.tb = tb;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseScroll(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseScroll(e);
                Refresh();
            }
            base.OnMouseMove(e);
        }

        private void MouseScroll(MouseEventArgs e)
        {
            int v = Maximum * (e.Y - _ThumbSize / 2) / (Height - _ThumbSize);
            Value = Math.Max(0, Math.Min(Maximum, v));
        }

        public virtual void OnScroll(ScrollEventType type = ScrollEventType.ThumbPosition)
        {
            if (Scroll != null)
                Scroll(this, new ScrollEventArgs(type, Value, ScrollOrientation.VerticalScroll));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Maximum <= 0)
                return;

            Rectangle thumbRect = new Rectangle(2, 1 + (value * (Height - 2 - _ThumbSize)) / Maximum, Width - 4, _ThumbSize);

            using (var brush = new SolidBrush(thumbColor))
                e.Graphics.FillRectangle(brush, thumbRect);



            Rectangle r = new Rectangle(4, 0, Width - 8, 0);

            DrawMarkList(e.Graphics, tb.CommentLines, _FirstLineY, _LineHeight, commentColor, r);
            DrawMarkList(e.Graphics, tb.ErrorLines, _FirstLineY, _LineHeight, errorColor, r);
            DrawMarkList(e.Graphics, tb.SearchMatchLines, _FirstLineY, _LineHeight, searchMatchColor, r);

            using (var pen = new Pen(borderColor))
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        private void DrawMarkList(Graphics g, SortedList<int, int> list, int firstLineY, float lineHeight, Color c, Rectangle baseRectangle)
        {
            for (int i = 0; i < list.Count;)
            {
                int ini = list.GetValueAtIndex(i);
                int fin = ini;
                i++;
                for (; i < list.Count && list.GetValueAtIndex(i) == fin + 1; i++)
                {
                    fin++;
                }

                baseRectangle.Y = Math.Min(firstLineY + (int)(ini * lineHeight), Height - 6);
                baseRectangle.Height = Math.Max(5, (int)((fin - ini + 1) * lineHeight));

                using (var brush = new SolidBrush(c))
                    g.FillRectangle(brush, baseRectangle);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams
{
    public abstract class DiagramObject
    {
        protected Diagram _Diagram;
        protected Rectangle _BoundingBox;
        protected bool _Highlighted = false;
        protected bool _Selected = false;
        protected bool _Suggested = false;
        protected Point? _StartDraggingPoint = null;
        protected Point? _CurrentDraggingPoint = null;
        protected int _ZIndex = -1;

        protected DiagramObject(Diagram diagram)
        {
            _Diagram = diagram;
        }

        public int ZIndex
        {
            get
            {
                return _ZIndex;
            }

            internal set
            {
                _ZIndex = value;
            }
        }

        public bool Suggested
        {
            get => _Suggested;
            set => _Suggested = value;
        }

        public Diagram Diagram
        {
            get
            {
                return _Diagram;
            }
        }

        public static Rectangle AddMargins(Rectangle r, int margin)
        {
            return AddMargins(r, margin, margin, margin, margin);
        }

        public static Rectangle AddMargins(Rectangle r, int topMargin, int rightMargin, int bottomMargin, int leftMargin)
        {
            r.X -= leftMargin;
            r.Y -= topMargin;
            r.Width += leftMargin + rightMargin;
            r.Height += topMargin + bottomMargin;

            return r;
        }

        private static float _Sqr(int X) { return X * X; }
        public static float CalculateDistanceToPointSquared(Point v, Point w) { return _Sqr(v.X - w.X) + _Sqr(v.Y - w.Y); }
        public static float CalculateDistanceToSegmentSquared(Point p, Point v, Point w)
        {
            float l2 = CalculateDistanceToPointSquared(v, w);
            if (l2 == 0) return CalculateDistanceToPointSquared(p, v);
            float t = ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2;
            t = Math.Max(0, Math.Min(1, t));

            Point p2 = new Point((int)(v.X + t * (w.X - v.X)), (int)(v.Y + t * (w.Y - v.Y)));

            return CalculateDistanceToPointSquared(p, p2);
        }

        protected DiagramObject()
        {
            _BoundingBox = new Rectangle();
        }

        public Rectangle BoundingBox { get { return _BoundingBox; } }

        public bool Dragging
        {
            get
            {
                return _CurrentDraggingPoint.HasValue && _StartDraggingPoint.HasValue;
            }
        }

        public Rectangle? DraggingBoundingBox
        {
            get
            {
                if (!Dragging) return null;

                Rectangle r = BoundingBox;
                r.X += _CurrentDraggingPoint!.Value.X - _StartDraggingPoint!.Value.X;
                r.Y += _CurrentDraggingPoint.Value.Y - _StartDraggingPoint.Value.Y;

                return r;
            }
        }

        public bool Highlighted { get { return _Highlighted; } set { _Highlighted = value; } }
        public bool Selected { get { return _Selected; } set { _Selected = value; } }

        public virtual bool IntersectsWith(Point p)
        {
            //if (DraggingBoundingBox.HasValue) return DraggingBoundingBox.Value.Contains(p);
            return _BoundingBox.Contains(p);
        }

        public virtual bool IntersectsWith(Rectangle r)
        {
            //if (DraggingBoundingBox.HasValue) return DraggingBoundingBox.Value.IntersectsWith(r);
            return _BoundingBox.IntersectsWith(r);
        }

        public abstract bool IsSelectable(Point p);
        public abstract float SelectingRank(Point p);

        public abstract float SelectingTolerance { get; }

        public abstract void Draw(Graphics g);

        public virtual void SetStartDraggingPoint(Point p)
        {
            _StartDraggingPoint = p;
        }

        public virtual void MoveDrag(Point p)
        {
            _CurrentDraggingPoint = p;
        }

        public virtual void CompleteDrag(Point p)
        {
            if (!_StartDraggingPoint.HasValue) throw new NotSupportedException();

            _BoundingBox.X += p.X - _StartDraggingPoint.Value.X;
            _BoundingBox.Y += p.Y - _StartDraggingPoint.Value.Y;
            _StartDraggingPoint = null;
            _CurrentDraggingPoint = null;
        }

        public virtual void MoveTo(Point p)
        {
            _BoundingBox.X = p.X - _BoundingBox.Width / 2;
            _BoundingBox.Y = p.Y - _BoundingBox.Height / 2;
        }

        protected static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}

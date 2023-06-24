using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams
{
    public class DrawableRelationOption
    {
        internal Point Point1;
        internal Point Point2;
        internal int SquaredDistance;
        internal SideEnum Side1;
        internal SideEnum Side2;
        internal bool Overlaps;

        public DrawableRelationOption(DiagramTable table1, DiagramTable table2, SideEnum side1, SideEnum side2, int margin1, int margin2)
        {
            Point1 = CalculateSideCentralPoint(table1, side1, margin1);
            Point2 = CalculateSideCentralPoint(table2, side2, margin2);
            Side1 = side1;
            Side2 = side2;

            int width = Point1.X - Point2.X;
            int height = Point1.Y - Point2.Y;
            SquaredDistance = width * width + height * height;

            Overlaps = false;

            switch (side1)
            {
                case SideEnum.Top:
                    Overlaps |= Point2.Y > Point1.Y;
                    break;
                case SideEnum.Right:
                    Overlaps |= Point2.X < Point1.X;
                    break;
                case SideEnum.Bottom:
                    Overlaps |= Point2.Y < Point1.Y;
                    break;
                case SideEnum.Left:
                    Overlaps |= Point2.X > Point1.X;
                    break;
                default:
                    throw new NotSupportedException();
            }

            switch (side2)
            {
                case SideEnum.Top:
                    Overlaps |= Point2.Y < Point1.Y;
                    break;
                case SideEnum.Right:
                    Overlaps |= Point2.X > Point1.X;
                    break;
                case SideEnum.Bottom:
                    Overlaps |= Point2.Y > Point1.Y;
                    break;
                case SideEnum.Left:
                    Overlaps |= Point2.X < Point1.X;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        internal static Point CalculateSideCentralPoint(DiagramTable dt, SideEnum side, int margin)
        {
            switch (side)
            {
                case SideEnum.Left:
                    return new Point(dt.Center.X - dt.BoundingBox.Width / 2 - margin, dt.Center.Y); ;
                case SideEnum.Right:
                    return new Point(dt.Center.X + dt.BoundingBox.Width / 2 + margin, dt.Center.Y);
                case SideEnum.Top:
                    return new Point(dt.Center.X, dt.Center.Y - dt.BoundingBox.Height / 2 - margin);
                case SideEnum.Bottom:
                    return new Point(dt.Center.X, dt.Center.Y + dt.BoundingBox.Height / 2 + margin);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

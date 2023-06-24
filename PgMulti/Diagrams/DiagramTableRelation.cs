using PgMulti.DataStructure;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Security.Policy;
using System.Windows.Forms;
using System.Xml;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace PgMulti.Diagrams
{
    public class DiagramTableRelation : DiagramObject
    {
        private const int IntersectTolerance = 25;
        private const int BoundingBoxTolerance = 50;
        private const int IntersectToleranceSquared = IntersectTolerance * IntersectTolerance;
        private const int RelationTypeIconSize1Width = 25;
        private const int RelationTypeIconSize2Width = 46;
        private const int RelationTypeIconSizeHeight = 25;
        private const int UnaryRelationSize = 50;
        private const int UnaryRelationBoundingBoxMargin = 100;

        private DiagramTable _ParentTable;
        private DiagramTable _ChildTable;
        private List<DiagramColumn> _ParentTableColumns;
        private List<DiagramColumn> _ChildTableColumns;
        private PropagationOptions _OnDelete;
        private PropagationOptions _OnUpdate;

        private RelationTypeOptions _ParentRelationType;
        private RelationTypeOptions _ChildRelationType;

        private static Pen _NormalLinePen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 2);
        private static Pen _HighlightedLineMainPen = new Pen(new SolidBrush(Color.FromArgb(255, 200, 70, 246)), 6);
        private static Pen _HighlightedLineSecondaryPen = new Pen(new SolidBrush(Color.FromArgb(255, 200, 70, 246)), 2);
        private static Pen _SelectedLinePen = new Pen(new SolidBrush(Color.FromArgb(255, 58, 136, 254)), 6);
        private static Pen _HighlightedBorderPen = new Pen(new SolidBrush(Color.FromArgb(255, 92, 33, 104)), 2);
        private static Pen _SelectedBorderPen = new Pen(new SolidBrush(Color.FromArgb(255, 36, 55, 118)), 2);
        private static Brush _FillSideBrush = new SolidBrush(Color.White);
        private static Brush _HighlightedTextBrush = new SolidBrush(Color.FromArgb(255, 92, 33, 104));
        private static Brush _SelectedTextBrush = new SolidBrush(Color.FromArgb(255, 36, 55, 118));
        private static Brush _TextBackgroundBrush = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
        private static Pen _SuggestedLinePen = new Pen(new SolidBrush(Color.FromArgb(255, 100, 100, 100)), 4);

        private static Font idFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        private Point? _ParentPoint = null;
        private Point? _ChildPoint = null;
        private SideEnum? _ParentSide = null;
        private SideEnum? _ChildSide = null;
        private Point[]? _BezierCurveDefinitionPoints = null;
        private Point[]? _BezierCurveRealPoints = null;
        private Rectangle? _InteractBoundingBox = null;
        private string _Id;

        public DiagramTableRelation(Diagram diagram, string id, DiagramTable parentTable, DiagramTable childTable, List<DiagramColumn> parentTableColumns, List<DiagramColumn> childTableColumns, PropagationOptions onDelete, PropagationOptions onUpdate) : base(diagram)
        {
            _Id = id;
            _ParentTable = parentTable;
            _ChildTable = childTable;
            _ParentTableColumns = parentTableColumns;
            _ChildTableColumns = childTableColumns;

            foreach (DiagramColumn c in _ChildTableColumns)
            {
                c.ForeignKey = true;
            }

            _OnDelete = onDelete;
            _OnUpdate = onUpdate;

            CalculateRelationType();
        }

        public DiagramTableRelation(Diagram diagram, DiagramTable parentTable, DiagramTable childTable, TableRelation r) : base(diagram)
        {
            _Id = r.Id;
            _ParentTable = parentTable;
            _ChildTable = childTable;

            _ParentTableColumns = new List<DiagramColumn>();
            foreach (string columnName in r.ParentColumns)
            {
                _ParentTableColumns.Add(_ParentTable.Columns.First(ci => ci.ColumnName == columnName));
            }

            _ChildTableColumns = new List<DiagramColumn>();
            foreach (string columnName in r.ChildColumns)
            {
                DiagramColumn c = _ChildTable.Columns.First(ci => ci.ColumnName == columnName);
                _ChildTableColumns.Add(c);
                c.ForeignKey = true;
            }

            _OnDelete = StringToPropagationOptions(r.OnDelete);
            _OnUpdate = StringToPropagationOptions(r.OnUpdate);

            CalculateRelationType();
        }

        public DiagramTableRelation(Diagram diagram, DiagramTable parentTable, DiagramTable childTable, XmlElement xeRelation) : base(diagram)
        {
            string? v;

            _ParentTable = parentTable;
            _ChildTable = childTable;

            _ParentTableColumns = new List<DiagramColumn>();
            foreach (XmlElement xeColumn in xeRelation.SelectNodes("parent_columns/column")!)
            {
                v = xeColumn.GetAttribute("column_name");
                if (string.IsNullOrEmpty(v)) throw new BadFormatException();

                _ParentTableColumns.Add(_ParentTable.Columns.First(ci => ci.ColumnName == v));
            }

            _ChildTableColumns = new List<DiagramColumn>();
            foreach (XmlElement xeColumn in xeRelation.SelectNodes("child_columns/column")!)
            {
                v = xeColumn.GetAttribute("column_name");
                if (string.IsNullOrEmpty(v)) throw new BadFormatException();

                DiagramColumn c = _ChildTable.Columns.First(ci => ci.ColumnName == v);
                _ChildTableColumns.Add(c);
                c.ForeignKey = true;
            }

            _Id = xeRelation.GetAttribute("id");

            v = xeRelation.GetAttribute("on_delete");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();
            _OnDelete = StringToPropagationOptions(v);

            v = xeRelation.GetAttribute("on_update");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();
            _OnUpdate = StringToPropagationOptions(v);

            CalculateRelationType();
        }

        public string Id { get { return _Id; } }

        public DiagramTable ParentTable { get => _ParentTable; set => _ParentTable = value; }
        public DiagramTable ChildTable { get => _ChildTable; set => _ChildTable = value; }
        public List<DiagramColumn> ParentTableColumns { get => _ParentTableColumns; set => _ParentTableColumns = value; }
        public List<DiagramColumn> ChildTableColumns { get => _ChildTableColumns; set => _ChildTableColumns = value; }
        public PropagationOptions OnDelete { get => _OnDelete; set => _OnDelete = value; }
        public PropagationOptions OnUpdate { get => _OnUpdate; set => _OnUpdate = value; }

        public SideEnum? ParentSide
        {
            get
            {
                return _ParentSide;
            }
        }

        public SideEnum? ChildSide
        {
            get
            {
                return _ChildSide;
            }
        }

        public Point? ParentPoint
        {
            get
            {
                return _ParentPoint;
            }

            set
            {
                _ParentPoint = value;
            }
        }

        public Point? ChildPoint
        {
            get
            {
                return _ChildPoint;
            }

            set
            {
                _ChildPoint = value;
            }
        }

        // Horizontal slope (left to right) seen from Point1 to Point2
        public float ParentHSlope
        {
            get
            {
                if (_ChildPoint!.Value.X == _ParentPoint!.Value.X) return _ParentPoint!.Value.Y > _ChildPoint!.Value.Y ? float.MaxValue : float.MinValue;
                return ((float)_ParentPoint!.Value.Y - _ChildPoint!.Value.Y) / (_ChildPoint!.Value.X - _ParentPoint!.Value.X);
            }
        }

        // Horizontal slope (left to right) seen from Point2 to Point1
        public float ChildHSlope
        {
            get
            {
                if (_ChildPoint!.Value.X == _ParentPoint!.Value.X) return _ChildPoint!.Value.Y > _ParentPoint!.Value.Y ? float.MaxValue : float.MinValue;
                return ((float)_ChildPoint!.Value.Y - _ParentPoint!.Value.Y) / (_ParentPoint!.Value.X - _ChildPoint!.Value.X);
            }
        }

        // Vertical slope (top to bottom) seen from Point1 to Point2
        public float ParentVSlope
        {
            get
            {
                if (_ChildPoint!.Value.Y == _ParentPoint!.Value.Y) return _ChildPoint!.Value.X > _ParentPoint!.Value.X ? float.MaxValue : float.MinValue;
                return ((float)_ChildPoint!.Value.X - _ParentPoint!.Value.X) / (_ChildPoint!.Value.Y - _ParentPoint!.Value.Y);
            }
        }

        // Vertical slope (top to bottom) seen from Point2 to Point1
        public float ChildVSlope
        {
            get
            {
                if (_ChildPoint!.Value.Y == _ParentPoint!.Value.Y) return _ParentPoint!.Value.X > _ChildPoint!.Value.X ? float.MaxValue : float.MinValue;
                return ((float)_ParentPoint!.Value.X - _ChildPoint!.Value.X) / (_ParentPoint!.Value.Y - _ChildPoint!.Value.Y);
            }
        }

        public int ParentRelationTypeIconWidth
        {
            get
            {
                return GetRelationTypeWidth(_ParentRelationType);
            }
        }

        public int ParentRelationTypeIconHeight
        {
            get
            {
                return GetRelationTypeHeight(_ParentRelationType);
            }
        }

        public int ChildRelationTypeIconWidth
        {
            get
            {
                return GetRelationTypeWidth(_ChildRelationType);
            }
        }

        public int ChildRelationTypeIconHeight
        {
            get
            {
                return GetRelationTypeHeight(_ChildRelationType);
            }
        }

        public XmlElement ToXml(XmlDocument xd)
        {
            XmlElement xeRelation = xd.CreateElement("relation");

            xeRelation.SetAttribute("id", Id);
            xeRelation.SetAttribute("parent_schema_name", ParentTable.SchemaName);
            xeRelation.SetAttribute("parent_table_name", ParentTable.TableName);
            xeRelation.SetAttribute("child_schema_name", ChildTable.SchemaName);
            xeRelation.SetAttribute("child_table_name", ChildTable.TableName);
            xeRelation.SetAttribute("on_delete", PropagationOptionsToString(OnDelete));
            xeRelation.SetAttribute("on_update", PropagationOptionsToString(OnUpdate));

            XmlElement xeParentColumns = xd.CreateElement("parent_columns");
            xeRelation.AppendChild(xeParentColumns);
            foreach (DiagramColumn dc in ParentTableColumns)
            {
                XmlElement xeColumn = xd.CreateElement("column");
                xeParentColumns.AppendChild(xeColumn);

                xeColumn.SetAttribute("column_name", dc.ColumnName);
            }

            XmlElement xeChildColumns = xd.CreateElement("child_columns");
            xeRelation.AppendChild(xeChildColumns);
            foreach (DiagramColumn dc in ChildTableColumns)
            {
                XmlElement xeColumn = xd.CreateElement("column");
                xeChildColumns.AppendChild(xeColumn);

                xeColumn.SetAttribute("column_name", dc.ColumnName);
            }

            return xeRelation;
        }

        private Image GetHighlightedPropagationIcon(PropagationOptions o)
        {
            switch (o)
            {
                case PropagationOptions.NoAction:
                case PropagationOptions.Restrict:
                    return Properties.DiagramIcons.highlighted_restrict;
                case PropagationOptions.Cascade:
                    return Properties.DiagramIcons.highlighted_cascade;
                case PropagationOptions.SetNull:
                    return Properties.DiagramIcons.highlighted_set_null;
                case PropagationOptions.SetDefault:
                    return Properties.DiagramIcons.highlighted_set_default;
                default:
                    throw new NotSupportedException();
            }
        }

        private Image GetSelectedPropagationIcon(PropagationOptions o)
        {
            switch (o)
            {
                case PropagationOptions.NoAction:
                case PropagationOptions.Restrict:
                    return Properties.DiagramIcons.selected_restrict;
                case PropagationOptions.Cascade:
                    return Properties.DiagramIcons.selected_cascade;
                case PropagationOptions.SetNull:
                    return Properties.DiagramIcons.selected_set_null;
                case PropagationOptions.SetDefault:
                    return Properties.DiagramIcons.selected_set_default;
                default:
                    throw new NotSupportedException();
            }
        }

        private void DrawId(Graphics g, Pen pen, Brush br, Point refPoint)
        {
            SizeF s = g.MeasureString(Id, idFont);

            int w = (int)Math.Round(s.Width);
            int h = (int)Math.Round(s.Height);
            int x = refPoint.X - w / 2;
            int y = refPoint.Y - h - 23;

            Rectangle r = new Rectangle(x - 2, y - 2, w + 4, h + 4);

            g.FillPath(_TextBackgroundBrush, RoundedRect(r, 5));
            g.DrawPath(pen, RoundedRect(r, 5));
            g.DrawString(Id, idFont, br, x + 2, y);
        }

        public override void Draw(Graphics g)
        {
            if (ParentTable.Dragging || ChildTable.Dragging) return;

            Pen pen;

            Point[] triangle;
            int middleIndex = _BezierCurveRealPoints!.Length / 2;
            Point p1 = _BezierCurveRealPoints![middleIndex - 1];
            Point p2 = _BezierCurveRealPoints![middleIndex];
            Point p3 = _BezierCurveRealPoints![middleIndex + 1];

            if (ParentTable == ChildTable)
            {
                // Hack to fit the triangle to the line

                //g.DrawLine(_NormalLinePen, new Point(p2.X - 5, p2.Y - 5), new Point(p2.X + 5, p2.Y + 5));
                //g.DrawLine(_NormalLinePen, new Point(p2.X - 5, p2.Y + 5), new Point(p2.X + 5, p2.Y - 5));

                switch (ParentSide)
                {
                    case SideEnum.Top:
                        p2.Y += 1;
                        break;
                    case SideEnum.Right:
                        p2.X -= 1;
                        break;
                    case SideEnum.Bottom:
                        p2.Y -= 1;
                        break;
                    case SideEnum.Left:
                        p2.X += 1;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            float angle = -(float)((Math.Atan2(p1.X - p3.X, p1.Y - p3.Y) * 180.0) / Math.PI);

            Matrix m = new Matrix();
            m.RotateAt(angle, new PointF(p2.X, p2.Y));

            if (_Highlighted)
            {
                pen = _HighlightedLineMainPen;
                triangle = CreateTriangle(p2, 25, 30);
            }
            else if (Suggested)
            {
                pen = _SuggestedLinePen;
                triangle = CreateTriangle(p2, 15, 20);
            }
            else if (ParentTable.Highlighted || ChildTable.Highlighted)
            {
                pen = _HighlightedLineSecondaryPen;
                triangle = CreateTriangle(p2, 25, 30);
            }
            else if (_Selected)
            {
                pen = _SelectedLinePen;
                triangle = CreateTriangle(p2, 25, 30);
            }
            else
            {
                pen = _NormalLinePen;
                triangle = CreateTriangle(p2, 15, 20);
            }

            g.DrawBeziers(pen, _BezierCurveDefinitionPoints!);
            m.TransformPoints(triangle);
            g.FillPolygon(pen.Brush, triangle);

            DrawParentSide(g, pen, _FillSideBrush);
            DrawChildSide(g, pen, _FillSideBrush);

            //for (int i = 0; i < _BezierCurveRealPoints!.Length - 1; i++)
            //{
            //    g.DrawLine(pen, _BezierCurveRealPoints[i], _BezierCurveRealPoints[i + 1]);
            //}

            if (_Highlighted)
            {
                DrawId(g, _HighlightedBorderPen, _HighlightedTextBrush, p2);

                g.FillPath(_TextBackgroundBrush, RoundedRect(new Rectangle(p2.X - 72, p2.Y + 16, 59, 29), 5));
                g.DrawPath(_HighlightedBorderPen, RoundedRect(new Rectangle(p2.X - 72, p2.Y + 16, 59, 29), 5));
                g.DrawImage(Properties.DiagramIcons.highlighted_on_delete, p2.X - 70, p2.Y + 18, 25, 25);
                g.DrawImage(GetHighlightedPropagationIcon(OnDelete), p2.X - 40, p2.Y + 18, 25, 25);

                g.FillPath(_TextBackgroundBrush, RoundedRect(new Rectangle(p2.X + 13, p2.Y + 16, 59, 29), 5));
                g.DrawPath(_HighlightedBorderPen, RoundedRect(new Rectangle(p2.X + 13, p2.Y + 16, 59, 29), 5));
                g.DrawImage(Properties.DiagramIcons.highlighted_on_update, p2.X + 15, p2.Y + 18, 25, 25);
                g.DrawImage(GetHighlightedPropagationIcon(OnUpdate), p2.X + 45, p2.Y + 18, 25, 25);
            }
            else if (_Selected)
            {
                DrawId(g, _SelectedBorderPen, _SelectedTextBrush, p2);

                g.FillPath(_TextBackgroundBrush, RoundedRect(new Rectangle(p2.X - 72, p2.Y + 16, 59, 29), 5));
                g.DrawPath(_SelectedBorderPen, RoundedRect(new Rectangle(p2.X - 72, p2.Y + 16, 59, 29), 5));
                g.DrawImage(Properties.DiagramIcons.selected_on_delete, p2.X - 70, p2.Y + 18, 25, 25);
                g.DrawImage(GetSelectedPropagationIcon(OnDelete), p2.X - 40, p2.Y + 18, 25, 25);

                g.FillPath(_TextBackgroundBrush, RoundedRect(new Rectangle(p2.X + 13, p2.Y + 16, 59, 29), 5));
                g.DrawPath(_SelectedBorderPen, RoundedRect(new Rectangle(p2.X + 13, p2.Y + 16, 59, 29), 5));
                g.DrawImage(Properties.DiagramIcons.selected_on_update, p2.X + 15, p2.Y + 18, 25, 25);
                g.DrawImage(GetSelectedPropagationIcon(OnUpdate), p2.X + 45, p2.Y + 18, 25, 25);
            }
        }

        public static Point[] CreateTriangle(Point center, int baseLength, int height)
        {
            Point[] triangleVertices = new Point[3];
            triangleVertices[0] = new Point(center.X, center.Y + height / 2);
            triangleVertices[1] = new Point(center.X - baseLength / 2, center.Y - height / 2);
            triangleVertices[2] = new Point(center.X + baseLength / 2, center.Y - height / 2);

            return triangleVertices;
        }

        public static Point[] Rotate(Point center, float radians, Point[] points)
        {
            // Calcular el seno y coseno del ángulo de rotación
            float cosTheta = (float)Math.Cos(radians);
            float sinTheta = (float)Math.Sin(radians);

            // Rotar cada punto alrededor del centro de rotación
            Point[] rotatedPoints = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                int x = points[i].X - center.X;
                int y = points[i].Y - center.Y;
                int xRotated = (int)(x * cosTheta - y * sinTheta);
                int yRotated = (int)(x * sinTheta + y * cosTheta);
                rotatedPoints[i] = new Point(xRotated + center.X, yRotated + center.Y);
            }

            return rotatedPoints;
        }

        public void DrawParentSide(Graphics g, Pen pen, Brush fillBrush)
        {
            DrawSide(g, pen, fillBrush, _ParentRelationType, ParentSide!.Value, ParentPoint!.Value);
        }

        public void DrawChildSide(Graphics g, Pen pen, Brush fillBrush)
        {
            DrawSide(g, pen, fillBrush, _ChildRelationType, ChildSide!.Value, ChildPoint!.Value);
        }

        public void DrawSide(Graphics g, Pen pen, Brush fillBrush, RelationTypeOptions relationType, SideEnum side, Point startPoint)
        {
            switch (relationType)
            {
                case RelationTypeOptions.One:
                    {
                        Point p0, p1, p2, p3;

                        p0 = startPoint;

                        switch (side)
                        {
                            case SideEnum.Top:
                                p1 = new Point(startPoint.X, startPoint.Y + 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y);
                                p3 = new Point(startPoint.X + 12, startPoint.Y);

                                break;
                            case SideEnum.Right:
                                p1 = new Point(startPoint.X - 25, startPoint.Y);
                                p2 = new Point(startPoint.X, startPoint.Y - 12);
                                p3 = new Point(startPoint.X, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                p1 = new Point(startPoint.X, startPoint.Y - 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y);
                                p3 = new Point(startPoint.X + 12, startPoint.Y);

                                break;
                            case SideEnum.Left:
                                p1 = new Point(startPoint.X + 25, startPoint.Y);
                                p2 = new Point(startPoint.X, startPoint.Y - 12);
                                p3 = new Point(startPoint.X, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p2, p3);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                        }
                    }

                    break;
                case RelationTypeOptions.Many:
                    {
                        Point p0, p1, p2, p3;

                        p0 = startPoint;

                        switch (side)
                        {
                            case SideEnum.Top:
                                p1 = new Point(startPoint.X, startPoint.Y + 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y + 25);
                                p3 = new Point(startPoint.X + 12, startPoint.Y + 25);

                                break;
                            case SideEnum.Right:
                                p1 = new Point(startPoint.X - 25, startPoint.Y);
                                p2 = new Point(startPoint.X - 25, startPoint.Y - 12);
                                p3 = new Point(startPoint.X - 25, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                p1 = new Point(startPoint.X, startPoint.Y - 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y - 25);
                                p3 = new Point(startPoint.X + 12, startPoint.Y - 25);

                                break;
                            case SideEnum.Left:
                                p1 = new Point(startPoint.X + 25, startPoint.Y);
                                p2 = new Point(startPoint.X + 25, startPoint.Y - 12);
                                p3 = new Point(startPoint.X + 25, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p0, p2);
                        g.DrawLine(pen, p0, p3);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p1, pen);
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                        }
                    }

                    break;
                case RelationTypeOptions.OneAndOnlyOne:
                    {
                        Point p0, p1, p2, p3, p4, p5;

                        switch (side)
                        {
                            case SideEnum.Top:
                                p0 = startPoint;
                                p1 = new Point(startPoint.X, startPoint.Y + 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y);
                                p3 = new Point(startPoint.X + 12, startPoint.Y);
                                p4 = new Point(startPoint.X - 12, startPoint.Y + 12);
                                p5 = new Point(startPoint.X + 12, startPoint.Y + 12);

                                break;
                            case SideEnum.Right:
                                p0 = startPoint;
                                p1 = new Point(startPoint.X - 25, startPoint.Y);
                                p2 = new Point(startPoint.X, startPoint.Y - 12);
                                p3 = new Point(startPoint.X, startPoint.Y + 12);
                                p4 = new Point(startPoint.X - 12, startPoint.Y - 12);
                                p5 = new Point(startPoint.X - 12, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                p0 = startPoint;
                                p1 = new Point(startPoint.X, startPoint.Y - 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y);
                                p3 = new Point(startPoint.X + 12, startPoint.Y);
                                p4 = new Point(startPoint.X - 12, startPoint.Y - 12);
                                p5 = new Point(startPoint.X + 12, startPoint.Y - 12);

                                break;
                            case SideEnum.Left:
                                p0 = startPoint;
                                p1 = new Point(startPoint.X + 25, startPoint.Y);
                                p2 = new Point(startPoint.X, startPoint.Y - 12);
                                p3 = new Point(startPoint.X, startPoint.Y + 12);
                                p4 = new Point(startPoint.X + 12, startPoint.Y - 12);
                                p5 = new Point(startPoint.X + 12, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p2, p3);
                        g.DrawLine(pen, p4, p5);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p1, pen);
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                            DrawRoundedEndOfLine(g, p4, pen);
                            DrawRoundedEndOfLine(g, p5, pen);
                        }
                    }

                    break;
                case RelationTypeOptions.OneOrMany:
                    {
                        Point p0, p1, p2, p3, p4, p5;

                        p0 = startPoint;

                        switch (side)
                        {
                            case SideEnum.Top:
                                p1 = new Point(startPoint.X, startPoint.Y + 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y + 25);
                                p3 = new Point(startPoint.X + 12, startPoint.Y + 25);
                                p4 = new Point(startPoint.X - 12, startPoint.Y);
                                p5 = new Point(startPoint.X + 12, startPoint.Y);

                                break;
                            case SideEnum.Right:
                                p1 = new Point(startPoint.X - 25, startPoint.Y);
                                p2 = new Point(startPoint.X - 25, startPoint.Y - 12);
                                p3 = new Point(startPoint.X - 25, startPoint.Y + 12);
                                p4 = new Point(startPoint.X, startPoint.Y - 12);
                                p5 = new Point(startPoint.X, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                p1 = new Point(startPoint.X, startPoint.Y - 25);
                                p2 = new Point(startPoint.X - 12, startPoint.Y - 25);
                                p3 = new Point(startPoint.X + 12, startPoint.Y - 25);
                                p4 = new Point(startPoint.X - 12, startPoint.Y);
                                p5 = new Point(startPoint.X + 12, startPoint.Y);

                                break;
                            case SideEnum.Left:
                                p1 = new Point(startPoint.X + 25, startPoint.Y);
                                p2 = new Point(startPoint.X + 25, startPoint.Y - 12);
                                p3 = new Point(startPoint.X + 25, startPoint.Y + 12);
                                p4 = new Point(startPoint.X, startPoint.Y - 12);
                                p5 = new Point(startPoint.X, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p0, p2);
                        g.DrawLine(pen, p0, p3);
                        g.DrawLine(pen, p4, p5);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p1, pen);
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                            DrawRoundedEndOfLine(g, p4, pen);
                            DrawRoundedEndOfLine(g, p5, pen);
                        }
                    }

                    break;
                case RelationTypeOptions.ZeroOrMany:
                    {
                        Rectangle r;
                        Point p0, p1, p2, p3;

                        switch (side)
                        {
                            case SideEnum.Top:
                                r = new Rectangle(startPoint.X - 11, startPoint.Y, 22, 22);

                                p0 = new Point(startPoint.X, startPoint.Y + 22);
                                p1 = new Point(startPoint.X, startPoint.Y + 46);
                                p2 = new Point(startPoint.X - 12, startPoint.Y + 46);
                                p3 = new Point(startPoint.X + 12, startPoint.Y + 46);

                                break;
                            case SideEnum.Right:
                                r = new Rectangle(startPoint.X - 22, startPoint.Y - 11, 22, 22);

                                p0 = new Point(startPoint.X - 22, startPoint.Y);
                                p1 = new Point(startPoint.X - 46, startPoint.Y);
                                p2 = new Point(startPoint.X - 46, startPoint.Y - 12);
                                p3 = new Point(startPoint.X - 46, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                r = new Rectangle(startPoint.X - 11, startPoint.Y - 22, 22, 22);

                                p0 = new Point(startPoint.X, startPoint.Y - 22);
                                p1 = new Point(startPoint.X, startPoint.Y - 46);
                                p2 = new Point(startPoint.X - 12, startPoint.Y - 46);
                                p3 = new Point(startPoint.X + 12, startPoint.Y - 46);

                                break;
                            case SideEnum.Left:
                                r = new Rectangle(startPoint.X, startPoint.Y - 11, 22, 22);

                                p0 = new Point(startPoint.X + 22, startPoint.Y);
                                p1 = new Point(startPoint.X + 46, startPoint.Y);
                                p2 = new Point(startPoint.X + 46, startPoint.Y - 12);
                                p3 = new Point(startPoint.X + 46, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.FillEllipse(fillBrush, r);
                        g.DrawEllipse(pen, r);

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p0, p2);
                        g.DrawLine(pen, p0, p3);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p1, pen);
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                        }
                    }

                    break;
                case RelationTypeOptions.ZeroOrOne:
                    {
                        Rectangle r;
                        Point p0, p1, p2, p3;

                        switch (side)
                        {
                            case SideEnum.Top:
                                r = new Rectangle(startPoint.X - 11, startPoint.Y, 22, 22);

                                p0 = new Point(startPoint.X, startPoint.Y + 22);
                                p1 = new Point(startPoint.X, startPoint.Y + 46);
                                p2 = new Point(startPoint.X - 12, startPoint.Y + 34);
                                p3 = new Point(startPoint.X + 12, startPoint.Y + 34);

                                break;
                            case SideEnum.Right:
                                r = new Rectangle(startPoint.X - 22, startPoint.Y - 11, 22, 22);

                                p0 = new Point(startPoint.X - 22, startPoint.Y);
                                p1 = new Point(startPoint.X - 46, startPoint.Y);
                                p2 = new Point(startPoint.X - 34, startPoint.Y - 12);
                                p3 = new Point(startPoint.X - 34, startPoint.Y + 12);

                                break;
                            case SideEnum.Bottom:
                                r = new Rectangle(startPoint.X - 11, startPoint.Y - 22, 22, 22);

                                p0 = new Point(startPoint.X, startPoint.Y - 22);
                                p1 = new Point(startPoint.X, startPoint.Y - 46);
                                p2 = new Point(startPoint.X - 12, startPoint.Y - 34);
                                p3 = new Point(startPoint.X + 12, startPoint.Y - 34);

                                break;
                            case SideEnum.Left:
                                r = new Rectangle(startPoint.X, startPoint.Y - 11, 22, 22);

                                p0 = new Point(startPoint.X + 22, startPoint.Y);
                                p1 = new Point(startPoint.X + 46, startPoint.Y);
                                p2 = new Point(startPoint.X + 34, startPoint.Y - 12);
                                p3 = new Point(startPoint.X + 34, startPoint.Y + 12);

                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        g.FillEllipse(fillBrush, r);
                        g.DrawEllipse(pen, r);

                        g.DrawLine(pen, p0, p1);
                        g.DrawLine(pen, p2, p3);

                        if (pen.Width > 2.0f)
                        {
                            DrawRoundedEndOfLine(g, p1, pen);
                            DrawRoundedEndOfLine(g, p2, pen);
                            DrawRoundedEndOfLine(g, p3, pen);
                        }
                    }

                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void DrawRoundedEndOfLine(Graphics g, Point p, Pen pen)
        {
            int penWidth = (int)pen.Width;
            int halfPenWidth = penWidth / 2;
            g.FillEllipse(pen.Brush, p.X - halfPenWidth, p.Y - halfPenWidth, penWidth, penWidth);
        }

        internal void RecalculateBinarySides()
        {
            List<DrawableRelationOption> options = new List<DrawableRelationOption>();

            if (ChildTable.Center.X >= ParentTable.Center.X)
            {
                if (ChildTable.Center.Y >= ParentTable.Center.Y)
                {
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Right, SideEnum.Left, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Right, SideEnum.Top, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Bottom, SideEnum.Left, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Bottom, SideEnum.Top, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                }
                else
                {
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Top, SideEnum.Bottom, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Top, SideEnum.Left, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Right, SideEnum.Bottom, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Right, SideEnum.Left, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                }
            }
            else
            {
                if (ChildTable.Center.Y >= ParentTable.Center.Y)
                {
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Bottom, SideEnum.Top, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Bottom, SideEnum.Right, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Left, SideEnum.Top, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Left, SideEnum.Right, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                }
                else
                {
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Top, SideEnum.Right, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Top, SideEnum.Bottom, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Left, SideEnum.Right, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                    options.Add(new DrawableRelationOption(ParentTable, ChildTable, SideEnum.Left, SideEnum.Bottom, ParentRelationTypeIconWidth, ChildRelationTypeIconWidth));
                }
            }

            DrawableRelationOption bestOption = options.OrderBy(o => o.Overlaps).ThenBy(o => o.SquaredDistance).First();
            _ParentSide = bestOption.Side1;
            _ChildSide = bestOption.Side2;
            _ParentPoint = bestOption.Point1;
            _ChildPoint = bestOption.Point2;
        }

        internal void ResetSides()
        {
            _ParentSide = null;
            _ChildSide = null;
        }

        internal void RecalculateUnarySide(Dictionary<SideEnum, int> binaryRelationCountBySide)
        {
            SideEnum maxSpaceAvailableSide = SideEnum.Top;
            int maxGapLength = 0;

            foreach (SideEnum side in binaryRelationCountBySide.Keys)
            {
                int n = binaryRelationCountBySide[side] + 3 * ParentTable.Relations.Where(dri => dri.ParentTable == dri.ChildTable && dri.ParentSide.HasValue && dri.ParentSide.Value == side).Count();
                int availableLength = (side == SideEnum.Left || side == SideEnum.Right) ? ParentTable.DrawBox.Height : ParentTable.DrawBox.Width;

                if (n == 0)
                {
                    maxSpaceAvailableSide = side;
                    break;
                }
                else
                {
                    int gapLength = availableLength / (n + 3);
                    if (gapLength > maxGapLength)
                    {
                        maxGapLength = gapLength;
                        maxSpaceAvailableSide = side;
                    }
                }
            }

            _ParentSide = maxSpaceAvailableSide;
            _ChildSide = maxSpaceAvailableSide;
            _ParentPoint = DrawableRelationOption.CalculateSideCentralPoint(ParentTable, maxSpaceAvailableSide, ParentRelationTypeIconWidth);
            _ChildPoint = DrawableRelationOption.CalculateSideCentralPoint(ParentTable, maxSpaceAvailableSide, ChildRelationTypeIconWidth);
        }

        public void RecalculateBezierPoints()
        {
            List<Point> bezierPoints = new List<Point>();

            int x1 = _ParentPoint!.Value.X;
            int y1 = _ParentPoint!.Value.Y;
            int x2 = _ChildPoint!.Value.X;
            int y2 = _ChildPoint!.Value.Y;

            if (ParentTable == ChildTable)
            {
                // Unary relation

                bezierPoints.Add(new Point(x1, y1));

                switch (ParentSide)
                {
                    case SideEnum.Top:
                        bezierPoints.Add(new Point(x1, y1 - UnaryRelationSize));
                        bezierPoints.Add(new Point(x2, y2 - UnaryRelationSize));
                        break;
                    case SideEnum.Right:
                        bezierPoints.Add(new Point(x1 + UnaryRelationSize, y1));
                        bezierPoints.Add(new Point(x2 + UnaryRelationSize, y2));
                        break;
                    case SideEnum.Bottom:
                        bezierPoints.Add(new Point(x1, y1 + UnaryRelationSize));
                        bezierPoints.Add(new Point(x2, y2 + UnaryRelationSize));
                        break;
                    case SideEnum.Left:
                        bezierPoints.Add(new Point(x1 - UnaryRelationSize, y1));
                        bezierPoints.Add(new Point(x2 - UnaryRelationSize, y2));
                        break;
                    default:
                        throw new NotSupportedException();
                }

                bezierPoints.Add(new Point(x2, y2));
            }
            else
            {
                // Binary relation

                if ((ParentSide == SideEnum.Left || ParentSide == SideEnum.Right) && (ChildSide == SideEnum.Left || ChildSide == SideEnum.Right))
                {
                    // Horizontal
                    bezierPoints.Add(new Point(x1, y1));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.25), y1));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.375), (int)(y1 + (y2 - y1) * 0.25)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.5), (int)(y1 + (y2 - y1) * 0.5)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.625), (int)(y1 + (y2 - y1) * 0.75)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.75), y2));
                    bezierPoints.Add(new Point(x2, y2));
                }
                else if ((ParentSide == SideEnum.Top || ParentSide == SideEnum.Bottom) && (ChildSide == SideEnum.Top || ChildSide == SideEnum.Bottom))
                {
                    // Vertical
                    bezierPoints.Add(new Point(x1, y1));
                    bezierPoints.Add(new Point(x1, (int)(y1 + (y2 - y1) * 0.25)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.25), (int)(y1 + (y2 - y1) * 0.375)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.5), (int)(y1 + (y2 - y1) * 0.5)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.75), (int)(y1 + (y2 - y1) * 0.625)));
                    bezierPoints.Add(new Point(x2, (int)(y1 + (y2 - y1) * 0.75)));
                    bezierPoints.Add(new Point(x2, y2));
                }
                else if ((ParentSide == SideEnum.Left || ParentSide == SideEnum.Right) && (ChildSide == SideEnum.Top || ChildSide == SideEnum.Bottom))
                {
                    // Concave
                    bezierPoints.Add(new Point(x1, y1));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.25), y1));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.5), y1));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.75), (int)(y1 + (y2 - y1) * 0.25)));
                    bezierPoints.Add(new Point(x2, (int)(y1 + (y2 - y1) * 0.5)));
                    bezierPoints.Add(new Point(x2, (int)(y1 + (y2 - y1) * 0.75)));
                    bezierPoints.Add(new Point(x2, y2));
                }
                else if ((ParentSide == SideEnum.Top || ParentSide == SideEnum.Bottom) && (ChildSide == SideEnum.Left || ChildSide == SideEnum.Right))
                {
                    // Convex
                    bezierPoints.Add(new Point(x1, y1));
                    bezierPoints.Add(new Point(x1, (int)(y1 + (y2 - y1) * 0.25)));
                    bezierPoints.Add(new Point(x1, (int)(y1 + (y2 - y1) * 0.5)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.25), (int)(y1 + (y2 - y1) * 0.75)));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.5), y2));
                    bezierPoints.Add(new Point((int)(x1 + (x2 - x1) * 0.75), y2));
                    bezierPoints.Add(new Point(x2, y2));
                }
            }

            _BezierCurveDefinitionPoints = bezierPoints.ToArray();

            int n = 10;
            _BezierCurveRealPoints = new Point[n + 1];
            for (int i = 0; i <= n; i++)
            {
                _BezierCurveRealPoints[i] = CalculateBezierPoint(_BezierCurveDefinitionPoints, ((float)i) / n);
            }
        }

        public static Point CalculateBezierPoint(Point[] bezierCurves, float t)
        {
            int numberOfCurves = bezierCurves.Length / 3;

            float distanceBetweenCurves = 1f / numberOfCurves;

            int currentCurveIndex = (int)(t / distanceBetweenCurves);
            if (currentCurveIndex >= numberOfCurves)
            {
                currentCurveIndex = numberOfCurves - 1;
            }

            float tOnCurrentCurve = (t - (currentCurveIndex * distanceBetweenCurves)) / distanceBetweenCurves;

            int startIndex = currentCurveIndex * 3;

            Point p0 = bezierCurves[startIndex];
            Point p1 = bezierCurves[startIndex + 1];
            Point p2 = bezierCurves[startIndex + 2];
            Point p3 = bezierCurves[startIndex + 3];

            return CalculateCubicBezierPoint(p0, p1, p2, p3, tOnCurrentCurve);
        }

        private static Point CalculateCubicBezierPoint(Point p0, Point p1, Point p2, Point p3, float t)
        {
            double u = 1 - t;
            double tt = t * t;
            double uu = u * u;
            double uuu = uu * u;
            double ttt = tt * t;

            double x = uuu * p0.X;
            double y = uuu * p0.Y;

            x += 3 * uu * t * p1.X;
            y += 3 * uu * t * p1.Y;
            x += 3 * u * tt * p2.X;
            y += 3 * u * tt * p2.Y;
            x += ttt * p3.X;
            y += ttt * p3.Y;

            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        public override bool IntersectsWith(Point p)
        {
            if (!_InteractBoundingBox!.Value.Contains(p)) return false;

            for (int i = 0; i < _BezierCurveRealPoints!.Length - 1; i++)
            {
                float distSquared = CalculateDistanceToSegmentSquared(p, _BezierCurveRealPoints[i], _BezierCurveRealPoints[i + 1]);
                if (distSquared < IntersectToleranceSquared) return true;
            }

            return false;
        }

        public override bool IsSelectable(Point p)
        {
            return _InteractBoundingBox!.Value.Contains(p);
        }

        public override float SelectingRank(Point p)
        {
            float[] distancesSquaredToPoints = new float[_BezierCurveRealPoints!.Length];

            for (int i = 0; i < _BezierCurveRealPoints!.Length; i++)
            {
                distancesSquaredToPoints[i] = CalculateDistanceToPointSquared(_BezierCurveRealPoints[i], p);
            }

            float minDist = float.MaxValue;
            int selectedIndex = -1;
            for (int i = 0; i < distancesSquaredToPoints.Length - 1; i++)
            {
                float dist = distancesSquaredToPoints[i] + distancesSquaredToPoints[i + 1];
                if (dist < minDist)
                {
                    selectedIndex = i;
                    minDist = dist;
                }
            }

            return CalculateDistanceToSegmentSquared(p, _BezierCurveRealPoints[selectedIndex], _BezierCurveRealPoints[selectedIndex + 1]);
        }

        public override float SelectingTolerance
        {
            get
            {
                return IntersectToleranceSquared;
            }
        }

        internal void RecalculateBoundingBox()
        {
            Rectangle r;

            if (ParentTable == ChildTable)
            {
                r = new Rectangle(
                    Math.Min(_ParentPoint!.Value.X, _ChildPoint!.Value.X),
                    Math.Min(_ParentPoint!.Value.Y, _ChildPoint!.Value.Y),
                    Math.Abs(_ParentPoint!.Value.X - _ChildPoint!.Value.X),
                    Math.Abs(_ParentPoint!.Value.Y - _ChildPoint!.Value.Y));
                r = AddMargins(r, UnaryRelationBoundingBoxMargin + Math.Max(UnaryRelationSize, TextRenderer.MeasureText(Id, idFont).Width));
            }
            else
            {
                r = new Rectangle(
                    Math.Min(_ParentPoint!.Value.X, _ChildPoint!.Value.X),
                    Math.Min(_ParentPoint!.Value.Y, _ChildPoint!.Value.Y),
                    Math.Abs(_ParentPoint!.Value.X - _ChildPoint!.Value.X),
                    Math.Abs(_ParentPoint!.Value.Y - _ChildPoint!.Value.Y));
            }

            _BoundingBox = AddMargins(r, BoundingBoxTolerance);
            _InteractBoundingBox = AddMargins(r, IntersectTolerance);
        }

        private PropagationOptions StringToPropagationOptions(string s)
        {
            switch (s.ToLower())
            {
                case "no action":
                    return PropagationOptions.NoAction;
                case "restrict":
                    return PropagationOptions.Restrict;
                case "cascade":
                    return PropagationOptions.Cascade;
                case "set null":
                    return PropagationOptions.SetNull;
                case "set default":
                    return PropagationOptions.SetDefault;
                default:
                    throw new NotSupportedException();
            }
        }

        private string PropagationOptionsToString(PropagationOptions o)
        {
            switch (o)
            {
                case PropagationOptions.NoAction:
                    return "no action";
                case PropagationOptions.Restrict:
                    return "restrict";
                case PropagationOptions.Cascade:
                    return "cascade";
                case PropagationOptions.SetNull:
                    return "set null";
                case PropagationOptions.SetDefault:
                    return "set default";
                default:
                    throw new NotSupportedException();
            }
        }

        private void CalculateRelationType()
        {
            _ChildRelationType = RelationTypeOptions.ZeroOrMany;
            _ParentRelationType = RelationTypeOptions.OneAndOnlyOne;

            foreach (DiagramColumn c in _ChildTableColumns)
            {
                c.ForeignKey = true;
                if (!c.NotNull) _ParentRelationType = RelationTypeOptions.ZeroOrOne;
            }
        }

        private int GetRelationTypeWidth(RelationTypeOptions relationType)
        {
            switch (relationType)
            {
                case RelationTypeOptions.One:
                    return RelationTypeIconSize1Width;
                case RelationTypeOptions.Many:
                    return RelationTypeIconSize1Width;
                case RelationTypeOptions.OneAndOnlyOne:
                    return RelationTypeIconSize1Width;
                case RelationTypeOptions.OneOrMany:
                    return RelationTypeIconSize1Width;
                case RelationTypeOptions.ZeroOrMany:
                    return RelationTypeIconSize2Width;
                case RelationTypeOptions.ZeroOrOne:
                    return RelationTypeIconSize2Width;
                default:
                    throw new NotSupportedException();
            }
        }

        private int GetRelationTypeHeight(RelationTypeOptions relationType)
        {
            return RelationTypeIconSizeHeight;
        }

        public enum PropagationOptions
        {
            NoAction,
            Restrict,
            Cascade,
            SetNull,
            SetDefault
        }

        public enum RelationTypeOptions
        {
            One,
            Many,
            OneAndOnlyOne,
            OneOrMany,
            ZeroOrMany,
            ZeroOrOne
        }
    }
}

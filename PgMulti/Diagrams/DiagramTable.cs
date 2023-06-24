using CsvHelper.Configuration.Attributes;
using Irony.Parsing;
using PgMulti.DataStructure;
using PgMulti.Export;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Xml;

namespace PgMulti.Diagrams
{
    public class DiagramTable : DiagramObject
    {
        public const int SingleColumnHeight = 30;

        private const int RoundingRadius = 20;
        private const int Padding = 20;
        private const int TitleMargin = 20;
        private const int ColumnIconSize = 25;
        private const int ColumnIconMargin = 5;
        private const int ColumnTypeMargin = 5;
        private const int ColumnsScrollMargin = 5;
        private const int ColumnsScrollWidth = 5;

        private string _SchemaName;
        private string _TableName;
        private List<DiagramColumn> _Columns;
        private List<DiagramTableRelation> _Relations;
        private int _VisibleColumns;

        private static Pen _NormalBorderPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 2);
        private static Pen _HighlightedBorderPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 6);
        private static Pen _ScrollBarPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 1);
        private static Brush _NormalBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private static Brush _HighlightedMainBrush = new SolidBrush(Color.FromArgb(255, 241, 201, 254));
        private static Brush _HighlightedSecondaryBrush = new SolidBrush(Color.FromArgb(255, 246, 223, 253));
        private static Brush _SelectedBrush = new SolidBrush(Color.FromArgb(255, 181, 207, 251));
        private static Brush _DraggingBrush = new SolidBrush(Color.FromArgb(150, 220, 220, 255));
        private static Brush _TextBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        private static Brush _HighlightedRelationKeyBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private static Brush _HighlightedRelationKeyBackgroundBrush = new SolidBrush(Color.FromArgb(255, 92, 33, 104));
        private static Brush _SelectedRelationKeyBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private static Brush _SelectedRelationKeyBackgroundBrush = new SolidBrush(Color.FromArgb(255, 36, 55, 118));
        private static Brush _SuggestedBrush = new SolidBrush(Color.FromArgb(255, 200, 200, 200));
        private static Brush _ScrollBarBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        private static Font _SchemaNameFont = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
        private static Font _TableNameFont = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
        private static Font _ColumnNameNormalFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private static Font _ColumnNameKeyFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private static Font _ColumnTypeFont = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Point);

        private Point? _Center = null;
        private Rectangle _ColumnsBoundingBox;
        private int? _MaxColumnNameWidth = null;
        private Rectangle _DrawBox;
        private int _ColumnsScroll = 0;
        private int _MaxColumnsScroll = 0;
        private int _TotalColumnsHeight;

        public DiagramTable(Diagram diagram, string schemaName, string tableName, List<DiagramColumn> columns, List<DiagramTableRelation> relations) : base(diagram)
        {
            _SchemaName = schemaName;
            _TableName = tableName;
            _Columns = columns;
            _Relations = relations;
            _VisibleColumns = Columns.Count;

            InitDimensions();
        }

        public DiagramTable(Diagram diagram, Table t) : base(diagram)
        {
            _SchemaName = t.IdSchema;
            _TableName = t.Id;

            _Columns = new List<DiagramColumn>();
            foreach (Column c in t.Columns)
            {
                Columns.Add(new DiagramColumn(c));
            }

            _Relations = new List<DiagramTableRelation>();
            _VisibleColumns = Columns.Count;

            InitDimensions();
        }

        public DiagramTable(Diagram diagram, XmlElement xeTable) : base(diagram)
        {
            string? v;
            int n;

            v = xeTable.GetAttribute("schema_name");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            _SchemaName = v;

            v = xeTable.GetAttribute("table_name");
            if (string.IsNullOrEmpty(v)) throw new BadFormatException();

            _TableName = v;

            _Columns = new List<DiagramColumn>();
            foreach (XmlElement xeColumn in xeTable.SelectNodes("columns/column")!)
            {
                _Columns.Add(new DiagramColumn(xeColumn));
            }

            _Relations = new List<DiagramTableRelation>();

            v = xeTable.GetAttribute("show_max_columns");
            if (!string.IsNullOrEmpty(v) && int.TryParse(v, out n) && n >= 0)
            {
                _VisibleColumns = Math.Min(Columns.Count, n);
            }
            else
            {
                _VisibleColumns = Columns.Count;
            }

            InitDimensions();

            v = xeTable.GetAttribute("x");
            if (string.IsNullOrEmpty(v) || !int.TryParse(v, out n)) throw new BadFormatException();

            _DrawBox.X = n - _DrawBox.Width / 2;

            v = xeTable.GetAttribute("y");
            if (string.IsNullOrEmpty(v) || !int.TryParse(v, out n)) throw new BadFormatException();

            _DrawBox.Y = n - _DrawBox.Height / 2;

            _BoundingBox = AddMargins(_DrawBox, 1);
            _RefreshColumnsBoundingBoxLocation();
        }

        public string SchemaName { get => _SchemaName; set => _SchemaName = value; }
        public string TableName { get => _TableName; set => _TableName = value; }
        public List<DiagramColumn> Columns { get => _Columns; set => _Columns = value; }
        public List<DiagramTableRelation> Relations { get => _Relations; set => _Relations = value; }
        public int VisibleColumns
        {
            get => _VisibleColumns;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > Columns.Count)
                {
                    value = Columns.Count;
                }

                int delta = value - _VisibleColumns;

                if (delta != 0)
                {
                    int deltaHeight = SingleColumnHeight * delta;
                    int deltaWidth = 0;

                    if ((_VisibleColumns < Columns.Count && _VisibleColumns > 0) != (value < Columns.Count && value > 0))
                    {
                        deltaWidth = ColumnsScrollMargin * 2 + ColumnsScrollWidth;

                        if (_VisibleColumns < Columns.Count && _VisibleColumns > 0)
                        {
                            deltaWidth *= -1;
                        }
                    }

                    _VisibleColumns = value;

                    _ColumnsBoundingBox.Height += deltaHeight;
                    _BoundingBox.Height += deltaHeight;
                    _DrawBox.Height += deltaHeight;
                    _MaxColumnsScroll = _TotalColumnsHeight - ColumnsBoundingBox.Height;
                    ColumnsScroll = ColumnsScroll;

                    _ColumnsBoundingBox.Width += deltaWidth;
                    _BoundingBox.Width += deltaWidth;
                    _DrawBox.Width += deltaWidth;

                    _Center = null;
                }
            }
        }
        public Rectangle ColumnsBoundingBox { get => _ColumnsBoundingBox; }

        public Point Center
        {
            get
            {
                if (!_Center.HasValue)
                {
                    _Center = new Point(DrawBox.X + DrawBox.Width / 2, DrawBox.Y + DrawBox.Height / 2);
                }

                return _Center.Value;
            }
        }

        public int ColumnsScroll
        {
            get => _ColumnsScroll;
            set
            {
                if (value < 0)
                {
                    _ColumnsScroll = 0;
                }
                else if (value > MaxColumnsScroll)
                {
                    _ColumnsScroll = MaxColumnsScroll;
                }
                else
                {
                    _ColumnsScroll = value;
                }
            }
        }
        public int MaxColumnsScroll { get => _MaxColumnsScroll; }

        internal Rectangle DrawBox
        {
            get
            {
                return _DrawBox;
            }
        }

        private Rectangle? DraggingDrawBox
        {
            get
            {
                if (!Dragging) return null;

                Rectangle r = DrawBox;
                r.X += _CurrentDraggingPoint!.Value.X - _StartDraggingPoint!.Value.X;
                r.Y += _CurrentDraggingPoint.Value.Y - _StartDraggingPoint.Value.Y;

                return r;
            }
        }

        public XmlElement ToXml(XmlDocument xd, Point basePoint)
        {
            XmlElement xeTable = xd.CreateElement("table");
            xeTable.SetAttribute("schema_name", SchemaName);
            xeTable.SetAttribute("table_name", TableName);
            if (VisibleColumns != -1 && VisibleColumns != Columns.Count) xeTable.SetAttribute("show_max_columns", VisibleColumns.ToString());
            xeTable.SetAttribute("x", (Center.X - basePoint.X).ToString());
            xeTable.SetAttribute("y", (Center.Y - basePoint.Y).ToString());

            XmlElement xeColumns = xd.CreateElement("columns");
            xeTable.AppendChild(xeColumns);
            foreach (DiagramColumn dc in Columns)
            {
                xeColumns.AppendChild(dc.ToXml(xd));
            }

            return xeTable;
        }

        public override void Draw(Graphics g)
        {
            Brush br;
            Pen pen = _NormalBorderPen;
            DiagramTableRelation? activeRelation = null;
            Brush? keyBrush = null;
            Brush? keyBackgroundBrush = null;

            if (_StartDraggingPoint.HasValue)
            {
                br = _DraggingBrush;
            }
            else if (_Highlighted)
            {
                br = _HighlightedMainBrush;
                pen = _HighlightedBorderPen;
            }
            else if (Suggested)
            {
                br = _SuggestedBrush;
            }
            else if (Relations.Any(dr => dr.Highlighted || OtherTableInRelation(dr).Highlighted))
            {
                br = _HighlightedSecondaryBrush;
                activeRelation = Relations.FirstOrDefault(dr => dr.Highlighted);
                keyBrush = _HighlightedRelationKeyBrush;
                keyBackgroundBrush = _HighlightedRelationKeyBackgroundBrush;
            }
            else if (_Selected)
            {
                br = _SelectedBrush;
                pen = _HighlightedBorderPen;
            }
            else
            {
                br = _NormalBrush;
                activeRelation = Relations.FirstOrDefault(dr => dr.Selected);
                keyBrush = _SelectedRelationKeyBrush;
                keyBackgroundBrush = _SelectedRelationKeyBackgroundBrush;
            }

            Rectangle r;

            if (_StartDraggingPoint.HasValue && _CurrentDraggingPoint.HasValue)
            {
                r = DraggingDrawBox!.Value;
            }
            else
            {
                r = DrawBox;
            }

            g.FillPath(br, RoundedRect(r, RoundingRadius));
            g.DrawPath(pen, RoundedRect(r, RoundingRadius));

            List<DiagramColumn> keys = new List<DiagramColumn>();
            if (activeRelation != null)
            {
                if (activeRelation.ParentTable == this)
                {
                    keys = activeRelation.ParentTableColumns;
                }
                else
                {
                    keys = activeRelation.ChildTableColumns;
                }
            }

            DrawContent(g, r, keys, keyBrush, keyBackgroundBrush);
        }

        private void DrawContent(Graphics g, Rectangle r, List<DiagramColumn> keys, Brush? keyBrush, Brush? keyBackgroundBrush)
        {
            int y = r.Y + Padding;
            int x = r.X + Padding;

            g.DrawString(SchemaName, _SchemaNameFont, _TextBrush, x, y);
            y += (int)(_SchemaNameFont.Size * 2);

            g.DrawString(TableName, _TableNameFont, _TextBrush, x, y);
            y += (int)(_TableNameFont.Size * 2);
            y += TitleMargin;

            if (VisibleColumns != 0)
            {
                using (Bitmap cb = new Bitmap(ColumnsBoundingBox.Width, ColumnsBoundingBox.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    Graphics gcb = Graphics.FromImage(cb);
                    gcb.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    gcb.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    int yc = -ColumnsScroll;
                    foreach (DiagramColumn dc in Columns)
                    {
                        if (dc.PrimaryKey)
                        {
                            gcb.DrawImage(Properties.DiagramIcons.primary_key, 0, yc, ColumnIconSize, ColumnIconSize);
                        }
                        else if (dc.ForeignKey)
                        {
                            if (dc.NotNull)
                            {
                                gcb.DrawImage(Properties.DiagramIcons.not_null_foreign_key, 0, yc, ColumnIconSize, ColumnIconSize);
                            }
                            else
                            {
                                gcb.DrawImage(Properties.DiagramIcons.nullable_foreign_key, 0, yc, ColumnIconSize, ColumnIconSize);
                            }
                        }
                        else
                        {
                            if (dc.NotNull)
                            {
                                gcb.DrawImage(Properties.DiagramIcons.not_null_field, 0, yc, ColumnIconSize, ColumnIconSize);
                            }
                            else
                            {
                                gcb.DrawImage(Properties.DiagramIcons.nullable_field, 0, yc, ColumnIconSize, ColumnIconSize);
                            }
                        }

                        Font f;
                        Brush br;

                        if (keys.Contains(dc))
                        {
                            f = _ColumnNameKeyFont;
                            br = keyBrush!;

                            gcb.FillPath(keyBackgroundBrush!, RoundedRect(new Rectangle(ColumnIconSize + ColumnIconMargin - 2, yc - 2, (int)g.MeasureString(dc.ColumnName, f).Width + 4, (int)(_ColumnNameNormalFont.Size * 3 - 2)), 5));
                        }
                        else
                        {
                            f = _ColumnNameNormalFont;
                            br = _TextBrush;
                        }

                        gcb.DrawString(dc.ColumnName, f, br, ColumnIconSize + ColumnIconMargin, yc);

                        gcb.DrawString(dc.TypeInitials, _ColumnTypeFont, _TextBrush, ColumnIconSize + ColumnIconMargin + _MaxColumnNameWidth!.Value + ColumnTypeMargin, yc);

                        yc += SingleColumnHeight;
                    }

                    g.DrawImage(cb, x, y);
                }

                if (VisibleColumns < Columns.Count)
                {
                    Rectangle outerScrollBarRect = new Rectangle(x + ColumnsBoundingBox.Width - ColumnsScrollMargin - ColumnsScrollWidth, y, ColumnsScrollWidth, ColumnsBoundingBox.Height);
                    g.DrawPath(_ScrollBarPen, RoundedRect(outerScrollBarRect, ColumnsScrollWidth / 2));

                    Rectangle rsb = new Rectangle(
                        outerScrollBarRect.X,
                        outerScrollBarRect.Y + ColumnsScroll * outerScrollBarRect.Height / _TotalColumnsHeight,
                        outerScrollBarRect.Width,
                        outerScrollBarRect.Height * VisibleColumns / Columns.Count);

                    g.FillPath(_ScrollBarBrush, RoundedRect(rsb, ColumnsScrollWidth / 2));
                }
            }
        }

        protected void InitDimensions()
        {
            _MaxColumnNameWidth = 0;
            foreach (DiagramColumn dc in Columns)
            {
                _MaxColumnNameWidth = Math.Max(_MaxColumnNameWidth.Value, TextRenderer.MeasureText(dc.ColumnName, (dc.PrimaryKey || dc.ForeignKey) ? _ColumnNameKeyFont : _ColumnNameNormalFont).Width);
            }

            int maxColumnTypeWidth = 0;
            foreach (DiagramColumn dc in Columns)
            {
                maxColumnTypeWidth = Math.Max(maxColumnTypeWidth, TextRenderer.MeasureText(dc.TypeInitials, _ColumnTypeFont).Width);
            }

            _ColumnsBoundingBox = new Rectangle(
                0,
                0,
                ColumnIconSize
                + ColumnIconMargin
                + _MaxColumnNameWidth.Value
                + ColumnTypeMargin
                + maxColumnTypeWidth,
                SingleColumnHeight * VisibleColumns
            );

            if (VisibleColumns < Columns.Count)
            {
                _TotalColumnsHeight = SingleColumnHeight * Columns.Count;
                _MaxColumnsScroll = _TotalColumnsHeight - ColumnsBoundingBox.Height;

                if (VisibleColumns > 0)
                {
                    _ColumnsBoundingBox.Width += ColumnsScrollMargin * 2 + ColumnsScrollWidth;
                }
            }
            else
            {
                _TotalColumnsHeight = ColumnsBoundingBox.Height;
            }

            int w = 2 * Padding
                + Math.Max(
                    Math.Max(
                        TextRenderer.MeasureText(SchemaName, _SchemaNameFont).Width,
                        TextRenderer.MeasureText(TableName, _TableNameFont).Width
                    ),
                    ColumnsBoundingBox.Width
                );

            int h = 2 * Padding
                + (int)(_SchemaNameFont.Size * 2)
                + (int)(_TableNameFont.Size * 2)
                + TitleMargin
                + ColumnsBoundingBox.Height;

            _DrawBox = new Rectangle(0, 0, w, h);
            _BoundingBox = AddMargins(_DrawBox, 1);
            _RefreshColumnsBoundingBoxLocation();
        }

        private void _RefreshColumnsBoundingBoxLocation()
        {
            _ColumnsBoundingBox.X = _DrawBox.X + Padding;
            _ColumnsBoundingBox.Y = _DrawBox.Y + Padding
                + (int)(_SchemaNameFont.Size * 2)
                + (int)(_TableNameFont.Size * 2)
                + TitleMargin;
        }

        public void RecalculateRelationPoints()
        {
            foreach (DiagramTableRelation dr in Relations.Where(dri => dri.ParentTable != dri.ChildTable))
            {
                dr.RecalculateBinarySides();
            }

            Dictionary<SideEnum, int> binaryRelationCountBySide = new Dictionary<SideEnum, int>();
            binaryRelationCountBySide[SideEnum.Top] = 0;
            binaryRelationCountBySide[SideEnum.Right] = 0;
            binaryRelationCountBySide[SideEnum.Bottom] = 0;
            binaryRelationCountBySide[SideEnum.Left] = 0;

            foreach (SideEnum side in binaryRelationCountBySide.Keys)
            {
                binaryRelationCountBySide[side] = Relations.Where(dri => dri.ParentTable != dri.ChildTable && (dri.ParentTable == this ? dri.ParentSide!.Value : dri.ChildSide!.Value) == side).Count();
            }

            foreach (DiagramTableRelation dr in Relations.Where(dri => dri.ParentTable == dri.ChildTable))
            {
                dr.ResetSides();
            }

            foreach (DiagramTableRelation dr in Relations.Where(dri => dri.ParentTable == dri.ChildTable))
            {
                dr.RecalculateUnarySide(binaryRelationCountBySide);
            }
        }

        public void DistributeRelationPoints()
        {
            Dictionary<SideEnum, List<DiagramTableRelation>> relations = new Dictionary<SideEnum, List<DiagramTableRelation>>();
            relations[SideEnum.Top] = new List<DiagramTableRelation>();
            relations[SideEnum.Right] = new List<DiagramTableRelation>();
            relations[SideEnum.Bottom] = new List<DiagramTableRelation>();
            relations[SideEnum.Left] = new List<DiagramTableRelation>();

            foreach (DiagramTableRelation dr in Relations.Where(dri => dri.ParentTable == dri.ChildTable))
            {
                relations[dr.ParentSide!.Value].Add(dr);
            }

            foreach (DiagramTableRelation dr in Relations.Where(dri => dri.ParentTable != dri.ChildTable))
            {
                SideEnum side = (dr.ParentTable == this ? dr.ParentSide!.Value : dr.ChildSide!.Value);
                relations[side].Add(dr);
            }

            foreach (var ri in relations)
            {
                DistributePoints(ri.Key, ri.Value);
            }
        }

        private void DistributePoints(SideEnum side, List<DiagramTableRelation> relations)
        {
            const int MaxGap = 50;

            Point p0 = DrawableRelationOption.CalculateSideCentralPoint(this, side, 0);

            int availableLength = (side == SideEnum.Left || side == SideEnum.Right) ? DrawBox.Height : DrawBox.Width;
            int gap;
            int count = relations.Where(dri => dri.ParentTable != dri.ChildTable).Count() + 3 * relations.Where(dri => dri.ParentTable == dri.ChildTable).Count();

            if (count > 1)
            {
                gap = Math.Min(MaxGap, availableLength / (count - 1));
            }
            else
            {
                gap = 0;
            }

            switch (side)
            {
                case SideEnum.Top:
                    relations = relations.OrderBy(dr => dr.ParentTable == dr.ChildTable).ThenByDescending(dr => dr.ParentTable == this ? dr.ParentVSlope : dr.ChildVSlope).ToList();
                    p0.X -= ((count - 1) * gap) / 2;
                    break;
                case SideEnum.Right:
                    relations = relations.OrderBy(dr => dr.ParentTable == dr.ChildTable).ThenByDescending(dr => dr.ParentTable == this ? dr.ParentHSlope : dr.ChildHSlope).ToList();
                    p0.Y -= ((count - 1) * gap) / 2;
                    break;
                case SideEnum.Bottom:
                    relations = relations.OrderBy(dr => dr.ParentTable == dr.ChildTable).ThenBy(dr => dr.ParentTable == this ? dr.ParentVSlope : dr.ChildVSlope).ToList();
                    p0.X -= ((count - 1) * gap) / 2;
                    break;
                case SideEnum.Left:
                    relations = relations.OrderBy(dr => dr.ParentTable == dr.ChildTable).ThenBy(dr => dr.ParentTable == this ? dr.ParentHSlope : dr.ChildHSlope).ToList();
                    p0.Y -= ((count - 1) * gap) / 2;
                    break;
                default:
                    throw new NotSupportedException();
            }

            foreach (DiagramTableRelation relation in relations)
            {
                Point p = p0;
                int w;

                if (relation.ParentTable == relation.ChildTable)
                {
                    // Unary relation
                    // Parent side
                    w = relation.ParentRelationTypeIconWidth;

                    switch (side)
                    {
                        case SideEnum.Top:
                            p.Y -= w;
                            break;
                        case SideEnum.Right:
                            p.X += w;
                            break;
                        case SideEnum.Bottom:
                            p.Y += w;
                            break;
                        case SideEnum.Left:
                            p.X -= w;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    relation.ParentPoint = p;

                    if (side == SideEnum.Left || side == SideEnum.Right)
                    {
                        p0.Y += 2 * gap;
                    }
                    else
                    {
                        p0.X += 2 * gap;
                    }

                    // Child side
                    p = p0;
                    w = relation.ChildRelationTypeIconWidth;

                    switch (side)
                    {
                        case SideEnum.Top:
                            p.Y -= w;
                            break;
                        case SideEnum.Right:
                            p.X += w;
                            break;
                        case SideEnum.Bottom:
                            p.Y += w;
                            break;
                        case SideEnum.Left:
                            p.X -= w;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    relation.ChildPoint = p;

                    if (side == SideEnum.Left || side == SideEnum.Right)
                    {
                        p0.Y += gap;
                    }
                    else
                    {
                        p0.X += gap;
                    }
                }
                else
                {
                    // Binary relation

                    if (relation.ParentTable == this)
                    {
                        w = relation.ParentRelationTypeIconWidth;
                    }
                    else
                    {
                        w = relation.ChildRelationTypeIconWidth;
                    }

                    switch (side)
                    {
                        case SideEnum.Top:
                            p.Y -= w;
                            break;
                        case SideEnum.Right:
                            p.X += w;
                            break;
                        case SideEnum.Bottom:
                            p.Y += w;
                            break;
                        case SideEnum.Left:
                            p.X -= w;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    if (relation.ParentTable == this)
                    {
                        relation.ParentPoint = p;
                    }
                    else
                    {
                        relation.ChildPoint = p;
                    }

                    if (side == SideEnum.Left || side == SideEnum.Right)
                    {
                        p0.Y += gap;
                    }
                    else
                    {
                        p0.X += gap;
                    }
                }
            }
        }

        private Point TestDragLimits(Point p)
        {
            int maxX = _Diagram.BoundingBox.X + Diagram.MaxWidth + _StartDraggingPoint!.Value.X - BoundingBox.X - BoundingBox.Width;
            int maxY = _Diagram.BoundingBox.Y + Diagram.MaxHeight + _StartDraggingPoint!.Value.Y - BoundingBox.Y - BoundingBox.Height;
            int minX = _Diagram.BoundingBox.X - Diagram.MaxWidth + _Diagram.BoundingBox.Width + _StartDraggingPoint!.Value.X - BoundingBox.X;
            int minY = _Diagram.BoundingBox.Y - Diagram.MaxHeight + _Diagram.BoundingBox.Height + _StartDraggingPoint!.Value.Y - BoundingBox.Y;

            if (p.X > maxX) p.X = maxX;
            if (p.Y > maxY) p.Y = maxY;
            if (p.X < minX) p.X = minX;
            if (p.Y < minY) p.Y = minY;

            return p;
        }

        public override void MoveDrag(Point p)
        {
            p = TestDragLimits(p);
            base.MoveDrag(p);
            _Center = null;
        }

        public override void CompleteDrag(Point p)
        {
            p = TestDragLimits(p);
            _DrawBox.X += p.X - _StartDraggingPoint!.Value.X;
            _DrawBox.Y += p.Y - _StartDraggingPoint!.Value.Y;
            _RefreshColumnsBoundingBoxLocation();
            base.CompleteDrag(p);
            _Center = null;

            _Diagram.UpdateDiagramRelocator(this);
        }

        private Point TestMoveLimits(Point p)
        {
            int maxX = _Diagram.BoundingBox.X + Diagram.MaxWidth - BoundingBox.Width / 2;
            int maxY = _Diagram.BoundingBox.Y + Diagram.MaxHeight - BoundingBox.Height / 2;
            int minX = _Diagram.BoundingBox.X - Diagram.MaxWidth + _Diagram.BoundingBox.Width + BoundingBox.Width / 2;
            int minY = _Diagram.BoundingBox.Y - Diagram.MaxHeight + _Diagram.BoundingBox.Height + BoundingBox.Height / 2;

            if (p.X > maxX) p.X = maxX;
            if (p.Y > maxY) p.Y = maxY;
            if (p.X < minX) p.X = minX;
            if (p.Y < minY) p.Y = minY;

            return p;
        }

        public override void MoveTo(Point p)
        {
            Point p2 = TestMoveLimits(p);
            _DrawBox.X = p2.X - _DrawBox.Width / 2;
            _DrawBox.Y = p2.Y - _DrawBox.Height / 2;
            _RefreshColumnsBoundingBoxLocation();
            base.MoveTo(p2);
            _Center = p2;

            if (!p.Equals(p2))
            {
                _Diagram.UpdateDiagramRelocator(this);
            }
        }

        public DiagramTable OtherTableInRelation(DiagramTableRelation dr)
        {
            return dr.ParentTable == this ? dr.ChildTable : dr.ParentTable;
        }

        internal void ReorderColumns()
        {
            _Columns = _Columns.OrderByDescending(ci => ci.PrimaryKey).ThenByDescending(ci => ci.ForeignKey).ThenBy(ci => ci.ColumnName).ToList();
        }

        public override bool IsSelectable(Point p)
        {
            return _BoundingBox.Contains(p);
        }

        public bool IsScrollable(Point p)
        {
            return VisibleColumns != -1 && VisibleColumns < Columns.Count && ColumnsBoundingBox.Contains(p);
        }

        public override float SelectingRank(Point p)
        {
            return 0.0f;
        }

        public override float SelectingTolerance
        {
            get
            {
                return 0.0f;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (base.Equals(obj)) return true;

            if (obj is Table)
            {
                Table other = (Table)obj;
                return other.IdSchema == SchemaName && other.Id == TableName;
            }
            else if (obj is DiagramTable)
            {
                DiagramTable other = (DiagramTable)obj;
                return other.SchemaName == SchemaName && other.TableName == TableName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return TableName.GetHashCode();
        }

        public override string ToString()
        {
            return TableName + " [" + SchemaName + "]";
        }
    }
}

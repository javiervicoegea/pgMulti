using FastColoredTextBoxNS;
using PgMulti.AppData;
using PgMulti.DataAccess;
using PgMulti.DataStructure;
using PgMulti.Diagrams.Efdg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PgMulti.Diagrams
{
    public class Diagram
    {
        private const int DiagramFileVersion = 1;
        public const int MaxWidth = 10000;
        public const int MaxHeight = 10000;

        public float Scale = 1.0f;
        public PointF Translate = new PointF(0.0f, 0.0f);

        private List<DiagramTable> _Tables;
        private List<DiagramTableRelation> _Relations;
        private List<DiagramTable>? _SuggestedRelatedTables = null;

        private Point? _StartDraggingPoint = null;
        private Point? _CurrentDraggingPoint = null;

        private DiagramRelocator? _DiagramRelocator = null;

        private Rectangle _BoundingBox;
        private Rectangle _MaxBoundingBox;

        //private static Pen _BorderPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 2);
        private static Brush _BackgroundBrush = new SolidBrush(Color.FromArgb(255, 245, 245, 245));

        public static Diagram LoadFile(string filename)
        {
            Diagram dg = new Diagram();
            XmlDocument xd = new XmlDocument();
            xd.Load(filename);

            XmlElement? root = xd.DocumentElement;
            if (root == null) throw new BadFormatException();

            if (root.Name != "pgmulti_diagram") throw new BadFormatException();

            int version;
            if (!int.TryParse(root.GetAttribute("file_version"), out version) || version < 1 || version > DiagramFileVersion) throw new BadFormatException();

            foreach (XmlElement xeTable in root.SelectNodes("tables/table")!)
            {
                DiagramTable dt = new DiagramTable(dg, xeTable);

                dg.Tables.Add(dt);
            }

            dg.UpdateBoundingBox();

            foreach (XmlElement xeRelation in root.SelectNodes("relations/relation")!)
            {
                string? parentSchemaName;
                parentSchemaName = xeRelation.GetAttribute("parent_schema_name");
                if (string.IsNullOrEmpty(parentSchemaName)) throw new BadFormatException();

                string? parentTableName;
                parentTableName = xeRelation.GetAttribute("parent_table_name");
                if (string.IsNullOrEmpty(parentTableName)) throw new BadFormatException();

                DiagramTable? parentTable = dg.Tables.FirstOrDefault(dgi => dgi.SchemaName == parentSchemaName && dgi.TableName == parentTableName);
                if (parentTable == null) throw new BadFormatException();

                string? childSchemaName;
                childSchemaName = xeRelation.GetAttribute("child_schema_name");
                if (string.IsNullOrEmpty(childSchemaName)) throw new BadFormatException();

                string? childTableName;
                childTableName = xeRelation.GetAttribute("child_table_name");
                if (string.IsNullOrEmpty(childTableName)) throw new BadFormatException();

                DiagramTable? childTable = dg.Tables.FirstOrDefault(dgi => dgi.SchemaName == childSchemaName && dgi.TableName == childTableName);
                if (childTable == null) throw new BadFormatException();

                DiagramTableRelation dr = new DiagramTableRelation(dg, parentTable, childTable, xeRelation);

                dg.Relations.Add(dr);
                dr.ParentTable.Relations.Add(dr);
                if (dr.ParentTable != dr.ChildTable) dr.ChildTable.Relations.Add(dr);

            }

            dg.ReorderTableColumns();
            dg.ReorderZIndex();

            return dg;
        }

        public Diagram()
        {
            _Tables = new List<DiagramTable>();
            _Relations = new List<DiagramTableRelation>();
            UpdateBoundingBox();
        }

        public List<DiagramTable> Tables { get => _Tables; }
        public List<DiagramTableRelation> Relations { get => _Relations; }

        public List<DiagramTable>? SuggestedRelatedTables
        {
            get => _SuggestedRelatedTables;
            set
            {
                if (_DiagramRelocator != null && _SuggestedRelatedTables != null)
                {
                    _DiagramRelocator.Remove(_SuggestedRelatedTables);
                }

                if (_DiagramRelocator != null && value != null)
                {
                    _DiagramRelocator.Add(value);
                }

                if (_SuggestedRelatedTables != null)
                {
                    foreach (DiagramTable dt in _SuggestedRelatedTables)
                    {
                        foreach (DiagramTableRelation dr in dt.Relations)
                        {
                            if (dr.ParentTable == dr.ChildTable)
                            {
                            }
                            else if (dr.ParentTable == dt)
                            {
                                dr.ChildTable.Relations.Remove(dr);
                            }
                            else
                            {
                                dr.ParentTable.Relations.Remove(dr);
                            }
                        }
                    }
                }

                _SuggestedRelatedTables = value;
            }
        }

        public IEnumerable<DiagramObject> Objects
        {
            get
            {
                foreach (DiagramObject o in Tables)
                {
                    yield return o;
                }

                foreach (DiagramObject o in Relations)
                {
                    yield return o;
                }

                if (SuggestedRelatedTables != null)
                {
                    foreach (DiagramTable dt in SuggestedRelatedTables)
                    {
                        yield return dt;

                        foreach (DiagramTableRelation dr in dt.Relations)
                        {
                            yield return dr;
                        }
                    }
                }
            }
        }

        public bool Dragging
        {
            get
            {
                return _CurrentDraggingPoint.HasValue;// && _StartDraggingPoint.HasValue;
            }
        }

        public DiagramRelocator? DiagramRelocator { get { return _DiagramRelocator; } set { _DiagramRelocator = value; } }

        public Rectangle BoundingBox
        {
            get
            {
                return _BoundingBox;
            }
        }

        public Rectangle MaxBoundingBox
        {
            get
            {
                return _MaxBoundingBox;
            }
        }

        private Point? _Center = null;
        public Point Center
        {
            get
            {
                if (!_Center.HasValue)
                {
                    Point p = BoundingBox.Location;
                    p.X += BoundingBox.Width / 2;
                    p.Y += BoundingBox.Height / 2;
                    _Center = p;
                }

                return _Center.Value;
            }
        }


        public void AddTable(Table t)
        {
            DiagramTable? dt = FindTable(t);

            if (dt == null)
            {
                dt = new DiagramTable(this, t);
                _Tables.Add(dt);
                UpdateBoundingBox();
                AddTableRelations(dt, t);
                dt.ReorderColumns();
                ReorderZIndex();

                if (DiagramRelocator != null)
                {
                    List<DiagramTable> l = new List<DiagramTable>();
                    l.Add(dt);
                    DiagramRelocator.Add(l);
                }
                UpdateDiagramRelocator(dt);
            }
            else
            {
                // Update table:
                // Clear dt columns
                // Add all columns in t to dt
                // Clear dt relations in dt and also in _Relations
                // AddTableRelations(dt, t);
            }
        }

        public void RemoveTable(DiagramTable dt)
        {
            foreach (DiagramTableRelation dr in dt.Relations)
            {
                _Relations.Remove(dr);
                dt.OtherTableInRelation(dr).Relations.Remove(dr);
            }

            _Tables.Remove(dt);

            ReorderZIndex();
            UpdateBoundingBox();
        }

        public DiagramTable? FindTable(Table t)
        {
            foreach (DiagramTable dti in Tables)
            {
                if (dti.Equals(t))
                {
                    return dti;
                }
            }

            return null;
        }

        private void AddTableRelations(DiagramTable dt, Table t)
        {
            foreach (TableRelation r in t.Relations)
            {
                DiagramTable? parentTable = null;
                DiagramTable? childTable = null;

                Table otherSideTable;
                if (t.Equals(r.ParentTable))
                {
                    otherSideTable = r.ChildTable!;
                    parentTable = dt;
                }
                else if (t.Equals(r.ChildTable))
                {
                    otherSideTable = r.ParentTable!;
                    childTable = dt;
                }
                else
                {
                    throw new NotSupportedException();
                }

                DiagramTable? otherSideDiagramTable = null;
                foreach (DiagramTable dti in _Tables)
                {
                    if (dti.Equals(otherSideTable))
                    {
                        bool allColumnsFound = true;
                        foreach (Column c in otherSideTable.Columns)
                        {
                            bool columnFound = false;
                            foreach (DiagramColumn dc in dti.Columns)
                            {
                                if (dc.Equals(c))
                                {
                                    columnFound = true;
                                    break;
                                }
                            }
                            if (!columnFound)
                            {
                                allColumnsFound = false;
                                break;
                            }
                        }

                        if (allColumnsFound)
                        {
                            otherSideDiagramTable = dti;
                            break;
                        }
                    }
                }

                if (otherSideDiagramTable != null)
                {
                    if (parentTable == null)
                    {
                        parentTable = otherSideDiagramTable;
                    }
                    else if (childTable == null)
                    {
                        childTable = otherSideDiagramTable;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    DiagramTableRelation dtr = new DiagramTableRelation(this, parentTable!, childTable!, r);
                    parentTable!.Relations.Add(dtr);
                    if (parentTable != childTable) childTable!.Relations.Add(dtr);
                    _Relations.Add(dtr);
                    otherSideDiagramTable.ReorderColumns();
                }
            }
        }

        private void ReorderTableColumns()
        {
            foreach (DiagramTable dt in Tables)
            {
                dt.ReorderColumns();
            }
        }

        public void RecalculateLocations()
        {
            foreach (DiagramTable dt in Tables)
            {
                dt.RecalculateRelationPoints();
            }

            if (SuggestedRelatedTables != null)
            {
                foreach (DiagramTable dt in SuggestedRelatedTables)
                {
                    dt.RecalculateRelationPoints();
                }
            }

            foreach (DiagramTable dt in Tables)
            {
                dt.DistributeRelationPoints();
            }

            if (SuggestedRelatedTables != null)
            {
                foreach (DiagramTable dt in SuggestedRelatedTables)
                {
                    dt.DistributeRelationPoints();
                }
            }

            foreach (DiagramTableRelation dr in Relations)
            {
                dr.RecalculateBezierPoints();
                dr.RecalculateBoundingBox();
            }

            if (SuggestedRelatedTables != null)
            {
                foreach (DiagramTable dt in SuggestedRelatedTables)
                {
                    foreach (DiagramTableRelation dr in dt.Relations)
                    {
                        dr.RecalculateBezierPoints();
                        dr.RecalculateBoundingBox();
                    }
                }
            }
        }
        public bool UpdateBoundingBox()
        {
            Rectangle r = new Rectangle(0, 0, 0, 0);

            if (Tables.Count > 0)
            {
                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;

                foreach (DiagramTable dt in Tables)
                {
                    minX = Math.Min(minX, dt.BoundingBox.X);
                    minY = Math.Min(minY, dt.BoundingBox.Y);
                    maxX = Math.Max(maxX, dt.BoundingBox.X + dt.BoundingBox.Width);
                    maxY = Math.Max(maxY, dt.BoundingBox.Y + dt.BoundingBox.Height);
                }

                if (SuggestedRelatedTables != null)
                {
                    foreach (DiagramTable dt in SuggestedRelatedTables)
                    {
                        minX = Math.Min(minX, dt.BoundingBox.X);
                        minY = Math.Min(minY, dt.BoundingBox.Y);
                        maxX = Math.Max(maxX, dt.BoundingBox.X + dt.BoundingBox.Width);
                        maxY = Math.Max(maxY, dt.BoundingBox.Y + dt.BoundingBox.Height);
                    }
                }

                r.X = minX;
                r.Y = minY;
                r.Width = maxX - minX;
                r.Height = maxY - minY;
            }

            bool boundsChanged = !r.Equals(_BoundingBox);

            _BoundingBox = r;
            _MaxBoundingBox = new Rectangle(_BoundingBox.X + (_BoundingBox.Width - MaxWidth) / 2, _BoundingBox.Y + (_BoundingBox.Height - MaxHeight) / 2, MaxWidth, MaxHeight);
            _Center = null;

            return boundsChanged;
        }

        public void SaveFile(string filename)
        {
            UpdateBoundingBox();

            XmlDocument xd = new XmlDocument();

            xd.AppendChild(xd.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement root = xd.CreateElement("pgmulti_diagram");
            root.SetAttribute("application_version", Application.ProductVersion);
            root.SetAttribute("file_version", DiagramFileVersion.ToString());
            xd.AppendChild(root);

            XmlElement xeTables = xd.CreateElement("tables");
            root.AppendChild(xeTables);

            foreach (DiagramTable dt in Tables)
            {
                xeTables.AppendChild(dt.ToXml(xd, _BoundingBox.Location));
            }

            XmlElement xeRelations = xd.CreateElement("relations");
            root.AppendChild(xeRelations);

            foreach (DiagramTableRelation r in Relations)
            {
                xeRelations.AppendChild(r.ToXml(xd));
            }

            xd.Save(filename);
        }

        public virtual void StartDrag(Point p)
        {
            _StartDraggingPoint = p;
        }

        public void MoveDrag(Point p)
        {
            if (p != _StartDraggingPoint)
            {
                _CurrentDraggingPoint = p;
            }
        }

        public void CompleteDrag(Point p)
        {
            if (!_StartDraggingPoint.HasValue) throw new NotSupportedException();

            Translate.X += p.X - _StartDraggingPoint.Value.X;
            Translate.Y += p.Y - _StartDraggingPoint.Value.Y;
            _StartDraggingPoint = null;
            _CurrentDraggingPoint = null;
        }

        public void CancelDrag()
        {
            _StartDraggingPoint = null;
            _CurrentDraggingPoint = null;
        }

        public void CenterTo(Rectangle dcRectangleViewPort, Point dcPoint)
        {
            Translate.X += dcRectangleViewPort.X + dcRectangleViewPort.Width / 2 - dcPoint.X;
            Translate.Y += dcRectangleViewPort.Y + dcRectangleViewPort.Height / 2 - dcPoint.Y;
        }

        public PointF ProjectToFloat(Point p)
        {
            return new PointF((p.X + Translate.X) * Scale, (p.Y + Translate.Y) * Scale);
        }

        public Point ProjectToInt(Point p)
        {
            return new Point((int)((p.X + Translate.X) * Scale), (int)((p.Y + Translate.Y) * Scale));
        }

        public RectangleF ProjectToFloat(Rectangle r)
        {
            PointF p0 = ProjectToFloat(r.Location);

            return new RectangleF(p0.X, p0.Y, r.Width * Scale, r.Height * Scale);
        }

        public Rectangle ProjectToInt(Rectangle r)
        {
            Point p0 = ProjectToInt(r.Location);

            return new Rectangle(p0.X, p0.Y, (int)(r.Width * Scale), (int)(r.Height * Scale));
        }

        public Point UnProject(PointF p)
        {
            return new Point((int)(p.X / Scale - Translate.X), (int)(p.Y / Scale - Translate.Y));
        }

        public Point UnProject(Point p)
        {
            return new Point((int)(p.X / Scale - Translate.X), (int)(p.Y / Scale - Translate.Y));
        }

        public Rectangle UnProject(RectangleF r)
        {
            Point p0 = UnProject(r.Location);

            return new Rectangle(p0.X, p0.Y, (int)(r.Width / Scale), (int)(r.Height / Scale));
        }

        public void Draw(Graphics g, Rectangle gcClipRectangle)
        {
            Rectangle dcClipRectangle = UnProject(gcClipRectangle);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ScaleTransform(Scale, Scale);
            g.TranslateTransform(Translate.X, Translate.Y);

            Rectangle backgroundRectangle = MaxBoundingBox;

            if (Dragging)
            {
                g.TranslateTransform(_CurrentDraggingPoint!.Value.X - _StartDraggingPoint!.Value.X, _CurrentDraggingPoint!.Value.Y - _StartDraggingPoint!.Value.Y);
                dcClipRectangle.X -= _CurrentDraggingPoint!.Value.X - _StartDraggingPoint!.Value.X;
                dcClipRectangle.Y -= _CurrentDraggingPoint!.Value.Y - _StartDraggingPoint!.Value.Y;
            }

            g.FillRectangle(_BackgroundBrush, backgroundRectangle);
            //g.DrawRectangle(_BorderPen, backgroundRectangle);

            foreach (DiagramObject dro in Objects.OrderBy(droi => droi.ZIndex))
            {
                if (dro.Dragging || dro.IntersectsWith(dcClipRectangle))
                {
                    dro.Draw(g);
                }
            }
        }

        internal void UpdateDiagramRelocator(DiagramTable dt)
        {
            if (_DiagramRelocator != null)
            {
                _DiagramRelocator.UpdateDiagramTableLocation(dt);
            }
        }

        public void ReorderZIndex()
        {
            int i = 0;
            foreach (DiagramObject dro in Objects.OrderBy(droi => droi is DiagramTable).ThenBy(droi => droi.Suggested).ThenBy(droi => droi.Selected))
            {
                dro.ZIndex = i++;
            }
        }

        public void IncludeSuggestedTable(DiagramTable st)
        {
            if (!st.Suggested) throw new ArgumentException();

            foreach (DiagramTableRelation dri in st.Relations)
            {
                Relations.Add(dri);
                dri.Suggested = false;
            }

            Tables.Add(st);
            st.Suggested = false;
            SuggestedRelatedTables!.Remove(st);
        }
    }
}

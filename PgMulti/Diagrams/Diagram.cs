using PgMulti.AppData;
using PgMulti.DataStructure;
using PgMulti.Diagrams.Efdg;
using System.Text;
using System.Xml;
using static PgMulti.Forms.DiagramPreviewForm;

namespace PgMulti.Diagrams
{
    public class Diagram
    {
        private const int DiagramFileVersion = 1;
        private const int MinDragLength = 10;

        public const int MaxWidth = 10000;
        public const int MaxHeight = 10000;

        public float Scale = 1.0f;
        public PointF Translate = new PointF(0.0f, 0.0f);

        private List<DiagramTable> _Tables;
        private List<DiagramRelation> _Relations;
        private List<DiagramTable>? _SuggestedRelatedTables = null;

        private Point? _StartDraggingPoint = null;
        private Point? _CurrentDraggingPoint = null;

        private DiagramTable? _RelatingTable = null;
        private Point? _DcLastMousePositionWhenRelatingTable = null;

        private DiagramRelocator? _DiagramRelocator = null;

        private Rectangle _BoundingBox;
        private Rectangle _MaxBoundingBox;

        private static Pen _RelatingTablesPen = new Pen(Color.FromArgb(255, 0, 0, 255), 5) { DashPattern = new float[] { 5, 5 } };
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

                DiagramRelation dr = new DiagramRelation(dg, parentTable, childTable, xeRelation);

                dg.Relations.Add(dr);
                dr.ParentTable.Relations.Add(dr);
                if (dr.ParentTable != dr.ChildTable) dr.ChildTable.Relations.Add(dr);

            }

            dg.ReorderZIndex();

            return dg;
        }

        public Diagram()
        {
            _Tables = new List<DiagramTable>();
            _Relations = new List<DiagramRelation>();
            UpdateBoundingBox();
        }

        public List<DiagramTable> Tables { get => _Tables; }
        public List<DiagramRelation> Relations { get => _Relations; }

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
                        foreach (DiagramRelation dr in dt.Relations)
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

                        foreach (DiagramRelation dr in dt.Relations)
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

        public DiagramTable? RelatingTable
        {
            get
            {
                return _RelatingTable;
            }
        }

        public Point? DcLastMousePositionWhenRelatingTable
        {
            get
            {
                return _DcLastMousePositionWhenRelatingTable;
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


        public DiagramTable AddTableFromDB(Table t, out bool isNewTable)
        {
            DiagramTable? dt = FindTable(t);

            if (dt == null)
            {
                dt = new DiagramTable(this, t);
                dt.ReorderColumns();
                AddNewDiagramTable(dt);
                isNewTable = true;
            }
            else
            {
                dt.UpdateFrom(t);
                isNewTable = false;
            }

            return dt;
        }

        public void AddNewDiagramTables(List<DiagramTable> newDiagramTables)
        {
            foreach (DiagramTable t in newDiagramTables)
            {
                AddNewDiagramTable(t);
            }
        }

        public List<DiagramTable> AddDBTables(List<Table> tables)
        {
            List<DiagramTable> newDiagramTables = new List<DiagramTable>();
            foreach (Table t in tables)
            {
                bool isNewTable;
                DiagramTable dt = AddTableFromDB(t, out isNewTable);
                if (isNewTable) newDiagramTables.Add(dt);
            }

            foreach (Table childTable in tables)
            {
                DiagramTable childDiagramTable = FindTable(childTable)!;

                foreach (DiagramRelation dtr in childDiagramTable.ParentRelations.Where(i => tables.Any(j => j.IdSchema == i.ParentTable.SchemaName && j.Id == i.ParentTable.TableName)))
                {
                    childDiagramTable.Relations.Remove(dtr);
                    dtr.ParentTable.Relations.Remove(dtr);
                    Relations.Remove(dtr);
                }

                foreach (TableRelation tr in childTable.ParentRelations.Where(i => tables.Contains(i.ParentTable!)))
                {
                    DiagramTable parentDiagramTable  = FindTable(tr.ParentTable!)!;
                    DiagramRelation dtr = new DiagramRelation(this, parentDiagramTable, childDiagramTable, tr);
                    parentDiagramTable.Relations.Add(dtr);
                    if (parentDiagramTable != childDiagramTable) childDiagramTable.Relations.Add(dtr);
                    Relations.Add(dtr);
                }
            }
            
            return newDiagramTables;
        }

        public void AddNewDiagramTable(DiagramTable dt)
        {
            _Tables.Add(dt);
            Refresh();

            if (DiagramRelocator != null)
            {
                List<DiagramTable> l = new List<DiagramTable>();
                l.Add(dt);
                DiagramRelocator.Add(l);
            }
            UpdateDiagramRelocator(dt);
        }

        public void RemoveTable(DiagramTable dt)
        {
            foreach (DiagramRelation dr in dt.Relations)
            {
                _Relations.Remove(dr);
                dt.OtherTableInRelation(dr).Relations.Remove(dr);
            }

            _Tables.Remove(dt);

            Refresh();
        }

        public void RemoveRelation(DiagramRelation dr)
        {
            _Relations.Remove(dr);
            dr.ParentTable.Relations.Remove(dr);
            dr.ChildTable.Relations.Remove(dr);

            Refresh();
        }

        public void Refresh()
        {
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

            foreach (DiagramRelation dr in Relations)
            {
                dr.RecalculateBezierPoints();
                dr.RecalculateBoundingBox();
            }

            if (SuggestedRelatedTables != null)
            {
                foreach (DiagramTable dt in SuggestedRelatedTables)
                {
                    foreach (DiagramRelation dr in dt.Relations)
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

            foreach (DiagramRelation r in Relations)
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
            if (Math.Abs(p.X - _StartDraggingPoint!.Value.X) + Math.Abs(p.Y - _StartDraggingPoint.Value.Y) > MinDragLength)
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

        public void StartRelatingTables(DiagramTable t)
        {
            _RelatingTable = t;
        }

        public void UpdateMousePositionRelatingTables(Point p)
        {
            if (_RelatingTable == null) throw new NotSupportedException();
            _DcLastMousePositionWhenRelatingTable = p;
        }

        public void StopRelatingTables()
        {
            _RelatingTable = null;
            _DcLastMousePositionWhenRelatingTable = null;
        }

        public void CenterTo(Rectangle dcRectangleViewPort, Point dcPoint)
        {
            Translate.X += dcRectangleViewPort.X + dcRectangleViewPort.Width / 2 - dcPoint.X;
            Translate.Y += dcRectangleViewPort.Y + dcRectangleViewPort.Height / 2 - dcPoint.Y;
        }

        public void ZoomFull(Size canvasSize, int margin, bool minScaleMode)
        {
            float scaleX = ((float)canvasSize.Width - 2 * margin) / BoundingBox.Width;
            float scaleY = ((float)canvasSize.Height - 2 * margin) / BoundingBox.Height;
            float scale;

            if (minScaleMode)
            {
                scale = Math.Min(scaleX, scaleY);
            }
            else
            {
                scale = (scaleX + scaleY) / 2.0f;
            }

            if (scale < 0.1f) scale = 0.1f;
            if (scale > 1.0f) scale = 1.0f;

            Scale = scale;

            Rectangle dcRectangleViewPort = UnProject(new Rectangle(0, 0, canvasSize.Width, canvasSize.Height));
            CenterTo(dcRectangleViewPort, new Point(BoundingBox.X + BoundingBox.Width / 2, BoundingBox.Y + BoundingBox.Height / 2));
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

        public void Draw(Graphics g, Rectangle gcClipRectangle, bool clearBackground, bool highResolution)
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

            if (clearBackground) g.FillRectangle(_BackgroundBrush, backgroundRectangle);
            //g.DrawRectangle(_BorderPen, backgroundRectangle);

            if (_RelatingTable != null && _DcLastMousePositionWhenRelatingTable.HasValue)
            {
                g.DrawLine(_RelatingTablesPen, _RelatingTable.Center, _DcLastMousePositionWhenRelatingTable.Value);
            }

            foreach (DiagramObject dro in Objects.OrderBy(droi => droi.ZIndex))
            {
                if (dro.Dragging || dro.IntersectsWith(dcClipRectangle))
                {
                    dro.Draw(g, highResolution);
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

            foreach (DiagramRelation dri in st.Relations)
            {
                Relations.Add(dri);
                dri.Suggested = false;
            }

            Tables.Add(st);
            st.Suggested = false;
            SuggestedRelatedTables!.Remove(st);
        }

        public void WriteSqlScriptFullDefinition(StringBuilder sb)
        {
            if (sb == null) throw new ArgumentException();

            sb.AppendLine("/* Schemas */");

            foreach (string schemaName in Tables.Select(i => i.SchemaName).Distinct())
            {
                sb.AppendLine($"CREATE SCHEMA IF NOT EXISTS {SqlSyntax.PostgreSqlGrammar.IdToString(schemaName)};");
            }

            sb.AppendLine("/* Tables */");

            foreach (DiagramTable dt in Tables)
            {
                dt.WriteSqlSentenceCreate(sb);
            }

            sb.AppendLine("/* Relations */");

            foreach (DiagramRelation dtr in Relations)
            {
                dtr.WriteSqlSentenceAlterTableAddConstraint(sb);
            }

            sb.AppendLine("/* Foreign key indexes */");

            foreach (DiagramRelation dtr in Relations)
            {
                dtr.WriteSqlSentenceCreateForeignKeyIndex(sb);
            }
        }

        public void WriteSqlScriptTransformDb(StringBuilder sb, DB db)
        {
            if (sb == null || db == null) throw new ArgumentException();

            sb.AppendLine("/* Schemas */");

            foreach (string schemaName in Tables.Select(i => i.SchemaName).Distinct())
            {
                sb.AppendLine($"CREATE SCHEMA IF NOT EXISTS {SqlSyntax.PostgreSqlGrammar.IdToString(schemaName)};");
            }

            List<DiagramTable> newTables = new List<DiagramTable>();
            List<DiagramTable> existingTables = new List<DiagramTable>();

            foreach (DiagramTable dt in Tables)
            {
                Table? t = dt.FindInDB(db);

                if (t == null)
                {
                    newTables.Add(dt);
                }
                else
                {
                    existingTables.Add(dt);
                }
            }

            sb.AppendLine("/* New tables to create */");

            foreach (DiagramTable dt in newTables)
            {
                dt.WriteSqlSentenceCreate(sb);
            }

            sb.AppendLine("/* Existing tables to transform */");

            foreach (DiagramTable dt in existingTables)
            {
                dt.WriteSqlSentenceAlter(sb, dt.FindInDB(db)!);
            }


            List<DiagramRelation> newRelations = new List<DiagramRelation>();
            List<DiagramRelation> existingRelations = new List<DiagramRelation>();
            List<TableRelation> extraRelations = new List<TableRelation>();

            foreach (DiagramRelation dtr in Relations)
            {
                TableRelation? tr = dtr.FindInDB(db);

                if (tr == null)
                {
                    newRelations.Add(dtr);
                }
                else
                {
                    existingRelations.Add(dtr);
                }
            }

            foreach (TableRelation tr in db.Schemas
                    .SelectMany(i => i.Tables.Where(j => Tables.Any(k => k.SchemaName == j.IdSchema && k.TableName == j.Id)))
                    .SelectMany(i => i.Relations.Where(j => j.ChildTable == i && Tables.Any(k => k.SchemaName == j.IdParentSchema && k.TableName == j.IdParentTable)))
                    .Where(i => !Tables.First(j => i.IdChildSchema == j.SchemaName && i.IdChildTable == j.TableName).ParentRelations.Any(j => j.Id == i.Id))
                )
            {
                extraRelations.Add(tr);
            }

            sb.AppendLine("/* New relations to create */");

            foreach (DiagramRelation dtr in newRelations)
            {
                dtr.WriteSqlSentenceAlterTableAddConstraint(sb);
            }

            sb.AppendLine("/* Existing relations to transform */");

            foreach (DiagramRelation dtr in existingRelations)
            {
                TableRelation tr = dtr.FindInDB(db)!;

                if (dtr.ParentTable.SchemaName != tr.IdParentSchema
                        || dtr.ParentTable.TableName != tr.IdParentTable
                        || DiagramRelation.PropagationOptionsToString(dtr.OnDelete).ToUpperInvariant() != tr.OnDelete
                        || DiagramRelation.PropagationOptionsToString(dtr.OnUpdate).ToUpperInvariant() != tr.OnUpdate
                        || dtr.ParentTableColumns.Count != tr.ParentColumns.Length
                        || dtr.ChildTableColumns.Count != tr.ChildColumns.Length
                        || dtr.ParentTableColumns.Where((i, index) => i.ColumnName != tr.ParentColumns[index]).Any()
                        || dtr.ChildTableColumns.Where((i, index) => i.ColumnName != tr.ChildColumns[index]).Any()
                    )
                {
                    dtr.WriteSqlSentenceAlterTableDropConstraint(sb);
                    dtr.WriteSqlSentenceAlterTableAddConstraint(sb);
                }
            }

            sb.AppendLine("/* Extra relations to delete */");

            foreach (TableRelation tr in extraRelations)
            {
                sb.AppendLine($"ALTER TABLE {SqlSyntax.PostgreSqlGrammar.IdToString(tr.ChildTable!.IdSchema)}.{SqlSyntax.PostgreSqlGrammar.IdToString(tr.ChildTable.Id)} DROP CONSTRAINT {SqlSyntax.PostgreSqlGrammar.IdToString(tr.Id)};");
            }

            sb.AppendLine("/* Foreign key indexes in new relations to create */");
            sb.AppendLine("/* (It needs review, as existing indices are not being checked and could be duplicated) */");

            foreach (DiagramRelation dtr in newRelations)
            {
                dtr.WriteSqlSentenceCreateForeignKeyIndex(sb);
            }
        }
    }
}

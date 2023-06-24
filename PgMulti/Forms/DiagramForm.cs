using FastColoredTextBoxNS;
using PgMulti.AppData;
using PgMulti.DataStructure;
using PgMulti.Diagrams;
using PgMulti.Diagrams.Efdg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PgMulti.Forms
{
    public partial class DiagramForm : Form
    {
        #region "Global"
        private static Cursor _DraggingCursor = new Cursor(Properties.Resources.hand_closed.GetHicon());
        private static Cursor _MoveCursor = new Cursor(Properties.Resources.hand_open.GetHicon());
        private static Brush _CheckedToolStripButton = new SolidBrush(SystemColors.ActiveCaption);

        private Data _Data;
        private Diagram _Diagram;
        private string _Filename;
        private DB? _PreselectedDB;

        private Canvas _Canvas;
        private ExpandDiagramPanel? _ExpandDiagramPanel = null;

        private DiagramObject? _DraggingObject = null;
        private DiagramObject? _HighlightedObject = null;
        private DiagramObject? _SelectedObject = null;
        private bool _DraggingDiagram = false;

        private DiagramTable? _ResizingTable = null;
        private int? _ResizingTableInitY = null;
        private int? _ResizingTableInitVisibleColumns = null;

        private bool _CtrlKeyPressed = false;
        private bool _ShiftKeyPressed = false;

        private bool _PendingSave = false;
        private bool _PreviousSavingError = false;


        private FormWindowState _LastWindowState = FormWindowState.Normal;
        private Size _LastDCCanvasSize;

        public DiagramForm(Data d, Diagram dg, string filename, DB? preselectedDB)
        {
            InitializeComponent();

            _Canvas = new Canvas();
            _Canvas.Dock = DockStyle.Fill;
            _Canvas.Location = new Point(0, 0);
            _Canvas.Size = new System.Drawing.Size(800, 395);
            _Canvas.TabIndex = 0;
            _Canvas.Paint += _Canvas_Paint;
            _Canvas.MouseDown += _Canvas_MouseDown;
            _Canvas.MouseMove += _Canvas_MouseMove;
            _Canvas.MouseUp += _Canvas_MouseUp;
            _Canvas.MouseWheel += _Canvas_MouseWheel;
            _Canvas.MouseLeave += _Canvas_MouseLeave;
            _Canvas.BackColor = Color.FromArgb(255, 200, 200, 200);

            this.ts.Renderer = new CheckedToolStripButtonRenderer();
            this.tsc.ContentPanel.Controls.Add(_Canvas);

            InitializeText();

            _Data = d;
            _Diagram = dg;
            _Filename = filename;

            DoubleBuffered = true;
            Text = string.Format(Properties.Text.diagram_title, Path.GetFileNameWithoutExtension(filename));
            _Diagram.RecalculateLocations();
            UpdateItemsInTscbTables();
            _LastDCCanvasSize = new Size(_Canvas.Width, _Canvas.Height);

            _PreselectedDB = preselectedDB;
        }

        private DB? _SuggestAddRelatedTablesDB;
        public DB? SuggestAddRelatedTablesDB
        {
            get
            {
                return _SuggestAddRelatedTablesDB;
            }

            set
            {
                _SuggestAddRelatedTablesDB = value;
            }
        }

        public void AddTables(List<Table> tables)
        {
            foreach (Table t in tables)
            {
                _Diagram.AddTable(t);
            }

            _Diagram.RecalculateLocations();

            _Canvas.Invalidate();
            _PendingSave = true;
            tmrSave.Enabled = true;
            UpdateItemsInTscbTables();

            Rectangle dcRectangleViewPort = _Diagram.UnProject(new Rectangle(0, 0, _Canvas.Width, _Canvas.Height));
            _Diagram.CenterTo(dcRectangleViewPort, _Diagram.Center);
        }

        public ExpandDiagramPanel OpenExpandDiagramPanel()
        {
            tsbExpandDiagram.Checked = true;
            return _ExpandDiagramPanel!;
        }

        public void Save()
        {
            string path = Path.GetDirectoryName(_Filename)!;
            string name = Path.GetFileNameWithoutExtension(_Filename);
            string ext = Path.GetExtension(_Filename);

            string tmp = Path.Combine(path, Path.ChangeExtension(name + "_old", ext));
            File.Move(_Filename, tmp);
            _Diagram.SaveFile(_Filename);
            File.Delete(tmp);

            _PendingSave = false;
        }

        public void ZoomFull(bool minScaleMode)
        {
            float scaleX = (float)_Canvas.Width / _Diagram.BoundingBox.Width;
            float scaleY = (float)_Canvas.Height / _Diagram.BoundingBox.Height;
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

            _Diagram.Scale = scale;

            Rectangle dcRectangleViewPort = _Diagram.UnProject(new Rectangle(0, 0, _Canvas.Width, _Canvas.Height));
            _Diagram.CenterTo(dcRectangleViewPort, new Point(_Diagram.BoundingBox.X + _Diagram.BoundingBox.Width / 2, _Diagram.BoundingBox.Y + _Diagram.BoundingBox.Height / 2));

            _Invalidate();
        }


        public void UpdateSuggestedTables(DiagramTable? selectedTable)
        {
            List<DiagramTable>? l = null;

            if (SuggestAddRelatedTablesDB != null && selectedTable != null)
            {
                Schema? s = SuggestAddRelatedTablesDB.Schemas.FirstOrDefault(si => si.Id == selectedTable.SchemaName);

                if (s != null)
                {
                    Table? t = s.Tables.FirstOrDefault(ti => ti.Id == selectedTable.TableName);
                    DiagramRelocator? reloc = _Diagram.DiagramRelocator;
                    _Diagram.DiagramRelocator = null;

                    if (t != null)
                    {
                        l = new List<DiagramTable>();
                        Random rand = new Random();

                        foreach (TableRelation r in t.Relations)
                        {
                            if (r.ParentTable == r.ChildTable) continue;

                            Table relatedTable;

                            if (r.ParentTable == t)
                            {
                                relatedTable = r.ChildTable!;
                            }
                            else
                            {
                                relatedTable = r.ParentTable!;
                            }

                            if (
                                !_Diagram.Tables.Any(dti => dti.SchemaName == relatedTable.IdSchema && dti.TableName == relatedTable.Id)
                                && !l.Any(dti => dti.SchemaName == relatedTable.IdSchema && dti.TableName == relatedTable.Id)

                            )
                            {
                                DiagramTable newDiagramTable = new DiagramTable(_Diagram, relatedTable);
                                newDiagramTable.Suggested = true;

                                foreach (TableRelation relatedTableRelation in relatedTable.Relations)
                                {
                                    DiagramTable? parentDiagramTable;
                                    if (relatedTableRelation.ParentTable == relatedTable)
                                    {
                                        parentDiagramTable = newDiagramTable;
                                    }
                                    else
                                    {
                                        parentDiagramTable = _Diagram.FindTable(relatedTableRelation.ParentTable!);
                                        if (parentDiagramTable == null) continue;
                                    }

                                    DiagramTable? childDiagramTable;
                                    if (relatedTableRelation.ChildTable == relatedTable)
                                    {
                                        childDiagramTable = newDiagramTable;
                                    }
                                    else
                                    {
                                        childDiagramTable = _Diagram.FindTable(relatedTableRelation.ChildTable!);
                                        if (childDiagramTable == null) continue;
                                    }

                                    DiagramTableRelation newDiagramTableRelation = new DiagramTableRelation(_Diagram, parentDiagramTable, childDiagramTable, relatedTableRelation);
                                    newDiagramTableRelation.Suggested = true;

                                    parentDiagramTable.Relations.Add(newDiagramTableRelation);
                                    if (parentDiagramTable != childDiagramTable) childDiagramTable.Relations.Add(newDiagramTableRelation);
                                }

                                double rx;
                                double ry;

                                switch (rand.Next(3))
                                {
                                    case 0:
                                        rx = -1.0;
                                        ry = (rand.NextDouble() - 0.5) * 2.0;
                                        break;
                                    case 1:
                                        rx = (rand.NextDouble() - 0.5) * 2.0;
                                        ry = (rand.Next(2) == 0) ? -1.0 : 1.0;
                                        break;
                                    case 2:
                                        rx = 1.0;
                                        ry = (rand.NextDouble() - 0.5) * 2.0;
                                        break;
                                    default:
                                        throw new NotSupportedException();
                                }


                                Point p = selectedTable.Center;

                                p.X += (int)((selectedTable.BoundingBox.Width / 2.0 + newDiagramTable.BoundingBox.Height / 2.0 + 100) * rx);
                                p.Y += (int)((selectedTable.BoundingBox.Height / 2.0 + newDiagramTable.BoundingBox.Height / 2.0 + 100) * ry);

                                newDiagramTable.MoveTo(p);

                                l.Add(newDiagramTable);
                            }
                        }
                    }

                    _Diagram.DiagramRelocator = reloc;
                }
            }

            if (_Diagram.SuggestedRelatedTables != null || l != null)
            {
                _Diagram.SuggestedRelatedTables = l;

                if (l != null)
                {
                    foreach (DiagramTable dti in l)
                    {
                        _Diagram.UpdateDiagramRelocator(dti);
                    }
                }

                _Diagram.RecalculateLocations();
                _Canvas.Invalidate();
            }
        }

        private Rectangle _RedrawWholeTable(DiagramTable dt)
        {
            Rectangle? dcClipRectangle = dt.BoundingBox;

            dt.RecalculateRelationPoints();
            dt.DistributeRelationPoints();
            foreach (DiagramTableRelation dr in dt.Relations)
            {
                DiagramTable dt2 = (dr.ParentTable == _ResizingTable ? dr.ChildTable : dr.ParentTable);
                dt2.DistributeRelationPoints();

                dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);
                dr.RecalculateBezierPoints();
                dr.RecalculateBoundingBox();
                dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);

                foreach (DiagramTableRelation dr2 in dt2.Relations)
                {
                    dcClipRectangle = Combine(dcClipRectangle, dr2.BoundingBox);
                    dr2.RecalculateBezierPoints();
                    dr2.RecalculateBoundingBox();
                    dcClipRectangle = Combine(dcClipRectangle, dr2.BoundingBox);
                }
            }

            return dcClipRectangle!.Value;
        }

        #endregion "Global"

        #region "Form events"
        private void DiagramForm_Load(object sender, EventArgs e)
        {
            tsbZoomFull_Click(sender, e);
        }

        private void DiagramForm_Resize(object sender, EventArgs e)
        {
            _LastWindowState = WindowState;

            if (WindowState != FormWindowState.Minimized)
            {
                Point lastCanvasCenter = _Diagram.UnProject(new Point(_LastDCCanvasSize.Width / 2, _LastDCCanvasSize.Height / 2));
                Rectangle dcRectangleViewPort = _Diagram.UnProject(new Rectangle(0, 0, _Canvas.Width, _Canvas.Height));
                _Diagram.CenterTo(dcRectangleViewPort, lastCanvasCenter);

                _Invalidate();
                _LastDCCanvasSize = new Size(_Canvas.Width, _Canvas.Height);
            }
        }

        private void DiagramForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_PendingSave)
            {
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(this, string.Format(Properties.Text.error_saving_diagram_on_close, ex.Message), Properties.Text.error, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void DiagramForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _CtrlKeyPressed = true;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                _ShiftKeyPressed = true;
            }
        }

        private void DiagramForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _CtrlKeyPressed = false;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                _ShiftKeyPressed = false;
            }
        }

        #endregion "Form events"

        #region "Other controls events"
        private void tsbAddTables_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string>> preselectedTableIds = new List<Tuple<string, string>>();

            foreach (DiagramTable dt in _Diagram.Tables)
            {
                preselectedTableIds.Add(new Tuple<string, string>(dt.SchemaName, dt.TableName));
            }

            SelectTablesForm stf = new SelectTablesForm(_Data, preselectedTableIds);
            if (stf.ShowDialog(this) != DialogResult.OK) return;

            AddTables(stf.SelectedTables!);
        }

        private void tsbExpandDiagram_CheckedChanged(object sender, EventArgs e)
        {
            if (tsbExpandDiagram.Checked)
            {
                _ExpandDiagramPanel = new ExpandDiagramPanel(_Data, _PreselectedDB, this, _Diagram, _Canvas);
                _ExpandDiagramPanel.Left = 0;
                _ExpandDiagramPanel.Top = 0;
                //_ExpandDiagramPanel.Paint += _ExpandDiagramPanel_Paint;
                _Canvas.Controls.Add(_ExpandDiagramPanel);
                _Canvas.Controls.SetChildIndex(_ExpandDiagramPanel, 0);
            }
            else
            {
                _ExpandDiagramPanel!.Close();
                _Canvas.Controls.Remove(_ExpandDiagramPanel);
                _ExpandDiagramPanel = null;
            }
        }

        private void tmrSave_Tick(object sender, EventArgs e)
        {
            tmrSave.Enabled = false;

            if (_PendingSave)
            {
                try
                {
                    Save();
                }
                catch (Exception ex)
                {
                    if (!_PreviousSavingError)
                    {
                        MessageBox.Show(this, string.Format(Properties.Text.error_saving_diagram, ex.Message), Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        _PreviousSavingError = true;
                    }
                }
            }
        }

        private void tsbZoomFull_Click(object sender, EventArgs e)
        {
            ZoomFull(false);
        }

        private bool _Ignore_tscbTables_SelectedIndexChanged = false;
        private void tscbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Ignore_tscbTables_SelectedIndexChanged) return;

            if (tscbTables.SelectedItem == null) return;
            DiagramTable dt = (DiagramTable)tscbTables.SelectedItem;

            if (_SelectedObject != null)
            {
                _SelectedObject.Selected = false;
            }

            _SelectedObject = dt;

            if (_SelectedObject != null)
            {
                _SelectedObject.Selected = true;
            }

            _Diagram.ReorderZIndex();

            Rectangle dcRectangleViewPort = _Diagram.UnProject(new Rectangle(0, 0, _Canvas.Width, _Canvas.Height));
            _Diagram.CenterTo(dcRectangleViewPort, dt.Center);

            _Invalidate();
        }

        private void UpdateItemsInTscbTables()
        {
            tscbTables.Items.Clear();
            foreach (DiagramTable dt in _Diagram.Tables)
            {
                tscbTables.Items.Add(dt);
            }
        }

        #endregion

        #region "_Canvas events"
        private void _Canvas_MouseWheel(object? sender, MouseEventArgs e)
        {
            Point dcMouseLocation = _Diagram.UnProject(e.Location);

            DiagramTable? scrollableTable = null;
            foreach (DiagramTable dt in _Diagram.Tables.OrderByDescending(dti => dti.ZIndex))
            {
                if (dt.IsSelectable(dcMouseLocation))
                {
                    if (dt.IsScrollable(dcMouseLocation))
                    {
                        scrollableTable = dt;
                    }
                    break;
                }
            }

            if (scrollableTable == null)
            {
                float scale0 = _Diagram.Scale;
                if (e.Delta > 0)
                {
                    _Diagram.Scale = Math.Min(1.0f, _Diagram.Scale * (1.0f + e.Delta / 1200.0f));
                }
                else
                {
                    _Diagram.Scale = Math.Max(0.1f, _Diagram.Scale / (1.0f - e.Delta / 1200.0f));
                }

                if (scale0 != _Diagram.Scale)
                {
                    _Diagram.Translate.X += e.X / _Diagram.Scale - e.X / scale0;
                    _Diagram.Translate.Y += e.Y / _Diagram.Scale - e.Y / scale0;

                    _Invalidate();
                }
            }
            else
            {
                int delta;

                delta = Math.Max(1, Math.Min(scrollableTable.VisibleColumns, 3) - 1) * DiagramTable.SingleColumnHeight;

                if (e.Delta > 0)
                {
                    delta *= -1;
                }

                scrollableTable.ColumnsScroll += delta;
                _Invalidate(_Diagram.ProjectToInt(scrollableTable.ColumnsBoundingBox));
            }
        }

        private void _Canvas_MouseDown(object? sender, MouseEventArgs e)
        {
            Point dcMouseLocation = _Diagram.UnProject(e.Location);
            if (e.Button == MouseButtons.Right)
            {
                _DraggingDiagram = true;
                _Diagram.StartDrag(dcMouseLocation);
            }
            else if (e.Button == MouseButtons.Left)
            {
                ClickOnPoint(e.Location, dcMouseLocation, _CtrlKeyPressed || _ShiftKeyPressed, true);
            }
        }

        private void ClickOnPoint(Point ccMouseLocation, Point dcMouseLocation, bool keyModifier, bool canInitDrag)
        {
            Rectangle? dcClipRectangle = null;

            DiagramObject? nextSelectedObject = null;

            List<DiagramObject> selectableObjects = new List<DiagramObject>();
            foreach (DiagramObject dro in _Diagram.Objects)
            {
                if (dro.IsSelectable(dcMouseLocation))
                {
                    selectableObjects.Add(dro);
                }
            }

            if (selectableObjects.Count > 0)
            {
                float minRank = float.MaxValue;
                foreach (DiagramObject dro in selectableObjects.OrderByDescending(droi => droi.ZIndex))
                {
                    float rank = dro.SelectingRank(dcMouseLocation);
                    if (rank < minRank && rank <= dro.SelectingTolerance)
                    {
                        nextSelectedObject = dro;
                        minRank = rank;
                        if (rank == 0) break;
                    }
                }
            }

            bool resize = false;
            if (nextSelectedObject != null && nextSelectedObject is DiagramTable)
            {
                DiagramTable dt = (DiagramTable)nextSelectedObject;
                int bottomY = _Diagram.ProjectToInt(new Point(0, dt.BoundingBox.Y + dt.BoundingBox.Height)).Y;
                if (ccMouseLocation.Y > bottomY - 10)
                {
                    resize = true;
                }
            }

            if (resize)
            {
                _ResizingTable = (DiagramTable)nextSelectedObject!;
                _ResizingTableInitY = dcMouseLocation.Y;
                _ResizingTableInitVisibleColumns = _ResizingTable.VisibleColumns;
                nextSelectedObject = null;
                canInitDrag = false;
            }
            else if ((nextSelectedObject != null) || !canInitDrag)
            {
                if (nextSelectedObject != _SelectedObject)
                {
                    if (_SelectedObject != null && !keyModifier)
                    {
                        _SelectedObject.Selected = false;
                        dcClipRectangle = Combine(dcClipRectangle, _SelectedObject.BoundingBox);

                        if (_SelectedObject is DiagramTableRelation)
                        {
                            DiagramTableRelation dr = (DiagramTableRelation)_SelectedObject;
                            dcClipRectangle = Combine(dcClipRectangle, dr.ParentTable.BoundingBox);
                            dcClipRectangle = Combine(dcClipRectangle, dr.ChildTable.BoundingBox);
                        }
                    }

                    if (nextSelectedObject == null)
                    {
                        _Ignore_tscbTables_SelectedIndexChanged = true;
                        tscbTables.SelectedIndex = -1;
                        _Ignore_tscbTables_SelectedIndexChanged = false;
                    }
                    else
                    {
                        if (nextSelectedObject.Suggested)
                        {
                            DiagramTable st;

                            if (nextSelectedObject is DiagramTable)
                            {
                                st = (DiagramTable)nextSelectedObject;
                            }
                            else if (nextSelectedObject is DiagramTableRelation)
                            {
                                DiagramTableRelation sr = (DiagramTableRelation)nextSelectedObject;

                                if (sr.ParentTable.Suggested)
                                {
                                    st = sr.ParentTable;
                                }
                                else
                                {
                                    st = sr.ChildTable;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }

                            _Diagram.IncludeSuggestedTable(st);

                            if (keyModifier)
                            {
                                nextSelectedObject = _SelectedObject!;
                            }
                        }

                        if (nextSelectedObject != _SelectedObject)
                        {
                            nextSelectedObject.Selected = true;

                            if (nextSelectedObject.Highlighted)
                            {
                                nextSelectedObject.Highlighted = false;

                                if (nextSelectedObject is DiagramTable)
                                {
                                    DiagramTable dt = (DiagramTable)nextSelectedObject;
                                    foreach (DiagramTableRelation dr in dt.Relations)
                                    {
                                        dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);
                                        dcClipRectangle = Combine(dcClipRectangle, dt.OtherTableInRelation(dr).BoundingBox);
                                    }
                                }
                                else if (nextSelectedObject is DiagramTableRelation)
                                {
                                    DiagramTableRelation dr = (DiagramTableRelation)nextSelectedObject;
                                    dcClipRectangle = Combine(dcClipRectangle, dr.ParentTable.BoundingBox);
                                    dcClipRectangle = Combine(dcClipRectangle, dr.ChildTable.BoundingBox);
                                }
                            }

                            dcClipRectangle = Combine(dcClipRectangle, nextSelectedObject.BoundingBox);

                            _Ignore_tscbTables_SelectedIndexChanged = true;
                            tscbTables.SelectedItem = nextSelectedObject is DiagramTable ? nextSelectedObject : null;
                            _Ignore_tscbTables_SelectedIndexChanged = false;
                        }
                    }

                    if (nextSelectedObject != _SelectedObject)
                    {
                        _SelectedObject = nextSelectedObject;

                        if (nextSelectedObject == null || nextSelectedObject is DiagramTable)
                        {
                            UpdateSuggestedTables((DiagramTable?)nextSelectedObject);
                        }

                        _Diagram.ReorderZIndex();
                    }
                }
            }
            else if (nextSelectedObject == null && canInitDrag)
            {
                _DraggingDiagram = true;
                _Diagram.StartDrag(dcMouseLocation);
            }

            if (nextSelectedObject != null && !nextSelectedObject.Suggested && canInitDrag)
            {
                _DraggingObject = nextSelectedObject;
                _DraggingObject.SetStartDraggingPoint(dcMouseLocation);
            }


            if (dcClipRectangle.HasValue)
            {
                _Invalidate(_Diagram.ProjectToInt(dcClipRectangle.Value));
            }
        }

        private void _Canvas_MouseUp(object? sender, MouseEventArgs e)
        {
            Point dcMouseLocation = _Diagram.UnProject(e.Location);
            Rectangle? dcClipRectangle = null;

            if (_ResizingTable != null)
            {
                dcClipRectangle = Combine(dcClipRectangle, _RedrawWholeTable(_ResizingTable));

                _ResizingTable = null;
                _ResizingTableInitY = null;
                _ResizingTableInitVisibleColumns = null;
                _PendingSave = true;
                tmrSave.Enabled = true;
            }
            else if (_DraggingObject != null)
            {
                Cursor = Cursors.Arrow;
                //dcClipRectangle = Combine(dcClipRectangle, _DraggingObject.BoundingBox);
                _DraggingObject.CompleteDrag(dcMouseLocation);
                if (_Diagram.UpdateBoundingBox())
                {
                    _Invalidate();
                }
                else
                {
                    dcClipRectangle = Combine(dcClipRectangle, _DraggingObject.BoundingBox);
                }

                if (_DraggingObject is DiagramTable)
                {
                    DiagramTable dt1 = (DiagramTable)_DraggingObject;
                    dcClipRectangle = Combine(dcClipRectangle, _RedrawWholeTable(dt1));
                }

                _DraggingObject = null;
                _PendingSave = true;
                tmrSave.Enabled = true;
            }
            else if (_Diagram.Dragging)
            {
                Cursor = _MoveCursor;
                _Diagram.CompleteDrag(dcMouseLocation);
                _Invalidate();
            }
            else if (e.Button == MouseButtons.Left)
            {
                ClickOnPoint(e.Location, dcMouseLocation, _CtrlKeyPressed || _ShiftKeyPressed, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                //_Diagram.CancelDrag();
                ClickOnPoint(e.Location, dcMouseLocation, _CtrlKeyPressed || _ShiftKeyPressed, false);
                cms.Show(_Canvas, e.Location);
            }

            _DraggingDiagram = false;

            if (dcClipRectangle.HasValue)
            {
                _Invalidate(_Diagram.ProjectToInt(dcClipRectangle.Value));
            }
        }

        private void _Canvas_MouseMove(object? sender, MouseEventArgs e)
        {
            Point dcMouseLocation = _Diagram.UnProject(e.Location);
            Rectangle? dcClipRectangle = null;

            if (_ResizingTable != null)
            {
                int delta = (int)Math.Round((dcMouseLocation.Y - _ResizingTableInitY!.Value) / (float)DiagramTable.SingleColumnHeight);

                if (delta != 0)
                {
                    int prevVisibleColumns = _ResizingTable.VisibleColumns;
                    Rectangle prevBoundingBox = _ResizingTable.BoundingBox;
                    _ResizingTable.VisibleColumns = _ResizingTableInitVisibleColumns!.Value + delta;

                    if (prevVisibleColumns != _ResizingTable.VisibleColumns)
                    {
                        dcClipRectangle = Combine(dcClipRectangle, prevBoundingBox);
                        dcClipRectangle = Combine(dcClipRectangle, _RedrawWholeTable(_ResizingTable));
                    }
                }
            }
            else if (_DraggingObject == null && !_DraggingDiagram)
            {
                DiagramObject? prevHighlightedObject = _HighlightedObject;
                _HighlightedObject = null;

                List<DiagramObject> highlightableObjects = new List<DiagramObject>();
                foreach (DiagramObject dro in _Diagram.Objects)
                {
                    if (dro.IsSelectable(dcMouseLocation))
                    {
                        highlightableObjects.Add(dro);
                    }
                }

                if (highlightableObjects.Count > 0)
                {
                    float minRank = float.MaxValue;
                    foreach (DiagramObject dro in highlightableObjects.OrderByDescending(droi => droi.ZIndex))
                    {
                        float rank = dro.SelectingRank(dcMouseLocation);
                        if (rank < minRank && rank <= dro.SelectingTolerance)
                        {
                            _HighlightedObject = dro;
                            minRank = rank;
                            if (rank == 0) break;
                        }
                    }
                }

                if (_HighlightedObject == null)
                {
                    Cursor = _MoveCursor;
                }


                if (_HighlightedObject != prevHighlightedObject)
                {
                    if (prevHighlightedObject != null)
                    {
                        prevHighlightedObject.Highlighted = false;
                        dcClipRectangle = Combine(dcClipRectangle, prevHighlightedObject.BoundingBox);

                        if (prevHighlightedObject is DiagramTable)
                        {
                            DiagramTable dt = (DiagramTable)prevHighlightedObject;
                            foreach (DiagramTableRelation dr in dt.Relations)
                            {
                                dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);
                                dcClipRectangle = Combine(dcClipRectangle, dt.OtherTableInRelation(dr).BoundingBox);
                            }
                        }
                        else if (prevHighlightedObject is DiagramTableRelation)
                        {
                            DiagramTableRelation dr = (DiagramTableRelation)prevHighlightedObject;
                            dcClipRectangle = Combine(dcClipRectangle, dr.ParentTable.BoundingBox);
                            dcClipRectangle = Combine(dcClipRectangle, dr.ChildTable.BoundingBox);
                        }
                    }

                    if (_HighlightedObject != null)
                    {
                        Cursor = Cursors.Arrow;
                        _HighlightedObject.Highlighted = true;
                        dcClipRectangle = Combine(dcClipRectangle, _HighlightedObject.BoundingBox);

                        if (_HighlightedObject is DiagramTable)
                        {
                            DiagramTable dt = (DiagramTable)_HighlightedObject;
                            foreach (DiagramTableRelation dr in dt.Relations)
                            {
                                dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);
                                dcClipRectangle = Combine(dcClipRectangle, dt.OtherTableInRelation(dr).BoundingBox);
                            }
                        }
                        else if (_HighlightedObject is DiagramTableRelation)
                        {
                            DiagramTableRelation dr = (DiagramTableRelation)_HighlightedObject;
                            dcClipRectangle = Combine(dcClipRectangle, dr.ParentTable.BoundingBox);
                            dcClipRectangle = Combine(dcClipRectangle, dr.ChildTable.BoundingBox);
                        }
                    }
                }

                bool resize = false;
                if (_HighlightedObject != null && _HighlightedObject is DiagramTable)
                {
                    DiagramTable dt = (DiagramTable)_HighlightedObject;
                    int bottomY = _Diagram.ProjectToInt(new Point(0, dt.BoundingBox.Y + dt.BoundingBox.Height)).Y;
                    if (e.Location.Y > bottomY - 10)
                    {
                        resize = true;
                    }
                }

                if (resize)
                {
                    Cursor = Cursors.SizeNS;
                }
                else if (Cursor == Cursors.SizeNS)
                {
                    Cursor = Cursors.Arrow;
                }
            }
            else if (_DraggingObject != null)
            {
                if (_DraggingObject is DiagramTable && !_DraggingObject.Dragging)
                {
                    DiagramTable dt = (DiagramTable)_DraggingObject;
                    foreach (DiagramTableRelation dr in dt.Relations)
                    {
                        dcClipRectangle = Combine(dcClipRectangle, dr.BoundingBox);
                    }

                    Cursor = _DraggingCursor;
                }

                dcClipRectangle = Combine(dcClipRectangle, _DraggingObject.DraggingBoundingBox);
                _DraggingObject.MoveDrag(dcMouseLocation);
                dcClipRectangle = Combine(dcClipRectangle, _DraggingObject.DraggingBoundingBox);
            }
            else if (_DraggingDiagram)
            {
                _Diagram.MoveDrag(dcMouseLocation);
                if (_Diagram.Dragging)
                {
                    Cursor = _DraggingCursor;
                    _Invalidate();
                }
            }

            if (dcClipRectangle.HasValue)
            {
                _Invalidate(_Diagram.ProjectToInt(dcClipRectangle.Value));
            }
        }

        private void _Canvas_MouseLeave(object? sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void _Canvas_Paint(object? sender, PaintEventArgs e)
        {
            //DrawWaypoints(e.Graphics);
            //return;

            _Diagram.Draw(e.Graphics, e.ClipRectangle);

            if (_ExpandDiagramPanel != null && e.ClipRectangle.IntersectsWith(_ExpandDiagramPanel.Bounds))
            {
                _ExpandDiagramPanel.Update();
            }
        }

        private void _Invalidate(Rectangle r)
        {
            r = DiagramObject.AddMargins(r, 10);
            _Canvas.Invalidate(r);
        }

        private void _Invalidate()
        {
            _Canvas.Invalidate();
        }

        private Rectangle? Combine(Rectangle? r1, Rectangle? r2)
        {
            if (!r1.HasValue) return r2;
            if (!r2.HasValue) return r1;

            int minX = Math.Min(r1.Value.X, r2.Value.X);
            int minY = Math.Min(r1.Value.Y, r2.Value.Y);
            int maxX = Math.Max(r1.Value.X + r1.Value.Width, r2.Value.X + r2.Value.Width);
            int maxY = Math.Max(r1.Value.Y + r1.Value.Height, r2.Value.Y + r2.Value.Height);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }


        //private void DrawWaypoints(Graphics g)
        //{
        //    Brush brush = new SolidBrush(Color.Black);
        //    Pen pen = new Pen(brush, 2);

        //    foreach (Point p in waypoints)
        //    {
        //        g.FillEllipse(brush, p.X - 5, p.Y - 5, 10, 10);
        //    }

        //    if (waypoints.Count < 4) return;
        //    List<Point> bezierPoints = new List<Point>();
        //    bezierPoints.Add(waypoints[0]);
        //    for (int i = 1; i + 2 < waypoints.Count; i += 3)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {
        //            bezierPoints.Add(waypoints[i + j]);
        //        }
        //    }
        //    g.DrawBeziers(pen, bezierPoints.ToArray());
        //}

        #endregion 

        #region TextI18n
        private void InitializeText()
        {
            this.tsbAddTables.Text = Properties.Text.add_tables;
            this.tsbExpandDiagram.Text = Properties.Text.expand_diagram;
            this.tsbZoomFull.Text = Properties.Text.zoom_full;
        }
        #endregion

        #region "CheckedToolStripButtonRenderer"
        private class CheckedToolStripButtonRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var btn = e.Item as ToolStripButton;
                if (btn != null && btn.CheckOnClick && btn.Checked)
                {
                    e.Graphics.FillRectangle(_CheckedToolStripButton, new Rectangle(Point.Empty, e.Item.Size));
                }

                base.OnRenderButtonBackground(e);
            }
        }
        #endregion
    }
}

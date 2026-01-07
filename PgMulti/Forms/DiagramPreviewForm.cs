using PgMulti.Diagrams;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace PgMulti.Forms
{
    public partial class DiagramPreviewForm : Form
    {
        private const int MouseResizeTolerance = 5;
        private const int MinRectangleSize = 50;
        private Diagram _Diagram;
        private Rectangle _RectangleSelection;
        private Rectangle _DCRectangleSelection;
        private Pen _SelectionPen = new Pen(Color.Red, 2);

        private Point? _DraggingStartPoint = null;
        private bool _DraggingSelectionRectangleLeft = false;
        private bool _DraggingSelectionRectangleTop = false;
        private bool _DraggingSelectionRectangleRight = false;
        private bool _DraggingSelectionRectangleBottom = false;
        private Rectangle? _DraggingRectangleSelection = null;

        public DiagramPreviewForm(string fileName)
        {
            InitializeComponent();
            InitializeText();

            _Diagram = Diagram.LoadFile(fileName);
            _Diagram.RecalculateLocations();

            int margin = Math.Max(cvCanvas.Width, cvCanvas.Height) / 10;

            _Diagram.ZoomFull(cvCanvas.Size, (int)(margin * 1.5), true);

            int w = cvCanvas.Width - 2 * margin;
            int h = cvCanvas.Height - 2 * margin;

            _RectangleSelection = new Rectangle((cvCanvas.Width - w) / 2, (cvCanvas.Height - h) / 2, w, h);
            _DCRectangleSelection = _Diagram.UnProject(_RectangleSelection);
        }

        public Diagram Diagram
        {
            get
            {
                return _Diagram;
            }
        }

        public Rectangle DCRectangleSelection
        {
            get
            {
                return _DCRectangleSelection;
            }
        }

        private void RefreshSize()
        {
            _Diagram.ZoomFull(cvCanvas.Size, Math.Max(cvCanvas.Width, cvCanvas.Height) / 10, true);

            _RectangleSelection = _Diagram.ProjectToInt(_DCRectangleSelection);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cvCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            _Diagram.Draw(e.Graphics, e.ClipRectangle, true, false);

            if (_DraggingRectangleSelection.HasValue)
            {
                e.Graphics.DrawRectangle(_SelectionPen, _Diagram.UnProject(_DraggingRectangleSelection.Value));
            }
            else
            {
                e.Graphics.DrawRectangle(_SelectionPen, _DCRectangleSelection);
            }
        }

        private void cvCanvas_Resize(object sender, EventArgs e)
        {
            RefreshSize();
            cvCanvas.Invalidate();
        }

        private void cvCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            switch (GetPositionInRectangle(_RectangleSelection, e.Location, MouseResizeTolerance))
            {
                case PositionInRectangle.LeftBorder:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleLeft = true;
                    break;
                case PositionInRectangle.TopBorder:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleTop = true;
                    break;
                case PositionInRectangle.RightBorder:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleRight = true;
                    break;
                case PositionInRectangle.BottomBorder:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleBottom = true;
                    break;
                case PositionInRectangle.LeftTopCorner:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleLeft = true;
                    _DraggingSelectionRectangleTop = true;
                    break;
                case PositionInRectangle.RightTopCorner:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleRight = true;
                    _DraggingSelectionRectangleTop = true;
                    break;
                case PositionInRectangle.LeftBottomCorner:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleLeft = true;
                    _DraggingSelectionRectangleBottom = true;
                    break;
                case PositionInRectangle.RightBottomCorner:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleRight = true;
                    _DraggingSelectionRectangleBottom = true;
                    break;
                case PositionInRectangle.Inside:
                    _DraggingStartPoint = e.Location;
                    _DraggingSelectionRectangleLeft = true;
                    _DraggingSelectionRectangleRight = true;
                    _DraggingSelectionRectangleTop = true;
                    _DraggingSelectionRectangleBottom = true;
                    break;
                case PositionInRectangle.Outside:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void cvCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (_DraggingStartPoint.HasValue)
            {
                if (_DraggingRectangleSelection.HasValue)
                {
                    _RectangleSelection = _DraggingRectangleSelection!.Value;
                    _DCRectangleSelection = _Diagram.UnProject(_RectangleSelection);
                }

                _DraggingStartPoint = null;
                _DraggingSelectionRectangleLeft = false;
                _DraggingSelectionRectangleTop = false;
                _DraggingSelectionRectangleRight = false;
                _DraggingSelectionRectangleBottom = false;
                _DraggingRectangleSelection = null;
            }
        }

        private void cvCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_DraggingStartPoint.HasValue)
            {
                int deltaX = e.Location.X - _DraggingStartPoint.Value.X;
                int deltaY = e.Location.Y - _DraggingStartPoint.Value.Y;

                Rectangle r = _RectangleSelection;

                if (_DraggingSelectionRectangleLeft)
                {
                    r.X += deltaX;
                    r.Width -= deltaX;
                    if (r.Width < MinRectangleSize && !_DraggingSelectionRectangleRight)
                    {
                        r.X = r.X + r.Width - MinRectangleSize;
                        r.Width = MinRectangleSize;
                    }
                }

                if (_DraggingSelectionRectangleRight)
                {
                    r.Width += deltaX;
                    if (!_DraggingSelectionRectangleLeft) r.Width = Math.Max(r.Width, MinRectangleSize);
                }

                if (_DraggingSelectionRectangleTop)
                {
                    r.Y += deltaY;
                    r.Height -= deltaY;
                    if (r.Height < MinRectangleSize && !_DraggingSelectionRectangleBottom)
                    {
                        r.Y = r.Y + r.Height - MinRectangleSize;
                        r.Height = MinRectangleSize;
                    }
                }

                if (_DraggingSelectionRectangleBottom)
                {
                    r.Height += deltaY;
                    if (!_DraggingSelectionRectangleTop) r.Height = Math.Max(r.Height, MinRectangleSize);
                }

                _DraggingRectangleSelection = r;
                Refresh();
            }
            else
            {
                switch (GetPositionInRectangle(_RectangleSelection, e.Location, MouseResizeTolerance))
                {
                    case PositionInRectangle.LeftBorder:
                        Cursor = Cursors.SizeWE;
                        break;
                    case PositionInRectangle.TopBorder:
                        Cursor = Cursors.SizeNS;
                        break;
                    case PositionInRectangle.RightBorder:
                        Cursor = Cursors.SizeWE;
                        break;
                    case PositionInRectangle.BottomBorder:
                        Cursor = Cursors.SizeNS;
                        break;
                    case PositionInRectangle.LeftTopCorner:
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case PositionInRectangle.RightTopCorner:
                        Cursor = Cursors.SizeNESW;
                        break;
                    case PositionInRectangle.LeftBottomCorner:
                        Cursor = Cursors.SizeNESW;
                        break;
                    case PositionInRectangle.RightBottomCorner:
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case PositionInRectangle.Inside:
                        Cursor = Cursors.SizeAll;
                        break;
                    case PositionInRectangle.Outside:
                        Cursor = Cursors.Arrow;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private PositionInRectangle GetPositionInRectangle(Rectangle r, Point p, int tolerance)
        {
            if (p.Y > r.Y - tolerance && p.Y < r.Y + tolerance && p.X > r.X + tolerance && p.X < r.X + r.Width - tolerance)
            {
                return PositionInRectangle.TopBorder;
            }
            else if (p.Y > r.Y + r.Height - tolerance && p.Y < r.Y + r.Height + tolerance && p.X > r.X + tolerance && p.X < r.X + r.Width - tolerance)
            {
                return PositionInRectangle.BottomBorder;
            }
            else if (p.X > r.X - tolerance && p.X < r.X + tolerance && p.Y > r.Y + tolerance && p.Y < r.Y + r.Height - tolerance)
            {
                return PositionInRectangle.LeftBorder;
            }
            else if (p.X > r.X + r.Width - tolerance && p.X < r.X + r.Width + tolerance && p.Y > r.Y + tolerance && p.Y < r.Y + r.Height - tolerance)
            {
                return PositionInRectangle.RightBorder;
            }
            else if (p.X > r.X - tolerance && p.X < r.X + tolerance && p.Y > r.Y - tolerance && p.Y < r.Y + tolerance)
            {
                return PositionInRectangle.LeftTopCorner;
            }
            else if (p.X > r.X + r.Width - tolerance && p.X < r.X + r.Width + tolerance && p.Y > r.Y - tolerance && p.Y < r.Y + tolerance)
            {
                return PositionInRectangle.RightTopCorner;
            }
            else if (p.X > r.X + r.Width - tolerance && p.X < r.X + r.Width + tolerance && p.Y > r.Y + r.Height - tolerance && p.Y < r.Y + r.Height + tolerance)
            {
                return PositionInRectangle.RightBottomCorner;
            }
            else if (p.X > r.X - tolerance && p.X < r.X + tolerance && p.Y > r.Y + r.Height - tolerance && p.Y < r.Y + r.Height + tolerance)
            {
                return PositionInRectangle.LeftBottomCorner;
            }
            else if (p.X > r.X + tolerance && p.X < r.X + r.Width - tolerance && p.Y > r.Y + tolerance && p.Y < r.Y + r.Height - tolerance)
            {
                return PositionInRectangle.Inside;
            }
            else
            {
                return PositionInRectangle.Outside;
            }
        }

        private void cvCanvas_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.Text = Properties.Text.select_area;
        }
        #endregion

        private enum PositionInRectangle
        {
            LeftBorder,
            TopBorder,
            RightBorder,
            BottomBorder,
            LeftTopCorner,
            RightTopCorner,
            LeftBottomCorner,
            RightBottomCorner,
            Inside,
            Outside
        }

        public class Canvas : Panel
        {
            public Canvas()
            {
                DoubleBuffered = true;
            }
        }
    }
}

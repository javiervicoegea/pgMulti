using PgMulti.AppData;
using PgMulti.Diagrams;
using PgMulti.Diagrams.Efdg;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace PgMulti.Forms
{
    public partial class RepositionTablesOptionsForm : Form
    {
        private DiagramForm _DiagramForm;
        private Diagram _Diagram;
        private Panel _Canvas;
        private Data _Data;
        private DB? _SuggestAddRelatedTablesSelectedDB;
        private Image _StartImage;
        private Image _StopImage;

        public RepositionTablesOptionsForm(Data d, DB? preselectedDB, DiagramForm diagramForm, Diagram diagram, Panel canvas)
        {
            InitializeComponent();
            InitializeText();

            DoubleBuffered = true;

            _Data = d;
            _SuggestAddRelatedTablesSelectedDB = preselectedDB;
            _DiagramForm = diagramForm;
            _Diagram = diagram;
            _Canvas = canvas;

            Left = diagramForm.Width - Width;
            Top = diagramForm.Height - Height;

            using (Image i = Properties.Resources.ejecutar) _StartImage = ResizeImage(i, 24, 24);
            using (Image i = Properties.Resources.detener) _StopImage = ResizeImage(i, 24, 24);

            btnToggleRepulsion.Image = _StartImage;
        }

        public bool IsRepelling
        {
            get
            {
                return tmrRelocator.Enabled;
            }
        }

        public void StartRepulsion()
        {
            _Diagram.DiagramRelocator = new DiagramRelocator(_Diagram, _Canvas);
            _Diagram.DiagramRelocator!.UpdateRepulsion(tbRepulsion.Value);

            tmrRelocator.Enabled = true;
        }

        public void StopRepulsion()
        {
            _Diagram.DiagramRelocator = null;
            tmrRelocator.Enabled = false;
        }

        private void ExpandDiagramOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopRepulsion();
        }

        private void tbRepulsion_Scroll(object sender, EventArgs e)
        {
            if (_Diagram.DiagramRelocator != null)
            {
                _Diagram.DiagramRelocator!.UpdateRepulsion(tbRepulsion.Value);
            }
        }

        private void tmrRelocator_Tick(object sender, EventArgs e)
        {
            //if (_DraggingObject != null) return;
            _Diagram.DiagramRelocator!.Draw();

            if (chkAutoZoom.Checked)
            {
                _DiagramForm.ZoomFull(true);
            }
        }

        private void btnToggleRepulsion_Click(object sender, EventArgs e)
        {
            if (IsRepelling)
            {
                StopRepulsion();
                btnToggleRepulsion.Text = Properties.Text.start_reposition;
                btnToggleRepulsion.Image = _StartImage;
            }
            else
            {
                StartRepulsion();
                btnToggleRepulsion.Text = Properties.Text.stop_reposition;
                btnToggleRepulsion.Image = _StopImage;
            }
        }

        public void ChooseTableSuggestions()
        {
            SelectDBForm f = new SelectDBForm(_Data, _SuggestAddRelatedTablesSelectedDB);
            f.ShowDialog(_DiagramForm);

            if (f.DialogResult != DialogResult.OK) return;
            _SuggestAddRelatedTablesSelectedDB = f.SelectedDB;
            EnableTableSuggestions();
        }

        public void EnableTableSuggestions()
        {
            _DiagramForm.SuggestAddRelatedTablesDB = _SuggestAddRelatedTablesSelectedDB;
            _DiagramForm.UpdateSuggestedTables(_Diagram.Tables.FirstOrDefault(dti => dti.Selected));
        }

        public void DisableTableSuggestions()
        {
            _DiagramForm.SuggestAddRelatedTablesDB = null;
            _DiagramForm.UpdateSuggestedTables(_Diagram.Tables.FirstOrDefault(dti => dti.Selected));
        }

        #region TextI18n
        private void InitializeText()
        {
            Text = Properties.Text.automatically_reposition_tables;
            lblExplanation.Text = Properties.Text.reposition_explanation;
            chkAutoZoom.Text = Properties.Text.auto_zoom;
            lblRepulsion.Text = Properties.Text.repulsion_force;
            btnToggleRepulsion.Text = Properties.Text.start_reposition;
        }
        #endregion

        private Image ResizeImage(Image src, int w, int h)
        {
            Bitmap dst = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(src, 0, 0, dst.Width, dst.Height);
            }

            return dst;
        }

        ~RepositionTablesOptionsForm()
        {
            _StartImage.Dispose();
            _StopImage.Dispose();
        }
    }
}

using PgMulti.AppData;
using PgMulti.Diagrams;
using PgMulti.Diagrams.Efdg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PgMulti.Forms
{
    public partial class ExpandDiagramPanel : UserControl
    {
        private DiagramForm _DiagramForm;
        private Diagram _Diagram;
        private Panel _Canvas;
        private Data _Data;
        private DB? _SuggestAddRelatedTablesSelectedDB;

        public ExpandDiagramPanel(Data d, DB? preselectedDB, DiagramForm diagramForm, Diagram diagram, Panel canvas)
        {
            InitializeComponent();
            InitializeText();

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;

            _Data = d;
            _SuggestAddRelatedTablesSelectedDB = preselectedDB;
            _DiagramForm = diagramForm;
            _Diagram = diagram;
            _Canvas = canvas;

            Left = diagramForm.Width - Width;
            Top = diagramForm.Height - Height;

            if (_SuggestAddRelatedTablesSelectedDB != null)
            {
                txtSuggestAddRelatedTablesSelectedDB.Text = _SuggestAddRelatedTablesSelectedDB.Alias;
                chkSuggestAddRelatedTables.Checked = true;
                chkSuggestAddRelatedTables_CheckedChanged(chkSuggestAddRelatedTables, new EventArgs());
            }
        }

        public void EnableAll()
        {
            chkEnableRepulsion.Checked = true;
            chkSuggestAddRelatedTables.Checked = true;
            txtSuggestAddRelatedTablesSelectedDB.Text = _SuggestAddRelatedTablesSelectedDB!.Alias;
            _DiagramForm.SuggestAddRelatedTablesDB = _SuggestAddRelatedTablesSelectedDB;
            _DiagramForm.UpdateSuggestedTables(_Diagram.Tables.FirstOrDefault(dti => dti.Selected));
        }

        public void Close()
        {
            if (chkEnableRepulsion.Checked)
            {
                chkEnableRepulsion.Checked = false;
            }
            if (chkSuggestAddRelatedTables.Checked)
            {
                chkSuggestAddRelatedTables.Checked = false;
            }
        }

        private void chkEnableRepulsion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableRepulsion.Checked)
            {
                _Diagram.DiagramRelocator = new DiagramRelocator(_Diagram, _Canvas);
                _Diagram.DiagramRelocator!.UpdateRepulsion(tbRepulsion.Value);

                tmrRelocator.Enabled = true;
            }
            else
            {
                _Diagram.DiagramRelocator = null;
                tmrRelocator.Enabled = false;
            }

            lblRepulsion.Enabled = chkEnableRepulsion.Checked;
            tbRepulsion.Enabled = chkEnableRepulsion.Checked;
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

        private void chkSuggestAddRelatedTables_CheckedChanged(object sender, EventArgs e)
        {
            txtSuggestAddRelatedTablesSelectedDB.Enabled = chkSuggestAddRelatedTables.Checked;
            btnSuggestAddRelatedTablesSelectDB.Enabled = chkSuggestAddRelatedTables.Checked;

            if (chkSuggestAddRelatedTables.Checked)
            {
                _DiagramForm.SuggestAddRelatedTablesDB = _SuggestAddRelatedTablesSelectedDB;
            }
            else
            {
                _DiagramForm.SuggestAddRelatedTablesDB = null;
            }

            _DiagramForm.UpdateSuggestedTables(_Diagram.Tables.FirstOrDefault(dti => dti.Selected));
        }

        private void btnSuggestAddRelatedTablesSelectDB_Click(object sender, EventArgs e)
        {
            SelectDBForm f = new SelectDBForm(_Data, _SuggestAddRelatedTablesSelectedDB);
            f.ShowDialog(_DiagramForm);

            if (f.DialogResult == DialogResult.OK)
            {
                _SuggestAddRelatedTablesSelectedDB = f.SelectedDB;
                txtSuggestAddRelatedTablesSelectedDB.Text = _SuggestAddRelatedTablesSelectedDB!.Alias;
            }
            else
            {
                _SuggestAddRelatedTablesSelectedDB = null;
                txtSuggestAddRelatedTablesSelectedDB.Text = "";
            }

            _DiagramForm.SuggestAddRelatedTablesDB = _SuggestAddRelatedTablesSelectedDB;
            _DiagramForm.UpdateSuggestedTables(_Diagram.Tables.FirstOrDefault(dti => dti.Selected));
        }

        #region TextI18n
        private void InitializeText()
        {
            this.lblExplanation.Text = Properties.Text.expand_diagram_explanation;
            this.chkAutoZoom.Text = Properties.Text.auto_zoom;
            this.chkEnableRepulsion.Text = Properties.Text.enable_repulsion;
            this.chkSuggestAddRelatedTables.Text = Properties.Text.suggest_related_tables;
            this.btnSuggestAddRelatedTablesSelectDB.Text = Properties.Text.select_db;
            this.lblRepulsion.Text = Properties.Text.repulsion_force;
        }
        #endregion
    }
}

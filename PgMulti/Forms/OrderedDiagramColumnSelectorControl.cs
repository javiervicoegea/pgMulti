using PgMulti.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PgMulti.Forms
{
    public partial class OrderedDiagramColumnSelectorControl : Control
    {
        public event EventHandler? SelectedColumnsChanged;

        private DiagramTable? _DiagramTable = null;
        private bool _OnlyKeyColumns = false;

        public OrderedDiagramColumnSelectorControl()
        {
            InitializeComponent();
            InitializeText();
        }

        public DiagramTable? DiagramTable
        {
            get
            {
                return _DiagramTable;
            }
            set
            {
                _DiagramTable = value;
                UpdateColumns();
            }
        }

        public bool OnlyKeyColumns
        {
            get
            {
                return _OnlyKeyColumns;
            }
            set
            {
                _OnlyKeyColumns = value;
                UpdateColumns();
            }
        }

        public IReadOnlyList<DiagramColumn> SelectedColumns
        {
            get
            {
                return lbCurrentColumns.Items.Cast<DiagramColumn>().ToList();
            }
            set
            {
                UpdateColumns();

                if (value == null) return;
                if (_DiagramTable == null) return;

                foreach (DiagramColumn f in value)
                {
                    if (lbAvailableColumns.Items.Cast<DiagramColumn>().Any(i => i == f))
                    {
                        lbAvailableColumns.Items.Remove(f);
                        lbCurrentColumns.Items.Add(f);
                    }
                }

                UpdateButtons();
            }
        }

        private void UpdateColumns()
        {
            lbCurrentColumns.Items.Clear();
            lbAvailableColumns.Items.Clear();

            if (_DiagramTable != null)
            {
                foreach (DiagramColumn dc in _DiagramTable.Columns.Where(i => !OnlyKeyColumns || i.PrimaryKey))
                {
                    lbAvailableColumns.Items.Add(dc);
                }
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            tsbLeft.Enabled = lbAvailableColumns.SelectedIndex != -1;
            tsbRight.Enabled = lbCurrentColumns.SelectedIndex != -1;
            tsbUp.Enabled = lbCurrentColumns.SelectedIndex > 0;
            tsbDown.Enabled = lbCurrentColumns.SelectedIndex != -1 && lbCurrentColumns.SelectedIndex < lbCurrentColumns.Items.Count - 1;
        }

        private void tsbRight_Click(object sender, EventArgs e)
        {
            RemoveSelectedCurrentColumns();
        }

        private void tsbLeft_Click(object sender, EventArgs e)
        {
            AddSelectedAvailableColumns();
        }

        private void tsbDown_Click(object sender, EventArgs e)
        {
            if (lbCurrentColumns.SelectedIndex == -1 || lbCurrentColumns.SelectedIndex >= lbCurrentColumns.Items.Count - 1) return;
            DiagramColumn dc = (DiagramColumn)lbCurrentColumns.SelectedItem!;
            int index = lbCurrentColumns.SelectedIndex;
            lbCurrentColumns.Items.Remove(dc);
            lbCurrentColumns.Items.Insert(index + 1, dc);
            if (SelectedColumnsChanged != null) SelectedColumnsChanged(this, EventArgs.Empty);
        }

        private void tsbUp_Click(object sender, EventArgs e)
        {
            if (lbCurrentColumns.SelectedIndex <= 0) return;
            DiagramColumn dc = (DiagramColumn)lbCurrentColumns.SelectedItem!;
            int index = lbCurrentColumns.SelectedIndex;
            lbCurrentColumns.Items.Remove(dc);
            lbCurrentColumns.Items.Insert(index - 1, dc);
            if (SelectedColumnsChanged != null) SelectedColumnsChanged(this, EventArgs.Empty);
        }

        private void lbAvailableFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void lbCurrentFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void lbCurrentColumns_DoubleClick(object sender, EventArgs e)
        {
            RemoveSelectedCurrentColumns();
        }

        private void lbAvailableColumns_DoubleClick(object sender, EventArgs e)
        {
            AddSelectedAvailableColumns();
        }

        private void RemoveSelectedCurrentColumns()
        {
            if (lbCurrentColumns.SelectedIndex == -1) return;
            DiagramColumn dc = (DiagramColumn)lbCurrentColumns.SelectedItem!;
            lbCurrentColumns.Items.Remove(dc);
            lbAvailableColumns.Items.Add(dc);
            if (SelectedColumnsChanged != null) SelectedColumnsChanged(this, EventArgs.Empty);
        }

        private void AddSelectedAvailableColumns()
        {
            if (lbAvailableColumns.SelectedIndex == -1) return;
            DiagramColumn dc = (DiagramColumn)lbAvailableColumns.SelectedItem!;
            lbCurrentColumns.Items.Add(dc);
            lbAvailableColumns.Items.Remove(dc);
            if (SelectedColumnsChanged != null) SelectedColumnsChanged(this, EventArgs.Empty);
        }

        #region TextI18n
        private void InitializeText()
        {
            this.lblAvailableColumns.Text = Properties.Text.available;
            this.lblCurrentColumns.Text = Properties.Text.selected;
        }
        #endregion

    }
}

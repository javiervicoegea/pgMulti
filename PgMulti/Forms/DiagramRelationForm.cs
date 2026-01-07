using PgMulti.Diagrams;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PgMulti.Forms
{
    public partial class DiagramRelationForm : Form
    {
        private Diagram _Diagram;
        private DiagramRelation? _Relation;
        private bool _IsNewRelation = false;
        private DiagramTable? _OriginalParentTable = null;
        private DiagramTable? _OriginalChildTable = null;

        public DiagramRelationForm(DiagramTable parentTable, DiagramTable childTable) : this(parentTable.Diagram)
        {
            if (parentTable == null) throw new ArgumentException();
            if (childTable == null) throw new ArgumentException();

            txtRelationId.Text = "fk_" + parentTable.TableName + "_" + childTable.TableName;

            cbRelationParentTable.SelectedItem = parentTable;
            odcsRelationParentColumns.DiagramTable = parentTable;
            odcsRelationParentColumns.SelectedColumns = parentTable.Columns.Where(i => i.PrimaryKey).ToList();

            cbRelationChildTable.SelectedItem = childTable;
            odcsRelationChildColumns.DiagramTable = childTable;
        }

        public DiagramRelationForm(DiagramRelation dtr) : this(dtr.Diagram)
        {
            if (dtr == null) throw new ArgumentException();

            _Relation = dtr;
            _OriginalParentTable = dtr.ParentTable;
            _OriginalChildTable = dtr.ChildTable;
            Text = string.Format(Properties.Text.edit_relation_x, dtr.Id);

            txtRelationId.Text = dtr.Id;
            txtRelationId.ReadOnly = true;

            cbRelationParentTable.SelectedItem = dtr.ParentTable;
            odcsRelationParentColumns.DiagramTable = dtr.ParentTable;
            odcsRelationParentColumns.SelectedColumns = dtr.ParentTableColumns;
            cbRelationParentType.SelectedItem = cbRelationParentType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value == dtr.ParentRelationType);

            cbRelationChildTable.SelectedItem = dtr.ChildTable;
            odcsRelationChildColumns.DiagramTable = dtr.ChildTable;
            odcsRelationChildColumns.SelectedColumns = dtr.ChildTableColumns;
            cbRelationChildType.SelectedItem = cbRelationChildType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value == dtr.ChildRelationType);

            cbRelationOnUpdate.SelectedItem = cbRelationOnUpdate.Items.Cast<PropagationOptionListItem>().First(i => i.Value == dtr.OnUpdate);
            cbRelationOnDelete.SelectedItem = cbRelationOnDelete.Items.Cast<PropagationOptionListItem>().First(i => i.Value == dtr.OnDelete);
        }

        public DiagramRelationForm(Diagram d)
        {
            InitializeComponent();
            InitializeText();

            Text = Properties.Text.new_relation;

            Font sectionFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            Font normalFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            gbParent.Font = sectionFont;
            gbChild.Font = sectionFont;

            foreach (Control c in gbParent.Controls) { c.Font = normalFont; }
            foreach (Control c in gbChild.Controls) { c.Font = normalFont; }

            _Diagram = d;
            _Relation = null;

            foreach (DiagramTable dti in Diagram.Tables)
            {
                cbRelationParentTable.Items.Add(dti);
                cbRelationChildTable.Items.Add(dti);
            }

            cbRelationParentType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.One));
            cbRelationParentType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.OneAndOnlyOne));
            cbRelationParentType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.ZeroOrOne));

            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.One));
            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.OneAndOnlyOne));
            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.ZeroOrOne));
            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.Many));
            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.OneOrMany));
            cbRelationChildType.Items.Add(new RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions.ZeroOrMany));

            cbRelationOnUpdate.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.Cascade));
            cbRelationOnUpdate.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.SetNull));
            cbRelationOnUpdate.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.SetDefault));
            cbRelationOnUpdate.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.Restrict));
            cbRelationOnUpdate.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.NoAction));

            cbRelationOnDelete.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.Cascade));
            cbRelationOnDelete.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.SetNull));
            cbRelationOnDelete.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.SetDefault));
            cbRelationOnDelete.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.Restrict));
            cbRelationOnDelete.Items.Add(new PropagationOptionListItem(DiagramRelation.PropagationOptions.NoAction));

            txtRelationId.Text = "";

            cbRelationParentTable.SelectedItem = null;
            odcsRelationParentColumns.DiagramTable = null;
            cbRelationParentType.SelectedItem = cbRelationParentType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value == DiagramRelation.RelationTypeOptions.One);

            cbRelationChildTable.SelectedItem = null;
            odcsRelationChildColumns.DiagramTable = null;
            cbRelationChildType.SelectedItem = cbRelationChildType.Items.Cast<RelationTypeOptionListItem>().First(i => i.Value == DiagramRelation.RelationTypeOptions.Many);

            cbRelationOnUpdate.SelectedItem = cbRelationOnUpdate.Items.Cast<PropagationOptionListItem>().First(i => i.Value == DiagramRelation.PropagationOptions.Restrict);
            cbRelationOnDelete.SelectedItem = cbRelationOnDelete.Items.Cast<PropagationOptionListItem>().First(i => i.Value == DiagramRelation.PropagationOptions.Restrict);
        }

        public DiagramRelation? Relation
        {
            get
            {
                return _Relation;
            }
        }

        public DiagramTable? OriginalParentTable
        {
            get
            {
                return _OriginalParentTable;
            }
        }

        public DiagramTable? OriginalChildTable
        {
            get
            {
                return _OriginalChildTable;
            }
        }

        public bool IsNewRelation
        {
            get
            {
                return _IsNewRelation;
            }
        }

        public Diagram Diagram
        {
            get
            {
                return _Diagram;
            }
        }


        private void cbRelationParentTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            DiagramTable? dt = (DiagramTable?)cbRelationParentTable.SelectedItem;
            odcsRelationParentColumns.DiagramTable = dt;
            if (dt != null) odcsRelationParentColumns.SelectedColumns = dt.Columns.Where(i => i.PrimaryKey).ToList();
        }

        private void cbRelationChildTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            DiagramTable? dt = (DiagramTable?)cbRelationChildTable.SelectedItem;
            odcsRelationChildColumns.DiagramTable = dt;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtRelationId.Text))
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.relation_id), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRelationId.Focus();
                return false;
            }

            if (cbRelationParentTable.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.parent_table), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationParentTable.Focus();
                return false;
            }

            if (odcsRelationParentColumns.SelectedColumns.Count == 0)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.columns), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                odcsRelationParentColumns.Focus();
                return false;
            }

            if (cbRelationParentType.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.type), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationParentType.Focus();
                return false;
            }

            if (cbRelationChildTable.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.child_table), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationParentTable.Focus();
                return false;
            }

            if (odcsRelationChildColumns.SelectedColumns.Count == 0)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.columns), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                odcsRelationChildColumns.Focus();
                return false;
            }

            if (odcsRelationParentColumns.SelectedColumns.Count != odcsRelationChildColumns.SelectedColumns.Count || odcsRelationParentColumns.SelectedColumns.Where((c, i) => odcsRelationChildColumns.SelectedColumns[i].TypeName != c.TypeName || odcsRelationChildColumns.SelectedColumns[i].TypeParams != c.TypeParams).Any())
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_incompatible_foraign_key, Properties.Text.columns), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                odcsRelationChildColumns.Focus();
                return false;
            }

            if (cbRelationChildType.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, Properties.Text.type), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationChildType.Focus();
                return false;
            }

            if (cbRelationOnUpdate.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, "on update"), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationOnUpdate.Focus();
                return false;
            }

            if (cbRelationOnDelete.SelectedItem == null)
            {
                MessageBox.Show(this, string.Format(Properties.Text.warning_empty_field, "on delete"), Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbRelationOnDelete.Focus();
                return false;
            }

            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if (Relation == null)
            {
                _Relation = Diagram.Relations.FirstOrDefault(i => i.Id == txtRelationId.Text);

                if (_Relation != null)
                {
                    if (MessageBox.Show(this, string.Format(Properties.Text.warning_existing_relation, txtRelationId.Text), Properties.Text.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes) return;

                    _OriginalParentTable = _Relation.ParentTable;
                    _OriginalChildTable = _Relation.ChildTable;
                }
            }

            if (Relation == null)
            {
                _Relation = new DiagramRelation(
                    Diagram, txtRelationId.Text,
                    (DiagramTable)cbRelationParentTable.SelectedItem!, (DiagramTable)cbRelationChildTable.SelectedItem!,
                    odcsRelationParentColumns.SelectedColumns.ToList(), odcsRelationChildColumns.SelectedColumns.ToList(),
                    ((PropagationOptionListItem)cbRelationOnDelete.SelectedItem).Value, ((PropagationOptionListItem)cbRelationOnUpdate.SelectedItem).Value);

                _IsNewRelation = true;
            }
            else
            {
                Relation.ParentTable = (DiagramTable)cbRelationParentTable.SelectedItem!;
                Relation.ChildTable = (DiagramTable)cbRelationChildTable.SelectedItem!;
                Relation.ParentTableColumns = odcsRelationParentColumns.SelectedColumns.ToList();
                Relation.ChildTableColumns = odcsRelationChildColumns.SelectedColumns.ToList();
                Relation.OnDelete = ((PropagationOptionListItem)cbRelationOnDelete.SelectedItem).Value;
                Relation.OnUpdate = ((PropagationOptionListItem)cbRelationOnUpdate.SelectedItem).Value;
            }

            Relation!.ParentRelationType = ((RelationTypeOptionListItem)cbRelationParentType.SelectedItem).Value;
            Relation.ChildRelationType = ((RelationTypeOptionListItem)cbRelationChildType.SelectedItem).Value;

            Diagram.Refresh();

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.lblRelationId.Text = Properties.Text.relation_id + ":";
            this.gbParent.Text = Properties.Text.parent_side_relation;
            this.lblRelationParentTable.Text = Properties.Text.table + ":";
            this.lblRelationParentColumns.Text = Properties.Text.columns + ":";
            this.lblRelationParentType.Text = Properties.Text.type + ":";
            this.gbChild.Text = Properties.Text.child_side_relation;
            this.lblRelationChildTable.Text = Properties.Text.table + ":";
            this.lblRelationChildColumns.Text = "- " + Properties.Text.columns + ":";
            this.lblRelationChildType.Text = "- " + Properties.Text.type + ":";
        }
        #endregion

        public class RelationTypeOptionListItem
        {
            public readonly DiagramRelation.RelationTypeOptions Value;
            public RelationTypeOptionListItem(DiagramRelation.RelationTypeOptions v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return DiagramRelation.RelationTypeOptionsToString(Value);
            }
        }

        public class PropagationOptionListItem
        {
            public readonly DiagramRelation.PropagationOptions Value;
            public PropagationOptionListItem(DiagramRelation.PropagationOptions v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return DiagramRelation.PropagationOptionsToString(Value);
            }
        }
    }
}

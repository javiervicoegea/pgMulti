using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using PgMulti.AppData;
using PgMulti.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskBand;

namespace PgMulti.Forms
{
    public partial class ExportImportConnectionsForm : Form
    {
        private Data _Data;
        private Node _NRoot;
        private ExportConnectionsFile _ExportConnectionsFile;
        private bool _Import;
        private TreeModel _TreeModel;
        private Font groupFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        private Font dbFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        public ExportImportConnectionsForm(Data d, ExportConnectionsFile ecf, bool import)
        {
            _Data = d;
            _ExportConnectionsFile = ecf;
            _Import = import;

            InitializeComponent();
            InitializeText();

            _TreeModel = new TreeModel();
            tvaConnections.Model = _TreeModel;
            _NRoot = new Node(Properties.Text.all_databases);
            _NRoot.Image = Properties.Resources.tva_grupo;
            _TreeModel.Nodes.Add(_NRoot);
            ntb.DrawText += ntb_DrawText;
            ncb.CheckStateChanged += ncb_CheckStateChanged;
            ncb.IsVisibleValueNeeded += ncb_IsVisibleValueNeeded;

            if (_Import)
            {
                Text = Properties.Text.import_databases;
                pnlPassword1.Visible = _ExportConnectionsFile.EncryptedPasswords;
            }
            else
            {
                Text = Properties.Text.export_databases;
                chkExportPasswords.Visible = true;
            }
            DialogResult = DialogResult.Cancel;
        }

        private void ExportImportConnectionsForm_Load(object sender, EventArgs e)
        { 
            tvaConnections.Root.Children[0].Expanded += root_Expanded;

            tvaConnections.BeginUpdate();
            Queue<Tuple<ExportGroupInfo, Node>> queue = new Queue<Tuple<ExportGroupInfo, Node>>();
            queue.Enqueue(new Tuple<ExportGroupInfo, Node>(_ExportConnectionsFile.RootGroup, _NRoot));

            while (queue.Count > 0)
            {
                Tuple<ExportGroupInfo, Node> t = queue.Dequeue();
                ExportGroupInfo gi = t.Item1;
                Node nGroup = t.Item2;

                foreach(ExportItemInfo childItemInfo in gi.Items)
                {
                    if(childItemInfo is ExportDBInfo)
                    {
                        ExportDBInfo childDBInfo = (ExportDBInfo)childItemInfo;
                        Node nBD = new Node(childDBInfo.Alias);
                        nBD.Tag = childDBInfo;
                        nBD.Image = Properties.Resources.tva_db;

                        nGroup.Nodes.Add(nBD);
                    }
                    else if (childItemInfo is ExportGroupInfo)
                    {
                        ExportGroupInfo childGroupInfo= (ExportGroupInfo)childItemInfo;
                        Node nChildGroup = new Node(childGroupInfo.Name);

                        nChildGroup.Tag = childGroupInfo;
                        nChildGroup.Image = Properties.Resources.tva_grupo;

                        nGroup.Nodes.Add(nChildGroup);
                        
                        queue.Enqueue(new Tuple<ExportGroupInfo, Node>(childGroupInfo, nChildGroup));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            tvaConnections.Root.Children[0].Expand(true);
            _NRoot.IsChecked = true;
            UpdateNodeCheck(_NRoot);
            tvaConnections.EndUpdate();
        }

        private Node FindNode(Node parentNode, ExportItemInfo eii)
        {
            foreach (Node gni in parentNode.Nodes)
            {
                if (gni.Tag == eii)
                {
                    return gni;
                }
            }

            throw new Exception();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ExportConnectionsFile ecf = new ExportConnectionsFile();
            Queue<Tuple<ExportGroupInfo, Node, ExportGroupInfo>> queue = new Queue<Tuple<ExportGroupInfo, Node, ExportGroupInfo>>();
            queue.Enqueue(new Tuple<ExportGroupInfo, Node, ExportGroupInfo>(_ExportConnectionsFile.RootGroup, _NRoot, ecf.RootGroup));

            while (queue.Count > 0)
            {
                Tuple<ExportGroupInfo, Node, ExportGroupInfo> t = queue.Dequeue();
                ExportGroupInfo gi = t.Item1;
                Node n = t.Item2;
                ExportGroupInfo gi2 = t.Item3;

                foreach (ExportItemInfo childItemInfo in gi.Items)
                {
                    Node nChild = FindNode(n, childItemInfo);
                    if (nChild.CheckState == CheckState.Unchecked) continue;

                    ExportItemInfo childItemInfo2 = childItemInfo.ShallowClone();
                    gi2.Items.Add(childItemInfo2);

                    if (childItemInfo is ExportGroupInfo)
                    {
                        ExportGroupInfo childGroupInfo = (ExportGroupInfo)childItemInfo;
                        ExportGroupInfo childGroupInfo2 = (ExportGroupInfo)childItemInfo2;
                        queue.Enqueue(new Tuple<ExportGroupInfo, Node, ExportGroupInfo>(childGroupInfo, nChild, childGroupInfo2));
                    }
                }
            }

            ecf.EncryptedPasswords = _ExportConnectionsFile.EncryptedPasswords;

            if (_Import)
            {
                if (ecf.EncryptedPasswords)
                {
                    try
                    {
                        ecf.DecryptPasswords(txtPassword1.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(Properties.Text.warning_invalid_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (ecf.HasDbAliasConflicts(_Data!) && MessageBox.Show(Properties.Text.warning_dbalias_import_conflict, Properties.Text.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }

                ecf.Import(_Data!);
            }
            else
            {
                if (chkEncryptPasswords.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtPassword1.Text))
                    {
                        MessageBox.Show(Properties.Text.warning_empty_password, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtPassword1.Focus();
                        return;
                    }

                    if (txtPassword1.Text != txtPassword2.Text)
                    {
                        MessageBox.Show(Properties.Text.warning_password_mismatch, Properties.Text.warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtPassword1.Focus();
                        return;
                    }

                    ecf.EncryptPasswords(txtPassword1.Text);
                }

                sfdExportConfig.FileName = "pgMultiConnections.pgcx";
                if (sfdExportConfig.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    ecf.SaveFile(sfdExportConfig.FileName, chkExportPasswords.Checked);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Text.error_saving_file + $":\r\n{sfdExportConfig.FileName}\r\n\r\n{ex.Message}", Properties.Text.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(string.Format(Properties.Text.export_ok_msg, sfdExportConfig.FileName), Properties.Text.export_ok, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkExportPasswords_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExportPasswords.Checked)
            {
                chkEncryptPasswords.Checked = true;
                chkEncryptPasswords.Visible = true;
            }
            else
            {
                chkEncryptPasswords.Checked = false;
                chkEncryptPasswords.Visible = false;
            }

            chkEncryptPasswords_CheckedChanged(sender, e);
        }

        private void chkEncryptPasswords_CheckedChanged(object sender, EventArgs e)
        {
            pnlPassword1.Visible = chkEncryptPasswords.Checked;
            pnlPassword2.Visible = chkEncryptPasswords.Checked;
        }

        private void ncb_IsVisibleValueNeeded(object? sender, NodeControlValueEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n == _NRoot || n.Tag is ExportGroupInfo || n.Tag is ExportDBInfo)
            {
                e.Value = true;
            }
            else
            {
                e.Value = false;
            }
        }

        private void ntb_DrawText(object? sender, DrawEventArgs e)
        {
            Node n = (Node)e.Node.Tag;
            if (n == _NRoot || n.Tag is ExportGroupInfo)
            {
                e.Font = groupFont;
            }
            else
            {
                e.Font = dbFont;
            }
        }

        private void root_Expanded(object? sender, TreeViewAdvEventArgs e)
        {
            foreach (TreeNodeAdv tna in tvaConnections.Root.Children[0].Children)
            {
                tna.Expand(true);
            }
        }

        private void UpdateNodeCheck(Node nEditedNode)
        {
            if (nEditedNode == null) throw new ArgumentException();

            bool v = nEditedNode.IsChecked;

            Stack<Node> pila = new Stack<Node>();

            foreach (Node tni in nEditedNode.Nodes) pila.Push(tni);

            while (pila.Count > 0)
            {
                Node tni = pila.Pop();

                tni.IsChecked = v;

                foreach (Node tnj in tni.Nodes) pila.Push(tnj);
            }

            Node tn = nEditedNode.Parent;
            while (tn != null)
            {
                CheckState? cs = null;
                foreach (Node tni in tn.Nodes)
                {
                    if (!cs.HasValue)
                    {
                        cs = tni.CheckState;
                    }
                    else if (cs.Value != tni.CheckState)
                    {
                        cs = CheckState.Indeterminate;
                        break;
                    }
                }

                tn.CheckState = cs!.Value;

                tn = tn.Parent;
            }

            List<ExportDBInfo> dbs = new List<ExportDBInfo>();
            UpdateNodeCounter(_NRoot, dbs);
        }

        private void ncb_CheckStateChanged(object? sender, TreePathEventArgs e)
        {
            Node n = _TreeModel.FindNode(e.Path)!;
            UpdateNodeCheck(n);
        }

        private int UpdateNodeCounter(Node tn, List<ExportDBInfo> dbs)
        {
            if (tn == null || !(tn.Tag is ExportGroupInfo || tn == _NRoot)) throw new ArgumentException();

            int n = 0;
            foreach (Node tni in tn.Nodes)
            {
                if (tni.Tag is ExportDBInfo)
                {
                    if (tni.IsChecked)
                    {
                        n++;
                        dbs.Add((ExportDBInfo)tni.Tag);
                    }
                }
                else
                {
                    n += UpdateNodeCounter(tni, dbs);
                }
            }

            string nombre = (tn.Tag is ExportGroupInfo ? ((ExportGroupInfo)tn.Tag).Name! : Properties.Text.all_databases);
            if (n > 0)
            {
                tn.Text = nombre + " (" + n + ")";
            }
            else
            {
                tn.Text = nombre;
            }

            return n;
        }

        #region TextI18n
        private void InitializeText()
        {
            this.btnOk.Text = Properties.Text.btn_ok;
            this.btnCancel.Text = Properties.Text.btn_cancel;
            this.chkExportPasswords.Text = Properties.Text.chk_export_passwords;
            this.chkEncryptPasswords.Text = Properties.Text.chk_encrypt_passwords;
            this.lblPassword1.Text = Properties.Text.export_password + ":";
            this.lblPassword2.Text = Properties.Text.repeat_password + ":";
            this.sfdExportConfig.Filter = Properties.Text.pgcx_file_filter;
            this.sfdExportConfig.Title = Properties.Text.select_save_file;
        }
        #endregion
    }
}

using Aga.Controls.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleApp
{
    public partial class Test : Form
    {
        private TreeModel _TreeModel;
        private Node _TnRaiz;

        public Test()
        {
            InitializeComponent();

            _TreeModel = new TreeModel();
            tvaServidores.Model = _TreeModel;
            _TnRaiz = new Node("Todos los servidores");
            _TnRaiz.Tag = 1;
            _TreeModel.Nodes.Add(_TnRaiz);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Node n = new Node("Hijo");
            n.Tag = 2;
            _TnRaiz.Nodes.Add(n);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stack<Node> nodes = new Stack<Node>();
            nodes.Push(_TnRaiz);
            List<Node> seleccionados = new List<Node>();
            while (nodes.Count>0)
            {
                Node n = nodes.Pop();
                if (n.IsChecked) seleccionados.Add(n);

                foreach(Node ni in n.Nodes)
                {
                    nodes.Push(ni);
                }
            }

            foreach(Node n in seleccionados)
            {
                Console.WriteLine(n.Tag);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (TreeNodeAdv tna in tvaServidores.SelectedNodes)
            {
                Console.WriteLine(((Node)tna.Tag).Tag);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (TreeNodeAdv tna in tvaServidores.SelectedNodes)
            {
                Node n = (Node)tna.Tag;
                n.CheckState = CheckState.Indeterminate;
            }
        }
    }
}

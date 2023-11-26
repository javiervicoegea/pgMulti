using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover.Graphs
{
    public class Arrow<T>
    {
        public Node<T> Source { get; set; }
        public Node<T> Target { get; set; }

        public Arrow(Node<T> source, Node<T> target)
        {
            Source = source;
            Target = target;
        }

        public override string ToString()
        {
            return Source.ToString() + " -> " + Target.ToString();
        }
    }
}

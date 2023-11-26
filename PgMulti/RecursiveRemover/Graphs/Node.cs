using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover.Graphs
{
    public class Node<T>
    {
        public List<Arrow<T>> IncomingArrows { get; }
        public List<Arrow<T>> OutgoingArrows { get; }
        public T Value { get; }

        public Node(T value)
        {
            IncomingArrows = new List<Arrow<T>>();
            OutgoingArrows = new List<Arrow<T>>();
            Value = value;
        }

        public List<Node<T>> IncomingNodes
        {
            get
            {
                return IncomingArrows.Select(a => a.Source).ToList();
            }
        }

        public List<Node<T>> OutgoingNodes
        {
            get
            {
                return OutgoingArrows.Select(a => a.Target).ToList();
            }
        }

        public override string? ToString()
        {
            return Value?.ToString();
        }
    }
}

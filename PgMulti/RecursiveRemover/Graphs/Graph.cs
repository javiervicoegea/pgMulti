using FastColoredTextBoxNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.RecursiveRemover.Graphs
{
    public class Graph<T>
    {
        public List<Node<T>> Nodes { get; }
        public List<Arrow<T>> Arrows { get; }

        public Graph(List<Node<T>> nodes, List<Arrow<T>> arrows)
        {
            Nodes = nodes;
            Arrows = arrows;
        }

        public TraverseGraphEnumerable CreateTraverseGraphEnumerable(bool directOrder)
        {
            return new TraverseGraphEnumerable(this, directOrder);
        }

        public class TraverseGraphEnumerable : IEnumerable<Node<T>>
        {
            private Graph<T> _Graph;
            private bool _DirectOrder;

            internal TraverseGraphEnumerable(Graph<T> graph, bool directOrder)
            {
                _Graph = graph;
                _DirectOrder = directOrder;
            }

            public IEnumerator<Node<T>> GetEnumerator()
            {
                HashSet<Node<T>> visited = new HashSet<Node<T>>();
                Queue<Node<T>> queue = new Queue<Node<T>>();

                // Enqueue source/sink nodes
                foreach (Node<T> n in _Graph.Nodes)
                {
                    if (_DirectOrder && n.IncomingArrows.Count == 0 || !_DirectOrder && n.OutgoingArrows.Count == 0)
                    {
                        queue.Enqueue(n);
                    }
                }


                Node<T>? firstNonVisitableNodeQueuedAfterLastReturn = null;

                while (queue.Count > 0)
                {
                    Node<T> node = queue.Dequeue();
                    if (node == firstNonVisitableNodeQueuedAfterLastReturn) throw new NotSupportedException("This graph is not a directed acyclic graph because it must contain a loop");

                    List<Node<T>> prevNodes = _DirectOrder ? node.IncomingNodes : node.OutgoingNodes;

                    if (prevNodes.Any(incNode => !visited.Contains(node)))
                    {
                        // Not visitable
                        queue.Enqueue(node);
                        if (firstNonVisitableNodeQueuedAfterLastReturn == null) firstNonVisitableNodeQueuedAfterLastReturn = node;
                    }
                    else
                    {
                        // Visitable
                        yield return node;
                        firstNonVisitableNodeQueuedAfterLastReturn = null;

                        List<Node<T>> nextNodes = _DirectOrder ? node.OutgoingNodes : node.IncomingNodes;

                        foreach (Node<T> nextNode in nextNodes)
                        {
                            queue.Enqueue(nextNode);
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

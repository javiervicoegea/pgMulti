using EpForceDirectedGraph.cs;
using PgMulti.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams.Efdg
{
    public class DiagramRelocator
    {
        private Diagram _Diagram;
        private Panel _Canvas;
        private ForceDirected2D _ForceDirected2D;
        private Renderer _Renderer;
        Dictionary<DiagramTable, Node> _Nodes;
        Dictionary<Tuple<DiagramTable, DiagramTable>, Edge> _Edges;
        private Graph _Graph;

        public DiagramRelocator(Diagram diagram, Panel canvas)
        {
            _Diagram = diagram;
            _Canvas = canvas;

            _Graph = new Graph();

            _Nodes = new Dictionary<DiagramTable, Node>();
            _Edges = new Dictionary<Tuple<DiagramTable, DiagramTable>, Edge>();

            _ForceDirected2D = new ForceDirected2D(
                _Graph, // instance of Graph
                81.76f, // stiffness of the spring
                2000000.0f, // node repulsion rate 
                0.5f    // damping rate  
            );

            _Renderer = new Renderer(_Diagram, _Canvas, _ForceDirected2D);

            Add(_Diagram.Tables);
            if (_Diagram.SuggestedRelatedTables != null) Add(_Diagram.SuggestedRelatedTables);
        }

        public void Add(List<DiagramTable> tables)
        {
            foreach (DiagramTable dt in tables)
            {
                Node n = _Graph.CreateNode(new DiagramTableNodeData(dt));

                EpForceDirectedGraph.cs.Point p = _ForceDirected2D.GetPoint(n);
                p.position = new FDGVector2(dt.Center.X, dt.Center.Y);
                p.Width = dt.BoundingBox.Width + 100;
                p.Height = dt.BoundingBox.Height + 100;

                _Nodes[dt] = n;
            }

            foreach (DiagramTable dt in tables)
            {
                Node n = _Nodes[dt];

                foreach (DiagramTableRelation dr in dt.Relations)
                {
                    Tuple<DiagramTable, DiagramTable> t = new Tuple<DiagramTable, DiagramTable>(dr.ParentTable, dr.ChildTable);
                    if (_Edges.ContainsKey(t)) continue;
                    if (!_Nodes.ContainsKey(dr.ParentTable)) continue;
                    if (!_Nodes.ContainsKey(dr.ChildTable)) continue;

                    Node node1 = _Nodes[dr.ParentTable];
                    Node node2 = _Nodes[dr.ChildTable];
                    Edge e = _Graph.CreateEdge(node1, node2, new DiagramTableRelationEdgeData(dr));

                    _Edges[t] = e;
                }
            }
        }

        public void Remove(List<DiagramTable> tables)
        {
            foreach (DiagramTable dt in tables)
            {
                Node n = _Nodes[dt];
                _Graph.RemoveNode(n);
                _Nodes.Remove(dt);

                foreach (DiagramTableRelation dr in dt.Relations)
                {
                    _Edges.Remove(new Tuple<DiagramTable, DiagramTable>(dr.ParentTable, dr.ChildTable));
                }
            }
        }

        public void Draw()
        {
            _Renderer.Draw(0.05f);
        }

        public void UpdateDiagramTableLocation(DiagramTable dt)
        {
            _ForceDirected2D.GetPoint(_Nodes[dt]).position = new FDGVector2(dt.Center.X, dt.Center.Y);
        }

        public void UpdateRepulsion(float v)
        {
            _ForceDirected2D.Repulsion = v;
        }
    }
}

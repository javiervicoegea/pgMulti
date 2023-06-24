using EpForceDirectedGraph.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams.Efdg
{
    public class Renderer : AbstractRenderer
    {
        private Diagram _Diagram;
        private Panel _Canvas;

        public Renderer(Diagram diagram, Panel canvas, IForceDirected iForceDirected) : base(iForceDirected)
        {
            _Diagram = diagram;
            _Canvas = canvas;
        }

        public override void Clear()
        {

        }

        protected override void drawEdge(Edge iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
        {
            
        }

        protected override void drawNode(Node iNode, AbstractVector iPosition)
        {
            DiagramTable dt=((DiagramTableNodeData)iNode.Data).DiagramTable;

            dt.MoveTo(new System.Drawing.Point((int)iPosition.x, (int)iPosition.y));
        }

        public override void completeDraw()
        {
            _Diagram.RecalculateLocations();
            _Diagram.UpdateBoundingBox();
            _Canvas.Invalidate();
        }
    }
}

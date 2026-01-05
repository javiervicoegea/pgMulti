using EpForceDirectedGraph.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams.Efdg
{
    public class DiagramTableRelationEdgeData : EdgeData
    {
        private DiagramRelation _DiagramTableRelation;

        public DiagramTableRelationEdgeData(DiagramRelation dr) : base()
        {
            _DiagramTableRelation = dr;
            label = dr.Id;
        }
        public DiagramRelation DiagramTableRelation
        {
            get { return _DiagramTableRelation; }
        }
    }
}

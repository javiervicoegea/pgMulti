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
        private DiagramTableRelation _DiagramTableRelation;

        public DiagramTableRelationEdgeData(DiagramTableRelation dr) : base()
        {
            _DiagramTableRelation = dr;
            label = dr.Id;
        }
        public DiagramTableRelation DiagramTableRelation
        {
            get { return _DiagramTableRelation; }
        }
    }
}

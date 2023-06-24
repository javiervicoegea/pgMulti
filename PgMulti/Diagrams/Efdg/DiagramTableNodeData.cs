using EpForceDirectedGraph.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.Diagrams.Efdg
{
    public class DiagramTableNodeData : NodeData
    {
        private DiagramTable _DiagramTable;

        public DiagramTableNodeData(DiagramTable dt) : base()
        {
            _DiagramTable = dt;
            label = dt.SchemaName + "." + dt.TableName;
        }
        public DiagramTable DiagramTable
        {
            get { return _DiagramTable; }
        }
    }
}

using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Graph
{
    internal interface ILogger
    {
        public List<Tuple<GraphNode, GraphEdge, string>> GetVisisted();
    }
}

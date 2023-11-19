using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DFSLogger
    {
        private List<Tuple<GraphNode, GraphEdge>> _visited;
        public DFSLogger()
        {
            _visited = new List<Tuple<GraphNode, GraphEdge>>();
        }

        public void AddMarkedElement(GraphNode node, GraphEdge edge)
        {
            _visited.Add(Tuple.Create(node, edge));
        }
    }
}

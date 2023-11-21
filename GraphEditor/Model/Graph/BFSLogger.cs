using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Graph
{
    public class BFSLogger : ILogger
    {
        public List<Tuple<GraphNode, GraphEdge, string>> Visited { get; private set; }

        public List<Tuple<GraphNode, GraphEdge, string>> GetVisisted() { 
            return Visited;
        }
        public BFSLogger()
        {
            Visited = new List<Tuple<GraphNode, GraphEdge, string>>();
        }
 
        public void AddLog(GraphNode node, GraphEdge edge, string comment)
        {
            Visited.Add(Tuple.Create(node, edge, comment));
        }
    }
}


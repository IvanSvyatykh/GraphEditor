using Model.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    public class PrimaLogger : ILogger
    {
        public List<Tuple<GraphNode, GraphEdge, string>> Visited { get; private set; }

        public PrimaLogger()
        {
            Visited = new List<Tuple<GraphNode, GraphEdge, string>>();
        }
        public List<Tuple<GraphNode, GraphEdge, string>> GetVisisted()
        {
            throw new NotImplementedException();
        }
        
        public void AddLog(GraphNode node, GraphEdge edge, string comment)
        {
            Visited.Add(Tuple.Create(node, edge, comment));
        }
    }
}

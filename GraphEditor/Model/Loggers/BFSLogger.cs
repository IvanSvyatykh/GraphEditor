using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    public class BFSLogger : ILogger
    {
        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> Visited { get; private set; }

        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> GetVisisted()
        {
            return Visited;
        }
        public BFSLogger()
        {
            Visited = new List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>>();
        }

        public void AddLog(NonOrientedGraphNode node, NonOrientedGraphEdge edge, string comment)
        {
            Visited.Add(Tuple.Create(node, edge, comment));
        }
    }
}


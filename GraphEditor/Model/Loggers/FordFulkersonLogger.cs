using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Loggers
{
    public class FordFulkersonLogger
    {
        // 0 - ничего не выделяем
        // 1 - выделить вершину цветом навсегда
        public List<Tuple<GraphNode, GraphEdge, string, byte>> Visited { get; private set; }

        public FordFulkersonLogger()
        {
            Visited = new List<Tuple<GraphNode, GraphEdge, string, byte>>();
        }

        public void AddLog(GraphNode node, GraphEdge edge, string comment, byte flag)
        {
            Visited.Add(Tuple.Create(node, edge, comment, flag));
        }

    }
}

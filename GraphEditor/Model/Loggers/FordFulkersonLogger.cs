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
        // 1 - выделить вершину цветом до встречи флага 2
        // 2 - выделить путь цветом
        public List<Tuple<GraphNode, GraphEdge, string, byte, List<GraphNode>>> Visited { get; private set; }

        public FordFulkersonLogger()
        {
            Visited = new List<Tuple<GraphNode, GraphEdge, string, byte, List<GraphNode>>>();
        }

        public void AddLog(GraphNode node, GraphEdge edge, string comment, byte flag, List<GraphNode> way = null)
        {
            Visited.Add(Tuple.Create(node, edge, comment, flag, way));
        }

    }
}

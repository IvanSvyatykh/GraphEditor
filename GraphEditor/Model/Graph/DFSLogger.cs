using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DFSLogger
    {
        public List<Tuple<GraphEdge, GraphNode, string>> Visited { get; private set; }
        public DFSLogger()
        {
            Visited = new List<Tuple<GraphEdge, GraphNode, string>>();
        }

        public void AddMarkedElement(GraphNode node, GraphEdge edge)
        {
            string log;
            if (edge == null)
            {
                log = $"Вершина {node.Name} является началом, просто посящаем ее и выделяем ее.";
            }
            else
            {
                log = $"Переходи из вершины {edge.FirstNode.Name} в {edge.SecondNode.Name}, отмечаем вершину {edge.SecondNode.Name}, как посященную.";
            }
            Visited.Add(Tuple.Create(edge, node, log));
        }

        public void AddCommentToLastLog(string str)
        {
            Visited[Visited.Count - 1] = Tuple.Create(Visited[Visited.Count - 1].Item1,
                Visited[Visited.Count - 1].Item2, Visited[Visited.Count - 1].Item3 + str);

        }
    }
}

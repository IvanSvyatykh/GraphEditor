using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DFSLogger
    {
        private List<Tuple<GraphEdge, GraphNode, string>> _visited;
        public DFSLogger()
        {
            _visited = new List<Tuple<GraphEdge, GraphNode, string>>();
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
            _visited.Add(Tuple.Create(edge, node, log));
        }

        public void AddCommentToLastLog(string str)
        {
            _visited[_visited.Count - 1] = Tuple.Create(_visited[_visited.Count - 1].Item1, 
                _visited[_visited.Count - 1].Item2, _visited[_visited.Count - 1].Item3 + str);

        }
    }
}

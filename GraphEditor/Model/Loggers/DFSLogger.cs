using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    public class DFSLogger : ILogger
    {
        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> Visited { get; private set; }
        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> GetVisisted()
        {
            return Visited;
        }
        public DFSLogger()
        {
            Visited = new List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>>();
        }

        public void AddMarkedElement(NonOrientedGraphNode node, NonOrientedGraphEdge edge)
        {
            string log;
            if (edge == null)
            {
                log = $"Вершина {node.Name} является началом, просто посещаем ее и выделяем ее.";
            }
            else
            {
                log = $"Переходим из вершины {edge.FirstNode.Name} в {edge.SecondNode.Name}, отмечаем вершину {edge.SecondNode.Name}, как посещенную.";
            }
            Visited.Add(Tuple.Create(node, edge, log));
        }

        public void AddComeback(NonOrientedGraphNode node, NonOrientedGraphEdge edge, string comment)
        {
            Visited.Add(Tuple.Create(node, edge, comment));
        }

        public void AddCommentToLastLog(string str)
        {
            Visited[Visited.Count - 1] = Tuple.Create(Visited[Visited.Count - 1].Item1,
                Visited[Visited.Count - 1].Item2, Visited[Visited.Count - 1].Item3 + str);

        }
    }
}

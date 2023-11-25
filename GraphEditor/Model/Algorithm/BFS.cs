using GraphEditor.Model.Loggers;
using Model.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class BFS
    {
        private NonOrientedGraph _graph;
        private Dictionary<string, List<string>> _matrix;
        private Dictionary<string, bool> _visited;
        private BFSLogger _logger;
        public BFS(Dictionary<string, List<string>> matrix)
        {
            _matrix = matrix;
            _graph = new NonOrientedGraph();
            Helper.TranslateToNonOrientedGraph(_matrix, _graph);
            //TranslateToGraph();
        }

        public BFSLogger StartAlgorithm(string startNodeName)
        {
            _logger = new BFSLogger();
            _visited = new Dictionary<string, bool>();
            _graph.Names.
                ToList().
                ForEach(name => { _visited.Add(name, false); });


            Traverse(startNodeName);
            return _logger;

        }
        private void Traverse(string name)
        {
            Queue<string> queue = new Queue<string>();
            _visited[name] = true;
            queue.Enqueue(name);

            while (queue.Count > 0)
            {
                NonOrientedGraphNode currentNode = _graph.GetNodeByName(queue.Dequeue());
                List<NonOrientedGraphNode> neighbors = currentNode.LinkedNodes;


                if (neighbors.Count >= 1)
                {
                    _logger.AddLog(_graph.GetNodeByName(currentNode.Name), null, $"Находимся в вершине {_graph.GetNodeByName(currentNode.Name).Name}.");

                    foreach (NonOrientedGraphNode neighbor in neighbors)
                    {
                        if (!_visited[neighbor.Name])
                        {
                            _visited[neighbor.Name] = true;
                            NonOrientedGraphEdge graphEdge = currentNode.GetEdgeBetween(currentNode, neighbor);
                            _logger.AddLog(neighbor, graphEdge,
                                $"Обошли вершину {neighbor.Name}. Выделим её и ребро между вершинами {currentNode.Name} и {neighbor.Name}.");
                            queue.Enqueue(neighbor.Name);
                        }
                    }

                    _logger.AddLog(null, null, $"Из вершины {currentNode.Name}, обошли всех соседей");
                }
            }
        }
    }
}

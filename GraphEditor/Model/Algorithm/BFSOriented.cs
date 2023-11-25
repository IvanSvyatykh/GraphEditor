using GraphEditor.Model.Loggers;
using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Algorithm
{
    public class BFSOriented
    {
        private OrientedGraph _graph;
        private Dictionary<string, List<Tuple<int, string>>> _matrix;
        private Dictionary<string, bool> _visited;
        private BFSOrientedLogger _logger;
        public BFSOriented(Dictionary<string, List<Tuple<int, string>>> matrix)
        {
            _matrix = matrix;
            _graph = new OrientedGraph(_matrix);
        }

        public BFSOrientedLogger StartAlgorithm(string startNodeName)
        {
            _logger = new BFSOrientedLogger();
            _logger = new BFSOrientedLogger();
            _visited = new Dictionary<string, bool>();
            _graph.NodesNames.
                ToList().
                ForEach(name => { _visited.Add(name, false); });

            _logger.AddLog(null, null, $"Начинаем обход графа, с вершины {startNodeName}.");
            Traverse(_graph, startNodeName);
            return _logger;

        }

        private void Traverse(OrientedGraph tempGrapg, string startNode)
        {
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            tempGrapg.NodesNames.ForEach(n => visited.Add(n, false));

            Queue<string> queue = new Queue<string>();
            _visited[startNode] = true;

            queue.Enqueue(startNode);

            while (queue.Count != 0)
            {
                string currentVertex = queue.Dequeue();

                _logger.AddLog(currentVertex, null, $"Находимся в вершине {currentVertex}.");

                foreach (Tuple<int, string> s in tempGrapg.Matrix[currentVertex])
                {
                    if (!visited[s.Item2])
                    {
                        visited[s.Item2] = true;
                        _logger.AddLog(s.Item2, _graph.GetEdgeBetwen(currentVertex, s.Item2),
                                $"Обошли вершину {s.Item2}. Выделим её и ребро между вершинами {currentVertex} и {s.Item2}.");
                        queue.Enqueue(s.Item2);
                    }
                }
                _logger.AddLog(null, null, $"Из вершины {currentVertex}, обошли всех соседей");
            }

        }
    }

}

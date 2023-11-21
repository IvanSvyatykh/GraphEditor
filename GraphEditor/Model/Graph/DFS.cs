using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DFS
    {
        private Graph _graph;
        private Dictionary<string, List<string>> _matrix;
        private Dictionary<string, bool> _visited;
        private DFSLogger _logger;
        public DFS(Dictionary<string, List<string>> matrix)
        {
            _matrix = matrix;
            _graph = new Graph();
            TranslateToGraph();
        }

        public DFSLogger StartAlgorithm(string startNodeName)
        {
            _logger = new DFSLogger();
            _visited = new Dictionary<string, bool>();
            _graph.Names.
                ToList().
                ForEach(name => { _visited.Add(name, false); });

            GraphNode basic = _graph.GetNodeByName(startNodeName);
            _logger.AddMarkedElement(basic, null);
            DFSRecursive(basic);

            return _logger;
        }

        private void DFSRecursive(GraphNode node)
        {
            _visited[node.Name] = true;

            foreach (GraphNode adjacentVertex in node.LinkedNodes)
            {
                if (!_visited[adjacentVertex.Name])
                {
                    GraphEdge graphEdge = node.GetEdgeBetween(node, adjacentVertex);
                    _logger.AddMarkedElement(adjacentVertex, graphEdge);
                    DFSRecursive(adjacentVertex);
                }
            }
            _logger.AddCommentToLastLog($" Больше из вершины {node.Name} идти не куда, возращаемся рекурсивно назад.");
        }

        private void TranslateToGraph()
        {

            foreach (var key in _matrix.Keys)
            {
                foreach (var node in _matrix[key])
                {
                    _graph.AddEdge(key, node);
                }
            }
        }
    }
}

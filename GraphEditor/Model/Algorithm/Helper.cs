using GraphEditor.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model.Graph
{
    public static class Helper
    {
        public static void TranslateToGraphWithWeights(Dictionary<string, List<Tuple<int, string>>> matrix, Graph graph)
        {
            foreach (var item in matrix.Keys)
            {
                if (matrix[item].Count == 0)
                {
                    matrix[item].Add(null);
                }
            }
            foreach (var key in matrix.Keys)
            {
                foreach (var node in matrix[key])
                {
                    if (node == null)
                    {
                        graph.AddEdge(key, null, edgeWeight: 0);
                    }
                    else
                    {
                        graph.AddEdge(key, node.Item2, edgeWeight: node.Item1);
                    }
                    
                }
            }
        }

        public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
   (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                    original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
        }

        public static void TranslateToNonOrientedGraph(Dictionary<string, List<string>> matrix, Graph graph)
        {
            foreach (var item in matrix.Keys)
            {
                if (matrix[item].Count == 0)
                {
                    matrix[item].Add(null);
                }
            }
            foreach (var key in matrix.Keys)
            {
                foreach (var node in matrix[key])
                {
                    graph.AddEdge(key, node);
                }
            }
        }
        public static void CheckGraph(Graph graph)
        {
            if (graph.Edges.Any(x => x.Weight == null) || graph.Edges.All(x => x.Weight == 0))
            {
                throw new ArgumentException($"Заданнный граф иммет стоимость всех ребер 0 или имеет ребро с весом null");
            }
        }

        public static Dictionary<string, bool> BFS(Graph graph, string start)
        {

            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            graph.Names.
                ToList().
                ForEach(name => { visited.Add(name, false); });

            Queue<string> queue = new Queue<string>();
            visited[start] = true;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                GraphNode currentNode = graph.GetNodeByName(queue.Dequeue());
                List<GraphNode> neighbors = currentNode.LinkedNodes;


                if (neighbors.Count >= 1)
                {

                    foreach (GraphNode neighbor in neighbors)
                    {
                        if (!visited[neighbor.Name])
                        {
                            visited[neighbor.Name] = true;
                            GraphEdge graphEdge = currentNode.GetEdgeBetween(currentNode, neighbor);
                            queue.Enqueue(neighbor.Name);
                        }
                    }
                }
            }

            return visited;
        }

        public static Dictionary<string, List<Tuple<int, string>>> MatrixRecovery(Dictionary<string, List<Tuple<int, string>>> matrix)
        {

            foreach (var key in matrix.Keys)
            {
                foreach (var item in matrix[key])
                {
                    var temp = Tuple.Create(item.Item1, key);
                    if (!matrix[item.Item2].Contains(temp))
                    {
                        matrix[item.Item2].Add(temp);
                    }
                }
            }

            return matrix;
        }
    }
}

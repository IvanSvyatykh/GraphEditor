using Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Model.Graph
{
    public class FordFalkerson
    {
        private List<string> _steps = new List<string>();
        private FordGraph Ford;
        private int _nodesAmount;
        private Dictionary<string, List<Tuple<int, string>>> _matrix;
        private FordFulkersonLogger _logger;
        private Graph _graph;


        public FordFalkerson(Dictionary<string, List<Tuple<int, string>>> matrix)
        {
            _matrix = matrix;
            _graph = new Graph(true);

            Helper.TranslateToGraphWithWeights(_matrix, _graph);

            _logger = new FordFulkersonLogger();
            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));

            Ford = new FordGraph(matrix.Keys.ToList(), Helper.TranslatoToMatrix(matrix), true);

            _nodesAmount = matrix.Count;
        }

        public FordFulkersonLogger GetSteps(string startNode, string endNode)
        {
            _steps.Clear();

            var result = FordFulkerson(_matrix.Keys.ToList().IndexOf(startNode), _matrix.Keys.ToList().IndexOf(endNode));
            _logger.AddLog(null, null, $"Максимальный поток найден: {result.Item2}. ", 4, null);
            return _logger;
        }
        private (List<List<string>>, int) FordFulkerson(int startNode, int endNode)
        {
            List<List<string>> allPaths = new List<List<string>>();

            int[][] tempGraph = new int[_nodesAmount][];
            for (int i = 0; i < _nodesAmount; i++)
            {
                tempGraph[i] = new int[_nodesAmount];
                for (int v = 0; v < _nodesAmount; v++)
                {
                    tempGraph[i][v] = Ford.Matrix[i][v];
                }
            }

            int[] parents = new int[_nodesAmount];
            int maxFlow = 0;
            bool flag = false;

            while (WaySearche(tempGraph, _nodesAmount, startNode, endNode, parents))
            {
                HashSet<GraphNode> graphNodes = new HashSet<GraphNode>();



                int pathFlow = int.MaxValue;
                List<string> currentPath = new List<string>();

                for (int i = endNode; i != startNode; i = parents[i])
                {
                    int iVertexParent = parents[i];
                    currentPath.Add(Ford.NodesNames[iVertexParent]);
                    pathFlow = Math.Min(pathFlow, tempGraph[iVertexParent][i]);
                }
                currentPath.Add(Ford.NodesNames[endNode]);
                currentPath.Sort();

                currentPath.ForEach(x => graphNodes.Add(_graph.GetNodeByName(x)));

                _logger.AddLog(null, null, $"Путь найден.", 2, graphNodes.ToList());

                allPaths.Add(currentPath);

                _logger.AddLog(null, null, $"Минимальный поток в найденном пути: {pathFlow}", 0, null);

                for (int i = endNode; i != startNode; i = parents[i])
                {
                    int currentNodeParent = parents[i];

                    _logger.AddLog(null, _graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[i]), _graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent])), $"Во временном графе по меняем вес ребра:" +
                           $" {tempGraph[currentNodeParent][i]} - {pathFlow}. Уменьшаем на мин.поток текущий поток по найденному пути.", 3, null, $"{_graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[i]), _graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent])).Weight - tempGraph[currentNodeParent][i] + pathFlow}/{_graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[i]), _graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent])).Weight}");
                    tempGraph[currentNodeParent][i] -= pathFlow;
                    //_logger.AddLog(null, _graph.GetNodeByName(_matrix.Keys.ToList()[i]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[currentNodeParent]), _graph.GetNodeByName(_matrix.Keys.ToList()[i])), $"Во временном графе по меняем вес ребра:" +
                    //           $" {tempGraph[i][currentNodeParent]} + {pathFlow}. Увеличиваем на мин.поток текущий поток по найденному пути.", 0, null, $"{pathFlow}/{tempGraph[i][currentNodeParent]}");
                    tempGraph[i][currentNodeParent] += pathFlow;

                    
                }

                _logger.AddLog(null, null, $"Прибавляем к счетчику максимального потока наш минимальный поток: {maxFlow} += {pathFlow}", 0, null);
                maxFlow += pathFlow;
            }


            _logger.AddLog(null, null, $"Путь не найден.", 0, null);


            return (allPaths, maxFlow);
        }

        private bool WaySearche(int[][] matrix, int vertices, int startNode, int endNode, int[] parents)
        {
            _logger.AddLog(_graph.GetNodeByName(_matrix.Keys.ToList()[startNode]), null,
                $"Начинаем обход графа в ширину из узла {_graph.GetNodeByName(_matrix.Keys.ToList()[startNode]).Name} в {_graph.GetNodeByName(_matrix.Keys.ToList()[endNode]).Name}", 1);
            bool[] visited = new bool[vertices];
            Queue<int> queue = new Queue<int>();
            _logger.AddLog(null, null, $"Добавили в очередь вершину {_graph.GetNodeByName(_matrix.Keys.ToList()[startNode]).Name}, пометили ее как пройденную.", 0);
            queue.Enqueue(startNode);
            visited[startNode] = true;
            parents[startNode] = startNode;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < vertices; v++)
                {
                    if (!visited[v] && matrix[u][v] > 0)
                    {
                        _logger.AddLog(_graph.GetNodeByName(_matrix.Keys.ToList()[v]),
                            _graph.GetNodeByName(_matrix.Keys.ToList()[u]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[u]), _graph.GetNodeByName(_matrix.Keys.ToList()[v])),
               $"Обошли узел {_graph.GetNodeByName(_matrix.Keys.ToList()[v]).Name}", 1);


                        queue.Enqueue(v);
                        parents[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return visited[endNode];
        }
    }

    public class FordGraph
    {

        public FordGraph(List<string> nodesNames, int[][] matrix, bool isOriented)
        {
            _nodesAmount = nodesNames.Count;
            NodesNames = nodesNames;
            Matrix = matrix;
            _isOriented = isOriented;
            _edges = GetEdges(matrix, isOriented);
        }


        public List<string> NodesNames { get; private set; }
        public int[][] Matrix { get; private set; }
        private int _nodesAmount;
        private List<Edge> _edges;
        private readonly bool _isOriented;

        private List<Edge> GetEdges(int[][] matrix, bool orientedGraph)
        {
            List<Edge> edges = new List<Edge>();
            bool[] visited = new bool[_nodesAmount];

            for (int y = 0; y < matrix.Length; y++)
            {
                for (int x = 0; x < matrix[y].Length; x++)
                {
                    if (x == y || (!orientedGraph && visited[x])) continue;
                    if (matrix[y][x] <= 0) continue;
                    edges.Add(new Edge(y, x, matrix[y][x]));
                }

                visited[y] = true;
            }

            return edges;
        }
    }

    public class Edge
    {
        public Edge(int startNode, int endNode, int weight)
        {
            EndNode = endNode;
            StartNode = startNode;
            Weight = weight;
        }

        public int StartNode { get; set; }
        public int EndNode { get; set; }
        public int Weight { get; set; }
    }
}
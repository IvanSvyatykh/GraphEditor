using Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _graph = new Graph(true);
            Helper.TranslateToGraphWithWeights(_matrix, _graph);
            _logger = new FordFulkersonLogger();
            _matrix = matrix;
            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));
            Ford = new FordGraph(matrix.Keys.ToList(), Helper.TranslatoToMatrix(matrix), true);
            _nodesAmount = matrix.Count;

        }

        public (List<List<string>>, List<string>) GetSteps(string startNode, string endNode)
        {
            _steps.Clear();

            var result = FordFulkerson(_matrix.Keys.ToList().IndexOf(startNode), _matrix.Keys.ToList().IndexOf(endNode));
            _steps.Add($"Максимальный поток найден: {result.Item2}. ");
            return (result.Item1, _steps);
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
            //_steps.Add($"Копируем исходный граф во временную матрицу смежности. Она будет служить сетью для изменений. ");
            int[] parents = new int[_nodesAmount];
            int maxFlow = 0;

            while (WaySearche(tempGraph, _nodesAmount, startNode, endNode, parents))
            {
                _steps.Add($"Путь был найден, продолжаем алгоритм. ");

                int pathFlow = int.MaxValue;
                List<string> currentPath = new List<string>();

                for (int i = endNode; i != startNode; i = parents[i])
                {
                    int iVertexParent = parents[i];
                    currentPath.Add(Ford.NodesNames[iVertexParent]);
                    pathFlow = Math.Min(pathFlow, tempGraph[iVertexParent][i]);
                }

                allPaths.Add(currentPath);
                _steps.Add($"Минимальный поток в найденном пути: {pathFlow}");

                for (int i = endNode; i != startNode; i = parents[i])
                {
                    int currentNodeParent = parents[i];
                    _steps.Add($"Устанавливаем следующее: во временном графе по индексу {currentNodeParent},{i} к этому элементу и " +
                               $" {tempGraph[currentNodeParent][i]} - {pathFlow}. Уменьшаем на мин.поток текущий поток по пути " +
                               $"найденному. ");
                    tempGraph[currentNodeParent][i] -= pathFlow;
                    _steps.Add($"Устанавливаем следующее: во временном графе по индексу {i},{currentNodeParent} к этому элементу и " +
                               $" {tempGraph[i][currentNodeParent]} + {pathFlow}. Увеличиваем на мин.поток текущий поток по пути " +
                               $"найденному. ");
                    tempGraph[i][currentNodeParent] += pathFlow;
                }

                _steps.Add($"Прибавляем к счетчику максимального потока наш минимальный поток: {maxFlow} += {pathFlow}");
                maxFlow += pathFlow;
            }

            return (allPaths, maxFlow);
        }

        private bool WaySearche(int[][] matrix, int vertices, int startNode, int endNode, int[] parents)
        {
            _logger.AddLog(_graph.GetNodeByName(_matrix.Keys.ToList()[startNode]), null,
                $"Начинаем обход графа в ширину из узла {_graph.GetNodeByName(_matrix.Keys.ToList()[startNode])} в {_graph.GetNodeByName(_matrix.Keys.ToList()[endNode])}", 1);
            bool[] visited = new bool[vertices];
            Queue<int> queue = new Queue<int>();
            _logger.AddLog(null, null, $"Добавили в очередь вершину по номеру {_graph.GetNodeByName(_matrix.Keys.ToList()[startNode])}, пометили ее как пройденную.", 0);
            queue.Enqueue(startNode);
            visited[startNode] = true;
            parents[startNode] = -1;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < vertices; v++)
                {
                    if (!visited[v] && matrix[u][v] > 0)
                    {
                        _logger.AddLog(_graph.GetNodeByName(_matrix.Keys.ToList()[v]), 
                            _graph.GetNodeByName(_matrix.Keys.ToList()[u]).GetEdgeBetween(_graph.GetNodeByName(_matrix.Keys.ToList()[u]), _graph.GetNodeByName(_matrix.Keys.ToList()[v])),
               $"Обошли узел {_graph.GetNodeByName(_matrix.Keys.ToList()[v])}", 1);

                        //_steps.Add($"Добавили в очередь вершину по номеру {v}, пометили ее как пройденную. ");

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
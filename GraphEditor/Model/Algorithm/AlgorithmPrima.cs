﻿using GraphEditor.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class AlgorithmPrima
    {

        private Graph _graph;
        private Dictionary<string, List<Tuple<int, string>>> _matrix;
        private Dictionary<string, bool> _visited;
        private PrimaLogger _logger;
        public AlgorithmPrima(Dictionary<string, List<Tuple<int, string>>> matrix, bool isOriented = false)
        {
            _matrix = matrix;
            _graph = new Graph(isOriented);
            Helper.TranslateToGraphWithWeights(_matrix, _graph);
        }

        public PrimaLogger StartAlgorithm()
        {
            Random random = new Random();
            _logger = new PrimaLogger();
            _visited = new Dictionary<string, bool>();
            _graph.Names.
                ToList().
                ForEach(name => { _visited.Add(name, false); });

            Helper.CheckGraph(_graph);




            string startNodeName = _graph.Names.ToList()[random.Next(0, _graph.Names.Count - 1)];
            if (Helper.BFS(_graph, startNodeName).Any(x => !x.Value))
            {
                throw new Exception($"Не возможно обойти все узлы в графе, возможно отсутвует связь междунекоторыми узлами");
            }
            _visited[startNodeName] = true;
            _logger.AddLog(_graph.GetNodeByName(startNodeName), null, $"Выберем случайную начальную точку в графе с котрой начнем строить дерево.Случайная точка стала {startNodeName}.", 1);
            MinOstTree(startNodeName);

            _logger.AddLog(null, null, $"Вот так выглядит итоговое остовное дерево для данного графа", 0);
            return _logger;
        }


        private void MinOstTree(string randNode)
        {
            List<string> currentNodes = new List<string>
            {
                randNode
            };
            int? weigth = 0;
            GraphNode minDisNode = null;
            GraphEdge minDisEdge = null;

            while (_visited.Any(x => !x.Value))
            {
                foreach (var item in currentNodes)
                {
                    List<GraphEdge> graphEdges = _graph.GetNodeByName(item).Edges;

                    foreach (var edge in graphEdges)
                    {

                        if (!_visited[edge.GetOtherNode(_graph.GetNodeByName(item)).Name])
                        {

                            if (minDisNode == null)
                            {
                                weigth = edge.Weight;
                                minDisNode = edge.GetOtherNode(_graph.GetNodeByName(item));
                                minDisEdge = edge;

                                _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(item)), edge,
                                    $"Рассмотрим путь из вершины {item}, до {edge.GetOtherNode(_graph.GetNodeByName(item)).Name}, это первое ребро которое мы рассматриваем после прошлого добавления, сравнивать ее не с кем.",
                                    2);
                            }
                            else if (weigth > edge.Weight)
                            {
                                _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(item)), edge,
                                    $"Рассмотрим путь из вершины {item}, до {edge.GetOtherNode(_graph.GetNodeByName(item)).Name}, длина до него составляет {edge.Weight}, это меньше чем длина {weigth} до {minDisNode.Name}. Пока рассмотрим ее как возможную к добавлению.",
                                    2);
                                weigth = edge.Weight;
                                minDisNode = edge.GetOtherNode(_graph.GetNodeByName(item));
                                minDisEdge = edge;

                            }
                            else
                            {
                                _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(item)), edge,
                                    $"Рассмотрим путь из вершины {item}, до {edge.GetOtherNode(_graph.GetNodeByName(item)).Name}, длина до него составляет {edge.Weight}, это больше чем длина {weigth} до {minDisNode.Name}. Из-за этого не рассматриваем ее к добавлению.",
                                    2);
                            }
                        }
                    }
                }


                currentNodes.Add(minDisNode.Name);
                _logger.AddLog(minDisNode, minDisEdge, $"Минимальным оказался путь до узла {minDisNode.Name} из узла {minDisEdge.GetOtherNode(minDisNode).Name}", 1);
                _visited[minDisNode.Name] = true;


                minDisNode = null;
                weigth = 0;
                minDisEdge = null;
            }
        }
    }
}

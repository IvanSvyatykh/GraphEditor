using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Graph
{
    public class OrientedGraph
    {
        public List<string> NodesNames { get; private set; }
        public Dictionary<string, List<Tuple<int, string>>> Matrix { get; private set; }
        public int NodesAmount { get; private set; }
        public List<OrientedEdge> Edges { get; set; }


        public OrientedGraph(Dictionary<string, List<Tuple<int, string>>> Matrix)
        {
            NodesNames = new List<string>(Matrix.Keys.ToArray());
            NodesAmount = NodesNames.Count;
            this.Matrix = Matrix;
            Edges = GetEdges(Matrix);
        }

        public OrientedEdge GetEdgeBetwen(string first, string second)
        {
            foreach (var edge in Edges)
            {
                if (IsEqual(first, second, edge))
                {
                    return edge;
                }
            }

            throw new ArgumentException("Нет связи с такими узлами");
        }

        private List<OrientedEdge> GetEdges(Dictionary<string, List<Tuple<int, string>>> matrix)
        {
            List<OrientedEdge> edges = new List<OrientedEdge>();
            Dictionary<string, bool> visitedNodes = new Dictionary<string, bool>();

            NodesNames.ForEach(x => visitedNodes.Add(x, false));

            foreach (string key in matrix.Keys)
            {
                foreach (Tuple<int, string> node in matrix[key])
                {
                    if (!visitedNodes[node.Item2])
                    {
                        edges.Add(new OrientedEdge(key, node.Item2, node.Item1));
                    }
                }
            }

            return edges;
        }

        private bool IsEqual(string first, string second, OrientedEdge edge)
        {
            return Equals(first, edge.StartNode) && Equals(second, edge.EndNode);
        }
    }

    public class OrientedEdge
    {
        public OrientedEdge(string startNode, string endNode, int weight)
        {
            EndNode = endNode;
            StartNode = startNode;
            Weight = weight;
        }

        public string StartNode { get; set; }
        public string EndNode { get; set; }
        public int Weight { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class GraphNode
    {
        private int? _value;
        public string Name { get; private set; }
        public bool IsLinked { get; private set; }

        public List<GraphEdge> Edges { get; private set; }

        public List<GraphNode> LinkedNodes { get; private set; }

        public GraphEdge GetEdgeBetween(GraphNode first, GraphNode second)
        {
            foreach (var el in Edges)
            {
                if (IsBetween(first, second, el))
                {
                    return el;

                }

            }
            return null;
        }

        public GraphNode(string name, int? value = null, List<GraphEdge> grapgEdges = null)
        {
            _value = value;
            LinkedNodes = new List<GraphNode>();
            Name = name;
            if (grapgEdges == null)
            {
                Edges = new List<GraphEdge>();
                IsLinked = false;
            }
            else
            {
                Edges = new List<GraphEdge>(grapgEdges);
                IsLinked = true;
            }

        }

        public void AddEdge(GraphNode node, int? weigtOfEdge = null)
        {

            if (LinkedNodes.All(n => !Equals(n.Name, node.Name)))
            {
                GraphEdge edge = new GraphEdge(this, node, weigtOfEdge);
                LinkedNodes.Add(node);
                Edges.Add(edge);
                IsLinked = true;
            }
            else
            {
                throw new ArgumentException($"Вершина с название {node.Name}, уже существует");
            }
        }

        private bool IsBetween(GraphNode first, GraphNode second, GraphEdge el)
        {
            return (el.FirstNode == first && el.SecondNode == second) || (el.SecondNode == first && el.FirstNode == second);
        }
    }
}

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
        public string Name { get; private set; }
        public bool IsLinked { get; private set; }

        public List<GraphEdge> Edges { get; private set; }

        public List<GraphNode> LinkedNodes { get; private set; }

        private bool _isOriented;

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

        public GraphNode(string name, bool IsOriented)
        {
            _isOriented = IsOriented;
            LinkedNodes = new List<GraphNode>();
            Name = name;

            Edges = new List<GraphEdge>();
            IsLinked = false;
        }

        public void AddEdge(GraphNode node, int weigtOfEdge = 0)
        {

            if (LinkedNodes.All(n => !Equals(n.Name, node.Name)))
            {
                GraphEdge edge = new GraphEdge(this, node, _isOriented, weigtOfEdge);
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
            return (Equals(el.FirstNode.Name, first.Name) && Equals(el.SecondNode.Name, second.Name))
                || (Equals(el.FirstNode.Name, second.Name) && Equals(el.SecondNode.Name, first.Name));
        }
    }
}

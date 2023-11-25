using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class NonOrientedGraphNode
    {
        private int? _value;
        public string Name { get; private set; }
        public bool IsLinked { get; private set; }

        public List<NonOrientedGraphEdge> Edges { get; private set; }

        public List<NonOrientedGraphNode> LinkedNodes { get; private set; }

        public NonOrientedGraphEdge GetEdgeBetween(NonOrientedGraphNode first, NonOrientedGraphNode second)
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

        public NonOrientedGraphNode(string name, int? value = null, List<NonOrientedGraphEdge> grapgEdges = null)
        {
            _value = value;
            LinkedNodes = new List<NonOrientedGraphNode>();
            Name = name;
            if (grapgEdges == null)
            {
                Edges = new List<NonOrientedGraphEdge>();
                IsLinked = false;
            }
            else
            {
                Edges = new List<NonOrientedGraphEdge>(grapgEdges);
                IsLinked = true;
            }

        }

        public void AddEdge(NonOrientedGraphNode node, int weigtOfEdge = 0)
        {

            if (LinkedNodes.All(n => !Equals(n.Name, node.Name)))
            {
                NonOrientedGraphEdge edge = new NonOrientedGraphEdge(this, node, weigtOfEdge);
                LinkedNodes.Add(node);
                Edges.Add(edge);
                IsLinked = true;
            }
            else
            {
                throw new ArgumentException($"Вершина с название {node.Name}, уже существует");
            }
        }

        private bool IsBetween(NonOrientedGraphNode first, NonOrientedGraphNode second, NonOrientedGraphEdge el)
        {
            return (el.FirstNode == first && el.SecondNode == second) || (el.SecondNode == first && el.FirstNode == second);
        }
    }
}

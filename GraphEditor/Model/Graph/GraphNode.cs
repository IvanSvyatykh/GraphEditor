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

        public List<GrapgEdge> Edges { get; private set; }

        private List<GraphNode> _linkedNodes;

        public GraphNode(int? value, string name, List<GrapgEdge> grapgEdges = null)
        {
            _value = value;
            _linkedNodes = new List<GraphNode>();
            Name = name;
            if (grapgEdges == null)
            {
                Edges = new List<GrapgEdge>();
                IsLinked = false;
            }
            else
            {
                Edges = new List<GrapgEdge>(grapgEdges);
                IsLinked = true;
            }

        }

        public void AddEdge(GraphNode node, int? weigtOfEdge = null)
        {
            if (_linkedNodes.All(n => !Equals(n.Name, node.Name)))
            {
                GrapgEdge edge = new GrapgEdge(this, node, weigtOfEdge);
                _linkedNodes.Add(node);
                Edges.Add(edge);
            }
            else
            {
                throw new ArgumentException($"Вершина с название {node.Name}, уже существует");
            }
        }

        public void RemoveEdge(GrapgEdge edge)
        {
            if (!Edges.Remove(edge))
            {
                throw new InvalidOperationException($"Вершина уже удалена");
            }
        }

    }
}

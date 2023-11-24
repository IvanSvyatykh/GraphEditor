using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Model.Graph
{
    public class Graph
    {
        private List<GraphNode> _nodes;
        public List<GraphEdge> Edges { get; private set; }
        public HashSet<string> Names { get; private set; }

        public Graph()
        {
            _nodes = new List<GraphNode>();
            Names = new HashSet<string>();
            Edges = new List<GraphEdge>();
        }

        public GraphNode GetNodeByName(string name)
        {
            if (!Names.Contains(name))
            {
                throw new ArgumentException($"Граф не имеет вершины с именем {name}");
            }
            else
            {
                return _nodes.First(x => Equals(x.Name, name));
            }
        }

        public void AddEdge(string firstNodeName, string secondNodeName, int? firstNodeWeight = null, int? secondNodeWeight = null, int edgeWeight = 0)
        {
            if (Edges.All(x => !x.IsEqual(firstNodeName, secondNodeName)))
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, firstNodeWeight);
                GraphNode secondGraphNode = new GraphNode(secondNodeName, secondNodeWeight);
                if (_nodes.All(x => !Equals(x.Name, firstNodeName)))
                {
                    _nodes.Add(firstGraphNode);
                }
                else
                {
                    firstGraphNode = _nodes.First(x => Equals(x.Name, firstNodeName));

                }
                if (_nodes.All(x => !Equals(x.Name, secondNodeName)))
                {
                    _nodes.Add(secondGraphNode);
                }
                else
                {
                    secondGraphNode = _nodes.First(x => Equals(x.Name, secondNodeName));

                }

                Names.Add(firstNodeName);
                Names.Add(secondNodeName);
                firstGraphNode.AddEdge(secondGraphNode, edgeWeight);
                secondGraphNode.AddEdge(firstGraphNode, edgeWeight);

                Edges.Add(new GraphEdge(firstGraphNode, secondGraphNode, edgeWeight));
            }
        }

    }
}

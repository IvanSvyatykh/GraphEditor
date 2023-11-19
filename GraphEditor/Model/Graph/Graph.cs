using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Model.Graph
{
    public class Graph
    {
        private List<GraphNode> _nodes;
        private List<GraphEdge> _edges;
        public HashSet<string> Names { get; private set; }

        public Graph()
        {
            _nodes = new List<GraphNode>();
            Names = new HashSet<string>();
            _edges = new List<GraphEdge>();
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

        public void AddEdge(string firstNodeName, string secondNodeName, int? firstNodeWeight = null, int? secondNodeWeight = null, int? edgeWeight = null)
        {
            if (_edges.All(x => !x.IsEqual(firstNodeName, secondNodeName)))
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
                firstGraphNode.AddEdge(secondGraphNode);
                secondGraphNode.AddEdge(firstGraphNode);

                _edges.Add(new GraphEdge(firstGraphNode, secondGraphNode, edgeWeight));
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Model.Graph
{
    public class NonOrientedGraph
    {
        private List<NonOrientedGraphNode> _nodes;
        public List<NonOrientedGraphEdge> Edges { get; private set; }
        public HashSet<string> Names { get; private set; }

        public NonOrientedGraph()
        {
            _nodes = new List<NonOrientedGraphNode>();
            Names = new HashSet<string>();
            Edges = new List<NonOrientedGraphEdge>();
        }

        public NonOrientedGraphNode GetNodeByName(string name)
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
                NonOrientedGraphNode firstGraphNode = new NonOrientedGraphNode(firstNodeName, firstNodeWeight);
                NonOrientedGraphNode secondGraphNode = new NonOrientedGraphNode(secondNodeName, secondNodeWeight);
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

                Edges.Add(new NonOrientedGraphEdge(firstGraphNode, secondGraphNode, edgeWeight));
            }
        }

    }
}

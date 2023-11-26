using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Model.Graph
{
    public class Graph
    {
        private List<GraphNode> _nodes;
        private bool _isOrinted;
        public List<GraphEdge> Edges { get; private set; }
        public HashSet<string> Names { get; private set; }

        public Graph(bool IsOriented)
        {
            _isOrinted = IsOriented;
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

        public void AddEdge(string firstNodeName, string secondNodeName, int edgeWeight = 0)
        {
            if (_isOrinted)
            {
                AddEdgeForOriented(firstNodeName, secondNodeName, edgeWeight);
            }
            else
            {
                AddEdgeForNonOreinted(firstNodeName, secondNodeName, edgeWeight);
            }
        }


        private void AddEdgeForOriented(string firstNodeName, string secondNodeName, int edgeWeight = 0)
        {
            if (secondNodeName == null)
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, _isOrinted);
                if (_nodes.All(x => !Equals(x.Name, firstNodeName)))
                {
                    _nodes.Add(firstGraphNode);
                }
                else
                {
                    firstGraphNode = _nodes.First(x => Equals(x.Name, firstNodeName));

                }

                Names.Add(firstNodeName);

            }
            else if (Edges.All(x => !x.IsEqual(firstNodeName, secondNodeName)))
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, _isOrinted);
                GraphNode secondGraphNode = new GraphNode(secondNodeName, _isOrinted);
                if (_nodes.All(x => !Equals(x.Name, firstNodeName)))
                {
                    _nodes.Add(firstGraphNode);
                }
                else
                {
                    firstGraphNode = _nodes.First(x => Equals(x.Name, firstNodeName));

                }

                Names.Add(firstNodeName);
                Names.Add(secondNodeName);
                firstGraphNode.AddEdge(secondGraphNode, edgeWeight);

                Edges.Add(new GraphEdge(firstGraphNode, secondGraphNode, _isOrinted, edgeWeight));
            }
        }

        private void AddEdgeForNonOreinted(string firstNodeName, string secondNodeName, int edgeWeight = 0)
        {
            if (secondNodeName == null)
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, _isOrinted);
                if (_nodes.All(x => !Equals(x.Name, firstNodeName)))
                {
                    _nodes.Add(firstGraphNode);
                }
                else
                {
                    firstGraphNode = _nodes.First(x => Equals(x.Name, firstNodeName));

                }

                Names.Add(firstNodeName);

            }
            else if (Edges.All(x => !x.IsEqual(firstNodeName, secondNodeName)))
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, _isOrinted);
                GraphNode secondGraphNode = new GraphNode(secondNodeName, _isOrinted);
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

                Edges.Add(new GraphEdge(firstGraphNode, secondGraphNode, _isOrinted, edgeWeight));
            }
        }

    }
}

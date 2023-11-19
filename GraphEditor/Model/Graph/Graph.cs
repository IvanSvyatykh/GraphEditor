using System.Collections.Generic;
using System.Linq;

namespace Model.Graph
{
    public class Graph
    {
        private List<GraphNode> _nodes;
        private List<GraphEdge> _edges;
        private List<string> _names;

        public Graph()
        {
            _nodes = new List<GraphNode>();
            _names = new List<string>();
            _edges = new List<GraphEdge>();
        }

        public void AddEdge(string firstNodeName, string secondNodeName, int? firstNodeWeight = null, int? secondNodeWeight = null, int? edgeWeight = null)
        {
            if (_edges.All(x => !x.IsEqual(firstNodeName, secondNodeName)))
            {
                GraphNode firstGraphNode = new GraphNode(firstNodeName, firstNodeWeight);
                GraphNode secondGraphNode = new GraphNode(secondNodeName, secondNodeWeight);
                _nodes.Add(firstGraphNode);
                _nodes.Add(secondGraphNode);
                firstGraphNode.AddEdge(secondGraphNode);
                secondGraphNode.AddEdge(firstGraphNode);

                _edges.Add(new GraphEdge(firstGraphNode, secondGraphNode, edgeWeight));
            }
        }

    }
}

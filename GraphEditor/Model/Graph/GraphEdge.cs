using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model.Graph
{
    public class GraphEdge
    {
        public GraphNode FirstNode { get; private set; }

        public GraphNode SecondNode { get; private set; }

        public int Weight { get; private set; }

        private bool _isOriented;

        public GraphEdge(GraphNode firstNode, GraphNode secondNode, bool isOriented, int weight = 0)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
            _isOriented = isOriented;
        }

        public bool IsEqual(string firstName, string secondName)
        {
            if (_isOriented)
            {
                return Equals(firstName, FirstNode.Name) && Equals(secondName, SecondNode.Name);
            }
            else
            {
                return IsEqualForNonOriented(firstName, secondName);
            }
        }

        private bool IsEqualForNonOriented(string firstName, string secondName)
        {
            bool first = Equals(firstName, FirstNode.Name) && Equals(secondName, SecondNode.Name);
            bool second = Equals(firstName, SecondNode.Name) && Equals(secondName, FirstNode.Name);
            return first || second;
        }

        public GraphNode GetOtherNode(GraphNode node)
        {
            if (_isOriented)
            {
                return GetOtherNodeForNonOriented(node);
            }
            else if (!_isOriented)
            {
                return GetOtherNodeForOriented(node);
            }
            else
            {
                throw new ArgumentException($"Ребро не содержи такой узел {node.Name}");
            }
        }

        private GraphNode GetOtherNodeForOriented(GraphNode node)
        {
            if (Equals(node.Name, FirstNode.Name))
            {
                return SecondNode;
            }
            else
            {
                throw new ArgumentException($"Нельзя пройти в другой узел, так как граф ориентированный");
            }
        }

        private GraphNode GetOtherNodeForNonOriented(GraphNode node)
        {
            if (Equals(node.Name, FirstNode.Name))
            {
                return SecondNode;
            }
            else if (Equals(node.Name, SecondNode.Name))
            {
                return FirstNode;
            }
            else
            {
                throw new ArgumentException($"Ребро не содержи такой узел {node.Name}");
            }
        }

    }
}

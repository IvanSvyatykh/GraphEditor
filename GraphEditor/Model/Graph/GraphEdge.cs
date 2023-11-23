using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class GraphEdge
    {
        public GraphNode FirstNode { get; private set; }

        public GraphNode SecondNode { get; private set; }

        public int? Weight { get; private set; }

        public GraphEdge(GraphNode firstNode, GraphNode secondNode, int? weight = null)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
        }

        public bool IsEqual(string firstName, string secondName)
        {
            bool first = Equals(firstName, FirstNode.Name) && Equals(secondName, SecondNode.Name);
            bool second = Equals(firstName, SecondNode.Name) && Equals(secondName, FirstNode.Name);
            return first || second;
        }

        public  GraphNode GetOtherNode(GraphNode node)
        {
            if (Equals(node.Name, FirstNode.Name))
            {
                return SecondNode;
            }
            else if(Equals(node.Name,SecondNode.Name))
            {
                return FirstNode; 
            }
            else
            {
                throw new ArgumentException($"Ребро не содержи такой узел {node.Name}");
            }
        }

        public GraphNode GetNext()
        {
            return SecondNode;
        }

    }
}

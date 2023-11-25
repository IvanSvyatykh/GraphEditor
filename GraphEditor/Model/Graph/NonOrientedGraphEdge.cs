using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class NonOrientedGraphEdge
    {
        public NonOrientedGraphNode FirstNode { get; private set; }

        public NonOrientedGraphNode SecondNode { get; private set; }

        public int Weight { get; private set; }

        public NonOrientedGraphEdge(NonOrientedGraphNode firstNode, NonOrientedGraphNode secondNode, int weight = 0)
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

        public  NonOrientedGraphNode GetOtherNode(NonOrientedGraphNode node)
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

        public NonOrientedGraphNode GetNext()
        {
            return SecondNode;
        }

    }
}

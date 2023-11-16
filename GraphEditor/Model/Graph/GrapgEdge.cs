using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class GrapgEdge
    {
        public GraphNode FirstNode { get; private set; }

        public GraphNode SecondNode { get; private set; }

        public int? Weight { get; private set; }

        public GrapgEdge(GraphNode firstNode, GraphNode secondNode, int? weight = null)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
        }

        public GraphNode GetNext()
        {
            return SecondNode;
        }

    }
}

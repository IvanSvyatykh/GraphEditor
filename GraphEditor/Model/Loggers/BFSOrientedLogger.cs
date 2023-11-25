using Model.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    public class BFSOrientedLogger
    {
        public List<Tuple<string, OrientedEdge, string>> Visited { get; private set; }

        public BFSOrientedLogger()
        {
            Visited = new List<Tuple<string, OrientedEdge, string>>();
        }

        public void AddLog(string node, OrientedEdge edge, string comment)
        {
            Visited.Add(Tuple.Create(node, edge, comment));
        }
    }
}

using Model.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GraphEditor.Model.Loggers
{
    public class BFSOrientedLogger : ILogger
    {
        public List<Tuple<string, OrientedEdge, string>> Visited { get; private set; }

        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>> GetVisisted()
        {
            var visited = new List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string>>();
            

            foreach (Tuple<string, OrientedEdge, string> line in Visited)
            {
                if (line.Item2 is not null)
                {
                    visited.Add(Tuple.Create(new NonOrientedGraphNode(line.Item1), new NonOrientedGraphEdge(new NonOrientedGraphNode(line.Item2.StartNode), new NonOrientedGraphNode(line.Item2.EndNode)), line.Item3));
                }
                else
                {
                        visited.Add(new Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string> (new NonOrientedGraphNode(line.Item1), null, line.Item3));
                }
            }    
            
            return visited;
        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DijkstraLogger
    {
        // 0 - ничего не делаем и не выделяем лог просто с сообщением
        // 1 - пометить как вершину из которой происходит обход соседей(временным цветом)
        // 2 - пометить вершину как пройденную навсегда (каким-то ярким цветом типо красного)
        public List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string, byte>> Visited { get; private set; }

        public DijkstraLogger()
        {
            Visited = new List<Tuple<NonOrientedGraphNode, NonOrientedGraphEdge, string, byte>>();
        }


        public void AddLog(NonOrientedGraphNode node, NonOrientedGraphEdge edge, string comment, byte flag)
        {
            Visited.Add(Tuple.Create(node, edge, comment, flag));
        }

    }
}

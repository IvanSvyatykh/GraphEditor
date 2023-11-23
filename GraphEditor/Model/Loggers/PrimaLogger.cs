using Model.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.Model.Loggers
{
    public class PrimaLogger
    {
        // 0 ничего не выделять просто лог сообщения
        // 1 выделить вершину и ребро как взятую
        // 2 выделить вершину и ребро как возможную к рассмотрению
        // 3 выделить вершину и ребро временно, на следующем шаге убрать
        public List<Tuple<GraphNode, GraphEdge, string,byte>> Visited { get; private set; }

        public PrimaLogger()
        {
            Visited = new List<Tuple<GraphNode, GraphEdge, string, byte>>();
        }
        public List<Tuple<GraphNode, GraphEdge, string, byte>> GetVisisted()
        {
            throw new NotImplementedException();
        }
        
        public void AddLog(GraphNode node, GraphEdge edge, string comment,byte flag)
        {
            Visited.Add(Tuple.Create(node, edge, comment,flag));
        }
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public static class Helper
    {
        public static void TranslateToGraph(Dictionary<string, List<string>> matrix, Graph graph)
        {

            foreach (var key in matrix.Keys)
            {
                foreach (var node in matrix[key])
                {
                    graph.AddEdge(key, node);
                }
            }
        }

    }
}

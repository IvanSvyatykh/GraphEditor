using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class Graph
    {
        private GraphNode _graphSource;

        private GraphNode _graphStock;
        public Graph(GraphNode graphSource, GraphNode graphStock)
        {
            _graphSource = graphSource;
            _graphStock = graphStock;
        }

        public void ChangeSource(GraphNode graphSource)
        {
            _graphSource = graphSource;
        }

        public void ChangeStock()
        {
            _graphStock = _graphSource;
        }
    }
}

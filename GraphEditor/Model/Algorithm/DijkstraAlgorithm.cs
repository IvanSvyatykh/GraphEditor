using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DijkstraAlgorithm
    {
        private Graph _graph;
        private Dictionary<string, List<Tuple<int, string>>> _matrix;
        private DijkstraLogger _logger;
        public DijkstraAlgorithm(Dictionary<string, List<Tuple<int, string>>> matrix)
        {
            _logger = new DijkstraLogger();
            _matrix = matrix;
            Helper.TranslateToGraphWithWeights(_matrix, _graph);
        }

        public DijkstraLogger StartAlgorithm(string startNode, string destNode)
        {

            return _logger;
        }
    }
}

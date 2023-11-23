using GraphEditor.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class AlgorithmPrima
    {

        private Graph _graph;
        private Dictionary<string, List<string>> _matrix;
        private Dictionary<string, bool> _visited;
        private PrimaLogger _logger;
        public AlgorithmPrima(Dictionary<string, List<string>> matrix)
        {
            _matrix = matrix;
            _graph = new Graph();
            Helper.TranslateToGraph(_matrix, _graph);
        }

        public PrimaLogger StartAlgorithm()
        {
            _logger = new PrimaLogger();
            _visited = new Dictionary<string, bool>();
            _graph.Names.
                ToList().
                ForEach(name => { _visited.Add(name, false); });

           

            return _logger;
        }

        private void MinOstTree()
        {

        }

    }
}

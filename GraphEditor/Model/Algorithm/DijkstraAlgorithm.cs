using GraphEditor.Model.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Graph
{
    public class DijkstraAlgorithm
    {
        private NonOrientedGraph _graph;
        private Dictionary<string, List<Tuple<int, string>>> _matrix;
        private DijkstraLogger _logger;
        private Dictionary<string, bool> _visited;
        private Dictionary<string, int> _distance;
        private List<string> _prev;
        public DijkstraAlgorithm(Dictionary<string, List<Tuple<int, string>>> matrix)
        {
            _graph = new NonOrientedGraph();
            _matrix = matrix;
            Helper.TranslateToGraphWithWeights(_matrix, _graph);
        }

        public DijkstraLogger StartAlgorithm(string startNode, string destNode)
        {
            _prev = new List<string>();
            _logger = new DijkstraLogger();
            _distance = new Dictionary<string, int>();
            _visited = new Dictionary<string, bool>();
            _graph.Names.ToList().ForEach(name => { _visited.Add(name, false); _distance.Add(name, int.MaxValue); _prev.Add(name); });

            if (_distance.ContainsKey(startNode))
            {
                _distance[startNode] = 0;
            }
            else
            {
                throw new ArgumentException($"Узла с именем {startNode} не существует.");
            }

            Helper.CheckGraph(_graph);

            _logger.AddLog(null, null, $"Начинаем поиск пути из узла {startNode} в {destNode}, пути до всех вершин кроме {startNode} стоят бесконечность. Путь до узла {startNode} 0",
                0, Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
            DijkstraAlgorithmStart();
            _logger.AddLog(null, null, $"Обошли все вершины, путь из вершины {startNode} в {destNode} состовялет {_distance[destNode]} ", 0, Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
            return _logger;
        }


        private void DijkstraAlgorithmStart()
        {
            while (_visited.Any(x => !x.Value))
            {
                string currentNodeName = _distance.Where(y => !_visited[y.Key]).MinBy(x => x.Value).Key;
                _logger.AddLog(_graph.GetNodeByName(currentNodeName), null,
                    $"Берем вершину из еще не пройденных путь до которой стоит меньше всего, это вершина {currentNodeName}.", 1,
                    Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));

                foreach (NonOrientedGraphEdge edge in _graph.GetNodeByName(currentNodeName).Edges)
                {
                    if (_visited[edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name])
                    {
                        _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)), edge,
                           $"Путь до вершины {edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name} через {currentNodeName} не возможен, так как она уже пройдена.", 0,
                           Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
                    }
                    else if (_distance[edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name] > _distance[currentNodeName] + edge.Weight)
                    {
                        _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)), edge,
                            $"Путь до вершины {edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name} через {currentNodeName} стоит {_distance[currentNodeName] + edge.Weight}, это меньше чем текущеe значение {_distance[edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name]}",
                            1, Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
                        _distance[edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name] = _distance[currentNodeName] + edge.Weight;
                    }
                    else
                    {
                        _logger.AddLog(edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)), edge,
                           $"Путь до вершины {edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name} через {currentNodeName} стоит {_distance[currentNodeName] + edge.Weight}, это больше чем текущеe значение {_distance[edge.GetOtherNode(_graph.GetNodeByName(currentNodeName)).Name]}, ничего не меняем",
                           1, Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
                    }
                }

                _logger.AddLog(_graph.GetNodeByName(currentNodeName), null, $"Обошли всех сосодей из вершины {currentNodeName}, помечаем ее как пройденную.", 2, Helper.CloneDictionaryCloningValues<string, string>(DictionaryTranslator(_distance, _prev)));
                _visited[currentNodeName] = true;
            }
        }

        private Dictionary<string, string> DictionaryTranslator(Dictionary<string, int> current, List<string> prev)
        {
            Dictionary<string, string> newDic = new Dictionary<string, string>();

            for (int i = 0; i < prev.Count; i++)
            {
                if (current[current.Keys.ToList()[i]] == int.MaxValue)
                {
                    newDic.Add(prev[i], current.Keys.ToList()[i] + "-" + "∞");
                }
                else
                {
                    newDic.Add(prev[i], current.Keys.ToList()[i] + "-" + current[current.Keys.ToList()[i]]);
                }

                prev[i] = current.Keys.ToList()[i] + current[current.Keys.ToList()[i]];
            }

            return newDic;
        }
    }
}

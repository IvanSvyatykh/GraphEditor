using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WorkWithFile
{
    public static class Reader
    {
        public static Dictionary<string, List<Tuple<int, string>>> ReadGraph(string path)
        {
            using StreamReader streamReader = new StreamReader(path);
            string line = streamReader.ReadLine();
            if (!Equals(line, "Graph"))
            {
                throw new ArgumentException($"Файл с именем {path}, не может содержать граф или файл был поврежден.");
            }
            line = streamReader.ReadLine();
            if (Equals(line, "Non Oriented"))
            {
                return ReadNonOriented(streamReader);
            }
            else
            {
                return ReadOriented(streamReader);
            }
        }

        private static Dictionary<string, List<Tuple<int, string>>> ReadOriented(StreamReader streamReader)
        {
            throw new Exception("Не готово");
        }

        private static Dictionary<string, List<Tuple<int, string>>> ReadNonOriented(StreamReader streamReader)
        {
            string line = streamReader.ReadLine();
            string[] splitted = line.Trim(' ').TrimEnd(';').Split(";");
            Dictionary<string, List<Tuple<int, string>>> matrix = new Dictionary<string, List<Tuple<int, string>>>();

            splitted.ToList().ForEach(x => { matrix.Add(x, new List<Tuple<int, string>>()); });


            line = streamReader.ReadLine();
            while (line != null)
            {

                string[] nodesLength = line.TrimEnd(';').Split(";");
                for (int i = 1; i < nodesLength.Length; i++)
                {
                    if (int.TryParse(nodesLength[i], out int res))
                    {
                        matrix[nodesLength[0]].Add(Tuple.Create(res, splitted[i-1]));
                    }
                }
                line = streamReader.ReadLine();
            }

            return matrix;
        }
    }
}

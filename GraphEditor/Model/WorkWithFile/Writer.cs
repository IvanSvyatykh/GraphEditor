using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WriteToFile
{
    public static class Writer
    {

        public static void WriteGraph(Dictionary<string, List<Tuple<int, string>>> matrix, string path, bool IsOriented)
        {
            using StreamWriter streamWriter = new StreamWriter(path);


            //matrix = MatrixRecovery(matrix);
            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));
            streamWriter.WriteLine("Graph");
            if (IsOriented)
            {
                streamWriter.WriteLine("Oriented");
            }
            else
            {
                streamWriter.WriteLine("Non Oriented");
            }

            streamWriter.WriteLine(HeadFormer(matrix.Keys.ToList()));

            foreach (var item in matrix)
            {
                streamWriter.WriteLine(StringFormer(item.Key, item.Value, matrix.Keys.ToList()));
            }

            streamWriter.Close();
        }

        private static string HeadFormer(List<string> nodes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");

            foreach (var node in nodes)
            {
                sb.Append(node.ToString() + ";");
            }
            return sb.ToString();
        }

        private static string StringFormer(string key, List<Tuple<int, string>> line, List<string> keys)
        {
            line.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            StringBuilder sb = new StringBuilder();

            sb.Append(key + ";");

            foreach (var k in keys)
            {
                Tuple<int, string> tuple = line.FirstOrDefault(x => Equals(x.Item2, k));

                if (tuple != null)
                {
                    sb.Append(tuple.Item1 + ";");
                }
                else
                {
                    sb.Append("-;");
                }
            }

            return sb.ToString();
        }

        private static Dictionary<string, List<Tuple<int, string>>> MatrixRecovery(Dictionary<string, List<Tuple<int, string>>> matrix)
        {

            foreach (var key in matrix.Keys)
            {
                foreach (var item in matrix[key])
                {
                    var temp = Tuple.Create(item.Item1, key);
                    if (!matrix[item.Item2].Contains(temp))
                    {
                        matrix[item.Item2].Add(temp);
                    }
                }
            }


            return matrix;
        }
    }
}

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

        public static void WriteNonOriented(Dictionary<string, List<Tuple<int, string>>> matrix, string path)
        {
            using StreamWriter streamWriter = new StreamWriter(path);


            matrix = MatrixRecovery(matrix);
            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));
            streamWriter.WriteLine("Graph");
            streamWriter.WriteLine("Non Oriented");
            streamWriter.WriteLine(HeadFormer(matrix.Keys.ToList()));

            foreach (var item in matrix)
            {
                streamWriter.WriteLine(StringFormer(item.Key, item.Value, matrix.Keys.ToList().Count));
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

        private static string StringFormer(string key, List<Tuple<int, string>> line, int amount)
        {
            line.Sort((x, y) => x.Item2.CompareTo(y.Item2));

            StringBuilder sb = new StringBuilder();

            sb.Append(key + ";");
            int count = 0;

            foreach (var item in line)
            {
                sb.Append(item.Item1.ToString() + ";");
                count++;
            }
            if (count < amount)
            {
                for (int i = count; i < amount; i++)
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

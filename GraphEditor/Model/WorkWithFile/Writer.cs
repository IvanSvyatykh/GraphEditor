using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Model.Graph;
using System.Threading.Tasks;
using System.Windows;

namespace Model.WriteToFile
{
    public static class Writer
    {

        public static void WriteGraph(Dictionary<string, List<Tuple<int, string>>> matrix, Dictionary<string, Point> coordinats, string path, bool isOriented)
        {
            using StreamWriter streamWriter = new StreamWriter(path);

            if (!isOriented)
            {
                matrix = Helper.MatrixRecovery(matrix);
            }

            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));
            streamWriter.WriteLine("Graph");
            if (isOriented)
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

            streamWriter.WriteLine("Coordinats");

            foreach (var item in coordinats)
            {
                streamWriter.WriteLine(FormCoordinatsLine(item.Key, item.Value));
            }

            streamWriter.Close();
        }

        public static void WriteOstTree(Dictionary<string, List<Tuple<int, string>>> matrix, Dictionary<string, Point> coordinats, string path)
        {
            using StreamWriter streamWriter = new StreamWriter(path);


            matrix = new Dictionary<string, List<Tuple<int, string>>>(new SortedDictionary<string, List<Tuple<int, string>>>(matrix));
            streamWriter.WriteLine("Graph");
            streamWriter.WriteLine("OstTree");

            streamWriter.WriteLine(HeadFormer(matrix.Keys.ToList()));

            foreach (var item in matrix)
            {
                streamWriter.WriteLine(StringFormer(item.Key, item.Value, matrix.Keys.ToList()));
            }

            streamWriter.WriteLine("Coordinats");

            foreach (var item in coordinats)
            {
                streamWriter.WriteLine(FormCoordinatsLine(item.Key, item.Value));
            }

            streamWriter.Close();
        }

        private static string FormCoordinatsLine(string name, Point coordinate)
        {
            return name + $";{coordinate.X};{coordinate.Y}";
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

    }
}

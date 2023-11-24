using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Model.WorkWithFile
{
    public static class Reader
    {
        public static Tuple<Dictionary<string, List<Tuple<int, string>>>, Dictionary<string, Point>> ReadGraph(string path)
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
                return Tuple.Create(ReadGraph(streamReader),ReadCoordinats(streamReader));
            }
            else
            {
                return Tuple.Create(ReadGraph(streamReader), ReadCoordinats(streamReader));
            }
        }

        private static Dictionary<string, Point> ReadCoordinats(StreamReader streamReader)
        {
            Dictionary<string, Point> coordinats = new Dictionary<string, Point>();
            string line = streamReader.ReadLine();

            while (line != null)
            {
                string[] nodesLength = line.Split(";");
                coordinats.Add(nodesLength[0], new Point(int.Parse(nodesLength[1]), int.Parse(nodesLength[2])));
            }

            return coordinats;
        }

        private static Dictionary<string, List<Tuple<int, string>>> ReadOriented(StreamReader streamReader)
        {
            throw new Exception("Не готово");
        }

        private static Dictionary<string, List<Tuple<int, string>>> ReadGraph(StreamReader streamReader)
        {
            string line = streamReader.ReadLine();
            string[] splitted = line.Trim(' ').TrimEnd(';').Split(";");
            Dictionary<string, List<Tuple<int, string>>> matrix = new Dictionary<string, List<Tuple<int, string>>>();

            splitted.ToList().ForEach(x => { matrix.Add(x, new List<Tuple<int, string>>()); });


            line = streamReader.ReadLine();
            while (line != "Coordinats")
            {

                string[] nodesLength = line.TrimEnd(';').Split(";");
                for (int i = 1; i < nodesLength.Length; i++)
                {
                    if (int.TryParse(nodesLength[i], out int res))
                    {
                        matrix[nodesLength[0]].Add(Tuple.Create(res, splitted[i - 1]));
                    }
                }
                line = streamReader.ReadLine();
            }

            return matrix;
        }
    }
}

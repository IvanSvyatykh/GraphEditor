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

            Dictionary<string, List<Tuple<int, string>>> matrix = new Dictionary<string, List<Tuple<int, string>>>();

            while (line != null)
            {

            }

            return matrix;
        }
    }
}

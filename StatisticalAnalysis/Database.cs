using System.IO;
using System.Xml.Serialization;

namespace StatisticalAnalysis
{
    public class Database
    {
        public static TDatabase LoadDatabase<TDatabase>(string path)
        {
            var serializer = new XmlSerializer(typeof(TDatabase));
            using (var reader = new StreamReader(path))
            {
                return (TDatabase)serializer.Deserialize(reader);
            }
        }

        public void SaveDatabase<TDatabase>(string path)
        {
            var serializer = new XmlSerializer(typeof(TDatabase));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}

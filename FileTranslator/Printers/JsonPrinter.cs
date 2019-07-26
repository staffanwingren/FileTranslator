using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FileTranslator.Printers
{
    public class JsonPrinter<T>
    {
        public void Print(StreamWriter writer, IEnumerable<T> instances)
        {
            var serializer = new JsonSerializer();
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, instances);
            }
        }
    }
}
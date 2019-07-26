using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileTranslator.Helpers;

namespace FileTranslator.Parsers
{
    public class CsvParser<T>
    {
        public char Delimiter { get; set; } = ';';

        public IEnumerable<T> Parse(StreamReader reader)
        {
            string[] headers = null;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (headers == null)
                {
                    (Delimiter, headers) = line.DetectCsvFormat();
                    continue;
                }

                var values = line.Split(Delimiter);

                var instance = Activator.CreateInstance<T>();
                for (var i = 0; i < Math.Max(headers.Length, values.Length); i++)
                {
                    var propertyName = headers[i].Replace(" ", string.Empty);
                    var propertyInfo = instance.GetType().GetProperty(propertyName);
                    propertyInfo.SetValue(instance, values[i]);
                }

                yield return instance;
            }
        }
    }
}
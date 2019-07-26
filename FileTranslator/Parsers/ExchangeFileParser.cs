using System;
using System.Collections.Generic;
using System.IO;

namespace FileTranslator.Parsers
{
    public class ExchangeFileParser<T> where T : class, new()
    {
        public IEnumerable<T> Parse(StreamReader reader)
        {
            T current = null;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (current != null)
                    {
                        yield return current;
                        current = null;
                    }
                    continue;
                }

                current = current ?? Activator.CreateInstance<T>();

                var delimiterIndex = line.IndexOf(':');
                var propertyName = line.Substring(0, delimiterIndex).Trim();
                var property = typeof(T).GetProperty(propertyName);
                var value = line.Substring(delimiterIndex + 1).Trim();
                if (value.StartsWith('{'))
                {
                    while (!value.EndsWith('}'))
                    {
                        line = reader.ReadLine();
                        value += line.Trim();
                    }
                }

                property.SetValue(current, value);
            }

            if (current != null)
            {
                yield return current;
            }
        }
    }
}

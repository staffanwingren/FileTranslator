using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FileTranslator.Parsers
{
    public class PSListParser<T> where T : class, new()
    {
        private enum ParserState
        {
            HeaderName,
            HeaderLength,
            Content
        }

        public IEnumerable<T> Parse(StreamReader reader)
        {
            var headerDefinitions = new List<HeaderDefinition>();
            var state = ParserState.HeaderName;
            var headerNames = string.Empty;
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

                if (state == ParserState.HeaderName)
                {
                    headerNames = line;
                    state = ParserState.HeaderLength;
                }
                else if (state == ParserState.HeaderLength)
                {
                    var matches = Regex.Matches(line, @"(-+\s*)");
                    var currentHeaderPos = 0;
                    for (var i = 0; i < matches.Count; i++)
                    {
                        var length = matches[i].Groups[1].Length;
                        var name = headerNames.Substring(currentHeaderPos, length).Trim();
                        currentHeaderPos += length;
                        headerDefinitions.Add(new HeaderDefinition
                        {
                            Order = i - 1,
                            Name = name,
                            Length = length
                        });
                    }

                    state = ParserState.Content;
                }
                else
                {
                    current = current ?? Activator.CreateInstance<T>();
                    var currentPos = 0;
                    foreach (var headerDefinition in headerDefinitions)
                    {
                        var property = typeof(T).GetProperty(headerDefinition.Name);
                        var value = line.Substring(currentPos, headerDefinition.Length).Trim();
                        currentPos += headerDefinition.Length;
                        property.SetValue(current, value);
                    }

                    yield return current;
                }
            }
        }

        private struct HeaderDefinition
        {
            public int Order;
            public string Name;
            public int Length;
        }
    }
}

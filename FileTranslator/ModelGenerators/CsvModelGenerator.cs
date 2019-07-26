using System;
using System.IO;
using FileTranslator.Helpers;

namespace FileTranslator.ModelGenerators
{
    public class CsvModelGenerator
    {
        public Type GenerateModel(StreamReader reader)
        {
            string[] headers = {};
            var hasDetectedHeaders = false;
            while (!hasDetectedHeaders && !reader.EndOfStream)
            {
                var line = reader.ReadLine();
                (_, headers) = line.DetectCsvFormat();
                hasDetectedHeaders = headers != null;
            }

            return headers.GenerateTypeFromHeaders();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FileTranslator.Helpers;

namespace FileTranslator.ModelGenerators
{
    public class CsvModelGenerator
    {
        public Type GenerateModel(StreamReader reader)
        {
            string[] headers = {};
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                (_, headers) = line.DetectCsvFormat();
            }

            return typeof(object);
        }
    }
}

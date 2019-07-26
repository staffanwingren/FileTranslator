using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTranslator.Printers
{
    public class CsvPrinter<T>
    {
        public void Print(StreamWriter writer, IEnumerable<T> instances)
        {
            var properties = typeof(T).GetProperties();
            var propertyNameStringBuilder = new StringBuilder();
            foreach (var propertyInfo in properties)
            {
                propertyNameStringBuilder.Append($"{propertyInfo.Name};");
            }

            writer.WriteLine(propertyNameStringBuilder);

            foreach (var instance in instances)
            {
                var stringBuilder = new StringBuilder();
                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(instance);
                    stringBuilder.Append($"{value};");
                }

                writer.WriteLine(stringBuilder);
            }
        }
    }
}

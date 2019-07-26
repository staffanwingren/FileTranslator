using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FileTranslator.Helpers
{
    public static class CsvHelpers
    {
        public static char DetectCsvDelimiter(this string line)
        {
            var delimiterIndex = line.IndexOfAny(new[] {',', ';'});
            return line[delimiterIndex];
        }

        public static (char, string[]) DetectCsvFormat(this string line)
        {
            var delimiter = line.DetectCsvDelimiter();
            var values = line.Split(delimiter);
            return (delimiter, values);
        }
    }
}

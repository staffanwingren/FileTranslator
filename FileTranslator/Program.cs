using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FileTranslator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new Arguments();
            Action<string> valueCaptureAction = s => { };
            foreach (var arg in args)
            {
                if (arg.StartsWith('-'))
                {
                    valueCaptureAction = GetCaptureAction(arguments, arg);
                }
                else
                {
                    valueCaptureAction(arg);
                }
            }

            if (arguments.IsValid)
            {
                Run(arguments);
            }
            else
            {
                Console.WriteLine("Invalid arguments.");
            }

            Console.WriteLine("Translation complete - press any key to continue.");
            Console.ReadLine();
        }

        private static Action<string> GetCaptureAction(Arguments builder, string arg)
        {
            switch (arg)
            {
                case "-in":
                    return s =>
                    {
                        builder.In = s.Trim('\'').Trim('"');
                        builder.Parser = string.IsNullOrEmpty(builder.Parser) ? FindComponentNameFromFileExtension(builder.In) : builder.Parser;
                    };
                case "-out":
                    return s =>
                    {
                        builder.Out = s.Trim('\'').Trim('"');
                        builder.Printer = string.IsNullOrEmpty(builder.Printer) ? FindComponentNameFromFileExtension(builder.Out) : builder.Printer;
                    };
                case "-encoding":
                    return s =>
                    {
                        var encoding = Encoding.GetEncoding(s);
                        builder.InputEncoding = encoding;
                        builder.OutputEncoding = encoding;
                    };
                case "-input-encoding":
                    return s => builder.InputEncoding = Encoding.GetEncoding(s);
                case "-output-encoding":
                    return s => builder.InputEncoding = Encoding.GetEncoding(s);
                case "-parser":
                    return s => builder.Parser = s;
                case "-printer":
                    return s => builder.Printer = s;
                case "-model":
                    return s => builder.Model = s;
            }

            return s => { };
        }

        private static void Run(Arguments arguments)
        {
            var modelType = GenerateModelFromInput(arguments);
            var parserType = Type.GetType($"FileTranslator.Parsers.{arguments.Parser}Parser`1");
            var genericParserType = parserType.MakeGenericType(modelType);
            var parser = Activator.CreateInstance(genericParserType);

            var printerType = Type.GetType($"FileTranslator.Printers.{arguments.Printer}Printer`1");
            var genericPrinterType = printerType.MakeGenericType(modelType);
            var printer = Activator.CreateInstance(genericPrinterType);

            var parseMethod = genericParserType.GetMethod("Parse");
            var printMethod = genericPrinterType.GetMethod("Print");

            var outFileInfo = new FileInfo(arguments.Out ?? "out.txt");
            Directory.CreateDirectory(outFileInfo.DirectoryName);

            using (var inStream = File.OpenRead(arguments.In))
            using (var reader = new StreamReader(inStream, arguments.InputEncoding))
            using (var outStream = File.OpenWrite(outFileInfo.FullName))
            using (var writer = new StreamWriter(outStream, arguments.OutputEncoding))
            {
                var parseResult = parseMethod.Invoke(parser, new object[] {reader});
                printMethod.Invoke(printer, new[] {writer, parseResult});
            }
        }

        private static string FindComponentNameFromFileExtension(string filePath)
        {
            var fileExtension = filePath.Split('.').Last();
            fileExtension = $"{char.ToUpperInvariant(fileExtension[0])}{fileExtension.Substring(1).ToLowerInvariant()}";
            return fileExtension;
        }

        private static Type GenerateModelFromInput(Arguments arguments)
        {
            if (!string.IsNullOrWhiteSpace(arguments.Model))
            {
                return Type.GetType($"FileTranslator.Models.{arguments.Model}");
            }

            var modelGeneratorType = Type.GetType($"FileTranslator.ModelGenerators.{arguments.Parser}ModelGenerator");
            var modelGenerator = Activator.CreateInstance(modelGeneratorType);
            var generatorMethod = modelGeneratorType.GetMethod("GenerateModel");

            using (var inStream = File.OpenRead(arguments.In))
            using (var reader = new StreamReader(inStream, arguments.InputEncoding))
            {
                return (Type)generatorMethod.Invoke(modelGenerator, new object[] {reader});
            }
        }
    }
}

using System.IO;
using System.Text;

namespace FileTranslator
{
    public class Arguments
    {
        public Arguments()
        {
            InputEncoding = Encoding.UTF8;
            OutputEncoding = Encoding.UTF8;
        }

        public string In { get; set; }

        public Encoding InputEncoding { get; set; } 

        public string Out { get; set; }

        public Encoding OutputEncoding { get; set; }

        public string Model { get; set; }

        public string Parser { get; set; }

        public string Printer { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(In) &&
                               //!string.IsNullOrWhiteSpace(Out) &&
                               //!string.IsNullOrWhiteSpace(Model) &&
                               !string.IsNullOrWhiteSpace(Parser) &&
                               !string.IsNullOrWhiteSpace(Printer) &&
                               File.Exists(In);
    }
}
using CommandLine;

using PdfiumViewer;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace pdfconv
{
    class Options
    {

        [Value(0, MetaName = "target_pdf", Required = true, HelpText = "PDF file name to convert")]
        public string fileName { get; set; }

        [Option('o', "outdir", Default = ".\\", HelpText = "base directory to save image")]
        public string OutputDir { get; set; }

        [Option('f', "force",
          Default = false,
          HelpText = "Overwrite image, if exists")]
        public bool OverwriteImage { get; set; }

        [Option('d', "dpi",
          Default = 300f,
          HelpText = "Image dpi")]
        public float Dpi{ get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);

        }

        static void RunOptions(Options opts)
        {
            //handle options

            if (!File.Exists(opts.fileName))
            {
                ColorConsole.WriteLine($"File Not Found. {opts.fileName}", ConsoleColor.Red);
                return;
            }

            using (var doc = PdfDocument.Load(opts.fileName))
            {
                try
                {
                    var outputPath = Path.Combine(
                        Path.GetFullPath(opts.OutputDir),
                        Path.GetFileNameWithoutExtension(opts.fileName) ); 

                    Directory.CreateDirectory(outputPath);

                    doc.SaveToImages( outputPath, ImageFormat.Jpeg, opts.OverwriteImage, opts.Dpi );
                }
                catch(Exception e)
                {
                    ColorConsole.WriteLine(e.Message, ConsoleColor.Red);
                    return;
                }

            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }

    }
}

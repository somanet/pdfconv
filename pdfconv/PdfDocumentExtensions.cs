using PdfiumViewer;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pdfconv
{
    public static class PdfDocumentExtensions
    {
        public static void SaveToImages(this PdfDocument document, string outputPath, ImageFormat imageFormat, bool overwriteImage = true)
        {
            var extension = imageFormat.ToString().ToLower();
            int skipCount = 0;

            var dpi = GetSystemDpi();

            for(var i=0; i< document.PageCount; i++)
            {
                var imgFileName = Path.Combine(outputPath, $"{i}.{extension}");

                if ( File.Exists(imgFileName) &&  !overwriteImage)
                {
                    skipCount++;

                    ColorConsole.WriteLine($"Skip extraction. {imgFileName} exists.", ConsoleColor.Yellow);
                    continue;
                }

                var pageSize = document.PageSizes[i];

                var image = document.Render(i,
                    PointToPixel(pageSize.Width, dpi.Width)*2,
                    PointToPixel(pageSize.Height, dpi.Height)*2, 
                    200,
                    200,  
                    false);


                image.Save(imgFileName,imageFormat);

                ColorConsole.WriteLine($"extract {i+1}/{document.PageCount} page to {imgFileName}", ConsoleColor.White);
            }


            if(skipCount > 0)
            {
                ColorConsole.WriteLine($"\nExtracted {document.PageCount-skipCount} page(s). Skipped {skipCount} page(s)", ConsoleColor.Green);
            }
            else
            {
                ColorConsole.WriteLine($"\nExtracted {document.PageCount} page(s).", ConsoleColor.Green);
            }
        }

        private static SizeF GetSystemDpi()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                return new SizeF(graphics.DpiX, graphics.DpiY);
        }

        private static int PointToPixel(float pointValue, float dpi)
        {
            return (int)Math.Round(pointValue * dpi / 72);
        }

    }


}

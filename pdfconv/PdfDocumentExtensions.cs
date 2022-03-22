using PdfiumViewer;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pdfconv
{
    public static class PdfDocumentExtensions
    {
        /// <summary>
        /// 자릿수(place value)구하기
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static uint GetPlaceValue(uint number)
        {
            uint power = 1;
            uint placeVal = 0;
            while (true)
            {
                if (number / Math.Pow(10, power) < 1)
                {
                    placeVal = power;
                    break;
                }

                power++;
            }

            return placeVal;
        }
        public static void SaveToImages(this PdfDocument document, string outputPath, ImageFormat imageFormat, bool overwriteImage, float dpi = 300f)
        {
            var extension = imageFormat.ToString().ToLower();
            int skipCount = 0;

            for(var pageIdx=0; pageIdx< document.PageCount; pageIdx++)
            {
                var imgFileName = Path.Combine(outputPath, $"{(pageIdx+1).ToString($"D{GetPlaceValue((uint)document.PageCount)}")}.{extension}");

                if ( File.Exists(imgFileName) &&  !overwriteImage)
                {
                    skipCount++;

                    ColorConsole.WriteLine($"Skip converting. {imgFileName} exists.", ConsoleColor.Yellow);
                    continue;
                }

                var pageSize = document.PageSizes[pageIdx];

                 
                using (var image = document.Render( pageIdx, 
                    (int)Math.Round(PointToPixel(pageSize.Width, dpi)),
                    (int)Math.Round(PointToPixel(pageSize.Height, dpi)), dpi, dpi, true))
                {
                    image.Save(imgFileName, imageFormat);
                }

                var percentage = (int)Math.Floor(((pageIdx + 1) / (double)document.PageCount) * 100);

                ColorConsole.WriteLine($"[{percentage}%] Extracted {pageIdx} ({pageIdx+1}/{document.PageCount}) page into {imgFileName}", ConsoleColor.White);
            }


            if(skipCount > 0)
                ColorConsole.WriteLine($"\nExtracted {document.PageCount-skipCount} page(s). Skipped {skipCount} page(s)", ConsoleColor.Green);
            else
                ColorConsole.WriteLine($"\nExtracted {document.PageCount} page(s).", ConsoleColor.Green);
        }

        private static SizeF GetSystemDpi()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                return new SizeF(graphics.DpiX, graphics.DpiY);
        }

        private static float PointToPixel(float pointValue, float dpi)
        {
            return (float)Math.Round(pointValue * dpi / 72);
        }

    }


}

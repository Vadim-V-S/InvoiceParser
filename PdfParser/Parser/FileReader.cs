using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;
using System.Reflection;

namespace PdfParser.Parser
{
    public class FileReader
    {
        string folderPath;

        public FileReader(string folderPath)
        {
            //string folderName = "Счета";
            //this.folderPath = Path.Combine(folderPath, folderName);
            this.folderPath = folderPath;
        }

        public FileInfo[] GetFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            var pdfFiles = directoryInfo.GetFiles("*.pdf");
            if (pdfFiles.Length > 0)
            {
                ConvertPdfToPng(pdfFiles);
            }

            //var files = directoryInfo.GetFiles("*.png");
            var files = directoryInfo.GetFiles("*.jpg")
                .Union(directoryInfo.GetFiles("*.jpeg"))
                .Union(directoryInfo.GetFiles("*.gif"))
                .Union(directoryInfo.GetFiles("*.bmp"))
                .Union(directoryInfo.GetFiles("*.png")).ToArray();

            return files;
        }

        private void ConvertPdfToPng(FileInfo[] pdfFiles)
        {
            foreach (var pdf in pdfFiles)
            {
                var pdfName = Path.GetFileNameWithoutExtension(pdf.FullName);
                var fileName = Path.Combine(folderPath, pdfName);

                int desired_dpi = 203;

                string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
                GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);

                using (var rasterizer = new GhostscriptRasterizer())
                {
                    rasterizer.Open(pdf.FullName, gvi, false);

                    //for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                    //{
                    //var pageFilePath = Path.Combine(folderPath, string.Format(fileName + "-{0}.png", pageNumber));

                    //var img = rasterizer.GetPage(desired_dpi, pageNumber);
                    //img.Save(pageFilePath, ImageFormat.Png);

                    var pageFilePath = Path.Combine(folderPath, string.Format(fileName + ".png"));

                    var img = rasterizer.GetPage(desired_dpi, 1);
                    img.Save(pageFilePath, ImageFormat.Png);

                    Console.WriteLine($"{pdf.Name} сконвертирован в {pdfName}.png");
                    //}
                }
            }
        }
    }
}

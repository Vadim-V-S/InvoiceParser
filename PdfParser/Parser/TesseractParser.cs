using PdfParser.Extensions;
using PdfParser.ReferenceData;
using System.Reflection;
using Tesseract;

namespace PdfParser.Parser
{
    public class TesseractParser
    {
        private readonly string filePath;
        private TesseractEngine engine;
        public TesseractParser(string filePath)
        {
            this.filePath = filePath;

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = Path.Combine(path, "tessdata");
            path = path.Replace("file:\\", "");
            engine = new TesseractEngine(path, "rus+eng", EngineMode.Default);
        }


        public string ParseImage()
        {
            var image = Pix.LoadFromFile(filePath);
            var page = engine.Process(image);

            var text = page.GetText();

            return text;
        }
    }
}


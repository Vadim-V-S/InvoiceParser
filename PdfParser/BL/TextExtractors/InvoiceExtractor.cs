using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // Выборка счета в целом использует базовую реализацию за исключением ExtractText
    public class InvoiceExtractor : TextExtractor, ITextExtractor
    {
        public InvoiceExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Invoice();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new Invoice());
        }

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var extractions = parsedData.CreateListByKeyWords(keyWords);
            var result = extractions.CreateListByKeyWords(new List<string>() { " от " });

            return result;
        }

        public override string GetResultValue()
        {
            var result = GetResultByIndex(new Invoice(), comparator.GetIndexByPartialRatio, keyWords);
            usedTokens[token.invoice] = result;

            return result;
        }
    }
}

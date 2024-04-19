using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    public class InvoiceNumberExtractor : TextExtractor, ITextExtractor
    {
        public InvoiceNumberExtractor(List<string> parsedData) : base(parsedData)
        {
            usedTokens.Clear();

            referenceData = new Invoice();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new Invoice());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var extractions = parsedData.CreateListByKeyWords(keyWords);
            var result = analyzer.ReturnElementsByHeaviestWeights(extractions, keyWords);

            return result;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = GetResultByIndex(extraction, new Invoice(), comparator.GetIndexByPartialRatio, keyWords);
            usedTokens[token.invoice] = result;

            return result;
        }
    }
}

using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    public class InvoiceNumberExtractor : DataExtractor, ITextExtractor
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
            var slice = parsedData.CreateListByKeyWords(keyWords);
            var result = analyzer.ReturnElementsByHeavyWeights(slice, keyWords);

            return result;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new Invoice(), comparator.GetIndexByPartialRatio, keyWords);
            }
            usedTokens[token.invoice] = result;

            return result;
        }
    }
}

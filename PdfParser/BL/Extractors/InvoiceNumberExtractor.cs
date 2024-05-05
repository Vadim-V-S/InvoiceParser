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
            keyWords = referenceData.GetKeyTokens();

            comparator = new Comparator(new Invoice());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.CutOffFooter(paymentHeaderTokens);

            var extract = slice.CreateListByKeyTokens(keyWords);
            extract = analyzer.ReturnElementsIgnoringWeakOnes(extract, keyWords);
            return analyzer.ReturnElementsByHeaviestWeights(extract, keyWords);
        }

        public override string GetResultValue()
        {
            var extraction = ClearResult(ExtractData(keyWords));

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new Invoice(), comparator.GetIndexByPartialRatio, keyWords);
            }
            //usedTokens[token.invoice] = result;

            return result;
        }
    }
}

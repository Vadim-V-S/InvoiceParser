using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extentions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // инн плательщика
    public class PayerInnExtractor : TextExtractor, ITextExtractor
    {
        public PayerInnExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerInn();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new PayerInn());
        }

        public override string GetResultValue()
        {
            var result = GetResultByIndex(new RecipientInn(), comparator.GetIndexByPartialRatio, keyWords).ExtractNextWordByReferenceText("инн");
            return result.GetNumberFromStringByLength(10);
        }
    }
}

using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extentions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    //инн получателя
    public class RecipientInnExtractor : TextExtractor, ITextExtractor
    {
        public RecipientInnExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientInn();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new RecipientInn());
        }

        public override string GetResultValue()
        {
            var result = GetResultByIndex(new RecipientInn(), comparator.GetIndexByPartialRatio, keyWords);
            return result.GetNumberFromStringByLength(10);
        }
    }
}


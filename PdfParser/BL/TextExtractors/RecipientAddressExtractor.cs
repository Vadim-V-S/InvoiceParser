using PdfParser.Extentions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // аддрес получателя
    public class RecipientAddressExtractor : TextExtractor
    {
        public RecipientAddressExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientAddress();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new RecipientAddress());
        }

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListBeforeWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction.RemoveElementsFromListByWords(exclusions);

            var result = parsedData.GetClosestElementToWord(usedValues[0], extraction);

            return result;
        }

        // GetResultByIndex ипользуем базовую логику

        public override string GetResultValue()
        {
            var result= GetResultByIndex(new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            usedValues.Add(result);
            return result;
        }
    }
}

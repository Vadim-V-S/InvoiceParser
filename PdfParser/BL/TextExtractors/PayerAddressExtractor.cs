using PdfParser.Extentions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // адрес плательщика
    public class PayerAddressExtractor : TextExtractor
    {
        public PayerAddressExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerAddress();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new PayerAddress());
        }

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListBeforeWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction.RemoveElementsFromListByWords(exclusions);
            extraction.RemoveElementsFromListByWords(usedValues);
            if (usedValues.Count > 0)
            {
                var result = parsedData.GetClosestElementToWord(usedValues[1], extraction); // находим адрес который находится ближе всего к плательщику, у получателя такого не делали, т.к первым адрес в списке скорее всего всегда будет получателя

                usedValues.Clear();
                return result;
            }
            return extraction;
        }

        public override string GetResultValue()
        {
            return GetResultByIndex(new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
        }
    }
}

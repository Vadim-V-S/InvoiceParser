using PdfParser.Extensions;
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
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction.RemoveElementsFromListByWords(exclusions);
            extraction.RemoveElementsFromListByToken(usedTokens[token.recipientAddress]);
            if (usedTokens.Count > 0)
            {
                var result = parsedData.GetClosestElementToWord(usedTokens[token.recipientAddress], extraction); // находим адрес который находится ближе всего к плательщику, у получателя такого не делали, т.к первым адрес в списке скорее всего всегда будет получателя

                usedTokens.Clear();
                return result;
            }
            return extraction;
        }

        public override string GetResultValue()
        {
            var result = GetResultByIndex(new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.payerAddress] = result;

            return result;
        }
    }
}

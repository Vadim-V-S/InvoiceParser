using PdfParser.Extensions;
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
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction.RemoveElementsFromListByWords(exclusions);

            var result = parsedData.GetClosestElementToWord(usedTokens[token.recipientName], extraction);

            return result;
        }

        // GetResultByIndex ипользуем базовую логику

        public override string GetResultValue()
        {
            var result= GetResultByIndex(new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.recipientAddress] = result;  // запоминаем наш выбор в статическом списке

            return result;
        }
    }
}

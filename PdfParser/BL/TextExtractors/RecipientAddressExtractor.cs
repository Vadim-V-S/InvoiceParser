using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.CompanyAddress;

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

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction = extraction.RemoveElementsFromListByWords(exclusions);
            //extraction = extraction.RemoveTheOnlyWordElementFromList();

            //extraction = parsedData.GetClosestElementToWord(usedTokens[token.recipientName], extraction);
            return new List<string>() { parsedData.ReturnNextItemWhenContainsKeyWord(extraction, usedTokens[token.recipientName]) };
            //return extraction;
        }

        // GetResultByIndex ипользуем базовую логику

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            }
            usedTokens[token.recipientAddress] = result;  // запоминаем наш выбор в статическом списке

            return result;
        }
    }
}

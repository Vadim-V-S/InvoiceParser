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
            //extraction = analyzer.ReturnElementsByHeavyWeights(extraction, keyWords.Union(referenceData.GetReferenceWords()).ToList());

            extraction = parsedData.GetClosestElementToWord(usedTokens[token.recipientName], extraction);
            return extraction;
        }

        // GetResultByIndex ипользуем базовую логику

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = GetResultByIndex(extraction, new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.recipientAddress] = result;  // запоминаем наш выбор в статическом списке

            return result;
        }
    }
}

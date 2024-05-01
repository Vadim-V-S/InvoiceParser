using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.CompanyAddress;

namespace PdfParser.BL.TextExtractors
{
    // адрес плательщика
    public class PayerAddressExtractor : DataExtractor
    {
        public PayerAddressExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerAddress();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new PayerAddress());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListByTwoWords(GetLastUsedToken(), paymentHeaderTokens);

            var extraction = slice.CreateListByKeyTokens(keyWords);
            extraction = extraction.RemoveElementsFromListByExclusions(exclusions);
            keyWords.Add(".");
            keyWords.Add(",");
            extraction = analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords);
            //extraction = parsedData.GetClosestElementToWord(usedTokens[token.payerName], extraction);

            if (usedTokens.Count > 0)
            {
                extraction = extraction.RemoveElementsFromListByToken(usedTokens[token.recipientAddress]);

                //return extraction;
                //return new List<string>() { parsedData.ReturnNextItemWhenContainsKeyWord(extraction, usedTokens[token.payerName]) };
            }

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = "Нет Данных!";
            if (extraction.Count > 0)
            {
                result = GetResultByIndex(extraction, new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            }
            usedTokens[token.payerAddress] = result;

            return result;
        }
    }
}

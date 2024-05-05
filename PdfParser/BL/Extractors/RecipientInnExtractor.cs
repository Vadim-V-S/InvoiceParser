using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyInn;

namespace PdfParser.BL.TextExtractors
{
    //инн получателя
    public class RecipientInnExtractor : DataExtractor, ITextExtractor
    {
        public RecipientInnExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientInn();
            keyWords = referenceData.GetKeyTokens();
            exclusions= referenceData.GetExclusions();

            comparator = new Comparator(new RecipientInn());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var index = parsedData.IndexOf("-recipientInn-");

            if (index == -1)
            {
                var slice = parsedData.CutOffFooter(paymentHeaderTokens);

                var extraction = slice.CreateListByKeyTokens(keyWords);
                extraction = extraction.RemoveElementsFromListByExclusions(exclusions);
                extraction = extraction.RemoveListElementsWithoutDigits();

                return extraction;
            }

            return new List<string>() { parsedData[index + 1] };
        }

        public override string GetResultValue()
        {
            var extraction = ClearResult(ExtractData(keyWords));

            var inn = GetResultByIndex(extraction, new RecipientInn(), comparator.GetIndexByPartialRatio, keyWords);
            inn = inn.GetNextWordByReferenceText("ИНН ");
            inn = inn.GetNumberFromStringByLength(10);

            var result = "Нет Данных!";
            if (inn != null && inn.Length >= 10 && inn.Length <= 12 && inn.IsStringDigits())
            {
                return usedTokens[token.recipientInn] = inn;
            }

            return usedTokens[token.recipientInn] = result;
        }
    }
}


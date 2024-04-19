using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyInn;

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

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction = extraction.RemoveTheOnlyWordElementFromList();
            extraction = extraction.RemoveListElementsWithoutDigits();

            if (usedTokens[token.recipientInn] != "Нет данных!")
            {
                extraction = extraction.RemoveElementsFromListByToken(usedTokens[token.recipientInn]);
                
                return analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords.Union(referenceData.GetReferenceWords()).ToList());
            }

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var inn = GetResultByIndex(extraction, new PayerInn(), comparator.GetIndexByPartialRatio, keyWords);
            inn = inn.GetNextWordByReferenceText("ИНН ");
            inn = inn.GetNumberFromStringByLength(10);

            var result = "Нет Данных!";
            //if (inn != null && inn.Length == 10 && inn.IsStringDigits())
            if (inn != null && inn.Length >= 10 && inn.IsStringDigits())
            {
                return usedTokens[token.payerInn] = inn;
            }

            return usedTokens[token.payerInn] = result;
        }
    }
}

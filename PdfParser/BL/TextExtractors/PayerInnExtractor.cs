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

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction = extraction.RemoveTheOnlyWordElementFromList();
            extraction = extraction.RemoveListElementsWithoutDigits();

            if (usedTokens[token.recipientInn] != "Нет данных!")
            {
                extraction = extraction.RemoveElementsFromListByToken(usedTokens[token.recipientInn]);

                var weghtsMatrix = extraction.SetWeightsByKeyWords(keyWords.Union(referenceData.GetReferenceWords()).ToList());

                if (weghtsMatrix.Count > 0)
                {
                    extraction = weghtsMatrix.GetMostHeavyElements();
                }
            }

            return extraction;
        }

        public override string GetResultValue()
        {
            var inn = GetResultByIndex(new PayerInn(), comparator.GetIndexByPartialRatio, keyWords).GetNextWordByReferenceText("инн");
            //inn = inn.GetNextWordByReferenceText("ИНН ").Replace(",","").Trim();
            inn = inn.Replace(",","").Trim();
            inn = inn.GetNumberFromStringByLength(10);
            var result = "Нет Данных!";
            //if (inn != null && inn.Length == 10 && inn.IsStringDigits())
            if (inn != null && inn.Length >= 10 && inn.IsStringDigits())
            {
                usedTokens[token.payerInn] = inn;
            }

                return usedTokens[token.payerInn] = result;
        }
    }
}

using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.CompanyName;

namespace PdfParser.BL.TextExtractors
{
    // Плательщик платежа
    public class PayerNameExtractor : TextExtractor
    {
        public PayerNameExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerName();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new PayerName());
        }

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListByTwoWords(usedTokens[token.recipientName], endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords.Union(referenceData.GetReferenceWords()).ToList());
            extraction.RemoveElementsFromListByWords(exclusions);
            extraction.RemoveTheOnlyWordElementFromList();
            if (usedTokens[token.recipientName] != "Нет данных!")
            {                
                extraction = extraction.RemoveElementsFromListByToken(usedTokens[token.recipientName]); // все тоже самое как и получателя, только дополнительно удаляем уже выбранного получателя платежа из списка

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
            var result = GetResultByExtraction(new PayerName(), comparator.ExtractOne, comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.payerName] = result.Replace(" - Уровень доверия низкий!", "").Trim();  // запоминаем наш выбор в статическом списке

            return result;
        }
    }
}

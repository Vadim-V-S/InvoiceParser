using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyName;

namespace PdfParser.BL.TextExtractors
{
    // Получатель платежа
    public class RecipientNameExtractor : TextExtractor
    {
        public RecipientNameExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientName();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new RecipientName());
        }
        internal override List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords); // выборка по ключевым словам
            extraction.RemoveElementsFromListByWords(exclusions);      // удаление лишнего по словам исключениям
            var result = extraction.RemoveTheOnlyWordElementFromList();  // удаление элементов состоящщих из одного слова

            var weghtsMatrix = result.SetWeightsByKeyWords(keyWords.Union(referenceData.GetReferenceWords()).ToList());

            if (weghtsMatrix.Count > 0)
            {
                result = weghtsMatrix.GetMostHeavyElements();
            }


            return result;
        }

        public override string GetResultValue()
        {
            //var result = GetResultByExtraction(new RecipientName(), comparator.ExtractOne, comparator.GetIndexByTokenRatio, keyWords);
            var result = GetResultByIndex(new RecipientName(), comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.recipientName] = result.Replace(" - Уровень доверия низкий!", "").Trim();  // запоминаем наш выбор в статическом списке
            
            return result;
        }
    }
}

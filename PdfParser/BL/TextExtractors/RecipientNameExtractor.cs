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
        internal override List<string> ExtractData(List<string> keyWords)
        {
            List<string> result = new List<string>();

            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords); // выборка по ключевым словам
            extraction.RemoveElementsFromListByWords(exclusions);      // удаление лишнего по словам исключениям
            extraction = extraction.RemoveTheOnlyWordElementFromList();  // удаление элементов состоящщих из одного слова
            
            result = analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords.Union(referenceData.GetReferenceWords()).ToList());

            return result;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = GetResultByIndex(extraction, new RecipientName(), comparator.GetIndexByTokenRatio, keyWords);
            usedTokens[token.recipientName] = result.Replace(" - Уровень доверия низкий!", "").Trim();  // запоминаем наш выбор в статическом списке
            
            return result;
        }
    }
}

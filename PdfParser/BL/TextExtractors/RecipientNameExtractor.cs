using PdfParser.Extentions;
using PdfParser.ReferenceData;

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
            var slice = parsedData.SliceListBeforeWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords); // выборка по ключевым словам
            extraction.RemoveElementsFromListByWords(exclusions);      // удаление лишнего по словам исключениям
            var result = extraction.RemoveTheOnlyWordElementFromList();  // удаление элементов состоящщих из одного слова
            return result;
        }

        public override string GetResultValue()
        {
            var result = GetResultByExtraction(new RecipientName(), comparator.ExtractOne, comparator.GetIndexByTokenRatio, keyWords);
            var valueToSave = result.Replace(" - Уровень доверия низкий!", "");
            usedValues.Add(valueToSave);  // запоминаем наш выбор в статическом списке
            return result;
        }
    }
}

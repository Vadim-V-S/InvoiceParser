using PdfParser.Extentions;
using PdfParser.ReferenceData;

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
            //var slice = parsedData.SliceListBeforeWords(endSliceWords);
            var slice = parsedData.SliceListByTwoWords(usedValues, endSliceWords);
            var extractions = slice.CreateListByKeyWords(keyWords);
            extractions.RemoveElementsFromListByWords(exclusions);
            extractions.RemoveTheOnlyWordElementFromList();
            if (usedValues.Count > 0)
            {
                extractions.RemoveElementsFromListByWords(usedValues); // все тоже самое как и получателя, только дополнительно удаляем уже выбранного получателя платежа из списка
            }

            return extractions;
        }

        public override string GetResultValue()
        {
            var result = GetResultByExtraction(new PayerName(), comparator.ExtractOne, comparator.GetIndexByTokenRatio, keyWords);
            var valueToSave = result.Replace(" - Уровень доверия низкий!", "");
            usedValues.Add(valueToSave); // плательщика тоже добавляем в статический список
            return result;
        }
    }
}

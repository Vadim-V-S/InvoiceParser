using PdfParser.Extentions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.BL.TextExtractors
{
    // назначение платежа
    public class PaymentExtractor : TextExtractor
    {
        List<string> startSliceWords;
        public PaymentExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Payment();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();
            startSliceWords = new List<string>()
            {
                "оплата",
                "сумма",
                "цена"
            };

            comparator = new Comparator(new Payment());
        }

        internal override List<string> ExtractText(List<string> keyWords)
        {
            var extraction = parsedData.SliceListByTwoWords(startSliceWords, endSliceWords);
            extraction.RemoveTheOnlyWordElementFromList(); // стандартно удаляем элементы с одним словом
            extraction.RemoveElementsFromListByWords(exclusions); //  удаляем по справочнику исключений

            return extraction;
        }

        internal override string GetResultByIndex(IReferenceData referenceData, ComparatorIndexDelegate indexHandler, List<string> extractedText)
        {
            var analyzer = new Analyzer(extractedText, indexHandler);

            var result = analyzer.SearchTextByIndexValue(referenceData);

            if (result.Trim() != "")
                return result;
            else
                return "Нет данных!";
        }

        // Назначений платежа может быть несколько поэтому получаем их в цикле
        public override string GetResultValue()
        {
            var result = string.Empty;
            var extractedText = ExtractText(keyWords);
            var tempList = new List<string>(extractedText);
            foreach (var item in extractedText)
            {
                var paymentItem = GetResultByIndex(new Payment(), comparator.GetIndexByPartialRatio, tempList);
                tempList.RemoveElementsFromListByWords(new List<string>() { item }); // при каждой итерации удаляем, то что уже нашли

                if (paymentItem.Length > 10)
                    //result += item + "\n";
                    result += paymentItem + "\n";
            }
            return result;
        }
    }
}

using PdfParser.Extensions;
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
                "ОПЛАТА",
                "СУММА",
                "ЦЕНА",
                "ТОВАРЫ"
            };
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var extraction = parsedData.SliceListUpToWords(endSliceWords);
            extraction = extraction.SliceFollowingOfWords(startSliceWords);
            if (extraction.DoesListContainWord(endSliceWords))
            {
                extraction = extraction.SliceListUpToWords(endSliceWords);
            }
            extraction.RemoveElementsFromListByWords(exclusions); //  удаляем по справочнику исключений

            return extraction.RemoveTheOnlyWordElementFromList(); // стандартно удаляем элементы с одним словом
        }

        public override string GetResultValue()
        {
            var result = string.Empty;
            var extraction = ExtractData(keyWords);

            foreach (var item in extraction)
            {
                result += item + "\n";
            }
            return result;
        }
    }
}

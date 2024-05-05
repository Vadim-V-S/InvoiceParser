using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // валюта
    public class CurrencyExtractor : DataExtractor, ITextExtractor
    {
        public CurrencyExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Currency();
            keyWords = referenceData.GetKeyTokens();

            comparator = new Comparator(new Currency());
        }

        internal virtual List<string> ExtractData(List<string> keyWords)
        {

            var result = parsedData.CreateListByKeyTokens(keyWords);

            return result;
        }

        public override string GetResultValue()
        {
            var extraction = ClearResult(ExtractData(keyWords));

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new Currency(), comparator.GetIndexByPartialRatio, keyWords);
                result = result.RemoveAllStringBesidesKeyWord(keyWords); // удаляем все элементы списка за исключением справочных
            }
            return result;
        }
    }
}

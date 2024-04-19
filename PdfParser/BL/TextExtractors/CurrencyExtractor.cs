using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // валюта
    public class CurrencyExtractor:TextExtractor, ITextExtractor
    {
        public CurrencyExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Currency();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new Currency());
        }

        internal virtual List<string> ExtractData(List<string> keyWords)
        {
            var extraction = parsedData.CreateListByKeyWords(keyWords);

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = GetResultByIndex(extraction, new Currency(), comparator.GetIndexByPartialRatio, keyWords);
            result = result.RemoveAllStringBesidesKeyWord(keyWords); // удаляем все элементы списка за исключением справочных

            return result;
        }
    }
}

using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyName;

namespace PdfParser.BL.TextExtractors
{
    // Плательщик платежа
    public class PayerNameExtractor : DataExtractor
    {
        public PayerNameExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerName();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new PayerName());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListByTwoWords(usedTokens[token.recipientName], endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords.Union(referenceData.GetReferenceWords()).ToList());
            extraction = extraction.RemoveElementsFromListByExclusions(exclusions);

            if (usedTokens[token.recipientName] != "Нет данных!")
            {
                extraction = analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords.Union(referenceData.GetReferenceWords()).ToList());
                return extraction.RemoveElementsFromListByToken(usedTokens[token.recipientName]); // все тоже самое как и получателя, только дополнительно удаляем уже выбранного получателя платежа из списка
            }

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByExtraction(extraction, new PayerName(), comparator.ExtractOne, comparator.GetIndexByTokenRatio, keyWords);
            }
            usedTokens[token.payerName] = result.Replace(" - Уровень доверия низкий!", "").Trim();  // запоминаем наш выбор в статическом списке

            if (result.Contains(":"))
            {
                return result.Split(":")[1];
            }
            return result;
        }
    }
}

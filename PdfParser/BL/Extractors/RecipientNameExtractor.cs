using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyName;

namespace PdfParser.BL.TextExtractors
{
    // Получатель платежа
    public class RecipientNameExtractor : DataExtractor
    {
        public RecipientNameExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientName();
            keyWords = referenceData.GetKeyTokens();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new RecipientName());
        }
        internal override List<string> ExtractData(List<string> keyWords)
        {
            var index = parsedData.IndexOf("-recipient-");

            if (index == -1)
            {
                var slice = parsedData.CutOffFooter(paymentHeaderTokens);
                slice = slice.GetRange(0, slice.Count / 2);

                var extraction = slice.CreateListByKeyTokens(keyWords); // выборка по ключевым словам
                extraction = extraction.RemoveElementsFromListByExclusions(exclusions);      // удаление лишнего по словам исключениям

                keyWords.Add(":");
                keyWords.Add("\"");

                return analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords.Union(referenceData.GetReferenceTokens()).ToList());
            }

            return new List<string>() { parsedData[index + 1] };
        }

        public override string GetResultValue()
        {
            var extraction = ClearResult(ExtractData(keyWords));

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new RecipientName(), comparator.GetIndexByTokenRatio, keyWords);
            }
            var valueToSave = result.GetTextFromQuotes();
            usedTokens[token.recipientName] = valueToSave.Replace(" - Уровень доверия низкий!", "").Trim();  // запоминаем наш выбор в статическом списке

            var index = result.IndexOf(":");
            if (index > 0)
            {
                return result.Substring(index);
            }

            return result;
        }
    }
}

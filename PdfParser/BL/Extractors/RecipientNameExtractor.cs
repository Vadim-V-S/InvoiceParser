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
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new RecipientName());
        }
        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.CutOffFooter(paymentHeaderTokens);
            //slice = slice.CutOffTop(keyWords);
            slice = slice.GetRange(0, slice.Count / 2);

            var extraction = slice.CreateListByKeyTokens(keyWords); // выборка по ключевым словам
            extraction = extraction.RemoveElementsFromListByExclusions(exclusions);      // удаление лишнего по словам исключениям
            
            keyWords.Add(":");
            keyWords.Add("\"");
            var result = analyzer.ReturnElementsByHeaviestWeights(extraction, keyWords.Union(referenceData.GetReferenceWords()).ToList());

            return result;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

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

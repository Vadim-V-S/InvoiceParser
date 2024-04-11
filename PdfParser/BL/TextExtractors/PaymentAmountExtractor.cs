using PdfParser.Extentions;
using PdfParser.ReferenceData;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace PdfParser.BL.TextExtractors
{
    // сумма платежа
    public class PaymentAmountExtractor : TextExtractor
    {
        public PaymentAmountExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PaymentAmount();
            keyWords = referenceData.GetKeyWords(); //инициализируем список своих ключевых слов
        }

        internal List<string> ExtractNumbers(List<string> keyWords)
        {
            string paymentText = string.Empty;

            string allText = String.Join(",", parsedData).ToLower(); // список превращаем в строку

            foreach (var word in keyWords)
            {
                var keyWordIndex = allText.IndexOf(word); //получаем индекс в строке слова всего, оплате и т.д. из справочника
                if (keyWordIndex > 0)
                {
                    paymentText = allText.Substring(keyWordIndex); // удаляем все что было до ключевого слова, теперь в тексте нет числовых значений от инн, кпп и т.п.
                    paymentText = paymentText.RemoveDatesFromString(); // удаляем все даты
                    paymentText = paymentText.Replace(',', '.'); // заменяем все запятые на точки
                    paymentText = paymentText.Replace(" ", ""); // удаляем пробелы

                    var allNumbers = paymentText.GetNumbersFromString(); // получаем все числа
                    var amounts = allNumbers.Where(n => n.Contains('.')); // оставляем только те которые содержат точку
                    var result = amounts.Where(n => n.Substring(n.IndexOf('.')).Length == 3).ToList(); // оставляем только те которые содержат 2 знака после точки
                    
                    return result;
                }
            }
            return new List<string>();
        }

        public override string GetResultValue()
        {
            Analyzer analyzer = new Analyzer();
            return analyzer.GetMaxAmount(ExtractNumbers(keyWords)); // кидаем в анализатор  для получения макс значения
        }
    }
}

﻿using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // сумма платежа
    public class PaymentAmountExtractor : DataExtractor
    {
        public PaymentAmountExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PaymentAmount();
            keyWords = referenceData.GetKeyTokens(); //инициализируем список своих ключевых слов
        }

        internal List<string> ExtractNumbers(List<string> keyWords)
        {
            var amountIndex = parsedData.IndexOf("-amount-");
            var paymentIndex = parsedData.IndexOf("-payment-");

            var slice = parsedData;

            if (amountIndex != -1)
            {
                slice = parsedData.GetRange(amountIndex, parsedData.Count - amountIndex);
            }
            else if (paymentIndex != -1)
            {
                slice = parsedData.GetRange(paymentIndex, parsedData.Count - paymentIndex);
            }

            string paymentText = string.Empty;

            string allText = String.Join(",", slice); // список превращаем в строку

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

                    return ClearResult(result);
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

using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.BL
{
    public delegate int ComparatorIndexDelegate(string text); // делегаты ссылка на анализ по индексу
    public delegate string ComparatorExtractionDelegate(string text, List<string> values); // делегаты ссылка на анализ по извлечению
    public class Analyzer
    {
        public List<string> parsedData { private get; set; }
        public ComparatorIndexDelegate IndexHandler { get; set; }
        public ComparatorExtractionDelegate ExtractionHandler { get; set; }

        // 3 конструктора с разной сигнатурой для разных экстракторов
        public Analyzer() { }
        public Analyzer(List<string> parsedData, ComparatorIndexDelegate indexHandler)
        {
            this.parsedData = parsedData;
            IndexHandler = indexHandler;
        }

        public Analyzer(List<string> parsedData, ComparatorExtractionDelegate extractionHandler)
        {
            this.parsedData = parsedData;
            ExtractionHandler = extractionHandler;
        }


        public string GetMaxAmount(List<string> numbers)
        {
            List<double> amounts = new List<double>();

            foreach (var number in numbers)
            {
                if (double.TryParse(number, out double resultAmount))
                {
                    amounts.Add(resultAmount);
                }
            }

            if (amounts.Count != 0)
            {
                var maxValue = amounts.Max();
                return string.Format("{0:f2}", maxValue);
            }
            else
            {
                return "Нет данных!";
            }
        }

        public List<string> ExtractTextByValue(IReferenceData targetField)
        {
            List<string> values = new List<string>();
            foreach (var value in parsedData)
            {
                var compareItem = new Comparator(targetField);
                string extractedValue = ExtractionHandler.Invoke(value, parsedData); // вызов  метода извлечения через делегат

                try
                {
                    values.Add(extractedValue);
                }
                catch
                {
                    //Console.WriteLine($"Уже существует {value}");
                }
            }
            return values;
        }

        public string SearchTextByIndexValue(IReferenceData targetField)
        {
            Dictionary<string, int> valuesByWeghts = new Dictionary<string, int>();

            string result = string.Empty;
            foreach (var value in parsedData)
            {
                var compareItem = new Comparator(targetField);
                int indexByTokenRatio = IndexHandler.Invoke(value); // вызов  метода анализа по индексу через делегат (всего в компараторе 3 метода анализа по индексу, для каждого случая используется свой)

                try
                {
                    valuesByWeghts.Add(value, indexByTokenRatio);
                }
                catch
                {
                    //Console.WriteLine($"Уже существует {value}");
                }
            }

            int maxWeight = 0;
            string resultValue = string.Empty;
            if (valuesByWeghts.Count != 0)
            {
                maxWeight = valuesByWeghts.Values.Max();

                resultValue = valuesByWeghts.MaxBy(entry => entry.Value).Key;
            }


            if (maxWeight < 25 && maxWeight >= 10)
            {
                if (resultValue != "Нет данных!")
                    result = resultValue + " - Уровень доверия низкий!";
                //result = resultValue;
            }
            else if (maxWeight < 10)
            {
                result = "Нет данных!";
                //result = "";
            }
            else
            {
                result = valuesByWeghts.MaxBy(entry => entry.Value).Key;
            }

            return result;
        }
    }
}

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

            return "Нет данных!";
        }

        public List<string> ExtractTextByValue(ReferenceData.Interfaces.IReferenceData targetField)
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

        public string SearchTextByIndexValue(ReferenceData.Interfaces.IReferenceData targetField)
        {
            Dictionary<string, int> valuesByWeights = new Dictionary<string, int>();

            string result = string.Empty;
            foreach (var value in parsedData)
            {
                var compareItem = new Comparator(targetField);
                int indexByTokenRatio = IndexHandler.Invoke(value); // вызов  метода анализа по индексу через делегат (всего в компараторе 3 метода анализа по индексу, для каждого случая используется свой)

                try
                {
                    valuesByWeights.Add(value, indexByTokenRatio);
                }
                catch
                {
                    //Console.WriteLine($"Уже существует {value}");
                }
            }

            int maxWeight = 0;
            string resultValue = string.Empty;
            if (valuesByWeights.Count != 0)
            {
                maxWeight = valuesByWeights.Values.Max();

                resultValue = valuesByWeights.MaxBy(entry => entry.Value).Key;
            }


            if (maxWeight < 25)
            {
                if (resultValue != "Нет данных!" && resultValue.Trim() != "")
                    return resultValue + " - Уровень доверия низкий!";
            }

            return resultValue;
        }


        public List<string> ReturnElementsByHeaviestWeights(List<string> extraction, List<string> keyWords)
        {
            List<string> result = new List<string>();
            var weghtsMatrix = SetWeightsByKeyWords(extraction, keyWords);

            if (weghtsMatrix.Count > 0)
            {
                result = GetHeaviestElements(weghtsMatrix);
            }

            return result;
        }

        public List<string> ReturnElementsIgnoringWeakOnes(List<string> extraction, List<string> keyWords)
        {
            List<string> result = new List<string>();
            var weghtsMatrix = SetWeightsByKeyWords(extraction, keyWords);

            if (weghtsMatrix.Count > 0)
            {
                result = RemoveWeakElements(weghtsMatrix);
            }

            return result;
        }

        public Dictionary<string, int> SetWeightsByKeyWords(List<string> allText, IEnumerable<string> keyWords)
        {
            Dictionary<string, int> weightsMatrix = new Dictionary<string, int>();

            foreach (var line in allText)
            {
                int weight = 0;
                foreach (var keyWord in keyWords)
                {
                    if (line.Contains(keyWord))
                    {
                        var i = 0;
                        var count = 0;
                        while ((i = line.IndexOf(keyWord, i)) != -1) { ++count; i += keyWord.Length; }
                        weight += count;
                    }
                }
                weightsMatrix.Add(line, weight);
            }

            return weightsMatrix;
        }

        public List<string> GetHeaviestElements(Dictionary<string, int> weightsMatrix)
        {
            var result = new List<string>();
            int maxWeight = weightsMatrix.Values.Max();

            foreach (var keyvaluepair in weightsMatrix)
            {
                if (keyvaluepair.Value == maxWeight)
                {
                    result.Add(keyvaluepair.Key);
                }
            }

            return result;
        }

        public List<string> RemoveWeakElements(Dictionary<string, int> weightsMatrix)
        {
            var result = new List<string>();

            foreach (var keyvaluepair in weightsMatrix)
            {
                if (keyvaluepair.Value > 1)
                {
                    result.Add(keyvaluepair.Key);
                }
            }

            return result;
        }
    }
}

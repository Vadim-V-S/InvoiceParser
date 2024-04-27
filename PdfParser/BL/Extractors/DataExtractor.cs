using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // родительский класс с базовой реализацией
    public class DataExtractor : ITextExtractor
    {
        internal ReferenceData.Interfaces.IReferenceData referenceData;
        internal Comparator comparator;
        internal ComparatorIndexDelegate indexHandler; // объект делагата (ссылки) на метод анализа Comparator по индексу
        internal ComparatorExtractionDelegate extractionHandler; // объект делагата (ссылки) на метод анализа Comparator по извлечению
        internal List<string> parsedData = new List<string>();
        internal List<string> keyWords; // справочник ключевых слов для выборки из распасенных данных
        internal List<string> exclusions; // справочник ключевых слов для исключения элементов списка из выборки
        internal List<string> invoiceFooterTokens = new List<string>();
        internal List<string> paymentHeaderTokens = new List<string>();
        internal static Dictionary<string, string> usedTokens = new Dictionary<string, string>(); // список для уже использованных выбранных значений. Используется только для получения наименований компаний
        internal Token token = new Token();
        internal Analyzer analyzer;

        public DataExtractor(List<string> parsedData) //инициализация
        {
            this.parsedData = parsedData;
            keyWords = new List<string>();

            invoiceFooterTokens = new List<string>()
            {
                "НАЗНАЧЕНИЕ",
                "ВСЕГО",
                "К ОПЛАТЕ"
                //"ИТОГО"
            };

            paymentHeaderTokens = new List<string>()
            {
                "ОПЛАТА",
                "СУММА",
                "ЦЕНА",
                "ТОВАРЫ"
            };

            analyzer = new Analyzer();
        }

        internal string GetLastUsedToken()
        {
            var usedData = usedTokens.Values.Where(v => v != "Нет Данных!").ToList();
            if (usedData.Count != 0)
                return usedData[usedData.Count - 1];
            else
                return string.Empty;
        }

        //Выборка данных по извлечению
        internal string GetResultByExtraction(List<string> extraction, ReferenceData.Interfaces.IReferenceData referenceData, ComparatorExtractionDelegate extractionDelegate, ComparatorIndexDelegate indexHandler, List<string> words)
        {
            analyzer = new Analyzer(extraction, extractionDelegate);
            var resultWords = analyzer.ExtractTextByValue(referenceData);

            analyzer = new Analyzer(resultWords, indexHandler);
            var result = analyzer.SearchTextByIndexValue(referenceData);

            if (result.Trim() != "")
                return result;
            else
                return "Нет данных!";
        }

        //Выборка данных по анализу индекса
        internal virtual string GetResultByIndex(List<string> extraction, ReferenceData.Interfaces.IReferenceData referenceData, ComparatorIndexDelegate indexHandler, List<string> words)
        {
            var analyzer = new Analyzer(extraction, indexHandler);

            var result = analyzer.SearchTextByIndexValue(referenceData).Replace(",", "").Trim();

            if (result != "")
            {
                return result;
            }

            return "Нет данных!";
        }

        //Первоначальная выборка данных из парсинга по ключевым словам
        internal virtual List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.CutOffFooter(invoiceFooterTokens);
            var extraction = slice.CreateListByKeyTokens(keyWords);

            return extraction;
        }

        public virtual string GetResultValue() //Стартовый основной метод получения готового результата
        {
            var extraction = ExtractData(keyWords);
            var result = GetResultByIndex(extraction, new Invoice(), comparator.GetIndexByPartialRatio, keyWords);

            return result;
        }
    }
}

using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extentions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.BL.TextExtractors
{
    // родительский класс с базовой реализацией
    public class TextExtractor : ITextExtractor
    {
        internal IReferenceData referenceData;
        internal Comparator comparator;
        internal ComparatorIndexDelegate indexHandler; // объект делагата (ссылки) на метод анализа Comparator по индексу
        internal ComparatorExtractionDelegate extractionHandler; // объект делагата (ссылки) на метод анализа Comparator по извлечению
        internal List<string> parsedData = new List<string>();
        internal List<string> keyWords; // справочник ключевых слов для выборки из распасенных данных
        internal List<string> exclusions; // справочник ключевых слов для исключения элементов списка из выборки
        internal List<string> endSliceWords = new List<string>();
        internal static List<string> usedValues; // список для уже использованных выбранных значений. Используется только для получения наименований компаний

        public TextExtractor(List<string> parsedData) //инициализация
        {
            this.parsedData = parsedData;
            keyWords = new List<string>();

            usedValues = new List<string>();

            endSliceWords = new List<string>()
            {
                "назначение",
                "всего",
                "итого"
            };
        }

        //Выборка данных по извлечению
        internal string GetResultByExtraction(IReferenceData referenceData, ComparatorExtractionDelegate extractionDelegate, ComparatorIndexDelegate indexHandler, List<string> words)
        {
            var targetWords = ExtractText(words);

            var analyzer = new Analyzer(targetWords, extractionDelegate);
            var resultWords = analyzer.ExtractTextByValue(referenceData);

            analyzer = new Analyzer(resultWords, indexHandler);
            var result = analyzer.SearchTextByIndexValue(referenceData);

            if (result.Trim() != "")
                return result;
            else
                return "Нет данных!";
        }

        //Выборка данных по анализу индекса
        internal virtual string GetResultByIndex(IReferenceData referenceData, ComparatorIndexDelegate indexHandler, List<string> words)
        {
            var targetWords = ExtractText(words);

            var analyzer = new Analyzer(targetWords, indexHandler);

            var result = analyzer.SearchTextByIndexValue(referenceData);

            if (result.Trim() != "")
                return result;
            else
                return "Нет данных!";
        }

        //Первоначальная выборка данных из парсинга по ключевым словам
        internal virtual List<string> ExtractText(List<string> keyWords)
        {
            var slice = parsedData.SliceListBeforeWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            return extraction;
        }

        public virtual string GetResultValue() //Стартовый основной метод получения готового результата
        {
            return GetResultByIndex(new Invoice(), comparator.GetIndexByPartialRatio, keyWords);
        }
    }
}

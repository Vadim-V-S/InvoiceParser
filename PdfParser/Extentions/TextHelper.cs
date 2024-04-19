using System.Text.RegularExpressions;

namespace PdfParser.Extensions
{
    public static class TextHelper
    {
        // извлекаем следущее слово из строки от нужного. Это для инн
        public static string GetNextWordByReferenceText(this string allText, string referenceText)
        {
            string targetValue = string.Empty;

            int startIndex = allText.IndexOf(referenceText);

            if (startIndex >= 0)
            {
                string temp = allText.Substring(startIndex + referenceText.Length).Trim();
                string[] parts = temp.Split(' ');
                return targetValue = parts[0];
            }

            if (targetValue != "")
            {
                return targetValue;
            }

            return "Нет данных!";
        }

        // удаляем элементы списка по словам исключениям
        public static List<string> RemoveElementsFromListByWords(this List<string> allText, IEnumerable<string> keyWords)
        {
            foreach (string word in keyWords)
            {
                var item = allText.FirstOrDefault(v => v.Replace(" ", "").Contains(word.Replace(" ", "")));

                if (item != null)
                    allText.Remove(item);
            }

            return allText;
        }

        // извлекаем список только содержащий ключевые значения
        public static List<string> CreateListByKeyWords(this IEnumerable<string> allText, IEnumerable<string> keyWords)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string line in allText)
            {
                foreach (var word in keyWords)
                    if (line.Contains(word))
                    {
                        result.Add(line);
                    }
            }
            return result.ToList();
        }

        // удаляем элементы списка состоящие из одного слова
        public static List<string> RemoveTheOnlyWordElementFromList(this IEnumerable<string> allText)
        {
            var result = new List<string>();
            foreach (string line in allText)
            {
                if (line.Trim().Split(' ').Length != 1)
                {
                    result.Add(line.Trim());
                }
            }

            return result;
        }

        // удаляем элементы списка содержащие слова
        public static List<string> RemoveElementsFromListByToken(this IEnumerable<string> allText, string token)
        {
            var result = new List<string>();
            foreach (var line in allText)
            {
                //if (!line.Replace(" ","").Replace(",","").Replace(".","").Contains(token.Replace(" ", "").Replace(",", "").Replace(".", "")))
                if (!line.Trim().Contains(token) && !token.Contains(line.Trim()))
                {
                    result.Add(line);
                }
            }

            return result;
        }
     

        // это срезы
        public static List<string> SliceListUpToWords(this List<string> allText, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(word)));
                if (endIndex > 0)
                {
                    return allText.GetRange(0, endIndex);
                }
            }

            return new List<string>() { "Нет данных!" };
        }

        public static List<string> SliceFollowingOfWords(this List<string> allText, List<string> words)
        {
            var endIndex = GetMinIndex(allText, words, 0) + 1;
            if (endIndex > 0)
            {
                return allText.GetRange(endIndex, allText.Count - endIndex);
            }

            return new List<string>() { "Нет данных!" };
        }

        //public static List<string> SliceListByTwoWords(this List<string> allText, List<string> startWords, List<string> endWords)
        //{
        //    var startIndex = GetMinIndex(allText, startWords, 0) + 1;
        //    var endIndex = GetMinIndex(allText, endWords, startIndex) - 1;

        //    if (endIndex > 0 && startIndex > 0 && endIndex > startIndex)
        //    {
        //        return allText.GetRange(startIndex, endIndex - startIndex);
        //    }

        //    return new List<string>() { "Нет данных!" };
        //}

        private static int GetMinIndex(List<string> allText, List<string> words, int refIndex)
        {
            int index = allText.Count - 1;

            foreach (var word in words)
            {
                var currentIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(word)));

                if (currentIndex < index && currentIndex >= 0 && currentIndex > refIndex)
                {
                    index = currentIndex;
                }
            }
            return index;
        }

        public static List<string> SliceListByTwoWords(this List<string> allText, string token, IEnumerable<string> Endwords)
        {
            var startIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(token)));
            foreach (var endWord in Endwords)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(endWord)));
                if (endIndex > 0 && startIndex >= 0 && endIndex - startIndex > 4)
                {
                    return allText.GetRange(startIndex, endIndex - startIndex);
                }
            }

            return new List<string>() { "Нет данных!" };
        }

        public static string GetTextFromQuotes(this string text)
        {
            int count = 0;
            foreach(var chr in text)
            {
                if(chr == '"') count++;
            }

            if (count == 2)
            {
                var item = text.Split('"');
                return item[1].Trim();
            }

            return text;
        }

        public static bool DoesListContainWord(this List<string> allText, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (allText.Any(i => i.Contains(word)))
                    return true;
            }
            return false;
        }

        // удаляем все элементы строки за исключением нужных
        public static string RemoveAllStringBesidesKeyWord(this string line, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var index = line.IndexOf(word);
                if (index != -1)
                    return line.Substring(index, word.Length);
            }

            return line;
        }

        public static string RemoveDatesFromString(this string text)
        {
            string result = string.Empty;
            var items = text.Split(" ");

            foreach (var item in items)
            {
                if (item.Where(d => d == '.').Count() != 2)
                {
                    result += item + " ";
                }
            }

            return result;
        }

        public static string GetNumberFromStringByLength(this string text, int len)
        {
            var numbers = new List<string>();

            text = text.Replace('/', ' ').Replace(',', ' ').Replace('.', ' ');
            var list = text.Split(' ');

            foreach (var item in list)
            {
                if (Regex.IsMatch(item, @"^\d+$"))
                {
                    numbers.Add(item);
                }
            }

            if (numbers.Count > 0)
                return numbers.FirstOrDefault(i => i.Length >= len);

            return text;
        }

        public static List<string> GetNumbersFromString(this string text)
        {
            List<string> amounts = new List<string>();

            string pattern = @"([\d]+[. ][\d]+[, ]*[\d]+)|([\d]+[,.][\d]+)|([\d]+[, ][\d]+[.][\d]+)";
            Regex regex = new Regex(pattern);
            var nums = regex.Matches(text);

            foreach (Match match in nums)
            {
                amounts.Add(match.Value.ToString());
            }

            return amounts;
        }

        // является ли строка числовым значением
        public static bool IsStringDigits(this string text)
        {
            return text.All(char.IsDigit);
        }

        // выбрать строки содержащие числовые значениея
        public static List<string> RemoveListElementsWithoutDigits(this List<string> text)
        {
            var result = new List<string>();

            foreach (var line in text)
            {
                if (line.Any(char.IsDigit))
                {
                    result.Add(line);
                }
            }

            return result;
        }


        // выбираем наиболее близкий текст (адрес) к требуемому.
        public static List<string> GetClosestElementToWord(this List<string> allText, string WordToCompare, IEnumerable<string> refWords)
        {
            var result = new List<string>();
            var refValue = 0;
            var value = GetElementIndexFromListByPartialMatch(allText, WordToCompare);

            foreach (var word in refWords)
            {
                refValue = GetElementIndexFromListByPartialMatch(allText, word);

                var dif = refValue - value;

                if (dif <= 2 && dif > 0)
                {
                    result.Add(word.Trim());
                }
            }
            if (result.Count > 0)
            {
                return result;
            }

            return result;
        }


        // это помощник возвращает индекс строки по частичному совпадению
        private static int GetElementIndexFromListByPartialMatch(this List<string> allText, string text)
        {
            for (int i = 0; i < allText.Count; i++)
            {
                if (allText[i].Contains(text))
                {
                    return i;
                };
            }

            return -1;
        }
    }
}

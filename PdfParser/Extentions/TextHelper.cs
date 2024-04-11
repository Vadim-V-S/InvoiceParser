using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PdfParser.Extentions
{
    public static class TextHelper
    {
        // текст в список
        public static List<string> GetTextList(this string allText)
        {
            string[] allTextArray;
            List<string> parsedValues = new List<string>();

            allText = Regex.Replace(allText, " {2,}", " ");

            allTextArray = allText.Split("\n");

            foreach (string text in allTextArray)
            {
                if (text.Trim() != "")
                {
                    parsedValues.Add(text);
                }
            };
            return parsedValues;
        }

        public static string RemoveAllUnreadableChars(this string text)
        {
            List<char> chars = new List<char>()
            {
                //'!','@','#','$','%','^','&','*','(',')','_','+','=','-','\'','\\',':','|','/','`','~','.','{','}'
                '@','#','^','&','*','_','+','=','-','\'','\\','|','`','~','{','}'
            };
            var result = text;
            result = new string(result.Where(c => !chars.Contains(c)).ToArray());
            //foreach (var chr in chars)
            //{
            //    result= text.Replace(chr, ' ');
            //}
            return result;
        }

        // извлекаем следущее слово из строки от нужного. Это для инн
        public static string ExtractNextWordByReferenceText(this string allText, string referenceText)
        {
            string targetValue = string.Empty;

            int startIndex = allText.ToLower().IndexOf(referenceText);

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
            else
            {
                return "Нет данных!";
            }
        }

        // удаляем элементы списка по словам исключениям
        public static List<string> RemoveElementsFromListByWords(this List<string> allText, IEnumerable<string> keyWords)
        {
            foreach (string word in keyWords)
            {
                var item = allText.FirstOrDefault(v => v.ToLower().Replace(" ", "").Contains(word.ToLower().Replace(" ", "")));

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
                    if (line.ToLower().Contains(word))
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
                if (line.Split(' ').Length != 1)
                {
                    result.Add(line);
                }

            }
            return result;
        }

        // это срезы
        public static List<string> SliceListBeforeWords(this List<string> allText, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.ToLower().Contains(word.ToLower())));
                if (endIndex > 0)
                {
                    return allText.GetRange(0, endIndex);
                }
            }
            return new List<string>() { "Нет данных!" };
        }

        public static List<string> SliceListByTwoWords(this List<string> allText, IEnumerable<string> startWords, IEnumerable<string> Endwords)
        {
            foreach (var startWord in startWords)
            {
                var startIndex = allText.IndexOf(allText.FirstOrDefault(v => v.ToLower().Contains(startWord.ToLower())));
                foreach (var endWord in Endwords)
                {
                    var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.ToLower().Contains(endWord.ToLower())));
                    if (endIndex > 0 && startIndex > 0 && endIndex > startIndex)
                    {
                        return allText.GetRange(startIndex, endIndex - startIndex);
                    }
                }
            }
            return new List<string>() { "Нет данных!" };
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
                return numbers.FirstOrDefault(i => i.Length == len);

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

        // выбираем наиболее близкий текст (адрес) к требуемому.
        public static List<string> GetClosestElementToWord(this List<string> allText, string WordToCompare, IEnumerable<string> refWords)
        {
            var result = new List<string>();
            var refValue = 0;
            var value = GetElementIndexFromListByPartialMatch(allText, WordToCompare);

            foreach (var word in refWords)
            {
                refValue = GetElementIndexFromListByPartialMatch(allText, word);

                //var dif = Math.Abs(refValue - value);
                var dif = refValue - value;

                if (dif <= 2 && dif > 0)
                {
                    result.Add(word);
                }
            }
            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                result.Add("");
            }
            return result;
        }

        // это помощник возвращает индекс строки по частичному совпадению
        private static int GetElementIndexFromListByPartialMatch(this List<string> allText, string text)
        {
            for (int i = 0; i < allText.Count; i++)
            {
                if (allText[i].ToLower().Contains(text.ToLower()))
                {
                    return i;
                };
            }
            return -1;
        }
    }
}

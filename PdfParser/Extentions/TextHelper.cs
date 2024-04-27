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
        public static List<string> RemoveElementsFromListByExclusions(this List<string> allText, IEnumerable<string> exclusions)
        {
            List<string> result = new List<string>();

            bool findings = false;
            foreach (var line in allText)
            {
                foreach (var exclusion in exclusions)
                {
                    if (line.Contains(exclusion))
                    {
                        findings = true;
                    }
                }

                if (!findings)
                    result.Add(line);

                findings= false;
            }

            return result;
        }

        // извлекаем список только содержащий ключевые значения
        public static List<string> CreateListByKeyTokens(this IEnumerable<string> allText, IEnumerable<string> keyTokens)
        {
            HashSet<string> result = new HashSet<string>();
            foreach (string line in allText)
            {
                foreach (var token in keyTokens)
                    if (line.Contains(token))
                    {
                        result.Add(line);
                    }
            }
            return result.ToList();
        }

        // удаляем элементы списка содержащие слова
        public static List<string> RemoveElementsFromListByToken(this IEnumerable<string> allText, string token)
        {
            var result = new List<string>();
            token = token.Replace(" ", "").Replace(",", "").Replace(".", "");

            foreach (var line in allText)
            {
                var currentLine = line.Replace(" ", "").Replace(",", "").Replace(".", "");
                if (!currentLine.Trim().Contains(token) && !token.Contains(currentLine.Trim()))
                {
                    result.Add(line);
                }
            }

            return result;
        }

        public static List<string> CutOffFooter(this List<string> allText, IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(token)));
                if (endIndex > 0)
                {
                    return allText.GetRange(0, endIndex);
                }
            }

            return allText;
        }

        public static List<string> CutOffTopInRecursion(this List<string> allText, IEnumerable<string> tokens)
        {
            var result = new List<string>();
            result = allText;

            foreach (var token in tokens)
            {
                var endIndex = result.IndexOf(result.FirstOrDefault(v => v.Contains(token)));
                if (endIndex > 0)
                {
                    result = result.GetRange(0, endIndex).CutOffTopInRecursion(tokens);
                }
            }

            return result;
        }

        public static List<string> CutOffTop(this List<string> allText, List<string> tokens)
        {
            var endIndex = GetMinIndex(allText, tokens, 0) + 1;
            if (endIndex > 0)
            {
                return allText.GetRange(endIndex, allText.Count - endIndex);
            }

            return new List<string>() { "Нет данных!" };
        }

        public static List<string> GetConsistentData(this List<string> allText, List<string> extractedData)
        {
            var result = new List<string>();
            bool flag = false;
            var firstIndex = 0;
            var secondIndex = 0;
            foreach (var line in extractedData)
            {
                if (!flag)
                {
                    result.Add(line);
                    firstIndex = allText.IndexOf(line);
                }
                else
                {
                    secondIndex = allText.IndexOf(line);

                    if (flag && secondIndex == firstIndex + 1)
                    {
                        firstIndex = allText.IndexOf(line);
                        result.Add(line);
                    }
                    else
                    {
                        return result;
                    }
                }
                flag = true;
            }

            return extractedData;
        }

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

        public static List<string> SliceListBetweenTwoTokens(this List<string> allText, string token, IEnumerable<string> footerTokens)
        {
            var startIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Replace(",", "").Contains(token)));
            foreach (var footerToken in footerTokens)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(footerToken)));
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
            foreach (var chr in text)
            {
                if (chr == '"') count++;
            }

            if (count == 2)
            {
                var item = text.Split('"');
                return item[1].Trim();
            }

            return text;
        }

        // удаляем все элементы строки за исключением нужных
        public static string RemoveAllStringBesidesKeyWord(this string line, IEnumerable<string> keyWords)
        {
            foreach (var word in keyWords)
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
                if (IsDigitInString(line))
                {
                    result.Add(line);
                }
            }

            return result;
        }


        public static bool DoesListContainsDigits(this List<string> text)
        {
            foreach(var line in text)
            {
                if (IsDigitInString(line))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool IsDigitInString(this string text)
        {
            if(text.Any(char.IsDigit))
                return true;
        
            return false;
        }


        // удаляем элементы списка состоящие из одного слова
        //public static List<string> RemoveTheOnlyWordElementFromList(this IEnumerable<string> allText)
        //{
        //    var result = new List<string>();
        //    foreach (string line in allText)
        //    {
        //        if (line.Trim().Split(' ').Length != 1)
        //        {
        //            result.Add(line.Trim());
        //        }
        //    }

        //    return result;
        //}

        // это срезы
        //public static List<string> SliceListUpToWords(this List<string> allText, IEnumerable<string> words)
        //{
        //    foreach (var word in words)
        //    {
        //        var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(word)));
        //        if (endIndex > 0)
        //        {
        //            return allText.GetRange(0, endIndex);
        //        }
        //    }

        //    return new List<string>() { "Нет данных!" };
        //}

        //public static List<string> SliceFollowingOfWord(this List<string> allText, string word)
        //{
        //    var line = allText.FirstOrDefault(l => l.Contains(word));
        //    var index = allText.IndexOf(line);

        //    return allText.GetRange(index,allText.Count-index);
        //}

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

        //public static bool DoesListContainWord(this List<string> allText, IEnumerable<string> words)
        //{
        //    foreach (var word in words)
        //    {
        //        if (allText.Any(i => i.Contains(word)))
        //            return true;
        //    }
        //    return false;
        //}

        //// выбираем наиболее близкий текст (адрес) к требуемому.
        //public static List<string> GetClosestElementToWord(this List<string> allText, string WordToCompare, IEnumerable<string> refWords)
        //{
        //    var result = new List<string>();
        //    var refValue = 0;
        //    var value = GetElementIndexFromListByPartialMatch(allText, WordToCompare);

        //    foreach (var word in refWords)
        //    {
        //        refValue = GetElementIndexFromListByPartialMatch(allText, word);

        //        var dif = refValue - value;

        //        if (dif <= 2 && dif > 0)
        //        {
        //            result.Add(word.Trim());
        //        }
        //    }
        //    if (result.Count > 0)
        //    {
        //        return result;
        //    }

        //    return result;
        //}

        //public static string ReturnNextItemWhenContainsKeyWord(this List<string> allText, List<string> refWords, string keyWord)
        //{
        //    foreach (var refWord in refWords)
        //    {
        //        var index = allText.IndexOf(refWord);

        //        if (index > 0)
        //        {
        //            if (allText[index - 1].Contains(keyWord) || allText[index + 1].Contains(keyWord))
        //                return refWord;
        //        }
        //    }

        //    return string.Empty;
        //}


        // это помощник возвращает индекс строки по частичному совпадению
        //private static int GetElementIndexFromListByPartialMatch(this List<string> allText, string text)
        //{
        //    for (int i = 0; i < allText.Count; i++)
        //    {
        //        if (allText[i].Contains(text))
        //        {
        //            return i;
        //        };
        //    }

        //    return -1;
        //}
    }
}

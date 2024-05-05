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

            return "";
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

                findings = false;
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
                var lineIndex = result.IndexOf(result.FirstOrDefault(v => v.Contains(token)));
                if (lineIndex > 0)
                {
                    result = result.GetRange(lineIndex, allText.Count - 1- lineIndex).CutOffTopInRecursion(tokens);
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

        public static List<string> CutOffTop(this List<string> allText, string token)
        {
            var index = GetMaxIndex(allText, token, 0) + 1;

            if (index > 0)
            {
                return allText.GetRange(index, allText.Count - index);
            }

            return allText;
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

        private static int GetMaxIndex(List<string> allText, string word, int refIndex)
        {
            int index = allText.Count - 1;

            var i = 0;

            for (; i < allText.Count; i++)
            {

                if (allText[i].Contains(word))
                {
                    index = i;
                }
            }
            return index;
        }

        public static List<string> SliceListByTwoTokens(this List<string> allText, string token, IEnumerable<string> footerTokens)
        {
            var startIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Replace(",", "").Contains(token)));
            foreach (var footerToken in footerTokens)
            {
                var endIndex = allText.IndexOf(allText.FirstOrDefault(v => v.Contains(footerToken)));
                if (endIndex < 0)
                {
                    endIndex = allText.Count - 1;
                }

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


        public static bool DoesListContainDigits(this List<string> text)
        {
            foreach (var line in text)
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
            if (text.Any(char.IsDigit))
                return true;

            return false;
        }

        public static bool DoesLineContain(this string line, List<string> refData)
        {
            //line = line.Replace(" ", "");
            return refData.Any(y => line.Trim().Contains(y.Trim()));
        }
    }
}

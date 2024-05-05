using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.CompanyName;
using PdfParser.ReferenceData.Interfaces;
using System.Text.RegularExpressions;

namespace PdfParser.BL.Normalizator
{
    public class Normalizer
    {
        List<string> recipientExclusions;
        List<string> payerExclusions;
        IReferenceData recipientName = new RecipientName();
        IReferenceData payerName = new PayerName();

        public Normalizer()
        {

            recipientExclusions = recipientName.GetExclusions();
            payerExclusions = payerName.GetExclusions();
        }
        public List<string> NormalizeText(string parsedText)
        {
            var text = RemoveAllUnreadableChars(parsedText).ToUpper();
            text = NormalizeCharsFormat(text);
            text = NormalizeInvoiceAttributes(text);

            var parsedList = GetParsedList(text);
            parsedList = NormalizeKeyWords(parsedList);
            parsedList = NormolizeInterferingExclusions(parsedList);
            parsedList = UnionSeparatedCompanyNames(parsedList);
            parsedList = RemoveDoublicatesInList(parsedList);
            parsedList = RemoveColonsAsFirstChar(parsedList);
            var result = MapInvoice(parsedList);

            return result;
        }

        private string RemoveAllUnreadableChars(string text)
        {
            List<char> chars = new List<char>()
            {
                //'!','@','#','$','%','^','&','*','(',')','_','+','=','-','\'','\\',':','|','/','`','~','.','{','}'
                '@','#','^','&','*','_','+','=','\'','\\','|','`','~','{','}'
            };
            var result = new string(text.Where(c => !chars.Contains(c)).ToArray());

            return result;
        }

        private string NormalizeCharsFormat(string text)
        {
            var result = text.Replace("«", "\"").Replace("»", "\"").Replace("Ё", "Е");

            return result;
        }

        private string NormalizeInvoiceAttributes(string parsedText)
        {
            List<InvoiceAttribute> attributes = new List<InvoiceAttribute>
            {
                new InnAttribute(),
                new PropertyAttribute()
            };

            foreach (var attribute in attributes)
            {
                parsedText = new string(NormalizeAttributes(parsedText, attribute));
            }

            return parsedText;
        }


        private string NormalizeAttributes(string text, InvoiceAttribute attribute)
        {
            foreach (var refWord in attribute.RefWords)
            {
                attribute.TargetWord = refWord.Replace("\n", "").Trim();

                foreach (string word in attribute.GetTokens())
                {
                    var currentWord = word.ToUpper().Trim().Replace(" ", "");
                    if (currentWord == "ИНН/КПП")
                    {
                        var inn = text.GetNextWordByReferenceText(currentWord + " ");
                        var newInn = inn.Replace("/", ", КПП ");

                        if (inn.Length > 0)
                        {
                            text = text.Replace(inn, "ИНН " + newInn).Replace(currentWord, "");
                        }
                    }
                    text = new string(text.Replace(word, refWord));
                    text = new string(text.Replace(word.Trim() + ": ", refWord));
                }
            }

            return text;
        }

        // текст в список
        private List<string> GetParsedList(string parsedText)
        {
            string[] textArray;
            List<string> parsedList = new List<string>();

            textArray = parsedText.Split("\n");

            foreach (string text in textArray)
            {
                if (text.Trim() != "")
                {
                    parsedList.Add(text);
                }
            };

            return parsedList;
        }

        private List<string> NormalizeKeyWords(List<string> parsedList)
        {
            IReferenceData recipientName = new RecipientName();
            IReferenceData payerName = new PayerName();

            var normalizeRecipient = UnionSeparatedKeyWordsAndNextLine(parsedList, recipientName.GetKeyTokens());
            var normalizedText = UnionSeparatedKeyWordsAndNextLine(normalizeRecipient, payerName.GetKeyTokens());

            return normalizedText;
        }

        //объединение строки с одиночным ключевым словом со следующей строкой
        private List<string> UnionSeparatedKeyWordsAndNextLine(List<string> parsedList, IEnumerable<string> keyWords)
        {
            var result = new List<string>();

            for (var i = 0; i < parsedList.Count; i++)
            {
                bool flag = false;
                var currentLine = parsedList[i].Replace(":", "").Trim();

                foreach (var word in keyWords)
                {
                    if (currentLine.Contains(word.ToUpper()) && currentLine.Split(' ').Length == 1)
                    {
                        result.Add($"{currentLine}: {parsedList[i + 1]}");
                        i++;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    result.Add(parsedList[i]);
                }
            }

            return result;
        }

        private List<string> UnionSeparatedCompanyNames(List<string> parsedList)
        {
            List<string> result = new List<string>();

            IReferenceData companyName = new CompanyName();
            var refWords = companyName.GetReferenceTokens();
            refWords.Add("ОБЩЕСТВО");
            refWords.Add("ОГРАНИЧЕННОЙ");
            refWords.Add("ОТВЕТСТВЕННОСТЬЮ");
            refWords.Add("АКЦИОНЕРНОЕ");

            for (var j = 0; j < parsedList.Count - 1; j++)
            {
                var currentLine = parsedList[j];
                var nextLine = parsedList[j + 1];
                var i = 0;
                var v = 0;
                var currentLineQuotes = 0;
                var nextLineQuotes = 0;
                while ((i = currentLine.IndexOf("\"", i)) != -1) { ++currentLineQuotes; i += "\"".Length; }
                while ((v = nextLine.IndexOf("\"", v)) != -1) { ++nextLineQuotes; v += "\"".Length; }

                if (refWords.Any(w => currentLine.Split(' ').Any(l => l.Contains(w))) && currentLineQuotes < 2 && nextLineQuotes >= 2)
                {
                    result.Add($"{currentLine} {nextLine}");
                    j++;
                }
                else
                {
                    result.Add(currentLine);
                }
            }
            return result;
        }

        private List<string> NormolizeInterferingExclusions(List<string> parsedList)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < parsedList.Count; i++)
            {
                var currentLine = parsedList[i].Split(" ");

                var recipientKeyTokens = recipientExclusions.Where(x => !payerExclusions.Contains(x)).ToList();
                var payerKeyTokens = payerExclusions.Where(x => !recipientExclusions.Contains(x)).ToList();

                bool recipientCheck = recipientKeyTokens.Any(y => currentLine.Any(x => x.Contains(y)));
                bool payerCheck = payerKeyTokens.Any(y => currentLine.Any(x => x.Contains(y)));

                if (recipientCheck && payerCheck)
                {
                    try
                    {
                        result.Add($"{currentLine[0]}: {parsedList[i + 1]}");
                        result.Add($"{currentLine[1]}: {parsedList[i + 2]}");
                        i++;
                        i++;

                        for (var j = i + 2; j < parsedList.Count; j++)
                        {
                            if (j % 2 == 0 && parsedList[j].Contains("ИНН"))
                            {
                                parsedList[j] = $"ПОЛУЧАТЕЛЬ: {parsedList[j]}";
                                i++;
                            }
                            if (j % 2 != 0 && parsedList[j].Contains("ИНН"))
                            {
                                parsedList[j] = $"ЗАКАЗЧИК: {parsedList[j]}";
                                i++;
                            }
                        }
                    }
                    catch { }
                }

                result.Add(parsedList[i]);
            }

            return result;
        }

        private List<string> RemoveColonsAsFirstChar(List<string> parsedList)
        {
            HashSet<string> result = new HashSet<string>();

            foreach (var line in parsedList)
            {
                var currentLine = NormalizeDoubleChars(line);

                if (line.Substring(0, 1) == ":")
                {
                    result.Add(currentLine.Substring(1).Trim());
                }
                else
                {
                    result.Add(currentLine);
                }
            }

            return result.ToList();
        }


        private string NormalizeDoubleChars(string text)
        {
            var result = Regex.Replace(text, " {2,}", " ");
            return Regex.Replace(result, ":{2,}", ":");
        }

        private List<string> RemoveDoublicatesInList(List<string> parsedList)
        {
            var result = new List<string>();
            foreach (var line in parsedList)
            {
                result.Add(RemoveDoublicatesFromString(line));
            }
            return result;
        }

        private string RemoveDoublicatesFromString(string text)
        {
            var splitedText = text.Split(" ").ToHashSet();

            return string.Join(" ", splitedText);
        }

        private List<string> MapInvoice(List<string> parsedList)
        {
            SliceRef sliceRef = new SliceRef();

            var recipientRefs = recipientName.GetReferenceTokens();
            var accountRefs = sliceRef.GetAccountTokens();
            var headerRefs = sliceRef.GetHeaderTokens();
            var footerRefs = sliceRef.GetFooterTokens();

            var recipientFlag = false;
            var accountFlag = false;

            var result = new List<string>();

            for (var i = 0; i < parsedList.Count; i++)
            {
                var currentLine = parsedList[i];

                bool recipientLinePos = currentLine.DoesLineContain(recipientRefs);
                bool recipientLineNeg = currentLine.DoesLineContain(recipientName.GetExclusions());
                bool recipientRef = currentLine.DoesLineContain(recipientName.GetKeyTokens());
                bool accountLine = currentLine.DoesLineContain(accountRefs);
                bool headerLine = currentLine.DoesLineContain(headerRefs);
                bool footerLine = currentLine.DoesLineContain(footerRefs);

                try
                {
                    if (recipientLinePos && recipientRef && currentLine.Trim().Split(" ").Count() > 1)
                    {
                        result.Add($"company!- {currentLine}");
                    }

                    if (recipientLinePos && !recipientFlag && !recipientLineNeg && currentLine.Split("\"").Count() == 3 && currentLine.Trim().Split(" ").Count() > 1)
                    {
                        result.Add("-recipient-");
                        result.Add(currentLine);
                        recipientFlag = true;

                        if (parsedList[i - 1].Contains("ИНН"))
                        {
                            result.Add("-recipientInn-");
                            result.Add(parsedList[i - 1]);
                        }
                        else if (parsedList[i + 1].Contains("ИНН"))
                        {
                            result.Add("-recipientInn-");
                            result.Add(parsedList[i + 1]);
                            i++;
                        }
                    }
                    else if (recipientLinePos && !recipientFlag && !recipientLineNeg && currentLine.Contains("ИП ") && currentLine.Trim().Split(" ").Count() > 1)
                    {
                        result.Add("-recipient-");
                        result.Add(currentLine);
                        recipientFlag = true;


                        if (parsedList[i - 1].Contains("ИНН"))
                        {
                            result.Add($"recipientInn!- {parsedList[i - 1]}");
                        }
                        else if (parsedList[i + 1].Contains("ИНН"))
                        {
                            result.Add($"recipientInn!- {parsedList[i + 1]}");
                            i++;
                        }
                    }
                    else if (accountLine && !accountFlag)
                    {
                        result.Add("-account-");
                        result.Add(currentLine);

                        accountFlag = true;
                    }
                    else if (headerLine && !footerLine && currentLine.Length > 10 && currentLine.Length < 80)
                    {
                        result.Add("-payment-");
                    }
                    else if (headerLine && footerLine && currentLine.Length > 10 && currentLine.Length < 80)
                    {
                        result.Add("-payment-");
                    }
                    else if (!headerLine && footerLine && currentLine.Length > 10 && currentLine.Length < 80)
                    {
                        result.Add("-amount-");

                        result.Add(currentLine);
                        result.Add(parsedList[i + 1]);
                        result.Add(parsedList[i + 2]);
                        result.Add("-end-");

                        break;
                    }
                    else
                    {
                        result.Add(currentLine);
                    }
                }
                catch { }
            }

            return result;
        }
    }
}

using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyName;
using PdfParser.ReferenceData.Interfaces;
using System.Text.RegularExpressions;

namespace PdfParser.BL.Normalizator
{
    public class Normalizer
    {
        List<string> recipientExclusions;
        List<string> payerExclusions;

        public Normalizer()
        {
            IReferenceData recipientName = new RecipientName();
            IReferenceData payerName = new PayerName();

            recipientExclusions = recipientName.GetExclusions();
            payerExclusions = payerName.GetExclusions();
        }
        public List<string> NormalizeText(string parsedText)
        {
            var text = RemoveAllUnreadableChars(parsedText).ToUpper();
            text = NormalizeQuotes(text);
            text = NormalizeInvoiceAttributes(text);

            var parsedList = GetParsedList(text);
            parsedList = NormalizeKeyWords(parsedList);
            parsedList = NormolizeInterferingExclusions(parsedList);
            var result = RemoveColonsAsFirstChar(parsedList);

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
            //foreach (var chr in chars)
            //{
            //    result= text.Replace(chr, ' ');
            //}
            return result;
        }

        private string NormalizeQuotes(string text)
        {
            var result = text.Replace("«", "\"").Replace("»", "\"");
            //result = Regex.Replace(result, " {2,}", " ");

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

                foreach (string word in attribute.GetVocalbuary())
                {
                    if (word.ToUpper() == "ИНН /КПП")
                    {
                        var inn = text.GetNextWordByReferenceText(word.ToUpper() + " ");
                        var newInn = inn.Replace("/", ", КПП ");
                        text = text.Replace(inn, newInn);
                    }
                    text = new string(text.Replace(word.ToUpper(), refWord.ToUpper()));
                }
            }

            return text;
        }

        // текст в список
        private List<string> GetParsedList(string parsedText)
        {
            string[] textArray;
            HashSet<string> parsedList = new HashSet<string>();

            textArray = parsedText.Split("\n");

            foreach (string text in textArray)
            {
                if (text.Trim() != "")
                {
                    parsedList.Add(text);
                }
            };

            return parsedList.ToList();
        }

        private List<string> NormalizeKeyWords(List<string> parsedList)
        {
            IReferenceData recipientName = new RecipientName();
            IReferenceData payerName = new PayerName();

            var normalizeRecipient = UnionSeparatedKeyWordsAndNextLine(parsedList, recipientName.GetKeyWords());
            var normalizedText = UnionSeparatedKeyWordsAndNextLine(normalizeRecipient, payerName.GetKeyWords());

            return normalizedText;
        }

        //объединение строки с одиночным ключевым словом со следующей строкой
        private List<string> UnionSeparatedKeyWordsAndNextLine(List<string> parsedList, IEnumerable<string> keyWords)
        {
            var result = new List<string>();

            for (var i = 0; i < parsedList.Count; i++)
            {
                var currentLine = parsedList[i].Replace(":", "").Trim();

                foreach (var word in keyWords)
                {
                    if (currentLine.Contains(word.ToUpper()) && currentLine.Split(' ').Length == 1)
                    {
                        result.Add($"{currentLine}: {parsedList[i + 1]}");
                    }
                }
                if (currentLine.Split(' ').Length != 1)
                {
                    result.Add(parsedList[i]);
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
                bool recipientCheck = recipientExclusions.Any(y => currentLine.Any(x => x.Contains(y)));
                bool payerCheck = payerExclusions.Any(y => currentLine.Any(x => x.Contains(y)));
                //bool recipientCheck = currentLine.Any(y => recipientExclusions.Any(x => x.Contains(y)));
                //bool payerCheck = currentLine.Any(y => payerExclusions.Any(x => x.Contains(y)));

                if (recipientCheck && payerCheck)
                {
                    try
                    {
                        result.Add($"{currentLine[0]}: {parsedList[i + 1]}");
                        result.Add($"{currentLine[1]}: {parsedList[i + 2]}");
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
                var currentLine = NormalizeSpaces(line);

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


        private string NormalizeSpaces(string text)
        {
            return Regex.Replace(text, " {2,}", " "); ;
        }
    }
}

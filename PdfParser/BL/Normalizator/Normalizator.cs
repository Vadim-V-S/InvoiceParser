using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyName;
using PdfParser.ReferenceData.Interfaces;
using System.Text.RegularExpressions;

namespace PdfParser.BL.Normalizator
{
    public class Normalizator
    {
        public List<string> NormalizeText(string parsedText)
        {
            var text = RemoveAllUnreadableChars(parsedText).ToUpper();
            text = NormalizeQuotesAndSpaces(text);
            text = NormalizeInvoiceAttributes(text);

            var parsedList = GetParsedList(text);
            var result = NormalizeKeyWords(parsedList);

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

        private string NormalizeQuotesAndSpaces(string text)
        {
            var result = text.Replace("«", "\"").Replace("»", "\"");
            result = Regex.Replace(result, " {2,}", " ");

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
        private List<string> UnionSeparatedKeyWordsAndNextLine(List<string> allText, IEnumerable<string> keyWords)
        {
            var result = new List<string>();

            for (var i = 0; i < allText.Count; i++)
            {
                var currentLine = allText[i].Replace(":", "").Trim();

                foreach (var word in keyWords)
                {
                    if (currentLine.Contains(word.ToUpper()) && currentLine.Split(' ').Length == 1)
                    {
                        result.Add($"{currentLine}: {allText[i + 1]}");
                    }
                }
                if (currentLine.Split(' ').Length != 1)
                {
                    result.Add(allText[i]);
                }
            }

            return result;
        }

    }
}

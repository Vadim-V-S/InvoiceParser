using PdfParser.Extensions;

namespace PdfParser.ReferenceData.Norm
{
    public abstract class Vocalbuary
    {
        internal List<string> refWords = new List<string>();
        internal string targetWord = string.Empty;

        public abstract List<string> GetVocalbuary();

        public string NormalizeText(string allText)
        {
            string text = allText.Replace("«", "\"").Replace("»", "\"").ToUpper();
            foreach (var refWord in refWords)
            {
                targetWord = refWord.ToUpper().Replace("\n", "").Trim();
                foreach (string word in GetVocalbuary())
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
    }
}

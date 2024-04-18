using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyInn
{
    public class CompanyInn : IReferenceData
    {
        internal List<string> inn = new List<string>();
        internal List<string> keyWords = new List<string>();
        internal List<string> exclusions = new List<string>();
        public CompanyInn()
        {
            inn.Add("ИНН ");
            inn.Add("ИНН");

            keyWords.Add("ИНН");
            keyWords.Add("ИНН/КПП");
        }

        public List<string> GetReferenceWords()
        {
            return inn;
        }

        public List<string> GetKeyWords()
        {
            return keyWords;
        }
        public List<string> GetExclusions()
        {
            return exclusions;
        }
    }
}

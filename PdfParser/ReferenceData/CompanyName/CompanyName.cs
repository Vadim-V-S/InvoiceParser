using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyName
{
    public class CompanyName : IReferenceData
    {
        internal List<string> companyName = new List<string>();
        internal List<string> keyWords = new List<string>();
        internal List<string> exclusions = new List<string>();
        public CompanyName()
        {
            //companyName.Add("Общество с ограниченной ответственностью ");
            companyName.Add("ООО ");
            companyName.Add("АО ");
            companyName.Add("ИП ");
            //companyName.Add("Акционерное Общество ");
            companyName.Add("ответственностью \"");
            companyName.Add("\"");

            keyWords.Add("ооо ");
            keyWords.Add("ао ");
            keyWords.Add("ип ");
        }

        public List<string> GetReferenceWords()
        {
            return companyName;
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

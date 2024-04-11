using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class PaymentAmount : IReferenceData
    {
        List<string> companyName = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();
        public PaymentAmount()
        {
            keyWords.Add("всего");
            keyWords.Add("итого");
            keyWords.Add("оплате");
            keyWords.Add("общество ");
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

using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class PaymentAmount : Interfaces.IReferenceData
    {
        List<string> keyWords = new List<string>();
        public PaymentAmount()
        {
            keyWords.Add("всего");
            keyWords.Add("итого");
            keyWords.Add("оплате");
            keyWords.Add("общество ");
        }
        public List<string> GetReferenceWords()
        {
            return new List<string>();
        }
        public List<string> GetKeyWords()
        {
            return keyWords.ConvertAll(x => x.ToUpper());
        }
        public List<string> GetExclusions()
        {
            return new List<string>();
        }
    }
}

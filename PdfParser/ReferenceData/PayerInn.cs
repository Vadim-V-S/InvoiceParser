using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class PayerInn : IReferenceData
    {
        List<string> inn = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();
        public PayerInn()
        {
            inn.Add("ИНН ");
            inn.Add("ИНН");

            keyWords.Add("получател");
            keyWords.Add("покупател");
            keyWords.Add("заказчик");
            keyWords.Add("лицензиат");
            keyWords.Add("плательщик");
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

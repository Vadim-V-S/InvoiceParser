using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class Currency : IReferenceData
    {
        List<string> currency = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();

        public Currency()
        {
            currency.Add("рубли");
            currency.Add("Сумма, руб. ");
            currency.Add("₽");
            currency.Add(" рублей ");
            currency.Add("руб.");

            keyWords.Add("рублей");
            keyWords.Add("рубл");
            keyWords.Add("руб.");
            keyWords.Add("евро");
            keyWords.Add("евр.");
            keyWords.Add("eur");
            keyWords.Add("долларов");
            keyWords.Add("дол.");
            keyWords.Add("юаней");
        }

        public List<string> GetReferenceWords()
        {
            return currency;
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

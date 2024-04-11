using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class Invoice: IReferenceData
    {
        List<string> invoice = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();

        public Invoice()
        {
            invoice.Add("Счет на оплату от");
            invoice.Add("Счёт на оплату от");
            invoice.Add("Счет от");
            invoice.Add("Счёт от");
            invoice.Add("Счет-оферта от");
            invoice.Add("Счёт-оферта от");

            keyWords.Add("счет");
            keyWords.Add("счёт");
        }

        public List<string> GetReferenceWords()
        {
            return invoice;
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

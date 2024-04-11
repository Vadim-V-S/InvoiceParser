using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class RecipientInn : IReferenceData
    {
        List<string> inn = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();
        public RecipientInn()
        {
            inn.Add("ИНН ");
            inn.Add("ИНН");

            keyWords.Add("инн");
            keyWords.Add("отправител");
            keyWords.Add("поставщик");
            keyWords.Add("лицензиат");
            keyWords.Add("продавец");
            keyWords.Add("лицензиар");
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

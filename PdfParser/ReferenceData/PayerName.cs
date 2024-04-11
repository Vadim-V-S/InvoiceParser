using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class PayerName : IReferenceData
    {
        List<string> companyName = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();
        public PayerName()
        {
            companyName.Add("Общество с ограниченной ответственностью ");
            companyName.Add("ООО ");
            companyName.Add("АО ");
            companyName.Add("Акционерное Общество ");
            companyName.Add("ответственностью \"");

            keyWords.Add("ооо ");
            keyWords.Add("ooo "); // английский
            keyWords.Add("ао ");
            keyWords.Add("ao ");  // английский
            keyWords.Add("организация");
            keyWords.Add("общество ");
            keyWords.Add("ответственностью ");
            keyWords.Add("плательщик");
            keyWords.Add("лицензиат");
            keyWords.Add("заказчик");
            keyWords.Add("покупатель");
            keyWords.Add("грузополучатель");

            exclusions.Add("банк");
            exclusions.Add("обязуется");
            exclusions.Add("оплатить");
            exclusions.Add("направляет");
            exclusions.Add("получатель");
            exclusions.Add("лицензиар");
            exclusions.Add("поставщик");
            exclusions.Add("исполнитель");
            exclusions.Add("грузоотправитель");
            exclusions.Add("bank");
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

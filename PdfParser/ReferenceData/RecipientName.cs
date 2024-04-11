using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class RecipientName : IReferenceData
    {
        List<string> companyName = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();

        public RecipientName()
        {
            companyName.Add("Общество с ограниченной ответственностью ");
            companyName.Add("ООО ");
            companyName.Add("OOO "); // английский
            companyName.Add("АО ");
            companyName.Add("AO "); // английский
            companyName.Add("Акционерное Общество ");
            companyName.Add("ответственностью \"");

            keyWords.Add("ооо ");
            keyWords.Add("ooo "); // английский
            keyWords.Add("ао ");
            keyWords.Add("ao ");  // английский
            keyWords.Add("организация");
            keyWords.Add("общество");
            keyWords.Add("ответственностью ");
            keyWords.Add("получатель");
            keyWords.Add("лицензиар");
            keyWords.Add("исполнитель");
            keyWords.Add("поставщик");
            keyWords.Add("грузоотправитель");

            exclusions.Add("банк");
            exclusions.Add("обязуется");
            exclusions.Add("направляет");
            exclusions.Add("плательщик");
            exclusions.Add("лицензиат");
            exclusions.Add("покупатель");
            exclusions.Add("заказчик");
            exclusions.Add("грузополучатель");
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

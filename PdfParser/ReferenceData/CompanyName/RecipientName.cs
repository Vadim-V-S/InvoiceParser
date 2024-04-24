using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyName
{
    public class RecipientName : CompanyName
    {
        public RecipientName()
        {
            keyWords.Add("организация");
            keyWords.Add("получател");
            keyWords.Add("лицензиар");
            keyWords.Add("исполнител");
            keyWords.Add("поставщик");
            keyWords.Add("продавец");
            keyWords.Add("грузоотправител");

            exclusions.Add("банк");
            exclusions.Add("обязуется");
            exclusions.Add("направляет");
            exclusions.Add("плательщик");
            exclusions.Add("лицензиат");
            exclusions.Add("покупател");
            exclusions.Add("заказчик");
            exclusions.Add("грузополучател");
            exclusions.Add("bank");
        }
    }
}

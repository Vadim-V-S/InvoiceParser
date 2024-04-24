using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyName
{
    public class PayerName : CompanyName
    {
        public PayerName()
        {
            keyWords.Add("организация");
            keyWords.Add("плательщик");
            keyWords.Add("лицензиат");
            keyWords.Add("заказчик");
            keyWords.Add("покупатель");
            keyWords.Add("грузополучатель");


            exclusions.Add("банк");
            exclusions.Add("обязуется");
            exclusions.Add("оплатить");
            exclusions.Add("направляет");
            exclusions.Add("получател");
            exclusions.Add("лицензиар");
            exclusions.Add("поставщик");
            exclusions.Add("продавец");
            exclusions.Add("исполнител");
            exclusions.Add("грузоотправител");
            exclusions.Add("bank");
        }
    }
}

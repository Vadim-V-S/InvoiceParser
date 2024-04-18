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
            exclusions.Add("получатель");
            exclusions.Add("лицензиар");
            exclusions.Add("поставщик");
            exclusions.Add("исполнитель");
            exclusions.Add("грузоотправитель");
            exclusions.Add("bank");
        }
    }
}

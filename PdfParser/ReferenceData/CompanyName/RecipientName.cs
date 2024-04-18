using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyName
{
    public class RecipientName : CompanyName
    {
        public RecipientName()
        {
            keyWords.Add("организация");
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
    }
}

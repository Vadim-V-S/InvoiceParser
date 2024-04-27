using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyInn
{
    public class PayerInn : CompanyInn
    {
        public PayerInn()
        {
            keyWords.Add("покупател");
            keyWords.Add("заказчик");
            keyWords.Add("лицензиат");
            keyWords.Add("плательщик");

            exclusions.Add("получател");
            exclusions.Add("лицензиар");
            exclusions.Add("поставщик");
            exclusions.Add("продавец");
            exclusions.Add("исполнител");
            exclusions.Add("грузоотправител");
        }
    }
}

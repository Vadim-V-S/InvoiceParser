using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyInn
{
    public class RecipientInn : CompanyInn
    {
        public RecipientInn()
        {
            keyWords.Add("отправител");
            keyWords.Add("поставщик");
            keyWords.Add("лицензиар");
            keyWords.Add("продавец");
            keyWords.Add("лицензиар");

            exclusions.Add("плательщик");
            exclusions.Add("лицензиат");
            exclusions.Add("покупател");
            exclusions.Add("заказчик");
            exclusions.Add("грузополучател");
        }
    }
}

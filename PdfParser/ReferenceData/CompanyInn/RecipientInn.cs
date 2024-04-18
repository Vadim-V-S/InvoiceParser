using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyInn
{
    public class RecipientInn : CompanyInn
    {
        public RecipientInn()
        {
            keyWords.Add("отправител");
            keyWords.Add("поставщик");
            keyWords.Add("лицензиат");
            keyWords.Add("продавец");
            keyWords.Add("лицензиар");
        }
    }
}

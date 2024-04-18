using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyInn
{
    public class PayerInn : CompanyInn
    {
        public PayerInn()
        {
            keyWords.Add("получател");
            keyWords.Add("покупател");
            keyWords.Add("заказчик");
            keyWords.Add("лицензиат");
            keyWords.Add("плательщик");
        }
    }
}

namespace PdfParser.ReferenceData.CompanyAddress
{
    public class RecipientAddress : CompanyAddress
    {
        public RecipientAddress()
        {
            exclusions.Add("покупатель");
            exclusions.Add("заказчик");
        }
    }
}

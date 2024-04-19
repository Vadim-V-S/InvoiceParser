namespace PdfParser.ReferenceData.CompanyAddress
{
    public class PayerAddress : CompanyAddress
    {
        public PayerAddress()
        {
            exclusions.Add("поставщик");
            exclusions.Add("исполнитель");
        }

        public List<string> GetReferenceWords()
        {
            return companyAddress.ConvertAll(x => x.ToUpper());
        }

        public List<string> GetKeyWords()
        {
            return keyWords.ConvertAll(x => x.ToUpper());
        }
        public List<string> GetExclusions()
        {
            return exclusions.ConvertAll(x => x.ToUpper());
        }
    }
}

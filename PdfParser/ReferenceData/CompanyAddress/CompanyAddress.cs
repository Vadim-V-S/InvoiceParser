using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData.CompanyAddress
{
    public class CompanyAddress : IReferenceData
    {
        internal List<string> companyAddress = new List<string>();
        internal List<string> keyWords = new List<string>();
        internal List<string> exclusions = new List<string>();

        public CompanyAddress()
        {
            companyAddress.Add(",  обл. , г. , ул. , дом , квартира ");
            companyAddress.Add(",  область , город. , улица. , дом , квартира ");
            companyAddress.Add(",  область , город. , улица. , дом , офис ");
            companyAddress.Add(",  обл, г. , ул. , д. , кв. ");
            companyAddress.Add("321000,  обл. , г. , ул. , дом , квартира ");
            companyAddress.Add("321000 ,  область , город. , улица. , дом , квартира ");
            companyAddress.Add("321000 ,  область , город. , улица. , дом , офис ");
            companyAddress.Add("321000 ,  обл. , г. , ул. , д. , кв. ");

            keyWords.Add("д. ");
            keyWords.Add("дом");
            keyWords.Add("ул.");
            keyWords.Add("ул,");
            keyWords.Add("улица");
            keyWords.Add("край");
            keyWords.Add("область");
            keyWords.Add("обл.");
            keyWords.Add("обл,");
            keyWords.Add("офис ");
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

using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class PayerAddress : IReferenceData
    {
        List<string> companyAddress = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();

        public PayerAddress()
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
            keyWords.Add("ул. ");
            keyWords.Add("ул, ");
            keyWords.Add("улица");
            keyWords.Add("край");
            keyWords.Add("область");
            keyWords.Add("офис ");

            exclusions.Add("поставщик");
            exclusions.Add("исполнитель");
        }

        public List<string> GetReferenceWords()
        {
            return companyAddress;
        }

        public List<string> GetKeyWords()
        {
            return keyWords;
        }
        public List<string> GetExclusions()
        {
            return exclusions;
        }
    }
}

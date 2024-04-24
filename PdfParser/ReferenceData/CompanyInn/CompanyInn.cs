namespace PdfParser.ReferenceData.CompanyInn
{
    public class CompanyInn : Interfaces.IReferenceData
    {
        internal List<string> inn = new List<string>();
        internal List<string> keyWords = new List<string>();
        public CompanyInn()
        {
            inn.Add("ИНН ");
            inn.Add("ИНН");

            keyWords.Add("ИНН");
            keyWords.Add("КПП");
            keyWords.Add("ИНН/КПП");
        }

        public List<string> GetReferenceWords()
        {
            return inn.ConvertAll(x=>x.ToUpper());
        }

        public List<string> GetKeyWords()
        {
            return keyWords.ConvertAll(x => x.ToUpper());
        }
        public List<string> GetExclusions()
        {
            return new List<string>();
        }
    }
}

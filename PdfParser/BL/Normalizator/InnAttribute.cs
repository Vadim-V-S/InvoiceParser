namespace PdfParser.BL.Normalizator
{
    public class InnAttribute : InvoiceAttribute
    {
        public InnAttribute()
        {
            RefWords.Add("\nИНН ");
        }
        public override List<string> GetVocalbuary()
        {
            var inn = new List<string>()
            {
                "ИНН", //ru
                "ИНН /КПП", //ru
                "uhh", //en
                "uHн",
                "uНН",
                "UнH",
                "ИHH",
                "Инн",
                "ИHн",
                "ИнH",
                "ann", //en
                "ahh", //en
                "АНН",
                "АНh",
                "Аhh",
                "АhН",
                "ahН",
                "aНh",
                "hhh",
                "ННН",
            };

            return inn.ConvertAll(x => x.ToUpper());
        }
    }
}

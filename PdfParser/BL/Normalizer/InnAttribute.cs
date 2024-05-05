namespace PdfParser.BL.Normalizator
{
    public class InnAttribute : InvoiceAttribute
    {
        public InnAttribute()
        {
            RefWords.Add("\nИНН ");
        }
        public override List<string> GetTokens()
        {
            var inn = new List<string>()
            {
                "ИНН ", //ru
                "ИНН /КПП ",
                "uhh ", //en
                "uHн ",
                "uНН ",
                "UнH ",
                "ИHH ",
                "Инн ",
                "ИHн ",
                "ИнH ",
                "ann ",
                "ahh ",
                //"АНН ",
                "АНh ",
                "Аhh ",
                "АhН ",
                "ahН ",
                "aНh ",
                "hhh ",
                "ННН ",
            };

            return inn.ConvertAll(x => x.ToUpper());
        }
    }
}

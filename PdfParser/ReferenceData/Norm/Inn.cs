namespace PdfParser.ReferenceData.Norm
{
    public class Inn : Vocalbuary
    {
        public Inn()
        {
            refWords.Add("\nИНН ");
        }
        public override List<string> GetVocalbuary()
        {
            return new List<string>()
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
        }
    }
}

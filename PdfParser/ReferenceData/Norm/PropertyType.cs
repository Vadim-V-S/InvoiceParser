namespace PdfParser.ReferenceData.Norm
{
    public class PropertyType : Vocalbuary
    {
        public PropertyType()
        {
            refWords.Add("\nАО ");
            refWords.Add("\nООО ");
            refWords.Add("\nИП ");
        }
        public override List<string> GetVocalbuary()
        {
            switch (targetWord)
            {
                case "АО":
                    return new List<string>()
                    {
                        "АО", //ru
                        "ao", //en
                        "Aо",
                        "аO",
                        "Акционерное общество",
                    };
                    break;

                case "ООО":
                    return new List<string>()
                    {
                        "ООО", //ru
                        "ooo", //en
                        "ООo",
                        "oОО",
                        "ОoО",
                        "oОo",
                        "ooО",
                        "Оoo",
                        "Общество с ограниченной ответственностью",
                        "ограниченной ответственностью",
                    };
                    break;
                case "ИП":
                    return new List<string>()
                    {
                        "ИП",
                        "индивидуальный предприниматель",
                        "uП",
                    };
                    break;
            }
            return new List<string>();
        }
    }
}

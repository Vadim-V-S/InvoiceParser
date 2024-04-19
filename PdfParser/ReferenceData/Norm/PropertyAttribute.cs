namespace PdfParser.ReferenceData.Norm
{
    public class PropertyAttribute : InvoiceAttribute
    {
        public PropertyAttribute()
        {
            RefWords.Add("\nАО ");
            RefWords.Add("\nООО ");
            RefWords.Add("\nИП ");
        }
        public override List<string> GetVocalbuary()
        {
            switch (TargetWord)
            {
                case "АО":
                    var ao = new List<string>()
                    {
                        "АО", //ru
                        "ao", //en
                        "Aо",
                        "аO",
                        "Акционерное общество",
                    };

                    return ao.ConvertAll(x => x.ToUpper());
                    break;

                case "ООО":
                    var ooo =  new List<string>()
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

                    return ooo.ConvertAll(x => x.ToUpper());
                    break;
                case "ИП":
                    var ip= new List<string>()
                    {
                        "ИП",
                        "индивидуальный предприниматель",
                        "uП",
                    };

                    return ip.ConvertAll(x => x.ToUpper());
                    break;
            }

            return new List<string>();
        }
    }
}

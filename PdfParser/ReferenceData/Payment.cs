using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class Payment : Interfaces.IReferenceData
    {
        List<string> paymentName = new List<string>();
        List<string> keyWords = new List<string>();
        List<string> exclusions = new List<string>();
        public Payment()
        {
            paymentName.Add("использование сервиса");
            paymentName.Add("Пополнение счета");
            paymentName.Add("Оплата услуги ");
            paymentName.Add("Оплата по счету ");
            paymentName.Add(" услугами ");
            paymentName.Add("Выполнение ");
            paymentName.Add("Доступ к");;
            paymentName.Add("Предоставление ");;
            paymentName.Add("Услуги ");;
            paymentName.Add(" за использование ");;

            exclusions.Add("наименование");
            exclusions.Add("всего");
            exclusions.Add("итого");
            exclusions.Add("ндс");
            exclusions.Add("кол-во");
            exclusions.Add("количество");
            exclusions.Add("инн ");
            exclusions.Add("внимание");
            exclusions.Add("банк ");
            exclusions.Add("bank ");
        }

        public List<string> GetReferenceWords()
        {
            return paymentName.ConvertAll(x => x.ToUpper());
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

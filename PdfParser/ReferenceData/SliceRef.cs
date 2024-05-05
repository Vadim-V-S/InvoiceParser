using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.ReferenceData
{
    public class SliceRef
    {
        List<string> accountTokens = new List<string>();
        List<string> footerTokens = new List<string>();
        List<string> paymentHeaderTokens = new List<string>();

        public SliceRef()
        {
            accountTokens = new List<string>()
            {
                "СЧ.№",
                "СЧ.№",
                "СЧ№",
                "Р/С№",
                "Р/СЧ№",
                "Р.СЧ№",
                "К/С№",
                "К/СЧ№",
                "К.СЧ№",
                "ЛИЦЕВОЙСЧЕТ№",
                "КОРР.СЧЕТ№",
                "КОРРЕСПОНДЕНСКИЙ",
                "РАСЧЕТНЫЙСЧЕТ№",
                "РАС.СЧЕТ№",
                "БИК0",
                "BUK0",
                "БАНК",
                "BANK",
            };

            footerTokens = new List<string>()
            {
                "ВСЕГО",
                "К ОПЛАТЕ",
                "ИТОГО"
            };

            paymentHeaderTokens = new List<string>()
            {
                "СУММА",
                "CYMMA",
                "ЦЕНА",
                "ТОВАРЫ",
                "КОЛ-ВО",
                //"ОПЛАТА",
            };

        }

        public List<string> GetFooterTokens()
        {
            return footerTokens.ConvertAll(x => x.ToUpper());
        }

        public List<string> GetHeaderTokens()
        {
            return paymentHeaderTokens.ConvertAll(x => x.ToUpper());
        }

        public List<string> GetAccountTokens()
        {
            return accountTokens.ConvertAll(x => x.ToUpper());
        }
    }
}

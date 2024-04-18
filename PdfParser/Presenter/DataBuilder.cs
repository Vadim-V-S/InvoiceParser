using PdfParser.BL.TextExtractors;
using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData.Norm;

namespace PdfParser.Presenter
{
    public class DataBuilder
    {
        // объявляем поля экстракторов
        ITextExtractor invoiceExtractor;
        ITextExtractor recipientInnExtractor;
        ITextExtractor payerInnExtractor;
        ITextExtractor recipientNameExtractor;
        ITextExtractor payerNameExtractor;
        ITextExtractor recipientAddressExtractor;
        ITextExtractor payerAddressExtractor;
        ITextExtractor amountExtractor;
        ITextExtractor paymentExtractor;
        ITextExtractor currencyExtractor;

        public DataBuilder(string parsedText) // инициализируем поля экстракторов
        {
            var parsedData = GetParsedDataList(parsedText);

            invoiceExtractor = new InvoiceExtractor(parsedData);
            recipientInnExtractor = new RecipientInnExtractor(parsedData);
            payerInnExtractor = new PayerInnExtractor(parsedData);
            recipientNameExtractor = new RecipientNameExtractor(parsedData);
            payerNameExtractor = new PayerNameExtractor(parsedData);
            recipientAddressExtractor = new RecipientAddressExtractor(parsedData);
            payerAddressExtractor = new PayerAddressExtractor(parsedData);
            amountExtractor = new PaymentAmountExtractor(parsedData);
            paymentExtractor = new PaymentExtractor(parsedData);
            currencyExtractor = new CurrencyExtractor(parsedData);
        }

        private List<string> GetParsedDataList(string parsedText)
        {
            var text = parsedText.RemoveAllUnreadableChars();
            var result = text;

            List<Vocalbuary> vocalbuary = new List<Vocalbuary>
            {
                new Inn(),
                new PropertyType()
            };
            foreach (var item in vocalbuary)
            {
                result = new string(item.NormalizeText(result));
            }

            return result.GetTextList();
        }

        public InvoiceData BuildResult()
        {
            // вызываем методы экстракторов с готовыми данными
            var invoice = invoiceExtractor.GetResultValue();
            var recipientName = recipientNameExtractor.GetResultValue();
            var payerName = payerNameExtractor.GetResultValue();
            var recipientInn = recipientInnExtractor.GetResultValue();
            var payerInn = payerInnExtractor.GetResultValue();
            var recipientAddress = recipientAddressExtractor.GetResultValue();
            var payerAddress = payerAddressExtractor.GetResultValue();
            var paymentAmount = amountExtractor.GetResultValue();
            var paymentName = paymentExtractor.GetResultValue();
            var currency = currencyExtractor.GetResultValue();


            return new InvoiceData
            {
                Invoice = invoice,
                RecipientName = recipientName,
                RecipientInn = recipientInn,
                RecipientAddress = recipientAddress,
                PayerName = payerName,
                PayerInn = payerInn,
                PayerAddress = payerAddress,
                PaymentName = paymentName,
                PaymentAmount = paymentAmount,
                Currency = currency,
            };
        }
    }
}

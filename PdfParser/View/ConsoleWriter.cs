using System.Text;

namespace PdfParser.View
{
    public class ConsoleWriter : IWriter
    {
        public void WriteData(InvoiceData invoiceData)
        {
            // собираем данные в строку для вывода
            var result = new StringBuilder();
            result.AppendFormat($"Счет: {invoiceData.Invoice}\n");
            result.AppendFormat($"Получатель: {invoiceData.RecipientName}\n");
            result.AppendFormat($"ИНН Получателя: {invoiceData.RecipientInn}\n");
            //result.AppendFormat($"Адрес Получателя: {invoiceData.RecipientAddress}\n");

            result.AppendFormat($"-\n");

            result.AppendFormat($"Плательщик: {invoiceData.PayerName}\n");
            result.AppendFormat($"ИНН Плательщика: {invoiceData.PayerInn}\n");
            //result.AppendFormat($"Адрес Плательщика: {invoiceData.PayerAddress}\n");

            result.AppendFormat($"-\n");

            result.AppendFormat($"Назначение платежа: {invoiceData.PaymentName}\n");
            result.AppendFormat($"Сумма к оплате: {invoiceData.PaymentAmount}\n");
            result.AppendFormat($"Валюта: {invoiceData.Currency}\n");

            Console.WriteLine( result.ToString() );
        }
    }
}

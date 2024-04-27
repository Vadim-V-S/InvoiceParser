using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.BL.TextExtractors
{
    // назначение платежа
    public class PaymentExtractor : DataExtractor
    {
        public PaymentExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Payment();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.CutOffFooter(invoiceFooterTokens);
            slice = slice.CutOffTop(paymentHeaderTokens);

            var extraction = slice.RemoveElementsFromListByExclusions(exclusions);
            var concintentData = parsedData.GetConsistentData(extraction);

            while (!concintentData.DoesListContainsDigits() && concintentData.Count != 0)
            {
                concintentData = extraction.Except(concintentData).ToList();
                concintentData = parsedData.GetConsistentData(concintentData);
            }


            return concintentData;
        }

        public override string GetResultValue()
        {
            var result = string.Empty;
            var extraction = ExtractData(keyWords);

            foreach (var item in extraction)
            {
                result += item + "\n";
            }
            return result;
        }
    }
}

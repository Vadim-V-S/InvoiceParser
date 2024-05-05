using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // назначение платежа
    public class PaymentExtractor : DataExtractor
    {
        public PaymentExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Payment();
            exclusions = referenceData.GetExclusions();
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var indexHeader = parsedData.IndexOf("-payment-");
            var indexAmount = parsedData.IndexOf("-amount-");

            var slice = parsedData;

            if (indexHeader != -1 && indexAmount != -1)
            {
                return parsedData.GetRange(indexHeader + 1, parsedData.Count - indexHeader - 1);
            }
            else if (indexHeader != -1 && indexAmount == -1)
            {
                return parsedData.GetRange(indexHeader, parsedData.Count - indexHeader);
            }
            else
            {
                slice = parsedData.CutOffFooter(invoiceFooterTokens);
            }

            if (paymentHeaderTokens.Any(t => slice.Any(s => s.Contains(t))))
            {
                paymentHeaderTokens.Add("ИТОГО");
                slice = slice.CutOffTopInRecursion(paymentHeaderTokens);

                var extraction = slice.RemoveElementsFromListByExclusions(exclusions);

                var concintentData = parsedData.GetConsistentData(extraction);
                while (!concintentData.DoesListContainDigits() && concintentData.Count != 0)
                {
                    concintentData = extraction.Except(concintentData).ToList();
                    concintentData = parsedData.GetConsistentData(concintentData);
                }
                return concintentData;
            }
            else
            {
                slice = slice.CutOffTop(GetLastUsedToken());
                slice = slice.RemoveElementsFromListByExclusions(exclusions);
            }

            return slice;
        }

        public override string GetResultValue()
        {
            var result = string.Empty;
            var extraction = ClearResult(ExtractData(keyWords));

            foreach (var item in extraction)
            {
                result += item + "\n";
            }
            return result;
        }
    }
}


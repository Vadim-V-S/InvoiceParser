﻿using PdfParser.Extensions;
using PdfParser.ReferenceData;
using PdfParser.ReferenceData.CompanyAddress;

namespace PdfParser.BL.TextExtractors
{
    // адрес плательщика
    public class PayerAddressExtractor : TextExtractor
    {
        public PayerAddressExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new PayerAddress();
            keyWords = referenceData.GetKeyWords();
            exclusions = referenceData.GetExclusions();

            comparator = new Comparator(new PayerAddress());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction = extraction.RemoveElementsFromListByWords(exclusions);
            //extraction = extraction.RemoveTheOnlyWordElementFromList();

            if (usedTokens.Count > 0)
            {
                extraction = extraction.RemoveElementsFromListByToken(usedTokens[token.recipientAddress]);

                //return parsedData.GetClosestElementToWord(usedTokens[token.recipientAddress], extraction); // находим адрес который находится ближе всего к плательщику, у получателя такого не делали, т.к первым адрес в списке скорее всего всегда будет получателя
                return new List<string>() { parsedData.ReturnNextItemWhenContainsKeyWord(extraction, usedTokens[token.payerName]) };
            }

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var result = "Нет Данных!";
            if (extraction.Count != 0)
            {
                result = GetResultByIndex(extraction, new RecipientAddress(), comparator.GetIndexByTokenRatio, keyWords);
            }
            usedTokens[token.payerAddress] = result;

            return result;
        }
    }
}

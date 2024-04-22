﻿using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData.CompanyInn;

namespace PdfParser.BL.TextExtractors
{
    //инн получателя
    public class RecipientInnExtractor : DataExtractor, ITextExtractor
    {
        public RecipientInnExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new RecipientInn();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new RecipientInn());
        }

        internal override List<string> ExtractData(List<string> keyWords)
        {
            var slice = parsedData.SliceListUpToWords(endSliceWords);
            var extraction = slice.CreateListByKeyWords(keyWords);
            extraction = extraction.RemoveListElementsWithoutDigits();

            return extraction;
        }

        public override string GetResultValue()
        {
            var extraction = ExtractData(keyWords);

            var inn = GetResultByIndex(extraction, new RecipientInn(), comparator.GetIndexByPartialRatio, keyWords);
            inn = inn.GetNextWordByReferenceText("ИНН ");
            inn = inn.GetNumberFromStringByLength(10);

            var result = "Нет Данных!";
            if (inn != null && inn.Length >= 10 && inn.Length <= 12 && inn.IsStringDigits())
            {
                return usedTokens[token.recipientInn] = inn;
            }

            return usedTokens[token.recipientInn] = result;
        }
    }
}

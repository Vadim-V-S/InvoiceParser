﻿using PdfParser.BL.TextExtractors.Interfaces;
using PdfParser.Extensions;
using PdfParser.ReferenceData;

namespace PdfParser.BL.TextExtractors
{
    // валюта
    public class CurrencyExtractor:TextExtractor, ITextExtractor
    {
        public CurrencyExtractor(List<string> parsedData) : base(parsedData)
        {
            referenceData = new Currency();
            keyWords = referenceData.GetKeyWords();

            comparator = new Comparator(new Currency());
        }

        public override string GetResultValue()
        {
            var result = GetResultByIndex(new Currency(), comparator.GetIndexByPartialRatio, keyWords);
            result.RemoveAllStringBesidesKeyWord(keyWords); // удаляем все элементы списка за исключением справочных
            usedTokens = new Dictionary<string, string>();

            return result;
        }
    }
}

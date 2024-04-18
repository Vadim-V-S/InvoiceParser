﻿using FuzzySharp;
using PdfParser.ReferenceData.Interfaces;

namespace PdfParser.BL
{
    public class Comparator
    {
        int levenshteinIndex = 0;
        IReferenceData referenceData;
        public Comparator(IReferenceData referenceData)
        {
            this.referenceData = referenceData;
        }

        public int GetIndexBySimpleRatio(string currentValue)
        {
            foreach (var referenceValue in referenceData.GetReferenceWords())
            {
                int newLevenshteinIndex = Fuzz.Ratio(referenceValue.ToUpper(), currentValue.ToUpper());
                GetIndex(newLevenshteinIndex);
            }
            return levenshteinIndex;
        }

        public int GetIndexByPartialRatio(string currentValue)
        {
            foreach (var referenceValue in referenceData.GetReferenceWords())
            {
                int newLevenshteinIndex = Fuzz.PartialRatio(referenceValue.ToUpper(), currentValue.ToUpper());
                GetIndex(newLevenshteinIndex);
            }
            return levenshteinIndex;
        }

        public int GetIndexByTokenRatio(string currentValue)
        {
            foreach (var referenceValue in referenceData.GetReferenceWords())
            {
                int newLevenshteinIndex = Fuzz.TokenSetRatio(referenceValue.ToUpper(), currentValue.ToUpper());
                GetIndex(newLevenshteinIndex);
            }
            return levenshteinIndex;
        }

        public string ExtractOne(string currentValue, List<string> values)
        {
            string extractedValue = Process.ExtractOne(currentValue.ToUpper(), values).Value;

            return extractedValue;
        }

        private void GetIndex(int value)
        {
            if (value > levenshteinIndex)
                levenshteinIndex = value;
        }
    }
}

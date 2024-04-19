﻿namespace PdfParser.ReferenceData.Norm
{
    public abstract class InvoiceAttribute
    {
        internal List<string> RefWords = new List<string>();
        internal string TargetWord = string.Empty;

        public abstract List<string> GetVocalbuary();
    }
}

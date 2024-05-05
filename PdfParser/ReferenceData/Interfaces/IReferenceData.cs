namespace PdfParser.ReferenceData.Interfaces
{
    public interface IReferenceData
    {
        List<string> GetReferenceTokens();
        List<string> GetKeyTokens();
        List<string> GetExclusions();
    }
}

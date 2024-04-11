namespace PdfParser.ReferenceData.Interfaces
{
    public interface IReferenceData
    {
        List<string> GetReferenceWords();
        List<string> GetKeyWords();
        List<string> GetExclusions();
    }
}

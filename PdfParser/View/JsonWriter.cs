using Newtonsoft.Json;

namespace PdfParser.View
{
    public class JsonWriter : IWriter
    {
        string filePath;
        public JsonWriter(string filePath)
        {
            //this.filePath = filePath + ".txt";
            this.filePath = filePath + ".json";
        }
        public void WriteData(InvoiceData invoiceData)
        {
            //string json = JsonConvert.SerializeObject(invoiceData);

            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, invoiceData);
            }
        }
    }
}

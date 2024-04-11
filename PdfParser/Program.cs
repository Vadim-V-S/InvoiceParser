using PdfParser.Parser;
using PdfParser.Presenter;
using PdfParser.View;

//string folderName = "Счета";
string folderName = "Счета\\3";
string desktoPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var folderPath = Path.Combine(desktoPath, folderName);

FileReader fileReader = new FileReader(folderPath);
var files = fileReader.GetFile();

if (files.Length > 0)
{
     ParseFiles(files);
}
else
{
    Console.WriteLine("Нет файлов для распознавания");
}

Console.ReadKey();



void ParseFiles(FileInfo[] files)
{
    foreach (var file in files)
    {
        Console.WriteLine($"\n--------------{file.Name}-----------------");

        TesseractParser parser = new TesseractParser(file.FullName);

        var parsedData = parser.ParseImage();

        DataBuilder dataBuilder = new DataBuilder(parsedData);

        IWriter consoleWriter = new ConsoleWriter(); // выводим в консоль
        consoleWriter.WriteData(dataBuilder.BuildResult());

        IWriter jsonWriter = new JsonWriter(file.FullName);  // выводим в json
        jsonWriter.WriteData(dataBuilder.BuildResult());
    }

    Console.WriteLine("Готово >");
}


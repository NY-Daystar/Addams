namespace Addams.Core.Utils;
public static class FileManager
{
    public static void ConcatFiles(string directory, string outputfilePath)
    {
        string[] inputFilePaths = Directory.GetFiles(directory, "*.log");
        List<string> logFiles = inputFilePaths.ToList().GetRange(0, inputFilePaths.Length - 1);

        using var outputStream = File.Create(outputfilePath);
        foreach (var log in logFiles)
        {
            using var inputStream = File.OpenRead(log);
            inputStream.CopyTo(outputStream);
        }
    }
}

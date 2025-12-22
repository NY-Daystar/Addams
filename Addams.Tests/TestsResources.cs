using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Addams.Tests;

[TestClass]
public class TestsResource
{
    [TestMethod]
    public void TestResourceUnused()
    {
        // Arrange
        var folder = @"D:/Dev/Addams";

        var files = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories).ToList();

        var resourceFile = Directory.GetFiles(folder, "Language.*.resx", SearchOption.AllDirectories).First();

        string input = File.ReadAllText(resourceFile);
        string pattern = "<data name=(\".*\") xml:space";
        RegexOptions options = RegexOptions.Multiline;

        // Act
        var resources = Regex.Matches(input, pattern, options).Select(v => v.Groups[1].ToString());

        Dictionary<string, string> output = new Dictionary<string, string>();
        foreach (var res in resources)
        {
            var found = false;

            foreach (var f in files)
            {
                var nbLine = 0;

                const int BufferSize = 128;
                using (var fileStream = File.OpenRead(f))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    string line = string.Empty;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        nbLine++;
                        var matches = Regex.Matches(line, res);
                        if (!matches.Any())
                            continue;
                        if (matches.Any())
                        {
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    output.Add(res, $"file: {new FileInfo(f).Name} - line: {nbLine}");
                    break;
                }
            }
            if (!found) output.Add(res, string.Empty);
        }

        Debug.WriteLine("List of resources");
        foreach (var o in output)
        {
            Debug.WriteLine("{0} - {1}", o.Key, o.Value);
        }

        Debug.WriteLine("---------------------------------------------");

        Debug.WriteLine("List of unused resources");
        var unused = output.Where(v => v.Value.Equals(string.Empty));
        foreach (var o in unused)
        {
            Debug.WriteLine("{0}", o.Key);
            Assert.Fail(string.Format("{0}", o.Key));
        }
    }
}
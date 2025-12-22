using System.Resources;

namespace Addams.Core.Utils;

public static class Language
{
    const string RESOURCE = "Addams.Core.Resources.Language";
    public static string GetString(string resName)
    {
        ResourceManager rm = new(RESOURCE, typeof(AddamsCore).Assembly);
        var value = rm.GetString(resName);
        return value ?? "Not found";
    }
}
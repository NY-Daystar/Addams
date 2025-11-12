using System;
using System.Resources;

namespace Addams.Utils;

public static class Language
{
    const string RESOURCE = "Addams.Resources.Language";
    public static string GetString(string resName)
    {
        if (resName is null)
        {
            throw new ArgumentNullException(nameof(resName));
        }

        ResourceManager rm = new(RESOURCE, typeof(Addams).Assembly);
        return rm.GetString(resName) ?? "Not found";
    }
}
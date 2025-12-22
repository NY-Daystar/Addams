using System;
using System.Security.Cryptography;
using System.Text;

namespace Addams.Utils;

/// <summary>
/// Class to cyphering code challenge for authorization with PKCE 
/// </summary>
public class Cypher
{
    public static string GenerateCodeVerifier()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[64];
        rng.GetBytes(bytes);
        return base64UrlEncode(bytes);
    }

    public static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
        return base64UrlEncode(bytes);
    }

    private static string base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}

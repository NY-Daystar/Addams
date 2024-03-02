using Newtonsoft.Json;
using System;

namespace Addams.Models;

/// <summary>
/// Token class converted from Token entity and saved in config
/// </summary>
public class TokenModel
{
    /// <summary>
    /// Value of the token
    /// </summary>
    [JsonProperty("access_token")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Type of the token (Default: bearer)
    /// </summary>
    [JsonProperty("token_type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Time in seconds for token expiration (Default: 3600 -> 1 hour)
    /// </summary>
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Scope list accessed by the token (Ex: "playlist-read-private user-library-read")
    /// </summary>
    [JsonProperty("scope")]
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// Datetime when token was generated
    /// </summary>
    public DateTime GeneratedAt { get; set; }

    /// <summary>
    /// Date of expiration date of the token
    /// </summary>
    public DateTime ExpiredDate { get; set; }

    /// <summary>
    /// Calculate expiration of the token
    /// </summary>
    public void CalculateExpiration()
    {
        GeneratedAt = DateTime.UtcNow;
        ExpiredDate = GeneratedAt.AddSeconds(ExpiresIn);
    }

    public override bool Equals(object? obj)
    {
        //Check for null and compare run-time types.
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        TokenModel p = (TokenModel)obj;

        return Value == p.Value
            && Type == p.Type
            && ExpiresIn == p.ExpiresIn
            && Scope == p.Scope
            && GeneratedAt == p.GeneratedAt
            && ExpiredDate == p.ExpiredDate;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override string ToString() => $"Value: {Value[..15]}................. - Type: {Type} - ExpiredDate: {ExpiredDate}";
}
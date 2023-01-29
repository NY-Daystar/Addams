namespace Addams.Entities
{
    /// <summary>
    /// Token class to get OAUTH2 token authorization
    /// </summary>
    public class Token
    {
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public int expires_in { get; set; }
        public string? scope { get; set; }
    }


}

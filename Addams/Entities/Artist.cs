using Newtonsoft.Json;

namespace Addams.Entities
{

    // TODO use attribute: [DefaultValue(5)]            
    // TODO use attribute: [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    /// <summary>
    /// Artist data
    /// </summary>
    public class Artist
    {
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; }
        public string? id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}

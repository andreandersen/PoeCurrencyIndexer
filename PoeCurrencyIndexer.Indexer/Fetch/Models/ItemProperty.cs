using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class ItemProperty
    {
        #nullable disable
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("values")]
        public object[][] Values { get; set; }
        [JsonPropertyName("displayMode")]
        public int DisplayMode { get; set; }
        [JsonPropertyName("progress")]
        public double? Progress { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
        #nullable restore
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }
    }

}

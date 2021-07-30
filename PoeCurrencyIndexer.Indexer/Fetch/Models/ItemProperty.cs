using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class ItemProperty
    {
        #nullable disable
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("values")]
        [JsonConverter(typeof(PropertyValueConverter))]
        public PropertyValue[] Values { get; set; }

        [JsonPropertyName("displayMode")]
        public int DisplayMode { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }
        #nullable restore
        
        [JsonPropertyName("progress")]
        public double? Progress { get; set; }

        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class ExtendedProperty
    {
        #nullable disable
        [JsonPropertyName("category")]
        public string Category { get; set; }
        #nullable restore

        [JsonPropertyName("subcategories")]
        public string[] SubCategories { get; set; } = System.Array.Empty<string>()!;
        [JsonPropertyName("prefixes")]
        public int? Prefixes { get; set; }
        [JsonPropertyName("suffixes")]
        public int? Suffixes { get; set; }
    }

}

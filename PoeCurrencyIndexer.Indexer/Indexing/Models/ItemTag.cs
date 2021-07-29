using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models
{
    public record ItemTag()
    {
        #nullable disable
        [JsonPropertyName("id")]
        public string Id { get; init; }
        #nullable restore

        [JsonPropertyName("text")]
        public string? Text { get; init; }

        [JsonPropertyName("image")]
        public string? Image { get; init; }

        #nullable disable
        [JsonIgnore]
        public ItemTagCategory Category {  get; internal set; }
        #nullable restore
    }
}

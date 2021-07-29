using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class ItemSocket
    {
        [JsonPropertyName("group")]
        public uint Group { get; set; }

        [JsonPropertyName("attr")]
        public string? Attr { get; set; }

        [JsonPropertyName("sColour")]
        public string? Colour { get; set; }
    }
}
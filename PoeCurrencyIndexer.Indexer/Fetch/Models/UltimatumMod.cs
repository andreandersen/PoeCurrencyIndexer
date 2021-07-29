using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class UltimatumMod
    {
        #nullable disable
        [JsonPropertyName("type")]
        public string Type { get; set; }
        #nullable restore

        [JsonPropertyName("tier")]
        public uint Tier { get; set; }
    }
}
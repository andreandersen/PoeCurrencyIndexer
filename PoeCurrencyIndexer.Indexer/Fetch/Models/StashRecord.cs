using System;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class StashRecord
    {
        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }

        [JsonPropertyName("lastCharacterName")]
        public string? LastCharacterName { get; set; }

        [JsonPropertyName("items")]
        public Item[] Items { get; set; } = Array.Empty<Item>();

        #nullable disable
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("league")]
        public string League { get; set; }

        [JsonPropertyName("stashType")]
        public string StashType { get; set; }

        [JsonPropertyName("stash")]
        public string Stash { get; set; }
        #nullable restore
    }

}

using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class RiverResponse
    {
        #nullable disable
        public string CurrentId { get; set; }

        [JsonPropertyName("next_change_id")]
        public string NextChangeId { get; set; }

        [JsonPropertyName("stashes")]
        public StashRecord[] Stashes { get; set; }
        #nullable restore
    }

}

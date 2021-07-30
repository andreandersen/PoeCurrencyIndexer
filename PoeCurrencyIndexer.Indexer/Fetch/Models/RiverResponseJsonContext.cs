using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    [JsonSerializable(typeof(ExtendedProperty))]
    [JsonSerializable(typeof(ItemProperty))]
    [JsonSerializable(typeof(UltimatumMod))]
    [JsonSerializable(typeof(ItemSocket))]
    [JsonSerializable(typeof(Item))]
    [JsonSerializable(typeof(StashRecord))]
    [JsonSerializable(typeof(RiverResponse))]
    public partial class RiverResponseJsonContext : JsonSerializerContext
    {

    }

}

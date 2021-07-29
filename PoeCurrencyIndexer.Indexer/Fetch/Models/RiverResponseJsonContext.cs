using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    [JsonSerializable(typeof(Item))]
    [JsonSerializable(typeof(ExtendedProperty))]
    [JsonSerializable(typeof(ItemProperty))]
    [JsonSerializable(typeof(StashRecord))]
    [JsonSerializable(typeof(RiverResponse))]
    public partial class RiverResponseJsonContext : JsonSerializerContext
    {

    }

}

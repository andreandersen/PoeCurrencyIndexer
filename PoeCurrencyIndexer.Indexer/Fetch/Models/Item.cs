using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class Item
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("w")]
        public uint Width { get; set; }

        [JsonPropertyName("h")]
        public uint Height { get; set; }

        [JsonPropertyName("support")]
        public bool? IsSupport { get; set; }

        [JsonPropertyName("influences")]
        public object? Influences { get; set; }

        [JsonPropertyName("elder")]
        public bool IsElder { get; set; }

        [JsonPropertyName("shaper")]
        public bool IsShaper { get; set; }

        [JsonPropertyName("abyssJewel")]
        public bool IsAbyssalJewel { get; set; }

        [JsonPropertyName("delve")]
        public bool IsDelve { get; set; }

        [JsonPropertyName("fractured")]
        public bool IsFractured { get; set; }

        [JsonPropertyName("synthesised")]
        public bool IsSynthesised { get; set; }

        [JsonPropertyName("sockets")]
        public ItemSocket[]? Sockets { get; set; }

        [JsonPropertyName("itemLevel")]
        public uint? ItemLevel { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("duplicated")]
        public bool IsDuplicated { get; set; }

        [JsonPropertyName("split")]
        public bool IsSplit { get; set; }

        [JsonPropertyName("corrupted")]
        public bool Corrupted { get; set; }

        [JsonPropertyName("talismanTier")]
        public int? TalismanTier { get; set; }

        [JsonPropertyName("frameType")]
        public FrameType FrameType { get; set; }

        [JsonPropertyName("stackSize")]
        public int? StackSize { get; set; }

        [JsonPropertyName("expicitMods")]
        public string[]? ExplicitMods { get; set; }

        [JsonPropertyName("utilityMods")]
        public string[]? UtilityMods { get; set; }
        
        [JsonPropertyName("implicitMods")]
        public string[]? ImplicitMods { get; set; }

        [JsonPropertyName("ultimatumMods")]
        public UltimatumMod[]? UltimatumMods { get; set; }

        [JsonPropertyName("craftedMods")]
        public string[]? CraftedMods { get; set; }

        [JsonPropertyName("enchantMods")]
        public string[]? EnchantMods { get; set; }

        [JsonPropertyName("properties")]
        public ItemProperty[]? Properties { get; set; }

        [JsonPropertyName("notableProperties")]
        public ItemProperty[]? NotableProperties { get; set; }

        [JsonPropertyName("requirements")]
        public ItemProperty[]? Requirements { get; set; }

        [JsonPropertyName("additionalProperties")]
        public ItemProperty[]? AdditionalProperties { get; set; }

        [JsonPropertyName("identified")]
        public bool Identified { get; set; }

        [JsonPropertyName("x")]
        public uint? X { get; set; }

        [JsonPropertyName("y")]
        public uint? Y { get; set; }
        
        [JsonPropertyName("inventoryId")]
        public string? InventoryId { get; set; }

        [JsonPropertyName("artFilename")]
        public string? ArtFileName { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty!;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty!;

        [JsonPropertyName("typeLine")]
        public string TypeLine { get; set; } = string.Empty!;

        [JsonPropertyName("baseType")]
        public string BaseType { get; set; } = string.Empty!;

        [AllowNull, JsonPropertyName("extended")]
        public ExtendedProperty Extended { get; set; }
    }

}

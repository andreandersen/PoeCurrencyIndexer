using System.Linq;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public static class ItemExtensions
    {
        public static bool IsMap(this Item item) =>
            item.Extended.Category == "maps" && ((item?.Properties?.Any(p => p.Name == "Map Tier")) ?? false);

        public static int GetMapTier(this Item item) =>
            int.Parse((string)item.Properties.First(p => p.Name == "Map Tier").Values[0][0]);

        public static string? GetBuyoutTag(Item item) => null;
    }

}

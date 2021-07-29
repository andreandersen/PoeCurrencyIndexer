using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models.TagLookups
{
    public class MapTagLookup : ItemTagLookup
    {
        public MapTagLookup(ILogger<IItemTagLookup> logger, ItemTagResult tags) : base(logger, tags)
        {
            logger.LogInformation("Hello world!");
        }

        private readonly static FrameType[] _compat = new[] {
            FrameType.Normal, FrameType.Magic, FrameType.Rare, FrameType.Unique };
        public override FrameType[] CompatibleFrameTypes => _compat;

        public override bool TryGet(Item item, out string result)
        {
            result = string.Empty;
            if (!item.IsMap())
                return false;

            if (item.BaseType.StartsWith("Blighted"))
            {
                if (Tags.TextToTag["BlightedMaps"].TryGetValue(item.BaseType, out var blightedBo))
                {
                    result = blightedBo.Id;
                    return true;
                }
                else
                {
                    Logger.LogWarning("Blighted map {BlightedMap} not found in tags", item.BaseType);
                }
            }

            var tier = item.GetMapTier();

            if (item.FrameType == FrameType.Unique)
            {
                if (Tags.TextToTag["MapsUnique"].TryGetValue($"{item.Name} (Tier {tier})", out var uniqueBo))
                {
                    result = uniqueBo.Id;
                    return true;
                }
                else
                {
                    Logger.LogWarning("Unique map {UniqueMap} not found in tags", item.Name + " - " + item.BaseType);
                }
            }

            var key = "MapsTier" + tier;
            if (Tags.TextToTag[key].TryGetValue($"{item.BaseType}", out var mapBo))
            {
                result = mapBo.Id;
                return true;
            }
            else
            {
                Logger.LogWarning("Map {MapType} not found in tags, tier {MapTier}", item.BaseType, tier);
            }

            return false;
        }
    }
}

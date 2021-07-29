using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models.TagLookups
{

    public interface IItemTagLookup
    {
        FrameType[] CompatibleFrameTypes { get; }
        bool TryGet(Item item, out string result);
    }

    public abstract class ItemTagLookup : IItemTagLookup
    {
        protected ILogger Logger;
        protected ItemTagResult Tags;

        public ItemTagLookup(ILogger logger, ItemTagResult tags)
        {
            Logger = logger;
            Tags = tags;
        }

        public abstract FrameType[] CompatibleFrameTypes { get; }

        public abstract bool TryGet(Item item, out string result);

        //protected bool TryGetFromCategory(Item item, ItemTagResult tags, string category, out string result)
        //{
        //    if (!tags.TextToTag.ContainsKey(category))
        //    {
        //        Logger.LogWarning("The category {Category} is not present", category);
        //        result = string.Empty;
        //        return false;
        //    }

        //    if (tags.TextToTag[category].TryGetValue(item.BaseType, out var boTag))
        //    {
        //        result = boTag.Id;
        //        return true;
        //    }

        //    result = string.Empty;
        //    return false;
        //}

    }
}

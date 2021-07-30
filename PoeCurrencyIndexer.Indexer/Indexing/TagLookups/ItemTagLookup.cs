using System;

using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Fetch.Models;
using PoeCurrencyIndexer.Indexer.Indexing.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing.TagLookups
{
    public interface IItemTagLookup
    {
        FrameType[] CompatibleFrameTypes { get; }
        bool TryGet(Item item, out string? result);
    }

    public abstract class ItemTagLookup : IItemTagLookup
    {
        protected ILogger Logger;
        protected ItemTagResultProvider TagsProvider;

        protected ItemTagResult Tags => 
            TagsProvider.Value ?? 
            throw new InvalidOperationException("ItemTags is not set");

        public ItemTagLookup(ILogger logger, ItemTagResultProvider tags)
        {
            Logger = logger;
            TagsProvider = tags;
        }

        public abstract FrameType[] CompatibleFrameTypes { get; }

        public abstract bool TryGet(Item item, out string? result);

        protected bool TryGetFromCategory(Item item, string category, out string result)
        {
            if (!Tags.TextToTag.ContainsKey(category))
            {
                Logger.LogWarning("The category {Category} is not present", category);
                result = string.Empty;
                return false;
            }

            if (Tags.TextToTag[category].TryGetValue(item.BaseType, out var boTag))
            {
                result = boTag.Id;
                return true;
            }

            result = string.Empty;
            return false;
        }

    }
}

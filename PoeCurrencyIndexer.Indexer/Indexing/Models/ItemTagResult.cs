using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models
{
    public class ItemTagResult
    {
        public List<ItemTagCategory> Result { get; }
        public Dictionary<string, Dictionary<string, ItemTag>> TextToTag { get; }
        public Dictionary<string, string> TagToText { get; }

        [JsonConstructor]
        public ItemTagResult(List<ItemTagCategory> result)
        {
            Result = result;

            TextToTag = result
                .ToDictionary(p => p.Id, p => p.Entries.ToDictionary(e => e.Text ?? string.Empty))!;

            TagToText = result
                .SelectMany(p => p.Entries)
                .ToDictionary(p => p.Id, p => p.Text ?? string.Empty);

        }

    }
}

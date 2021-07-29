using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models
{
    public class ItemTagCategory
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public List<ItemTag> Entries { get; private set; }

        [JsonConstructor]
        public ItemTagCategory(string id, string label, List<ItemTag> entries)
        {
            Id = id;
            Label = string.IsNullOrWhiteSpace(label) ? id : label;

            foreach (var item in entries)
            {
                item.Category = this;
            }

            Entries = entries;
        }

    }
}

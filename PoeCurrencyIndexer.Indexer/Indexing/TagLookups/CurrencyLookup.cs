using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Fetch.Models;
using PoeCurrencyIndexer.Indexer.Indexing.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing.TagLookups
{
    public class CurrencyLookup : ItemTagLookup
    {
        private readonly object _lock = new();
        private readonly string[] _categoriesToUse = new[] {
            "Currency", "Splinters", "Fragments", "Expedition",
            "DeliriumOrbs", "Catalysts", "Oils", "Incubators",
            "Scarabs", "DelveResonators", "DelveFossils",
            "Essences", "Misc" };

        private bool _isInitialized;
        private Dictionary<string, string>? _currencyLookup;
        private HashSet<string> _blackList = new HashSet<string>();

        public CurrencyLookup(ILogger<CurrencyLookup> logger, ItemTagResultProvider tagsProvider) :
            base(logger, tagsProvider) { }

        private void EnsureInitialized()
        {
            if (_isInitialized)
                return;

            lock (_lock)
            {
                _currencyLookup = Tags.Result
                    .Where(p => _categoriesToUse.Contains(p.Id))
                    .SelectMany(p => p.Entries)
                    .ToDictionary(p => p.Text!, p => p.Id);

                _isInitialized = true;
            }
        }

        public override FrameType[] CompatibleFrameTypes => new[] {
            FrameType.Currency, FrameType.Normal };

        public override bool TryGet(Item item, out string? result)
        {
            EnsureInitialized();
            
            if (!_currencyLookup!.TryGetValue(item.BaseType, out result))
            {
                if (!_blackList.Contains(item.BaseType) && (item.FrameType == FrameType.Currency || item.Extended.Category == "currency"))
                {
                    _blackList.Add(item.BaseType);
                    var subcats = (" " + string.Join(", ", item.Extended.SubCategories)).TrimEnd();
                    Logger.LogWarning("{BaseType} not found in tags with category {cat}{subcats}", item.BaseType, item.Extended.Category, subcats);
                }
                return false;
            }

            return true;
        }
    }
}

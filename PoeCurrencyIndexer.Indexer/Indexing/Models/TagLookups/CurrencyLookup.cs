using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing.Models.TagLookups
{
    public class CurrencyLookup : ItemTagLookup
    {

        public CurrencyLookup(ILogger logger, ItemTagResult tagResult) : base(logger, tagResult) { }

        public override FrameType[] CompatibleFrameTypes => new[] { FrameType.Currency };

        public override bool TryGet(Item item, out string result) => throw new System.NotImplementedException();

        //public override bool TryGet(Item item, ItemTagResult tags, out string result) =>
        //    TryGetFromCategory(item, tags, "Currency", out result);

    }
}

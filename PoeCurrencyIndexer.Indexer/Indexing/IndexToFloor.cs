using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Common;
using PoeCurrencyIndexer.Indexer.Fetch.Models;
using PoeCurrencyIndexer.Indexer.Indexing.TagLookups;

namespace PoeCurrencyIndexer.Indexer.Indexing
{
    public class IndexToFloor : HostedService
    {
        private readonly ILogger<IndexToFloor> _logger;
        private readonly ChannelReader<RiverResponse> _responses;
        private readonly IItemTagLookup[] _itemTagLookups;

        public IndexToFloor(
            ILogger<IndexToFloor> logger,
            ChannelReader<RiverResponse> responses,
            IEnumerable<IItemTagLookup> tagLookups)
        {
            _logger = logger;
            _responses = responses;
            _itemTagLookups = tagLookups.ToArray();
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var _ = _logger.BeginScope("INDEX");

            var lookupsPerFrameType = _itemTagLookups
                .SelectMany((e, p) => e.CompatibleFrameTypes, (e, p) => (e, p))
                .ToLookup(p => p.p, p => p.e)
                .ToDictionary(p => p.Key, p => p.ToList());

            while (!cancellationToken.IsCancellationRequested)
            {
                RiverResponse response;
                try { response = await _responses.ReadAsync(cancellationToken); } catch { break; }

                _logger.LogDebug("{numStashes} stashes discovered in {id}",
                    response.Stashes?.Length, response.CurrentId);

                if (response.Stashes == null || response.Stashes.Length == 0)
                    continue;

                foreach (var stash in response.Stashes)
                {
                    if (stash.League != "Expedition")
                        continue;

                    foreach (var item in stash.Items)
                    {
                        if (!item.Note.IsPriced || 
                            !lookupsPerFrameType.ContainsKey(item.FrameType))
                            continue;

                        foreach (var look in lookupsPerFrameType[item.FrameType])
                        {
                            if (look.TryGet(item, out var id))
                            {
                                _logger.LogDebug("{id} for {notePrice} {noteCurrency} ({orig})",
                                    id, item.Note.Price, item.Note.Currency, item.Note.Original);
                                
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
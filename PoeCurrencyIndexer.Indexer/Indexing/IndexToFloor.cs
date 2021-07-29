using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Common;
using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing
{
    public class IndexToFloor : HostedService
    {
        private readonly ILogger<IndexToFloor> _logger;
        private readonly ChannelReader<RiverResponse> _responses;
        private readonly BuyoutTagRetriever _buyoutTagRetriever;

        public IndexToFloor(
            ILogger<IndexToFloor> logger,
            ChannelReader<RiverResponse> responses)
        {
            _logger = logger;
            _responses = responses;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            //var tags = await _buyoutTagRetriever.GetTags();

            HashSet<string> cats = new HashSet<string>();

            using var _ = _logger.BeginScope("INDEX");
            while (!cancellationToken.IsCancellationRequested)
            {
                RiverResponse response;
                try { response = await _responses.ReadAsync(cancellationToken); } catch { break; }

                //_logger.LogInformation("{id} has {numStashes} stashes",
                //    response.CurrentId, response.Stashes?.Count);

                if (response.Stashes == null)
                    continue;

                foreach (var stash in response.Stashes)
                {
                    if (stash.League == "Expedition")
                    {
                        foreach (var item in stash.Items)
                        {
                            var cat = item.Extended.Category;
                            if (!cats.Contains(cat))
                            {
                                _logger.LogInformation(cat);
                                cats.Add(cat);
                            }
                            continue;

                            if (string.IsNullOrEmpty(item.Note))
                                item.Note = stash.Stash;

                            if (!string.IsNullOrEmpty(item.Note))
                                item.Note = item.Note.Trim();

                            if (string.IsNullOrEmpty(item.Note) || item.Note[0] != '~')
                                continue;

                            if (string.IsNullOrEmpty(item.Name))
                                _logger.LogInformation("{Type,-90} {note,33}",
                                    item.TypeLine, item.Note);
                            else
                                _logger.LogInformation("{Name,-45} {Type,-44} {note,33}",
                                    item.Name, item.TypeLine, item.Note);
                        }
                    }
                }
            }
        }
    }
}
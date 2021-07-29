using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Indexing.Models;

namespace PoeCurrencyIndexer.Indexer.Indexing
{
    public class BuyoutTagRetriever : IHostedService
    {
        private ItemTagResultProvider _tagResultProvider;
        private readonly HttpClient _http;
        private readonly ILogger _logger;

        public BuyoutTagRetriever(
            ILogger<BuyoutTagRetriever> logger,
            ItemTagResultProvider tagResultProvider,
            HttpClient http)
        {
            _logger = logger;
            _tagResultProvider = tagResultProvider;
            _http = http;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var _ = _logger.BeginScope("ITEMTAG");

            _logger.LogInformation("Getting Item tags...");

            var result = await _http.GetFromJsonAsync<ItemTagResult>(
                "https://www.pathofexile.com/api/trade/data/static");

            _tagResultProvider.SetResult(result!);
            _logger.LogInformation("Item tags retrieved");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

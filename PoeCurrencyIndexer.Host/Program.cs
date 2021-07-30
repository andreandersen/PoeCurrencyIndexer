using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PoeCurrencyIndexer.Host.Configuration;
using PoeCurrencyIndexer.Indexer;
using PoeCurrencyIndexer.Indexer.Fetch;
using PoeCurrencyIndexer.Indexer.Fetch.Models;
using PoeCurrencyIndexer.Indexer.Indexing;
using PoeCurrencyIndexer.Indexer.Indexing.Models;
using PoeCurrencyIndexer.Indexer.Indexing.TagLookups;

namespace PoeCurrencyIndexer.Host
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureDefaults(args)
                .ConfigureAppConfiguration(cb => {
                    _ = cb.AddUserSecrets<Program>(true);
                })
                .ConfigureLogging()
                .ConfigureServices((ctx, services) => ConfigureServices(ctx, services));

            await hostBuilder.RunConsoleAsync();
        }

        private static void ConfigureServices(
            HostBuilderContext ctx, IServiceCollection services)
        {
            var config = ctx.Configuration;

            _ = services
                .AddOptions()
                .Configure<ClientCredentials>(config.GetSection(nameof(ClientCredentials)))

                .AddHttpClient()

                .AddSingleton<AuthorizationHeaderProvider>()

                .AddSingleton(_ => Channel.CreateBounded<RiverResponse>(32))
                .AddSingleton(s => s.GetRequiredService<Channel<RiverResponse>>().Reader)
                .AddSingleton(s => s.GetRequiredService<Channel<RiverResponse>>().Writer)

                .AddSingleton(_ => Channel.CreateBounded<ChangeId>(32))
                .AddSingleton(s => s.GetRequiredService<Channel<ChangeId>>().Writer)
                .AddSingleton(s => s.GetRequiredService<Channel<ChangeId>>().Reader)

                .AddSingleton<ILastChangeIdReader, LastChangeIdFromPoeNinja>()
                .AddSingleton<ItemTagResultProvider>()
                .AddHostedService<ItemTagRetriever>()

                .AddSingleton(typeof(IItemTagLookup), typeof(CurrencyLookup))
                .AddSingleton(typeof(IItemTagLookup), typeof(MapTagLookup))

                // Hosted Services
                .AddHostedService<Fetcher>()
                .AddHostedService<IndexToFloor>();
        }
    }
}
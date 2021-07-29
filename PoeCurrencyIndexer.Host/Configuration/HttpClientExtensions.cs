using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace PoeCurrencyIndexer.Host.Configuration
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClient(this IServiceCollection services)
        {
            return services.AddSingleton(sp => {

                var httpClient = new HttpClient(new BrotliHandler())
                {
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    DefaultRequestVersion = HttpVersion.Version20
                };
                _ = httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent", "Exalter Indexer, thezensei@gmail.com");

                return httpClient;

            });
        }
    }
}

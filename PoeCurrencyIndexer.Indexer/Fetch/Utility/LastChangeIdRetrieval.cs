using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PoeCurrencyIndexer.Indexer.Fetch
{
    public interface ILastChangeIdReader
    {
        Task<string> GetAsync();
    }
    public class LastChangeIdFromPoeNinja : ILastChangeIdReader
    {
        private readonly HttpClient _http;
        public LastChangeIdFromPoeNinja(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetAsync()
        {
            var json = await _http.GetStringAsync(
                "https://poe.ninja/api/Data/GetStats");

            var jsonElement = JsonSerializer
                .Deserialize<JsonElement>(json);

            if (!jsonElement.TryGetProperty("next_change_id", out var val))
            {
                // TODO: Improve exception
                throw new InvalidOperationException(
                    "Could not retrieve next change id from Poe.ninja result");
            }

            return val.GetString()!;
        }
    }
}

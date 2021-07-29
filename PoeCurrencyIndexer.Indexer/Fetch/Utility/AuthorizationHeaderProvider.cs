using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PoeCurrencyIndexer.Indexer.Fetch
{
    public class AuthorizationHeaderProvider
    {
        private readonly IOptions<ClientCredentials> _creds;
        private readonly HttpClient _http;
        private readonly ILogger _logger;

        public AuthorizationHeaderProvider(
            IOptions<ClientCredentials> credentials,
            HttpClient http, ILogger<AuthorizationHeaderProvider> logger)
        {
            _creds = credentials;
            _http = http;
            _logger = logger;
        }

        public async Task<AuthenticationHeaderValue> GetAsync()
        {
            if (_creds == null || _creds.Value == null ||
                string.IsNullOrEmpty(_creds.Value.ClientSecret) ||
                string.IsNullOrEmpty(_creds.Value.ClientId))
            {
                _logger.LogWarning("Client Credentials not found. Using no authorization");
                return new AuthenticationHeaderValue(null!);
            }

            var e = new FormUrlEncodedContent(new List<KeyValuePair<string?, string?>>
            {
                new("client_id", _creds.Value.ClientId),
                new("client_secret", _creds.Value.ClientSecret),
                new("grant_type", "client_credentials"),
                new("scope", "service:psapi")
            });

            using var response = await _http.PostAsync("https://www.pathofexile.com/oauth/token", e);
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            var bearer = result.GetProperty("access_token").GetString();
            return new AuthenticationHeaderValue("Bearer", bearer);
        }
    }
}

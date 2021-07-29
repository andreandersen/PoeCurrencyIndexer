using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PoeCurrencyIndexer.Indexer.Common;
using PoeCurrencyIndexer.Indexer.Fetch;
using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer
{
    public class Fetcher : BackgroundService, IFetcher
    {
        private const string BaseUrl =
            "https://api.pathofexile.com/public-stash-tabs?id=";

        private DateTime _lastAccess = default;
        private readonly static TimeSpan TimeBetweenRequests =
            TimeSpan.FromMilliseconds(510);

        private readonly ILogger<Fetcher> _logger;
        private readonly HttpClient _http;
        private readonly ILastChangeIdReader _lastChangeReader;
        private readonly ChannelWriter<RiverResponse> _riverWriter;
        private readonly ChannelWriter<ChangeId> _changeIdWriter;
        private readonly ChannelReader<ChangeId> _changeIdReader;

        public Fetcher(
            HttpClient httpClient, ILogger<Fetcher> logger,
            ChannelWriter<RiverResponse> riverWriter,
            ChannelWriter<ChangeId> changeIdWriter,
            ChannelReader<ChangeId> changeIdReader,
            ILastChangeIdReader lastChangeReader)
        {
            _logger = logger;
            _http = httpClient;
            _riverWriter = riverWriter;
            _changeIdWriter = changeIdWriter;
            _changeIdReader = changeIdReader;
            _lastChangeReader = lastChangeReader;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _ = _logger.BeginScope("FETCH");

                var nextId = await _lastChangeReader.GetAsync();
                _ = _changeIdWriter.TryWrite(nextId);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var id = await _changeIdReader.ReadAsync(stoppingToken);

                    await HandleWait(stoppingToken);

                    var response = await Connect(id);

                    _ = GetChanges(response, id);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(exception: ex, ex.Message);
                _logger.LogWarning("Exiting fetcher...");
            }
        }

        private async Task HandleWait(CancellationToken stoppingToken)
        {
            if (_lastAccess == default)
                return;

            var sinceLast = DateTime.Now.Subtract(_lastAccess).TotalMilliseconds;
            //_logger.LogDebug("Dispatch: Time since last request is {sinceLast}",
            //    Convert.ToInt32(sinceLast));

            var wait = Convert.ToInt32(TimeBetweenRequests.TotalMilliseconds - sinceLast);
            if (wait > 0)
            {
                //_logger.LogDebug("Dispatch: Waiting for {wait} ms...", wait);
                await Task.Delay(wait, stoppingToken);
            }
        }

        private async Task<HttpResponseMessage> Connect(string id)
        {
            var sw = Stopwatch.StartNew();
            //_logger.LogDebug("Connecting - {id}", id);
            _lastAccess = DateTime.Now.AddMilliseconds(150);

            var response = await _http.GetAsync(BaseUrl + id,
                HttpCompletionOption.ResponseHeadersRead);

            _logger.LogDebug("Connected - {id}, took {elapsed} ms",
                id, sw.RestartAndGetElapsedMs());

            //_logger.LogInformation("Rate-limit state: {state} ({limit})",
            //    response.Headers.NonValidated["X-Rate-Limit-Ip-State"].ToString(),
            //    response.Headers.NonValidated["X-Rate-Limit-Ip"].ToString());

            // TODO: Add 429-handling etc
            if (!response.IsSuccessStatusCode)
                throw new Exception($"No good response");

            return response;
        }

        private async Task GetChanges(HttpResponseMessage response, string id)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                using var sourceStream = await response.Content.ReadAsStreamAsync();
                using var memStream = new MemoryStream();

                var nextId = ReadInitial(sourceStream, memStream);
                await _changeIdWriter.WriteAsync(nextId);

                _logger.LogDebug("Found next - {id}, took {elapsed} ms",
                    id, sw.RestartAndGetElapsedMs(), nextId);

                await sourceStream.CopyToAsync(memStream);

                _logger.LogDebug("Downloaded - {id}, took {elapsed} ms [{totalSize} KiB]",
                    id, sw.RestartAndGetElapsedMs(), Convert.ToInt32(memStream.Length / 1024f));

                sw.Restart();
                var riverResponse = memStream.ToRiverResponse(id);

                _logger.LogDebug("Parsed - {id}, took {elapsed} ms [{size} KiB]",
                    id, sw.RestartAndGetElapsedMs(), Convert.ToInt32(memStream.Length / 1024f));

                await _riverWriter.WriteAsync(riverResponse);
            }
            finally
            {
                response.Dispose();
            }
        }

        private static string ReadInitial(
            Stream stream, MemoryStream outputStream)
        {
            Span<byte> buffer = stackalloc byte[128];
            var read = stream.Read(buffer);
            buffer = buffer.Slice(0, read);

            outputStream.Write(buffer);
            return buffer.ParseNextChangeId();
        }

    }
}


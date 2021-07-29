using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PoeCurrencyIndexer.Host
{
public class BrotliHandler : DelegatingHandler
{
    public BrotliHandler() : base(new SocketsHttpHandler() { EnableMultipleHttp2Connections = true }) { }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "br");

        var response = await base.SendAsync(request, cancellationToken);

        if (response.Content.Headers.TryGetValues("Content-Encoding", out var ce)
            && ce.First() == "br")
        {
            var encodedStream = await response.Content
                .ReadAsStreamAsync(cancellationToken);

            response.Content = new StreamContent(
                new BrotliStream(encodedStream, CompressionMode.Decompress));
        }

        return response;
    }
}

}

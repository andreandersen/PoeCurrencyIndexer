using System;
using System.IO;
using System.Text;
using System.Text.Json;

using PoeCurrencyIndexer.Indexer.Fetch.Models;

namespace PoeCurrencyIndexer.Indexer.Common
{
    public static class ParseExtensions
    {
        internal readonly static JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true
        };

        private readonly static byte Colon = (byte)':';
        private readonly static byte Quote = (byte)'"';

        internal static string ParseNextChangeId(this Span<byte> span)
        {
            var colonIndex = span.IndexOf(Colon);

            if (colonIndex == -1)
                throw new Exception("Invalid data while parsing next change id");

            span = span[(colonIndex + 2)..];
            var quoteIndex = span.IndexOf(Quote);

            if (quoteIndex == -1)
                throw new Exception("Invalid data while parsing next change id");

            span = span.Slice(0, quoteIndex);
            return Encoding.UTF8.GetString(span);
        }

        internal static RiverResponse ToRiverResponse(this MemoryStream input, string currentId)
        {
            var buffer = input.GetBuffer().AsSpan().Slice(0, (int)input.Length);
            var response = (RiverResponse)JsonSerializer.Deserialize(
                buffer, typeof(RiverResponse), RiverResponseJsonContext.Default)!;
            response.CurrentId = currentId;

            return response;
        }
    }
}

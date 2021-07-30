using FluentAssertions;

using PoeCurrencyIndexer.Indexer;
using PoeCurrencyIndexer.Indexer.Fetch.Models;

using System;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace PoeCurrencyIndexer.Tests
{
    public class ParseTests
    {
        [Fact]
        public void Test1()
        {
            //// 794225848-807323323-771171918-871117731-831770329
            //using var json1 = File.OpenRead(@"TestData\public-stash-tabs-1.json");
            //var parsed = await new Fetcher().Parse(json1);

            //parsed.NextChangeId.Should().Be("794225848-807323323-771171918-871117731-831770329");


            var json1 = File.ReadAllText(@"TestData\simple-item.json");
            _ = System.Text.Json.JsonSerializer.Deserialize<Item>(json1);
        }
    }
}

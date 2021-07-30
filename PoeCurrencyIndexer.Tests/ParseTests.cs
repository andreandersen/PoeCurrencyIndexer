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
        public void Can_Deserialize_Item()
        {
            var json1 = File.ReadAllText(@"TestData\simple-item.json");
            _ = System.Text.Json.JsonSerializer.Deserialize<Item>(json1);
        }

        [Fact]
        public void Can_Deserialize_Response()
        {
            var json1 = File.ReadAllText(@"TestData\public-stash-tabs-1.json");
            _ = System.Text.Json.JsonSerializer.Deserialize<RiverResponse>(json1);
        }

        [Theory]
        [InlineData("~price  3.45  chaos pepe", 3.45, "chaos")]
        [InlineData("~b/o 1.24 chaos", 1.24, "chaos")]
        [InlineData("~price 1.24 chaos", 1.24, "chaos")]
        [InlineData("~price 1/2 chaos", 0.5, "chaos")]
        public void Can_Interpret_Price(string stringNote, decimal expectedPrice, string expectedCurrency)
        {
            Note note = stringNote;
            
            note.Price.Should().Be(expectedPrice);
            note.Currency.Should().Be(expectedCurrency);
        }
    }
}

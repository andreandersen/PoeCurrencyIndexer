using System;

using Microsoft.Toolkit.HighPerformance;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class Note
    {
        public readonly static Note Empty = new();

        public string Original { get; }
        
        public bool IsPriced { get; }
        public bool IsFractional { get; }

        public decimal? Price { get; }
        public string? Currency { get; }

        public static Note Create(ReadOnlySpan<char> note) =>
            note == null || note.Length == 0 ? Empty : new Note(note);

        private Note(ReadOnlySpan<char> note)
        {
            Original = note.ToString();

            if (note[0] != '~')
                return;

            var tokenized = note.Tokenize(' ');
            var word = 0;
            
            ReadOnlySpan<char> priceWord = default;
            ReadOnlySpan<char> currencyWord = default;

            foreach (var token in tokenized)
            {
                if (token.Length == 0)
                    continue;

                if (word == 1)
                    priceWord = token;
                else if (word == 2)
                    currencyWord = token;
               
                word++;

                if (word > 2)
                    break;
            }

            if (priceWord != default && currencyWord != default)
            {
                Currency = currencyWord.ToString().ToLowerInvariant();

                // fractional pricing
                var slashIdx = priceWord.IndexOf('/');
                if (slashIdx > 0)
                {
                    var x1 = priceWord[0..slashIdx];
                    var x2 = priceWord[(slashIdx + 1)..];
                    
                    if (int.TryParse(x1, out var n1) && 
                        int.TryParse(x2, out var n2) &&
                        n2 > 0 && n1 > 0)
                    {
                        Price = n1 / (decimal)n2;
                        IsPriced = true;
                        IsFractional = true;
                    }

                } else if (decimal.TryParse(priceWord, out var priceDecimal) &&
                    priceDecimal > 0)
                {
                    Price = priceDecimal;
                    IsPriced = true;
                }
            }
        }

        private Note()
        {
            Original = string.Empty;
        }

        public static implicit operator Note(string note) => Note.Create(note);
        public static implicit operator string(Note note) => note.Original;
    }
}

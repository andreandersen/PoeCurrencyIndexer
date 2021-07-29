using System;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public struct ChangeId
    {
        private string _value;
        public ChangeId(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            _value = value;
        }

        public static implicit operator ChangeId(string val) => new ChangeId(val);
        public static implicit operator string(ChangeId val) => val._value;
    }
}

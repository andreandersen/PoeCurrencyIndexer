namespace PoeCurrencyIndexer.Indexer.Indexing.Models
{
    public class ItemTagResultProvider
    {
        public ItemTagResult? Value { get; private set;  }

        public void SetResult(ItemTagResult result) => Value = result;
    }
}

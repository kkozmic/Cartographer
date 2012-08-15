namespace Cartographer.Internal.Collections
{
	public class KeyedValue<TKey, TValue>
	{
		public KeyedValue(TKey key)
		{
			Key = key;
		}

		public TKey Key { get; private set; }

		public TValue Value { get; private set; }

		public void UpdateValue(TValue newValue)
		{
			Value = newValue;
		}
	}
}
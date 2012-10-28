namespace Cartographer.Internal.Collections
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Cartographer.Internal.Extensions;

	public class OrderedKeyedCollection<TKey, TValue>: IEnumerable<TValue>
	{
		readonly KeyedValue<TKey, TValue>[] items;

		public OrderedKeyedCollection(TKey[] keys)
		{
			items = CollectionsUtil.ConvertAll(keys, k => new KeyedValue<TKey, TValue>(k));
		}

		public IEnumerable<KeyedValue<TKey, TValue>> ByKey
		{
			get { return items; }
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return items.Select(item => item.Value).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
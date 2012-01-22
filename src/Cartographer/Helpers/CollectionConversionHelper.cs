namespace Cartographer.Helpers
{
	using System.Collections;
	using System.Collections.Generic;

	public static class CollectionConversionHelper
	{
		public static TOut[] MapCollection<TOut>(IEnumerable collection, MappingContext context)
		{
			if (collection == null)
			{
				return null;
			}
			var mapper = context.Mapper;
			var results = new List<TOut>();
			foreach (var item in collection)
			{
				var mapped = mapper.Convert<TOut>(item);
				results.Add(mapped);
			}
			return results.ToArray();
		}
	}
}
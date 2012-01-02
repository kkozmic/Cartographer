namespace Cartographer.Helpers
{
	using System.Collections;
	using System.Collections.Generic;

	public class CollectionConversionHelper
	{
		public static TOut[] MapCollection<TOut>(IEnumerable collection, MappingContext context)
		{
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
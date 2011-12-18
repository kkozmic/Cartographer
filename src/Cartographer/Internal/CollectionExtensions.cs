namespace Cartographer.Internal
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CollectionExtensions
	{
		static readonly Type[] SupportedGenericCollections = new[]
		                                                     {
		                                                     	typeof (IEnumerable<>),
		                                                     	typeof (ICollection<>),
		                                                     	typeof (IList<>),
		                                                     };

		public static bool Contains<T>(this T[] array, T item)
		{
			return Array.IndexOf(array, item) >= 0;
		}

		public static Type GetArrayItemType(this Type type)
		{
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			if (type.IsGenericType == false)
			{
				return null;
			}
			var definition = type.GetGenericTypeDefinition();
			if (SupportedGenericCollections.Contains(definition) == false)
			{
				return null;
			}
			return type.GetGenericArguments().Single();
		}
	}
}
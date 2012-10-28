namespace Cartographer.Internal.Extensions
{
	using System;

	public static class CollectionsUtil
	{
		public static TOut[] ConvertAll<TIn, TOut>(TIn[] array, Func<TIn, TOut> converter)
		{
			var length = array.Length;
			var result = new TOut[length];
			for (var i = 0; i < length; i++)
			{
				result[i] = converter(array[i]);
			}
			return result;
		}
	}
}
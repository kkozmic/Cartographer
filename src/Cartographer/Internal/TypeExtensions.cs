namespace Cartographer.Internal
{
	using System;

	public static class TypeExtensions
	{
		public static bool IsNullable(this Type type)
		{
			if (type.IsValueType)
			{
				return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
			}
			return true;
		}
	}
}
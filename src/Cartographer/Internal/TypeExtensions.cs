namespace Cartographer.Internal
{
	using System;

	public static class TypeExtensions
	{
		public static bool Is<TOther>(this Type type)
		{
			return type.Is(typeof (TOther));
		}

		public static bool Is(this Type type, Type other)
		{
			return other.IsAssignableFrom(type);
		}

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
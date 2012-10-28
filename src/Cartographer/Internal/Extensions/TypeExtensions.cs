namespace Cartographer.Internal.Extensions
{
	using System;
	using System.Linq;
	using System.Reflection;

	public static class TypeExtensions
	{
		public static Type[] EmptyTypes = new Type[0];

		public static Type GetInterface(this Type type, Type @interface)
		{
			var allInterfaces = type.GetInterfaces();
			try
			{
				return allInterfaces.SingleOrDefault(i => string.Equals(i.Name, @interface.Name, StringComparison.OrdinalIgnoreCase) &&
				                                          string.Equals(i.Namespace, @interface.Namespace, StringComparison.OrdinalIgnoreCase));
			}
			catch (InvalidOperationException e)
			{
				throw new AmbiguousMatchException(string.Format("More than one interface matched for {0} on type {1}", @interface, type), e);
			}
		}

		public static bool Is<TOther>(this Type type)
		{
			return type.Is(typeof (TOther));
		}

		public static bool Is(this Type type, Type other)
		{
			return other.IsAssignableFrom(type);
		}

		public static bool IsGenericInterface(this Type type, Type @interface)
		{
			return type.Is(@interface) || type.GetInterface(@interface) != null;
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
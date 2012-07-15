namespace Cartographer.Internal
{
	using System.Reflection;

	public static class PropertyInfoExtenions
	{
		public static bool IsWriteable(this PropertyInfo property)
		{
			var setter = property.GetSetMethod(true);
			if (setter == null)
			{
				return false;
			}
			// TODO: we'll also need to consider internal setter where InternalsVisibleTo has been set
			// this is probably as post-v1 feature though.
			return setter.IsPublic;
		}
	}
}
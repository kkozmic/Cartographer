namespace Cartographer.Internal
{
	using System.Reflection;

	public static class MappingContextMeta
	{
		public static readonly MethodInfo Argument = typeof (MappingContext).GetMethod("Argument");

		public static readonly PropertyInfo Mapper = typeof (MappingContext).GetProperty("Mapper");

		public static readonly PropertyInfo SourceInstance = typeof (MappingContext).GetProperty("SourceInstance");

		public static readonly PropertyInfo TargetInstance = typeof (MappingContext).GetProperty("TargetInstance");
	}
}
namespace Cartographer.Internal
{
	using System.Reflection;

	public static class MappingContextMeta
	{
		public static readonly PropertyInfo Mapper = typeof (MappingContext).GetProperty("Mapper");
		public static readonly PropertyInfo SourceInstance = typeof (MappingContext).GetProperty("SourceInstance");
	}
}

namespace Cartographer.Internal.Extensions
{
	using System;
	using System.Linq;
	using System.Reflection;

	public static class AssemblyExtensions
	{
		public static Type[] GetAvailableTypes(this Assembly assembly)
		{
			try
			{
				return assembly.GetExportedTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null).ToArray();
			}
		}
	}
}
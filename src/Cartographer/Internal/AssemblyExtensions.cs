namespace Cartographer.Internal
{
	using System;
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
				return Array.FindAll(e.Types, t => t != null);
			}
		}
	}
}
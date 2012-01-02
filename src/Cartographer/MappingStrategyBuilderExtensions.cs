namespace Cartographer
{
	using System;
	using System.ComponentModel;
	using Cartographer.Internal;
	using Cartographer.Patterns;
	using Cartographer.Steps;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class MappingStrategyBuilderExtensions
	{
		public static void AddConversionPatternType<TConversionPattern>(this IMapperBuilderSettings settings, Func<MappingStep, bool> predicate)
		{
			AddConversionPatternType(settings, typeof (TConversionPattern), predicate);
		}

		public static void AddConversionPatternType(this IMapperBuilderSettings settings, Type type, Func<MappingStep, bool> predicate)
		{
			settings.AddConversionPattern(new DelegatingConversionPattern(type, predicate, null));
		}

		public static void AddConversionPatternType(this IMapperBuilderSettings settings, Type patternTypeOpenGeneric, Func<MappingStep, bool> predicate, Func<MappingStep, Type[]> getGenericArguments)
		{
			settings.AddConversionPattern(new DelegatingConversionPattern(patternTypeOpenGeneric, predicate, getGenericArguments));
		}
	}
}
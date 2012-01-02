namespace Cartographer.Patterns
{
	using System;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class DelegatingConversionPattern: IConversionPattern
	{
		readonly Func<MappingStep, Type[]> genericArguments;

		readonly Type patternType;

		readonly Func<MappingStep, bool> predicate;

		public DelegatingConversionPattern(Type patternType, Func<MappingStep, bool> predicate, Func<MappingStep, Type[]> genericArguments)
		{
			this.predicate = predicate;
			this.patternType = patternType;
			this.genericArguments = genericArguments;
		}

		public void Apply(MappingStep mapping)
		{
			if (predicate(mapping))
			{
				var pattern = CreatePatternInstance(mapping);
				pattern.Apply(mapping);
			}
		}

		IConversionPattern CreatePatternInstance(MappingStep mapping)
		{
			var type = patternType;
			if (genericArguments != null)
			{
				type = type.MakeGenericType(genericArguments(mapping));
			}
			return (IConversionPattern)Activator.CreateInstance(type);
		}
	}
}
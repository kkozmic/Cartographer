namespace Cartographer.Compiler
{
	using System;
	using Cartographer.Patterns;
	using Cartographer.Steps;

	public class MappingStrategyBuilder: IMappingStrategyBuilder
	{
		readonly MappingConverter[] conversionPatterns;

		readonly IMappingDescriptor descriptor;

		readonly IMappingPattern[] mappingPatterns;

		public MappingStrategyBuilder(IMappingDescriptor descriptor, MappingConverter[] conversionPatterns, params IMappingPattern[] mappingPatterns)
		{
			this.descriptor = descriptor;
			this.conversionPatterns = conversionPatterns;
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(Type source, Type target)
		{
			var strategy = new MappingStrategy(source, target, descriptor);
			foreach (var pattern in mappingPatterns)
			{
				pattern.Contribute(strategy);
			}
			foreach (var mappingStep in strategy.MappingSteps)
			{
				ApplyConverter(mappingStep);
			}
			return strategy;
		}

		void ApplyConverter(MappingStep mapping)
		{
			foreach (var pattern in conversionPatterns)
			{
				pattern.Apply(mapping);
			}
		}
	}
}
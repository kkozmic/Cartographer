namespace Cartographer
{
	using Cartographer.Steps;

	public class MappingBuilder: IMappingBuilder
	{
		readonly IMappingDescriptor descriptor;

		readonly IConversionPattern[] conversionPatterns;

		readonly IMappingPattern[] mappingPatterns;

		public MappingBuilder(IMappingDescriptor descriptor, IConversionPattern[] conversionPatterns, params IMappingPattern[] mappingPatterns)
		{
			this.descriptor = descriptor;
			this.conversionPatterns = conversionPatterns;
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(TypeModel source, TypeModel target)
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
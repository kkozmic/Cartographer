namespace Cartographer
{
	using Cartographer.Steps;

	public class MappingBuilder: IMappingBuilder
	{
		readonly IConversionPattern[] conversionPatterns;
		readonly IMappingPattern[] mappingPatterns;

		public MappingBuilder(IConversionPattern[] conversionPatterns, params IMappingPattern[] mappingPatterns)
		{
			this.conversionPatterns = conversionPatterns;
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(TypeModel source, TypeModel target)
		{
			var strategy = new MappingStrategy(source, target);
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

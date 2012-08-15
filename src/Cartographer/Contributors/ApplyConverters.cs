namespace Cartographer.Contributors
{
	using Cartographer.Compiler;

	public class ApplyConverters: IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			foreach (var mappingStep in strategy.MappingSteps)
			{
				mappingStep.Conversion = context.ApplyConverter(mappingStep, withFallback: true);
			}
		}
	}
}
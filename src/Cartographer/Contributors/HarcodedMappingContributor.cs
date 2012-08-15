namespace Cartographer.Contributors
{
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class HarcodedMappingContributor: IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			var directMappingStep = new DirectMappingStep(strategy.Source, strategy.Target);
			var converter = context.ApplyConverter(directMappingStep, withFallback: false);
			if (converter != null)
			{
				directMappingStep.Conversion = converter;
				strategy.InitTargetStep = directMappingStep;
				context.MarkAsCompleted();
			}
		}
	}
}
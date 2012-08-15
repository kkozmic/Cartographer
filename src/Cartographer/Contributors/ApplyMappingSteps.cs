namespace Cartographer.Contributors
{
	using Cartographer.Compiler;

	public class ApplyMappingSteps: IMappingStrategyContributor
	{
		readonly IMappingPattern[] mappingPatterns;

		public ApplyMappingSteps(IMappingPattern[] mappingPatterns)
		{
			this.mappingPatterns = mappingPatterns;
		}

		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			foreach (var pattern in mappingPatterns)
			{
				pattern.Contribute(strategy);
			}
		}
	}
}
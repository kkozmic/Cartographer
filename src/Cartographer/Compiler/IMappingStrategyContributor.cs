namespace Cartographer.Compiler
{
	public interface IMappingStrategyContributor
	{
		void Apply(MappingStrategy strategy, MappingStrategyBuildContext context);
	}
}
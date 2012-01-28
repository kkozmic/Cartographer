namespace Cartographer.Compiler
{
	public interface IMappingStrategyBuilder
	{
		MappingStrategy BuildMappingStrategy(MappingInfo mappingInfo);
	}
}
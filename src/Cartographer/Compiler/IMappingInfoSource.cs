namespace Cartographer.Compiler
{
	public interface IMappingInfoSource
	{
		MappingInfo GetMappingInfo(MappingRequest request);
	}
}
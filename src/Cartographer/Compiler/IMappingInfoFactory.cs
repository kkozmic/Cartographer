namespace Cartographer.Compiler
{
	public interface IMappingInfoFactory
	{
		MappingInfo GetMappingInfo(MappingRequest request);
	}
}
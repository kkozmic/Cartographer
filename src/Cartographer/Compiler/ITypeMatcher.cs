namespace Cartographer.Compiler
{
	public interface ITypeMatcher
	{
		MappingInfo Match(MappingRequest request);
	}
}
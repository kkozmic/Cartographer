namespace Cartographer.Compiler
{
	public interface IMappingPattern
	{
		void Contribute(MappingStrategy strategy);
	}
}
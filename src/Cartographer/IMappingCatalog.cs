namespace Cartographer
{
	using Cartographer.Compiler;

	public interface IMappingCatalog
	{
		void Collect(IIMappingBag mappings);
	}
}
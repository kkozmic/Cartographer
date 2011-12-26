namespace Cartographer.Compiler
{
	using Cartographer.Steps;

	public interface IConversionPattern
	{
		void Apply(MappingStep mapping);
	}
}
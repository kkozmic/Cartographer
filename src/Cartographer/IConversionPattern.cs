namespace Cartographer
{
	using Cartographer.Steps;

	public interface IConversionPattern
	{
		void Apply(MappingStep mapping);
	}
}

namespace Cartographer.Contributors
{
	using Cartographer.Compiler;

	public class DescriptorContributor: IMappingStrategyContributor
	{
		readonly IMappingDescriptor descriptor;

		public DescriptorContributor(IMappingDescriptor descriptor)
		{
			this.descriptor = descriptor;
		}

		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			strategy.Descriptor = descriptor;
		}
	}
}
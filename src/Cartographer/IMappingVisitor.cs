namespace Cartographer
{
	using Cartographer.Steps;

	public interface IMappingVisitor<TStep> where TStep: MappingStep
	{
		void Visit(TStep step, MappingContext context);
	}
}

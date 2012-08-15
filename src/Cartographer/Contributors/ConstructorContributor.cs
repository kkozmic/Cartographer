namespace Cartographer.Contributors
{
	using System.Linq;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal.Collections;
	using Cartographer.Steps;

	public class ConstructorContributor: IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			if (strategy.HasTargetInstance)
			{
				return;
			}
			var ctors = strategy.Target.GetConstructors();
			if (ctors.Length != 1)
			{
				return;
			}
			strategy.TargetConstructor = ctors.Single();
			strategy.ConstructorParameterMappingSteps = new OrderedKeyedCollection<ParameterInfo, MappingStep>(strategy.TargetConstructor.GetParameters());
		}
	}
}
namespace CartographerTests.MappingPatterns
{
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Steps;

	public class MapMissingCtorParametersFromCallSite: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			if (strategy.ConstructorParameterMappingSteps == null)
			{
				return;
			}
			foreach (var mappingStep in strategy.ConstructorParameterMappingSteps.ByKey)
			{
				if (mappingStep.Value == null)
				{
					var assign = new SimpleStep(mappingStep.Key.ParameterType, mappingStep.Key.ParameterType,
					                            (s, c) => Expression.Call(s.ContextExpression, MappingContextMeta.Argument.MakeGenericMethod(mappingStep.Key.ParameterType)));
					mappingStep.UpdateValue(assign);
				}
			}
		}
	}
}
namespace Cartographer.Patterns
{
	using System;
	using System.Linq;
	using Cartographer.Compiler;
	using Cartographer.Internal.Extensions;
	using Cartographer.Steps;

	public class MatchByNameMappingPattern: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			var properties = strategy.Source.GetProperties();
			foreach (var targetProperty in strategy.Target.GetProperties().Where(p => p.IsWriteable()))
			{
				var sourceProperty = properties.FirstOrDefault(p => p.Name == targetProperty.Name);
				if (sourceProperty != null)
				{
					var assign = new Assign(targetProperty, sourceProperty);

					strategy.AddMappingStep(assign);
				}
			}
			if (strategy.HasTargetInstance)
			{
				return;
			}
			foreach (var mappingStep in strategy.ConstructorParameterMappingSteps.ByKey)
			{
				if (mappingStep.Value == null)
				{
					var sourceProperty = properties.FirstOrDefault(p => string.Equals(p.Name, mappingStep.Key.Name, StringComparison.OrdinalIgnoreCase));
					if (sourceProperty != null)
					{
						var assign = new ConstructorAssign(mappingStep.Key, sourceProperty);
						mappingStep.UpdateValue(assign);
					}
				}
			}
		}
	}
}
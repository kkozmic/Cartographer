namespace Cartographer.Patterns
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Internal.Extensions;
	using Cartographer.Steps;

	public class MatchByNameFlattenMappingPattern: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			var sourceProperties = strategy.Source.GetProperties();
			foreach (var targetProperty in strategy.Target.GetProperties().Where(p => p.IsWriteable()))
			{
				var sourcePropertyChain = BuildPropertyChain(targetProperty.Name, sourceProperties);
				if (sourcePropertyChain.Length > 1)
				{
					strategy.AddMappingStep(new AssignChain(targetProperty, sourcePropertyChain));
				}
			}
			foreach (var mappingStep in strategy.ConstructorParameterMappingSteps.ByKey)
			{
				if (mappingStep.Value == null)
				{
					var targetParameter = mappingStep.Key;
					var sourcePropertyChain = BuildPropertyChain(targetParameter.Name, sourceProperties);
					if (sourcePropertyChain.Length > 1)
					{
						mappingStep.UpdateValue(new ConstructorAssignChain(mappingStep.Key, sourcePropertyChain));
					}
				}
			}
		}

		static PropertyInfo[] BuildPropertyChain(string targetName, PropertyInfo[] sourceProperties)
		{
			var name = targetName;
			var currentIndex = 0;
			var currentPropertySet = sourceProperties;
			var properties = new List<PropertyInfo>(4);
			while (currentIndex < name.Length)
			{
				var property = currentPropertySet.FirstOrDefault(p => name.IndexOf(p.Name, currentIndex, StringComparison.OrdinalIgnoreCase) == currentIndex);
				if (property == null)
				{
					return new PropertyInfo[0];
				}
				properties.Add(property);
				currentIndex += property.Name.Length;
				currentPropertySet = property.PropertyType.GetProperties();
			}
			return properties.ToArray();
		}
	}
}
namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Cartographer.Steps;

	public class MatchByNameFlattenMappingPattern: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			foreach (var targetProperty in strategy.Target.Properties)
			{
				var sourcePropertyChain = BuildPropertyChain(targetProperty, strategy.Source.Properties);
				if (sourcePropertyChain.Length > 1)
				{
					strategy.AddMappingStep(new AssignChain(targetProperty, sourcePropertyChain));
				}
			}
		}

		static PropertyInfo[] BuildPropertyChain(PropertyInfo targetProperty, PropertyInfo[] sourceProperties)
		{
			var name = targetProperty.Name;
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

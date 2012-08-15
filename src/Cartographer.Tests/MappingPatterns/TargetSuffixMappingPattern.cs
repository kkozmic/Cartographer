namespace CartographerTests.MappingPatterns
{
	using System;
	using System.Linq;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class TargetSuffixMappingPattern: IMappingPattern
	{
		readonly string targetPropertyNameSuffix;

		public TargetSuffixMappingPattern(string targetPropertyNameSuffix)
		{
			this.targetPropertyNameSuffix = targetPropertyNameSuffix;
		}

		public void Contribute(MappingStrategy strategy)
		{
			foreach (var targetProperty in strategy.Target.GetProperties().Where(p => p.Name.EndsWith(targetPropertyNameSuffix, StringComparison.OrdinalIgnoreCase)))
			{
				var expectedName = GetExpectedName(targetProperty);
				var sourceProperty = strategy.Source.GetProperties().FirstOrDefault(p => p.Name == expectedName);
				if (sourceProperty != null)
				{
					var assign = new Assign(targetProperty, sourceProperty);

					strategy.AddMappingStep(assign);
				}
			}
		}

		string GetExpectedName(PropertyInfo targetProperty)
		{
			return targetProperty.Name.Substring(0, targetProperty.Name.Length - targetPropertyNameSuffix.Length);
		}
	}
}
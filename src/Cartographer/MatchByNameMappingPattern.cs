namespace Cartographer
{
	using System.Linq;

	using Cartographer.Steps;

	public class MatchByNameMappingPattern: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			foreach (var targetProperty in strategy.Target.Properties)
			{
				var sourceProperty = strategy.Source.Properties.FirstOrDefault(p => p.Name == targetProperty.Name);
				if (sourceProperty != null)
				{
					strategy.AddMappingStep(new Assign(targetProperty, sourceProperty));
				}
			}
		}
	}
}

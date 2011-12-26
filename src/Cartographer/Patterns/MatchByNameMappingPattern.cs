namespace Cartographer.Patterns
{
	using System.Linq;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class MatchByNameMappingPattern: IMappingPattern
	{
		public void Contribute(MappingStrategy strategy)
		{
			foreach (var targetProperty in strategy.Target.GetProperties())
			{
				var sourceProperty = strategy.Source.GetProperties().FirstOrDefault(p => p.Name == targetProperty.Name);
				if (sourceProperty != null)
				{
					var assign = new Assign(targetProperty, sourceProperty);

					strategy.AddMappingStep(assign);
				}
			}
		}
	}
}
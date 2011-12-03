namespace Cartographer
{
	using System;

	public class MappingBuilder: IMappingBuilder
	{
		readonly IMappingPattern[] patterns;

		public MappingBuilder(params IMappingPattern[] patterns)
		{
			this.patterns = patterns;
		}

		public MappingStrategy BuildMappingStrategy(TypeModel source, TypeModel target)
		{
			var strategy = new MappingStrategy { Source = source, Target = target };
			foreach (var pattern in patterns)
			{
				pattern.Contribute(strategy);
			}
			return strategy;
		}
	}
}
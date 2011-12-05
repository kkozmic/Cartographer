namespace Cartographer
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Cartographer.Steps;

	public class MappingStrategy
	{
		readonly IList<MappingStep> mappingSteps = new List<MappingStep>();

		public MappingStrategy(TypeModel source, TypeModel target)
		{
			Source = source;
			Target = target;
		}

		public IEnumerable<MappingStep> MappingSteps
		{
			get { return mappingSteps; }
		}

		public TypeModel Source { get; private set; }
		public TypeModel Target { get; private set; }

		public PropertyInfo[] UnusedSourceProperties
		{
			get { return Source.Properties.Except(mappingSteps.SelectMany(s => s.SourcePropertiesUsed)).ToArray(); }
		}

		public PropertyInfo[] UnusedTargetProperties
		{
			get { return Target.Properties.Except(mappingSteps.SelectMany(s => s.TargetPropertiesUsed)).ToArray(); }
		}

		public void AddMappingStep(MappingStep mappingStep)
		{
			mappingSteps.Add(mappingStep);
		}
	}
}

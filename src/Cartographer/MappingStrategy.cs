namespace Cartographer
{
	using System.Collections.Generic;

	using Cartographer.Steps;

	public class MappingStrategy
	{
		readonly IList<MappingStep> mappingSteps = new List<MappingStep>();

		public IEnumerable<MappingStep> MappingSteps
		{
			get { return mappingSteps; }
		}

		public TypeModel Source { get; set; }
		public TypeModel Target { get; set; }

		public void AddMappingStep(MappingStep mappingStep)
		{
			mappingSteps.Add(mappingStep);
		}
	}
}

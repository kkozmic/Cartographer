namespace Cartographer
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	using Cartographer.Internal;
	using Cartographer.Steps;

	public class MappingStrategy
	{
		readonly IList<MappingStep> mappingSteps = new List<MappingStep>();

		public MappingStrategy(TypeModel source, TypeModel target)
		{
			Source = source;
			Target = target;

			ContextExpression = Expression.Parameter(typeof (MappingContext), "context");
			SourceExpression = Expression.Variable(Source.Type, "source");
			TargetExpression = Expression.Variable(Target.Type, "target");
			MapperExpression = Expression.Property(ContextExpression, MappingContextMeta.Mapper);
		}

		public ParameterExpression ContextExpression { get; set; }
		public Expression MapperExpression { get; set; }

		public IEnumerable<MappingStep> MappingSteps
		{
			get { return mappingSteps; }
		}

		public TypeModel Source { get; private set; }
		public ParameterExpression SourceExpression { get; set; }
		public TypeModel Target { get; private set; }
		public ParameterExpression TargetExpression { get; set; }

		public PropertyInfo[] UnusedSourceProperties
		{
			get { return Source.Properties.Except(mappingSteps.SelectMany(s => s.SourcePropertiesUsed)).ToArray(); }
		}

		public PropertyInfo[] UnusedTargetProperties
		{
			get { return Target.Properties.Except(mappingSteps.SelectMany(s => s.TargetPropertiesUsed)).ToArray(); }
		}

		public Expression ValueExpression { get; set; }

		public void AddMappingStep(MappingStep mappingStep)
		{
			mappingSteps.Add(mappingStep);
		}
	}
}

namespace Cartographer.Compiler
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Internal;
	using Cartographer.Internal.Collections;
	using Cartographer.Steps;

	public class MappingStrategy
	{
		readonly IList<MappingStep> mappingSteps = new List<MappingStep>();

		public MappingStrategy(MappingInfo mappingInfo, IMappingDescriptor descriptor)
		{
			Descriptor = descriptor;
			Source = mappingInfo.MappingSourceType;
			Target = mappingInfo.MappingTargetType;
			HasTargetInstance = mappingInfo.MapIntoExistingTargetInstance;
			try
			{
				TargetConstructor = Target.GetConstructors().Single();
				ConstructorParameterMappingSteps = new OrderedKeyedCollection<ParameterInfo, MappingStep>(TargetConstructor.GetParameters());
			}
			catch (InvalidOperationException)
			{
				throw new ArgumentException("Target type must have single public constructor. This is the only scenario supported at the moment.", "target");
			}

			ContextExpression = Expression.Parameter(typeof (MappingContext), "context");
			SourceExpression = Expression.Variable(Source, "source");
			TargetExpression = Expression.Variable(Target, "target");
			MapperExpression = Expression.Property(ContextExpression, MappingContextMeta.Mapper);
		}

		public OrderedKeyedCollection<ParameterInfo, MappingStep> ConstructorParameterMappingSteps { get; private set; }

		public ParameterExpression ContextExpression { get; private set; }

		public IMappingDescriptor Descriptor { get; private set; }

		public bool HasTargetInstance { get; private set; }

		public MappingStep InitTargetStep { get; set; }

		public Expression MapperExpression { get; private set; }


		public IEnumerable<MappingStep> MappingSteps
		{
			get { return mappingSteps; }
		}

		public Type Source { get; private set; }

		public ParameterExpression SourceExpression { get; private set; }

		public Type Target { get; private set; }

		public ConstructorInfo TargetConstructor { get; private set; }

		public ParameterExpression TargetExpression { get; private set; }

		[TechnicalDebt("This should go. Not sure how to nicely replace it yet but it's ugly as sin")]
		public Expression ValueExpression { get; set; }

		public void AddMappingStep(MappingStep mappingStep)
		{
			mappingSteps.Add(mappingStep);
		}
	}
}
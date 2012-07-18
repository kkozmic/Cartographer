namespace Cartographer.Patterns
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Internal.Collections;
	using Cartographer.Steps;

	public class DescriptorContributor: IMappingStrategyContributor
	{
		readonly IMappingDescriptor descriptor;

		public DescriptorContributor(IMappingDescriptor descriptor)
		{
			this.descriptor = descriptor;
		}

		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			strategy.Descriptor = descriptor;
		}
	}

	public class ConstructorContributor: IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			var ctors = strategy.Target.GetConstructors();
			if (ctors.Length != 1)
			{
				return;
			}
			strategy.TargetConstructor = ctors.Single();
			strategy.ConstructorParameterMappingSteps = new OrderedKeyedCollection<ParameterInfo, MappingStep>(strategy.TargetConstructor.GetParameters());
		}
	}

	public class HarcodedMappingContributor : IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			var directMappingStep = new DirectMappingStep(strategy.Source, strategy.Target);
			var converter = context.ApplyConverter(directMappingStep, withFallback: false);
			if (converter != null)
			{
				directMappingStep.Conversion = converter;
				strategy.InitTargetStep = directMappingStep;
				context.MarkAsCompleted();
			}

		}
	}

	public class ApplyMappingSteps:IMappingStrategyContributor
	{
		readonly IMappingPattern[] mappingPatterns;

		public ApplyMappingSteps(IMappingPattern[] mappingPatterns)
		{
			this.mappingPatterns = mappingPatterns;
		}

		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			foreach (var pattern in mappingPatterns)
			{
				pattern.Contribute(strategy);
			}
		}
	}

	public class ApplyConverters:IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{

			foreach (var mappingStep in strategy.MappingSteps)
			{
				mappingStep.Conversion = context.ApplyConverter(mappingStep, withFallback: true);
			}
		}
	}

	public class InitTarget : IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			if (strategy.HasTargetInstance)
			{
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.Convert(Expression.Property(s.ContextExpression, MappingContextMeta.TargetInstance), s.Target));
			}
			else
			{
				if(strategy.TargetConstructor == null)
				{
					throw new ArgumentException("Constructor not set. This exception message is a work in progress");
				}

				if (strategy.ConstructorParameterMappingSteps == null)
				{
					throw new ArgumentException("ConstructorParameterMappingSteps not set. This exception message is a work in progress");
				}

				foreach (var mappingStep in strategy.ConstructorParameterMappingSteps.ByKey)
				{
					if (mappingStep.Value == null)
					{
						throw new InvalidOperationException(String.Format("No mapping for constructor parameter {0} has been specified. All constructor parameters need value", mappingStep.Key));
					}
					mappingStep.Value.Conversion = context.ApplyConverter(mappingStep.Value, withFallback: true);
				}
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.New(s.TargetConstructor, GetConstructorParameters(s)));
			}
		}


		static Expression BuildParameterExpression(MappingStep step, MappingStrategy strategy)
		{
			var map = step.Apply(strategy, step.Conversion);
			return map;
		}

		static Expression[] GetConstructorParameters(MappingStrategy strategy)
		{
			return strategy.ConstructorParameterMappingSteps.Select(s => BuildParameterExpression(s, strategy)).ToArray();
		}
	}
}
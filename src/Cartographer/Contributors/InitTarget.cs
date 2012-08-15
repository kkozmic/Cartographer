namespace Cartographer.Contributors
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Steps;

	public class InitTarget: IMappingStrategyContributor
	{
		public void Apply(MappingStrategy strategy, MappingStrategyBuildContext context)
		{
			if (strategy.HasTargetInstance)
			{
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.Convert(Expression.Property(s.ContextExpression, MappingContextMeta.TargetInstance), s.Target));
			}
			else
			{
				if (strategy.TargetConstructor == null)
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
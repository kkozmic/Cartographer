namespace Cartographer.Compiler
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Cartographer.Internal;
	using Cartographer.Patterns;
	using Cartographer.Steps;

	public class MappingStrategyBuilder: IMappingStrategyBuilder
	{
		readonly IConversionPatternRepository conversionPatternRepository;

		readonly IMappingDescriptor descriptor;

		readonly IMappingPattern[] mappingPatterns;

		public MappingStrategyBuilder(IMappingDescriptor descriptor, IConversionPatternRepository conversionPatternRepository, params IMappingPattern[] mappingPatterns)
		{
			this.descriptor = descriptor;
			this.conversionPatternRepository = conversionPatternRepository;
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(MappingInfo mappingInfo)
		{
			var strategy = new MappingStrategy(mappingInfo, descriptor);

			//first try to shortcircuit
			var directMappingStep = new DirectMappingStep(strategy.Source, strategy.Target);
			var converter = ApplyConverter(directMappingStep, withFallback: false);
			if (converter != null)
			{
				directMappingStep.Conversion = converter;
				strategy.InitTargetStep = directMappingStep;
				return strategy;
			}
			foreach (var pattern in mappingPatterns)
			{
				pattern.Contribute(strategy);
			}
			foreach (var mappingStep in strategy.MappingSteps)
			{
				mappingStep.Conversion = ApplyConverter(mappingStep, withFallback: true);
			}
			if (strategy.HasTargetInstance)
			{
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.Convert(Expression.Property(s.ContextExpression, MappingContextMeta.TargetInstance), s.Target));
			}
			else
			{
				foreach (var mappingStep in strategy.ConstructorParameterMappingSteps.ByKey)
				{
					if (mappingStep.Value == null)
					{
						throw new InvalidOperationException(string.Format("No mapping for constructor parameter {0} has been specified. All constructor parameters need value", mappingStep.Key));
					}
					mappingStep.Value.Conversion = ApplyConverter(mappingStep.Value, withFallback: true);
				}
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.New(s.TargetConstructor, GetConstructorParameters(s)));
			}
			return strategy;
		}

		DelegatingConversionStep ApplyConverter(MappingStep mapping, bool withFallback)
		{
			dynamic instance = null;
			try
			{
				instance = conversionPatternRepository.LeaseConversionPatternFor(mapping.SourceValueType, mapping.TargetValueType);
				if (ReferenceEquals((object)instance, null) == false)
				{
					var expression = instance.BuildConversionExpression(mapping) as LambdaExpression;
					if (expression != null)
					{
						return new DelegatingConversionStep(expression);
					}
				}
			}
			finally
			{
				conversionPatternRepository.Recycle((object)instance);
			}
			if (withFallback == false || mapping.TargetValueType.IsAssignableFrom(mapping.SourceValueType))
			{
				return null;
			}
			// fallabck behavior
			instance = Activator.CreateInstance(typeof (MapConversionPattern<>).MakeGenericType(mapping.TargetValueType));
			return new DelegatingConversionStep(instance.BuildConversionExpression(mapping) as LambdaExpression);
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
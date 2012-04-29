namespace Cartographer.Compiler
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Cartographer.Internal;
	using Cartographer.Steps;

	public class MappingStrategyBuilder: IMappingStrategyBuilder
	{
		readonly IConversionPatternGenericCloser conversionPatternGenericCloser;

		readonly Type[] conversionPatterns;

		readonly IMappingDescriptor descriptor;

		readonly IMappingPattern[] mappingPatterns;

		readonly Type[] rootConversionPatterns;

		public MappingStrategyBuilder(IMappingDescriptor descriptor, IConversionPatternGenericCloser conversionPatternGenericCloser, Type[] conversionPatterns, params IMappingPattern[] mappingPatterns)
		{
			this.descriptor = descriptor;
			this.conversionPatternGenericCloser = conversionPatternGenericCloser;
			this.conversionPatterns = conversionPatterns;
			rootConversionPatterns = Array.FindAll(conversionPatterns, typeof (IRootConversionPattern).IsAssignableFrom);
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(MappingInfo mappingInfo)
		{
			var strategy = new MappingStrategy(mappingInfo, descriptor);

			//first try to shortcircuit
			var directMappingStep = new DirectMappingStep(strategy.Source, strategy.Target);
			var converter = ApplyConverter(directMappingStep, rootConversionPatterns);
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
				mappingStep.Conversion = ApplyConverter(mappingStep, conversionPatterns);
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
					mappingStep.Value.Conversion = ApplyConverter(mappingStep.Value, conversionPatterns);
				}
				strategy.InitTargetStep = new SimpleStep(strategy.Target, strategy.Target, (s, _) => Expression.New(s.TargetConstructor, GetConstructorParameters(s)));
			}
			return strategy;
		}

		DelegatingConversionStep ApplyConverter(MappingStep mapping, Type[] patternTypes)
		{
			foreach (var patternType in patternTypes)
			{
				var type = conversionPatternGenericCloser.Close(patternType, mapping.SourceValueType, mapping.TargetValueType);
				if (type == null)
				{
					continue;
				}

				dynamic instance = Activator.CreateInstance(type);
				LambdaExpression expression = instance.BuildConversionExpression(mapping);
				if (expression != null)
				{
					return new DelegatingConversionStep(expression);
				}
			}
			return null;
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
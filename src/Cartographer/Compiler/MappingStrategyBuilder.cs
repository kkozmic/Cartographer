namespace Cartographer.Compiler
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Steps;

	public class MappingStrategyBuilder: IMappingStrategyBuilder
	{
		readonly IConversionPatternGenericCloser conversionPatternGenericCloser;

		readonly Type[] conversionPatterns;

		readonly IMappingDescriptor descriptor;

		readonly IMappingPattern[] mappingPatterns;

		public MappingStrategyBuilder(IMappingDescriptor descriptor, IConversionPatternGenericCloser conversionPatternGenericCloser, Type[] conversionPatterns, params IMappingPattern[] mappingPatterns)
		{
			this.descriptor = descriptor;
			this.conversionPatternGenericCloser = conversionPatternGenericCloser;
			this.conversionPatterns = conversionPatterns;
			this.mappingPatterns = mappingPatterns;
		}

		public MappingStrategy BuildMappingStrategy(Type source, Type target)
		{
			var strategy = new MappingStrategy(source, target, descriptor);
			foreach (var pattern in mappingPatterns)
			{
				pattern.Contribute(strategy);
			}
			foreach (var mappingStep in strategy.MappingSteps)
			{
				ApplyConverter(mappingStep);
			}
			return strategy;
		}

		void ApplyConverter(MappingStep mapping)
		{
			foreach (var patternType in conversionPatterns)
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
					mapping.Conversion = new DelegatingConversionStep(expression);
					return;
				}
			}
		}
	}
}
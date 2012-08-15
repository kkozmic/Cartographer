namespace Cartographer.Compiler
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Patterns;
	using Cartographer.Steps;

	public class MappingStrategyBuilder: IMappingStrategyBuilder
	{
		readonly IMappingStrategyContributor[] contributors;

		readonly IConversionPatternRepository conversionPatternRepository;

		public MappingStrategyBuilder(IConversionPatternRepository conversionPatternRepository, IMappingStrategyContributor[] contributors)
		{
			this.conversionPatternRepository = conversionPatternRepository;
			this.contributors = contributors;
		}

		public MappingStrategy BuildMappingStrategy(MappingInfo mappingInfo)
		{
			var strategy = new MappingStrategy(mappingInfo);
			var context = new MappingStrategyBuildContext(ApplyConverter);
			foreach (var contributor in contributors)
			{
				contributor.Apply(strategy, context);
				if (context.Completed)
				{
					break;
				}
			}
			return strategy;
		}

		DelegatingConversionStep ApplyConverter(MappingStep mapping, bool withFallback)
		{
			dynamic instance = null;
			try
			{
				instance = conversionPatternRepository.LeaseConversionPatternFor(mapping.SourceValueType, mapping.TargetValueType);
				if (((object)instance) != null)
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
	}
}
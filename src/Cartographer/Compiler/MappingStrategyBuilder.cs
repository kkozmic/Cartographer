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
			object instance = null;
			try
			{
				instance = conversionPatternRepository.LeaseConversionPatternFor(mapping.SourceValueType, mapping.TargetValueType);
				if (instance != null)
				{
					var expression = BuildConversionExpression(instance, mapping);
					if (expression != null)
					{
						return new DelegatingConversionStep(expression);
					}
				}
			}
			finally
			{
				conversionPatternRepository.Recycle(instance);
			}
			if (withFallback == false || mapping.TargetValueType.IsAssignableFrom(mapping.SourceValueType))
			{
				return null;
			}
			// fallabck behavior
			instance = Activator.CreateInstance(typeof (MapConversionPattern<>).MakeGenericType(mapping.TargetValueType));
			return new DelegatingConversionStep(BuildConversionExpression(instance, mapping));
		}

		static LambdaExpression BuildConversionExpression(object instance, MappingStep mapping)
		{
			var buildConversionExpression = instance.GetType().GetMethod("BuildConversionExpression");
			var expression = (LambdaExpression)buildConversionExpression.Invoke(instance, new object[] { mapping });
			return expression;
		}
	}
}
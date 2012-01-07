namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Steps;



	public class MappingConverter
	{
		readonly Func<MappingStep, Type[]> genericArguments;

		readonly Type patternType;

		readonly Func<MappingStep, bool> condition;

		public MappingConverter(Type patternType, Func<MappingStep, bool> condition, Func<MappingStep, Type[]> genericArguments)
		{
			this.condition = condition;
			this.patternType = patternType;
			this.genericArguments = genericArguments;
		}

		public void Apply(MappingStep mapping)
		{
			if (condition(mapping))
			{
				var pattern = CreatePatternInstance(mapping);
				LambdaExpression expression = pattern.BuildConversionExpression(mapping);
				if (expression != null)
				{
					mapping.Conversion = new DelegatingConversionStep(expression);
				}
			}
		}

		dynamic CreatePatternInstance(MappingStep mapping)
		{
			var type = patternType;
			if (genericArguments != null)
			{
				type = type.MakeGenericType(genericArguments(mapping));
			}
			return Activator.CreateInstance(type);
		}
	}
}
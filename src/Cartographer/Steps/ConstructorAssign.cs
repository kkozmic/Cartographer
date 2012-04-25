namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;

	public class ConstructorAssign: MappingStep
	{
		readonly ParameterInfo parameterInfo;

		readonly PropertyInfo sourceProperty;

		public ConstructorAssign(ParameterInfo parameterInfo, PropertyInfo sourceProperty)
		{
			this.parameterInfo = parameterInfo;
			this.sourceProperty = sourceProperty;
		}

		public override Type SourceValueType
		{
			get { return sourceProperty.PropertyType; }
		}

		public override Type TargetValueType
		{
			get { return parameterInfo.ParameterType; }
		}

		public override Expression Apply(MappingStrategy strategy, ConversionStep conversion)
		{
			var get = Expression.Property(strategy.SourceExpression, sourceProperty);
			strategy.ValueExpression = get;
			if (conversion != null)
			{
				var convert = conversion.BuildConversionExpression(strategy);
				strategy.ValueExpression = convert;
			}
			return strategy.ValueExpression;
		}
	}
}
namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;

	public class Assign: MappingStep
	{
		readonly PropertyInfo sourceProperty;

		readonly PropertyInfo targetProperty;

		public Assign(PropertyInfo targetProperty, PropertyInfo sourceProperty)
		{
			this.targetProperty = targetProperty;
			this.sourceProperty = sourceProperty;
		}

		public override Type SourceValueType
		{
			get { return sourceProperty.PropertyType; }
		}

		public override Type TargetValueType
		{
			get { return targetProperty.PropertyType; }
		}

		public override Expression Apply(MappingStrategy strategy, ConversionStep conversion)
		{
			var get = Expression.Property(strategy.SourceExpression, sourceProperty);
			strategy.ValueExpression = get;
			if (conversion != null)
			{
				var convert = conversion.BuildConversionExpression(strategy, this);
				strategy.ValueExpression = convert;
			}
			var property = Expression.Property(strategy.TargetExpression, targetProperty);
			return Expression.Assign(property, strategy.ValueExpression);
		}
	}
}
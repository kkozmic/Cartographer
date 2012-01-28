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

		public override Expression BuildGetSourceValueExpression(MappingStrategy context)
		{
			return Expression.Property(context.SourceExpression, sourceProperty);
		}

		public override Expression BuildSetTargetValueExpression(MappingStrategy context)
		{
			var property = Expression.Property(context.TargetExpression, targetProperty);
			return Expression.Assign(property, context.ValueExpression);
		}
	}
}
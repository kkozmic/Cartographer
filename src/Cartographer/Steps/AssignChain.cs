namespace Cartographer.Steps
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	public class AssignChain: MappingStep
	{
		readonly PropertyInfo[] sourcePropertyChain;
		readonly PropertyInfo targetProperty;

		public AssignChain(PropertyInfo targetProperty, PropertyInfo[] sourcePropertyChain)
		{
			this.targetProperty = targetProperty;
			this.sourcePropertyChain = sourcePropertyChain;
		}

		public override PropertyInfo[] SourcePropertiesUsed
		{
			get { return new[] { TargetProperty }; }
		}

		public PropertyInfo[] SourcePropertyChain
		{
			get { return sourcePropertyChain; }
		}

		public override Type SourceValueType
		{
			get { return sourcePropertyChain.Last().PropertyType; }
		}

		public override PropertyInfo[] TargetPropertiesUsed
		{
			get { return new[] { SourcePropertiesUsed[0] }; }
		}

		public PropertyInfo TargetProperty
		{
			get { return targetProperty; }
		}

		public override Type TargetValueType
		{
			get { return targetProperty.PropertyType; }
		}

		public override Expression BuildGetSourceValueExpression(MappingContext context)
		{
			Expression expression = context.SourceParameter;
			for (var i = 0; i < sourcePropertyChain.Length; i++)
			{
				expression = Expression.Property(expression, sourcePropertyChain[i]);
			}

			return expression;
		}

		public override Expression BuildSetTargetValueExpression(MappingContext context)
		{
			var property = Expression.Property(context.TargetParameter, targetProperty);
			return Expression.Assign(property, context.ValueParameter);
		}
	}
}

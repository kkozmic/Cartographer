namespace Cartographer.Steps
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal.Expressions;

	public class AssignChain: MappingStep
	{
		readonly PropertyInfo[] sourcePropertyChain;

		readonly PropertyInfo targetProperty;

		List<PropertyInfo> nullableProperties;

		public AssignChain(PropertyInfo targetProperty, PropertyInfo[] sourcePropertyChain)
		{
			this.targetProperty = targetProperty;
			this.sourcePropertyChain = sourcePropertyChain;

			foreach (var property in sourcePropertyChain)
			{
				if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
				{
					AllowNullValueOf(property);
				}
			}
		}

		public override Type SourceValueType
		{
			get { return sourcePropertyChain.Last().PropertyType; }
		}

		public override Type TargetValueType
		{
			get { return targetProperty.PropertyType; }
		}

		public void AllowNullValueOf(PropertyInfo property)
		{
			if (nullableProperties == null)
			{
				nullableProperties = new List<PropertyInfo>(4);
			}
			nullableProperties.Add(property);
		}

		public override Expression Apply(MappingStrategy strategy, ConversionStep conversion)
		{
			return BuildGetSourceValueExpression(strategy, conversion);
		}

		Expression BuildBody(Expression owner, int index, MappingStrategy strategy, ConversionStep conversion)
		{
			if (index == sourcePropertyChain.Length - 1)
			{
				return SetValue(strategy, conversion, owner);
			}

			if (nullableProperties.Contains(sourcePropertyChain[index]) == false)
			{
				return BuildBody(Expression.Property(owner, sourcePropertyChain[index + 1]), index + 1, strategy, conversion);
			}
			var local = Expression.Variable(owner.Type);
			var property = new PropertyIfNotNullInnerExpression(Expression.Property(local, sourcePropertyChain[index + 1]));
			var body = BuildBody(property, index + 1, strategy, conversion);
			var expression = new PropertyIfNotNullExpression(owner, body, local, TargetValueType);
			property.Owner = expression;
			return expression;
		}

		Expression BuildGetSourceValueExpression(MappingStrategy context, ConversionStep conversion)
		{
			if (nullableProperties == null)
			{
				var value = sourcePropertyChain.Aggregate<PropertyInfo, Expression>(context.SourceExpression, Expression.Property);
				return SetValue(context, conversion, value);
			}
			return BuildBody(Expression.Property(context.SourceExpression, sourcePropertyChain[0]), 0, context, conversion);
		}

		Expression BuildSetTargetValueExpression(MappingStrategy context)
		{
			var property = Expression.Property(context.TargetExpression, targetProperty);
			return Expression.Assign(property, context.ValueExpression);
		}

		Expression SetValue(MappingStrategy strategy, ConversionStep conversion, Expression value)
		{
			strategy.ValueExpression = value;
			if (conversion != null)
			{
				var convert = conversion.BuildConversionExpression(strategy, this);
				strategy.ValueExpression = convert;
			}
			return BuildSetTargetValueExpression(strategy);
		}
	}
}
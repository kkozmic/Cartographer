namespace Cartographer.Steps
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal.Expressions;

	public class ConstructorAssignChain: MappingStep
	{
		readonly ParameterInfo parameterInfo;

		readonly PropertyInfo[] sourcePropertyChain;

		List<PropertyInfo> nullableProperties;

		public ConstructorAssignChain(ParameterInfo parameterInfo, PropertyInfo[] sourcePropertyChain)
		{
			this.parameterInfo = parameterInfo;
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
			get { return parameterInfo.ParameterType; }
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
			if (nullableProperties == null)
			{
				var value = sourcePropertyChain.Aggregate<PropertyInfo, Expression>(strategy.SourceExpression, Expression.Property);
				return SetValue(strategy, conversion, value);
			}
			return BuildBody(Expression.Property(strategy.SourceExpression, sourcePropertyChain[0]), 0, strategy, conversion);
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

		Expression SetValue(MappingStrategy strategy, ConversionStep conversion, Expression value)
		{
			strategy.ValueExpression = value;
			if (conversion != null)
			{
				var convert = conversion.BuildConversionExpression(strategy);
				strategy.ValueExpression = convert;
			}
			return strategy.ValueExpression;
		}
	}
}
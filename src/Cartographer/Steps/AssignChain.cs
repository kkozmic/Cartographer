namespace Cartographer.Steps
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal;

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

		public override PropertyInfo SourceProperty
		{
			get { return sourcePropertyChain[0]; }
		}

		public PropertyInfo[] SourcePropertyChain
		{
			get { return sourcePropertyChain; }
		}

		public override Type SourceValueType
		{
			get { return sourcePropertyChain.Last().PropertyType; }
		}

		public override PropertyInfo TargetProperty
		{
			get { return targetProperty; }
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

		public override Expression BuildGetSourceValueExpression(MappingStrategy context)
		{
			if (nullableProperties == null)
			{
				return sourcePropertyChain.Aggregate<PropertyInfo, Expression>(context.SourceExpression, Expression.Property);
			}
			return BuildChainWithNullChecks(context);
		}

		public override Expression BuildSetTargetValueExpression(MappingStrategy context)
		{
			var property = Expression.Property(context.TargetExpression, targetProperty);
			return Expression.Assign(property, context.ValueExpression);
		}

		Expression BuildBody(Expression expression, int index)
		{
			if (index == sourcePropertyChain.Length)
			{
				if (sourcePropertyChain.Last().PropertyType.IsNullable() == false)
				{
					return Expression.Convert(expression, TargetValueType);
				}
				return expression;
			}

			var owner = Expression.Property(expression, sourcePropertyChain[index]);
			if (nullableProperties.Contains(sourcePropertyChain[index]) == false)
			{
				return BuildBody(owner, index + 1);
			}
			var local = Expression.Variable(owner.Type);
			var body = new Expression[]
			           {
			           	Expression.Assign(local, owner),
			           	Expression.Condition(Expression.ReferenceNotEqual(local, Expression.Default(local.Type)),
			           	                     BuildBody(local, index + 1),
			           	                     Expression.Default(TargetValueType),
			           	                     TargetValueType)
			           };
			return Expression.Block(TargetValueType, new[] { local }, body);
		}

		Expression BuildChainWithNullChecks(MappingStrategy context)
		{
			var body = BuildBody(context.SourceExpression, 0);
			var result = Expression.Block(TargetValueType, body);
			return result;
		}
	}
}
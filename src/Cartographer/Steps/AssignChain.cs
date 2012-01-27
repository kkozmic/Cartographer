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

		Expression BuildChainWithNullChecks(MappingStrategy context)
		{
			/*
			var propertyAccess = Property(owner, property);
			var reduced = Condition(ReferenceNotEqual(owner, Default(owner.Type)), propertyAccess, @default, Type);
			return reduced;
			 */




			var localTarget = Expression.Variable(TargetValueType, "__value");
			var body = new[]
			           {
			           	Expression.Assign(localTarget, Expression.Default(TargetValueType)),
			           	BuildBody(context.SourceExpression, localTarget, 0),
			           	localTarget
			           };
			var result = Expression.Block(new[] { localTarget }, body);
			return result;
		}

		public override Expression BuildSetTargetValueExpression(MappingStrategy context)
		{
			var property = Expression.Property(context.TargetExpression, targetProperty);
			return Expression.Assign(property, context.ValueExpression);
		}

		Expression BuildBody(Expression expression, ParameterExpression localTarget, int index)
		{	
			if (index == sourcePropertyChain.Length)
			{
				if (sourcePropertyChain.Last().PropertyType.IsNullable() == false)
				{
					return Expression.Assign(localTarget, Expression.Convert(expression, localTarget.Type));
				}
				return Expression.Assign(localTarget, expression);
			}

			var owner = Expression.Property(expression, sourcePropertyChain[index]);
			if (nullableProperties.Contains(sourcePropertyChain[index]) == false)
			{
				return BuildBody(owner, localTarget, index + 1);
			}
			var local = Expression.Variable(owner.Type);
			var body = new Expression[]
			           {
			           	Expression.Assign(local, owner),
			           	Expression.IfThen(Expression.ReferenceNotEqual(local, Expression.Constant(null)),
			           	                  BuildBody(local, localTarget, index + 1))
			           };
			return Expression.Block(new[] { local }, body);
		}
	}
}
namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Cartographer.Internal;

	public class MappingCompiler: IMappingCompiler
	{

		public Delegate Compile(MappingStrategy strategy)
		{
			return new Func<MappingContext, dynamic>(c => Map(c, strategy));
		}

		object Map(MappingContext context, MappingStrategy strategy)
		{
			var body = new List<Expression>();
			
			context.ContextExpression = Expression.Parameter(typeof(MappingContext), "context");
			context.SourceExpression = Expression.Variable(context.SourceType, "source");
			context.TargetExpression = Expression.Variable(context.TargetType, "target");
			context.MapperExpression = Expression.Property(context.ContextExpression, MappingContextMeta.Mapper);

			body.Add(Expression.Assign(context.SourceExpression, Expression.Convert(Expression.Property(context.ContextExpression, MappingContextMeta.SourceInstance), context.SourceType)));

			body.Add(Expression.Assign(context.TargetExpression, Expression.New(context.TargetType)));
			foreach (var step in strategy.MappingSteps)
			{
				context.ValueExpression = step.BuildGetSourceValueExpression(context);
				if (step.Conversion != null)
				{
					context.ValueExpression = step.Conversion.BuildConversionExpression(context, step);
				}
				body.Add(step.BuildSetTargetValueExpression(context));
			}
			body.Add(context.TargetExpression);
			var lambda = Expression.Lambda(Expression.Block(new[] { context.TargetExpression,context.SourceExpression }, body), string.Format("{0} to {1} Converter", context.SourceType.Name, context.TargetType.Name),
			                               new[] { context.ContextExpression });
			dynamic @delegate = lambda.Compile();

			return @delegate.Invoke(context);
		}
	}
}

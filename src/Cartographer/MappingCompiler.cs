namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Cartographer.Steps;

	public class MappingCompiler: IMappingCompiler
	{
		readonly IDictionary<Type, object> visitors = new Dictionary<Type, object>();

		public MappingCompiler(params object[] mappingVisitors)
		{
			foreach (dynamic visitor in mappingVisitors)
			{
				AddVisitor(visitor);
			}
		}

		public Delegate Compile(MappingStrategy strategy)
		{
			return new Func<MappingContext, dynamic>(c => Map(c, strategy));
		}

		void AddVisitor<TStep>(IMappingVisitor<TStep> visitor) where TStep: MappingStep
		{
			visitors.Add(typeof (TStep), visitor);
		}

		object Map(MappingContext context, MappingStrategy strategy)
		{
			var target = Activator.CreateInstance(context.TargetType);
			context.SourceParameter = Expression.Parameter(context.SourceType, "source");
			context.TargetParameter = Expression.Parameter(context.TargetType, "target");
			context.MapperParameter = Expression.Parameter(typeof (IMapper), "mapper");
			// TODO: we'll also need some 'context'/'arguments' parameter too
			context.TargetInstance = target;
			var body = new List<Expression>();
			foreach (var step in strategy.MappingSteps)
			{
				context.ValueParameter = step.BuildGetSourceValueExpression(context);
				if (step.Conversion != null)
				{
					context.ValueParameter = step.Conversion.BuildConversionExpression(context, step);
				}
				body.Add(step.BuildSetTargetValueExpression(context));
			}

			var lambda = Expression.Lambda(Expression.Block(body), string.Format("{0} to {1} Converter", context.SourceType.Name, context.TargetType.Name),
			                               new[] { context.SourceParameter, context.TargetParameter, context.MapperParameter });
			dynamic @delegate = lambda.Compile();

			@delegate.Invoke((dynamic)context.SourceInstance, (dynamic)context.TargetInstance, context.Mapper);
			return target;
		}
	}
}

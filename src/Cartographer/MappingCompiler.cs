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
			context.TargetInstance = target;
			foreach (var step in strategy.MappingSteps)
			{
				context.ValueParameter = Expression.Parameter(step.SourceValueType, "value");
				dynamic getter = BuildGetter(context, step);
				var value = getter.Invoke((dynamic)context.SourceInstance, (dynamic)context.TargetInstance, context.Mapper);
				if (step.Conversion != null)
				{
					dynamic converter = BuildConverter(context, step);
					value = converter.Invoke(value, (dynamic)context.TargetInstance, context.Mapper);
				}
				context.ValueParameter = Expression.Parameter(step.TargetValueType, "value");
				dynamic setter = BuildSetter(context, step);
				setter.Invoke(value, (dynamic)context.TargetInstance, context.Mapper);
			}
			return target;
		}

		static Delegate BuildConverter(MappingContext context, MappingStep step)
		{
			var body = step.Conversion.BuildConversionExpression(context, step);
			var lambda = Expression.Lambda(body,
			                               context.ValueParameter,
			                               context.TargetParameter,
			                               context.MapperParameter);
			return lambda.Compile();
		}

		static Delegate BuildGetter(MappingContext context, MappingStep step)
		{
			var body = step.BuildGetSourceValueExpression(context);
			var lambda = Expression.Lambda(body,
			                               context.SourceParameter,
			                               context.TargetParameter,
			                               context.MapperParameter);
			return lambda.Compile();
		}

		static Delegate BuildSetter(MappingContext context, MappingStep step)
		{
			var body = step.BuildSetTargetValueExpression(context);
			var lambda = Expression.Lambda(body,
			                               context.ValueParameter,
			                               context.TargetParameter,
			                               context.MapperParameter);
			return lambda.Compile();
		}
	}
}

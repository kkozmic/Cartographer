namespace Cartographer.Compiler
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Cartographer.Internal;
	using Cartographer.Internal.Expressions;

	public class MappingCompiler: IMappingCompiler
	{
		public Delegate Compile(MappingStrategy strategy)
		{
			var body = new List<Expression>();
			InitDescriptor(strategy);
			InitSource(strategy, body);
			InitTarget(strategy, body);
			GenerateMapping(strategy, body);
			var lambda = GenerateLambda(strategy, body);
			var reduced = new ReduceExpressionVisitor().Reduce(lambda);
			return reduced.Compile();
		}

		void InitDescriptor(MappingStrategy strategy)
		{
			strategy.Descriptor.DescribeMapping(strategy.Source, strategy.Target);
		}

		static LambdaExpression GenerateLambda(MappingStrategy strategy, List<Expression> body)
		{
			var lambda = Expression.Lambda(Expression.Block(new[] { strategy.TargetExpression, strategy.SourceExpression }, body),
			                               string.Format("{0} to {1} Converter", strategy.Source.Name, strategy.Target.Name),
			                               new[] { strategy.ContextExpression });
			return lambda;
		}

		static void GenerateMapping(MappingStrategy strategy, List<Expression> body)
		{
			foreach (var step in strategy.MappingSteps)
			{
				var map = step.Apply(strategy, step.Conversion);
				strategy.Descriptor.DescribeStep(map);
				body.Add(map);
			}
			body.Add(strategy.TargetExpression);
		}

		static void InitSource(MappingStrategy strategy, List<Expression> body)
		{
			var step = Expression.Assign(strategy.SourceExpression, Expression.Convert(Expression.Property(strategy.ContextExpression, MappingContextMeta.SourceInstance), strategy.Source));
			// we don't describe source. It gets cast from the source to its actual type but that's not interesting and only adds noise to the mapping
			body.Add(step);
		}

		static void InitTarget(MappingStrategy strategy, List<Expression> body)
		{
			var step = Expression.Assign(strategy.TargetExpression, TargetInstanceExpression(strategy));
			strategy.Descriptor.DescribeStep(step);
			body.Add(step);
		}

		static Expression TargetInstanceExpression(MappingStrategy strategy)
		{
			return strategy.InitTargetStep.Apply(strategy, strategy.InitTargetStep.Conversion);
		}
	}
}
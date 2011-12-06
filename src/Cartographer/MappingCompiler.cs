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
			var body = new List<Expression>();
			InitSource(strategy, body);
			InitTarget(strategy, body);
			GenerateMapping(strategy, body);
			var lambda = GenerateLambda(strategy, body);
			return lambda.Compile();
		}

		static LambdaExpression GenerateLambda(MappingStrategy strategy, List<Expression> body)
		{
			var lambda = Expression.Lambda(Expression.Block(new[] { strategy.TargetExpression, strategy.SourceExpression }, body),
			                               string.Format("{0} to {1} Converter", strategy.Source.Type.Name, strategy.Target.Type.Name),
			                               new[] { strategy.ContextExpression });
			return lambda;
		}

		static void GenerateMapping(MappingStrategy strategy, List<Expression> body)
		{
			foreach (var step in strategy.MappingSteps)
			{
				strategy.ValueExpression = step.BuildGetSourceValueExpression(strategy);
				if (step.Conversion != null)
				{
					strategy.ValueExpression = step.Conversion.BuildConversionExpression(strategy, step);
				}
				body.Add(step.BuildSetTargetValueExpression(strategy));
			}
			body.Add(strategy.TargetExpression);
		}

		static void InitSource(MappingStrategy strategy, List<Expression> body)
		{
			body.Add(Expression.Assign(strategy.SourceExpression, Expression.Convert(Expression.Property(strategy.ContextExpression, MappingContextMeta.SourceInstance), strategy.Source.Type)));
		}

		static void InitTarget(MappingStrategy strategy, List<Expression> body)
		{
			body.Add(Expression.Assign(strategy.TargetExpression, Expression.New(strategy.Target.Type)));
		}
	}
}

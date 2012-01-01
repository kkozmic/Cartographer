namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public class DelegatingConversionVisitor: ExpressionVisitor
	{
		readonly MappingStrategy strategy;

		public DelegatingConversionVisitor(MappingStrategy strategy)
		{
			this.strategy = strategy;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if(node.Type == typeof(IMapper))
			{
				return strategy.MapperExpression;
			}
			if(node.Type == strategy.Source)
			{
				return strategy.SourceExpression;
			}
			if(node.Type == strategy.Target)
			{
				return strategy.TargetExpression;
			}
			throw new NotSupportedException(string.Format("Parameter of type {0} is not supported.", node.Type));
		}
	}
}
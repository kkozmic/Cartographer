namespace Cartographer.Steps
{
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public class DelegatingConversionVisitor: ExpressionVisitor
	{
		readonly ParameterExpression contextParameter;

		readonly ParameterExpression mapperParameter;

		readonly ParameterExpression sourceValueParameter;

		readonly MappingStrategy strategy;


		public DelegatingConversionVisitor(MappingStrategy strategy, ParameterExpression sourceValueParameter, ParameterExpression mapperParameter, ParameterExpression contextParameter)
		{
			this.strategy = strategy;
			this.sourceValueParameter = sourceValueParameter;
			this.mapperParameter = mapperParameter;
			this.contextParameter = contextParameter;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (node == sourceValueParameter)
			{
				return strategy.ValueExpression;
			}
			if (node == mapperParameter)
			{
				return strategy.MapperExpression;
			}
			if (node == contextParameter)
			{
				return strategy.ContextExpression;
			}
			return base.VisitParameter(node);
		}
	}
}
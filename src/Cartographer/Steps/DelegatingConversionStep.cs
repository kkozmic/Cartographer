namespace Cartographer.Steps
{
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public class DelegatingConversionStep: ConversionStep
	{
		readonly LambdaExpression expression;

		public DelegatingConversionStep(LambdaExpression expression)
		{
			this.expression = expression;
		}

		public override Expression BuildConversionExpression(MappingStrategy strategy, MappingStep step)
		{
			var visitor = new DelegatingConversionVisitor(strategy);
			return visitor.Visit(expression.Body);
		}
	}
}
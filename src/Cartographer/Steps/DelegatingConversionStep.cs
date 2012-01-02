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
			var parameters = expression.Parameters;
			var visitor = new DelegatingConversionVisitor(strategy, parameters[0], parameters[1], parameters[2]);
			return visitor.Visit(expression.Body);
		}
	}
}
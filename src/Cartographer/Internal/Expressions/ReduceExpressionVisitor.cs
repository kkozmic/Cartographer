namespace Cartographer.Internal.Expressions
{
	using System.Diagnostics;
	using System.Linq.Expressions;

	public class ReduceExpressionVisitor: ExpressionVisitor, IReducingExpressionVisitor
	{
		public Expression VisitReducible(Expression node)
		{
			Debug.Assert(node.CanReduce);
			return Visit(node.Reduce());
		}
	}
}
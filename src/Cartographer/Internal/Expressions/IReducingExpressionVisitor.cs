namespace Cartographer.Internal.Expressions
{
	using System.Linq.Expressions;

	public interface IReducingExpressionVisitor
	{
		Expression VisitReducible(Expression node);
	}
}
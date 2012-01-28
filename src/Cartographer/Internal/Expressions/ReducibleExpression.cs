namespace Cartographer.Internal.Expressions
{
	using System;
	using System.Linq.Expressions;

	public abstract class ReducibleExpression: Expression
	{
		public abstract override Type Type { get; }

		protected abstract ExpressionType ConnectorExpressionType { get; }

		public override bool CanReduce
		{
			get { return true; }
		}

		public override System.Linq.Expressions.ExpressionType NodeType
		{
			get { return (System.Linq.Expressions.ExpressionType)ConnectorExpressionType; }
		}

		public abstract override Expression Reduce();

		protected override Expression Accept(ExpressionVisitor visitor)
		{
			var reducible = visitor as IReducingExpressionVisitor;
			if (reducible != null)
			{
				return reducible.VisitReducible(this);
			}
			return base.Accept(visitor);
		}
	}
}
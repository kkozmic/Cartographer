namespace Cartographer.Internal.Expressions
{
	using System;
	using System.Linq.Expressions;

	public class PropertyIfNotNullInnerExpression: ReducibleExpression
	{
		readonly MemberExpression inner;


		public PropertyIfNotNullInnerExpression(MemberExpression inner)
		{
			this.inner = inner;
		}

		public MemberExpression Inner
		{
			get { return inner; }
		}

		public override Type Type
		{
			get { return inner.Type; }
		}

		protected override ExpressionType ConnectorExpressionType
		{
			get { return ExpressionType.PropertyIfNotNullInner; }
		}

		public override Expression Reduce()
		{
			return inner;
		}
	}
}
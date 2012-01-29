namespace Cartographer.Internal.Expressions
{
	using System;
	using System.Linq.Expressions;

	public class PropertyIfNotNullInnerExpression: ReducibleExpression
	{
		public PropertyIfNotNullInnerExpression(MemberExpression inner)
		{
			Inner = inner;
		}

		public MemberExpression Inner { get; private set; }

		public PropertyIfNotNullExpression Owner { get; set; }

		public override Type Type
		{
			get { return Inner.Type; }
		}

		protected override ExpressionType ConnectorExpressionType
		{
			get { return ExpressionType.PropertyIfNotNullInner; }
		}

		public override Expression Reduce()
		{
			return Inner;
		}

		public override string ToString()
		{
			return Inner.ToString();
		}
	}
}
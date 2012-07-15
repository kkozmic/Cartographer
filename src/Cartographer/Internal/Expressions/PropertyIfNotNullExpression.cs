namespace Cartographer.Internal.Expressions
{
	using System;
	using System.Linq.Expressions;

	[TechnicalDebt("This needs a far better name....")]
	public class PropertyIfNotNullExpression: ReducibleExpression
	{
		readonly Expression inner;

		readonly ParameterExpression local;

		readonly Expression owner;

		readonly Type targetValueType;

		public PropertyIfNotNullExpression(Expression owner, Expression inner, ParameterExpression local, Type targetValueType)
		{
			this.owner = owner;
			this.inner = inner;
			this.local = local;
			this.targetValueType = targetValueType;
		}

		public Expression Inner
		{
			get { return inner; }
		}

		public Expression Owner
		{
			get { return owner; }
		}

		public override Type Type
		{
			get { return targetValueType; }
		}

		protected override ExpressionType ConnectorExpressionType
		{
			get { return ExpressionType.PropertyIfNotNull; }
		}

		public override Expression Reduce()
		{
			var body = new Expression[]
			{
				Assign(local, owner),
				Condition(ReferenceNotEqual(local, Default(local.Type)),
				          inner,
				          Default(targetValueType),
				          targetValueType)
			};
			return Block(targetValueType, new[] { local }, body);
		}

		public override string ToString()
		{
			return owner + "?!" + inner;
		}
	}
}
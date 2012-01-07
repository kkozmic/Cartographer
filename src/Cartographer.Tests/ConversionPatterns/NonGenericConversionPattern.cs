namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class NonGenericConversionPattern: IConversionPattern<decimal, decimal>
	{
		public Expression<Func<decimal, IMapper, MappingContext, decimal>> BuildConversionExpression(MappingStep mapping)
		{
			return (d, m, c) => Decimal.Round(d, 2);
		}
	}
}
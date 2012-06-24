namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class ConvertConversionPattern<TTarget, TSource>: IConversionPattern<TSource, TTarget> where TSource: IConvertible
	{
		public Expression<Func<TSource, IMapper, MappingContext, TTarget>> BuildConversionExpression(MappingStep mapping)
		{
			return (d, m, c) => (TTarget)Convert.ChangeType(d, typeof (TTarget));
		}
	}
}
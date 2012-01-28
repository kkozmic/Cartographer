namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class NoOpConversionPattern<T>: IConversionPattern<T, T>
	{
		public Expression<Func<T, IMapper, MappingContext, T>> BuildConversionExpression(MappingStep mapping)
		{
			return (d, m, c) => d;
		}
	}
}
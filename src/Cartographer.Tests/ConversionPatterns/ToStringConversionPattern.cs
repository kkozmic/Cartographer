namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;
	

	public class ToStringConversionPattern<T>: IConversionPattern<T, string>
	{
		public Expression<Func<T, IMapper, MappingContext, string>> BuildConversionExpression(MappingStep mapping)
		{
			return (d, m, c) => d.ToString();
		}
	}
}
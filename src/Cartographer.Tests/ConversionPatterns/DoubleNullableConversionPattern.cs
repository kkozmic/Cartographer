namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class DoubleNullableConversionPattern<T1, T2>: IConversionPattern<T1?, T2?> where T1: struct
	                                                                                   where T2: struct
	{
		public Expression<Func<T1?, IMapper, MappingContext, T2?>> BuildConversionExpression(MappingStep mapping)
		{
			return (d, m, c) => d.HasValue ? m.Convert<T2>(d.Value) : default(T2);
		}
	}
}
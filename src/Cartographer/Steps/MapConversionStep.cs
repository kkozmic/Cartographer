namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Patterns;

	public class MapConversionPattern<TFrom, TTo>: ConversionPattern<TFrom, TTo>
	{
		protected override Expression<Func<TFrom, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping)
		{
			return (source, mapper, context) => mapper.Convert<TTo>(source);
		}
	}
}
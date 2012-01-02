namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Steps;

	public class MapConversionPattern<TTo>: ConversionPattern<object, TTo>
	{
		protected override Expression<Func<object, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping)
		{
			return (source, mapper, context) => mapper.Convert<TTo>(source);
		}
	}
}
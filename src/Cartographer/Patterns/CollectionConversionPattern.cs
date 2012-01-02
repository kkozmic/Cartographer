namespace Cartographer.Patterns
{
	using System;
	using System.Collections;
	using System.Linq.Expressions;
	using Cartographer.Helpers;
	using Cartographer.Steps;

	public class CollectionConversionPattern<TTargetItem>: ConversionPattern<IEnumerable, TTargetItem[]>
	{
		protected override Expression<Func<IEnumerable, IMapper, MappingContext, TTargetItem[]>> BuildConversionExpression(MappingStep mapping)
		{
			return (source, mapper, context) => CollectionConversionHelper.MapCollection<TTargetItem>(source, context);
		}
	}
}
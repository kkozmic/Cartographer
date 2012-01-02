namespace Cartographer.Steps
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Cartographer.Helpers;
	using Cartographer.Patterns;

	public class CollectionConversionPattern<TSourceItem, TTargetItem>: ConversionPattern<IEnumerable<TSourceItem>, IEnumerable<TTargetItem>>
	{
		protected override Expression<Func<IEnumerable<TSourceItem>, IMapper, MappingContext, IEnumerable<TTargetItem>>> BuildConversionExpression(MappingStep mapping)
		{
			return (source, mapper, context) => CollectionConversionHelper.MapCollection<TSourceItem, TTargetItem>(source, context);
		}
	}
}
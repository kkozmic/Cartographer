namespace Cartographer.Patterns
{
	using System;
	using System.Collections;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Helpers;
	using Cartographer.Steps;

	public class CollectionConversionPattern<TTargetItem>: IConversionPattern<IEnumerable, TTargetItem[]>, IRootConversionPattern
	{
		public Expression<Func<IEnumerable, IMapper, MappingContext, TTargetItem[]>> BuildConversionExpression(MappingStep mapping)
		{
			return (source, mapper, context) => CollectionConversionHelper.MapCollection<TTargetItem>(source, context);
		}
	}
}
namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Steps;

	public class MapConversionPattern<TTo>: IConversionPattern<object, TTo>
	{
		public Expression<Func<object, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping)
		{
			if (mapping.SourceValueType.Is(mapping.TargetValueType))
			{
				// no conversion needed
				return null;
			}

			return (source, mapper, context) => mapper.Convert<TTo>(source);
		}
	}
}
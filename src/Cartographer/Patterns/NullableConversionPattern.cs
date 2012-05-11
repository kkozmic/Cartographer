namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class NullableConversionPattern<T>: IConversionPattern<T, T?> where T: struct
	{
		public Expression<Func<T, IMapper, MappingContext, T?>> BuildConversionExpression(MappingStep mapping)
		{
			if (mapping.SourceValueType == typeof (T?))
			{
				return null;
			}

			return (d, m, c) => d; //implicit conversion
		}
	}
}
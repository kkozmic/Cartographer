namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public abstract class ConversionPattern<TFrom, TTo>: IConversionPattern
	{
		protected abstract Expression<Func<TFrom, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping);

		public void Apply(MappingStep mapping)
		{
			var expression = BuildConversionExpression(mapping);
			mapping.Conversion = new DelegatingConversionStep(expression);
		}
	}
}
namespace Cartographer.Patterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public abstract class ConversionPattern<TFrom, TTo>: IConversionPattern
	{
		/// <summary>
		/// Builds expression that will be used as a blueprint for converting matched steps.
		/// Should return <c>null</c> if the pattern should not be applied.
		/// </summary>
		/// <param name="mapping"></param>
		/// <returns></returns>
		protected abstract Expression<Func<TFrom, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping);

		public void Apply(MappingStep mapping)
		{
			var expression = BuildConversionExpression(mapping);
			if (expression != null)
			{
				mapping.Conversion = new DelegatingConversionStep(expression);
			}
		}
	}
}
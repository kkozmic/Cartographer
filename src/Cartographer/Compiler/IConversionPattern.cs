namespace Cartographer.Compiler
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Steps;

	public interface IConversionPattern<TFrom, TTo>
	{
		/// <summary>
		/// 	Builds expression that will be used as a blueprint for converting matched steps. Should return <c>null</c> if the pattern should not be applied.
		/// </summary>
		/// <param name="mapping"> </param>
		/// <returns> </returns>
		Expression<Func<TFrom, IMapper, MappingContext, TTo>> BuildConversionExpression(MappingStep mapping);
	}
}
namespace Cartographer.Compiler
{
	/// <summary>
	///   Marker interface applied to those <see cref="IConversionPattern{TFrom,TTo}" /> s that can directly convert root instance to target, not just their properties or parameters.
	/// </summary>
	public interface IRootConversionPattern
	{
	}
}
namespace Cartographer.Compiler
{
	using System;

	public interface IConversionPatternRepository
	{
		dynamic LeaseConversionPatternFor(Type sourceValueType, Type targetValueType);

		void Recycle(object conversionPattern);
	}
}
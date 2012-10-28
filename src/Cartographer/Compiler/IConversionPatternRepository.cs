namespace Cartographer.Compiler
{
	using System;

	public interface IConversionPatternRepository
	{
		object LeaseConversionPatternFor(Type sourceValueType, Type targetValueType);

		void Recycle(object conversionPattern);
	}
}
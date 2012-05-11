namespace Cartographer.Compiler
{
	using System;

	public interface IConversionPatternRepository
	{
		dynamic Lease(Type conversionPatternType);

		void Recycle(object conversionPattern);
	}
}
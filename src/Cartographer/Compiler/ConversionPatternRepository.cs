namespace Cartographer.Compiler
{
	using System;

	public class ConversionPatternRepository: IConversionPatternRepository
	{
		public dynamic Lease(Type conversionPatternType)
		{
			return Activator.CreateInstance(conversionPatternType);
		}

		public void Recycle(object conversionPattern)
		{
			// nothing to do here... well perhaps we could dispose of it...
		}
	}
}
namespace Cartographer.Compiler
{
	using System;

	public class ConversionPatternRepository: IConversionPatternRepository
	{
		readonly IConversionPatternGenericCloser conversionPatternGenericCloser;

		readonly Type[] conversionPatterns;


		public ConversionPatternRepository(IConversionPatternGenericCloser conversionPatternGenericCloser, params Type[] conversionPatterns)
		{
			this.conversionPatterns = conversionPatterns;
			this.conversionPatternGenericCloser = conversionPatternGenericCloser;
		}

		public object LeaseConversionPatternFor(Type sourceValueType, Type targetValueType)
		{
			foreach (var patternType in conversionPatterns)
			{
				var type = conversionPatternGenericCloser.Close(patternType, sourceValueType, targetValueType);
				if (type == null)
				{
					continue;
				}
				return Activator.CreateInstance(type);
			}
			return null;
		}

		public void Recycle(object conversionPattern)
		{
			// nothing to do here... well perhaps we could dispose of it...
		}
	}
}
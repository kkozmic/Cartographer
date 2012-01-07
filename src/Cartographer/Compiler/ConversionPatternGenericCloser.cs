namespace Cartographer.Compiler
{
	using System;
	using Cartographer.Internal;

	public class ConversionPatternGenericCloser: IConversionPatternGenericCloser
	{
		public Type Close(Type conversionPatternType, Type sourceType, Type targetType)
		{
			var @interface = conversionPatternType.GetInterface(typeof (IConversionPattern<,>).Name);
			if (@interface == null)
			{
				throw new ArgumentException(string.Format("Type {0} doesn't implement {1} and therefore is invalid for this operation.", conversionPatternType, typeof (IConversionPattern<,>)));
			}
			if (conversionPatternType.IsGenericType == false)
			{
				var arguments = @interface.GetGenericArguments();

				if (sourceType.Is(arguments[0]) && targetType.Is(arguments[1]))
				{
					return conversionPatternType;
				}
				return null;
			}
			throw new NotImplementedException();
		}
	}
}
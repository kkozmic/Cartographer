namespace Cartographer.Compiler
{
	using System;

	public interface IConversionPatternGenericCloser
	{
		Type Close(Type conversionPatternType, Type sourceType, Type targetType);
	}
}
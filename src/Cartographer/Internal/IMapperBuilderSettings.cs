namespace Cartographer.Internal
{
	using System;
	using System.IO;
	using Cartographer.Compiler;

	public interface IMapperBuilderSettings
	{
		Type[] ConversionPatternTypes { get; }

		Action<string, Exception> ErrorLog { get; set; }

		Action<string> InfoLog { get; set; }

		IMappingCompiler MappingCompiler { get; set; }

		IMappingDescriptor MappingDescriptor { get; set; }

		TextWriter MappingDescriptorWriter { get; set; }

		IMappingPattern[] MappingPatterns { get; }

		IMappingStrategyBuilder MappingStrategyBuilder { get; set; }

		ITypeMapper TypeMapper { get; set; }

		IConversionPatternGenericCloser ConversionPatternGenericCloser { get; set; }

		void AddConversionPatternType(Type conversionPatternType);

		void AddMappingPattern(IMappingPattern pattern);
	}
}
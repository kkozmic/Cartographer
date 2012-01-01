namespace Cartographer.Internal
{
	using System;
	using System.IO;
	using Cartographer.Compiler;

	public interface IMapperBuilderSettings
	{
		IConversionPattern[] ConversionPatterns { get; }

		Action<string, Exception> ErrorLog { get; set; }

		Action<string> InfoLog { get; set; }

		IMappingCompiler MappingCompiler { get; set; }

		IMappingDescriptor MappingDescriptor { get; set; }

		TextWriter MappingDescriptorWriter { get; set; }

		IMappingPattern[] MappingPatterns { get; }

		IMappingStrategyBuilder MappingStrategyBuilder { get; set; }

		ITypeMapper TypeMapper { get; set; }

		void AddConversionPattern(IConversionPattern pattern);

		void AddMappingPattern(IMappingPattern pattern);
	}
}
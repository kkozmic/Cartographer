namespace Cartographer.Internal
{
	using System;
	using System.IO;
	using Cartographer.Compiler;
	using Cartographer.Patterns;

	public interface IMapperBuilderSettings
	{
		MappingConverter[] ConversionPatterns { get; }

		Action<string, Exception> ErrorLog { get; set; }

		Action<string> InfoLog { get; set; }

		IMappingCompiler MappingCompiler { get; set; }

		IMappingDescriptor MappingDescriptor { get; set; }

		TextWriter MappingDescriptorWriter { get; set; }

		IMappingPattern[] MappingPatterns { get; }

		IMappingStrategyBuilder MappingStrategyBuilder { get; set; }

		ITypeMapper TypeMapper { get; set; }

		void AddConversionPattern(MappingConverter pattern);

		void AddMappingPattern(IMappingPattern pattern);
	}
}
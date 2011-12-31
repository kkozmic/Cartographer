namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Patterns;

	public class MapperBuilder: MapperBuilder.IMapperBuilderSettings
	{
		readonly List<IConversionPattern> conversionPatterns = new List<IConversionPattern>
		                                                       {
		                                                       	new CollectionConversionPattern(),
		                                                       	new MapConversionPattern()
		                                                       };

		readonly List<IMappingPattern> mappingPatterns = new List<IMappingPattern>
		                                                 {
		                                                 	new MatchByNameMappingPattern(),
		                                                 	new MatchByNameMappingPattern()
		                                                 };


		IMapper mapper;

		public IMapperBuilderSettings Settings
		{
			get { return this; }
		}

		IConversionPattern[] IMapperBuilderSettings.ConversionPatterns
		{
			get { return conversionPatterns.ToArray(); }
		}

		Action<string, Exception> IMapperBuilderSettings.ErrorLog { get; set; }

		Action<string> IMapperBuilderSettings.InfoLog { get; set; }

		IMappingCompiler IMapperBuilderSettings.MappingCompiler { get; set; }

		IMappingDescriptor IMapperBuilderSettings.MappingDescriptor { get; set; }

		TextWriter IMapperBuilderSettings.MappingDescriptorWriter { get; set; }

		IMappingPattern[] IMapperBuilderSettings.MappingPatterns
		{
			get { return mappingPatterns.ToArray(); }
		}

		IMappingStrategyBuilder IMapperBuilderSettings.MappingStrategyBuilder { get; set; }

		ITypeMapper IMapperBuilderSettings.TypeMapper { get; set; }

		public virtual IMapper BuildMapper()
		{
			if (mapper != null)
			{
				// log
				Settings.InfoLog.Invoke(MethodBase.GetCurrentMethod().Name + " Invoked more than once. Returning cached instance! Make sure you do not call this method multiple times.");
			}
			else
			{
				mapper = new Mapper(Settings.TypeMapper = Settings.TypeMapper ?? BuildTypeMapper(),
				                    Settings.MappingStrategyBuilder = Settings.MappingStrategyBuilder ?? BuildMappingStrategyBuilder(),
				                    Settings.MappingCompiler = Settings.MappingCompiler ?? BuildMappingCompiler());
			}
			return mapper;
		}

		protected virtual IMappingCompiler BuildMappingCompiler()
		{
			return new MappingCompiler();
		}

		protected virtual IMappingDescriptor BuildMappingDescriptor()
		{
			return new MappingDescriptor(Settings.MappingDescriptorWriter = Settings.MappingDescriptorWriter ?? BuildMappingDescriptorWriter());
		}

		protected virtual TextWriter BuildMappingDescriptorWriter()
		{
			return Console.Out;
		}

		protected virtual IMappingStrategyBuilder BuildMappingStrategyBuilder()
		{
			return new MappingStrategyBuilder(Settings.MappingDescriptor = Settings.MappingDescriptor ?? BuildMappingDescriptor(),
			                                  Settings.ConversionPatterns,
			                                  Settings.MappingPatterns);
		}

		protected virtual ITypeMapper BuildTypeMapper()
		{
			return new TypeMapper();
		}

		void IMapperBuilderSettings.AddConversionPattern(IConversionPattern pattern)
		{
			conversionPatterns.Insert(0, pattern);
		}

		void IMapperBuilderSettings.AddMappingPattern(IMappingPattern pattern)
		{
			mappingPatterns.Insert(0, pattern);
		}

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
}
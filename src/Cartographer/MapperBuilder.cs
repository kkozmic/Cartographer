namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Patterns;

	public class MapperBuilder
	{
		readonly MapperBuilderSettings settings;

		IMapper mapper;

		public MapperBuilder()
		{
			settings = new MapperBuilderSettings(this);
		}

		public MapperBuilderSettings Settings
		{
			get { return settings; }
		}

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

		public class MapperBuilderSettings
		{
			readonly MapperBuilder builder;


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

			public MapperBuilderSettings(MapperBuilder builder)
			{
				this.builder = builder;
				ErrorLog = (s, e) => Console.WriteLine("ERROR: {0} {1}", s, e);
				InfoLog = Console.WriteLine;
			}

			public IConversionPattern[] ConversionPatterns
			{
				get { return conversionPatterns.ToArray(); }
			}

			public Action<string, Exception> ErrorLog { get; set; }

			public Action<string> InfoLog { get; set; }

			public IMappingCompiler MappingCompiler { get; set; }

			public IMappingDescriptor MappingDescriptor { get; set; }

			public TextWriter MappingDescriptorWriter { get; set; }

			public IMappingPattern[] MappingPatterns
			{
				get { return mappingPatterns.ToArray(); }
			}

			public IMappingStrategyBuilder MappingStrategyBuilder { get; set; }

			public ITypeMapper TypeMapper { get; set; }

			public void AddConversionPattern(IConversionPattern pattern)
			{
				conversionPatterns.Insert(0, pattern);
			}

			public void AddMappingPattern(IMappingPattern pattern)
			{
				mappingPatterns.Insert(0, pattern);
			}
		}
	}
}
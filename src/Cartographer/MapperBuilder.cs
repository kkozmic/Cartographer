namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Patterns;

	public class MapperBuilder
	{
		IMapper mapper;

		public MapperBuilder()
		{
			Settings = new MapperBuilderSettings();
		}

		public MapperBuilderSettings Settings { get; set; }

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

		protected virtual IConversionPatternGenericCloser BuildConversionPatternGenericCloser()
		{
			return new ConversionPatternGenericCloser();
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
			                                  Settings.ConversionPatternGenericCloser ?? BuildConversionPatternGenericCloser(),
			                                  Settings.ConversionPatternTypes,
			                                  Settings.MappingPatterns);
		}

		protected virtual ITypeMapper BuildTypeMapper()
		{
			return new TypeMapper();
		}

		public class MapperBuilderSettings
		{
			readonly List<Type> conversionPatterns = new List<Type>
			                                         {
			                                         	typeof (CollectionConversionPattern<>),
			                                         	typeof (MapConversionPattern<>),
			                                         	typeof (NullableConversionPattern<>)
			                                         };

			readonly List<IMappingPattern> mappingPatterns = new List<IMappingPattern>
			                                                 {
			                                                 	new MatchByNameMappingPattern(),
			                                                 	new MatchByNameFlattenMappingPattern()
			                                                 };

			public MapperBuilderSettings(params IMappingPattern[] mappingPatterns)
			{
				this.mappingPatterns.InsertRange(0, mappingPatterns);
			}

			public IConversionPatternGenericCloser ConversionPatternGenericCloser { get; set; }

			public Type[] ConversionPatternTypes
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

			public void AddConversionPatternType(Type conversionPatternType)
			{
				if (conversionPatternType.IsGenericInterface(typeof (IConversionPattern<,>)) == false)
				{
					throw new InvalidOperationException(string.Format("Type {0} is not a valid conversion pattern type. Type must implement {1}.", conversionPatternType, typeof (IConversionPattern<,>)));
				}
				conversionPatterns.Insert(0, conversionPatternType);
			}

			public void AddMappingPattern(IMappingPattern pattern)
			{
				mappingPatterns.Insert(0, pattern);
			}
		}
	}
}
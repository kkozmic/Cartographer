namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Patterns;

	public class MapperBuilder
	{
		readonly List<IMappingCatalog> catalogs = new List<IMappingCatalog>();

		IMapper mapper;

		public MapperBuilder()
		{
			Settings = new MapperBuilderSettings();
		}

		public MapperBuilderSettings Settings { get; set; }

		public MapperBuilder AddCatalogs(params IMappingCatalog[] catalogs)
		{
			this.catalogs.AddRange(catalogs);
			return this;
		}

		public MapperBuilder AddCatalogsFromAssembly(Assembly assembly)
		{
			var types = assembly.GetAvailableTypes()
				.Where(t => t.IsClass && t.IsAbstract == false && typeof (IMappingCatalog).IsAssignableFrom(t));
			var mappingCatalogs = types.Select(t => Activator.CreateInstance(t) as IMappingCatalog).ToArray();
			return AddCatalogs(mappingCatalogs);
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
				var mapperLocal = new Mapper(Settings.MappingStrategyBuilder = Settings.MappingStrategyBuilder ?? BuildMappingStrategyBuilder(),
				                             Settings.MappingCompiler = Settings.MappingCompiler ?? BuildMappingCompiler(),
				                             Settings.TypeMatchers = Settings.TypeMatchers ?? BuildTypeMatchers());
				var mappings = new List<MappingInfo>();
				var bag = new MappingBag(mappings);
				foreach (var catalog in catalogs)
				{
					catalog.Collect(bag);
				}
				mappings.Sort();
				foreach (var mapping in mappings)
				{
					mapperLocal.RegisterMapping(mapping);
				}
				mapper = mapperLocal;
			}
			return mapper;
		}

		protected virtual IConversionPatternGenericCloser BuildConversionPatternGenericCloser()
		{
			return new ConversionPatternGenericCloser();
		}

		protected virtual IConversionPatternRepository BuildConversionPatternRepository()
		{
			return new ConversionPatternRepository(Settings.ConversionPatternGenericCloser = Settings.ConversionPatternGenericCloser ?? BuildConversionPatternGenericCloser(),
			                                       Settings.ConversionPatternTypes);
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
			                                  Settings.ConversionPatternRepository = Settings.ConversionPatternRepository ?? BuildConversionPatternRepository(),
			                                  Settings.MappingPatterns);
		}

		protected virtual ITypeMatcher[] BuildTypeMatchers()
		{
			return new ITypeMatcher[0];
		}

		public class MapperBuilderSettings
		{
			readonly List<Type> conversionPatterns = new List<Type>
			                                         {
			                                         	typeof (CollectionConversionPattern<>),
			                                         	//typeof (MapConversionPattern<>),
			                                         	typeof (NullableConversionPattern<>)
			                                         };

			readonly List<IMappingPattern> mappingPatterns = new List<IMappingPattern>
			                                                 {
			                                                 	new MatchByNameMappingPattern(),
			                                                 	new MatchByNameFlattenMappingPattern()
			                                                 };

			public MapperBuilderSettings()
			{
			}

			public MapperBuilderSettings(params IMappingPattern[] mappingPatterns)
			{
				this.mappingPatterns.InsertRange(0, mappingPatterns);
			}

			public IConversionPatternGenericCloser ConversionPatternGenericCloser { get; set; }

			public IConversionPatternRepository ConversionPatternRepository { get; set; }

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

			public ITypeMatcher[] TypeMatchers { get; set; }

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
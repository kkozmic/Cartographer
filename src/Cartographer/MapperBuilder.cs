﻿namespace Cartographer
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Contributors;
	using Cartographer.Internal;
	using Cartographer.Internal.Extensions;
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
				Settings.InfoLog.Invoke("MapperBuilder.BuildMapper() invoked more than once. Returning cached instance! Make sure you do not call this method multiple times.");
			}
			else
			{
				var mapperLocal = new Mapper(Settings.MappingStrategyBuilder = Settings.MappingStrategyBuilder ?? BuildMappingStrategyBuilder(),
				                             Settings.MappingCompiler = Settings.MappingCompiler ?? BuildMappingCompiler(),
				                             BuildTypeMatchers(Settings.TypeMatchers));
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
			var console = typeof (Object).Assembly.GetType("System.Console");
			if (console == null)
			{
				return TextWriter.Null;
			}
			var consoleOut = console.GetProperty("Out", BindingFlags.Public | BindingFlags.Static);
			return (TextWriter)consoleOut.GetValue(null, null);
		}

		protected virtual IMappingPattern[] BuildMappingPatterns(IMappingPattern[] customPatterns)
		{
			var patterns = new List<IMappingPattern>
			{
				new MatchByNameMappingPattern(),
				new MatchByNameFlattenMappingPattern()
			};
			if (customPatterns != null && customPatterns.Length > 0)
			{
				patterns.AddRange(customPatterns);
			}
			return patterns.ToArray();
		}

		protected virtual IMappingStrategyBuilder BuildMappingStrategyBuilder()
		{
			return new MappingStrategyBuilder(Settings.ConversionPatternRepository = Settings.ConversionPatternRepository ?? BuildConversionPatternRepository(),
			                                  BuildMappingStrategyContributors(Settings.MappingStategyContributors));
		}

		protected virtual IMappingStrategyContributor[] BuildMappingStrategyContributors(IMappingStrategyContributor[] customContributors)
		{
			var contributors = new List<IMappingStrategyContributor>
			{
				new DescriptorContributor(Settings.MappingDescriptor = Settings.MappingDescriptor ?? BuildMappingDescriptor()),
				new HarcodedMappingContributor(),
				new ConstructorContributor(),
				new ApplyMappingSteps(BuildMappingPatterns(Settings.MappingPatterns)),
				new ApplyConverters(),
				new InitTarget(),
			};
			if (customContributors != null && customContributors.Length > 0)
			{
				contributors.AddRange(customContributors);
			}
			return contributors.ToArray();
		}

		protected virtual ITypeMatcher[] BuildTypeMatchers(ITypeMatcher[] customMatchers)
		{
			var matchers = new List<ITypeMatcher>
			{
			};
			if (customMatchers != null && customMatchers.Length > 0)
			{
				matchers.AddRange(customMatchers);
			}
			return matchers.ToArray();
		}

		public class MapperBuilderSettings
		{
			readonly List<Type> conversionPatterns = new List<Type>
			{
				typeof (CollectionConversionPattern<>),
				typeof (NullableConversionPattern<>)
			};

			public MapperBuilderSettings()
			{
				ErrorLog = (t, e) => { };
				InfoLog = t => { };
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

			public IMappingPattern[] MappingPatterns { get; set; }

			public IMappingStrategyContributor[] MappingStategyContributors { get; set; }

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

			public void AddConversionPatternTypes(params Type[] conversionPatternTypes)
			{
				foreach (var type in conversionPatternTypes)
				{
					AddConversionPatternType(type);
				}
			}
		}
	}
}
namespace Cartographer
{
	using System;
	using System.Collections.Concurrent;
	using System.ComponentModel;
	using Cartographer.Compiler;
	using Cartographer.Internal;

	public class Mapper: IMapper
	{
		readonly IMappingCompiler mappingCompiler;

		readonly IMappingStrategyBuilder mappingStrategyBuilder;

		readonly ConcurrentDictionary<MappingInfo, Delegate> mappins = new ConcurrentDictionary<MappingInfo, Delegate>();

		readonly ITypeMatcher[] typeMatchers;

		/// <summary>
		///   It is not recommended to use the constructor directly. Use <see cref="MapperBuilder" /> instead to create your instance of <see
		///    cref="Mapper" /> .
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Mapper(IMappingStrategyBuilder mappingStrategyBuilder, IMappingCompiler mappingCompiler, ITypeMatcher[] typeMatchers)
		{
			this.typeMatchers = typeMatchers;
			this.mappingStrategyBuilder = mappingStrategyBuilder;
			this.mappingCompiler = mappingCompiler;
		}

		public TTarget Convert<TTarget>(object source)
		{
			return ConvertWithArguments<TTarget>(source, null);
		}

		public TTarget Convert<TTarget>(object source, TTarget target)
		{
			return ConvertWithArguments(source, target, null);
		}

		public TTarget ConvertWithArguments<TTarget>(object source, object inlineArgumentsAsAnonymousType)
		{
			var key = Match(source.GetType(), typeof (TTarget), null);
			var mapper = (Func<MappingContext, TTarget>)mappins.GetOrAdd(key, CreateMapping);

			return mapper.Invoke(new MappingContext(new Arguments(inlineArgumentsAsAnonymousType))
			                     {
			                     	SourceInstance = source,
			                     	Mapper = this
			                     });
		}


		public TTarget ConvertWithArguments<TTarget>(object source, TTarget target, object inlineArgumentsAsAnonymousType)
		{
			var key = Match(source.GetType(), typeof (TTarget), GetType(target));
			var mapper = (Func<MappingContext, TTarget>)mappins.GetOrAdd(key, CreateMapping);

			return mapper.Invoke(new MappingContext(new Arguments(inlineArgumentsAsAnonymousType))
			                     {
			                     	SourceInstance = source,
			                     	TargetInstance = target,
			                     	Mapper = this
			                     });
		}

		internal void RegisterMapping(MappingInfo mappingInfo)
		{
			// this is temporary (famous last words)
			var key = Match(mappingInfo);
			mappins.GetOrAdd(key, CreateMapping);
		}

		Delegate CreateMapping(MappingInfo mappingInfo)
		{
			var strategy = mappingStrategyBuilder.BuildMappingStrategy(mappingInfo);
			return mappingCompiler.Compile(strategy);
		}

		MappingInfo Match(Type actualSourceType, Type targetConstrtaintType, Type actualTargetType)
		{
			var cacheKey = new MappingInfo(actualSourceType, targetConstrtaintType, actualTargetType);
			return Match(cacheKey);
		}

		MappingInfo Match(MappingInfo cacheKey)
		{
			foreach (var matcher in typeMatchers)
			{
				if (matcher.Match(cacheKey))
				{
					return cacheKey;
				}
			}
			// we fallback to default behaviour
			return cacheKey;
		}

		static Type GetType(object item)
		{
			if (item == null)
			{
				return null;
			}
			return item.GetType();
		}
	}
}
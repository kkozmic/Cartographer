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

		readonly ITypeMapper typeMapper;

		/// <summary>
		///   It is not recommended to use the constructor directly. Use <see cref="MapperBuilder" /> instead to create your instance of <see
		///    cref="Mapper" /> .
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Mapper(ITypeMapper typeMapper, IMappingStrategyBuilder mappingStrategyBuilder, IMappingCompiler mappingCompiler)
		{
			this.typeMapper = typeMapper;
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
			var key = typeMapper.GetMappingInfo(source.GetType(), typeof (TTarget), false);
			var mapper = (Func<MappingContext, TTarget>)mappins.GetOrAdd(key, CreateMapping);

			return mapper.Invoke(new MappingContext(new Arguments(inlineArgumentsAsAnonymousType))
			                     {
			                     	TargetType = key.Target,
			                     	SourceInstance = source,
			                     	Mapper = this
			                     });
		}


		public TTarget ConvertWithArguments<TTarget>(object source, TTarget target, object inlineArgumentsAsAnonymousType)
		{
			var key = typeMapper.GetMappingInfo(source.GetType(), typeof (TTarget), Equals(target, default(TTarget)) == false);
			var mapper = (Func<MappingContext, TTarget>)mappins.GetOrAdd(key, CreateMapping);

			return mapper.Invoke(new MappingContext(new Arguments(inlineArgumentsAsAnonymousType))
			                     {
			                     	TargetType = key.Target,
			                     	SourceInstance = source,
			                     	TargetInstance = target,
			                     	Mapper = this
			                     });
		}

		Delegate CreateMapping(MappingInfo arg)
		{
			var strategy = mappingStrategyBuilder.BuildMappingStrategy(arg);
			return mappingCompiler.Compile(strategy);
		}
	}
}
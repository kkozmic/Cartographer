namespace Cartographer
{
	using System;
	using System.Collections.Concurrent;

	public class Mapper: IMapper
	{
		readonly IMappingBuilder mappingBuilder;

		readonly IMappingCompiler mappingCompiler;

		readonly ConcurrentDictionary<MappingKey, Delegate> mappins = new ConcurrentDictionary<MappingKey, Delegate>();

		readonly ITypeMapper typeMapper;

		public Mapper(ITypeMapper typeMapper, IMappingBuilder mappingBuilder, IMappingCompiler mappingCompiler)
		{
			this.typeMapper = typeMapper;
			this.mappingBuilder = mappingBuilder;
			this.mappingCompiler = mappingCompiler;
		}

		public TResult Convert<TResult>(object source)
		{
			var key = typeMapper.GetMappingKey(source.GetType(), typeof (TResult));
			var mapper = (Func<MappingContext, TResult>)mappins.GetOrAdd(key, CreateMapping);

			return mapper.Invoke(new MappingContext
			                     {
			                     	TargetType = key.Target,
			                     	SourceInstance = source,
			                     	Mapper = this
			                     });
		}

		MappingStrategy BuildStrategy(MappingKey arg)
		{
			var mappingStrategy = mappingBuilder.BuildMappingStrategy(arg.Source, arg.Target);
			return mappingStrategy;
		}

		Delegate CreateMapping(MappingKey arg)
		{
			var mappingStrategy = BuildStrategy(arg);
			return mappingCompiler.Compile(mappingStrategy);
		}
	}
}
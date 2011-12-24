namespace Cartographer
{
	using System;
	using System.Collections.Concurrent;

	public class Mapper: IMapper
	{
		readonly IMappingBuilder mappingBuilder;

		readonly IMappingCompiler mappingCompiler;

		readonly ConcurrentDictionary<MappingKey, Delegate> mappins = new ConcurrentDictionary<MappingKey, Delegate>();

		readonly ITypeModelBuilder modelBuilder;

		readonly ITypeMapper typeMapper;

		public Mapper(ITypeMapper typeMapper, ITypeModelBuilder modelBuilder, IMappingBuilder mappingBuilder, IMappingCompiler mappingCompiler)
		{
			this.typeMapper = typeMapper;
			this.modelBuilder = modelBuilder;
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
			var sourceModel = modelBuilder.BuildModel(arg.Source);
			var targetModel = modelBuilder.BuildModel(arg.Target);

			var mappingStrategy = mappingBuilder.BuildMappingStrategy(sourceModel, targetModel);
			return mappingStrategy;
		}

		Delegate CreateMapping(MappingKey arg)
		{
			var mappingStrategy = BuildStrategy(arg);
			return mappingCompiler.Compile(mappingStrategy);
		}
	}
}
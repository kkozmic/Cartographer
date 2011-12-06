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
			var sourceType = source.GetType();

			var key = typeMapper.GetMappingKey(sourceType, typeof (TResult));

			dynamic mapper = mappins.GetOrAdd(key, CreateMapping);
			var mappingContext = new MappingContext { TargetType = key.Target, SourceInstance = source, Mapper = this };

			return mapper.Invoke(mappingContext);
		}


		Delegate CreateMapping(MappingKey arg)
		{
			var sourceModel = modelBuilder.BuildModel(arg.Source);
			var targetModel = modelBuilder.BuildModel(arg.Target);

			var mappingStrategy = mappingBuilder.BuildMappingStrategy(sourceModel, targetModel);
			return mappingCompiler.Compile(mappingStrategy);
		}
	}
}

namespace Cartographer
{
	using System;
	using System.Collections.Concurrent;

	public class Mapper: IMapper
	{
		readonly IMappingBuilder mappingBuilder;

		readonly ConcurrentDictionary<Tuple<Type, Type>, Delegate> mappins = new ConcurrentDictionary<Tuple<Type, Type>, Delegate>();

		readonly ITypeModelBuilder modelBuilder;
		readonly ITypeMapper typeMapper;


		public Mapper(ITypeMapper typeMapper, ITypeModelBuilder modelBuilder, IMappingBuilder mappingBuilder)
		{
			this.typeMapper = typeMapper;
			this.modelBuilder = modelBuilder;
			this.mappingBuilder = mappingBuilder;
		}


		public TResult Convert<TResult>(object source)
		{
			var sourceType = source.GetType();

			var targetType = typeMapper.GetTargetType(sourceType, typeof (TResult));
			var mappingContext = new MappingContext { TargetType = targetType };

			dynamic mapper = mappins.GetOrAdd(Tuple.Create(sourceType, targetType), CreateMapping);

			return mapper.Invoke(source, mappingContext);
		}


		Delegate CreateMapping(Tuple<Type, Type> arg)
		{
			var sourceModel = modelBuilder.BuildModel(arg.Item1);

			var targetModel = modelBuilder.BuildModel(arg.Item2);


			return mappingBuilder.BuildMapping(sourceModel, targetModel);
		}
	}
}

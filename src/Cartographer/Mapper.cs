using System;
using System.Collections.Concurrent;

namespace Cartographer
{
	public class Mapper : IMapper
	{
		private readonly IMappingBuilder mappingBuilder;

		private readonly ConcurrentDictionary<Tuple<Type, Type>, Delegate> mappins =
			new ConcurrentDictionary<Tuple<Type, Type>, Delegate>();

		private readonly ITypeModelBuilder modelBuilder;
		private readonly ITypeMapper typeMapper;


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

			dynamic mapper = mappins.GetOrAdd(Tuple.Create(sourceType, targetType), CreateMapping);

			return mapper.Invoke(source);
		}


		private Delegate CreateMapping(Tuple<Type, Type> arg)

		{
			var sourceModel = modelBuilder.BuildModel(arg.Item1);

			var targetModel = modelBuilder.BuildModel(arg.Item2);


			return mappingBuilder.BuildMapping(sourceModel, targetModel);
		}
	}
}
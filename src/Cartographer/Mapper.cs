﻿namespace Cartographer
{
	using System;
	using System.Collections.Concurrent;
	using System.ComponentModel;
	using Cartographer.Compiler;

	public class Mapper: IMapper
	{
		readonly IMappingCompiler mappingCompiler;

		readonly IMappingStrategyBuilder mappingStrategyBuilder;

		readonly ConcurrentDictionary<MappingKey, Delegate> mappins = new ConcurrentDictionary<MappingKey, Delegate>();

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
			var mappingStrategy = mappingStrategyBuilder.BuildMappingStrategy(arg.Source, arg.Target);
			return mappingStrategy;
		}

		Delegate CreateMapping(MappingKey arg)
		{
			var mappingStrategy = BuildStrategy(arg);
			return mappingCompiler.Compile(mappingStrategy);
		}
	}
}
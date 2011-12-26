namespace Cartographer.Compiler
{
	using System;

	public interface IMappingStrategyBuilder
	{
		MappingStrategy BuildMappingStrategy(Type source, Type target);
	}
}
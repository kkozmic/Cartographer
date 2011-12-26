namespace Cartographer
{
	using System;

	public interface IMappingBuilder
	{
		MappingStrategy BuildMappingStrategy(Type source, Type target);
	}
}
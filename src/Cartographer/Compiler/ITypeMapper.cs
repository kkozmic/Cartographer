namespace Cartographer.Compiler
{
	using System;

	public interface ITypeMapper
	{
		MappingKey GetMappingKey(Type sourceInstanceType, Type requestedTargetType);
	}
}
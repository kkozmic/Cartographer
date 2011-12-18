namespace Cartographer
{
	using System;

	public interface ITypeMapper
	{
		MappingKey GetMappingKey(Type sourceInstanceType, Type requestedTargetType);
	}
}
namespace Cartographer.Compiler
{
	using System;

	public interface ITypeMapper
	{
		MappingInfo GetMappingKey(Type sourceInstanceType, Type requestedTargetType, bool preexistingTargetInstance);
	}
}
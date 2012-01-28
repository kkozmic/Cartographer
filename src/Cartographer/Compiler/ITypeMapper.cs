namespace Cartographer.Compiler
{
	using System;

	public interface ITypeMapper
	{
		MappingInfo GetMappingInfo(Type sourceInstanceType, Type requestedTargetType, bool preexistingTargetInstance);
	}
}
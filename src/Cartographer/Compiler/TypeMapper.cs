namespace Cartographer.Compiler
{
	using System;

	public class TypeMapper: ITypeMapper
	{
		public MappingKey GetMappingKey(Type sourceInstanceType, Type requestedTargetType, bool preexistingTargetInstance)
		{
			return new MappingKey(sourceInstanceType, requestedTargetType, preexistingTargetInstance);
		}
	}
}
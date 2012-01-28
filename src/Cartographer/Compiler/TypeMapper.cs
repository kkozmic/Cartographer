namespace Cartographer.Compiler
{
	using System;

	public class TypeMapper: ITypeMapper
	{
		public MappingInfo GetMappingInfo(Type sourceInstanceType, Type requestedTargetType, bool preexistingTargetInstance)
		{
			return new MappingInfo(sourceInstanceType, requestedTargetType, preexistingTargetInstance);
		}
	}
}
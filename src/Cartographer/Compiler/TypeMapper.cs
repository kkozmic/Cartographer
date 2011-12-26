namespace Cartographer.Compiler
{
	using System;

	public class TypeMapper: ITypeMapper
	{
		public MappingKey GetMappingKey(Type sourceInstanceType, Type requestedTargetType)
		{
			return new MappingKey(sourceInstanceType, requestedTargetType);
		}
	}
}
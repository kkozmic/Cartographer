namespace Cartographer
{
	using System;

	public interface ITypeMapper
	{
		Type GetTargetType(Type sourceType, Type requestedTargetType);
	}
}

using System;

namespace Cartographer
{
	public interface ITypeMapper

	{
		Type GetTargetType(Type sourceType, Type requestedTargetType);
	}
}
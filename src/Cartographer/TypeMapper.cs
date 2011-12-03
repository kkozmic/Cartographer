namespace Cartographer
{
	using System;

	public class TypeMapper: ITypeMapper
	{
		public Type GetTargetType(Type sourceType, Type requestedTargetType)
		{
			return requestedTargetType;
		}
	}
}
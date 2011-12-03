namespace Cartographer
{
	using System;

	public interface IMappingCompiler
	{
		Delegate Compile(MappingStrategy strategy);
	}
}

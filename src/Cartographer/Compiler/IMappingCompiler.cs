namespace Cartographer.Compiler
{
	using System;

	public interface IMappingCompiler
	{
		Delegate Compile(MappingStrategy strategy);
	}
}
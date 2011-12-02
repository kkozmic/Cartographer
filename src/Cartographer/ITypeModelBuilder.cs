namespace Cartographer
{
	using System;

	public interface ITypeModelBuilder
	{
		TypeModel BuildModel(Type type);
	}
}

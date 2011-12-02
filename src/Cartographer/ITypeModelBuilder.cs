using System;

namespace Cartographer
{
	public interface ITypeModelBuilder

	{
		TypeModel BuildModel(Type type);
	}
}
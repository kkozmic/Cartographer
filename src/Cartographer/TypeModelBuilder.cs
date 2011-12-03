namespace Cartographer
{
	using System;

	public class TypeModelBuilder: ITypeModelBuilder
	{
		public TypeModel BuildModel(Type type)
		{
			return new TypeModel
			       {
			       	Properties = type.GetProperties(),
			       	Type = type
			       };
		}
	}
}

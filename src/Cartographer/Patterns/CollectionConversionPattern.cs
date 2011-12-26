namespace Cartographer.Patterns
{
	using System;
	using Cartographer.Compiler;
	using Cartographer.Internal;
	using Cartographer.Steps;

	public class CollectionConversionPattern: IConversionPattern
	{
		public void Apply(MappingStep mapping)
		{
			if (IsSupportedCollectionType(mapping.SourceProperty.PropertyType) &&
			    IsSupportedCollectionType(mapping.TargetProperty.PropertyType))
			{
				mapping.Conversion = new CollectionConversionStep();
			}
		}

		bool IsSupportedCollectionType(Type propertyType)
		{
			return propertyType.GetArrayItemType() != null;
		}
	}
}
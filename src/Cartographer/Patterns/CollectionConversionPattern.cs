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
			var sourceItem = mapping.SourceProperty.PropertyType.GetArrayItemType();
			var targetItem = mapping.TargetProperty.PropertyType.GetArrayItemType();
			if (sourceItem == null || targetItem == null)
			{
				return;
			}

			// NOTE: this is temporary hack to ensure the underlying infrastructure is operating.
			var instance = (IConversionPattern)Activator.CreateInstance(typeof (CollectionConversionPattern<,>).MakeGenericType(sourceItem, targetItem));
			instance.Apply(mapping);
		}
	}
}
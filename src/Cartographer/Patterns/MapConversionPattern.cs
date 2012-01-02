namespace Cartographer.Patterns
{
	using System;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class MapConversionPattern: IConversionPattern
	{
		public void Apply(MappingStep mapping)
		{
			if (mapping.TargetValueType.IsAssignableFrom(mapping.SourceValueType) || mapping.Conversion != null)
			{
				return;
			}
			// NOTE: this is temporary hack to ensure the underlying infrastructure is operating.
			var instance = (IConversionPattern)Activator.CreateInstance(typeof (MapConversionPattern<,>).MakeGenericType(mapping.SourceValueType, mapping.TargetValueType));
			instance.Apply(mapping);
		}
	}
}
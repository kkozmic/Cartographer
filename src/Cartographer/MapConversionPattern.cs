namespace Cartographer
{
	using Cartographer.Steps;

	public class MapConversionPattern: IConversionPattern
	{
		public void Apply(MappingStep mapping)
		{
			if (mapping.TargetValueType.IsAssignableFrom(mapping.SourceValueType))
			{
				return;
			}
			mapping.Conversion = new MapConversionStep();
		}
	}
}

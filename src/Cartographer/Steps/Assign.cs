namespace Cartographer.Steps
{
	using System.Reflection;

	public class Assign: MappingStep
	{
		readonly PropertyInfo sourceProperty;
		readonly PropertyInfo targetProperty;

		public Assign(PropertyInfo targetProperty, PropertyInfo sourceProperty)
		{
			this.targetProperty = targetProperty;
			this.sourceProperty = sourceProperty;
		}

		public PropertyInfo SourceProperty
		{
			get { return sourceProperty; }
		}

		public PropertyInfo TargetProperty
		{
			get { return targetProperty; }
		}
	}
}

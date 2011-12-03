namespace Cartographer.Steps
{
	using System.Reflection;

	public class AssignChain: MappingStep
	{
		readonly PropertyInfo[] sourcePropertyChain;
		readonly PropertyInfo targetProperty;

		public AssignChain(PropertyInfo targetProperty, PropertyInfo[] sourcePropertyChain)
		{
			this.targetProperty = targetProperty;
			this.sourcePropertyChain = sourcePropertyChain;
		}

		public PropertyInfo[] SourcePropertyChain
		{
			get { return sourcePropertyChain; }
		}

		public PropertyInfo TargetProperty
		{
			get { return targetProperty; }
		}
	}
}

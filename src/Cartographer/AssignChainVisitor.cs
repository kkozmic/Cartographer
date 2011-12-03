namespace Cartographer
{
	using Cartographer.Steps;

	public class AssignChainVisitor: IMappingVisitor<AssignChain>
	{
		public void Visit(AssignChain step, object source, object target, MappingContext context, MappingStrategy strategy)
		{
			var value = source;
			foreach (var property in step.SourcePropertyChain)
			{
				value = property.GetValue(value, null);
				if (value == null)
				{
					break;
				}
			}
			if (value != null)
			{
				step.TargetProperty.SetValue(target, value, null);
			}
		}
	}
}

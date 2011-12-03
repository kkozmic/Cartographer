namespace Cartographer.Visitors
{
	using Cartographer.Steps;

	public class AssignChainVisitor: IMappingVisitor<AssignChain>
	{
		public void Visit(AssignChain step, MappingContext context)
		{
			var value = context.SourceInstance;
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
				step.TargetProperty.SetValue(context.TargetInstance, value, null);
			}
		}
	}
}

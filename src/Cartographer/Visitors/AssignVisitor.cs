namespace Cartographer.Visitors
{
	using Cartographer.Steps;

	public class AssignVisitor: IMappingVisitor<Assign>
	{
		public void Visit(Assign step, MappingContext context)
		{
			var value = step.SourceProperty.GetValue(context.SourceInstance, null);
			step.TargetProperty.SetValue(context.TargetInstance, value, null);
		}
	}
}

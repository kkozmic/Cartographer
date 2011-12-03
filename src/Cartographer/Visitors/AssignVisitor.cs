namespace Cartographer.Visitors
{
	using Cartographer.Steps;

	public class AssignVisitor: IMappingVisitor<Assign>
	{
		public void Visit(Assign step, object source, object target, MappingContext context, MappingStrategy strategy)
		{
			var value = step.SourceProperty.GetValue(source, null);
			step.TargetProperty.SetValue(target, value, null);
		}
	}
}

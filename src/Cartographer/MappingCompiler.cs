namespace Cartographer
{
	using System;

	using Cartographer.Steps;

	public class MappingCompiler: IMappingCompiler
	{
		public Delegate Compile(MappingStrategy strategy)
		{
			return new Func<object, MappingContext, dynamic>((s, c) => Map(s, c, strategy));
		}

		object Map(object source, MappingContext context, MappingStrategy strategy)
		{
			var target = Activator.CreateInstance(context.TargetType);

			foreach (var step in strategy.MappingSteps)
			{
				var assign = DowncastToAssign(step);
				var value = assign.SourceProperty.GetValue(source, null);
				assign.TargetProperty.SetValue(target, value, null);
			}
			return target;
		}

		static Assign DowncastToAssign(MappingStep step)
		{
			var assign = step as Assign;
			if (assign == null)
			{
				throw new NotSupportedException(string.Format("non-assign steps, like {0}  are not supported yet.", step));
			}
			return assign;
		}
	}
}

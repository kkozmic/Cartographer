namespace Cartographer
{
	using System;
	using System.Collections.Generic;

	using Cartographer.Steps;

	public class MappingCompiler: IMappingCompiler
	{
		readonly IDictionary<Type, object> visitors = new Dictionary<Type, object>();

		public MappingCompiler(params object[] mappingVisitors)
		{
			foreach (dynamic visitor in mappingVisitors)
			{
				AddVisitor(visitor);
			}
		}

		public Delegate Compile(MappingStrategy strategy)
		{
			return new Func<object, MappingContext, dynamic>((s, c) => Map(s, c, strategy));
		}

		void AddVisitor<TStep>(IMappingVisitor<TStep> visitor) where TStep: MappingStep
		{
			visitors.Add(typeof (TStep), visitor);
		}

		IMappingVisitor<TStep> GetVisitor<TStep>() where TStep: MappingStep
		{
			object visitor;
			if (visitors.TryGetValue(typeof (TStep), out visitor) == false)
			{
				throw new NotSupportedException(string.Format("Step {0} is not supported.", typeof (TStep)));
			}
			return (IMappingVisitor<TStep>)visitor;
		}

		object Map(object source, MappingContext context, MappingStrategy strategy)
		{
			var target = Activator.CreateInstance(context.TargetType);
			foreach (dynamic step in strategy.MappingSteps)
			{
				Visit(step, target, source, context, strategy);
			}
			return target;
		}

		void Visit<TStep>(TStep step, object target, object source, MappingContext context, MappingStrategy strategy) where TStep: MappingStep
		{
			var visitor = GetVisitor<TStep>();
			visitor.Visit(step, source, target, context, strategy);
		}
	}
}

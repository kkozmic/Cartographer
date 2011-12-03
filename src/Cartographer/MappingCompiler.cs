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
			return new Func<MappingContext, dynamic>(c => Map(c, strategy));
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

		object Map(MappingContext context, MappingStrategy strategy)
		{
			var target = Activator.CreateInstance(context.TargetType);
			context.TargetInstance = target;
			foreach (dynamic step in strategy.MappingSteps)
			{
				Visit(step, context);
			}
			return target;
		}

		void Visit<TStep>(TStep step, MappingContext context) where TStep: MappingStep
		{
			var visitor = GetVisitor<TStep>();
			visitor.Visit(step, context);
		}
	}
}

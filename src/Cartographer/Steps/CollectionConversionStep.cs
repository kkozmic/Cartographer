namespace Cartographer.Steps
{
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;
	using Cartographer.Helpers;
	using Cartographer.Internal;

	public class CollectionConversionStep: ConversionStep
	{
		static readonly MethodInfo MapCollection =
			typeof (CollectionConversionHelper).GetMethods(BindingFlags.Static | BindingFlags.Public)
				.Single();

		public override Expression BuildConversionExpression(MappingStrategy context, MappingStep step)
		{
			var from = step.SourceValueType.GetArrayItemType();
			var to = step.TargetValueType.GetArrayItemType();
			Debug.Assert(from != null);
			Debug.Assert(to != null);

			return Expression.Call(MapCollection.MakeGenericMethod(from, to), context.ValueExpression, context.ContextExpression);
		}
	}
}
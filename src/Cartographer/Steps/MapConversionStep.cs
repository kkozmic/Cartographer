namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;

	public class MapConversionStep: ConversionStep
	{
		readonly Expression<Func<object, IMapper, object>> blueprint = (value, mapper) => mapper.Convert<object>(value);

		public override Expression BuildConversionExpression(MappingStrategy context, MappingStep step)
		{
			var callExpression = (MethodCallExpression)blueprint.Body;
			if (step.TargetValueType == typeof (object))
			{
				return callExpression;
			}
			var method = callExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(step.TargetValueType);
			return Expression.Call(context.MapperExpression, method, context.ValueExpression);
		}
	}
}
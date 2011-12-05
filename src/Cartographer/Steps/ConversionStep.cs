namespace Cartographer.Steps
{
	using System.Linq.Expressions;

	public abstract class ConversionStep
	{
		public abstract Expression BuildConversionExpression(MappingContext context, MappingStep step);
	}
}

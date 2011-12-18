namespace Cartographer.Steps
{
	using System.Linq.Expressions;

	public abstract class ConversionStep
	{
		public abstract Expression BuildConversionExpression(MappingStrategy context, MappingStep step);
	}
}
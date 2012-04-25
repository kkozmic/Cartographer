namespace Cartographer.Steps
{
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public abstract class ConversionStep
	{
		public abstract Expression BuildConversionExpression(MappingStrategy strategy);
	}
}
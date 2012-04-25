namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public class DirectMappingStep: MappingStep
	{
		readonly Type sourceValueType;

		readonly Type targetValueType;

		public DirectMappingStep(Type sourceValueType, Type targetValueType)
		{
			this.sourceValueType = sourceValueType;
			this.targetValueType = targetValueType;
		}

		public override Type SourceValueType
		{
			get { return sourceValueType; }
		}

		public override Type TargetValueType
		{
			get { return targetValueType; }
		}

		public override Expression Apply(MappingStrategy strategy, ConversionStep conversion)
		{
			strategy.ValueExpression = strategy.SourceExpression;
			return conversion.BuildConversionExpression(strategy);
		}
	}
}
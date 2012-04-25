namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public class SimpleStep: MappingStep
	{
		readonly Func<MappingStrategy, ConversionStep, Expression> apply;

		readonly Type sourceValueType;

		readonly Type targetValueType;

		public SimpleStep(Type sourceValueType, Type targetValueType, Func<MappingStrategy, ConversionStep, Expression> apply)
		{
			this.sourceValueType = sourceValueType;
			this.targetValueType = targetValueType;
			this.apply = apply;
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
			return apply(strategy, conversion);
		}
	}
}
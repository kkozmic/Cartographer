namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using Cartographer.Compiler;

	public abstract class MappingStep
	{
		public abstract Type SourceValueType { get; }

		public abstract Type TargetValueType { get; }

		public ConversionStep Conversion { get; set; }

		public abstract Expression Apply(MappingStrategy strategy, ConversionStep conversion);
	}
}
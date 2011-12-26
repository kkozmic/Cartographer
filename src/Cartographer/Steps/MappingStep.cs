namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Cartographer.Compiler;

	public abstract class MappingStep
	{
		public abstract PropertyInfo SourceProperty { get; }

		public abstract Type SourceValueType { get; }

		public abstract PropertyInfo TargetProperty { get; }

		public abstract Type TargetValueType { get; }

		public ConversionStep Conversion { get; set; }

		public abstract Expression BuildGetSourceValueExpression(MappingStrategy context);

		public abstract Expression BuildSetTargetValueExpression(MappingStrategy context);
	}
}
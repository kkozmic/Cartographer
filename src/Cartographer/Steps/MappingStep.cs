namespace Cartographer.Steps
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;

	public abstract class MappingStep
	{
		public abstract PropertyInfo[] SourcePropertiesUsed { get; }

		public abstract Type SourceValueType { get; }
		public abstract PropertyInfo[] TargetPropertiesUsed { get; }
		public abstract Type TargetValueType { get; }

		public ConversionStep Conversion { get; set; }

		public abstract Expression BuildGetSourceValueExpression(MappingContext context);

		public abstract Expression BuildSetTargetValueExpression(MappingContext context);
	}
}

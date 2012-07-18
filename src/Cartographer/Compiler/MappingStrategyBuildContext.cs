namespace Cartographer.Compiler
{
	using System;
	using Cartographer.Steps;

	public class MappingStrategyBuildContext
	{
		readonly Func<MappingStep, bool, DelegatingConversionStep> applyConverter;

		public MappingStrategyBuildContext(Func<MappingStep, bool, DelegatingConversionStep> applyConverter)
		{
			this.applyConverter = applyConverter;
		}

		public bool Completed { get; private set; }

		public void MarkAsCompleted()
		{
			Completed = true;
		}

		public DelegatingConversionStep ApplyConverter(MappingStep mapping, bool withFallback)
		{
			return applyConverter(mapping, withFallback);
		}
	}
}
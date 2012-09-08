namespace CartographerTests.ContainerIntegration
{
	using System;
	using Cartographer.Compiler;
	using Castle.Core;
	using Castle.MicroKernel.Handlers;

	public class ConversionPatterStrategy: IGenericServiceStrategy
	{
		readonly IConversionPatternGenericCloser closer;

		public ConversionPatterStrategy(IConversionPatternGenericCloser closer)
		{
			this.closer = closer;
		}

		public bool Supports(Type service, ComponentModel component)
		{
			var arguments = service.GetGenericArguments();
			var closed = closer.Close(component.Implementation, arguments[0], arguments[1]);
			return closed != null;
		}
	}
}
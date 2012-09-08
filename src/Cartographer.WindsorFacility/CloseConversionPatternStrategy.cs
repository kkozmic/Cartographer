namespace CartographerFacility
{
	using System;
	using Cartographer.Compiler;
	using Castle.Core;
	using Castle.MicroKernel.Context;
	using Castle.MicroKernel.Handlers;

	public class CloseConversionPatternStrategy: IGenericImplementationMatchingStrategy
	{
		readonly IConversionPatternGenericCloser closer;

		public CloseConversionPatternStrategy(IConversionPatternGenericCloser closer)
		{
			this.closer = closer;
		}

		public Type[] GetGenericArguments(ComponentModel model, CreationContext context)
		{
			var closed = closer.Close(model.Implementation, context.GenericArguments[0], context.GenericArguments[1]);

			return closed.GetGenericArguments();
		}
	}
}
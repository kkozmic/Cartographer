namespace CartographerFacility
{
	using System;
	using Cartographer.Compiler;
	using Castle.MicroKernel;

	public class WindsorConversionPatternRepository: IConversionPatternRepository
	{
		readonly IKernel kernel;

		public WindsorConversionPatternRepository(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public object LeaseConversionPatternFor(Type sourceValueType, Type targetValueType)
		{
			var actualType = typeof (IConversionPattern<,>).MakeGenericType(sourceValueType, targetValueType);
			var handler = kernel.GetHandler(actualType);
			if (handler != null)
			{
				return kernel.Resolve(actualType);
			}
			return null;
		}

		public void Recycle(object conversionPattern)
		{
			kernel.ReleaseComponent(conversionPattern);
		}
	}
}
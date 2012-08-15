namespace CartographerTests.ContainerIntegration
{
	using System.Linq;
	using Cartographer.Compiler;
	using Castle.Core;
	using Castle.Core.Internal;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.ModelBuilder;

	public class CloseConversionPatternContributor: IContributeComponentModelConstruction
	{
		readonly IGenericServiceStrategy genericServiceStrategy;

		readonly IGenericImplementationMatchingStrategy implemenetationMatchingStrategy;

		public CloseConversionPatternContributor(IGenericImplementationMatchingStrategy implemenetationMatchingStrategy, IGenericServiceStrategy genericServiceStrategy)
		{
			this.implemenetationMatchingStrategy = implemenetationMatchingStrategy;
			this.genericServiceStrategy = genericServiceStrategy;
		}

		public void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (SupportsOpenConversionPattern(model) == false)
			{
				return;
			}
			if (model.ExtendedProperties.Contains(Constants.GenericImplementationMatchingStrategy) == false)
			{
				model.ExtendedProperties[Constants.GenericImplementationMatchingStrategy] = implemenetationMatchingStrategy;
			}
			if (model.ExtendedProperties.Contains(Constants.GenericServiceStrategy) == false)
			{
				model.ExtendedProperties[Constants.GenericServiceStrategy] = genericServiceStrategy;
			}
		}

		static bool SupportsOpenConversionPattern(ComponentModel model)
		{
			return model.Services.Contains(typeof (IConversionPattern<,>));
		}
	}
}
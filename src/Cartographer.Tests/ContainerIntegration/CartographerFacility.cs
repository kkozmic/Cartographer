namespace CartographerTests.ContainerIntegration
{
	using System;
	using System.Linq;
	using Cartographer;
	using Cartographer.Compiler;
	using Castle.Core;
	using Castle.Core.Internal;
	using Castle.Facilities.TypedFactory;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Context;
	using Castle.MicroKernel.Facilities;
	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.ModelBuilder;
	using Castle.MicroKernel.Registration;

	public class CartographerFacility: AbstractFacility
	{
		public CartographerFacility()
		{
			ConversionPatternGenericCloser = new ConversionPatternGenericCloser();
		}

		public IConversionPatternGenericCloser ConversionPatternGenericCloser { get; set; }

		protected override void Init()
		{
			if (Kernel.GetFacilities().Any(f => f is TypedFactoryFacility) == false)
			{
				throw new InvalidOperationException(string.Format("This facility depends on {0}. Please add it first before adding {1}", typeof (TypedFactoryFacility), GetType()));
			}

			Kernel.ComponentModelBuilder.AddContributor(new CloseConversionPatternContributor(
				                                            new CloseConversionPatternStrategy(ConversionPatternGenericCloser),
				                                            new ConversionPatterStrategy(ConversionPatternGenericCloser)));
			Kernel.Register(
				Component.For<IConversionPatternRepository>().ImplementedBy<WindsorConversionPatternRepository>().LifestyleTransient(),
				Component.For<MapperBuilder.MapperBuilderSettings>(),
				Component.For<MapperBuilder>(),
				Component.For<IMapper>().UsingFactory((MapperBuilder b) => b.BuildMapper()));
		}
	}

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

	public class WindsorConversionPatternRepository: IConversionPatternRepository
	{
		readonly IKernel kernel;

		public WindsorConversionPatternRepository(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public dynamic LeaseConversionPatternFor(Type sourceValueType, Type targetValueType)
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
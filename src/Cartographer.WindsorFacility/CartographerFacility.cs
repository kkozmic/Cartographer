namespace CartographerFacility
{
	using System;
	using System.Linq;
	using Cartographer;
	using Cartographer.Compiler;
	using Castle.Facilities.TypedFactory;
	using Castle.MicroKernel.Facilities;
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
}
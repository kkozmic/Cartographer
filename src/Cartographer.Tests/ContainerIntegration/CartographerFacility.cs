namespace CartographerTests.ContainerIntegration
{
	using Cartographer;
	using Castle.MicroKernel.Facilities;
	using Castle.MicroKernel.Registration;

	public class CartographerFacility: AbstractFacility
	{
		protected override void Init()
		{
			Kernel.Register(
				Component.For<MapperBuilder.MapperBuilderSettings>(),
				Component.For<MapperBuilder>(),
				Component.For<IMapper>().UsingFactory((MapperBuilder b) => b.BuildMapper()));
		}
	}
}
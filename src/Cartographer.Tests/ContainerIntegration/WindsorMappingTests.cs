namespace CartographerTests.ContainerIntegration
{
	using System;
	using Cartographer;
	using Cartographer.Compiler;
	using CartographerTests.ConversionPatterns;
	using CartographerTests.Types;
	using Castle.Facilities.TypedFactory;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using Xunit;

	public class WindsorMappingTests: IDisposable
	{
		readonly IWindsorContainer container;

		public WindsorMappingTests()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.AddFacility<CartographerFacility>();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
		}

		[Fact]
		public void Can_use_conversion_pattern_registered_in_the_container()
		{
			container.Register(Component.For(typeof (IConversionPattern<,>)).ImplementedBy(typeof (ToStringConversionPattern<>)));
			var mapper = container.Resolve<IMapper>();
			var dto = mapper.Convert<Account6Dto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal("42", dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		public void Dispose()
		{
			container.Dispose();
		}
	}
}
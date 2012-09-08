namespace CartographerTests.ContainerIntegration
{
	using System;
	using Cartographer;
	using Cartographer.Compiler;
	using CartographerFacility;
	using CartographerTests.Types;
	using Castle.Facilities.TypedFactory;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using NSubstitute;
	using Xunit;

	public class WindsorIntegrationSettingsTests: IDisposable
	{
		readonly IWindsorContainer container;

		public WindsorIntegrationSettingsTests()
		{
			container = new WindsorContainer()
				.AddFacility<TypedFactoryFacility>()
				.AddFacility<CartographerFacility>();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
		}

		[Fact(Skip = "This won't work, since in Windsor the closer is not used as component but as extension to handler.")]
		public void Can_override_ConversionPatternGenericCloser_from_the_container()
		{
			var instance = Substitute.For<IConversionPatternGenericCloser>();
			container.Register(Component.For<IConversionPatternGenericCloser>().Instance(instance));

			var mapper = container.Resolve<IMapper>();
			mapper.Convert<UserDto>(new User());

			instance.ReceivedWithAnyArgs().Close(null, null, null);
		}

		[Fact]
		public void Can_override_descriptor_from_the_container()
		{
			var descriptor = Substitute.For<IMappingDescriptor>();
			container.Register(Component.For<IMappingDescriptor>().Instance(descriptor));

			var mapper = container.Resolve<IMapper>();
			mapper.Convert<UserDto>(new User());

			descriptor.ReceivedWithAnyArgs().DescribeStep(null);
		}

		[Fact]
		public void Can_override_mapping_info_source_from_the_container()
		{
			var source = Substitute.For<ITypeMatcher>();
			container.Register(Component.For<ITypeMatcher>().Instance(source));

			var mapper = container.Resolve<IMapper>();
			mapper.Convert<UserDto>(new User());

			source.ReceivedWithAnyArgs().Match(null);
		}

		[Fact]
		public void Can_resolve_mapper_from_the_container()
		{
			container.Resolve<IMapper>();
		}

		public void Dispose()
		{
			container.Dispose();
		}
	}
}
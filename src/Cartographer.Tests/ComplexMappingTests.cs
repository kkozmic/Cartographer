namespace CartographerTests
{
	using System;
	using Cartographer;
	using CartographerTests.ConversionPatterns;
	using CartographerTests.MappingPatterns;
	using CartographerTests.Types;
	using Xunit;

	public class ComplexMappingTests
	{
		readonly MapperBuilder builder;

		public ComplexMappingTests()
		{
			builder = new MapperBuilder();
		}

		MapperBuilder.MapperBuilderSettings BuilderSettings
		{
			get { return builder.Settings; }
		}

		[Fact]
		public void Can_map_complex_property_to_new_object()
		{
			BuilderSettings.AddConversionPatternType(typeof (IdentifierConversionPattern));
			BuilderSettings.AddMappingPattern(new TargetSuffixMappingPattern("Identifier"));

			var mapper = BuildMapper();
			var lastModified = DateTime.Now;
			var dto = mapper.Convert<Order2Dto>(new Order2
			                                    {
			                                    	Customer = new Customer { ItemId = 42, LastModified = lastModified }
			                                    });
			Assert.NotNull(dto.CustomerIdentifier);
			Assert.Equal(42, dto.CustomerIdentifier.Id);
			Assert.Equal(lastModified, dto.CustomerIdentifier.Timestamp);
		}

		IMapper BuildMapper()
		{
			return builder.BuildMapper();
		}
	}
}
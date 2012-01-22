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

		[Fact]
		public void Can_use_call_site_arguments_in_mapping()
		{
			BuilderSettings.AddConversionPatternType(typeof (ToLocalDateInTimeZonePattern));
			var mapper = BuildMapper();

			var dateTime = DateTime.UtcNow.Date.Add(TimeSpan.FromHours(15));

			var utcOffset = TimeSpan.FromHours(10);
			var timeZone = TimeZoneInfo.CreateCustomTimeZone("My time zone", utcOffset, "x", "x", "x", null, disableDaylightSavingTime: true);
			var dto = mapper.ConvertWithArguments<Order3Dto>(new Order3
			                                                 {
			                                                 	OrderTime = dateTime
			                                                 },
			                                                 new { TimeZone = timeZone });

			var expectedDate = dateTime.Add(utcOffset).Date;
			Assert.Equal(expectedDate, dto.OrderTime);
		}

		IMapper BuildMapper()
		{
			return builder.BuildMapper();
		}
	}
}
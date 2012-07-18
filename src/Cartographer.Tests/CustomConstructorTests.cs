namespace CartographerTests
{
	using Cartographer;
	using CartographerTests.ConversionPatterns;
	using CartographerTests.Types;
	using Xunit;

	public class CustomConstructorTests
	{
		[Fact]
		public void Can_map_using_custom_constructor()
		{
			var mapper = new MapperBuilder().BuildMapper();
			var dto = mapper.Convert<AccountWithCtorDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Can_map_when_harcoded_mapping()
		{
			var builder = new MapperBuilder();
			builder.Settings.AddConversionPatternType(typeof (AccountWith2ConstructorsMappingPattern));
			var mapper = builder.BuildMapper();
			var dto = mapper.Convert<AccountWith2CtorsDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}
	}
}
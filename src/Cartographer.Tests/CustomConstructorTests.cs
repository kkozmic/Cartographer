namespace CartographerTests
{
	using Cartographer;
	using Cartographer.Compiler;
	using CartographerTests.ConversionPatterns;
	using CartographerTests.MappingPatterns;
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

		[Fact]
		public void Can_map_when_target_exists()
		{
			var mapper = new MapperBuilder().BuildMapper();
			var dto = mapper.Convert(new Account { Number = "abc123", Owner = new Person { Id = 42 } }, new AccountWith3CtorsDto());
			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact(Skip = "Not implemented yet. We need strongly typed context support for that")]
		public void Can_map_with_callsite_parameter()
		{
			var mapper = new MapperBuilder().BuildMapper();
			var dto = mapper.ConvertWithArguments<AccountWithAdditionalParameterDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } }, new { language = "en-AU" });
			Assert.Equal("en-AU", dto.Language);
			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Can_map_with_callsite_parameter_via_custom_mapping_step()
		{
			var builder = new MapperBuilder();
			builder.Settings.MappingPatterns = new IMappingPattern[] { new MapMissingCtorParametersFromCallSite(), };
			var mapper = builder.BuildMapper();
			var dto = mapper.ConvertWithArguments<AccountWithAdditionalParameterDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } }, new { language = "en-AU" });
			Assert.Equal("en-AU", dto.Language);
			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}
	}
}
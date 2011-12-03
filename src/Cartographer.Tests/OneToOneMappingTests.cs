namespace CartographerTests
{
	using Cartographer;

	using CartographerTests.Types;

	using Xunit;

	public class OneToOneMappingTests
	{
		readonly Mapper mapper;

		public OneToOneMappingTests()
		{
			mapper = new Mapper(new TypeMapper(), new TypeModelBuilder(), new MappingBuilder());
		}

		[Fact]
		public void Can_map_one_to_one_type_with_string_properties()
		{
			var dto = mapper.Convert<UserDto>(new User { FirstName = "Stefan", LastName = "Mucha" });

			Assert.Equal("Stefan", dto.FirstName);
			Assert.Equal("Mucha", dto.LastName);
		}
	}
}
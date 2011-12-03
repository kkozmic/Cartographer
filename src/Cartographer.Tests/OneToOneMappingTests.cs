namespace CartographerTests
{
	using Cartographer;
	using Cartographer.Visitors;

	using CartographerTests.Types;

	using Xunit;

	public class OneToOneMappingTests
	{
		readonly Mapper mapper;

		public OneToOneMappingTests()
		{
			mapper = new Mapper(
				new TypeMapper(),
				new TypeModelBuilder(),
				new MappingBuilder(new MatchByNameMappingPattern(), new MatchByNameFlattenMappingPattern()),
				new MappingCompiler(new AssignVisitor(), new AssignChainVisitor()));
		}

		[Fact]
		public void Can_flatten_Foo_Bar_to_FooBar()
		{
			var dto = mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
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

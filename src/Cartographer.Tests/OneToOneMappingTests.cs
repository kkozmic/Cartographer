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
				new MappingBuilder(new IConversionPattern[] { new MapConversionPattern() }, new MatchByNameMappingPattern(), new MatchByNameFlattenMappingPattern()),
				new MappingCompiler(new AssignVisitor(), new AssignChainVisitor()));
		}

		[Fact]
		public void Can_flatten_Foo_Bar_to_FooBar()
		{
			var dto = mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact(Skip = "This is a more complex problem than appears at first sight. A rule for handling this needs to be carefully thought through")]
		public void Can_handle_nulls_on_flattening_path()
		{
			var dto = mapper.Convert<Account2Dto>(new Account { Number = "abc123", Owner = null });
			Assert.Equal(null, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Can_map_one_to_one_type_with_string_properties()
		{
			var dto = mapper.Convert<UserDto>(new User { FirstName = "Stefan", LastName = "Mucha" });

			Assert.Equal("Stefan", dto.FirstName);
			Assert.Equal("Mucha", dto.LastName);
		}

		[Fact]
		public void Can_map_recoursively()
		{
			var dto = mapper.Convert<User2Dto>(new User2
			                                   {
			                                   	FirstName = "Stefan",
			                                   	LastName = "Mucha",
			                                   	Address = new Address
			                                   	          {
			                                   	          	AddressLine1 = "42 Some Street",
			                                   	          	AddressLine2 = "Apartment 42",
			                                   	          	City = "El Dorado",
			                                   	          	ZipCode = "42-42"
			                                   	          }
			                                   });

			Assert.Equal("Stefan", dto.FirstName);
			Assert.Equal("42 Some Street", dto.Address.AddressLine1);
		}
	}
}

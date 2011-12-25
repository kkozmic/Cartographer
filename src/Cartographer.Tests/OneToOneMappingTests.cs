namespace CartographerTests
{
	using System;
	using Cartographer;
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
				new MappingBuilder(new MappingDescriptor(Console.Out), new IConversionPattern[] { new MapConversionPattern(), new CollectionConversionPattern(), },
				                   new MatchByNameMappingPattern(), new MatchByNameFlattenMappingPattern()),
				new MappingCompiler());
		}

		[Fact]
		public void Can_flatten_Foo_Bar_to_FooBar()
		{
			var dto = mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Can_handle_nulls_on_flattening_path()
		{
			var dto = mapper.Convert<Account2Dto>(new Account { Number = "abc123", Owner = null });
			Assert.Equal(null, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Can_map_collections()
		{
			var dto = mapper.Convert<OrderDto>(new Order
			                                   {
			                                   	OrderLines = new[]
			                                   	             {
			                                   	             	new OrderLine
			                                   	             	{
			                                   	             		ItemId = 1,
			                                   	             		ItemName = "The Ring"
			                                   	             	},
			                                   	             	new OrderLine
			                                   	             	{
			                                   	             		ItemId = 2,
			                                   	             		ItemName = "A dagger"
			                                   	             	}
			                                   	             }
			                                   });

			Assert.Equal(2, dto.OrderLines.Length);
			Assert.Equal("A dagger", dto.OrderLines[1].ItemName);
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
namespace CartographerTests
{
	using CartographerTests.Types;
	using Xunit;

	public class MappingToExistingObjectTests: AbstractMappingTests
	{
		[Fact]
		public void Can_map_to_existing_object()
		{
			var dto = new AccountDto();
			mapper.Convert(new Account { Number = "abc123", Owner = new Person { Id = 42 } }, dto);

			Assert.Equal("abc123", dto.Number);
		}

		[Fact]
		public void Mapping_to_new_and_existing_objects_can_coexist()
		{
			var dto = new AccountDto();
			mapper.Convert(new Account { Number = "abc123", Owner = new Person { Id = 42 } }, dto);
			Assert.Equal("abc123", dto.Number);

			dto = mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
			Assert.Equal("abc123", dto.Number);
		}
	}
}
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
	}
}